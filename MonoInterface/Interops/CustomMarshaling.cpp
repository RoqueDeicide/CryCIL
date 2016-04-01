#include "stdafx.h"

#include "CustomMarshaling.h"

void CustomMarshalingInterop::InitializeInterops()
{
	REGISTER_METHOD(GetUtf8String);
}

mono::string CustomMarshalingInterop::GetUtf8String(const char *stringHandle)
{
	return ToMonoString(stringHandle);
}
