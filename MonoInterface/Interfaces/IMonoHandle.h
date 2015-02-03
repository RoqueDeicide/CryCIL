#pragma once

#include "IMonoAliases.h"

//! Base type of objects that wrap MonoObject instances granting access to Mono API.
struct IMonoHandle : public IMonoFunctionalityWrapper
{
	//! Returns an instance of MonoObject this object is wrapped around.
	VIRTUAL_API virtual mono::object Get() = 0;
	//! Gets the value of the field. Only use it with objects of managed types.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//! // Let's get the value of integer field. obj is our IMonoHandle instance.
	//!
	//! // We need to declare a variable that will store the value of the field.
	//! int fieldValue;		// No need to assign it.
	//!
	//! // Now we can get the value, just provide the address of the variable.
	//! obj->GetField("IntegerField", &fieldValue);
	//!
	//! // Now fieldValue variable contains the value of that field.
	//! //--------------------------------------------------------------------------
	//! // Let's do the same with a field of managed type.
	//!
	//! // Same deal, but we always use one of the mono::object aliases.
	//! mono::string text;
	//! obj->GetField("TextField", &text);
	//! @endcode
	//!
	//! @param name  Name of the field.
	//! @param value Pointer to the memory that will store the value of the field after this
	//!              function completes execution.
	VIRTUAL_API virtual void GetField(const char *name, void *value) = 0;
	//! Sets the value of the field. Only use it with objects of managed types.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//! // Let's set the value of integer field. obj is our IMonoHandle instance.
	//!
	//! // We need to declare a variable that stores the new value for the field.
	//! int fieldValue = 10;
	//!
	//! // Now we can set the value, just provide the address of the variable.
	//! obj->SetField("IntegerField", &fieldValue);
	//!
	//! // Now IntegerField has a value of 10.
	//! //--------------------------------------------------------------------------
	//! // Let's do the same with a field of managed type.
	//!
	//! // Same deal, but we always use one of the mono::object aliases.
	//! mono::string text = ToMonoString("Some new text");
	//! obj->SetField("TextField", &text);
	//! @endcode
	//!
	//! @param name  Name of the field.
	//! @param value Pointer to the memory that stores the value that should be assigned to the field.
	VIRTUAL_API virtual void SetField(const char *name, void *value) = 0;
	//! Gets one of the properties of this object.
	//!
	//! Delete returned object once you don't need it anymore.
	//!
	//! @param name Name of the property to get.
	VIRTUAL_API virtual IMonoProperty *GetProperty(const char *name) = 0;
	//! Gets one of the events of this object.
	//!
	//! Delete returned object once you don't need it anymore.
	//!
	//! @param name Name of the event to get.
	VIRTUAL_API virtual IMonoEvent *GetEvent(const char *name) = 0;
	//! Unboxes value of this object. Don't use with non-value types.
	//!
	//! @tparam T Type of the value to unbox. bool, for instance if
	//!           managed object is of type System.Boolean.
	//!
	//! @returns Verbatim copy of managed memory block held by the object.
	template<class T> T Unbox()
	{
		return *(T *)this->UnboxObject();
	}
	//! Gets managed type that represents wrapped object.
	VIRTUAL_API virtual IMonoClass *GetClass() = 0;

protected:
	VIRTUAL_API virtual void *UnboxObject() = 0;
};