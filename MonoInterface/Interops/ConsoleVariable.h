#pragma once

#include "IMonoInterface.h"

struct ConsoleVariableInterop : public IMonoInterop<true, true>
{


	virtual const char *GetName() { return "ConsoleVariable"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized();

	static void         Release        (mono::object handle);
	static void         ClearFlags     (mono::object handle, int flags);
	static int          GetFlags       (mono::object handle);
	static int          SetFlags       (mono::object handle, int flags);
	static int          GetInt         (mono::object handle);
	static float        GetFloat       (mono::object handle);
	static mono::string GetString      (mono::object handle);
	static void         SetString      (mono::object handle, mono::string s);
	static void         SetFloat       (mono::object handle, float f);
	static void         SetInt         (mono::object handle, int i);
	static int          GetVariableType(mono::object handle);
	static mono::string GetNameVar     (mono::object handle);
	static mono::string GetHelp        (mono::object handle);
};