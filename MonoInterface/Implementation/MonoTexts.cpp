#include "stdafx.h"

#include "MonoTexts.h"

#if 0
#define TextsMessage CryLogAlways
#else
#define TextsMessage(...) void(0)
#endif

mono::string MonoTexts::ToManaged(const char *text)
{
	return mono::string(mono_string_new(mono_domain_get(), text));
}

mono::string MonoTexts::ToManaged(const wchar_t *text)
{
	return mono::string(mono_string_new_utf16(mono_domain_get(),
		reinterpret_cast<const mono_unichar2 *>(text), wcslen(text)));
}

const char *MonoTexts::ToNative(mono::string text)
{
	if (!text)
	{
		return nullptr;
	}

	TextsMessage("Getting the unmanaged version of text.");

	MonoError er;
	char *t = mono_string_to_utf8_checked(reinterpret_cast<MonoString *>(text), &er);
	if (!mono_error_ok(&er))
	{
		gEnv->pLog->LogError("%s", mono_error_get_message(&er));
		return nullptr;
	}

	TextsMessage("Got the unmanaged version of text.");

	Text *tex = new Text(t);
	mono_free(t);
	t = const_cast<char *>(tex->ToNTString());
	delete tex;
	return t;
}

const wchar_t *MonoTexts::ToNative16(mono::string text)
{
	if (!text)
	{
		return nullptr;
	}

	wchar_t *chars = reinterpret_cast<wchar_t *>(mono_string_to_utf16(reinterpret_cast<MonoString *>(text)));

	int length = wcslen(chars);

	wchar_t *t = new wchar_t[length + 1];

	for (int i = 0; i < length; i++)
	{
		t[i] = chars[i];
	}
	t[length] = '\0';

	mono_free(chars);

	return t;
}
