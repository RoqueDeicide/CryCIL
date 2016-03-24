using System;
using System.Linq;

namespace CryCil.Engine.Input.ActionMapping
{
	internal static class InputSpecDefaults
	{
		internal const ActionActivationMode DefaultActivationMode = ActionActivationMode.Invalid;
		internal const float DefaultPressTriggerDelay = 0;
		internal const bool DefaultOverridePressDelay = false;
		internal const int DefaultPressDelayPriority = 0;
		internal const float DefaultHoldTriggerDelay = 0;
		internal const float DefaultHoldRepeatDelay = 0;
		internal const float DefaultOverrideHoldRepeat = -1;
		internal const float DefaultReleaseThreshold = -1;
		internal const AnalogComparisonOperation DefaultOperation = AnalogComparisonOperation.None;
		internal const float DefaultAnalogCompareValue = 0;
		internal const InputBlockMode DefaultBlockMode = InputBlockMode.None;
		internal const InputId[] DefaultBlockedInputs = null;
		internal const float DefaultBlockDuration = 0;
		internal const byte DefaultBlockedDevice = byte.MaxValue;
	}
}