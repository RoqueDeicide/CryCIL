#pragma once

#include "IMonoInterface.h"

//! Represents a wrapper for MonoObject that doesn't pin the object in the heap.
//!
//! Never use objects of this type for longer then execution of one short method,
//! because there is a high chance that the object will be either moved or
//! destroyed on the next GC session.
struct MonoHandleFree : public IMonoHandle
{
private:
	mono::object handle;
public:
	MonoHandleFree(mono::object obj)
	{

	}
	//! Encapsulates a different managed object into this wrapper.
	virtual void Hold(mono::object object) override
	{
		throw std::logic_error("The method or operation is not implemented.");
	}

	virtual void Release() override
	{
		throw std::logic_error("The method or operation is not implemented.");
	}

	virtual mono::object Get() override
	{
		throw std::logic_error("The method or operation is not implemented.");
	}

	virtual void * GetWrappedPointer() const override
	{
		throw std::logic_error("The method or operation is not implemented.");
	}

};