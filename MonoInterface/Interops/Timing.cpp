#include "stdafx.h"

#include "Timing.h"

void TimingInterop::Update()
{
	if (!gEnv || !gEnv->pTimer)
	{
		return;
	}
	
	int64 frameStart   = gEnv->pTimer->GetFrameStartTime(ITimer::ETIMER_GAME).GetValue() * CE_to_Mono_factor;
	int64 frameStartUi = gEnv->pTimer->GetFrameStartTime(ITimer::ETIMER_UI).GetValue() * CE_to_Mono_factor;
	int64 frame        = (int64)(gEnv->pTimer->GetFrameTime()) * Mono_ticks_per_second;
	int64 frameReal    = (int64)(gEnv->pTimer->GetRealFrameTime()) * Mono_ticks_per_second;
	float scale        = gEnv->pTimer->GetTimeScale();
	float frameRate    = gEnv->pTimer->GetFrameRate();
	
	setTimings(frameStart, frameStartUi, frame, frameReal, scale, frameRate);
}

void TimingInterop::OnRunTimeInitialized()
{
	setTimings = (SetTimingsRawThunk)MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())
													  ->GetFunction("SetTimings")->RawThunk;

	REGISTER_METHOD(get_Async);
	REGISTER_METHOD(get_AsyncCurrent);
	REGISTER_METHOD(SetTimeScale);
	REGISTER_METHOD(ClearScaling);
}

int64 TimingInterop::get_Async()
{
	if (gEnv && gEnv->pTimer)
	{
		return gEnv->pTimer->GetAsyncTime().GetValue() * CE_to_Mono_factor;
	}
	return 0;
}

int64 TimingInterop::get_AsyncCurrent()
{
	if (gEnv && gEnv->pTimer)
	{
		return (int64)(gEnv->pTimer->GetAsyncCurTime()) * Mono_ticks_per_second;
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

int64 TimingInterop::CE_to_Mono_factor = 100L;
int64 TimingInterop::Mono_ticks_per_second = 10000000L;
