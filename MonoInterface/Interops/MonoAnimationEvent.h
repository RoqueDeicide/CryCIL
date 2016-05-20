#pragma once

#include "IMonoInterface.h"

struct AnimEventInstance;

struct MonoAnimationEvent
{
	float time;
	float endTime;
	uint eventNameLowercaseCrc32;
	mono::string eventName;
	mono::string parameter;
	mono::string boneName;
	Vec3 offset;
	Vec3 direction;

	explicit MonoAnimationEvent(const AnimEventInstance &eventInstance);
};
