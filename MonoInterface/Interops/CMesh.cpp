#include "stdafx.h"

#include "CMesh.h"
#include <IIndexedMesh.h>

void CMeshInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetStreamPtr);
	REGISTER_METHOD(ReallocateStream);
	REGISTER_METHOD(GetSubsetCount);
	REGISTER_METHOD(SetSubsetCount);
	REGISTER_METHOD(CopyMesh);
	REGISTER_METHOD(CompareStreams);
	REGISTER_METHOD(AppendData);
	REGISTER_METHOD(AppendSpecificData);
	REGISTER_METHOD(RemoveRangeFromStreamInternal);
	REGISTER_METHOD(ValidateInternal);
	REGISTER_METHOD(ValidateMessage);
}

void *CMeshInterop::GetStreamPtr(CMesh *handle, int stream, int *pElementCount)
{
	void *ptr;
	handle->GetStreamInfo(stream, ptr, *pElementCount);
	return ptr;
}

int CMeshInterop::GetSubsetCount(CMesh *handle)
{
	return handle->m_subsets.size();
}

void CMeshInterop::SetSubsetCount(CMesh *handle, int count)
{
	handle->m_subsets.resize(count);
}

void CMeshInterop::CopyMesh(CMesh *handle, CMesh &mesh)
{
	handle->Copy(mesh);
}

bool CMeshInterop::CompareStreams(CMesh *handle, CMesh &mesh)
{
	return handle->CompareStreams(mesh);
}

mono::string CMeshInterop::AppendData(CMesh *handle, CMesh &mesh, bool returnErrorMessage)
{
	auto message = handle->Append(mesh);
	if (returnErrorMessage)
	{
		return ToMonoString(message);
	}
	ReportError(message);
	return nullptr;
}

mono::string CMeshInterop::AppendSpecificData(CMesh *handle, CMesh &mesh, int fromVertex, int vertexCount, int fromFace, int faceCount, bool returnErrorMessage)
{
	auto message = handle->Append(mesh, fromVertex, vertexCount, fromFace, faceCount);
	if (returnErrorMessage)
	{
		return ToMonoString(message);
	}
	ReportError(message);
	return nullptr;
}

void CMeshInterop::RemoveRangeFromStreamInternal(CMesh *handle, int stream, int nFirst, int nCount)
{
	handle->RemoveRangeFromStream(stream, nFirst, nCount);
}

bool CMeshInterop::ValidateInternal(CMesh *handle)
{
	return handle->Validate(nullptr);
}

bool CMeshInterop::ValidateMessage(CMesh *handle, mono::string &ppErrorDescription)
{
	const char *message;
	bool success = handle->Validate(&message);
	ppErrorDescription = ToMonoString(message);
	return success;
}
