#pragma once

struct TimeUtilities
{
	static long long TicksCryEngineToToMono(long long ticks);
	static long long TicksToMonoSeconds(float seconds);
};

inline long long TimeUtilities::TicksCryEngineToToMono(long long ticks)
{
	return ticks * 100L;
}

inline long long TimeUtilities::TicksToMonoSeconds(float seconds)
{
	return (long long)(seconds * 10000000L);
}
