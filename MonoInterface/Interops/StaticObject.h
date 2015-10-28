#pragma once

#include "IMonoInterface.h"

struct GeometryParameters;

struct StaticObjectInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "StaticObject"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.StaticObjects"; }

	virtual void OnRunTimeInitialized() override;

	static int AddRef(IStatObj *handle);
	static int Release(IStatObj *handle);
	static void SetFlags(IStatObj *handle, int nFlags);
	static int GetFlags(IStatObj *handle);
	static int GetIdMatBreakable(IStatObj *handle);
	static IIndexedMesh *GetIndexedMeshInternal(IStatObj *handle, bool bCreateIfNone);
	static IRenderMesh *GetRenderMesh(IStatObj *handle);
	static phys_geometry *GetPhysGeom(IStatObj *handle);
	static IStatObj *UpdateVerticesInternal(IStatObj *handle, Vec3* vertices, Vec3* normals, int firstVertex,
											int vertexCount, float scale);
	static void SetPhysGeom(IStatObj *handle, phys_geometry *pPhysGeom);
	static ITetrLattice *GetTetrLattice(IStatObj *handle);
	static void SetMaterial(IStatObj *handle, IMaterial *pMaterial);
	static IMaterial *GetMaterial(IStatObj *handle);
	static AABB GetBox(IStatObj *handle);
	static void SetBBox(IStatObj *handle, AABB vBBoxMin);
	static float GetRadius(IStatObj *handle);
	static void RefreshInternal(IStatObj *handle, int nFlags);
	static PosNorm GetRandomPos(IStatObj *handle, EGeomForm eForm);
	static IStatObj *GetLodObjectInternal(IStatObj *handle, int nLodLevel, bool bReturnNearest);
	static IStatObj *GetLowestLodInternal(IStatObj *handle);
	static int FindNearesLoadedLodInternal(IStatObj *handle, int nLodIn, bool bSearchUp);
	static int FindHighestLodInternal(IStatObj *handle, int nBias);
	static mono::string GetFilePath(IStatObj *handle);
	static void SetFilePath(IStatObj *handle, mono::string szFileName);
	static mono::string GetGeoName(IStatObj *handle);
	static void SetGeoName(IStatObj *handle, mono::string szGeoName);
	static Vec3 GetHelperPos(IStatObj *handle, mono::string szHelperName);
	static Matrix34 GetHelperTM(IStatObj *handle, mono::string szHelperName);
	static bool IsDefaultObject(IStatObj *handle);
	static void FreeIndexedMesh(IStatObj *handle);
	static bool IsPhysicsExist(IStatObj *handle);
	static void InvalidateInternal(IStatObj *handle, bool bPhysics, float tolerance);
	static int GetSubObjectCount(IStatObj *handle);
	static void SetSubObjectCount(IStatObj *handle, int nCount);
	static IStatObj::SSubObject *GetSubObject(IStatObj *handle, int nIndex);
	static bool IsSubObjectInternal(IStatObj *handle);
	static IStatObj *GetParentObject(IStatObj *handle);
	static IStatObj *GetCloneSourceObject(IStatObj *handle);
	static IStatObj::SSubObject *FindSubObject(IStatObj *handle, mono::string sNodeName);
	static IStatObj::SSubObject *FindSubObject_CGA(IStatObj *handle, mono::string sNodeName);
	static IStatObj::SSubObject *FindSubObject_StrStr(IStatObj *handle, mono::string sNodeName);
	static bool RemoveSubObject(IStatObj *handle, int nIndex);
	static bool CopySubObject(IStatObj *handle, int nToIndex, IStatObj *pFromObj, int nFromIndex);
	static IStatObj::SSubObject *AddSubObject(IStatObj *handle, IStatObj *pStatObj);
	static int PhysicalizeSubobjects(IStatObj *handle, IPhysicalEntity *pent, Matrix34 *pMtx, float mass, float density,
									 int id0, mono::string szPropsOverride);
	static int PhysicalizeInternal(IStatObj *handle, IPhysicalEntity *pent, GeometryParameters *pgp, int id,
								   mono::string szPropsOverride);
	static bool IsDeformableInternal(IStatObj *handle);
	static bool SaveToCGF(IStatObj *handle, mono::string sFilename, bool bHavePhysicalProxy);
	static IStatObj *CloneInternal(IStatObj *handle, bool bCloneGeometry, bool bCloneChildren, bool bMeshesOnly);
	static int SetDeformationMorphTargetInternal(IStatObj *handle, IStatObj *pDeformed);
	static IStatObj *DeformMorph(IStatObj *handle, Vec3 *pt, float r, float strength);
	static IStatObj *HideFoliage(IStatObj *handle);
	static int Serialize(IStatObj *handle, TSerialize ser);
	static mono::string GetProperties(IStatObj *handle);
	static bool GetPhysicalPropertiesInternal(IStatObj *handle, float *mass, float *density);
	static IStatObj *GetLastBooleanOp(IStatObj *handle, float *scale);
	static void GetStatistics(IStatObj *handle, IStatObj::SStatistics &stats);
	static IStatObj *CreateStatObjOptionalIndexedMesh(bool createIndexedMesh);
	static IStatObj *UpdateDeformableStatObj(IGeometry *pPhysGeom, bop_meshupdate *pLastUpdate);
};