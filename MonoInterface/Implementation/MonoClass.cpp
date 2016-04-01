#include "stdafx.h"
#include "API_ImplementationHeaders.h"
#include "List.h"
#include "MonoProperty.h"
#include "MonoEvent.h"
#include "MonoField.h"

#if 0
#define ClassMessage CryLogAlways
#define ClassDebug
#else
#define ClassMessage(...) void(0)
#endif

#if 0
#define ClassCtorMessage CryLogAlways
#else
#define ClassCtorMessage(...) void(0)
#endif

MonoClassWrapper::MonoClassWrapper(MonoClass *klass)
	: fullName(nullptr)
	, fullNameIL(nullptr)
	, events(30)
	, methods(30, strcmp)
	, properties(30, strcmp)
	, fields(30)
	, flatMethodList(100)
	, flatPropertyList(100)
{
	ClassCtorMessage("Creating a wrapper.");

	this->wrappedClass = klass;

	ClassCtorMessage("Stored a pointer to the class.");

	this->name      = mono_class_get_name(klass);
	this->nameSpace = mono_class_get_namespace(klass);

	ClassCtorMessage("Stored a name and a namespace of the class.");
	
	MonoClass *base = klass;
	while (base)
	{
		// Cache methods.
		void *iter = nullptr;
		while (MonoMethod *met  = mono_class_get_methods(base, &iter))
		{
			const char *methodName = mono_method_get_name(met);

			ClassCtorMessage("Found a method %s", methodName);
			
			List<IMonoFunction *> *overloads;
			if (!this->methods.TryGet(methodName, overloads))
			{
				overloads = new List<IMonoFunction *>(5);
				this->methods.Add(methodName, overloads);
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

			ClassCtorMessage("Created a wrapper for a method %s", methodName);

			overloads->Add(methodWrapper);

			this->flatMethodList.Add(methodWrapper);
		}
		// Cache properties.
		iter = nullptr;
		while (MonoProperty *prop = mono_class_get_properties(base, &iter))
		{
			const char *propName = mono_property_get_name(prop);

			ClassCtorMessage("Found a property %s", propName);

			List<IMonoProperty *> *overloads;
			if (!this->properties.TryGet(propName, overloads))
			{
				overloads = new List<IMonoProperty *>(5);
				this->properties.Add(propName, overloads);
			}
			auto wrapper = new MonoPropertyWrapper(prop, this);
			overloads->Add(wrapper);

			this->flatPropertyList.Add(wrapper);
		}
		// Cache events.
		iter = nullptr;
		while (MonoEvent *ev   = mono_class_get_events(base, &iter))
		{
			MonoEventWrapper *_event = new MonoEventWrapper(ev, this);

			ClassCtorMessage("Found an event %s", _event->Name);

			this->events.Add(_event);
		}
		// Cache fields.
		iter = nullptr;
		while (MonoClassField *f = mono_class_get_fields(base, &iter))
		{
			MonoField *field = new MonoField(f, this);

			ClassCtorMessage("Found a field %s", field->Name);

			this->fields.Add(field);
		}

		base = mono_class_get_parent(base);
		if (base)
		{
			ClassCtorMessage("Proceeding to the next base class.");
		}
	}

	this->methods.Trim();
	this->properties.Trim();
	this->events.Trim();
	this->fields.Trim();

	this->flatMethodList.Trim();
	this->flatPropertyList.Trim();

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

const IMonoFunction *MonoClassWrapper::GetFunction(const char *name, int paramCount) const
{
	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->methods.At(names[i]);
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

const IMonoFunction *MonoClassWrapper::GetFunction(const char *name, const char *params) const
{
	ClassMessage("Looking for the function %s(%s).", name, params);

	if (params == nullptr)
	{
		return this->GetFunction(name, int(0));
	}
	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->methods.At(names[i]);
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
#ifdef ClassDebug

		ClassMessage("Looking through overloads of the method %s.", name);

		for (int i = 0; i < overloads->Length; i++)
		{
			ClassMessage("Overload #%d: %s", i + 1, overloads->At(i)->Parameters);
		}

#endif // ClassDebug

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

const IMonoFunction *MonoClassWrapper::GetFunction(const char *name, IMonoArray<> &types) const
{
	if (!types)
	{
		return this->GetFunction(name, int(0));
	}

	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->methods.At(names[i]);
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

const IMonoFunction *MonoClassWrapper::GetFunction(const char *name, List<IMonoClass *> &classes) const
{
	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->methods.At(names[i]);
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

const IMonoFunction *MonoClassWrapper::GetFunction(const char *name, List<ClassSpec> &specifiedClasses) const
{
#ifdef ClassDebug
	const char *argList = nullptr;
	if (specifiedClasses.Length != 0)
	{
		TextBuilder builder(50);
		ClassSpec spec = specifiedClasses[0];
		builder << spec.Value1->FullName << spec.Value2;
		for (int i = 1; i < specifiedClasses.Length; i++)
		{
			spec = specifiedClasses[i];
			builder << ", " << spec.Value1->FullName << spec.Value2;
		}

		argList = builder.ToNTString();
	}
	ClassMessage("Getting the function %s(%s)", name, argList ? argList : "");
	if (argList) delete argList;
#endif // ClassDebug

	auto paramTypeNames = List<const char *>(specifiedClasses.Length);

	for (int i = 0; i < specifiedClasses.Length; i++)
	{
		TextBuilder typeName = TextBuilder(50);
		typeName << specifiedClasses[i].Value1->FullNameIL << specifiedClasses[i].Value2;

		const char *typeNameNt = typeName.ToNTString();
		paramTypeNames.Add(typeNameNt);
	}

	auto foundMethod = this->GetFunction(name, paramTypeNames);

	paramTypeNames.DeleteAll();

	return foundMethod;
}

const IMonoFunction *MonoClassWrapper::GetFunction(const char *name, List<const char *> &paramTypeNames) const
{
#ifdef ClassDebug
	const char *argList = nullptr;
	if (paramTypeNames.Length != 0)
	{
		TextBuilder builder(50);
		builder << paramTypeNames[0];
		for (int i = 1; i < paramTypeNames.Length; i++)
		{
			builder << ", " << paramTypeNames[i];
		}

		argList = builder.ToNTString();
	}
	ClassMessage("Getting the function %s(%s)", name, argList ? argList : "");
	if (argList) delete argList;
#endif // ClassDebug

	List<IMonoFunction *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->methods.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->methods.At(names[i]);
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
const List<IMonoFunction *> *MonoClassWrapper::GetFunctions(const char *name, int paramCount) const
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
const List<IMonoFunction *> *MonoClassWrapper::GetFunctions(const char *name) const
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

const IMonoField *MonoClassWrapper::GetField(const char *name) const
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
void MonoClassWrapper::GetField(mono::object obj, const char *name, void *value) const
{
	this->GetFieldValue(obj, mono_class_get_field_from_name(this->wrappedClass, name), value);
}
//! Sets the value of the object's field.
//!
//! Value parameter can either be a pointer to the value-type object to set, or it can be
//! a mono::object that represents a managed object to set.
void MonoClassWrapper::SetField(mono::object obj, const char *name, void *value) const
{
	this->SetFieldValue(obj, mono_class_get_field_from_name(this->wrappedClass, name), value);
}
//! Gets the value of the object's field.
void MonoClassWrapper::GetField(mono::object obj, const IMonoField *field, void *value) const
{
	this->GetFieldValue(obj, field->GetHandle<MonoClassField>(), value);
}

//! Sets the value of the object's field.
//!
//! Value parameter can either be a pointer to the value-type object to set, or it can be
//! a mono::object that represents a managed object to set.
void MonoClassWrapper::SetField(mono::object obj, const IMonoField *field, void *value) const
{
	this->SetFieldValue(obj, field->GetHandle<MonoClassField>(), value);
}

void MonoClassWrapper::GetFieldValue(mono::object obj, MonoClassField *field, void *value) const
{
	if (obj)
	{
		mono_field_get_value(reinterpret_cast<MonoObject *>(obj), field, value);
	}
	else
	{
		mono_field_static_get_value(this->vtable, field, value);
	}
}

void MonoClassWrapper::SetFieldValue(mono::object obj, MonoClassField *field, void *value) const
{
	if (obj)
	{
		mono_field_set_value(reinterpret_cast<MonoObject *>(obj), field, value);
	}
	else
	{
		mono_field_static_set_value(this->vtable, field, value);
	}
}


const IMonoProperty *MonoClassWrapper::GetProperty(const char *name) const
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

const IMonoProperty *MonoClassWrapper::GetProperty(const char *name, IMonoArray<> &types) const
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->properties.At(names[i]);
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

const IMonoProperty *MonoClassWrapper::GetProperty(const char *name, List<IMonoClass *> &classes) const
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->properties.At(names[i]);
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

const IMonoProperty *MonoClassWrapper::GetProperty(const char *name, List<ClassSpec> &specifiedClasses) const
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

const IMonoProperty *MonoClassWrapper::GetProperty(const char *name, List<const char *> &paramTypeNames) const
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->properties.At(names[i]);
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

const IMonoProperty *MonoClassWrapper::GetProperty(const char *name, int paramCount) const
{
	List<IMonoProperty *> *overloads;
	if (!name)
	{
		// Take any name.
		auto names = this->properties.Keys;
		for (int i = 0; i < names.Length; i++)
		{
			overloads = this->properties.At(names[i]);
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
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list, int paramCount) const
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
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list,
														   List<const char *> &paramTypeNames) const
{
	for (int i = 0; i < list.Length; i++)
	{
		auto element = list[i];
		auto m = element->GetFunc();

		if (element->ParameterCount != paramTypeNames.Length)
		{
			continue;
		}

		ClassMessage("Found a method with %d parameters: %s(%s).", paramTypeNames.Length, m->Name, m->Parameters);

		auto currentClasses = m->ParameterTypeNames;
		bool match = true;

		for (int j = 0; j < paramTypeNames.Length; j++)
		{
			if (strcmp(currentClasses->At(j), paramTypeNames[j]) != 0)
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
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list,
														   List<IMonoClass *> &classes) const
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
__forceinline result_type *MonoClassWrapper::SearchTheList(List<result_type *> &list, IMonoArray<> &types) const
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
			MonoType *type = *GET_BOXED_OBJECT_DATA(MonoType *, types[j]);
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

const char *MonoClassWrapper::BuildFullName(bool ilStyle, const char *& field) const
{
	ClassMessage("Querying the full name of the class.");

	if (!field)
	{
		ClassMessage("Building the full name of the class.");

		TextBuilder fullName(100);
		if (MonoClass *nestingClass = mono_class_get_nesting_type(this->wrappedClass))
		{
			IMonoClass *nest = MonoClassCache::Wrap(nestingClass);

			fullName << NtText(ilStyle ? nest->FullNameIL : nest->FullName) << '+';
		}
		else
		{
			fullName << this->nameSpace << '.';
		}

		fullName << this->name;

		ClassMessage("TextBuilder is done.");

		field = fullName.ToNTString();

		ClassMessage("Created a null-terminated version of the name.");
	}
	return field;
}

const IMonoEvent *MonoClassWrapper::GetEvent(const char *name) const
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
bool MonoClassWrapper::Inherits(const char *nameSpace, const char *className) const
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

bool MonoClassWrapper::Inherits(IMonoClass *klass) const
{
	MonoClass *base = mono_class_get_parent(this->wrappedClass);

	while (base)
	{
		if (base == klass->GetWrappedPointer())
		{
			return true;
		}
		base = mono_class_get_parent(base);
	}

	return false;
}
//! Determines whether this class implements from specified class.
bool MonoClassWrapper::Inherits(const char *nameSpace, const char *className, bool direct) const
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

bool MonoClassWrapper::Inherits(IMonoClass *klass, bool direct) const
{
	MonoClass *base = mono_class_get_parent(this->wrappedClass);

	if (direct)
	{
		return base == klass->GetWrappedPointer();
	}

	while (base)
	{
		if (base == klass->GetWrappedPointer())
		{
			return true;
		}
		base = mono_class_get_parent(base);
	}

	return false;
}

//! Boxes given value.
mono::object MonoClassWrapper::Box(void *value) const
{
	MonoClass *klass = this->wrappedClass;
	if (mono_class_is_valuetype(klass))
	{
		return mono::object(mono_value_box(static_cast<MonoDomain *>(MonoEnv->AppDomain), klass, value));
	}
	return nullptr;
}

const char *MonoClassWrapper::GetName() const
{
	return this->name;
}

const char *MonoClassWrapper::GetNameSpace() const
{
	return this->nameSpace;
}

const char *MonoClassWrapper::GetFullName() const
{
	return this->BuildFullName(false, const_cast<MonoClassWrapper *>(this)->fullName);
}

const char *MonoClassWrapper::GetFullNameIL() const
{
	return this->BuildFullName(true, const_cast<MonoClassWrapper *>(this)->fullNameIL);
}

const IMonoAssembly *MonoClassWrapper::GetAssembly() const
{
	return MonoEnv->Assemblies->Wrap(mono_image_get_assembly(mono_class_get_image(this->wrappedClass)));
}

void *MonoClassWrapper::GetWrappedPointer() const
{
	return this->wrappedClass;
}

bool MonoClassWrapper::Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses) const
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

bool MonoClassWrapper::Implements(IMonoClass *interfacePtr, bool searchBaseClasses /*= true*/) const
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

const IMonoClass *MonoClassWrapper::GetBase() const
{
	return MonoClassCache::Wrap(mono_class_get_parent(this->wrappedClass));
}

#if 0
#define NestedTypeMessage CryLogAlways
#else
#define NestedTypeMessage(...) void(0)
#endif

const IMonoClass *MonoClassWrapper::GetNestedType(const char *name) const
{
	NestedTypeMessage("Getting the type %s that is supposed to be nested in %s.", name, this->FullName);

	void *iter = nullptr;
	while (MonoClass *nestedType = mono_class_get_nested_types(this->wrappedClass, &iter))
	{
		NestedTypeMessage("Checking the nested type.");

		const char *nestedTypeName = mono_class_get_name(nestedType);

		NestedTypeMessage("Type is named %s.", nestedTypeName);

		if (strcmp(nestedTypeName, name) == 0)
		{
			return MonoClassCache::Wrap(nestedType);
		}
	}
	return nullptr;
}

mono::type MonoClassWrapper::GetType() const
{
	return mono::type(mono_type_get_object(mono_domain_get(), mono_class_get_type(this->wrappedClass)));
}

mono::type MonoClassWrapper::MakeArrayType() const
{
	return mono::type(mono_type_get_object(mono_domain_get(), mono_class_get_type(mono_bounded_array_class_get(this->wrappedClass, 1, false))));
}

mono::type MonoClassWrapper::MakeArrayType(int rank) const
{
	return mono::type(mono_type_get_object(mono_domain_get(), mono_class_get_type(mono_bounded_array_class_get(this->wrappedClass, rank, false))));
}

mono::type MonoClassWrapper::MakeByRefType() const
{
	return mono::type(mono_type_get_object(mono_domain_get(), mono_class_get_byref_type(this->wrappedClass)));
}

mono::type MonoClassWrapper::MakePointerType() const
{
	return mono::type(mono_type_get_object(mono_domain_get(), mono_class_get_type(mono_ptr_class_get(mono_class_get_type(this->wrappedClass)))));
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

ReadOnlyList<IMonoField *> MonoClassWrapper::GetFields() const
{
	return this->fields.AsReadOnly;
}

ReadOnlyList<IMonoFunction *> MonoClassWrapper::GetFunctions() const
{
	return this->flatMethodList.AsReadOnly;
}

ReadOnlyList<IMonoProperty *> MonoClassWrapper::GetProperties() const
{
	return this->flatPropertyList.AsReadOnly;
}

ReadOnlyList<IMonoEvent *> MonoClassWrapper::GetEvents() const
{
	return this->events.AsReadOnly;
}

bool MonoClassWrapper::GetIsValueType() const
{
	return mono_class_is_valuetype(this->wrappedClass) != 0;
}

bool MonoClassWrapper::GetIsEnum() const
{
	return mono_class_is_enum(this->wrappedClass) != 0;
}

bool MonoClassWrapper::GetIsDelegate() const
{
	return mono_class_is_delegate(this->wrappedClass) != 0;
}

#if 0
#define ClassCacheMessage CryLogAlways
#else
#define ClassCacheMessage(...) void(0)
#endif

SortedList<MonoClass *, MonoClassWrapper *> MonoClassCache::cachedClasses(50);


IMonoClass *MonoClassCache::Wrap(MonoClass *klass)
{
	ClassCacheMessage("Looking for the class %s in the cache.", mono_class_get_name(klass));

	MonoClassWrapper *wrapper;
	if (cachedClasses.TryGet(klass, wrapper))
	{
		return wrapper;
	}

	ClassCacheMessage("Class is not in the cache.");

	// Register a new one.
	wrapper = new MonoClassWrapper(klass);

	ClassCacheMessage("Created a wrapper for a class %s.", mono_class_get_name(klass));

	MonoClassCache::cachedClasses.Add(klass, wrapper);

	ClassCacheMessage("Added a wrapper to the cache.");

	return wrapper;
}

void MonoClassCache::Dispose()
{
	cachedClasses.Dispose();
}
