#include "stdafx.h"
#include "IGeometry.h"
#include "ForeignData.h"

void IGeometryInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(CreateMesh);
	REGISTER_METHOD(CreateMeshBv);
	REGISTER_METHOD(CreateMeshVg);
	REGISTER_METHOD(CreatePrimitive);

	REGISTER_METHOD(GetGeometryType);
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(Lock);
	REGISTER_METHOD(Unlock);
	REGISTER_METHOD(GetBBox);
	REGISTER_METHOD(PointInsideStatus);
	REGISTER_METHOD(IntersectLocked);
	REGISTER_METHOD(IntersectLockedDefault);
	REGISTER_METHOD(FindClosestPointInternal);
	REGISTER_METHOD(CalcVolumetricPressure);
	REGISTER_METHOD(CalculateBuoyancyInternal);
	REGISTER_METHOD(CalculateMediumResistanceInternal);
	REGISTER_METHOD(IsConvexInternal);
	REGISTER_METHOD(GetPrimitiveCount);
	REGISTER_METHOD(GetVolume);
	REGISTER_METHOD(GetCenter);
	REGISTER_METHOD(Subtraction);
	REGISTER_METHOD(GetSubtractionsCount);
	REGISTER_METHOD(GetMeshUpdatesInternal);
	REGISTER_METHOD(GetForeignData);
	REGISTER_METHOD(SetForeignData);
	REGISTER_METHOD(GetErrorCount);
	REGISTER_METHOD(BoxifyInternal);
	REGISTER_METHOD(SanityCheck);
	REGISTER_METHOD(GetData);
}

IGeometry *IGeometryInterop::CreateMesh(Vec3* vertices, uint16* indices, byte* materialIds, int* foreignIds,
										int triangleCount, int flags, float approximationTolerance,
										int minTrianglesPerNode, int maxTrianglesPerNode, float favorAabb)
{
	return gEnv->pPhysicalWorld->GetGeomManager()->CreateMesh(vertices, indices, reinterpret_cast<char *>(materialIds),
															  foreignIds, triangleCount, flags, approximationTolerance,
															  minTrianglesPerNode, maxTrianglesPerNode, favorAabb);
}

IGeometry *IGeometryInterop::CreateMeshBv(Vec3* vertices, uint16* indices, byte* materialIds, int* foreignIds,
										  int triangleCount, int flags, SMeshBVParams *bvParams,
										  float approximationTolerance)
{
	return gEnv->pPhysicalWorld->GetGeomManager()->CreateMesh(vertices, indices, reinterpret_cast<char *>(materialIds),
															  foreignIds, triangleCount, flags, approximationTolerance,
															  bvParams);
}

IGeometry *IGeometryInterop::CreateMeshVg(Vec3* vertices, uint16* indices, byte* materialIds, int* foreignIds,
										  int triangleCount, int flags, SMeshBVParams *vgParams,
										  float approximationTolerance)
{
	return gEnv->pPhysicalWorld->GetGeomManager()->CreateMesh(vertices, indices, reinterpret_cast<char *>(materialIds),
															  foreignIds, triangleCount, flags, approximationTolerance,
															  vgParams);
}

IGeometry *IGeometryInterop::CreatePrimitive(int type, primitives::primitive *prim)
{
	return gEnv->pPhysicalWorld->GetGeomManager()->CreatePrimitive(type, prim);
}

int IGeometryInterop::GetGeometryType(IGeometry *handle)
{
	return handle->GetType();
}

int IGeometryInterop::AddRef(IGeometry *handle)
{
	return handle->AddRef();
}

void IGeometryInterop::Release(IGeometry *handle)
{
	handle->Release();
}

void IGeometryInterop::Lock(IGeometry *handle, int bWrite)
{
	handle->Lock(bWrite);
}

void IGeometryInterop::Unlock(IGeometry *handle, int bWrite)
{
	handle->Unlock(bWrite);
}

void IGeometryInterop::GetBBox(IGeometry *handle, primitives::box *pbox)
{
	handle->GetBBox(pbox);
}

int IGeometryInterop::PointInsideStatus(IGeometry *handle, Vec3 *pt)
{
	return handle->PointInsideStatus(*pt);
}

mono::Array IGeometryInterop::IntersectLocked(IGeometry *handle, IGeometry *pCollider, geom_world_data *pdata1,
											  geom_world_data *pdata2, intersection_params *pparams)
{
	WriteLockCond lock;

	geom_contact *contactsPtr;
	int contactCount = handle->IntersectLocked(pCollider, pdata1, pdata2, pparams, contactsPtr, lock);

	if (contactCount == 0)
	{
		return nullptr;
	}

	IMonoClass *geomContactClass = MonoEnv->Cryambly->GetClass("CryCil.Engine.Physics", "GeometryContact");
	IMonoArray<geom_contact> contacts = MonoEnv->Objects->Arrays->Create(contactCount, geomContactClass);

	MonoGCHandle gcHandle = MonoEnv->GC->Pin(contacts);

	for (int i = 0; i < contactCount; i++)
	{
		contacts[i] = contactsPtr[i];
	}

	return contacts;
}

mono::Array IGeometryInterop::IntersectLockedDefault(IGeometry *handle, IGeometry *pCollider)
{
	WriteLockCond lock;

	geom_contact *contactsPtr;
	int contactCount = handle->IntersectLocked(pCollider, nullptr, nullptr, nullptr, contactsPtr, lock);

	if (contactCount == 0)
	{
		return nullptr;
	}

	IMonoClass *geomContactClass = MonoEnv->Cryambly->GetClass("CryCil.Engine.Physics", "GeometryContact");
	IMonoArray<geom_contact> contacts = MonoEnv->Objects->Arrays->Create(contactCount, geomContactClass);

	MonoGCHandle gcHandle = MonoEnv->GC->Pin(contacts);

	for (int i = 0; i < contactCount; i++)
	{
		contacts[i] = contactsPtr[i];
	}

	return contacts;
}

int IGeometryInterop::FindClosestPointInternal(IGeometry *handle, geom_world_data *pgwd, int *iPrim, int *iFeature,
											   Vec3 *ptdst0, Vec3 *ptdst1, Vec3* ptres, int nMaxIters)
{
	return handle->FindClosestPoint(pgwd, *iPrim, *iFeature, *ptdst0, *ptdst1, ptres, nMaxIters);
}

void IGeometryInterop::CalcVolumetricPressure(IGeometry *handle, geom_world_data *gwd, Vec3 *epicenter, float k,
											  float rmin, Vec3 *P, Ang3 *L)
{
	handle->CalcVolumetricPressure(gwd, *epicenter, k, rmin, handle->GetCenter(), *P, *reinterpret_cast<Vec3 *>(L));
}

float IGeometryInterop::CalculateBuoyancyInternal(IGeometry *handle, primitives::plane *pplane, geom_world_data *pgwd,
												  Vec3 *submergedMassCenter)
{
	return handle->CalculateBuoyancy(pplane, pgwd, *submergedMassCenter);
}

void IGeometryInterop::CalculateMediumResistanceInternal(IGeometry *handle, primitives::plane *pplane,
														 geom_world_data *pgwd, Vec3 *dPres, Vec3 *dLres)
{
	handle->CalculateMediumResistance(pplane, pgwd, *dPres, *dLres);
}

int IGeometryInterop::IsConvexInternal(IGeometry *handle, float tolerance)
{
	return handle->IsConvex(tolerance);
}

int IGeometryInterop::GetPrimitiveCount(IGeometry *handle)
{
	return handle->GetPrimitiveCount();
}

float IGeometryInterop::GetVolume(IGeometry *handle)
{
	return handle->GetVolume();
}

Vec3 IGeometryInterop::GetCenter(IGeometry *handle)
{
	return handle->GetCenter();
}

int IGeometryInterop::Subtraction(IGeometry *handle, IGeometry *pGeom, geom_world_data *pdata1, geom_world_data *pdata2,
								  bool logUpdates)
{
	return handle->Subtract(pGeom, pdata1, pdata2, logUpdates ? 1 : 0);
}

int IGeometryInterop::GetSubtractionsCount(IGeometry *handle)
{
	return handle->GetSubtractionsCount();
}

bop_meshupdate *IGeometryInterop::GetMeshUpdatesInternal(IGeometry *handle)
{
	return static_cast<bop_meshupdate *>(handle->GetForeignData(DATA_MESHUPDATE));
}

ForeignData IGeometryInterop::GetForeignData(IGeometry *handle)
{
	ForeignData data;
	data.id = handle->GetiForeignData();
	data.handle = handle->GetForeignData(data.id);
	return data;
}

void IGeometryInterop::SetForeignData(IGeometry *handle, ForeignData data)
{
	handle->SetForeignData(data.handle, data.id);
}

int IGeometryInterop::GetErrorCount(IGeometry *handle)
{
	return handle->GetErrorCount();
}

int IGeometryInterop::BoxifyInternal(IGeometry *handle, primitives::box *pboxes, int nMaxBoxes,
									 IGeometry::SBoxificationParams *parameters)
{
	return handle->Boxify(pboxes, nMaxBoxes, *parameters);
}

int IGeometryInterop::SanityCheck(IGeometry *handle)
{
	return handle->SanityCheck();
}

const primitives::primitive *IGeometryInterop::GetData(IGeometry *handle)
{
	return handle->GetData();
}
