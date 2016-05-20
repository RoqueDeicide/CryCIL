#include "stdafx.h"

#include "MeshObject.h"

void MeshObjectInterop::InitializeInterops()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(GetRefCount);
	REGISTER_METHOD(GetAABB);
	REGISTER_METHOD(GetRandomPos);
	REGISTER_METHOD(GetMaterial);
	REGISTER_METHOD(GetRenderMesh);
	REGISTER_METHOD(GetPhysEntity);
	REGISTER_METHOD(GetPhysGeom);

	REGISTER_METHOD(GetStaticObject);
	REGISTER_METHOD(GetCharacter);
}

int MeshObjectInterop::AddRef(IMeshObj *handle)
{
	return handle->AddRef();
}

int MeshObjectInterop::Release(IMeshObj *handle)
{
	return handle->Release();
}

int MeshObjectInterop::GetRefCount(IMeshObj *handle)
{
	return handle->GetRefCount();
}

AABB MeshObjectInterop::GetAABB(IMeshObj *handle)
{
	return handle->GetAABB();
}

void MeshObjectInterop::GetRandomPos(IMeshObj *handle, PosNorm &ran, CRndGen &seed, EGeomForm eForm)
{
	handle->GetExtent(eForm);
	handle->GetRandomPos(ran, seed, eForm);
}

IMaterial *MeshObjectInterop::GetMaterial(IMeshObj *handle)
{
	return handle->GetMaterial();
}

IRenderMesh *MeshObjectInterop::GetRenderMesh(IMeshObj *handle)
{
	return handle->GetRenderMesh();
}

IPhysicalEntity *MeshObjectInterop::GetPhysEntity(IMeshObj *handle)
{
	return handle->GetPhysEntity();
}

phys_geometry *MeshObjectInterop::GetPhysGeom(IMeshObj *handle)
{
	return handle->GetPhysGeom();
}

IStatObj *MeshObjectInterop::GetStaticObject(IMeshObj *handle)
{
	return InterfaceCastSemantics::cryinterface_cast<IStatObj>(handle);
}

ICharacterInstance *MeshObjectInterop::GetCharacter(IMeshObj *handle)
{
	return InterfaceCastSemantics::cryinterface_cast<ICharacterInstance>(handle);
}
