#include "stdafx.h"
#include "NativeString.h"

Scriptbind_NativeString::Scriptbind_NativeString()
{
	REGISTER_METHOD(GetMonoString);
	REGISTER_METHOD(GetNativeString);
	REGISTER_METHOD(GetMonoWideString);
	REGISTER_METHOD(GetNativeWideString);
}

Scriptbind_NativeString::~Scriptbind_NativeString()
{}

mono::string Scriptbind_NativeString::GetMonoString(const char *value)
{
	return (mono::string)mono_string_new((MonoDomain *)GetMonoScriptSystem()->GetActiveDomain(), value);
}

mono::string Scriptbind_NativeString::GetMonoWideString(const wchar_t *value)
{
	return (mono::string)mono_string_from_utf16((mono_unichar2 *)value);
}

const char * Scriptbind_NativeString::GetNativeString(mono::string value)
{
	return mono_string_to_utf8((MonoString *)value);
}

const wchar_t * Scriptbind_NativeString::GetNativeWideString(mono::string value)
{
	return (wchar_t *)mono_string_to_utf16((MonoString *)value);
}