#include "stdafx.h"

#include "ActionInputSpecification.h"

const char *GetInputName(EKeyId input)
{
	if (input == eKI_XI_Disconnect)
	{
		return "disconnect";
	}
	return gEnv->pInput->LookupSymbol(eIDT_Unknown, -1, input)->name.c_str();
}

EActionInputDevice GetDeviceFromInput(EKeyId input)
{
	if (input < KI_XINPUT_BASE)
	{
		return eAID_KeyboardMouse;
	}
	if (input < KI_ORBIS_BASE)
	{
		return eAID_XboxPad;
	}
	return eAID_PS4Pad;
}

SActionInput ActionInputSpecification::CreateActionInput(EKeyId input) const
{
	SActionInput actionInput;

	actionInput.activationMode = this->ActivationMode;
	actionInput.fPressTriggerDelay = this->PressTriggerDelay;
	actionInput.fPressTriggerDelayRepeatOverride = this->OverridePressTriggerDelayWithRepeat ? 1.0f : -1.0f;
	actionInput.fHoldTriggerDelay = this->HoldTriggerDelay;
	actionInput.fHoldRepeatDelay = this->HoldTriggerRepeatDelay;
	actionInput.fHoldTriggerDelayRepeatOverride = this->HoldTriggerRepeatDelayOverride;
	actionInput.fReleaseTriggerThreshold = this->ReleaseTriggerThreshold;
	actionInput.iPressDelayPriority = this->PressDelayPriority;

	const char *inputName = GetInputName(input);
	actionInput.input = inputName;
	actionInput.defaultInput = inputName;
	actionInput.inputDevice = GetDeviceFromInput(input);

	if (this->ComparisonOperation != eAACO_None)
	{
		actionInput.analogCompareOp = this->ComparisonOperation;
		actionInput.fAnalogCompareVal = this->AnalogCompareValue;
	}

	SActionInputBlockData blockData;
	blockData.blockType = this->BlockMode;
	if (blockData.blockType == eAIBT_BlockInputs)
	{
		// Transfer all blocked inputs into block data object.
		IMonoArray<EKeyId> blockedInputs = this->BlockedInputs;

		int blockedInputsCount = blockedInputs.Length;
		for (int i = 0; i < blockedInputsCount; i++)
		{
			SActionInputBlocker blocker;
			blocker.keyId = blockedInputs[i];
			blockData.inputs.push_back(blocker);
		}
	}
	actionInput.inputBlockData = blockData;

	return actionInput;
}
