#pragma once

#include "IMonoInterface.h"

struct ConsoleVariableInterop : public IMonoInterop<true, true>
{


	virtual const char *GetName() override { return "ConsoleVariable"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized() override;

	static void         Release        (ICVar **handle);
	static void         ClearFlags     (ICVar **handle, int flags);
	static int          GetFlags       (ICVar **handle);
	static int          SetFlags       (ICVar **handle, int flags);
	static int          GetInt         (ICVar **handle);
	static float        GetFloat       (ICVar **handle);
	static mono::string GetString      (ICVar **handle);
	static void         SetString      (ICVar **handle, mono::string s);
	static void         SetFloat       (ICVar **handle, float f);
	static void         SetInt         (ICVar **handle, int i);
	static int          GetVariableType(ICVar **handle);
	static mono::string GetNameVar     (ICVar **handle);
	static mono::string GetHelp        (ICVar **handle);
};