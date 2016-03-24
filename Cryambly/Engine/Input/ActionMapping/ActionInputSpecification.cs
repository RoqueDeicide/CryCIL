using System;
using System.Linq;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Encapsulates the data that specify one of the action inputs.
	/// </summary>
	public struct ActionInputSpecification
	{
		/// <summary>
		/// Specifies how the action is activated.
		/// </summary>
		public ActionActivationMode ActivationMode;
		/// <summary>
		/// Specifies relations of the action with other inputs in terms of blocking them.
		/// </summary>
		public InputBlockMode BlockMode;
		/// <summary>
		/// An array of inputs that should be blocked.
		/// </summary>
		public InputId[] BlockedInputs;
		/// <summary>
		/// Duration of input block.
		/// </summary>
		public float BlockDuration;
		/// <summary>
		/// Index of the blocked device.
		/// </summary>
		public byte BlockedDevice;
		/// <summary>
		/// Delay before the action is activated by a Press event.
		/// </summary>
		public float PressTriggerDelay;
		/// <summary>
		/// Indicates whether repeating the input will override repetition of Press events.
		/// </summary>
		public bool OverridePressTriggerDelayWithRepeat;
		/// <summary>
		/// When two inputs are delayed, one with a higher priority will override another.
		/// </summary>
		public int PressDelayPriority;
		/// <summary>
		/// Delay before Release event is not fired on release of the key.
		/// </summary>
		public float ReleaseTriggerThreshold;
		/// <summary>
		/// Delay before the action is activated by a Hold event.
		/// </summary>
		public float HoldTriggerDelay;
		/// <summary>
		/// Delay before the action is activated by a repeated Hold event.
		/// </summary>
		public float HoldTriggerRepeatDelay;
		/// <summary>
		/// Delay repetition of Hold events stops.
		/// </summary>
		public float HoldTriggerRepeatDelayOverride;
		/// <summary>
		/// Value to compare analog input to.
		/// </summary>
		public float AnalogCompareValue;
		/// <summary>
		/// Boolean operation to use when comparing <see cref="AnalogCompareValue"/> to the current analog
		/// input.
		/// </summary>
		public AnalogComparisonOperation ComparisonOperation;
		/// <summary>
		/// Creates an object that contains input specification from an attribute.
		/// </summary>
		/// <param name="specifier">An attribute to extract input specification information from.</param>
		public static implicit operator ActionInputSpecification(ActionMapInputSpecifier specifier)
		{
			return new ActionInputSpecification
			{
				ActivationMode = specifier.ActivationMode,
				AnalogCompareValue = specifier.AnalogCompareValue,
				BlockDuration = specifier.BlockDuration,
				BlockedDevice = specifier.BlockedDevice,
				BlockedInputs = specifier.BlockedInputs,
				BlockMode = specifier.BlockMode,
				ComparisonOperation = specifier.ComparisonOperation,
				HoldTriggerDelay = specifier.HoldTriggerDelay,
				HoldTriggerRepeatDelay = specifier.HoldTriggerRepeatDelay,
				HoldTriggerRepeatDelayOverride = specifier.HoldTriggerRepeatDelayOverride,
				OverridePressTriggerDelayWithRepeat = specifier.OverridePressTriggerDelayWithRepeat,
				PressDelayPriority = specifier.PressDelayPriority,
				PressTriggerDelay = specifier.PressTriggerDelay,
				ReleaseTriggerThreshold = specifier.ReleaseTriggerThreshold
			};
		}
	}
}