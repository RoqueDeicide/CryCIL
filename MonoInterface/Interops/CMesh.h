#pragma once

#include "IMonoInterface.h"

struct CMeshInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryMesh"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.StaticObjects"; }

	virtual void InitializeInterops() override;

	static void        *GetStreamPtr(CMesh *handle, int stream, int *pElementCount);
	static void         ReallocateStream(CMesh *handle, int stream, int newCount);
	static int          GetSubsetCount(CMesh *handle);
	static void         SetSubsetCount(CMesh *handle, int count);
	static void         CopyMesh(CMesh *handle, CMesh &mesh);
	static bool         CompareStreams(CMesh *handle, CMesh &mesh);
	static mono::string AppendData(CMesh *handle, CMesh &mesh, bool returnErrorMessage);
	static mono::string AppendSpecificData(CMesh *handle, CMesh &mesh, int fromVertex, int vertexCount, int fromFace,
										   int faceCount, bool returnErrorMessage);
	static void         RemoveRangeFromStreamInternal(CMesh *handle, int stream, int nFirst, int nCount);
	static bool         ValidateInternal(CMesh *handle);
	static bool         ValidateMessage(CMesh *handle, mono::string &ppErrorDescription);
};