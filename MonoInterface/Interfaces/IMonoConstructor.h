#pragma once

#include "IMonoAliases.h"

//! Represents a constructor function in Mono.
//!
//! Constructors are always instance methods and when invoked using Invoke() with null passed as an
//! instance, they create a new object and initialize it. You cannot polymorph a constructor.
struct IMonoConstructor : public IMonoMethod
{

};