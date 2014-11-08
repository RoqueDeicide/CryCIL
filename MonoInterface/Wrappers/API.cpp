#include "stdafx.h"
#include "API_ImplementationHeaders.h"

#pragma region Default Boxinator

mono::object DefaultBoxinator::BoxUPtr(void *value)
{
	return this->box(mono_get_uintptr_class(), &value);
}

mono::object DefaultBoxinator::BoxPtr(void *value)
{
	return this->box(mono_get_intptr_class(), &value);
}

mono::object DefaultBoxinator::Box(bool value)
{
	return this->box(mono_get_boolean_class(), &value);
}

mono::object DefaultBoxinator::Box(char value)
{
	return this->box(mono_get_char_class(), &value);
}

mono::object DefaultBoxinator::Box(signed char value)
{
	return this->box(mono_get_sbyte_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned char value)
{
	return this->box(mono_get_byte_class(), &value);
}

mono::object DefaultBoxinator::Box(short value)
{
	return this->box(mono_get_int16_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned short value)
{
	return this->box(mono_get_uint16_class(), &value);
}

mono::object DefaultBoxinator::Box(int value)
{
	return this->box(mono_get_int32_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned int value)
{
	return this->box(mono_get_uint32_class(), &value);
}

mono::object DefaultBoxinator::Box(__int64 value)
{
	return this->box(mono_get_int64_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned __int64 value)
{
	return this->box(mono_get_uint64_class(), &value);
}

mono::object DefaultBoxinator::Box(float value)
{
	return this->box(mono_get_single_class(), &value);
}

mono::object DefaultBoxinator::Box(double value)
{
	return this->box(mono_get_double_class(), &value);
}

mono::object DefaultBoxinator::Box(Vec2 value)
{
	return this->box("Vector2", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Vec3 value)
{
	return this->box("Vector3", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Vec4 value)
{
	return this->box("Vector4", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Ang3 value)
{
	return this->box("EulerAngles", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Quat value)
{
	return this->box("Quaternion", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(QuatT value)
{
	return this->box("QuaternionTranslation", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Matrix33 value)
{
	return this->box("Matrix33", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Matrix34 value)
{
	return this->box("Matrix34", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Matrix44 value)
{
	return this->box("Matrix44", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Plane value)
{
	return this->box("Plane", "CryCil.Mathematics.Geometry", &value);
}

mono::object DefaultBoxinator::Box(Ray value)
{
	return this->box("Ray", "CryCil.Mathematics.Geometry", &value);
}

mono::object DefaultBoxinator::Box(ColorB value)
{
	return this->box("ColorByte", "CryCil.Graphics", &value);
}

mono::object DefaultBoxinator::Box(ColorF value)
{
	return this->box("ColorSingle", "CryCil.Graphics", &value);
}

mono::object DefaultBoxinator::Box(AABB value)
{
	return this->box("BoundingBox", "CryCil.Mathematics", &value);
}
#pragma endregion
#pragma region Mono Array

//
// MonoArrayBase
//


mono::object MonoArrayBase::GetItem(int index)
{
	return *(mono::object *)mono_array_addr_with_size
		(GetWrappedArray, this->GetElementSize(), index);
}
//! Sets an element at specified position.
void MonoArrayBase::SetItem(int index, mono::object value)
{
	*(mono::object *)mono_array_addr_with_size(GetWrappedArray, this->GetElementSize(), index) =
		value;
}

//! Returns number of elements in the array.
int MonoArrayBase::GetSize()
{
	if (this->size == -1)
	{
		this->size = (int)mono_array_length(GetWrappedArray);
	}
	return this->size;
}
//! Returns the size of one element in the memory.
int MonoArrayBase::GetElementSize()
{
	if (this->elementSize == -1)
	{
		this->elementSize =
			mono_array_element_size(mono_object_get_class((MonoObject *)this->GetWrappedPointer()));
	}
	return this->elementSize;
}
//! Gets the wrapper for class that represents elements of the array.
IMonoClass *MonoArrayBase::GetElementClass()
{
	if (this->klass == nullptr)
	{
		this->klass = MonoClassCache::Wrap
		(
			mono_class_get_element_class
			(
				mono_object_get_class((MonoObject *)this->GetWrappedPointer())
			)
		);
	}
	return this->klass;
}

//
// MonoArrayFree
//

MonoArrayFree::MonoArrayFree(int size)
{
	this->size = size;

	this->wrappedArray =
		mono_array_new((MonoDomain *)MonoEnv->AppDomain, mono_get_object_class(), this->size);
}
//! Creates a new array of elements of specific type.
MonoArrayFree::MonoArrayFree(IMonoClass *elementClass, int size)
{
	this->size = size;

	this->wrappedArray = mono_array_new((MonoDomain *)MonoEnv->AppDomain,
		(MonoClass *)elementClass->GetWrappedPointer(), this->size);
}
//! Wraps an existing Mono array.
MonoArrayFree::MonoArrayFree(mono::object arrayHandle)
{
	this->wrappedArray = (MonoArray *)arrayHandle;
	this->size = mono_array_length(this->wrappedArray);
}
//! Returns a pointer to the wrapped array.
void *MonoArrayFree::GetWrappedPointer()
{
	return this->wrappedArray;
}
//! Does nothing since there is nothing to release.
void MonoArrayFree::Release()
{}

//
// MonoArrayPersistent
//

MonoArrayPersistent::MonoArrayPersistent(int size)
{
	this->size = size;
	// Create an array.
	MonoArray *wrappedArray =
		mono_array_new((MonoDomain *)MonoEnv->AppDomain, mono_get_object_class(), this->size);
	// Hold it.
	this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)wrappedArray, false);
}
//! Creates a new array of elements of specific type.
MonoArrayPersistent::MonoArrayPersistent(IMonoClass *elementClass, int size)
{
	this->size = size;
	// Create an array.
	MonoArray *wrappedArray = mono_array_new((MonoDomain *)MonoEnv->AppDomain,
		(MonoClass *)elementClass->GetWrappedPointer(), this->size);
	// Hold it.
	this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)wrappedArray, false);
}
//! Wraps an existing Mono array.
MonoArrayPersistent::MonoArrayPersistent(mono::object arrayHandle)
{
	MonoArray *handle = (MonoArray *)arrayHandle;
	this->size = mono_array_length(handle);
	// Hold it.
	this->wrappedArrayHandle = mono_gchandle_new((MonoObject *)handle, false);
}
//! Returns a pointer to the wrapped array.
void *MonoArrayPersistent::GetWrappedPointer()
{
	if (this->wrappedArrayHandle == -1)
	{
		return nullptr;
	}
	return mono_gchandle_get_target(this->wrappedArrayHandle);
}
//! Removes GC tracking from this array, allowing it to be collected.
void MonoArrayPersistent::Release()
{
	mono_gchandle_free(this->wrappedArrayHandle);
	this->wrappedArrayHandle = -1;
}
#pragma endregion
#pragma region Mono Assembly
MonoAssemblyWrapper::MonoAssemblyWrapper(MonoAssembly *assembly)
{
	this->assembly = assembly;
	this->image = mono_assembly_get_image(assembly);
}
//! Attempts to load assembly located in the file.
//!
//! @param assemblyFile Path to the assembly file to try loading.
//! @param failed       Indicates whether this constructor was successful.
MonoAssemblyWrapper::MonoAssemblyWrapper(const char *assemblyFile, bool &failed)
{
	if (Pdb2MdbThunks::Convert)
	{
		mono::exception ex;
		Pdb2MdbThunks::Convert(MonoEnv->ToManagedString(assemblyFile), &ex);
	}
	this->assembly = mono_domain_assembly_open((MonoDomain *)MonoEnv->AppDomain, assemblyFile);
	failed = !this->assembly;
	this->image = (failed) ? nullptr : mono_assembly_get_image(assembly);
}
//! Gets the class.
//!
//! @param nameSpace Name space where the class is defined.
//! @param className Name of the class to get.
IMonoClass *MonoAssemblyWrapper::GetClass(const char *nameSpace, const char *className)
{
	return MonoClassCache::Wrap(mono_class_from_name(this->image, nameSpace, className));
}
//! Returns a method that satisfies given description.
//!
//! @param nameSpace  Name space where the class where the method is declared is located.
//! @param className  Name of the class where the method is declared.
//! @param methodName Name of the method to look for.
//! @param params     A comma-separated list of names of types of arguments. Can be null
//!                   if method accepts no arguments.
//!
//! @returns A pointer to object that implements IMonoMethod that grants access to
//!          requested method if found, otherwise returns null.
IMonoMethod *MonoAssemblyWrapper::MethodFromDescription
(
	const char *nameSpace, const char *className,
	const char *methodName, const char *params
)
{
	// Get the class.
	IMonoClass *klass = this->GetClass(nameSpace, className);
	// Get the method.
	return klass->GetMethod(methodName, params);
}
//! Returns a pointer to the MonoAssembly for Mono API calls.
void *MonoAssemblyWrapper::GetWrappedPointer()
{
	return this->assembly;
}

mono::assembly MonoAssemblyWrapper::GetReflectionObject()
{
	return (mono::assembly)mono_assembly_get_object((MonoDomain *)MonoEnv->AppDomain, this->assembly);
}
#pragma endregion
#pragma region Mono Class
std::vector<MonoClassWrapper *> MonoClassCache::cachedClasses(50);

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
	std::vector<MonoMethod *> methods(mono_class_num_methods(klass));
	void *iter = 0;
	while (MonoMethod *currentMethod = mono_class_get_methods(klass, &iter))
	{
		MonoMethodSignature *sig = mono_method_signature(currentMethod);
		if (mono_signature_get_param_count(sig) == paramCount &&
			!strcmp(mono_method_get_name(currentMethod), name))
		{
			methods.push_back(currentMethod);
		}
	}
	foundCount = methods.size();
	IMonoMethod **foundMethods = new IMonoMethod *[foundCount];
	int methodIndex = 0;
	for each (auto method in methods)
	{
		foundMethods[methodIndex++] = new MonoMethodWrapper(method);
	}
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
	std::vector<MonoMethod *> methods(mono_class_num_methods(klass));
	void *iter = 0;
	while (MonoMethod *currentMethod = mono_class_get_methods(klass, &iter))
	{
		if (!strcmp(mono_method_get_name(currentMethod), name))
		{
			methods.push_back(currentMethod);
		}
	}
	foundCount = methods.size();
	IMonoMethod **foundMethods = new IMonoMethod *[foundCount];
	int methodIndex = 0;
	for each (auto method in methods)
	{
		foundMethods[methodIndex++] = new MonoMethodWrapper(method);
	}
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
	return this->wrappedClass;
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
#pragma endregion
#pragma region Mono Method
MonoMethodWrapper::MonoMethodWrapper(MonoMethod *method)
{
	this->wrappedMethod = method;
	this->signature = mono_method_signature(this->wrappedMethod);
	this->paramCount = mono_signature_get_param_count(this->signature);
	this->name = mono_method_get_name(this->wrappedMethod);
}

mono::object MonoMethodWrapper::Invoke(mono::object object, IMonoArray *params, bool polymorph)
{
	void **pars = new void*[params->Length];
	for (int i = 0; i < params->Length; i++)
	{
		pars[i] = params->GetItem(i);
	}
	mono::object result = this->Invoke(object, pars, polymorph);
	delete pars;
	return result;
}

mono::object MonoMethodWrapper::Invoke(mono::object object, void **params, bool polymorph)
{
	MonoMethod *methodToInvoke;
	if (polymorph)
	{
		methodToInvoke =
			mono_object_get_virtual_method((MonoObject *)object, this->wrappedMethod);
	}
	else
	{
		methodToInvoke = this->wrappedMethod;
	}
	mono::object exception;
	mono::object result = (mono::object)
		mono_runtime_invoke(methodToInvoke, object, params, (MonoObject **)&exception);
	if (exception)
	{
		MonoEnv->HandleException(exception);
		return nullptr;
	}
	return result;
}

void *MonoMethodWrapper::GetThunk()
{
	return mono_method_get_unmanaged_thunk(this->wrappedMethod);
}

const char *MonoMethodWrapper::GetName()
{
	return this->name;
}

int MonoMethodWrapper::GetParameterCount()
{
	return this->paramCount;
}

void *MonoMethodWrapper::GetWrappedPointer()
{
	return this->wrappedMethod;
}
#pragma endregion
#pragma region Mono Handle
mono::object MonoHandleBase::CallMethod(const char *name, IMonoArray *args)
{
	MonoObject *obj = (MonoObject *)this->Get();
	MonoMethod *method =
		mono_class_get_method_from_name(this->getMonoClass(), name, args->Length);
	MonoObject *exception = nullptr;
	MonoObject *result = mono_runtime_invoke_array
		(method, obj, (MonoArray *)args->GetWrappedPointer(), &exception);
	if (exception)
	{
		MonoEnv->HandleException((mono::object)exception);
	}
	else
	{
		return (mono::object)result;
	}
	return nullptr;
}
//! Gets the value of the object's field.
//!
//! @param name Name of the field which value to get.
mono::object MonoHandleBase::GetField(const char *name)
{
	return this->GetClass()->GetField(this->Get(), name);
}
//! Sets the value of the object's field.
//!
//! @param name  Name of the field which value to set.
//! @param value New value to assign to the field.
void MonoHandleBase::SetField(const char *name, mono::object value)
{
	this->GetClass()->SetField(this->Get(), name, value);
}
//! Gets the value of the object's property.
//!
//! @param name Name of the property which value to get.
mono::object MonoHandleBase::GetProperty(const char *name)
{
	return this->GetClass()->GetProperty(this->Get(), name);
}
//! Sets the value of the object's property.
//!
//! @param name  Name of the property which value to set.
//! @param value New value to assign to the property.
void MonoHandleBase::SetProperty(const char *name, mono::object value)
{
	this->GetClass()->SetProperty(this->Get(), name, value);
}
//! Gets the wrapper for the class of this object.
IMonoClass *MonoHandleBase::GetClass()
{
	if (!this->type)
	{
		// Cache the type of this object, so we don't have to get it over and over again.
		this->type = MonoClassCache::Wrap(this->getMonoClass());
	}
	return this->type;
}

void *MonoHandleBase::UnboxObject()
{
	return mono_object_unbox((MonoObject *)this->Get());
}

void *MonoHandleBase::GetWrappedPointer()
{
	return this->Get();
}
#pragma endregion