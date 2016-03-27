#pragma once

#include "IMonoInterface.h"
#include <IActionMapManager.h>

struct ActionInputSpecification
{
	EActionActivationMode ActivationMode;
	EActionInputBlockType BlockMode;
	mono::Array BlockedInputs;
	float BlockDuration;
	byte BlockedDevice;
	float PressTriggerDelay;
	bool OverridePressTriggerDelayWithRepeat;
	int PressDelayPriority;
	float ReleaseTriggerThreshold;
	float HoldTriggerDelay;
	float HoldTriggerRepeatDelay;
	float HoldTriggerRepeatDelayOverride;
	float AnalogCompareValue;
	EActionAnalogCompareOperation ComparisonOperation;

	SActionInput CreateActionInput(EKeyId input) const;
};

extern const char *GetInputName(EKeyId input);
extern EActionInputDevice GetDeviceFromInput(EKeyId input);