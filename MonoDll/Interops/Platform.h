#pragma once
#include "IMonoInterop.h"
class PlatformInterop :
	public IMonoInterop
{
public:
	PlatformInterop();
	~PlatformInterop();

	virtual const char *GetClassName() { return "PlatformInterop"; }

	static bool AreMeshIndicesInt16();
	static bool AreMeshTangentsSingle();
};
