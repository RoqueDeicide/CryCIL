#include "stdafx.h"

#include "RenderMesh.h"
#include <IRenderMesh.h>
#include <IIndexedMesh.h>

void RenderMeshInterop::InitializeInterops()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(CanRender);
	REGISTER_METHOD(GetTypeName);
	REGISTER_METHOD(GetSourceName);
	REGISTER_METHOD(GetIndicesCount);
	REGISTER_METHOD(GetVerticesCount);
	REGISTER_METHOD(GetVertexFormat);
	REGISTER_METHOD(GetMeshType);
	REGISTER_METHOD(GetGeometricMeanFaceArea);
	REGISTER_METHOD(SetMeshInternal);
	REGISTER_METHOD(CopyToInternal);
	REGISTER_METHOD(SetSkinningDataVegetation);
	REGISTER_METHOD(SetSkinningDataCharacterInternal);
	REGISTER_METHOD(GetIndexedMesh);

	REGISTER_METHOD(GenerateMorphWeights);
	REGISTER_METHOD(GetMorphBuddy);
	REGISTER_METHOD(SetMorphBuddy);
	REGISTER_METHOD(UpdateVerticesInternal);
	REGISTER_METHOD(UpdateIndicesInternal);
	REGISTER_METHOD(SetCustomTexID);
	REGISTER_METHOD(GenerateQTangentsInternal);
	REGISTER_METHOD(GetVertexContainer);
	REGISTER_METHOD(SetVertexContainer);
	REGISTER_METHOD(SetBBox);
	REGISTER_METHOD(GetBBox);
	REGISTER_METHOD(UpdateBBoxFromMesh);
	REGISTER_METHOD(GetPhysVertexMap);
	REGISTER_METHOD(IsEmptyInternal);
	REGISTER_METHOD(GetPosPtrNoCache);
	REGISTER_METHOD(GetPosPtr);
	REGISTER_METHOD(GetColorPtr);
	REGISTER_METHOD(GetNormPtr);
	REGISTER_METHOD(GetUVPtrNoCache);
	REGISTER_METHOD(GetUVPtr);
	REGISTER_METHOD(GetTangentPtr);
	REGISTER_METHOD(GetQTangentPtr);

	REGISTER_METHOD(GetHWSkinPtr);
	REGISTER_METHOD(GetVelocityPtr);
	REGISTER_METHOD(UnlockStreamInternal);
	REGISTER_METHOD(UnlockIndexStreamInternal);
	REGISTER_METHOD(GetIndexPtr);
	REGISTER_METHOD(GetAllocatedBytesInternal);
	REGISTER_METHOD(SetMeshLodInternal);

	REGISTER_METHOD(LockForThreadAccessInternal);
	REGISTER_METHOD(UnLockForThreadAccessInternal);

	REGISTER_METHOD(OffsetPositionInternal);
	REGISTER_METHOD(CreateRenderMesh);
}

void RenderMeshInterop::AddRef(IRenderMesh *handle)
{
	handle->AddRef();
}

int RenderMeshInterop::Release(IRenderMesh *handle)
{
	return handle->Release();
}

bool RenderMeshInterop::CanRender(IRenderMesh *handle)
{
	return handle->CanRender();
}

mono::string RenderMeshInterop::GetTypeName(IRenderMesh *handle)
{
	return ToMonoString(handle->GetTypeName());
}

mono::string RenderMeshInterop::GetSourceName(IRenderMesh *handle)
{
	return ToMonoString(handle->GetSourceName());
}

int RenderMeshInterop::GetIndicesCount(IRenderMesh *handle)
{
	return handle->GetIndicesCount();
}

int RenderMeshInterop::GetVerticesCount(IRenderMesh *handle)
{
	return handle->GetVerticesCount();
}

EVertexFormat RenderMeshInterop::GetVertexFormat(IRenderMesh *handle)
{
	return handle->GetVertexFormat();
}

ERenderMeshType RenderMeshInterop::GetMeshType(IRenderMesh *handle)
{
	return handle->GetMeshType();
}

float RenderMeshInterop::GetGeometricMeanFaceArea(IRenderMesh *handle)
{
	return handle->GetGeometricMeanFaceArea();
}

uint RenderMeshInterop::SetMeshInternal(IRenderMesh *handle, CMesh &mesh, int nSecColorsSetOffset, uint flags,
										Vec3 &pPosOffset, bool requiresLock)
{
	return handle->SetMesh(mesh, nSecColorsSetOffset, flags, &pPosOffset, requiresLock);
}

void RenderMeshInterop::CopyToInternal(IRenderMesh *handle, IRenderMesh *pDst, int nAppendVtx, bool bDynamic,
									   bool fullCopy)
{
	handle->CopyTo(pDst, nAppendVtx, bDynamic, fullCopy);
}

void RenderMeshInterop::SetSkinningDataVegetation(IRenderMesh *handle, SMeshBoneMapping_uint8 *pBoneMapping)
{
	handle->SetSkinningDataVegetation(pBoneMapping);
}

void RenderMeshInterop::SetSkinningDataCharacterInternal(IRenderMesh *handle, CMesh &mesh,
														 SMeshBoneMapping_uint16 *pBoneMapping,
														 SMeshBoneMapping_uint16 *pExtraBoneMapping)
{
	handle->SetSkinningDataCharacter(mesh, pBoneMapping, pExtraBoneMapping);
}

IIndexedMesh *RenderMeshInterop::GetIndexedMesh(IRenderMesh *handle)
{
	return handle->GetIndexedMesh();
}

IRenderMesh *RenderMeshInterop::GenerateMorphWeights(IRenderMesh *handle)
{
	return handle->GenerateMorphWeights();
}

IRenderMesh *RenderMeshInterop::GetMorphBuddy(IRenderMesh *handle)
{
	return handle->GetMorphBuddy();
}

void RenderMeshInterop::SetMorphBuddy(IRenderMesh *handle, IRenderMesh *pMorph)
{
	handle->SetMorphBuddy(pMorph);
}

bool RenderMeshInterop::UpdateVerticesInternal(IRenderMesh *handle, void *pVertBuffer, int nVertCount, int nOffset,
											   int stream, uint copyFlags, bool requiresLock /*= true*/)
{
	return handle->UpdateVertices(pVertBuffer, nVertCount, nOffset, stream, copyFlags, requiresLock);
}

bool RenderMeshInterop::UpdateIndicesInternal(IRenderMesh *handle, uint *pNewInds, int nInds, int nOffsInd, uint copyFlags,
											  bool requiresLock /*= true*/)
{
	return handle->UpdateIndices(pNewInds, nInds, nOffsInd, copyFlags, requiresLock);
}

void RenderMeshInterop::SetCustomTexID(IRenderMesh *handle, int nCustomTID)
{
	handle->SetCustomTexID(nCustomTID);
}

void RenderMeshInterop::GenerateQTangentsInternal(IRenderMesh *handle)
{
	handle->GenerateQTangents();
}

IRenderMesh *RenderMeshInterop::GetVertexContainer(IRenderMesh *handle)
{
	return handle->GetVertexContainer();
}

void RenderMeshInterop::SetVertexContainer(IRenderMesh *handle, IRenderMesh *pBuf)
{
	handle->SetVertexContainer(pBuf);
}

void RenderMeshInterop::SetBBox(IRenderMesh *handle, Vec3 &vBoxMin, Vec3 &vBoxMax)
{
	handle->SetBBox(vBoxMin, vBoxMax);
}

void RenderMeshInterop::GetBBox(IRenderMesh *handle, Vec3 &vBoxMin, Vec3 &vBoxMax)
{
	handle->GetBBox(vBoxMin, vBoxMax);
}

void RenderMeshInterop::UpdateBBoxFromMesh(IRenderMesh *handle)
{
	handle->UpdateBBoxFromMesh();
}

uint *RenderMeshInterop::GetPhysVertexMap(IRenderMesh *handle)
{
	return handle->GetPhysVertexMap();
}

bool RenderMeshInterop::IsEmptyInternal(IRenderMesh *handle)
{
	return handle->IsEmpty();
}

byte *RenderMeshInterop::GetPosPtrNoCache(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetPosPtrNoCache(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetPosPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetPosPtr(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetColorPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetColorPtr(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetNormPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetNormPtr(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetUVPtrNoCache(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetUVPtrNoCache(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetUVPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetUVPtr(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetTangentPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetTangentPtr(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetQTangentPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetQTangentPtr(nStride, nFlags, nOffset);
}

byte *RenderMeshInterop::GetHWSkinPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset, bool remapped)
{
	return handle->GetHWSkinPtr(nStride, nFlags, nOffset, remapped);
}

byte *RenderMeshInterop::GetVelocityPtr(IRenderMesh *handle, int &nStride, uint nFlags, int nOffset)
{
	return handle->GetVelocityPtr(nStride, nFlags, nOffset);
}

void RenderMeshInterop::UnlockStreamInternal(IRenderMesh *handle, int nStream)
{
	handle->UnlockStream(nStream);
}

void RenderMeshInterop::UnlockIndexStreamInternal(IRenderMesh *handle)
{
	handle->UnlockIndexStream();
}

uint *RenderMeshInterop::GetIndexPtr(IRenderMesh *handle, uint nFlags, int nOffset)
{
	return handle->GetIndexPtr(nFlags, nOffset);
}

int RenderMeshInterop::GetAllocatedBytesInternal(IRenderMesh *handle, bool bVideoMem)
{
	return handle->GetAllocatedBytes(bVideoMem);
}

void RenderMeshInterop::SetMeshLodInternal(IRenderMesh *handle, int nLod)
{
	handle->SetMeshLod(nLod);
}

void RenderMeshInterop::LockForThreadAccessInternal(IRenderMesh *handle)
{
	handle->LockForThreadAccess();
}

void RenderMeshInterop::UnLockForThreadAccessInternal(IRenderMesh *handle)
{
	handle->UnLockForThreadAccess();
}

void RenderMeshInterop::OffsetPositionInternal(IRenderMesh *handle, Vec3 &delta)
{
	handle->OffsetPosition(delta);
}

IRenderMesh *RenderMeshInterop::CreateRenderMesh(mono::string szType, mono::string szSourceName,
												 IRenderMesh::SInitParamerers *pInitParams, ERenderMeshType eBufType)
{
	auto renderMesh = gEnv->pRenderer->CreateRenderMesh(NtText(szType), NtText(szSourceName), pInitParams, eBufType);
	return ReleaseOwnership(renderMesh);
}
