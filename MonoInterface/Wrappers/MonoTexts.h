#pragma once

#include "IMonoInterface.h"

struct MonoTexts : public IMonoTexts
{
	virtual mono::string ToManaged(const char *text);

	virtual const char *ToNative(mono::string text);
};