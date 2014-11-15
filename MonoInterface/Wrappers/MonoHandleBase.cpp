#include "stdafx.h"
#include "API_ImplementationHeaders.h"

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
void MonoHandleBase::SetField(const char *name, void *value)
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
void MonoHandleBase::SetProperty(const char *name, void *value)
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