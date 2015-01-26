#include "stdafx.h"
#include "API_ImplementationHeaders.h"

mono::object MonoHandle::CallMethod(const char *name, IMonoArray *args)
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
void MonoHandle::GetField(const char *name, void *value)
{
	return this->GetClass()->GetField(this->Get(), name, value);
}
//! Sets the value of the object's field.
void MonoHandle::SetField(const char *name, void *value)
{
	this->GetClass()->SetField(this->Get(), name, value);
}

IMonoProperty *MonoHandle::GetProperty(const char *name)
{
	return this->GetClass()->GetProperty(name);
}

IMonoEvent *MonoHandle::GetEvent(const char *name)
{
	return this->GetClass()->GetEvent(name);
}
//! Gets the wrapper for the class of this object.
IMonoClass *MonoHandle::GetClass()
{
	if (!this->type)
	{
		// Cache the type of this object, so we don't have to get it over and over again.
		this->type = MonoClassCache::Wrap(this->getMonoClass());
	}
	return this->type;
}

void *MonoHandle::UnboxObject()
{
	return mono_object_unbox((MonoObject *)this->Get());
}

void *MonoHandle::GetWrappedPointer()
{
	return this->Get();
}
