#pragma once

#include "IMonoAliases.h"

//! Encapsulates virtual API for all Mono objects.
//!
//! When using these wrappers bear in mind that constructing a new one is somewhat expensive, since one of
//! the actions taken during construction process is looking up the correct class of the object.
//!
//! For the sake of optimization there is an assignment operator that allows user to encapsulate a new
//! pointer to the managed object that will not update the class that will represent that object. When the
//! new object is of the same class, it's fine, but you have to make the wrapper update the class
struct IMonoObject
{
private:
	mono::object obj;			//!< Wrapped pointer to the Mono object.
	IMonoClass *klass;			//!< Wrapper for a type of the wrapped Mono object.
public:
	IMonoObject(mono::object obj)
	{
		this->obj = obj;
		this->klass = (obj) ? MonoEnv->Objects->GetObjectClass(obj) : nullptr;
	}
	IMonoObject(const MonoGCHandle &handle)
	{
		this->obj = handle.Object;
		this->klass = (obj) ? MonoEnv->Objects->GetObjectClass(obj) : nullptr;
	}
	//! Invalidates this wrapper so, when it's assigned a new mono::object, it will update itself completely.
	//!
	//! Use this call when you want to assign this object a mono::object that is represented by a different
	//! class.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//!
	//! // Let's say we have two objects: one is String and another is StringBuilder.
	//! mono::string str;
	//! mono::object strBuilder;
	//! // Let's say, they are initialized.
	//! Init(str, strBuilder);
	//!
	//! // Lets wrap the normal string.
	//! auto wrapper = IMonoObject(str);
	//! // Now our wrapper will always treat its object as an object of type System.String.
	//! // Even, if we assign it our string builder as an object:
	//! wrapper = strBuilder;       // Wrong type!
	//!
	//! // To make wrapper update the class of the wrapped object, either call Invalidate() prior to
	//! // assignment or call Update() after it:
	//!
	//! if (firstOption)
	//! {
	//!     wrapper.Invalidate() = strBuilder;
	//! }
	//! else
	//! {
	//!     (wrapper = strBuilder).Update();
	//! }
	//!
	//! // Now our wrapper is valid again!
	//!
	//! @endcode
	IMonoObject &Invalidate()
	{
		this->klass = nullptr;
		return *this;
	}
	IMonoObject &operator=(mono::object obj)
	{
		this->obj = obj;
		if (!this->klass)
		{
			this->klass = (obj) ? MonoEnv->Objects->GetObjectClass(obj) : nullptr;
		}
	}
};