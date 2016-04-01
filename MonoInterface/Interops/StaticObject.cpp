#include "stdafx.h"

#include "StaticObject.h"
#include "PhysicsGeometryStructs.h"

void StaticObjectInterop::InitializeInterops()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(GetIdMatBreakable);
	REGISTER_METHOD(GetRenderMesh);
	REGISTER_METHOD(GetIndexedMeshInternal);
	REGISTER_METHOD(GetPhysGeom);
	REGISTER_METHOD(UpdateVerticesInternal);
	REGISTER_METHOD(SetPhysGeom);
	REGISTER_METHOD(GetTetrLattice);
	REGISTER_METHOD(SetMaterial);
	REGISTER_METHOD(GetMaterial);
	REGISTER_METHOD(GetBox);
	REGISTER_METHOD(SetBBox);
	REGISTER_METHOD(GetRadius);
	REGISTER_METHOD(RefreshInternal);
	REGISTER_METHOD(GetRandomPos);
	REGISTER_METHOD(GetLodObjectInternal);
	REGISTER_METHOD(GetLowestLodInternal);
	REGISTER_METHOD(FindNearesLoadedLodInternal);
	REGISTER_METHOD(FindHighestLodInternal);
	REGISTER_METHOD(GetFilePath);
	REGISTER_METHOD(SetFilePath);
	REGISTER_METHOD(GetGeoName);
	REGISTER_METHOD(SetGeoName);
	REGISTER_METHOD(GetHelperPos);
	REGISTER_METHOD(GetHelperTM);
	REGISTER_METHOD(IsDefaultObject);
	REGISTER_METHOD(FreeIndexedMesh);
	REGISTER_METHOD(IsPhysicsExist);
	REGISTER_METHOD(InvalidateInternal);
	REGISTER_METHOD(GetSubObjectCount);
	REGISTER_METHOD(SetSubObjectCount);
	REGISTER_METHOD(GetSubObject);
	REGISTER_METHOD(IsSubObjectInternal);
	REGISTER_METHOD(GetParentObject);
	REGISTER_METHOD(GetCloneSourceObject);
	REGISTER_METHOD(FindSubObject);
	REGISTER_METHOD(FindSubObject_CGA);
	REGISTER_METHOD(FindSubObject_StrStr);
	REGISTER_METHOD(RemoveSubObject);
	REGISTER_METHOD(CopySubObject);
	REGISTER_METHOD(AddSubObject);
	REGISTER_METHOD(PhysicalizeSubobjects);
	REGISTER_METHOD(PhysicalizeInternal);
	REGISTER_METHOD(IsDeformableInternal);
	REGISTER_METHOD(SaveToCGF);
	REGISTER_METHOD(CloneInternal);
	REGISTER_METHOD(SetDeformationMorphTargetInternal);
	REGISTER_METHOD(DeformMorph);
	REGISTER_METHOD(HideFoliage);
	REGISTER_METHOD(Serialize);
	REGISTER_METHOD(GetProperties);
	REGISTER_METHOD(GetPhysicalPropertiesInternal);
	REGISTER_METHOD(GetLastBooleanOp);
	REGISTER_METHOD(GetStatistics);
	REGISTER_METHOD(CreateStatObjOptionalIndexedMesh);
	REGISTER_METHOD(UpdateDeformableStatObj);
}

int StaticObjectInterop::AddRef(IStatObj *handle)
{
	return handle->AddRef();
}

int StaticObjectInterop::Release(IStatObj *handle)
{
	return handle->Release();
}

void StaticObjectInterop::SetFlags(IStatObj *handle, int nFlags)
{
	handle->SetFlags(nFlags);
}

int StaticObjectInterop::GetFlags(IStatObj *handle)
{
	return handle->GetFlags();
}

int StaticObjectInterop::GetIdMatBreakable(IStatObj *handle)
{
	return handle->GetIDMatBreakable();
}

IIndexedMesh *StaticObjectInterop::GetIndexedMeshInternal(IStatObj *handle, bool bCreateIfNone)
{
	return handle->GetIndexedMesh(bCreateIfNone);
}

phys_geometry *StaticObjectInterop::GetPhysGeom(IStatObj *handle)
{
	return handle->GetPhysGeom();
}

IStatObj *StaticObjectInterop::UpdateVerticesInternal(IStatObj *handle, Vec3* vertices, Vec3* normals, int firstVertex, int vertexCount, float scale)
{
	return handle->UpdateVertices(vertices, normals, firstVertex, vertexCount, nullptr, scale);
}

void StaticObjectInterop::SetPhysGeom(IStatObj *handle, phys_geometry *pPhysGeom)
{
	handle->SetPhysGeom(pPhysGeom);
}

ITetrLattice *StaticObjectInterop::GetTetrLattice(IStatObj *handle)
{
	return handle->GetTetrLattice();
}

void StaticObjectInterop::SetMaterial(IStatObj *handle, IMaterial *pMaterial)
{
	handle->SetMaterial(pMaterial);
}

IMaterial *StaticObjectInterop::GetMaterial(IStatObj *handle)
{
	return handle->GetMaterial();
}

AABB StaticObjectInterop::GetBox(IStatObj *handle)
{
	return handle->GetAABB();
}

void StaticObjectInterop::SetBBox(IStatObj *handle, AABB vBBoxMin)
{
	handle->SetBBoxMin(vBBoxMin.min);
	handle->SetBBoxMax(vBBoxMin.max);
}

float StaticObjectInterop::GetRadius(IStatObj *handle)
{
	return handle->GetRadius();
}

void StaticObjectInterop::RefreshInternal(IStatObj *handle, int nFlags)
{
	handle->Refresh(nFlags);
}

PosNorm StaticObjectInterop::GetRandomPos(IStatObj *handle, EGeomForm eForm)
{
	handle->GetExtent(eForm);
	PosNorm posNorm;
	handle->GetRandomPos(posNorm, eForm);
	return posNorm;
}

IStatObj *StaticObjectInterop::GetLodObjectInternal(IStatObj *handle, int nLodLevel, bool bReturnNearest)
{
	return handle->GetLodObject(nLodLevel, bReturnNearest);
}

IStatObj *StaticObjectInterop::GetLowestLodInternal(IStatObj *handle)
{
	return handle->GetLowestLod();
}

int StaticObjectInterop::FindNearesLoadedLodInternal(IStatObj *handle, int nLodIn, bool bSearchUp)
{
	return handle->FindNearesLoadedLOD(nLodIn, bSearchUp);
}

int StaticObjectInterop::FindHighestLodInternal(IStatObj *handle, int nBias)
{
	return handle->FindHighestLOD(nBias);
}

mono::string StaticObjectInterop::GetFilePath(IStatObj *handle)
{
	return ToMonoString(handle->GetFilePath());
}

void StaticObjectInterop::SetFilePath(IStatObj *handle, mono::string szFileName)
{
	handle->SetFilePath(NtText(szFileName));
}

mono::string StaticObjectInterop::GetGeoName(IStatObj *handle)
{
	return ToMonoString(handle->GetGeoName());
}

void StaticObjectInterop::SetGeoName(IStatObj *handle, mono::string szGeoName)
{
	handle->SetGeoName(NtText(szGeoName));
}

Vec3 StaticObjectInterop::GetHelperPos(IStatObj *handle, mono::string szHelperName)
{
	return handle->GetHelperPos(NtText(szHelperName));
}

Matrix34 StaticObjectInterop::GetHelperTM(IStatObj *handle, mono::string szHelperName)
{
	return handle->GetHelperTM(NtText(szHelperName));
}

bool StaticObjectInterop::IsDefaultObject(IStatObj *handle)
{
	return handle->IsDefaultObject();
}

void StaticObjectInterop::FreeIndexedMesh(IStatObj *handle)
{
	handle->FreeIndexedMesh();
}

bool StaticObjectInterop::IsPhysicsExist(IStatObj *handle)
{
	return handle->IsPhysicsExist();
}

void StaticObjectInterop::InvalidateInternal(IStatObj *handle, bool bPhysics, float tolerance)
{
	handle->Invalidate(bPhysics, tolerance);
}

int StaticObjectInterop::GetSubObjectCount(IStatObj *handle)
{
	return handle->GetSubObjectCount();
}

void StaticObjectInterop::SetSubObjectCount(IStatObj *handle, int nCount)
{
	handle->SetSubObjectCount(nCount);
}

IStatObj::SSubObject *StaticObjectInterop::GetSubObject(IStatObj *handle, int nIndex)
{
	return handle->GetSubObject(nIndex);
}

bool StaticObjectInterop::IsSubObjectInternal(IStatObj *handle)
{
	return handle->IsSubObject();
}

IStatObj *StaticObjectInterop::GetParentObject(IStatObj *handle)
{
	return handle->GetParentObject();
}

IStatObj *StaticObjectInterop::GetCloneSourceObject(IStatObj *handle)
{
	return handle->GetCloneSourceObject();
}

IStatObj::SSubObject *StaticObjectInterop::FindSubObject(IStatObj *handle, mono::string sNodeName)
{
	return handle->FindSubObject(NtText(sNodeName));
}

IStatObj::SSubObject *StaticObjectInterop::FindSubObject_CGA(IStatObj *handle, mono::string sNodeName)
{
	return handle->FindSubObject_CGA(NtText(sNodeName));
}

IStatObj::SSubObject *StaticObjectInterop::FindSubObject_StrStr(IStatObj *handle, mono::string sNodeName)
{
	return handle->FindSubObject_StrStr(NtText(sNodeName));
}

bool StaticObjectInterop::RemoveSubObject(IStatObj *handle, int nIndex)
{
	return handle->RemoveSubObject(nIndex);
}

bool StaticObjectInterop::CopySubObject(IStatObj *handle, int nToIndex, IStatObj *pFromObj, int nFromIndex)
{
	return handle->CopySubObject(nToIndex, pFromObj, nFromIndex);
}

IStatObj::SSubObject *StaticObjectInterop::AddSubObject(IStatObj *handle, IStatObj *pStatObj)
{
	return &handle->AddSubObject(pStatObj);
}

int StaticObjectInterop::PhysicalizeSubobjects(IStatObj *handle, IPhysicalEntity *pent, Matrix34 *pMtx, float mass, float density, int id0, mono::string szPropsOverride)
{
	return handle->PhysicalizeSubobjects(pent, pMtx, mass, density, id0, nullptr, NtText(szPropsOverride));
}

int StaticObjectInterop::PhysicalizeInternal(IStatObj *handle, IPhysicalEntity *pent, GeometryParameters *pgp, int id, mono::string szPropsOverride)
{
	pe_geomparams params;
	pgp->ToGeomParams(params);
	return handle->Physicalize(pent, &params, id, NtText(szPropsOverride));
}

bool StaticObjectInterop::IsDeformableInternal(IStatObj *handle)
{
	return handle->IsDeformable();
}

bool StaticObjectInterop::SaveToCGF(IStatObj *handle, mono::string sFilename, bool bHavePhysicalProxy)
{
	return handle->SaveToCGF(NtText(sFilename), nullptr, bHavePhysicalProxy);
}

IStatObj *StaticObjectInterop::CloneInternal(IStatObj *handle, bool bCloneGeometry, bool bCloneChildren, bool bMeshesOnly)
{
	return handle->Clone(bCloneGeometry, bCloneChildren, bMeshesOnly);
}

int StaticObjectInterop::SetDeformationMorphTargetInternal(IStatObj *handle, IStatObj *pDeformed)
{
	return handle->SetDeformationMorphTarget(pDeformed);
}

IStatObj *StaticObjectInterop::DeformMorph(IStatObj *handle, Vec3 *pt, float r, float strength)
{
	return handle->DeformMorph(*pt, r, strength);
}

IStatObj *StaticObjectInterop::HideFoliage(IStatObj *handle)
{
	return handle->HideFoliage();
}

int StaticObjectInterop::Serialize(IStatObj *handle, TSerialize ser)
{
	return handle->Serialize(ser);
}

mono::string StaticObjectInterop::GetProperties(IStatObj *handle)
{
	return ToMonoString(handle->GetProperties());
}

bool StaticObjectInterop::GetPhysicalPropertiesInternal(IStatObj *handle, float *mass, float *density)
{
	return handle->GetPhysicalProperties(*mass, *density);
}

IStatObj *StaticObjectInterop::GetLastBooleanOp(IStatObj *handle, float *scale)
{
	return handle->GetLastBooleanOp(*scale);
}

void StaticObjectInterop::GetStatistics(IStatObj *handle, IStatObj::SStatistics &stats)
{
	handle->GetStatistics(stats);
}

IStatObj *StaticObjectInterop::CreateStatObjOptionalIndexedMesh(bool createIndexedMesh)
{
	return gEnv->p3DEngine->CreateStatObjOptionalIndexedMesh(createIndexedMesh);
}

IStatObj *StaticObjectInterop::UpdateDeformableStatObj(IGeometry *pPhysGeom, bop_meshupdate *pLastUpdate)
{
	return gEnv->p3DEngine->UpdateDeformableStatObj(pPhysGeom, pLastUpdate);
}

IRenderMesh *StaticObjectInterop::GetRenderMesh(IStatObj *handle)
{
	return handle->GetRenderMesh();
}
