#pragma once

#include <IMonoScriptBind.h>
#include <IEntity.h>
#include <IStatObj.h>
#include "3DEngine.h"
#include "IIndexedMesh.h"

struct MeshHandles
{
	IIndexedMesh *indexedMesh;
	CMesh *mesh;
};

class Scriptbind_StaticObject : public IMonoScriptBind
{
public:
	Scriptbind_StaticObject();
	~Scriptbind_StaticObject();

	virtual const char *GetNamespace() { return "CryEngine.StaticObjects.Native"; }
	virtual const char *GetClassName() { return "NativeStaticObjectMethods"; }

	static IStatObj *CreateStaticObject();
	static void ReleaseStaticObject(IStatObj *obj);
	static MeshHandles GetMeshHandles(IStatObj *obj);
};