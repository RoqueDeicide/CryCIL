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
		/// Finalizes this instance and makes it usable.
		/// </summary>
		public void Complete()
		{
			if (this.ComparisonOperation != AnalogComparisonOperation.None)
			{
				this.ActivationMode |= ActionActivationMode.AnalogCompare;
			}

			if (!this.BlockedInputs.IsNullOrEmpty())
			{
				this.BlockMode = InputBlockMode.Block;
			}
		}
		/// <summary>
		/// Combines this specification with another.
		/// </summary>
		/// <param name="other">Another specification.</param>
		public void InheritFrom(ActionInputSpecification other)
		{
			bool hasAnalogCompare = (this.ActivationMode & ActionActivationMode.AnalogCompare) != 0;

			this.ActivationMode |= other.ActivationMode;
			if (!hasAnalogCompare && (this.ActivationMode & ActionActivationMode.AnalogCompare) != 0)
			{
				this.ComparisonOperation = other.ComparisonOperation;
				this.AnalogCompareValue = other.AnalogCompareValue;
			}

			if (this.BlockMode != InputBlockMode.Clear)
			{
				// If one of these has block duration greater then zero, then block inputs.
				this.BlockMode =
					this.BlockDuration + other.BlockDuration < 2 * MathHelpers.ZeroTolerance // Mathematics...
						? InputBlockMode.None
						: InputBlockMode.Block;
				// If both are blocking inputs, then combine lists of inputs to block.
				if (this.BlockMode == InputBlockMode.Block && other.BlockMode == InputBlockMode.Block)
				{
					this.BlockedInputs =
						(this.BlockedInputs ?? new InputId[] {}).Concat(other.BlockedInputs ?? new InputId[] {}).ToArray();
				}
				// If we still don't have anything to block then don't block anything.
				if (this.BlockedInputs.IsNullOrEmpty())
				{
					this.BlockMode = InputBlockMode.None;
					this.BlockDuration = 0;
				}
			}
			// Inherit everything else that is not specified.
			if (this.PressTriggerDelay == InputSpecDefaults.DefaultPressTriggerDelay)
			{
				this.PressTriggerDelay = other.PressTriggerDelay;
			}
			if (this.OverridePressTriggerDelayWithRepeat == InputSpecDefaults.DefaultOverridePressDelay)
			{
				this.OverridePressTriggerDelayWithRepeat = other.OverridePressTriggerDelayWithRepeat;
			}
			if (this.PressDelayPriority == InputSpecDefaults.DefaultPressDelayPriority)
			{
				this.PressDelayPriority = other.PressDelayPriority;
			}
			if (this.ReleaseTriggerThreshold == InputSpecDefaults.DefaultReleaseThreshold)
			{
				this.ReleaseTriggerThreshold = other.ReleaseTriggerThreshold;
			}
			if (this.HoldTriggerDelay == InputSpecDefaults.DefaultHoldTriggerDelay)
			{
				this.HoldTriggerDelay = other.HoldTriggerDelay;
			}
			if (this.HoldTriggerRepeatDelay == InputSpecDefaults.DefaultHoldRepeatDelay)
			{
				this.HoldTriggerRepeatDelay = other.HoldTriggerRepeatDelay;
			}
			if (this.HoldTriggerRepeatDelayOverride == InputSpecDefaults.DefaultOverrideHoldRepeat)
			{
				this.HoldTriggerRepeatDelayOverride = other.HoldTriggerRepeatDelayOverride;
			}
		}
	}
}