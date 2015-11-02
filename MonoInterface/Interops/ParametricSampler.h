#pragma once

#include "IMonoInterface.h"

struct SParametricSampler;

struct ParametricSamplerInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "ParametricSampler"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void OnRunTimeInitialized() override;

	static uint8 GetCurrentSegmentIndexBSpace(SParametricSampler *handle);
};