#pragma once
#include <IMonoInterface.h>

struct CryMarshalInterop : public IDefaultMonoInterop<true>
{
	virtual const char *GetName() { return "CryMarshal"; };

	virtual void OnRunTimeInitialized();
	// Allocate an array of bytes of given length. Returns a pointer to the first byte.
	// Allocated memory should not be referenced from unmanaged code, unless you have access
	// to instance of INativeMemoryWrapper type that works with this memory and
	// that object has not disposed of the memory cluster.
	static void *AllocateMemory(unsigned __int64 size);
	// Reallocates the memory.
	static void *ReallocateMemory(void *ptr, unsigned __int64 sizeNew);
	// Frees memory that has been allocated.
	static void FreeMemory(void * pointer);
};
