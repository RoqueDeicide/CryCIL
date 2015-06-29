#pragma once

#include "IMonoInterface.h"

struct MonoTexts : public IMonoTexts
{
	virtual mono::string ToManaged(const char *text) override;
	virtual mono::string ToManaged(const wchar_t *text) override;

	virtual const char *ToNative(mono::string text) override;
	virtual const wchar_t *ToNative16(mono::string text) override;
};