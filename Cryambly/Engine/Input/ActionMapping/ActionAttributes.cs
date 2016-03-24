using System;
using System.Linq;
using CryCil.Annotations;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Marks the event that is raised to handle input actions.
	/// </summary>
	/// <remarks>
	/// <para>Important notes:</para>
	/// <para>When declaring an event that will be an action:</para>
	/// <list type="number">
	/// <item>
	/// Don't create custom implementation of that event with a custom pair of <c>add</c>/ <c>remove</c>
	/// methods.
	/// </item>
	/// <item>
	/// If you do need that custom implementation, then make sure that you have a static field of type
	/// <see cref="InputActionHandler"/> with the same name as the event but with an underscore '_' symbol
	/// in front of it (e.g. if the event is called "Event", the field must be named "_Event"), because the
	/// value of that field will be taken and invoked every time the action is activated.
	/// </item>
	/// <item>
	/// Internally names of action maps and actions are converted into lowercase, so make sure there are no
	/// case-insensitive conflicts.
	/// </item>
	/// </list>
	/// </remarks>
	/// <example>
	/// <code source="ActionMaps/Sample.cs"/>
	/// </example>
	[AttributeUsage(AttributeTargets.Event)]
	[MeansImplicitUse]
	public class ActionAttribute : ActionMapInputSpecifier
	{
		#region Properties
		/// <summary>
		/// Gets custom name of this action map.
		/// </summary>
		public string Name { get; private set; }
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
		/// <see cref="ActionMapInputSpecifier.PressTriggerDelay"/>
		/// </param>
		/// <param name="overridePressDelay">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.OverridePressTriggerDelayWithRepeat"/>
		/// </param>
		/// <param name="pressDelayPriority">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.PressDelayPriority"/>
		/// </param>
		/// <param name="holdTriggerDelay">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.HoldTriggerDelay"/>
		/// </param>
		/// <param name="holdRepeatDelay">   
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.HoldTriggerRepeatDelay"/>
		/// </param>
		/// <param name="overrideHoldRepeat">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.HoldTriggerRepeatDelayOverride"/>
		/// </param>
		/// <param name="releaseThreshold">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.ReleaseTriggerThreshold"/>
		/// </param>
		/// <param name="op">                
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.ComparisonOperation"/>
		/// </param>
		/// <param name="analogCompareValue">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.AnalogCompareValue"/>
		/// </param>
		/// <param name="blockMode">         
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockMode"/>.
		/// </param>
		/// <param name="blockedInputs">     
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockedInputs"/>.
		/// </param>
		/// <param name="blockDuration">     
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockDuration"/>.
		/// </param>
		/// <param name="blockedDevice">     
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockedDevice"/>.
		/// </param>
		public ActionAttribute(string name = null,
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
			this.ActivationMode = activationMode;
			this.PressTriggerDelay = pressTriggerDelay;
			this.OverridePressTriggerDelayWithRepeat = overridePressDelay;
			this.HoldTriggerDelay = holdTriggerDelay;
			this.HoldTriggerRepeatDelay = holdRepeatDelay;
			this.HoldTriggerRepeatDelayOverride = overrideHoldRepeat;
			this.ReleaseTriggerThreshold = releaseThreshold;
			this.PressDelayPriority = pressDelayPriority;
			this.ComparisonOperation = op;
			this.AnalogCompareValue = analogCompareValue;
			this.BlockMode = blockMode;
			this.BlockedInputs = blockedInputs;
			this.BlockDuration = blockDuration;
			this.BlockedDevice = blockedDevice;

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
	public class InputActionAttribute : ActionMapInputSpecifier
	{
		#region Properties
		/// <summary>
		/// Gets identifier of the input that can trigger the action.
		/// </summary>
		public InputId Input { get; private set; }
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
		/// <see cref="ActionMapInputSpecifier.PressTriggerDelay"/>
		/// </param>
		/// <param name="overridePressDelay">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.OverridePressTriggerDelayWithRepeat"/>
		/// </param>
		/// <param name="pressDelayPriority">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.PressDelayPriority"/>
		/// </param>
		/// <param name="holdTriggerDelay">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.HoldTriggerDelay"/>
		/// </param>
		/// <param name="holdRepeatDelay">   
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.HoldTriggerRepeatDelay"/>
		/// </param>
		/// <param name="overrideHoldRepeat">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.HoldTriggerRepeatDelayOverride"/>
		/// </param>
		/// <param name="releaseThreshold">  
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.ReleaseTriggerThreshold"/>
		/// </param>
		/// <param name="op">                
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.ComparisonOperation"/>
		/// </param>
		/// <param name="analogCompareValue">
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.AnalogCompareValue"/>
		/// </param>
		/// <param name="blockMode">         
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockMode"/>.
		/// </param>
		/// <param name="blockedInputs">     
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockedInputs"/>.
		/// </param>
		/// <param name="blockDuration">     
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockDuration"/>.
		/// </param>
		/// <param name="blockedDevice">     
		/// An optional value that can be used to initialize backing field for
		/// <see cref="ActionMapInputSpecifier.BlockedDevice"/>.
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
			this.ActivationMode = activationMode;
			this.PressTriggerDelay = pressTriggerDelay;
			this.OverridePressTriggerDelayWithRepeat = overridePressDelay;
			this.HoldTriggerDelay = holdTriggerDelay;
			this.HoldTriggerRepeatDelay = holdRepeatDelay;
			this.HoldTriggerRepeatDelayOverride = overrideHoldRepeat;
			this.ReleaseTriggerThreshold = releaseThreshold;
			this.PressDelayPriority = pressDelayPriority;
			this.ComparisonOperation = op;
			this.AnalogCompareValue = analogCompareValue;
			this.BlockMode = blockMode;
			this.BlockedInputs = blockedInputs;
			this.BlockDuration = blockDuration;
			this.BlockedDevice = blockedDevice;
		}
		#endregion
	}
}