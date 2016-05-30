#pragma once

#include "IMonoInterface.h"
#include <Cry3DEngine/IIndexedMesh.h>

struct IndexedMeshInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryIndexedMesh"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.StaticObjects"; }

	virtual void InitializeInterops() override;

	static void   ReleaseInternal(IIndexedMesh *handle);
	static void   GetMeshDescription(IIndexedMesh *handle, IIndexedMesh::SMeshDescription &meshDesc);
	static CMesh *GetMesh(IIndexedMesh *handle);
	static void   SetMesh(IIndexedMesh *handle, CMesh &mesh);
	static void   FreeStreams(IIndexedMesh *handle);
	static int    GetFaceCount(IIndexedMesh *handle);
	static void   SetFaceCount(IIndexedMesh *handle, int nNewCount);
	static int    GetVertexCount(IIndexedMesh *handle);
	static void   SetVertexCount(IIndexedMesh *handle, int nNewCount);
	static int    GetTexCoordCount(IIndexedMesh *handle);
	static void   SetTexCoordCount(IIndexedMesh *handle, int nNewCount);
	static int    GetIndexCount(IIndexedMesh *handle);
	static void   SetIndexCount(IIndexedMesh *handle, int nNewCount);
	static int    GetSubSetCount(IIndexedMesh *handle);
	static void   SetSubSetCount(IIndexedMesh *handle, int nSubsets);
	static const SMeshSubset &GetSubSet(IIndexedMesh *handle, int nIndex);
	static void   SetSubsetBounds(IIndexedMesh *handle, int nIndex, Vec3 &vCenter, float fRadius);
	static void   SetSubsetIndexVertexRanges(IIndexedMesh *handle, int nIndex, int nFirstIndexId, int nNumIndices, int nFirstVertId, int nNumVerts);
	static void   SetSubsetMaterialId(IIndexedMesh *handle, int nIndex, int nMatID);
	static void   SetSubsetMaterialProperties(IIndexedMesh *handle, int nIndex, int nMatFlags, int nPhysicalizeType);
	static void   SetBBox(IIndexedMesh *handle, AABB &box);
	static AABB   GetBBox(IIndexedMesh *handle);
	static void   CalcBBox(IIndexedMesh *handle);
	static void   OptimizeInternal(IIndexedMesh *handle);
};