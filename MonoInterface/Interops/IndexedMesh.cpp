#include "stdafx.h"

#include "IndexedMesh.h"

void IndexedMeshInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(ReleaseInternal);
	REGISTER_METHOD(GetMeshDescription);
	REGISTER_METHOD(GetMesh);
	REGISTER_METHOD(SetMesh);
	REGISTER_METHOD(FreeStreams);
	REGISTER_METHOD(GetFaceCount);
	REGISTER_METHOD(SetFaceCount);
	REGISTER_METHOD(GetVertexCount);
	REGISTER_METHOD(SetVertexCount);
	REGISTER_METHOD(GetTexCoordCount);
	REGISTER_METHOD(SetTexCoordCount);
	REGISTER_METHOD(GetIndexCount);
	REGISTER_METHOD(SetIndexCount);
	REGISTER_METHOD(GetSubSetCount);
	REGISTER_METHOD(SetSubSetCount);
	REGISTER_METHOD(GetSubSet);
	REGISTER_METHOD(SetSubsetBounds);
	REGISTER_METHOD(SetSubsetIndexVertexRanges);
	REGISTER_METHOD(SetSubsetMaterialId);
	REGISTER_METHOD(SetSubsetMaterialProperties);
	REGISTER_METHOD(SetBBox);
	REGISTER_METHOD(GetBBox);
	REGISTER_METHOD(CalcBBox);
	REGISTER_METHOD(OptimizeInternal);
}

void IndexedMeshInterop::ReleaseInternal(IIndexedMesh *handle)
{
	handle->Release();
}

void IndexedMeshInterop::GetMeshDescription(IIndexedMesh *handle, IIndexedMesh::SMeshDescription &meshDesc)
{
	handle->GetMeshDescription(meshDesc);
}

CMesh *IndexedMeshInterop::GetMesh(IIndexedMesh *handle)
{
	return handle->GetMesh();
}

void IndexedMeshInterop::SetMesh(IIndexedMesh *handle, CMesh &mesh)
{
	handle->SetMesh(mesh);
}

void IndexedMeshInterop::FreeStreams(IIndexedMesh *handle)
{
	handle->FreeStreams();
}

int IndexedMeshInterop::GetFaceCount(IIndexedMesh *handle)
{
	return handle->GetFaceCount();
}

void IndexedMeshInterop::SetFaceCount(IIndexedMesh *handle, int nNewCount)
{
	handle->SetFaceCount(nNewCount);
}

int IndexedMeshInterop::GetVertexCount(IIndexedMesh *handle)
{
	return handle->GetVertexCount();
}

void IndexedMeshInterop::SetVertexCount(IIndexedMesh *handle, int nNewCount)
{
	handle->SetVertexCount(nNewCount);
}

int IndexedMeshInterop::GetTexCoordCount(IIndexedMesh *handle)
{
	return handle->GetTexCoordCount();
}

void IndexedMeshInterop::SetTexCoordCount(IIndexedMesh *handle, int nNewCount)
{
	handle->SetTexCoordCount(nNewCount);
}

int IndexedMeshInterop::GetIndexCount(IIndexedMesh *handle)
{
	return handle->GetIndexCount();
}

void IndexedMeshInterop::SetIndexCount(IIndexedMesh *handle, int nNewCount)
{
	handle->SetIndexCount(nNewCount);
}

int IndexedMeshInterop::GetSubSetCount(IIndexedMesh *handle)
{
	return handle->GetSubSetCount();
}

void IndexedMeshInterop::SetSubSetCount(IIndexedMesh *handle, int nSubsets)
{
	handle->SetSubSetCount(nSubsets);
}

const SMeshSubset &IndexedMeshInterop::GetSubSet(IIndexedMesh *handle, int nIndex)
{
	return handle->GetSubSet(nIndex);
}

void IndexedMeshInterop::SetSubsetBounds(IIndexedMesh *handle, int nIndex, Vec3 &vCenter, float fRadius)
{
	handle->SetSubsetBounds(nIndex, vCenter, fRadius);
}

void IndexedMeshInterop::SetSubsetIndexVertexRanges(IIndexedMesh *handle, int nIndex, int nFirstIndexId, int nNumIndices,
													int nFirstVertId, int nNumVerts)
{
	handle->SetSubsetIndexVertexRanges(nIndex, nFirstIndexId, nNumIndices, nFirstVertId, nNumVerts);
}

void IndexedMeshInterop::SetSubsetMaterialId(IIndexedMesh *handle, int nIndex, int nMatID)
{
	handle->SetSubsetMaterialId(nIndex, nMatID);
}

void IndexedMeshInterop::SetSubsetMaterialProperties(IIndexedMesh *handle, int nIndex, int nMatFlags, int nPhysicalizeType)
{
	handle->SetSubsetMaterialProperties(nIndex, nMatFlags, nPhysicalizeType);
}

void IndexedMeshInterop::SetBBox(IIndexedMesh *handle, AABB &box)
{
	handle->SetBBox(box);
}

AABB IndexedMeshInterop::GetBBox(IIndexedMesh *handle)
{
	return handle->GetBBox();
}

void IndexedMeshInterop::CalcBBox(IIndexedMesh *handle)
{
	handle->CalcBBox();
}

void IndexedMeshInterop::OptimizeInternal(IIndexedMesh *handle)
{
	handle->Optimize();
}
