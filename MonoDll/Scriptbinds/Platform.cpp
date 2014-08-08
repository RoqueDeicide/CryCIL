#include "stdafx.h"
#include "Platform.h"
#include "ProjectDefines.h"
#include "VertexFormats.h"

Scriptbind_Platform::Scriptbind_Platform()
{}

Scriptbind_Platform::~Scriptbind_Platform()
{}

bool Scriptbind_Platform::AreMeshIndicesInt16()
{
	return sizeof(vtx_idx) == 16;
}
bool Scriptbind_Platform::AreMeshTangentsSingle()
{
#ifdef TANG_FLOATS
	return true;
#else
	return false;
#endif
}