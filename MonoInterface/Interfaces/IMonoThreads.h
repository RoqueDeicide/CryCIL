#pragma once

#include "IMonoAliases.h"

//! Provides functionality for interaction with Mono threads.
struct IMonoThreads
{
	virtual ~IMonoThreads() {}

	//! Attaches calling thread to Mono run-time if not attached, otherwise returns a Mono object
	//! for it.
	//!
	//! If this method is called from a thread that wasn't created by Mono run-time and wasn't
	//! attached before, then run-time will create a special object that will allow that thread
	//! to interact with the Mono run-time. All threads created in Mono are already attached.
	//!
	//! @returns MonoThread object that represents calling thread.
	VIRTUAL_API virtual mono::Thread Attach() = 0;

	//! Creates a new thread that executes given parameterless method when started.
	//!
	//! @param method A delegate of type System.Threading.ThreadStart that represents a method that
	//!               will be invoked and executed in a new thread when the latter is started.
	VIRTUAL_API virtual mono::Thread Create(mono::delegat method) = 0;
	//! Creates a new thread that executes given method that accepts one argument when started.
	//!
	//! @param method A delegate of type System.Threading.ParameterizedThreadStart that represents
	//!               a method that will be invoked and executed in a new thread when the latter is
	//!               started.
	VIRTUAL_API virtual mono::Thread CreateParametrized(mono::delegat method) = 0;

	//! Puts calling thread into sleep for a specified amount of time.
	//!
	//! @param timeSpan Duration of sleep in milliseconds.
	VIRTUAL_API virtual void Sleep(int timeSpan) = 0;
};