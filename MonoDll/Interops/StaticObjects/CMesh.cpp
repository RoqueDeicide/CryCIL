#include "stdafx.h"
#include "CMesh.h"

CMeshInterop::CMeshInterop()
{
	REGISTER_METHOD(GetFaceCount);
	REGISTER_METHOD(GetVertexCount);
	REGISTER_METHOD(GetTexCoordCount);
	REGISTER_METHOD(GetTangentCount);
	REGISTER_METHOD(GetSubSetCount);
	REGISTER_METHOD(GetIndexCount);
	REGISTER_METHOD(SetFaceCount);
	REGISTER_METHOD(SetVertexCount);
	REGISTER_METHOD(SetTexCoordsCount);
	REGISTER_METHOD(SetTexCoordsAndTangentsCount);
	REGISTER_METHOD(SetIndexCount);
	REGISTER_METHOD(GetStreamHandle);
	REGISTER_METHOD(ReallocateStream);
	REGISTER_METHOD(Export);
	REGISTER_METHOD(GetNumberOfElements);
}

CMeshInterop::~CMeshInterop()
{}

int CMeshInterop::GetFaceCount(CMesh *cMeshHandle)
{
	if (cMeshHandle)
	{
		return cMeshHandle->GetFaceCount();
	}
	return -1;
}
int CMeshInterop::GetVertexCount(CMesh *cMeshHandle)
{
	if (cMeshHandle)
	{
		return cMeshHandle->GetVertexCount();
	}
	return -1;
}
int CMeshInterop::GetTexCoordCount(CMesh *cMeshHandle)
{
	if (cMeshHandle)
	{
		return cMeshHandle->GetTexCoordCount();
	}
	return -1;
}
int CMeshInterop::GetTangentCount(CMesh *cMeshHandle)
{
	if (cMeshHandle)
	{
		return cMeshHandle->GetTangentCount();
	}
	return -1;
}
int CMeshInterop::GetSubSetCount(CMesh *cMeshHandle)
{
	if (cMeshHandle)
	{
		return cMeshHandle->GetSubSetCount();
	}
	return -1;
}
int CMeshInterop::GetIndexCount(CMesh *cMeshHandle)
{
	if (cMeshHandle)
	{
		return cMeshHandle->GetIndexCount();
	}
	return -1;
}
void CMeshInterop::SetFaceCount(CMesh *cMeshHandle, int nNewCount)
{
	if (cMeshHandle)
	{
		cMeshHandle->SetFaceCount(nNewCount);
	}
}
void CMeshInterop::SetVertexCount(CMesh *cMeshHandle, int nNewCount)
{
	if (cMeshHandle)
	{
		cMeshHandle->SetVertexCount(nNewCount);
	}
}
void CMeshInterop::SetTexCoordsCount(CMesh *cMeshHandle, int nNewCount)
{
	if (cMeshHandle)
	{
		cMeshHandle->SetTexCoordsCount(nNewCount);
	}
}
void CMeshInterop::SetTexCoordsAndTangentsCount(CMesh *cMeshHandle, int nNewCount)
{
	if (cMeshHandle)
	{
		cMeshHandle->SetTexCoordsAndTangentsCount(nNewCount);
	}
}
void CMeshInterop::SetIndexCount(CMesh *cMeshHandle, int nNewCount)
{
	if (cMeshHandle)
	{
		cMeshHandle->SetIndexCount(nNewCount);
	}
}
void *CMeshInterop::GetStreamHandle(CMesh *cMeshHandle, int streamIdentifier)
{
	if (cMeshHandle)
	{
		void *streamHandle = nullptr;		// This will hold pointer that we need.
		int elementSize = 0;				// Not used but needed to complete list of arguments.
		// Acquire the data.
		cMeshHandle->GetStreamInfo(streamIdentifier, streamHandle, elementSize);
		// Return it.
		return streamHandle;
	}
	return nullptr;
}
void CMeshInterop::ReallocateStream(CMesh *cMeshHandle, int streamIdentifier, int newCount)
{
	if (cMeshHandle)
	{
		cMeshHandle->ReallocStream(streamIdentifier, newCount);
	}
}
void CMeshInterop::Export(IStatObj *obj)
{
	// Check everything, and get all of the data.
	if (!obj)
	{
		return;
	}

	IIndexedMesh* indexedMeshHandle = obj->GetIndexedMesh();
	if (!indexedMeshHandle)
	{
		return;
	}

	CMesh *cMeshHandle = indexedMeshHandle->GetMesh();
	if (!cMeshHandle)
	{
		return;
	}
	// Checks are done.

	// Create a bounding box.
	indexedMeshHandle->CalcBBox();
	// Optimize mesh.
	indexedMeshHandle->Optimize();
	// The following call is questionable. It is probably needed because Optimize method tampers with Faces stream.
	indexedMeshHandle->RestoreFacesFromIndices();
	// Tell static object to create new render mesh before next rendering frame.
	obj->Invalidate(false);
}
int CMeshInterop::GetNumberOfElements(CMesh *cMeshHandle, int streamIdentifier)
{
	return
		(cMeshHandle && streamIdentifier >= 0 && streamIdentifier < CMesh::LAST_STREAM)
		?
		cMeshHandle->m_streamSize[streamIdentifier]
		:
		-1;
}