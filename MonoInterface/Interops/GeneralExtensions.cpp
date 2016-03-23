#include "stdafx.h"

#include "GeneralExtensions.h"

void GeneralExtensionsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD_NCN(this->GetInteropNameSpace(), "StringExtensions", "CopyToBuffer", CopyToBuffer);
}

void GeneralExtensionsInterop::CopyToBuffer(mono::string str, wchar_t *chars, int start, int count)
{
	if (!str)
	{
		return;
	}

	if (!chars)
	{
		ArgumentNullException("The pointer to the buffer cannot be null.").Throw();
	}
	if (start < 0)
	{
		IndexOutOfRangeException("Index of the first element in the buffer cannot be less then 0.").Throw();
	}

	IMonoText t(str);
	MonoGCHandle handle = MonoEnv->GC->Pin(str);

	if (t.Length < count)
	{
		handle.Release();
		ArgumentException("text", "Input string doesn't contain enough characters.").Throw();
	}

	for (int i = 0; i < count; i++)
	{
		chars[start + i] = t[i];
	}
}
