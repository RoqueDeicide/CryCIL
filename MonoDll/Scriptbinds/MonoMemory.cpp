#include "stdafx.h"
#include "MonoMemory.h"

Scriptbind_MonoMemory::Scriptbind_MonoMemory()
{
	REGISTER_METHOD(FreeMonoMemory);
}

Scriptbind_MonoMemory::~Scriptbind_MonoMemory()
{}

void Scriptbind_MonoMemory::FreeMonoMemory(char *value)
{
	mono_free(value);
}