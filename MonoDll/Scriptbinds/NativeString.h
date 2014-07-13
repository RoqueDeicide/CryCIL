#pragma once

#include <IMonoScriptBind.h>
#include <MonoCommon.h>
#include <mono/metadata/object.h>
#include <mono/utils/mono-publib.h>

class Scriptbind_NativeString : public IMonoScriptBind
{
public:
	Scriptbind_NativeString();
	~Scriptbind_NativeString();

	virtual const char *GetNamespace() { return "CryEngine.NativeMemory"; }
	virtual const char *GetClassName() { return "NativeString"; }

	static mono::string GetMonoString(const char *value);
	static mono::string GetMonoWideString(const wchar_t *value);
	static const char * GetNativeString(mono::string value);
	static const wchar_t * GetNativeWideString(mono::string value);
};
