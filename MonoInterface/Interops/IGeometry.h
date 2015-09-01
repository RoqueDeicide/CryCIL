#pragma once

#include "IMonoInterface.h"

struct IGeometryInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "GeometryShape"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized() override;

	static IGeometry *CreateMesh(Vec3* vertices, uint16* indices, byte* materialIds, int* foreignIds, int triangleCount,
								 int flags, float approximationTolerance, int minTrianglesPerNode, int maxTrianglesPerNode,
								 float favorAabb);
	static IGeometry *CreateMeshBv(Vec3* vertices, uint16* indices, byte* materialIds, int* foreignIds, int triangleCount,
								   int flags, SMeshBVParams *vgParams, float approximationTolerance);
	static IGeometry *CreateMeshVg(Vec3* vertices, uint16* indices, byte* materialIds, int* foreignIds, int triangleCount,
								   int flags, SMeshBVParams *vgParams, float approximationTolerance);
};