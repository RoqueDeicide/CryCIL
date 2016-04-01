#pragma once

#include "IMonoInterface.h"

struct RenderMeshInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryRenderMesh"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.StaticObjects"; }

	virtual void InitializeInterops() override;

	static void AddRef(IRenderMesh *handle);
	static int Release(IRenderMesh *handle);
	static bool CanRender(IRenderMesh *handle);
	static mono::string GetTypeName(IRenderMesh *handle);
	static mono::string GetSourceName(IRenderMesh *handle);
	static int GetIndicesCount(IRenderMesh *handle);
	static int GetVerticesCount(IRenderMesh *handle);
	static EVertexFormat GetVertexFormat(IRenderMesh *handle);
	static ERenderMeshType GetMeshType(IRenderMesh *handle);
	static float GetGeometricMeanFaceArea(IRenderMesh *handle);
	static uint SetMeshInternal(IRenderMesh *handle, CMesh &mesh, int nSecColorsSetOffset, uint flags, Vec3 &pPosOffset,
								bool requiresLock);
	static void CopyToInternal(IRenderMesh *handle, IRenderMesh *pDst, int nAppendVtx, bool bDynamic, bool fullCopy);
	static void SetSkinningDataVegetation(IRenderMesh *handle, SMeshBoneMapping_uint8* pBoneMapping);
	static void SetSkinningDataCharacterInternal(IRenderMesh *handle, CMesh &mesh,SMeshBoneMapping_uint16* pBoneMapping,
												 SMeshBoneMapping_uint16* pExtraBoneMapping);
	static IIndexedMesh *GetIndexedMesh(IRenderMesh *handle);

	static IRenderMesh *GenerateMorphWeights(IRenderMesh *handle);
	static IRenderMesh *GetMorphBuddy(IRenderMesh *handle);
	static void SetMorphBuddy(IRenderMesh *handle, IRenderMesh *pMorph);
	static bool UpdateVerticesInternal(IRenderMesh *handle, void* pVertBuffer, int nVertCount, int nOffset, int stream,
									   uint copyFlags, bool requiresLock = true);
	static bool UpdateIndicesInternal(IRenderMesh *handle, uint* pNewInds, int nInds, int nOffsInd,uint copyFlags,
									  bool requiresLock = true);
	static void SetCustomTexID(IRenderMesh *handle, int nCustomTID);
	static void GenerateQTangentsInternal(IRenderMesh *handle);
	static IRenderMesh *GetVertexContainer(IRenderMesh *handle);
	static void SetVertexContainer(IRenderMesh *handle, IRenderMesh *pBuf);
	static void SetBBox(IRenderMesh *handle, Vec3 &vBoxMin, Vec3 &vBoxMax);
	static void GetBBox(IRenderMesh *handle, Vec3 &vBoxMin, Vec3 &vBoxMax);
	static void UpdateBBoxFromMesh(IRenderMesh *handle);
	static uint* GetPhysVertexMap(IRenderMesh *handle);
	static bool IsEmptyInternal(IRenderMesh *handle);
	static byte* GetPosPtrNoCache(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset);
	static byte* GetPosPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset);
	static byte* GetColorPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset);
	static byte* GetNormPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset);
	static byte* GetUVPtrNoCache(IRenderMesh *handle, int &nStride, uint nFlags,int nOffset);
	static byte* GetUVPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset);
	static byte* GetTangentPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset);
	static byte* GetQTangentPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset);

	static byte* GetHWSkinPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset, bool remapped = false);
	static byte* GetVelocityPtr(IRenderMesh *handle, int &nStride, uint nFlags,int nOffset);
	static void UnlockStreamInternal(IRenderMesh *handle, int nStream);
	static void UnlockIndexStreamInternal(IRenderMesh *handle);
	static uint* GetIndexPtr(IRenderMesh *handle, uint nFlags, int nOffset);
	static int GetAllocatedBytesInternal(IRenderMesh *handle, bool bVideoMem);
	static void SetMeshLodInternal(IRenderMesh *handle, int nLod);

	static void LockForThreadAccessInternal(IRenderMesh *handle);
	static void UnLockForThreadAccessInternal(IRenderMesh *handle);

	static void OffsetPositionInternal(IRenderMesh *handle, Vec3 &delta);
	static IRenderMesh *CreateRenderMesh(mono::string szType, mono::string szSourceName,
										 IRenderMesh::SInitParamerers* pInitParams, ERenderMeshType eBufType);
};