#pragma once

#include "IMonoInterface.h"
#include "Wrappers/MonoObjectWrapper.h"

//! Represents a wrapper for MonoObject that doesn't pin the object in the heap.
//!
//! Never use objects of this type for longer then execution of one short method,
//! because there is a high chance that the object will be either moved or
//! destroyed on the next GC session.
struct MonoHandleFree : public MonoObjectWrapper
{
private:
	mono::object handle;
public:
	MonoHandleFree(mono::object obj)
	{
		this->handle = obj;
	}
	//! Encapsulates a different managed object into this wrapper.
	virtual void Hold(mono::object object) override
	{
		this->handle = object;
	}

	virtual void Release() override
	{
		this->handle = nullptr;
	}

	virtual mono::object Get() override
	{
		return this->handle;
	}

	virtual void * GetWrappedPointer() override
	{
		return this->handle;
	}

};