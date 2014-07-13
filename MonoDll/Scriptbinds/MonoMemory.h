#pragma once

#include <IMonoScriptBind.h>
#include <MonoCommon.h>
#include <mono/metadata/object.h>
#include <mono/utils/mono-publib.h>

class Scriptbind_MonoMemory : public IMonoScriptBind
{
public:
	Scriptbind_MonoMemory();
	~Scriptbind_MonoMemory();

	virtual const char *GetNamespace() { return "CryEngine.NativeMemory"; }
	virtual const char *GetClassName() { return "MonoMemory"; }

	static void FreeMonoMemory(char *value);
};
