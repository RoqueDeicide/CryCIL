#include "stdafx.h"
#include "API_ImplementationHeaders.h"
#include "List.h"

MonoClassWrapper::MonoClassWrapper(MonoClass *klass)
{
	this->wrappedClass = MonoEnv->WrapObject((mono::object)klass, true);
	this->name = mono_class_get_name(klass);
	this->nameSpace = mono_class_get_namespace(klass);
}
MonoClassWrapper::~MonoClassWrapper()
{
	this->wrappedClass->Release();
	delete this->name; this->name = nullptr;
	delete this->nameSpace; this->nameSpace = nullptr;
}
//! Creates an instance of this class.
mono::object MonoClassWrapper::CreateInstance(IMonoArray *args)
{
	MonoObject *obj = mono_object_new(mono_domain_get(), this->GetWrappedClass());
	if (!args || args->Length == 0)
	{
		mono_runtime_object_init(obj);
	}
	else
	{
		// Get the constructor.
		this->GetMethod(".ctor", args)->Invoke(obj, args, false);
	}
	return (mono::object)obj;
}
//! Gets method that can accept arguments of specified types.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, IMonoArray *types)
{
	if (types == nullptr)
	{
		return this->GetMethod(name, (int)0);
	}
	IMonoMethod *result = nullptr;
	// Split the params string into parts.
	int parameterCount = types->Length;
	// Iterate through methods that have given name.
	void *methodIterator = 0;
	while
	(
		MonoMethod *currentMethod =
		mono_class_get_methods(this->GetWrappedClass(), &methodIterator)
	)
	{
		// Check the name.
		if (strcmp(mono_method_get_name(currentMethod), name) == 0)
		{
			// Check the parameters.
			MonoMethodSignature *sig = mono_method_signature(currentMethod);
			// Check the number of parameters.
			if (mono_signature_get_param_count(sig) == parameterCount)
			{
				// Check the type names.
				char **pars = new char *[parameterCount];
				mono_method_get_param_names(currentMethod, (const char **)pars);

				bool mismatchFound = false;
				void *parameterIterator = 0;
				int currentParameterNameIndex = 0;
				while
				(
					MonoType *currentParameterType =
					mono_signature_get_params(sig, &parameterIterator)
				)
				{
					MonoObject *paramObject = types->At<MonoObject *>(currentParameterNameIndex);
					MonoType *paramType = mono_class_get_type(mono_object_get_class(paramObject));
					const char *currentParameterName = mono_type_get_name(paramType);

					if (strcmp(mono_type_get_name(currentParameterType), currentParameterName))
					{
						mismatchFound = true;
					}
					delete currentParameterName;
					currentParameterNameIndex++;
				}
				if (!mismatchFound)
				{
					result = new MonoMethodWrapper(currentMethod);
					break;
				}
			}
		}
	}
	return result;
}
//! Gets the first that matches given description.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, int paramCount)
{
	return
		new MonoMethodWrapper
		(mono_class_get_method_from_name(this->GetWrappedClass(), name, paramCount));
}
//! Gets the method that matches given description.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, const char *params)
{
	if (params == nullptr)
	{
		return this->GetMethod(name, (int)0);
	}
	IMonoMethod *result = nullptr;
	Text *parTypes = new Text(params);
	// Split the params string into parts.
	int parameterCount;
	Text **parameterTypeNames = parTypes->Split(',', parameterCount, true);
	delete parTypes;
	// Iterate through methods that have given name.
	void *methodIterator = 0;
	while
	(
		MonoMethod *currentMethod =
		mono_class_get_methods(this->GetWrappedClass(), &methodIterator)
	)
	{
		// Check the name.
		if (strcmp(mono_method_get_name(currentMethod), name) == 0)
		{
			// Check the parameters.
			MonoMethodSignature *sig = mono_method_signature(currentMethod);
			// Check the number of parameters.
			if (mono_signature_get_param_count(sig) == parameterCount)
			{
				// Check the type names.
				char **pars = new char *[parameterCount];
				mono_method_get_param_names(currentMethod, (const char **)pars);

				bool mismatchFound = false;
				void *parameterIterator = 0;
				int currentParameterNameIndex = 0;
				while
				(
					MonoType *currentParameterType =
					mono_signature_get_params(sig, &parameterIterator)
				)
				{
					const char *currentParameterName =
						parameterTypeNames[currentParameterNameIndex]->ToNTString();
					if (strcmp(mono_type_get_name(currentParameterType), currentParameterName))
					{
						mismatchFound = true;
					}
					delete currentParameterName;
					currentParameterNameIndex++;
				}
				if (!mismatchFound)
				{
					result = new MonoMethodWrapper(currentMethod);
					break;
				}
			}
		}
	}
	for (int i = 0; i < parameterCount; i++)
	{
		delete parameterTypeNames[i];
	}
	delete parameterTypeNames;
	return result;
}
//! Gets an array of methods that matches given description.
IMonoMethod **MonoClassWrapper::GetMethods(const char *name, int paramCount, int &foundCount)
{
	MonoClass *klass = this->GetWrappedClass();
	List<MonoMethod *> methods(mono_class_num_methods(klass));
	void *iter = 0;
	while (MonoMethod *currentMethod = mono_class_get_methods(klass, &iter))
	{
		MonoMethodSignature *sig = mono_method_signature(currentMethod);
		if (mono_signature_get_param_count(sig) == paramCount &&
			!strcmp(mono_method_get_name(currentMethod), name))
		{
			methods.Add(currentMethod);
		}
	}
	foundCount = methods.Length;
	IMonoMethod **foundMethods = new IMonoMethod *[foundCount];
	methods.ThroughEach
	(
		[&foundMethods](MonoMethod *m, int i) { foundMethods[i] = new MonoMethodWrapper(m); }
	);
	return foundMethods;
}
//! Gets an array of overload of the method.
IMonoMethod **MonoClassWrapper::GetMethods(const char *name, int &foundCount)
{
	MonoClass *klass = this->GetWrappedClass();
	List<MonoMethod *> methods(mono_class_num_methods(klass));
	void *iter = 0;
	while (MonoMethod *currentMethod = mono_class_get_methods(klass, &iter))
	{
		if (strcmp(mono_method_get_name(currentMethod), name) == 0)
		{
			methods.Add(currentMethod);
		}
	}
	foundCount = methods.Length;
	IMonoMethod **foundMethods = new IMonoMethod *[foundCount];
	methods.ThroughEach
	(
		[&foundMethods](MonoMethod *m, int i) { foundMethods[i] = new MonoMethodWrapper(m); }
	);
	return foundMethods;
}
//! Gets the value of the object's field.
mono::object MonoClassWrapper::GetField(mono::object obj, const char *name)
{
	if (obj)
	{
		return (mono::object)mono_field_get_value_object
		(
			(MonoDomain *)MonoEnv->AppDomain,
			mono_class_get_field_from_name(this->GetWrappedClass(), name),
			(MonoObject *)obj
		);
	}
	MonoVTable *table = mono_class_vtable(mono_domain_get(), this->GetWrappedClass());
	if (!table)
	{
		CryFatalError("Unable to get a Mono VTable.");
	}
	MonoClassField *field = mono_class_get_field_from_name(this->GetWrappedClass(), name);
	if (field == nullptr)
	{
		CryLogAlways("Field named %s is null.", name);
	}
	MonoType *fieldType = mono_field_get_type(field);
	if (fieldType == nullptr)
	{
		CryLogAlways("Field type is null.");
	}
	MonoClass *fieldClass = mono_class_from_mono_type(fieldType);
	if (fieldClass == nullptr)
	{
		CryLogAlways("Field class is null.");
	}
	if (mono_class_is_valuetype(fieldClass))
	{
		unsigned char *value = new unsigned char[mono_class_value_size(fieldClass, nullptr)];
		mono_field_static_get_value(table, field, value);
		// Box the value, so we can release the value now.
		mono::object result = (mono::object)mono_value_box(mono_domain_get(), fieldClass, value);
		delete value;
		return result;
	}
	else
	{
		mono::object value;
		mono_field_static_get_value(table, field, &value);
		return value;
	}
}
//! Sets the value of the object's field.
void MonoClassWrapper::SetField(mono::object obj, const char *name, void *value)
{
	if (obj)
	{
		mono_field_set_value
		(
			(MonoObject *)obj,
			mono_class_get_field_from_name(this->GetWrappedClass(), name),
			value
		);
	}
	else
	{
		MonoClassField *field = mono_class_get_field_from_name(this->GetWrappedClass(), name);
		MonoVTable *vTable = mono_class_vtable(mono_domain_get(), this->GetWrappedClass());
		mono_field_static_set_value(vTable, field, value);
	}
}
//! Gets the value of the object's property.
mono::object MonoClassWrapper::GetProperty(void *obj, const char *name)
{
	MonoObject *exception;
	mono::object result = (mono::object)mono_property_get_value
	(
		mono_class_get_property_from_name(this->GetWrappedClass(), name),
		obj,
		nullptr,
		&exception
	);
	if (exception)
	{
		MonoEnv->HandleException((mono::object)exception);
		return nullptr;
	}
	return result;
}
//! Sets the value of the object's property.
void MonoClassWrapper::SetProperty(void *obj, const char *name, void *value)
{
	void *pars[1];
	pars[0] = value;
	MonoObject *exception;
	mono_property_set_value
	(
		mono_class_get_property_from_name(this->GetWrappedClass(), name),
		obj,
		pars,
		&exception
	);
	if (exception)
	{
		MonoEnv->HandleException((mono::exception)exception);
	}
}
//! Determines whether this class implements from specified class.
bool MonoClassWrapper::Inherits(const char *nameSpace, const char *className)
{
	MonoClass *base = mono_class_get_parent(this->GetWrappedClass());
	return !strcmp(mono_class_get_name(base), className) &&
		!strcmp(mono_class_get_namespace(base), nameSpace);
}
//! Boxes given value.
mono::object MonoClassWrapper::Box(void *value)
{
	MonoClass *klass = this->GetWrappedClass();
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

IMonoAssembly *MonoClassWrapper::GetAssembly()
{
	return MonoEnv->WrapAssembly(mono_image_get_assembly(mono_class_get_image(this->GetWrappedClass())));
}

void *MonoClassWrapper::GetWrappedPointer()
{
	return this->wrappedClass->GetWrappedPointer();
}

bool MonoClassWrapper::Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses)
{
	void *iterator = 0;
	while (MonoClass *currentInterface = mono_class_get_interfaces(this->GetWrappedClass(), &iterator))
	{
		if (!strcmp(mono_class_get_name(currentInterface), interfaceName) &&
			!strcmp(mono_class_get_namespace(currentInterface), nameSpace))
		{
			return true;
		}
	}
	if (searchBaseClasses)
	{
		MonoClass *base = mono_class_get_parent(this->GetWrappedClass());
		if (base != mono_get_object_class())
		{
			return MonoClassCache::Wrap(base)->Implements(nameSpace, interfaceName);
		}
	}
	return false;
}

IMonoClass *MonoClassWrapper::GetBase()
{
	return MonoClassCache::Wrap(mono_class_get_parent(this->GetWrappedClass()));
}