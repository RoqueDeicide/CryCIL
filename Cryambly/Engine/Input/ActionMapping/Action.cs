using System;
using CryCil.Annotations;

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
	/// <summary>
	/// Enumeration of input block modes.
	/// </summary>
	public enum InputBlockMode
	{
		/// <summary>
		/// Nothing is blocked or cleared.
		/// </summary>
		None,
		/// <summary>
		/// Specified inputs will be blocked.
		/// </summary>
		Block,
		/// <summary>
		/// The input will stop all inputs from being blocked.
		/// </summary>
		Clear
	}
	/// <summary>
	/// Enumeration of boolean operations that can be used when comparing analog input.
	/// </summary>
	public enum AnalogComparisonOperation
	{
		/// <summary>
		/// Default value.
		/// </summary>
		None = 0,
		/// <summary>
		/// Equals (==) operation.
		/// </summary>
		Equals,
		/// <summary>
		/// Not equals (!=) operation.
		/// </summary>
		NotEquals,
		/// <summary>
		/// Less then (&gt;) operation.
		/// </summary>
		GreaterThan,
		/// <summary>
		/// Less then (&lt;) operation.
		/// </summary>
		LessThan
	}
	/// <summary>
	/// Base class that defines specification for inputs that can trigger actions.
	/// </summary>
	public interface IInputSpecification
	{
		/// <summary>
		/// When implemented in derived class, gets that value that specifies the activation mode of the
		/// input.
		/// </summary>
		ActionActivationMode ActivationMode { get; }

		/// <summary>
		/// When implemented in derived class, gets the value that indicates whether the action is going to
		/// block certain inputs or stop all inputs from being blocked or neither.
		/// </summary>
		InputBlockMode BlockMode { get; }
		/// <summary>
		/// When implemented in derived class, gets the array of identifiers of inputs that will be blocked
		/// for a duration specified by <see cref="BlockDuration"/> property.
		/// </summary>
		/// <remarks>
		/// If this array is specified by the attribute, then <see cref="BlockMode"/> property must return
		/// <see cref="InputBlockMode.Block"/>.
		/// </remarks>
		InputId[] BlockedInputs { get; }
		/// <summary>
		/// When implemented in derived class, gets the duration for which the inputs specified by
		/// <see cref="BlockedInputs"/> array will be blocked in seconds.
		/// </summary>
		/// <remarks>
		/// If this value is specified by the attribute, then <see cref="BlockMode"/> property must return
		/// <see cref="InputBlockMode.Block"/>.
		/// </remarks>
		float BlockDuration { get; }
		/// <summary>
		/// When implemented in derived class, gets the index of the device which inputs must be blocked,
		/// if equal to <see cref="byte.MaxValue"/> all devices of specific type will be blocked.
		/// </summary>
		byte BlockedDevice { get; }

		/// <summary>
		/// When implemented in derived class, gets the time in seconds that must pass since key was
		/// pressed until the action is triggered via a Press event.
		/// </summary>
		float PressTriggerDelay { get; }
		/// <summary>
		/// When implemented in derived class, indicates whether press trigger delay should be ignored when
		/// repeating the input.
		/// </summary>
		bool OverridePressTriggerDelayWithRepeat { get; }

		/// <summary>
		/// When implemented in derived class, gets the time threshold in seconds that, if exceeded by the
		/// time-span since key press event till current frame start time, will cause the action to not be
		/// triggered on release event.
		/// </summary>
		/// <remarks>Ignored when left at its default value of -1.0f.</remarks>
		float ReleaseTriggerThreshold { get; }
		/// <summary>
		/// When implemented in derived class, gets the priority of the input when it comes to being
		/// delayed.
		/// </summary>
		/// <remarks>
		/// When two inputs are delayed, one with a higher priority will override another.
		/// </remarks>
		int PressDelayPriority { get; }

		/// <summary>
		/// When implemented in derived class, gets the time in seconds that must pass since key was
		/// pressed until the action is triggered via a Hold event.
		/// </summary>
		float HoldTriggerDelay { get; }
		/// <summary>
		/// When implemented in derived class, gets the time in seconds that must pass since last Hold
		/// event until a new is triggered.
		/// </summary>
		/// <remarks>If this property returns -1.0f, then Hold event is not repeated.</remarks>
		float HoldTriggerRepeatDelay { get; }
		/// <summary>
		/// When implemented in derived class, gets the time in seconds that must pass since key was
		/// pressed until Hold event is repeated for the last time.
		/// </summary>
		/// <remarks>Ignored when left at its default value of -1.0f.</remarks>
		float HoldTriggerRepeatDelayOverride { get; }

		/// <summary>
		/// When implemented in derived class, gets the value between -1.0 and 1.0 to compare the analog
		/// value with when <see cref="ActivationMode"/> has
		/// <see cref="ActionActivationMode.AnalogCompare"/> flag set.
		/// </summary>
		float AnalogCompareValue { get; }
		/// <summary>
		/// When implemented in derived class, gets the value that indicates which kind of boolean
		/// operation must be used to compare current analog input with <see cref="AnalogCompareValue"/>.
		/// </summary>
		AnalogComparisonOperation ComparisonOperation { get; }
	}
	/// <summary>
	/// Marks the event that is raised to handle input actions.
	/// </summary>
	/// <remarks>
	/// <para>Important notes:</para>
	/// <para>When declaring an event that will be an action:</para>
	/// <para>
	/// 1) Don't create custom implementation of that event with a custom pair of <c>add</c>/ <c>remove</c>
	///    methods.
	/// </para>
	/// <para>
	/// 2) If you do need that custom implementation, then make sure that you have a static field of type
	///    <see cref="InputActionHandler"/> with the same name as the event but with an underscore '_'
	///    symbol in front of it (e.g. if the event is called "Event", the field must be named "_Event"),
	///    because the value of that field will be taken and invoked every time the action is activated.
	/// </para>
	/// <para>
	/// 3) Internally names of action maps and actions are converted into lowercase, so make sure there are
	/// no case-insensitive conflicts.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code source="ActionMapSample"/>
	/// </example>
	[AttributeUsage(AttributeTargets.Event)]
	[MeansImplicitUse]
	public class ActionAttribute : Attribute, IInputSpecification
	{
		#region Fields
		internal ActionInputSpecification MasterSpec;
		#endregion
		#region Properties
		/// <summary>
		/// Gets custom name of this action map.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets a set of flags that describe how the action can be activated.
		/// </summary>
		/// <remarks>
		/// This value is combined with the value that is returned by
		/// <see cref="InputActionAttribute.ActivationMode"/> property of any attributes that derive from
		/// <see cref="InputActionAttribute"/> that are applied to the same action.
		/// </remarks>
		public ActionActivationMode ActivationMode
		{
			get { return this.MasterSpec.ActivationMode; }
		}
		/// <summary>
		/// Indicates whether this action is going to: 1) Do nothing special. 2) Block specific inputs for
		/// specific amount of time from a specific device. 3) Stop any inputs from being blocked.
		/// </summary>
		public InputBlockMode BlockMode
		{
			get { return this.MasterSpec.BlockMode; }
		}
		/// <summary>
		/// An array of identifiers of inputs that will be blocked when <see cref="BlockMode"/> is set to
		/// <see cref="InputBlockMode.Block"/>.
		/// </summary>
		/// <remarks>
		/// This array should only contain identifiers of inputs that can come from one device type from
		/// <see cref="AnalogComparisonOperation"/> enumeration.
		/// </remarks>
		public InputId[] BlockedInputs
		{
			get { return this.MasterSpec.BlockedInputs; }
		}
		/// <summary>
		/// Amount of time in seconds the inputs specified in <see cref="BlockedInputs"/> will be blocked
		/// for.
		/// </summary>
		public float BlockDuration
		{
			get { return this.MasterSpec.BlockDuration; }
		}
		/// <summary>
		/// A zero-based index of the device (e.g. a controller) to block inputs from, if left at default
		/// value of <see cref="Byte.MaxValue"/> all devices of types specified by
		/// <see cref="BlockedInputs"/> will be blocked.
		/// </summary>
		public byte BlockedDevice
		{
			get { return this.MasterSpec.BlockedDevice; }
		}
		/// <summary>
		/// A time that must pass from the moment the key was pressed until the action is activated via a
		/// Press event.
		/// </summary>
		public float PressTriggerDelay
		{
			get { return this.MasterSpec.PressTriggerDelay; }
		}
		/// <summary>
		/// Gets the priority of the input when it comes to being delayed.
		/// </summary>
		/// <remarks>
		/// When two inputs are delayed, one with a higher priority will override another.
		/// </remarks>
		public int PressDelayPriority
		{
			get { return this.MasterSpec.PressDelayPriority; }
		}
		/// <summary>
		/// Indicates whether <see cref="PressTriggerDelay"/> can be overridden by repeating the input.
		/// </summary>
		public bool OverridePressTriggerDelayWithRepeat
		{
			get { return this.MasterSpec.OverridePressTriggerDelayWithRepeat; }
		}
		/// <summary>
		/// A time that must pass since the key was pressed until key is released to stop Release event
		/// from activating the action. When left at default value of -1.0 it is ignored.
		/// </summary>
		public float ReleaseTriggerThreshold
		{
			get { return this.MasterSpec.ReleaseTriggerThreshold; }
		}
		/// <summary>
		/// A time that must pass from the moment the key was pressed until the action is activated via a
		/// Hold event.
		/// </summary>
		public float HoldTriggerDelay
		{
			get { return this.MasterSpec.HoldTriggerDelay; }
		}
		/// <summary>
		/// A time that must pass from the moment the lest Hold event has activated the action until the
		/// action is activated again via a Hold event.
		/// </summary>
		public float HoldTriggerRepeatDelay
		{
			get { return this.MasterSpec.HoldTriggerRepeatDelay; }
		}
		/// <summary>
		/// A time that must pass from the moment the key was pressed until the action is activated via a
		/// Hold event for the last time until the key is pressed again.
		/// </summary>
		public float HoldTriggerRepeatDelayOverride
		{
			get { return this.MasterSpec.HoldTriggerRepeatDelayOverride; }
		}
		/// <summary>
		/// A boolean operation to use to compare analog input to <see cref="AnalogCompareValue"/>. Analog
		/// input can only activate the action when this.MasterSpec comparison succeeds (returns
		/// <c>true</c>). If left at its default value of <see cref="AnalogComparisonOperation.None"/>, no
		/// comparison is done.
		/// </summary>
		public AnalogComparisonOperation ComparisonOperation
		{
			get { return this.MasterSpec.ComparisonOperation; }
		}
		/// <summary>
		/// A value to compare analog input to using <see cref="ComparisonOperation"/>.
		/// </summary>
		public float AnalogCompareValue
		{
			get { return this.MasterSpec.AnalogCompareValue; }
		}
		/// <summary>
		/// An optional value that represents the input from keyboard/mouse input device that can activate
		/// the action.
		/// </summary>
		/// <remarks>
		/// Specified value must be greater then or equal to <see cref="InputIdBase.Keyboard"/> and less
		/// then <see cref="InputIdBase.XboxController"/>, otherwise the error will occur (In DEBUG
		/// configuration) and the value will be ignored.
		/// </remarks>
		public InputId KeyboardMouseInput { get; set; }
		/// <summary>
		/// An optional value that represents the input from Xbox controller that can activate the action.
		/// </summary>
		/// <remarks>
		/// Specified value must be greater then or equal to <see cref="InputIdBase.XboxController"/> and
		/// less then <see cref="InputIdBase.OrbisController"/>, otherwise the error will occur (In DEBUG
		/// configuration) and the value will be ignored.
		/// </remarks>
		public InputId XboxInput { get; set; }
		/// <summary>
		/// An optional value that represents the input from Orbis-compatible input device (e.g. Dualshock
		/// 4) that can activate the action.
		/// </summary>
		/// <remarks>
		/// Specified value must be greater then or equal to <see cref="InputIdBase.OrbisController"/> and
		/// less then <see cref="InputIdBase.SystemInput"/>, otherwise the error will occur (In DEBUG
		/// configuration) and the value will be ignored.
		/// </remarks>
		public InputId OrbisInput { get; set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates an attribute that provides common input specification for an action with an option to
		/// clear the blocking inputs.
		/// </summary>
		/// <param name="name">              
		/// An optional name for the action, must not contain characters that are not valid in Xml tags. A
		/// name of the event will be used as a name if this parameter is left at <c>null</c>.
		/// </param>
		/// <param name="activationMode">    
		/// A optional set of flags that specifies how this action can be activated.
		/// </param>
		/// <param name="pressTriggerDelay"> 
		/// An optional value that can be used to initialize backing field for
		/// <see cref="PressTriggerDelay"/>
		/// </param>
		/// <param name="overridePressDelay">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="OverridePressTriggerDelayWithRepeat"/>
		/// </param>
		/// <param name="pressDelayPriority">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="PressDelayPriority"/>
		/// </param>
		/// <param name="holdTriggerDelay">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="HoldTriggerDelay"/>
		/// </param>
		/// <param name="holdRepeatDelay">   
		/// An optional value that can be used to initialize backing field for
		/// <see cref="HoldTriggerRepeatDelay"/>
		/// </param>
		/// <param name="overrideHoldRepeat">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="HoldTriggerRepeatDelayOverride"/>
		/// </param>
		/// <param name="releaseThreshold">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ReleaseTriggerThreshold"/>
		/// </param>
		/// <param name="op">                
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ComparisonOperation"/>
		/// </param>
		/// <param name="analogCompareValue">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="AnalogCompareValue"/>
		/// </param>
		/// <param name="blockMode">         
		/// An optional value that can be used to initialize backing field for <see cref="BlockMode"/>.
		/// </param>
		/// <param name="blockedInputs">     
		/// An optional value that can be used to initialize backing field for <see cref="BlockedInputs"/>.
		/// </param>
		/// <param name="blockDuration">     
		/// An optional value that can be used to initialize backing field for <see cref="BlockDuration"/>.
		/// </param>
		/// <param name="blockedDevice">     
		/// An optional value that can be used to initialize backing field for <see cref="BlockedDevice"/>.
		/// </param>
		public ActionAttribute(
			string name = null,
			ActionActivationMode activationMode = InputSpecDefaults.DefaultActivationMode,
			float pressTriggerDelay = InputSpecDefaults.DefaultPressTriggerDelay,
			bool overridePressDelay = InputSpecDefaults.DefaultOverridePressDelay,
			int pressDelayPriority = InputSpecDefaults.DefaultPressDelayPriority,
			float holdTriggerDelay = InputSpecDefaults.DefaultHoldTriggerDelay,
			float holdRepeatDelay = InputSpecDefaults.DefaultHoldRepeatDelay,
			float overrideHoldRepeat = InputSpecDefaults.DefaultOverrideHoldRepeat,
			float releaseThreshold = InputSpecDefaults.DefaultReleaseThreshold,
			AnalogComparisonOperation op = InputSpecDefaults.DefaultOperation,
			float analogCompareValue = InputSpecDefaults.DefaultAnalogCompareValue,
			InputBlockMode blockMode = InputSpecDefaults.DefaultBlockMode,
			InputId[] blockedInputs = InputSpecDefaults.DefaultBlockedInputs,
			float blockDuration = InputSpecDefaults.DefaultBlockDuration,
			byte blockedDevice = InputSpecDefaults.DefaultBlockedDevice)
		{
			this.Name = name;
			this.MasterSpec.ActivationMode = activationMode;
			this.MasterSpec.PressTriggerDelay = pressTriggerDelay;
			this.MasterSpec.OverridePressTriggerDelayWithRepeat = overridePressDelay;
			this.MasterSpec.HoldTriggerDelay = holdTriggerDelay;
			this.MasterSpec.HoldTriggerRepeatDelay = holdRepeatDelay;
			this.MasterSpec.HoldTriggerRepeatDelayOverride = overrideHoldRepeat;
			this.MasterSpec.ReleaseTriggerThreshold = releaseThreshold;
			this.MasterSpec.PressDelayPriority = pressDelayPriority;
			this.MasterSpec.ComparisonOperation = op;
			this.MasterSpec.AnalogCompareValue = analogCompareValue;
			this.MasterSpec.BlockMode = blockMode;
			this.MasterSpec.BlockedInputs = blockedInputs;
			this.MasterSpec.BlockDuration = blockDuration;
			this.MasterSpec.BlockedDevice = blockedDevice;

			this.KeyboardMouseInput = InputId.Unknown;
			this.XboxInput = InputId.Unknown;
			this.OrbisInput = InputId.Unknown;
		}
		#endregion
	}
	/// <summary>
	/// Base class for attributes that provide information about inputs that can activate actions.
	/// </summary>
	[AttributeUsage(AttributeTargets.Event, AllowMultiple = true)]
	public class InputActionAttribute : Attribute, IInputSpecification
	{
		#region Fields
		internal ActionInputSpecification ExtraSpec;
		#endregion
		#region Properties
		/// <summary>
		/// Gets identifier of the input that can trigger the action.
		/// </summary>
		public InputId Input { get; private set; }
		/// <summary>
		/// Gets a set of flags that describe how the action can be activated.
		/// </summary>
		/// <remarks>
		/// This value is combined with the value that is returned by
		/// <see cref="ActionAttribute.ActivationMode"/> property of the attribute that defines this
		/// action.
		/// </remarks>
		public ActionActivationMode ActivationMode
		{
			get { return this.ExtraSpec.ActivationMode; }
		}
		/// <summary>
		/// Indicates whether activation of the action via this input is going to: 1) Do nothing special.
		/// 2) Block specific inputs for specific amount of time from a specific device. 3) Stop any inputs
		/// from being blocked.
		/// </summary>
		public InputBlockMode BlockMode
		{
			get { return this.ExtraSpec.BlockMode; }
		}
		/// <summary>
		/// An array of identifiers of inputs that will be blocked when <see cref="BlockMode"/> is set to
		/// <see cref="InputBlockMode.Block"/>.
		/// </summary>
		/// <remarks>
		/// This array should only contain identifiers of inputs that can come from one device type from
		/// <see cref="AnalogComparisonOperation"/> enumeration.
		/// </remarks>
		public InputId[] BlockedInputs
		{
			get { return this.ExtraSpec.BlockedInputs; }
		}
		/// <summary>
		/// Amount of time in seconds the inputs specified in <see cref="BlockedInputs"/> will be blocked
		/// for.
		/// </summary>
		public float BlockDuration
		{
			get { return this.ExtraSpec.BlockDuration; }
		}
		/// <summary>
		/// A zero-based index of the device (e.g. a controller) to block inputs from, if left at default
		/// value of <see cref="Byte.MaxValue"/> all devices of type specified by
		/// <see cref="BlockedInputs"/> will be blocked.
		/// </summary>
		public byte BlockedDevice
		{
			get { return this.ExtraSpec.BlockedDevice; }
		}
		/// <summary>
		/// A time that must pass from the moment the key was pressed until the action is activated via a
		/// Press event.
		/// </summary>
		public float PressTriggerDelay
		{
			get { return this.ExtraSpec.PressTriggerDelay; }
		}
		/// <summary>
		/// Gets the priority of the input when it comes to being delayed.
		/// </summary>
		/// <remarks>
		/// When two inputs are delayed, one with a higher priority will override another.
		/// </remarks>
		public int PressDelayPriority
		{
			get { return this.ExtraSpec.PressDelayPriority; }
		}
		/// <summary>
		/// Indicates whether <see cref="PressTriggerDelay"/> can be overridden by repeating the input.
		/// </summary>
		public bool OverridePressTriggerDelayWithRepeat
		{
			get { return this.ExtraSpec.OverridePressTriggerDelayWithRepeat; }
		}
		/// <summary>
		/// A time that must pass since the key was pressed until key is released to stop Release event
		/// from activating the action. When left at default value of -1.0 it is ignored.
		/// </summary>
		public float ReleaseTriggerThreshold
		{
			get { return this.ExtraSpec.ReleaseTriggerThreshold; }
		}
		/// <summary>
		/// A time that must pass from the moment the key was pressed until the action is activated via a
		/// Hold event.
		/// </summary>
		public float HoldTriggerDelay
		{
			get { return this.ExtraSpec.HoldTriggerDelay; }
		}
		/// <summary>
		/// A time that must pass from the moment the lest Hold event has activated the action until the
		/// action is activated again via a Hold event.
		/// </summary>
		public float HoldTriggerRepeatDelay
		{
			get { return this.ExtraSpec.HoldTriggerRepeatDelay; }
		}
		/// <summary>
		/// A time that must pass from the moment the key was pressed until the action is activated via a
		/// Hold event for the last time until the key is pressed again.
		/// </summary>
		public float HoldTriggerRepeatDelayOverride
		{
			get { return this.ExtraSpec.HoldTriggerRepeatDelayOverride; }
		}
		/// <summary>
		/// A boolean operation to use to compare analog input to <see cref="AnalogCompareValue"/>. Analog
		/// input can only activate the action when this comparison succeeds (returns <c>true</c>). If left
		/// at its default value of <see cref="AnalogComparisonOperation.None"/>, no comparison is done.
		/// </summary>
		public AnalogComparisonOperation ComparisonOperation
		{
			get { return this.ExtraSpec.ComparisonOperation; }
		}
		/// <summary>
		/// A value to compare analog input to using <see cref="ComparisonOperation"/>.
		/// </summary>
		public float AnalogCompareValue
		{
			get { return this.ExtraSpec.AnalogCompareValue; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates an attribute that provides common input specification for an action with an option to
		/// clear the blocking inputs.
		/// </summary>
		/// <param name="input">             Identifier of the input.</param>
		/// <param name="activationMode">    
		/// A set of flags that specifies how this action can be activated.
		/// </param>
		/// <param name="pressTriggerDelay"> 
		/// An optional value that can be used to initialize backing field for
		/// <see cref="PressTriggerDelay"/>
		/// </param>
		/// <param name="overridePressDelay">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="OverridePressTriggerDelayWithRepeat"/>
		/// </param>
		/// <param name="pressDelayPriority">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="PressDelayPriority"/>
		/// </param>
		/// <param name="holdTriggerDelay">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="HoldTriggerDelay"/>
		/// </param>
		/// <param name="holdRepeatDelay">   
		/// An optional value that can be used to initialize backing field for
		/// <see cref="HoldTriggerRepeatDelay"/>
		/// </param>
		/// <param name="overrideHoldRepeat">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="HoldTriggerRepeatDelayOverride"/>
		/// </param>
		/// <param name="releaseThreshold">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ReleaseTriggerThreshold"/>
		/// </param>
		/// <param name="op">                
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ComparisonOperation"/>
		/// </param>
		/// <param name="analogCompareValue">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="AnalogCompareValue"/>
		/// </param>
		/// <param name="blockMode">         
		/// An optional value that can be used to initialize backing field for <see cref="BlockMode"/>.
		/// </param>
		/// <param name="blockedInputs">     
		/// An optional value that can be used to initialize backing field for <see cref="BlockedInputs"/>.
		/// </param>
		/// <param name="blockDuration">     
		/// An optional value that can be used to initialize backing field for <see cref="BlockDuration"/>.
		/// </param>
		/// <param name="blockedDevice">     
		/// An optional value that can be used to initialize backing field for <see cref="BlockedDevice"/>.
		/// </param>
		public InputActionAttribute(InputId input,
									ActionActivationMode activationMode = InputSpecDefaults.DefaultActivationMode,
									float pressTriggerDelay = InputSpecDefaults.DefaultPressTriggerDelay,
									bool overridePressDelay = InputSpecDefaults.DefaultOverridePressDelay,
									int pressDelayPriority = InputSpecDefaults.DefaultPressDelayPriority,
									float holdTriggerDelay = InputSpecDefaults.DefaultHoldTriggerDelay,
									float holdRepeatDelay = InputSpecDefaults.DefaultHoldRepeatDelay,
									float overrideHoldRepeat = InputSpecDefaults.DefaultOverrideHoldRepeat,
									float releaseThreshold = InputSpecDefaults.DefaultReleaseThreshold,
									AnalogComparisonOperation op = InputSpecDefaults.DefaultOperation,
									float analogCompareValue = InputSpecDefaults.DefaultAnalogCompareValue,
									InputBlockMode blockMode = InputSpecDefaults.DefaultBlockMode,
									InputId[] blockedInputs = InputSpecDefaults.DefaultBlockedInputs,
									float blockDuration = InputSpecDefaults.DefaultBlockDuration,
									byte blockedDevice = InputSpecDefaults.DefaultBlockedDevice)
		{
			this.Input = input;
			this.ExtraSpec.ActivationMode = activationMode;
			this.ExtraSpec.PressTriggerDelay = pressTriggerDelay;
			this.ExtraSpec.OverridePressTriggerDelayWithRepeat = overridePressDelay;
			this.ExtraSpec.HoldTriggerDelay = holdTriggerDelay;
			this.ExtraSpec.HoldTriggerRepeatDelay = holdRepeatDelay;
			this.ExtraSpec.HoldTriggerRepeatDelayOverride = overrideHoldRepeat;
			this.ExtraSpec.ReleaseTriggerThreshold = releaseThreshold;
			this.ExtraSpec.PressDelayPriority = pressDelayPriority;
			this.ExtraSpec.ComparisonOperation = op;
			this.ExtraSpec.AnalogCompareValue = analogCompareValue;
			this.ExtraSpec.BlockMode = blockMode;
			this.ExtraSpec.BlockedInputs = blockedInputs;
			this.ExtraSpec.BlockDuration = blockDuration;
			this.ExtraSpec.BlockedDevice = blockedDevice;
		}
		#endregion
	}
}