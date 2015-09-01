#include "stdafx.h"
#include "IGeometry.h"

void IGeometryInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(CreateMesh);
	REGISTER_METHOD(CreateMeshBv);
	REGISTER_METHOD(CreateMeshVg);
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
