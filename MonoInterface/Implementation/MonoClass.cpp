#include "stdafx.h"
#include "API_ImplementationHeaders.h"
#include "List.h"
#include "MonoProperty.h"
#include "MonoEvent.h"
#include "MonoField.h"

MonoClassWrapper::MonoClassWrapper(MonoClass *klass)
	: fullName(nullptr)
	, fullNameIL(nullptr)
{
	this->wrappedClass = klass;

	this->name      = mono_class_get_name(klass);
	this->nameSpace = mono_class_get_namespace(klass);

	this->methods    = List<IMonoMethod *>  (30);
	this->properties = List<IMonoProperty *>(30);
	this->events     = List<IMonoEvent *>   (30);
	this->fields     = List<IMonoField *>   (30);

	MonoClass *base = klass;
	while (base)
	{
		// Cache methods.
		void *iter = 0;
		while (MonoMethod     *met  = mono_class_get_methods(base, &iter))
		{
			this->methods.Add(new MonoMethodWrapper(met));
		}
		// Cache properties.
		iter = 0;
		while (MonoProperty   *prop = mono_class_get_properties(base, &iter))
		{
			this->properties.Add(new MonoPropertyWrapper(prop));
		}
		// Cache events.
		iter = 0;
		while (MonoEvent      *ev   = mono_class_get_events(base, &iter))
		{
			this->events.Add(new MonoEventWrapper(ev));
		}
		// Cache fields.
		iter = 0;
		while (MonoClassField *f = mono_class_get_fields(base, &iter))
		{
			this->fields.Add(new MonoField(f, this));
		}

		base = mono_class_get_parent(base);
	}

	this->methods.Trim();
	this->properties.Trim();
	this->events.Trim();
	this->fields.Trim();

	this->vtable = mono_class_vtable(mono_domain_get(), klass);
}
MonoClassWrapper::~MonoClassWrapper()
{
	if (this->fullName)
	{
		delete this->fullName;
	}
	if (this->fullNameIL)
	{
		delete this->fullNameIL;
	}

	for (int i = 0; i < this->properties.Length; i++)
	{
		delete this->properties[i];
	}
	this->properties.Dispose();

	for (int i = 0; i < this->events.Length; i++)
	{
		delete this->events[i];
	}
	this->events.Dispose();

	for (int i = 0; i < this->methods.Length; i++)
	{
		delete this->methods[i];
	}
	this->methods.Dispose();

	for (int i = 0; i < this->fields.Length; i++)
	{
		delete this->fields[i];
	}
	this->fields.Dispose();
}

IMonoConstructor *MonoClassWrapper::GetConstructor(IMonoArray *types /*= nullptr*/)
{
	return static_cast<IMonoConstructor *>(this->GetMethod(".ctor", types));
}

IMonoConstructor *MonoClassWrapper::GetConstructor(List<IMonoClass *> &classes)
{
	return static_cast<IMonoConstructor *>(this->GetMethod(".ctor", classes));
}

IMonoConstructor *MonoClassWrapper::GetConstructor(List<Pair<IMonoClass *, const char *>> &specifiedClasses)
{
	return static_cast<IMonoConstructor *>(this->GetMethod(".ctor", specifiedClasses));
}

IMonoConstructor *MonoClassWrapper::GetConstructor(const char *params)
{
	return static_cast<IMonoConstructor *>(this->GetMethod(".ctor", params));
}

IMonoConstructor *MonoClassWrapper::GetConstructor(List<const char *> &paramTypeNames)
{
	return static_cast<IMonoConstructor *>(this->GetMethod(".ctor", paramTypeNames));
}

IMonoConstructor *MonoClassWrapper::GetConstructor(const char *name, int paramCount)
{
	return static_cast<IMonoConstructor *>(this->GetMethod(".ctor", paramCount));
}
//! Gets the first that matches given description.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, int paramCount)
{
	for (int i = 0; i < this->methods.Length; i++)
	{
		if (this->methods[i]->ParameterCount == paramCount &&
			strcmp(this->methods[i]->Name, name) == 0)
		{
			return this->methods[i];
		}
	}
	return nullptr;
}
//! Gets the method that matches given description.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, const char *params)
{
	if (params == nullptr)
	{
		return this->GetMethod(name, (int)0);
	}
	for (int i = 0; i < this->methods.Length; i++)
	{
		if (strcmp(this->methods[i]->Name, name) == 0 &&
			strcmp(this->methods[i]->Parameters, params) == 0)
		{
			return this->methods[i];
		}
	}
	return nullptr;
}

IMonoMethod *MonoClassWrapper::GetMethod(const char *name, IMonoArray *types /*= nullptr*/)
{
	for (int i = 0; i < this->methods.Length; i++)
	{
		if (this->methods[i]->ParameterCount != types->Length ||
			strcmp(this->methods[i]->Name, name) != 0)
		{
			continue;
		}

		auto typeNames = this->methods[i]->ParameterTypeNames;

		bool match = true;
		for (int j = 0; j < typeNames->Length; j++)
		{
			// Look at definition of _MonoReflectionType in mono sources to see what is going on here.
			MonoType *type = (MonoType *)((unsigned char *)types->Item(j) + sizeof(MonoObject));
			if (strcmp(typeNames->At(j), mono_type_get_name(type)) != 0)
			{
				match = false;
				break;
			}
		}
		if (match)
		{
			return this->methods[i];
		}
	}
	return nullptr;
}

IMonoMethod *MonoClassWrapper::GetMethod(const char *name, List<IMonoClass *> &classes)
{
	for (int i = 0; i < this->methods.Length; i++)
	{
		if (this->methods[i]->ParameterCount != classes.Length ||
			strcmp(this->methods[i]->Name, name) != 0)
		{
			continue;
		}
		auto currentClasses = this->methods[i]->ParameterClasses;
		bool match = true;
		for (int j = 0; j < classes.Length; j++)
		{
			if (currentClasses->At(j) != classes[j])
			{
				match = false;
				break;
			}
		}
		if (match)
		{
			return this->methods[i];
		}
	}
	return nullptr;
}

IMonoMethod *MonoClassWrapper::GetMethod(const char *name, List<Pair<IMonoClass *, const char *>> &specifiedClasses)
{
	auto paramTypeNames = List<const char *>(specifiedClasses.Length);

	for (int i = 0; i < specifiedClasses.Length; i++)
	{
		ConstructiveText typeName = ConstructiveText(10);
		typeName << specifiedClasses[i].Value1->FullNameIL << specifiedClasses[i].Value2;

		const char *typeNameNt = typeName.ToNTString();
		paramTypeNames.Add(typeNameNt);
	}

	IMonoMethod *foundMethod = this->GetMethod(name, paramTypeNames);

	for (int i = 0; i < paramTypeNames.Length; i++)
	{
		delete paramTypeNames[i];
	}

	return foundMethod;
}

IMonoMethod *MonoClassWrapper::GetMethod(const char *name, List<const char *> &paramTypeNames)
{
	for (int i = 0; i < this->methods.Length; i++)
	{
		if (this->methods[i]->ParameterCount != paramTypeNames.Length ||
			strcmp(this->methods[i]->Name, name) != 0)
		{
			continue;
		}
		auto typeNames = this->methods[i]->ParameterTypeNames;
		bool match = true;
		for (int j = 0; j < typeNames->Length; j++)
		{
			if (strcmp(typeNames->At(j), paramTypeNames[j]) != 0)
			{
				match = false;
				break;
			}
		}
		if (match)
		{
			return this->methods[i];
		}
	}
	return nullptr;
}

//! Gets an array of methods that matches given description.
IMonoMethod **MonoClassWrapper::GetMethods(const char *name, int paramCount, int &foundCount)
{
	List<IMonoMethod *> foundMethods = List<IMonoMethod *>(this->methods.Length);

	for (int i = 0; i < this->methods.Length; i++)
	{
		if (this->methods[i]->ParameterCount == paramCount &&
			strcmp(this->methods[i]->Name, name) == 0)
		{
			foundMethods.Add(this->methods[i]);
		}
	}

	foundMethods.Trim();

	return foundMethods.Detach(foundCount);
}
//! Gets an array of overload of the method.
IMonoMethod **MonoClassWrapper::GetMethods(const char *name, int &foundCount)
{
	List<IMonoMethod *> foundMethods = List<IMonoMethod *>(this->methods.Length);

	for (int i = 0; i < this->methods.Length; i++)
	{
		if (strcmp(this->methods[i]->Name, name) == 0)
		{
			foundMethods.Add(this->methods[i]);
		}
	}

	foundMethods.Trim();

	return foundMethods.Detach(foundCount);
}

//! Gets the value of the object's field.
void MonoClassWrapper::GetField(mono::object obj, const char *name, void *value)
{
	this->GetFieldValue(obj, mono_class_get_field_from_name(this->wrappedClass, name), value);
}
//! Sets the value of the object's field.
void MonoClassWrapper::SetField(mono::object obj, const char *name, void *value)
{
	this->SetFieldValue(obj, mono_class_get_field_from_name(this->wrappedClass, name), value);
}
//! Gets the value of the object's field.
void MonoClassWrapper::GetField(mono::object obj, IMonoField *field, void *value)
{
	this->GetFieldValue(obj, field->GetHandle<MonoClassField>(), value);
}
//! Sets the value of the object's field.
void MonoClassWrapper::SetField(mono::object obj, IMonoField *field, void *value)
{
	this->SetFieldValue(obj, field->GetHandle<MonoClassField>(), value);
}

void MonoClassWrapper::GetFieldValue(mono::object obj, MonoClassField *field, void *value)
{
	if (obj)
	{
		mono_field_get_value((MonoObject *)obj, field, value);
	}
	else
	{
		mono_field_static_get_value(this->vtable, field, value);
	}
}

void MonoClassWrapper::SetFieldValue(mono::object obj, MonoClassField *field, void *value)
{
	if (obj)
	{
		mono_field_set_value((MonoObject *)obj, field, value);
	}
	else
	{
		mono_field_static_set_value(this->vtable, field, value);
	}
}


IMonoProperty *MonoClassWrapper::GetProperty(const char *name)
{
	for (int i = 0; i < this->properties.Length; i++)
	{
		if (strcmp(this->properties[i]->Name, name) == 0)
		{
			return this->properties[i];
		}
	}
	return nullptr;
}

IMonoEvent *MonoClassWrapper::GetEvent(const char *name)
{
	for (int i = 0; i < this->events.Length; i++)
	{
		if (strcmp(this->events[i]->Name, name) == 0)
		{
			return this->events[i];
		}
	}
	return nullptr;
}
//! Determines whether this class implements from specified class.
bool MonoClassWrapper::Inherits(const char *nameSpace, const char *className)
{
	MonoClass *base = mono_class_get_parent(this->wrappedClass);

	while (base)
	{
		if (strcmp(mono_class_get_name(base), className) == 0 &&
			strcmp(mono_class_get_namespace(base), nameSpace) == 0)
		{
			return true;
		}
		base = mono_class_get_parent(base);
	}

	return false;
}
//! Boxes given value.
mono::object MonoClassWrapper::Box(void *value)
{
	MonoClass *klass = this->wrappedClass;
	if (mono_class_is_valuetype(klass))
	{
		return (mono::object)mono_value_box((MonoDomain *)MonoEnv->AppDomain, klass, value);
	}
	return nullptr;
}

const char *MonoClassWrapper::GetName()
{
	return this->name;
}

const char *MonoClassWrapper::GetNameSpace()
{
	return this->nameSpace;
}

const char *MonoClassWrapper::GetFullName()
{
	if (!this->fullName)
	{
		ConstructiveText fullName;
		if (MonoClass *nestingClass = mono_class_get_nesting_type(this->wrappedClass))
		{
			const char *nestingName = MonoClassCache::Wrap(nestingClass)->FullName;

			fullName = ConstructiveText(strlen(this->name) + strlen(nestingName) + 1);

			fullName << nestingName << "." << this->name;

			delete nestingName;
		}
		else
		{
			ConstructiveText fullName =
				ConstructiveText(strlen(this->name) + strlen(this->nameSpace) + 1);

			fullName << this->nameSpace << "." << this->name;
		}
		this->fullName = fullName.ToNTString();
	}
	return this->fullName;
}

const char *MonoClassWrapper::GetFullNameIL()
{
	if (!this->fullNameIL)
	{
		ConstructiveText fullName;
		if (MonoClass *nestingClass = mono_class_get_nesting_type(this->wrappedClass))
		{
			const char *nestingName = MonoClassCache::Wrap(nestingClass)->FullName;

			fullName = ConstructiveText(strlen(this->name) + strlen(nestingName) + 1);

			fullName << nestingName << "+" << this->name;

			delete nestingName;
		}
		else
		{
			ConstructiveText fullName =
				ConstructiveText(strlen(this->name) + strlen(this->nameSpace) + 1);

			fullName << this->nameSpace << "." << this->name;
		}
		this->fullNameIL = fullName.ToNTString();
	}
	return this->fullNameIL;
}

IMonoAssembly *MonoClassWrapper::GetAssembly()
{
	return MonoEnv->Assemblies->Wrap(mono_image_get_assembly(mono_class_get_image(this->wrappedClass)));
}

void *MonoClassWrapper::GetWrappedPointer()
{
	return this->wrappedClass;
}

bool MonoClassWrapper::Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses)
{
	void *iterator = 0;
	while (MonoClass *currentInterface = mono_class_get_interfaces(this->wrappedClass, &iterator))
	{
		if (!strcmp(mono_class_get_name(currentInterface), interfaceName) &&
			!strcmp(mono_class_get_namespace(currentInterface), nameSpace))
		{
			return true;
		}
	}
	if (searchBaseClasses)
	{
		MonoClass *base = mono_class_get_parent(this->wrappedClass);
		if (base != mono_get_object_class())
		{
			return MonoClassCache::Wrap(base)->Implements(nameSpace, interfaceName);
		}
	}
	return false;
}

IMonoClass *MonoClassWrapper::GetBase()
{
	return MonoClassCache::Wrap(mono_class_get_parent(this->wrappedClass));
}

IMonoClass *MonoClassWrapper::GetNestedType(const char *name)
{
	void *iter;
	MonoClass *nestedType;
	while (nestedType = mono_class_get_nested_types(this->wrappedClass, &iter))
	{
		if (strcmp(mono_class_get_name(nestedType), name) == 0)
		{
			return MonoClassCache::Wrap(nestedType);
		}
	}
	return nullptr;
}

mono::type MonoClassWrapper::GetType()
{
	return (mono::type)mono_type_get_object(mono_domain_get(), mono_class_get_type(this->wrappedClass));
}

mono::type MonoClassWrapper::MakeArrayType()
{
	return MonoEnv->CoreLibrary->GetClass("System", "Type")
							   ->GetMethod("MakeArrayType", 0)
							   ->Invoke(this->GetType());
}

mono::type MonoClassWrapper::MakeArrayType(int rank)
{
	void *par = &rank;
	return MonoEnv->CoreLibrary->GetClass("System", "Type")
							   ->GetMethod("MakeArrayType", 1)
							   ->Invoke(this->GetType(), &par);
}

mono::type MonoClassWrapper::MakeByRefType()
{
	return MonoEnv->CoreLibrary->GetClass("System", "Type")
							   ->GetMethod("MakeByRefType", 0)
							   ->Invoke(this->GetType());
}

mono::type MonoClassWrapper::MakePointerType()
{
	return MonoEnv->CoreLibrary->GetClass("System", "Type")
							   ->GetMethod("MakePointerType", 0)
							   ->Invoke(this->GetType());
}

ReadOnlyList<IMonoField *> *MonoClassWrapper::GetFields()
{
	return (ReadOnlyList<IMonoField *> *)&this->fields;
}

ReadOnlyList<IMonoMethod *> *MonoClassWrapper::GetMethods()
{
	return (ReadOnlyList<IMonoMethod *> *)&this->methods;
}

ReadOnlyList<IMonoProperty *> *MonoClassWrapper::GetProperties()
{
	return (ReadOnlyList<IMonoProperty *> *)&this->properties;
}

ReadOnlyList<IMonoEvent *> *MonoClassWrapper::GetEvents()
{
	return (ReadOnlyList<IMonoEvent *> *)&this->events;
}

List<MonoClassWrapper *> MonoClassCache::cachedClasses(50);


IMonoClass *MonoClassCache::Wrap(MonoClass *klass)
{
	for (int i = 0; i < cachedClasses.Length; i++)
	{
		IMonoClass *wrapper = cachedClasses[i];
		if (wrapper->GetWrappedPointer() == klass)
		{
			return wrapper;
		}
	}
	// Register a new one.
	MonoClassWrapper *wrapper = new MonoClassWrapper(klass);
	MonoClassCache::cachedClasses.Add(wrapper);
	return wrapper;
}