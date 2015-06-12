#pragma once

struct TimeUtilities
{
	static long long CryEngineTicksToMonoTicks(long long ticks);
	static long long SecondsToMonoTicks(float seconds);
};

inline long long TimeUtilities::CryEngineTicksToMonoTicks(long long ticks)
{
	return ticks * 100L;
}

inline long long TimeUtilities::SecondsToMonoTicks(float seconds)
{
	return (long long)(seconds * 10000000L);
}
