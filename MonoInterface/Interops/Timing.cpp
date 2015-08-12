#include "stdafx.h"

#include "Timing.h"
#include "TimeUtilities.h"

void TimingInterop::Update()
{
	if (!gEnv || !gEnv->pTimer)
	{
		return;
	}

	auto fs   = gEnv->pTimer->GetFrameStartTime(ITimer::ETIMER_GAME).GetValue();
	auto fsui = gEnv->pTimer->GetFrameStartTime(ITimer::ETIMER_UI).GetValue();
	
	int64 frameStart   = TimeUtilities::CryEngineTicksToMonoTicks(fs);
	int64 frameStartUi = TimeUtilities::CryEngineTicksToMonoTicks(fsui);
	int64 frame        = TimeUtilities::SecondsToMonoTicks(gEnv->pTimer->GetFrameTime());
	int64 frameReal    = TimeUtilities::SecondsToMonoTicks(gEnv->pTimer->GetRealFrameTime());
	float scale        = gEnv->pTimer->GetTimeScale();
	float frameRate    = gEnv->pTimer->GetFrameRate();
	
	setTimings(frameStart, frameStartUi, frame, frameReal, scale, frameRate);
}

void TimingInterop::OnRunTimeInitialized()
{
	setTimings = SetTimingsRawThunk(MonoEnv->Cryambly->GetClass(this->GetInteropNameSpace(), this->GetInteropClassName())
										   ->GetFunction("SetTimings")->RawThunk);

	REGISTER_METHOD(get_Async);
	REGISTER_METHOD(get_AsyncCurrent);
	REGISTER_METHOD(SetTimeScale);
	REGISTER_METHOD(ClearScaling);
}

int64 TimingInterop::get_Async()
{
	if (gEnv && gEnv->pTimer)
	{
		return TimeUtilities::CryEngineTicksToMonoTicks(gEnv->pTimer->GetAsyncTime().GetValue());
	}
	return 0;
}

int64 TimingInterop::get_AsyncCurrent()
{
	if (gEnv && gEnv->pTimer)
	{
		return TimeUtilities::SecondsToMonoTicks(gEnv->pTimer->GetAsyncCurTime());
	}
	return 0;
}

void TimingInterop::SetTimeScale(float value)
{
	if (gEnv && gEnv->pTimer)
	{
		gEnv->pTimer->SetTimeScale(value);
	}
}

void TimingInterop::ClearScaling()
{
	if (gEnv && gEnv->pTimer)
	{
		gEnv->pTimer->ClearTimeScales();
	}
}

SetTimingsRawThunk TimingInterop::setTimings;
