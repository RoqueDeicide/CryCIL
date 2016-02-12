#pragma once

#include "IMonoInterface.h"

struct MathHelpersInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "MathHelpers"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil"; }

	virtual void OnRunTimeInitialized() override;

	static float RsqrtSingle(float value);
	static double RsqrtDouble(double value);
	static void SinCosSingle(float value, float &sine, float &cosine);
	static void SinCosDouble(double value, double &sine, double &cosine);
	static Vec3 LogQuat(Quat value);
	static Quat ExpVector(Vec3 value);
};