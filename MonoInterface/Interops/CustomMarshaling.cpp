#include "stdafx.h"

#include "CustomMarshaling.h"

void CustomMarshalingInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetUtf8String);
}

mono::string CustomMarshalingInterop::GetUtf8String(const char *stringHandle)
{
	return ToMonoString(stringHandle);
}
