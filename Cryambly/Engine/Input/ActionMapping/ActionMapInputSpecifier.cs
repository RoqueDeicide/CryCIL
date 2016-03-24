using System;
using System.Linq;
using static CryCil.MathHelpers;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Base class that defines specification for inputs that can trigger actions.
	/// </summary>
	public abstract class ActionMapInputSpecifier : Attribute
	{
		#region Properties
		/// <summary>
		/// Gets or sets that value that specifies the activation mode of the input.
		/// </summary>
		public ActionActivationMode ActivationMode { get; set; }

		/// <summary>
		/// Gets or sets the value that indicates whether the action is going to block certain inputs or
		/// stop all inputs from being blocked or neither.
		/// </summary>
		public InputBlockMode BlockMode { get; set; }
		/// <summary>
		/// Gets or sets the array of identifiers of inputs that will be blocked for a duration specified by
		/// <see cref="BlockDuration"/> property.
		/// </summary>
		/// <remarks>
		/// If this array is specified by the attribute, then <see cref="BlockMode"/> property must return
		/// <see cref="InputBlockMode.Block"/>.
		/// </remarks>
		public InputId[] BlockedInputs { get; set; }
		/// <summary>
		/// Gets or sets the duration for which the inputs specified by <see cref="BlockedInputs"/> array
		/// will be blocked in seconds.
		/// </summary>
		/// <remarks>
		/// If this value is specified by the attribute, then <see cref="BlockMode"/> property must return
		/// <see cref="InputBlockMode.Block"/>.
		/// </remarks>
		public float BlockDuration { get; set; }
		/// <summary>
		/// Gets or sets the index of the device which inputs must be blocked, if equal to
		/// <see cref="byte.MaxValue"/> all devices of specific type will be blocked.
		/// </summary>
		public byte BlockedDevice { get; set; }

		/// <summary>
		/// Gets or sets the time in seconds that must pass since key was pressed until the action is
		/// triggered via a Press event.
		/// </summary>
		public float PressTriggerDelay { get; set; }
		/// <summary>
		/// When implemented in derived class, indicates whether press trigger delay should be ignored when
		/// repeating the input.
		/// </summary>
		public bool OverridePressTriggerDelayWithRepeat { get; set; }

		/// <summary>
		/// Gets or sets the time threshold in seconds that, if exceeded by the time-span since key press
		/// event till current frame start time, will cause the action to not be triggered on release event.
		/// </summary>
		/// <remarks>Ignored when left at its default value of -1.0f.</remarks>
		public float ReleaseTriggerThreshold { get; set; }
		/// <summary>
		/// Gets or sets the priority of the input when it comes to being delayed.
		/// </summary>
		/// <remarks>
		/// When two inputs are delayed, one with a higher priority will override another.
		/// </remarks>
		public int PressDelayPriority { get; set; }

		/// <summary>
		/// Gets or sets the time in seconds that must pass since key was pressed until the action is
		/// triggered via a Hold event.
		/// </summary>
		public float HoldTriggerDelay { get; set; }
		/// <summary>
		/// Gets or sets the time in seconds that must pass since last Hold event until a new is triggered.
		/// </summary>
		/// <remarks>If this property returns -1.0f, then Hold event is not repeated.</remarks>
		public float HoldTriggerRepeatDelay { get; set; }
		/// <summary>
		/// Gets or sets the time in seconds that must pass since key was pressed until Hold event is
		/// repeated for the last time.
		/// </summary>
		/// <remarks>Ignored when left at its default value of -1.0f.</remarks>
		public float HoldTriggerRepeatDelayOverride { get; set; }

		/// <summary>
		/// Gets or sets the value between -1.0 and 1.0 to compare the analog value with when
		/// <see cref="ActivationMode"/> has <see cref="ActionActivationMode.AnalogCompare"/> flag set.
		/// </summary>
		public float AnalogCompareValue { get; set; }
		/// <summary>
		/// Gets or sets the value that indicates which kind of boolean operation must be used to compare
		/// current analog input with <see cref="AnalogCompareValue"/>.
		/// </summary>
		public AnalogComparisonOperation ComparisonOperation { get; set; }
		#endregion
		#region Interface
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
		public void InheritFrom(ActionMapInputSpecifier other)
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
					this.BlockDuration + other.BlockDuration < 2 * ZeroTolerance // Mathematics...
						? InputBlockMode.None
						: InputBlockMode.Block;
				// If both are blocking inputs, then combine lists of inputs to block.
				if (this.BlockMode == InputBlockMode.Block && other.BlockMode == InputBlockMode.Block)
				{
					this.BlockedInputs =
						(this.BlockedInputs ?? new InputId[] { }).Concat(other.BlockedInputs ?? new InputId[] { }).ToArray();
				}
				// If we still don't have anything to block then don't block anything.
				if (this.BlockedInputs.IsNullOrEmpty())
				{
					this.BlockMode = InputBlockMode.None;
					this.BlockDuration = 0;
				}
			}
			// Inherit everything else that is not specified.
			if (Math.Abs(this.PressTriggerDelay - InputSpecDefaults.DefaultPressTriggerDelay) < ZeroTolerance)
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
			if (Math.Abs(this.ReleaseTriggerThreshold - InputSpecDefaults.DefaultReleaseThreshold) < ZeroTolerance)
			{
				this.ReleaseTriggerThreshold = other.ReleaseTriggerThreshold;
			}
			if (Math.Abs(this.HoldTriggerDelay - InputSpecDefaults.DefaultHoldTriggerDelay) < ZeroTolerance)
			{
				this.HoldTriggerDelay = other.HoldTriggerDelay;
			}
			if (Math.Abs(this.HoldTriggerRepeatDelay - InputSpecDefaults.DefaultHoldRepeatDelay) < ZeroTolerance)
			{
				this.HoldTriggerRepeatDelay = other.HoldTriggerRepeatDelay;
			}
			if (Math.Abs(this.HoldTriggerRepeatDelayOverride - InputSpecDefaults.DefaultOverrideHoldRepeat) < ZeroTolerance)
			{
				this.HoldTriggerRepeatDelayOverride = other.HoldTriggerRepeatDelayOverride;
			}
		}
		#endregion
	}
}