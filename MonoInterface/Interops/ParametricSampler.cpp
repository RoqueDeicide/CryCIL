#include "stdafx.h"

#include "ParametricSampler.h"
#include <CryCharAnimationParams.h>

void ParametricSamplerInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetCurrentSegmentIndexBSpace);
}

uint8 ParametricSamplerInterop::GetCurrentSegmentIndexBSpace(SParametricSampler *handle)
{
	return handle->GetCurrentSegmentIndexBSpace();
}
