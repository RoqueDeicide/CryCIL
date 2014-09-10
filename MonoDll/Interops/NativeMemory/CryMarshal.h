#pragma once
#include <IMonoScriptBind.h>

class Scriptbind_CryMarshal : public IMonoScriptBind
{
public:
	Scriptbind_CryMarshal();
	~Scriptbind_CryMarshal();

	virtual const char *GetClassName() { return "MemoryHandlingInterop"; };

	// Allocate an array of bytes of given length. Returns a pointer to the first byte.
	// Allocated memory should not be referenced from unmanaged code, unless you have access
	// to instance of INativeMemoryWrapper type that works with this memory and
	// that object has not disposed of the memory cluster.
	static void *AllocateMemory(unsigned __int64 size);
	// Frees memory that has been allocated.
	static void FreeMemory(void * pointer);
};
