#include "stdafx.h"

#include "ParametricSampler.h"
#include <CryCharAnimationParams.h>

void ParametricSamplerInterop::InitializeInterops()
{
	REGISTER_METHOD(GetCurrentSegmentIndexBSpace);
}

uint8 ParametricSamplerInterop::GetCurrentSegmentIndexBSpace(SParametricSampler *handle)
{
	return handle->GetCurrentSegmentIndexBSpace();
}
