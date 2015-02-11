#pragma once

#include "IMonoAliases.h"

//! Base interface for objects that wrap Mono functionality.
struct IMonoFunctionalityWrapper
{
	virtual ~IMonoFunctionalityWrapper() {}

	//! Returns pointer to Mono object this wrapper uses.
	VIRTUAL_API virtual void *GetWrappedPointer() = 0;

	template<typename T> T *GetHandle()
	{
		return (T *)this->GetWrappedPointer();
	}
};