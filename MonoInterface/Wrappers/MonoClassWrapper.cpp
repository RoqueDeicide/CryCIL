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
//!
//! @param args Arguments to pass to the constructor, can be null if latter has no parameters.
mono::object MonoClassWrapper::CreateInstance(IMonoArray *args)
{
	mono::exception exception;
	mono::object obj = MonoClassThunks::CreateInstance
	(
		(mono::object)mono_class_get_type(this->GetWrappedClass()),
		(args == nullptr) ? nullptr : (mono::object)args->GetWrappedPointer(),
		&exception
	);
	if (exception)
	{
		MonoEnv->HandleException(exception);
		return nullptr;
	}
	return obj;
}
//! Gets method that can accept arguments of specified types.
//!
//! @param name  Name of the method to get.
//! @param types An array of arguments which types specify method signature to use.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, IMonoArray *types)
{
	MonoMethod *currentMethod;
	void *iterator = 0;
	bool foundMatch = false;
	int typesCount = ((types == nullptr) ? 0 : types->Length);
	// Iterate through methods.
	while ((currentMethod = mono_class_get_methods(this->GetWrappedClass(), &iterator)) && !foundMatch)
	{
		MonoMethodSignature *sig = mono_method_signature(currentMethod);
		// Check number of parameters.
		if (!strcmp(name, mono_method_get_name(currentMethod)) &&
			mono_signature_get_param_count(sig) == typesCount &&
			this->ParametersMatch(sig, types))
		{
			foundMatch = true;
		}
	}
	return currentMethod == nullptr ? nullptr : new MonoMethodWrapper(currentMethod);
}
//! Gets the first that matches given description.
//!
//! @param name       Name of the method to find.
//! @param paramCount Number of arguments the method should take.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, int paramCount)
{
	return
		new MonoMethodWrapper
		(mono_class_get_method_from_name(this->GetWrappedClass(), name, paramCount));
}
//! Gets the method that matches given description.
//!
//! @param name   Name of the method to find.
//! @param params Text that describes types arguments the method should take.
//!
//! @returns A pointer to the wrapper to the found method. Null is returned if
//!          no method matching the description was found.
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
//!
//! @param name       Name of the methods to find.
//! @param paramCount Number of arguments the methods should take.
//! @param foundCount Reference to the variable that will contain
//!                   number of found methods.
//! @returns A pointer to the first found method. You should release
//!          resultant array once you don't need it anymore.
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
//!
//! @param name       Name of the method which overloads to find.
//! @param foundCount Reference to the variable that will contain
//!                   number of found methods.
//! @returns A pointer to the first found method. You should release
//!          resultant array once you don't need it anymore.
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
//!
//! @param obj  Object which field to get.
//! @param name Name of the field which value to get.
mono::object MonoClassWrapper::GetField(mono::object obj, const char *name)
{
	return (mono::object)mono_field_get_value_object
	(
		(MonoDomain *)MonoEnv->AppDomain,
		mono_class_get_field_from_name(this->GetWrappedClass(), name),
		(MonoObject *)obj
	);
}
//! Sets the value of the object's field.
//!
//! @param obj   Object which field to set.
//! @param name  Name of the field which value to set.
//! @param value New value to assign to the field.
void MonoClassWrapper::SetField(mono::object obj, const char *name, mono::object value)
{
	mono_field_set_value
	(
		(MonoObject *)obj,
		mono_class_get_field_from_name(this->GetWrappedClass(), name),
		value
	);
}
//! Gets the value of the object's property.
//!
//! @param obj  Object which property to get.
//! @param name Name of the property which value to get.
mono::object MonoClassWrapper::GetProperty(mono::object obj, const char *name)
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
//!
//! @param obj   Object which property to set.
//! @param name  Name of the property which value to set.
//! @param value New value to assign to the property.
void MonoClassWrapper::SetProperty(mono::object obj, const char *name, mono::object value)
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
		MonoEnv->HandleException((mono::object)exception);
	}
}
//! Determines whether this class implements from specified class.
//!
//! @param nameSpace Full name of the name space where the class is located.
//! @param className Name of the class.
//!
//! @returns True, if this class is a subclass of specified one.
bool MonoClassWrapper::Inherits(const char *nameSpace, const char *className)
{
	MonoClass *base = mono_class_get_parent(this->GetWrappedClass());
	return !strcmp(mono_class_get_name(base), className) &&
		!strcmp(mono_class_get_namespace(base), nameSpace);
}
//! Boxes given value.
//!
//! @returns Null if this class is not a value-type, or reference to the boxed object, if it is.
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
bool MonoClassWrapper::ParametersMatch(MonoMethodSignature *sig, IMonoArray *pars)
{
	if (!pars)
	{
		// This code will only execute if there are no parameters in the signature,
		// therefore, signatures do match since there are no parameters to compare.
		return true;
	}
	// Go through parameters.
	void *paramIterator = 0;
	for (int i = 0; i < pars->Length; i++)
	{
		MonoType *paramType = mono_signature_get_params(sig, &paramIterator);
		MonoType *arrayParamType =
			mono_class_get_type(mono_object_get_class((MonoObject *)pars->GetItem(i)));

		mono::object exception;
		mono::object comparisonResult =
			MonoClassThunks::StaticEquals
			((mono::object)paramType, (mono::object)arrayParamType, &exception);
		if (exception)
		{
			return false;
		}
		bool match = MonoEnv->WrapObject(comparisonResult)->Unbox<bool>();
		if (!match)
		{
			return false;
		}
	}
	return true;
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