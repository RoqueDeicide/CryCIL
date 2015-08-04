#include "stdafx.h"
#include "API_ImplementationHeaders.h"
#include "List.h"
#include "MonoProperty.h"
#include "MonoEvent.h"
#include "MonoField.h"

MonoClassWrapper::MonoClassWrapper(MonoClass *klass)
	: fullName(nullptr)
	, fullNameIL(nullptr)
	, events(30)
	, fields(30)
{
	this->wrappedClass = klass;

	this->name      = mono_class_get_name(klass);
	this->nameSpace = mono_class_get_namespace(klass);

	this->methods    = SortedList<const char *, List<IMonoFunction *> *>(30, strcmp);
	this->properties = SortedList<const char *, List<IMonoProperty *> *>(30, strcmp);
	
	MonoClass *base = klass;
	while (base)
	{
		// Cache methods.
		void *iter = nullptr;
		while (MonoMethod *met  = mono_class_get_methods(base, &iter))
		{
			const char *methodName = mono_method_get_name(met);

			if (!this->methods.Contains(methodName))
			{
				this->methods.At(methodName) = new List<IMonoFunction *>(5);
			}
			IMonoFunction *methodWrapper;
			if (strcmp(mono_method_get_name(met), ".ctor") == 0)
			{
				methodWrapper = new MonoConstructor(met, this);
			}
			else if (mono_signature_is_instance(mono_method_signature(met)))
			{
				methodWrapper = new MonoMethodWrapper(met, this);
			}
			else
			{
				methodWrapper = new MonoStaticMethod(met, this);
			}

			this->methods.At(methodName)->Add(methodWrapper);
		}
		// Cache properties.
		iter = nullptr;
		while (MonoProperty *prop = mono_class_get_properties(base, &iter))
		{
			const char *propName = mono_property_get_name(prop);

			if (!this->properties.Contains(propName))
			{
				this->properties.At(propName) = new List<IMonoProperty *>(5);
			}
			this->properties.At(propName)->Add(new MonoPropertyWrapper(prop));
		}
		// Cache events.
		iter = nullptr;
		while (MonoEvent *ev   = mono_class_get_events(base, &iter))
		{
			this->events.Add(new MonoEventWrapper(ev));
		}
		// Cache fields.
		iter = nullptr;
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
	// Create a simple list of methods for occasions when a simple list needs to be iterated through.
	this->flatMethodList = List<IMonoFunction *>(this->methods.Length * 2);
	this->methods.ForEach
	(
		[this](const char *name, List<IMonoFunction *> *overloads)
		{
			for (int i = 0; i < overloads->Length; i++)
			{
				this->flatMethodList.Add(overloads->At(i));
			}
		}
	);

	this->flatMethodList.Trim();

	this->vtable = mono_class_vtable(mono_domain_get(), klass);
}
MonoClassWrapper::~MonoClassWrapper()
{
	SAFE_DELETE(this->fullName);
	SAFE_DELETE(this->fullNameIL);

	auto deletePropertyOverloads = [](const char *name, List<IMonoProperty *> *&overloads)
	{
		overloads->DeleteAll();
		delete overloads;
	};
	this->properties.ForEach(deletePropertyOverloads);

	this->events.DeleteAll();

	auto deleteMethodOverloads = [](const char *name, List<IMonoFunction *> *&overloads)
	{
		overloads->DeleteAll();
		delete overloads;
	};
	this->methods.ForEach(deleteMethodOverloads);

	this->fields.DeleteAll();
}

IMonoFunction *MonoClassWrapper::GetFunction(const char *name, int paramCount)
{
	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->methods.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoFunction>(*overloads, paramCount))
			{
				return found;
			}
		}
	}
	if (this->methods.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoFunction>(*overloads, paramCount);
	}
	return nullptr;
}

IMonoFunction *MonoClassWrapper::GetFunction(const char *name, const char *params)
{
	if (params == nullptr)
	{
		return this->GetFunction(name, (int)0);
	}
	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->methods.At(names->At(i));
			for (int j = 0; j < overloads->Length; j++)
			{
				IMonoFunction *m = overloads->At(j);
				if (strcmp(m->Parameters, params) == 0)
				{
					return m;
				}
			}
		}
	}
	if (this->methods.TryGet(name, overloads))
	{
		for (int i = 0; i < overloads->Length; i++)
		{
			IMonoFunction *m = overloads->At(i);
			if (strcmp(m->Parameters, params) == 0)
			{
				return m;
			}
		}
	}
	return nullptr;
}

IMonoFunction *MonoClassWrapper::GetFunction(const char *name, IMonoArray<> &types)
{
	if (!types)
	{
		return this->GetFunction(name, (int)0);
	}

	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->methods.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoFunction>(*overloads, types))
			{
				return found;
			}
		}
	}
	if (this->methods.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoFunction>(*overloads, types);
	}
	return nullptr;
}

IMonoFunction *MonoClassWrapper::GetFunction(const char *name, List<IMonoClass *> &classes)
{
	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->methods.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoFunction>(*overloads, classes))
			{
				return found;
			}
		}
	}
	if (this->methods.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoFunction>(*overloads, classes);
	}
	return nullptr;
}

IMonoFunction *MonoClassWrapper::GetFunction(const char *name, List<ClassSpec> &specifiedClasses)
{
	auto paramTypeNames = List<const char *>(specifiedClasses.Length);

	for (int i = 0; i < specifiedClasses.Length; i++)
	{
		TextBuilder typeName = TextBuilder(10);
		typeName << specifiedClasses[i].Value1->FullNameIL << specifiedClasses[i].Value2;

		const char *typeNameNt = typeName.ToNTString();
		paramTypeNames.Add(typeNameNt);
	}

	IMonoFunction *foundMethod = this->GetFunction(name, paramTypeNames);

	for (int i = 0; i < paramTypeNames.Length; i++)
	{
		delete paramTypeNames[i];
	}

	return foundMethod;
}

IMonoFunction *MonoClassWrapper::GetFunction(const char *name, List<const char *> &paramTypeNames)
{
	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->methods.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoFunction>(*overloads, paramTypeNames))
			{
				return found;
			}
		}
	}
	if (this->methods.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoFunction>(*overloads, paramTypeNames);
	}
	return nullptr;
}

//! Gets an array of methods that matches given description.
List<IMonoFunction *> *MonoClassWrapper::GetFunctions(const char *name, int paramCount)
{
	auto foundMethods = new List<IMonoFunction *>(this->methods.Length);

	List<IMonoFunction *> *overloads;
	if (this->methods.TryGet(name, overloads))
	{
		for (int i = 0; i < overloads->Length; i++)
		{
			IMonoFunction *m = overloads->At(i);
			
			if (m->ParameterCount == paramCount)
			{
				foundMethods->Add(m);
			}
		}
	}

	foundMethods->Trim();

	return foundMethods;
}
//! Gets an array of overload of the method.
List<IMonoFunction *> *MonoClassWrapper::GetFunctions(const char *name)
{
	auto foundMethods = new List<IMonoFunction *>(this->methods.Length);

	List<IMonoFunction *> *overloads;
	if (this->methods.TryGet(name, overloads))
	{
		for (int i = 0; i < overloads->Length; i++)
		{
			foundMethods->Add(overloads->At(i));
		}
	}

	foundMethods->Trim();

	return foundMethods;
}

IMonoField *MonoClassWrapper::GetField(const char *name)
{
	for (int i = 0; i < this->fields.Length; i++)
	{
		if (strcmp(this->fields[i]->Name, name) == 0)
		{
			return this->fields[i];
		}
	}
	return nullptr;
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
	List<IMonoProperty *> *overloads;
	if (this->properties.TryGet(name, overloads))
	{
		for (int i = 0; i < overloads->Length; i++)
		{
			IMonoProperty *prop = overloads->At(i);
			if (prop->Identifier->ParameterCount == 0)
			{
				return prop;
			}
		}
		return overloads->At(0);
	}
	return nullptr;
}

IMonoProperty *MonoClassWrapper::GetProperty(const char *name, IMonoArray<> &types)
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->properties.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoProperty>(*overloads, types))
			{
				return found;
			}
		}
	}
	if (this->properties.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoProperty>(*overloads, types);
	}
	return nullptr;
}

IMonoProperty *MonoClassWrapper::GetProperty(const char *name, List<IMonoClass *> &classes)
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->properties.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoProperty>(*overloads, classes))
			{
				return found;
			}
		}
	}
	if (this->properties.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoProperty>(*overloads, classes);
	}
	return nullptr;
}

IMonoProperty *MonoClassWrapper::GetProperty(const char *name, List<ClassSpec> &specifiedClasses)
{
	auto paramTypeNames = List<const char *>(specifiedClasses.Length);

	for (int i = 0; i < specifiedClasses.Length; i++)
	{
		TextBuilder typeName = TextBuilder(10);
		typeName << specifiedClasses[i].Value1->FullNameIL << specifiedClasses[i].Value2;

		const char *typeNameNt = typeName.ToNTString();
		paramTypeNames.Add(typeNameNt);
	}

	auto foundProp = this->GetProperty(name, paramTypeNames);

	for (int i = 0; i < paramTypeNames.Length; i++)
	{
		delete paramTypeNames[i];
	}

	return foundProp;
}

IMonoProperty *MonoClassWrapper::GetProperty(const char *name, List<const char *> &paramTypeNames)
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->properties.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoProperty>(*overloads, paramTypeNames))
			{
				return found;
			}
		}
	}
	if (this->properties.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoProperty>(*overloads, paramTypeNames);
	}
	return nullptr;
}

IMonoProperty *MonoClassWrapper::GetProperty(const char *name, int paramCount)
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names->Length; i++)
		{
			overloads = this->properties.At(names->At(i));
			if (auto found = this->SearchTheList<IMonoProperty>(*overloads, paramCount))
			{
				return found;
			}
		}
	}
	if (this->properties.TryGet(name, overloads))
	{
		return this->SearchTheList<IMonoProperty>(*overloads, paramCount);
	}
	return nullptr;
}

template<typename result_type>
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list, int paramCount)
{
	if (paramCount == -1)
	{
		// Take any number of parameters.
		return list.Length == 0 ? nullptr : list[0];
	}
	for (int i = 0; i < list.Length; i++)
	{
		auto element = list[i];
		if (element->ParameterCount == paramCount)
		{
			return element;
		}
	}
	return nullptr;
}

template<typename result_type>
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list, List<const char *> &paramTypeNames)
{
	for (int i = 0; i < list.Length; i++)
	{
		auto element = list[i];
		auto m = element->GetFunc();

		if (element->ParameterCount != paramTypeNames.Length)
		{
			continue;
		}

		auto currentClasses = m->ParameterTypeNames;
		bool match = true;

		for (int j = 0; j < paramTypeNames.Length; j++)
		{
			if (currentClasses->At(j) != paramTypeNames[j])
			{
				match = false;
				break;
			}
		}
		if (match)
		{
			return element;
		}
	}
	return nullptr;
}

template<typename result_type>
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list, List<IMonoClass *> &classes)
{
	for (int i = 0; i < list.Length; i++)
	{
		auto element = list[i];
		auto m = element->GetFunc();

		if (element->ParameterCount != classes.Length)
		{
			continue;
		}

		auto currentClasses = m->ParameterClasses;
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
			return element;
		}
	}
	return nullptr;
}

template<typename result_type>
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list, IMonoArray<> &types)
{
	for (int i = 0; i < list.Length; i++)
	{
		auto element = list[i];
		auto m = element->GetFunc();

		int paramCount = element->ParameterCount;

		if (paramCount != types.Length)
		{
			continue;
		}

		auto typeNames = m->ParameterTypeNames;

		bool match = true;
		for (int j = 0; j < paramCount; j++)
		{
			// Look at definition of _MonoReflectionType in mono sources to see what is going on here.
			MonoType *type = (MonoType *)((unsigned char *)(&types[j]) + sizeof(MonoObject));
			if (strcmp(typeNames->At(j), mono_type_get_name(type)) != 0)
			{
				match = false;
				break;
			}
		}
		if (match)
		{
			return element;
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

bool MonoClassWrapper::Inherits(IMonoClass *klass)
{
	MonoClass *base = mono_class_get_parent(this->wrappedClass);

	while (base)
	{
		if (this->wrappedClass == klass->GetWrappedPointer())
		{
			return true;
		}
		base = mono_class_get_parent(base);
	}

	return false;
}
//! Determines whether this class implements from specified class.
bool MonoClassWrapper::Inherits(const char *nameSpace, const char *className, bool direct)
{
	MonoClass *base = mono_class_get_parent(this->wrappedClass);

	if (direct)
	{
		return  strcmp(mono_class_get_name(base), className) == 0 &&
				strcmp(mono_class_get_namespace(base), nameSpace) == 0;
	}

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

bool MonoClassWrapper::Inherits(IMonoClass *klass, bool direct)
{
	MonoClass *base = mono_class_get_parent(this->wrappedClass);

	if (direct)
	{
		return this->wrappedClass == klass->GetWrappedPointer();
	}

	while (base)
	{
		if (this->wrappedClass == klass->GetWrappedPointer())
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
		TextBuilder fullName;
		if (MonoClass *nestingClass = mono_class_get_nesting_type(this->wrappedClass))
		{
			const char *nestingName = MonoClassCache::Wrap(nestingClass)->FullName;

			fullName = TextBuilder(strlen(this->name) + strlen(nestingName) + 1);

			fullName << nestingName << "." << this->name;

			delete nestingName;
		}
		else
		{
			fullName = TextBuilder(strlen(this->name) + strlen(this->nameSpace) + 1);

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
		TextBuilder fullName;
		if (MonoClass *nestingClass = mono_class_get_nesting_type(this->wrappedClass))
		{
			const char *nestingName = MonoClassCache::Wrap(nestingClass)->FullName;

			fullName = TextBuilder(strlen(this->name) + strlen(nestingName) + 1);

			fullName << nestingName << "+" << this->name;

			delete nestingName;
		}
		else
		{
			fullName = TextBuilder(strlen(this->name) + strlen(this->nameSpace) + 1);

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
	void *iterator = nullptr;
	MonoClass *currentClass = this->wrappedClass;
	do
	{
		while (MonoClass *currentInterface = mono_class_get_interfaces(currentClass, &iterator))
		{
			if (!strcmp(mono_class_get_name(currentInterface), interfaceName) &&
				!strcmp(mono_class_get_namespace(currentInterface), nameSpace))
			{
				return true;
			}
		}
		// Move to the base class, if needed.
		currentClass =
			searchBaseClasses
			? mono_class_get_parent(currentClass)
			: nullptr;
	} while (currentClass && currentClass != MonoEnv->CoreLibrary->Object->GetWrappedPointer());

	return false;
}

bool MonoClassWrapper::Implements(IMonoClass *interfacePtr, bool searchBaseClasses /*= true*/)
{
	void *iterator = nullptr;
	MonoClass *currentClass = this->wrappedClass;
	do
	{
		while (MonoClass *currentInterface = mono_class_get_interfaces(currentClass, &iterator))
		{
			if (currentInterface == interfacePtr->GetWrappedPointer())
			{
				return true;
			}
		}
		// Move to the base class, if needed.
		currentClass =
			searchBaseClasses
			? mono_class_get_parent(currentClass)
			: nullptr;
	} while (currentClass && currentClass != MonoEnv->CoreLibrary->Object->GetWrappedPointer());

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
	return (mono::type)mono_type_get_object(mono_domain_get(), mono_class_get_type(mono_bounded_array_class_get(this->wrappedClass, 1, false)));
}

mono::type MonoClassWrapper::MakeArrayType(int rank)
{
	return (mono::type)mono_type_get_object(mono_domain_get(), mono_class_get_type(mono_bounded_array_class_get(this->wrappedClass, rank, false)));
}

mono::type MonoClassWrapper::MakeByRefType()
{
	return (mono::type)mono_type_get_object(mono_domain_get(), mono_class_get_byref_type(this->wrappedClass));
}

mono::type MonoClassWrapper::MakePointerType()
{
	return (mono::type)mono_type_get_object(mono_domain_get(), mono_class_get_type(mono_ptr_class_get(mono_class_get_type(this->wrappedClass))));
}

// IMonoClass *MonoClassWrapper::Inflate(List<IMonoClass *> &types)
// {
// 	MonoType *thisType = mono_class_get_type(this->wrappedClass);
// 	MonoReflectionType *thisTypeObj = mono_type_get_object(mono_domain_get(), thisType);
// 	unsigned int thisTypeGcHandle = mono_gchandle_new((MonoObject *)thisTypeObj, true);
// 
// 	MonoClassWrapper *typeClass = static_cast<MonoClassWrapper *>(MonoEnv->CoreLibrary->Type);
// 	MonoArray *typesArray = mono_array_new(mono_domain_get(), typeClass->wrappedClass, types.Length);
// 	// Pin the array of types.
// 	unsigned int typesGcHandle = mono_gchandle_new((MonoObject *)typesArray, true);
// 	// For pinning types within the array, just for extra safety.
// 	List<unsigned int> typeGcHandles(types.Length);
// 	// Fill the Mono array with types.
// 	for (int i = 0; i < types.Length; i++)
// 	{
// 		MonoType *currentType = mono_class_get_type(((MonoClassWrapper *)types[i])->wrappedClass);
// 		MonoReflectionType *currentTypeObj = mono_type_get_object(mono_domain_get(), currentType);
// 		// Pin the type.
// 		typeGcHandles.Add(mono_gchandle_new((MonoObject *)currentTypeObj, true));
// 		// Put the type in the array.
// 		*mono_array_addr(typesArray, MonoReflectionType *, i) = currentTypeObj;
// 	}
// 	// Now invoke a method that will inflate the generic type for us.
// 	void *params[1];
// 	params[0] = typesArray;
// 	mono::type inflatedType = typeClass->GetMethod("MakeGenericType", 1)->Invoke(thisTypeObj, params);
// 	if (!inflatedType)
// 	{
// 		return this;
// 	}
// 	IMonoClass *result = MonoClassCache::Wrap(mono_class_from_mono_type(GET_BOXED_OBJECT_DATA(MonoType, inflatedType)));
// 	// Unpin everything.
// 	mono_gchandle_free(thisTypeGcHandle);
// 	mono_gchandle_free(typesGcHandle);
// 	for (int i = 0; i < typeGcHandles.Length; i++)
// 	{
// 		mono_gchandle_free(typeGcHandles[i]);
// 	}
// 	return result;
// }

ReadOnlyList<IMonoField *> *MonoClassWrapper::GetFields()
{
	return (ReadOnlyList<IMonoField *> *)&this->fields;
}

ReadOnlyList<IMonoFunction *> *MonoClassWrapper::GetFunctions()
{
	return (ReadOnlyList<IMonoFunction *> *)&this->flatMethodList;
}

ReadOnlyList<IMonoProperty *> *MonoClassWrapper::GetProperties()
{
	return (ReadOnlyList<IMonoProperty *> *)&this->properties;
}

ReadOnlyList<IMonoEvent *> *MonoClassWrapper::GetEvents()
{
	return (ReadOnlyList<IMonoEvent *> *)&this->events;
}

bool MonoClassWrapper::GetIsValueType()
{
	return mono_class_is_valuetype(this->wrappedClass) != 0;
}

bool MonoClassWrapper::GetIsEnum()
{
	return mono_class_is_enum(this->wrappedClass) != 0;
}

bool MonoClassWrapper::GetIsDelegate()
{
	return mono_class_is_delegate(this->wrappedClass) != 0;
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

void MonoClassCache::Dispose()
{
	cachedClasses.DeleteAll();
	cachedClasses.Clear();
}
