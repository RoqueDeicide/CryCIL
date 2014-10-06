#pragma once
#include "IMonoInterop.h"

#include <ProjectDefines.h>
#include <VertexFormats.h>

class PlatformInterop :
	public IMonoInterop
{
public:
	PlatformInterop()
	{
		REGISTER_METHOD(AreMeshIndicesInt16);
		REGISTER_METHOD(AreMeshTangentsSingle);
		REGISTER_METHOD(AreMeshIndicesInt16);
	}
	~PlatformInterop()
	{}

	virtual const char *GetClassName() { return "PlatformInterop"; }

	static bool AreMeshIndicesInt16()
	{
		return sizeof(vtx_idx) == 16;
	}
	static bool AreMeshTangentsSingle()
	{
#ifdef TANG_FLOATS
		return true;
#else
		return false;
#endif
	}
};
