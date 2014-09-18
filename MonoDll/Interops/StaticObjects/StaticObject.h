#pragma once

#include <IMonoInterop.h>
#include <IEntity.h>
#include <IStatObj.h>
#include "3DEngine.h"
#include "IIndexedMesh.h"

struct MeshHandles
{
	IIndexedMesh *indexedMesh;
	CMesh *mesh;
};

class StaticObjectInterop : public IMonoInterop
{
public:
	StaticObjectInterop();
	~StaticObjectInterop();

	virtual const char *GetClassName() { return "StaticObjectInterop"; }

	static IStatObj *CreateStaticObject();
	static void ReleaseStaticObject(IStatObj *obj);
	static MeshHandles GetMeshHandles(IStatObj *obj);
};