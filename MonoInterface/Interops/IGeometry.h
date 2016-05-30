#pragma once

#include "IMonoInterface.h"
#include <CryPhysics/primitives.h>

struct ForeignData;

struct IGeometryInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "GeometryShape"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void InitializeInterops() override;

	static IGeometry *CreateMesh(Vec3 *vertices, uint16 *indices, byte *materialIds, int *foreignIds, int triangleCount,
								 int flags, float approximationTolerance, int minTrianglesPerNode, int maxTrianglesPerNode,
								 float favorAabb);
	static IGeometry *CreateMeshBv(Vec3 *vertices, uint16 *indices, byte *materialIds, int *foreignIds, int triangleCount,
								   int flags, SMeshBVParams *vgParams, float approximationTolerance);
	static IGeometry *CreateMeshVg(Vec3 *vertices, uint16 *indices, byte *materialIds, int *foreignIds, int triangleCount,
								   int flags, SMeshBVParams *vgParams, float approximationTolerance);
	static IGeometry *CreatePrimitive(int type, primitives::primitive *prim);

	static int  GetGeometryType(IGeometry *handle);
	static int  AddRef(IGeometry *handle);
	static void Release(IGeometry *handle);
	static void Lock(IGeometry *handle, int bWrite);
	static void Unlock(IGeometry *handle, int bWrite);
	static void GetBBox(IGeometry *handle, primitives::box *pbox);
	static int  PointInsideStatus(IGeometry *handle, Vec3 *pt);

	static mono::Array IntersectLocked(IGeometry *handle, IGeometry *pCollider, geom_world_data *pdata1,
									   geom_world_data *pdata2, intersection_params *pparams);
	static mono::Array IntersectLockedDefault(IGeometry *handle, IGeometry *pCollider);
	static int         FindClosestPointInternal(IGeometry *handle, geom_world_data *pgwd, int *iPrim, int *iFeature, Vec3 *ptdst0,
												Vec3 *ptdst1, Vec3 *ptres, int nMaxIters);
	static void CalcVolumetricPressure(IGeometry *handle, geom_world_data *gwd, Vec3 *epicenter, float k, float rmin,
									   Vec3 *P, Ang3 *L);
	static float CalculateBuoyancyInternal(IGeometry *handle, primitives::plane *pplane, geom_world_data *pgwd,
										   Vec3 *submergedMassCenter);
	static void CalculateMediumResistanceInternal(IGeometry *handle, primitives::plane *pplane, geom_world_data *pgwd,
												  Vec3 *dPres, Vec3 *dLres);

	static int   IsConvexInternal(IGeometry *handle, float tolerance);
	static int   GetPrimitiveCount(IGeometry *handle);
	static float GetVolume(IGeometry *handle);
	static Vec3  GetCenter(IGeometry *handle);

	static int Subtraction(IGeometry *handle, IGeometry *pGeom, geom_world_data *pdata1, geom_world_data *pdata2,
						   bool logUpdates);

	static int                          GetSubtractionsCount(IGeometry *handle);
	static bop_meshupdate              *GetMeshUpdatesInternal(IGeometry *handle);
	static ForeignData                  GetForeignData(IGeometry *handle);
	static void                         SetForeignData(IGeometry *handle, ForeignData data);
	static int                          GetErrorCount(IGeometry *handle);
	static int                          BoxifyInternal(IGeometry *handle, primitives::box *pboxes, int nMaxBoxes, IGeometry::SBoxificationParams *parameters);
	static int                          SanityCheck(IGeometry *handle);
	static const primitives::primitive *GetData(IGeometry *handle);
};