#pragma once

#include "IMonoAliases.h"

//! Wraps an object that represents a Mono delegate.
struct IMonoDelegate : public IMonoHandle
{
	VIRTUAL_API virtual IMonoMethod *GetMethod() = 0;
	VIRTUAL_API virtual mono::object GetTarget() = 0;
	VIRTUAL_API virtual void *GetFunctionPointer() = 0;
};