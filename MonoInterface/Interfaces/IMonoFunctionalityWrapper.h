#pragma once

#include "IMonoAliases.h"

//! Base interface for objects that wrap Mono functionality.
struct IMonoFunctionalityWrapper
{
	virtual ~IMonoFunctionalityWrapper() {}

	//! Returns pointer to Mono object that is wrapped by this object.
	VIRTUAL_API virtual void *GetWrappedPointer() = 0;

	template<typename T> T *GetHandle()
	{
		return reinterpret_cast<T *>(this->GetWrappedPointer());
	}
};