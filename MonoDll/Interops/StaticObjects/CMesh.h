#pragma once
#include "Headers\IMonoScriptBind.h"
#include "IIndexedMesh.h"
#include "IStatObj.h"

/**
* Scriptbind for CMesh structure. Allows to work with meshes without copying them to Mono memory.
*/
class Scriptbind_CMesh :
	public IMonoScriptBind
{
public:
	Scriptbind_CMesh();
	~Scriptbind_CMesh();

	virtual const char *GetClassName() { return "MeshInterop"; }

	static int GetFaceCount(CMesh *cMeshHandle);
	static int GetVertexCount(CMesh * cMeshHandle);
	static int GetTexCoordCount(CMesh *cMeshHandle);
	static int GetTangentCount(CMesh *cMeshHandle);
	static int GetSubSetCount(CMesh *cMeshHandle);
	static int GetIndexCount(CMesh *cMeshHandle);
	static void SetFaceCount(CMesh *cMeshHandle, int nNewCount);
	static void SetVertexCount(CMesh *cMeshHandle, int nNewCount);
	static void SetTexCoordsCount(CMesh *cMeshHandle, int nNewCount);
	static void SetTexCoordsAndTangentsCount(CMesh *cMeshHandle, int nNewCount);
	static void SetIndexCount(CMesh *cMeshHandle, int nNewCount);
	static void *GetStreamHandle(CMesh *cMeshHandle, int streamIdentifier);
	static void ReallocateStream(CMesh *cMeshHandle, int streamIdentifier, int newCount);
	static void Export(IStatObj *obj);
	static int GetNumberOfElements(CMesh *cMeshHandle, int streamIdentifier);
};
