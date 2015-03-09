#pragma once

#include "IMonoInterface.h"

struct MonoTexts : public IMonoTexts
{
	virtual mono::string ToManaged(const char *text);
	virtual mono::string ToManaged(const wchar_t *text);

	virtual const char *ToNative(mono::string text);
	virtual const wchar_t *ToNative16(mono::string text);
};