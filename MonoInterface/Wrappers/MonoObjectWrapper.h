#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

#include "Wrappers/MonoClassWrapper.h"

#include <ISystem.h>

//! Extra base class for mono handles.
struct MonoObjectWrapper : public IMonoHandle
{
	//! Calls a Mono method associated with this object.
	//!
	//! @remark Make sure that each overload of the method you are
	//!         calling has unique number of parameters.
	//!
	//! @param name Name of the method to invoke.
	//! @param args Array of arguments to pass, that also defines which method overload to use.
	virtual mono::object CallMethod(const char *name, IMonoArray *args)
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
	virtual mono::object GetField(const char *name)
	{
		return this->GetClass()->GetField(this->Get(), name);
	}
	//! Sets the value of the object's field.
	//!
	//! @param name  Name of the field which value to set.
	//! @param value New value to assign to the field.
	virtual void SetField(const char *name, mono::object value)
	{
		this->GetClass()->SetField(this->Get(), name, value);
	}
	//! Gets the value of the object's property.
	//!
	//! @param name Name of the property which value to get.
	virtual mono::object GetProperty(const char *name)
	{
		return this->GetClass()->GetProperty(this->Get(), name);
	}
	//! Sets the value of the object's property.
	//!
	//! @param name  Name of the property which value to set.
	//! @param value New value to assign to the property.
	virtual void SetProperty(const char *name, mono::object value)
	{
		this->GetClass()->SetProperty(this->Get(), name, value);
	}
	//! Gets the wrapper for the class of this object.
	virtual struct IMonoClass * GetClass()
	{
		if (!this->type)
		{
			// Cache the type of this object, so we don't have to get it over and over again.
			this->type = MonoClassCache::Wrap(this->getMonoClass());
		}
		return this->type;
	}

	virtual void * UnboxObject()
	{
		return mono_object_unbox((MonoObject *)this->Get());
	}

	virtual void * GetWrappedPointer()
	{
		return this->Get();
	}
protected:
	IMonoClass *type = nullptr;
private:
	MonoClass *monoClass = nullptr;
	MonoClass *getMonoClass()
	{
		if (!this->monoClass)
		{
			this->monoClass = mono_object_get_class((MonoObject *)this->Get());
		}
		return this->monoClass;
	}
};