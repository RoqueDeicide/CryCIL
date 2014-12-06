#include "stdafx.h"
#include "CryMarshal.h"

CryMarshalInterop::CryMarshalInterop()
{
	REGISTER_METHOD(AllocateMemory);
	REGISTER_METHOD(FreeMemory);
}
CryMarshalInterop::~CryMarshalInterop()
{}

void * CryMarshalInterop::AllocateMemory(unsigned __int64 size)
{
#ifdef WIN64
	return malloc(size);
#else
	return malloc((unsigned int)size);
#endif
}

void CryMarshalInterop::FreeMemory(void * pointer)
{
	free(pointer);
}
