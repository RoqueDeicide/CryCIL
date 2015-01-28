#include "stdafx.h"
#include "API_ImplementationHeaders.h"
#include "List.h"
#include "MonoProperty.h"
#include "MonoEvent.h"

MonoClassWrapper::MonoClassWrapper(MonoClass *klass)
{
	this->wrappedClass = klass;
	this->name = mono_class_get_name(klass);
	this->nameSpace = mono_class_get_namespace(klass);
}
MonoClassWrapper::~MonoClassWrapper()
{
}
//! Creates an instance of this class.
mono::object MonoClassWrapper::CreateInstance(IMonoArray *args)
{
	MonoObject *obj = mono_object_new(mono_domain_get(), this->wrappedClass);
	if (!args || args->Length == 0)
	{
		mono_runtime_object_init(obj);
	}
	else
	{
		// Create a type spec for the constructor look-up.
		List<TypeSpec> *typeSpecs = new List<TypeSpec>(args->Length);
		for (int i = 0; i < args->Length; i++)
		{
			typeSpecs->Add(TypeSpec(MonoClassCache::Wrap(mono_object_get_class((MonoObject *)args->At<mono::object>(i)))));
		}
		// Get the constructor.
		this->GetMethod(".ctor", typeSpecs)->Invoke(obj, args, false);
	}
	return (mono::object)obj;
}
//! Gets method that can accept arguments of specified types.
IMonoMethod *MonoClassWrapper::GetMethod(const char *name, List<TypeSpec> *types)
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
		mono_class_get_methods(this->wrappedClass, &methodIterator)
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
				// Check the type names and their specifications.
				List<MonoType *> *parTypes = new List<MonoType *>(parameterCount);
				// Get the list of parameter types from the signature.
				void *parIter = 0;
				while (MonoType *currentParameterType = mono_signature_get_params(sig, &parIter))
				{
					parTypes->Add(currentParameterType);
				}
				// Compare types and given specifications.
				int i;
				for (i = 0; i < parameterCount; i++)
				{
					IMonoClass *parClass = MonoClassCache::Wrap(mono_class_from_mono_type(parTypes->At(i)));
					// Check the name.
					if (strcmp(parClass->FullName, types->At(i).Class->FullName) == 0)
					{
						// Check the specification.
						bool parIsByRef = mono_type_is_byref(parTypes->At(i)) != 0;
						bool parIsPointer = mono_type_get_type(parTypes->At(i)) == MONO_TYPE_PTR;
						if (parIsByRef == types->At(i).IsByRef
							&& parIsPointer == types->At(i).IsPointer)
						{
							continue;
						}
						else
						{
							break;
						}
					}
				}
				if (i == parameterCount)
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
		(mono_class_get_method_from_name(this->wrappedClass, name, paramCount));
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
		mono_class_get_methods(this->wrappedClass, &methodIterator)
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
	MonoClass *klass = this->wrappedClass;
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
	MonoClass *klass = this->wrappedClass;
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
void MonoClassWrapper::GetField(mono::object obj, const char *name, void *value)
{
	if (obj)
	{
		mono_field_get_value
			((MonoObject *)obj, mono_class_get_field_from_name(this->wrappedClass, name), value);
	}
	else
	{
		MonoClassField *field = mono_class_get_field_from_name(this->wrappedClass, name);
		MonoVTable *vTable = mono_class_vtable(mono_domain_get(), this->wrappedClass);
		mono_field_static_get_value(vTable, field, value);
	}
}
//! Sets the value of the object's field.
void MonoClassWrapper::SetField(mono::object obj, const char *name, void *value)
{
	if (obj)
	{
		mono_field_set_value
		((MonoObject *)obj, mono_class_get_field_from_name(this->wrappedClass, name), value);
	}
	else
	{
		MonoClassField *field = mono_class_get_field_from_name(this->wrappedClass, name);
		MonoVTable *vTable = mono_class_vtable(mono_domain_get(), this->wrappedClass);
		mono_field_static_set_value(vTable, field, value);
	}
}

IMonoProperty *MonoClassWrapper::GetProperty(const char *name)
{
	return new MonoPropertyWrapper(mono_class_get_property_from_name(this->wrappedClass, name));
}

IMonoEvent *MonoClassWrapper::GetEvent(const char *name)
{
	void *iter = 0;
	MonoEvent *_ev = nullptr;
	while (_ev = mono_class_get_events(this->wrappedClass, &iter))
	{
		if (strcmp(name, mono_event_get_name(_ev)) == 0)
		{
			break;
		}
	}
	if (!_ev)
	{
		return nullptr;
	}
	return new MonoEventWrapper(_ev);
}
//! Determines whether this class implements from specified class.
bool MonoClassWrapper::Inherits(const char *nameSpace, const char *className)
{
	MonoClass *base = mono_class_get_parent(this->wrappedClass);
	return !strcmp(mono_class_get_name(base), className) &&
		!strcmp(mono_class_get_namespace(base), nameSpace);
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
	return fullName.ToNTString();
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

IMonoClass * MonoClassWrapper::GetNestedType(const char *name)
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

List<MonoClassWrapper *> MonoClassCache::cachedClasses(50);


IMonoClass *MonoClassCache::Wrap(MonoClass *klass)
{
	// Cool lambda expression.
	auto condition = [&klass](MonoClassWrapper *w)
	{
		return w->GetWrappedPointer() == klass;
	};
	if (MonoClassWrapper *wrapper = cachedClasses.Find(condition))
	{
		// Return registered wrapper.
		return wrapper;
	}
	// Register a new one.
	MonoClassWrapper *wrapper = new MonoClassWrapper(klass);
	MonoClassCache::cachedClasses.Add(wrapper);
	return wrapper;
}