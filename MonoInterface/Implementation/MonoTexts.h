#pragma once

#include "IMonoInterface.h"

struct MonoTexts : public IMonoTexts
{
	mono::string ToManaged(const char *text) override;
	mono::string ToManaged(const wchar_t *text) override;

	const char    *ToNative(mono::string text) override;
	const wchar_t *ToNative16(mono::string text) override;
};