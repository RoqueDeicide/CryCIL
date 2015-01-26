#include "stdafx.h"
#include "CryMarshal.h"

void CryMarshalInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(AllocateMemory);
	REGISTER_METHOD(ReallocateMemory);
	REGISTER_METHOD(FreeMemory);
}

void *CryMarshalInterop::AllocateMemory(unsigned __int64 size)
{
#ifdef WIN64
	return malloc(size);
#else
	return malloc((unsigned int)size);
#endif
}

void *CryMarshalInterop::ReallocateMemory(void *ptr, unsigned __int64 sizeNew)
{
#ifdef WIN64
	return realloc(ptr, sizeNew);
#else
	return realloc(ptr, (unsigned int)sizeNew);
#endif
}


void CryMarshalInterop::FreeMemory(void *pointer)
{
	free(pointer);
}