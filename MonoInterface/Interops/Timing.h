#pragma once

#include "IMonoInterface.h"

typedef void(*SetTimingsRawThunk)(int64, int64, int64, int64, float, float);

struct TimingInterop : public IMonoInterop<false, true>
{
	virtual const char *GetName() override { return "Time"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine"; }

	virtual void Update() override;

	virtual void OnRunTimeInitialized() override;

	static int64 get_Async();
	static int64 get_AsyncCurrent();
	static void SetTimeScale(float value);
	static void ClearScaling();

	static SetTimingsRawThunk setTimings;
};