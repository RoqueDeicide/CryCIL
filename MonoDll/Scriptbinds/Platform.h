#pragma once
#include "IMonoScriptBind.h"
class Scriptbind_Platform :
	public IMonoScriptBind
{
public:
	Scriptbind_Platform();
	~Scriptbind_Platform();

	virtual const char *GetClassName() { return "PlatformMethods"; }

	static bool AreMeshIndicesInt16();
	static bool AreMeshTangentsSingle();
};
