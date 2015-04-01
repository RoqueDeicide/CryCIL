#pragma once

#include "IMonoAliases.h"

//! Encapsulates virtual API for all Mono objects.
//!
//! There are two ways of encapsulating Mono objects when using this type: construction and assignment:
//!     1) Construction creates a fully fleshed out, valid (unless provided pointer is null) wrapper.
//!     2) Assignment only updates the internal object pointer, but doesn't update internal class data,
//!        which is OK when a new object is represented by the same class, but if it's different, then
//!        the wrapper will cease to be valid.
struct IMonoObject
{
protected:
	mono::object obj;			//!< Wrapped pointer to the Mono object.
	IMonoClass *klass;			//!< Wrapper for a type of the wrapped Mono object.
public:
	IMonoObject()
		: obj(nullptr)
		, klass(nullptr)
	{

	}
	//! Constructs a new wrapper for given Mono object.
	//!
	//! Resultant wrapper is valid as long as provided pointer is valid.
	//!
	//! @param obj A pointer to the Mono object that needs encapsulation.
	IMonoObject(mono::object obj)
	{
		this->obj = obj;
		this->klass = (obj) ? MonoEnv->Objects->GetObjectClass(obj) : nullptr;
	}
	IMonoObject(mono::object obj, IMonoClass *klass)
	{
		this->obj = obj;
		this->klass = klass;
	}
	//! Constructs a new wrapper for the Mono object represented by a GC handle.
	//!
	//! Resultant wrapper is valid as long as provided handle is valid.
	//!
	//! @param handle A GC handle that represents the Mono object that needs encapsulation.
	IMonoObject(const MonoGCHandle &handle)
	{
		this->obj = handle.Object;
		this->klass = (obj) ? MonoEnv->Objects->GetObjectClass(obj) : nullptr;
	}
	//! Updates encapsulated pointer to Mono object without updating its class.
	IMonoObject &operator=(mono::object obj)
	{
		this->obj = obj;
		return *this;
	}
	//! Updates encapsulated pointer to Mono object without updating its class.
	IMonoObject &operator=(const MonoGCHandle &handle)
	{
		this->obj = handle.Object;
		return *this;
	}
	//! Implicitly provides encapsulated object.
	operator mono::object() const
	{
		return this->obj;
	}
	//! Provides class object that is assumed to represent the object that is encapsulated by this wrapper.
	__declspec(property(get = GetClass)) IMonoClass *Class;
	IMonoClass *GetClass() const
	{
		return this->klass;
	}
	//! Gets the value of the field. Only use it with objects of managed types.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//! // Let's get the value of integer field. obj is our IMonoObject instance.
	//!
	//! // We need to declare a variable that will store the value of the field.
	//! int fieldValue;		// No need to assign it.
	//!
	//! // Now we can get the value, just provide the address of the variable.
	//! obj.GetField("IntegerField", &fieldValue);
	//!
	//! // Now fieldValue variable contains the value of that field.
	//! //--------------------------------------------------------------------------
	//! // Let's do the same with a field of managed type.
	//!
	//! // Same deal, but we always use one of the mono::object aliases.
	//! mono::string text;
	//! obj.GetField("TextField", &text);
	//! @endcode
	//!
	//! @param name  Name of the field.
	//! @param value Pointer to the memory that will store the value of the field after this
	//!              function completes execution.
	void GetField(const char *name, void *value)
	{
		if (this->klass && this->obj)
		{
			this->klass->GetField(this->obj, name, value);
		}
	}
	//! Sets the value of the field. Only use it with objects of managed types.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//! // Let's set the value of integer field. obj is our IMonoObject instance.
	//!
	//! // We need to declare a variable that stores the new value for the field.
	//! int fieldValue = 10;
	//!
	//! // Now we can set the value, just provide the address of the variable.
	//! obj.SetField("IntegerField", &fieldValue);
	//!
	//! // Now IntegerField has a value of 10.
	//! //--------------------------------------------------------------------------
	//! // Let's do the same with a field of managed type.
	//!
	//! // Same deal, but we always use one of the mono::object aliases.
	//! mono::string text = ToMonoString("Some new text");
	//! obj.SetField("TextField", &text);
	//! @endcode
	//!
	//! @param name  Name of the field.
	//! @param value Pointer to the memory that stores the value that should be assigned to the field.
	void SetField(const char *name, void *value)
	{
		if (this->klass && this->obj)
		{
			this->klass->SetField(this->obj, name, value);
		}
	}
	//! Gets one of the properties of this object.
	//!
	//! @param name Name of the property to get.
	IMonoProperty *GetProperty(const char *name)
	{
		if (this->klass)
		{
			return this->klass->GetProperty(name);
		}
		return nullptr;
	}
	//! Gets one of the events of this object.
	//!
	//! @param name Name of the event to get.
	IMonoEvent *GetEvent(const char *name)
	{
		if (this->klass)
		{
			return this->klass->GetEvent(name);
		}
		return nullptr;
	}
};