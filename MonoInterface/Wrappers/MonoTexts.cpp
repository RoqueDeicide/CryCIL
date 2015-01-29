#include "stdafx.h"

#include "MonoTexts.h"

mono::string MonoTexts::ToManaged(const char *text)
{
	return (mono::string)mono_string_new(mono_domain_get(), text);
}

const char *MonoTexts::ToNative(mono::string text)
{
	const char *monoNativeText = mono_string_to_utf8((MonoString *)text);

	ConstructiveText nativeText(monoNativeText);

	mono_free((void *)monoNativeText);

	return nativeText.ToNTString();
}