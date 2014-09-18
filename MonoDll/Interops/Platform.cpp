#include "stdafx.h"
#include "Platform.h"
#include "ProjectDefines.h"
#include "VertexFormats.h"

PlatformInterop::PlatformInterop()
{}

PlatformInterop::~PlatformInterop()
{}

bool PlatformInterop::AreMeshIndicesInt16()
{
	return sizeof(vtx_idx) == 16;
}
bool PlatformInterop::AreMeshTangentsSingle()
{
#ifdef TANG_FLOATS
	return true;
#else
	return false;
#endif
}