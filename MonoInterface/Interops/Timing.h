#pragma once

#include "IMonoInterface.h"

typedef void(*SetTimingsRawThunk)(int64, int64, int64, int64, float, float);

struct TimingInterop : public IMonoInterop<false, true>
{
	virtual const char *GetName() { return "Time"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine"; }

	virtual void Update();

	virtual void OnRunTimeInitialized();

	static int64 get_Async();
	static int64 get_AsyncCurrent();
	static void SetTimeScale(float value);
	static void ClearScaling();

	static int64 Mono_ticks_per_second;
	static int64 CE_to_Mono_factor;

	static SetTimingsRawThunk setTimings;
};