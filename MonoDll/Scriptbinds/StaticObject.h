#pragma once

#include <IMonoScriptBind.h>
#include <IEntity.h>
#include <IStatObj.h>

class Scriptbind_StaticObject : public IMonoScriptBind
{
public:
	Scriptbind_StaticObject();
	~Scriptbind_StaticObject();

	virtual const char *GetNamespace() { return "CryEngine.StaticObjects.Native"; }
	virtual const char *GetClassName() { return "NativeStaticObjectMethods"; }

	static IStatObj *CreateStaticObject();
	static void ReleaseStaticObject(IStatObj *obj);
};