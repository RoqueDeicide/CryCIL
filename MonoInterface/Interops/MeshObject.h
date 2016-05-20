#pragma once

#include "IMonoInterface.h"

struct MeshObjectInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "MeshObject"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models"; }

	virtual void InitializeInterops() override;

	static int AddRef(IMeshObj *handle);
	static int Release(IMeshObj *handle);
	static int GetRefCount(IMeshObj *handle);
	static AABB GetAABB(IMeshObj *handle);
	static void GetRandomPos(IMeshObj *handle, PosNorm &ran, CRndGen &seed, EGeomForm eForm);
	static IMaterial *GetMaterial(IMeshObj *handle);
	static IRenderMesh *GetRenderMesh(IMeshObj *handle);
	static IPhysicalEntity *GetPhysEntity(IMeshObj *handle);
	static phys_geometry *GetPhysGeom(IMeshObj *handle);

	static IStatObj *GetStaticObject(IMeshObj *handle);
	static ICharacterInstance *GetCharacter(IMeshObj *handle);
};
