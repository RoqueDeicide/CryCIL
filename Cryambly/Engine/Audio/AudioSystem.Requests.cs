using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.DebugServices;
using CryCil.RunTime.Registration;

namespace CryCil.Engine.Audio
{
	public static partial class AudioSystem
	{
		#region Fields
		private static readonly List<AudioSystemImplementation> loadedImplementations =
			new List<AudioSystemImplementation>();
		#endregion
		#region Properties
		/// <summary>
		/// Sets the location of the audio listener.
		/// </summary>
		/// <remarks>Audio listener is a player (in a simplest case).</remarks>
		/// <exception cref="ArgumentException">
		/// Matrix that represents the new location of the audio listener must be valid.
		/// </exception>
		public static Matrix34 ListenerLocation
		{
			set
			{
				if (!value.IsValid)
				{
					throw new ArgumentException("Matrix that represents the new location of the audio listener must be valid.");
				}

				RequestSetPosition(ref value);
			}
		}
		/// <summary>
		/// Changes the master volume.
		/// </summary>
		/// <value>
		/// The value is a value between 0 and 1 (presumably) where 0 means muting the sound and 1 means
		/// full sound volume.
		/// </value>
		public static float Volume
		{
			set
			{
				value = MathHelpers.Clamp(value, 0, 1);

				RequestSetVolume(value);
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Switches audio system to a different implementation.
		/// </summary>
		/// <param name="type">Type of implementation.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="type"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// Given type is not a registered implementation of the audio system.
		/// </exception>
		public static void SetAudioSystemImplementation(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!AudioSystemImplementations.RegisteredImplementations.ContainsKey(type.Name))
			{
				throw new NotSupportedException(type.FullName + " not a registered implementation of the audio system.");
			}

			var loadedImplementation = loadedImplementations.Find(implementation => implementation.GetType() == type);

			if (loadedImplementation == null)
			{
				// ReSharper disable once PossibleNullReferenceException

				// ReSharper disable ExceptionNotDocumented
				loadedImplementation = type.GetConstructor(Type.EmptyTypes).Invoke(null) as AudioSystemImplementation;
				// ReSharper restore ExceptionNotDocumented

				if (loadedImplementation == null)
				{
					Log.Error(true, "Failed to create managed object of type {0}.", type.FullName);
					return;
				}

				loadedImplementation.Handle = CreateNativeImplementationObject(loadedImplementation);

				loadedImplementations.Add(loadedImplementation);
			}

			RequestSetImpl(loadedImplementation.Handle);
		}
		/// <summary>
		/// Switches audio system to a different implementation. Unlike
		/// <see cref="M:CryCil.Engine.Audio.AudioSystem.SetAudioSystemImplementation(System.Type)"/> this
		/// overload can be used to set native implementations.
		/// </summary>
		/// <param name="name">Name of the implementation to set.</param>
		/// <exception cref="ArgumentNullException">
		/// Name of the audio system implementation cannot be null.
		/// </exception>
		public static void SetAudioSystemImplementation(string name)
		{
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name", "Name of the audio system implementation cannot be null.");
			}

			Type registeredType;
			if (AudioSystemImplementations.RegisteredImplementations.TryGetValue(name, out registeredType))
			{
				SetAudioSystemImplementation(registeredType);
			}

			// If given audio system is not a registered managed implementation, try setting the console
			// variable, in case we were given the name of native implementation.
			var consoleVariable = CryConsole.GetVariable("s_AudioSystemImplementationName");
			consoleVariable.ValueString = name;
		}
		/// <summary>
		/// Reserves an identifier for an audio object.
		/// </summary>
		/// <remarks>
		/// <para>
		/// There is no documentation available about reservation of identifiers and there are no known use
		/// cases.
		/// </para>
		/// <para>Right now it's recommended to pass references to static fields.</para>
		/// </remarks>
		/// <param name="id">  Reference to the identifier to reserve.</param>
		/// <param name="name">Name to associated with the reserved identifier.</param>
		public static void ReserveAudioObjectId(ref AudioId id, [CanBeNull] string name)
		{
			RequestReserveAudioId(ref id, name);
		}
		/// <summary>
		/// Requests a set of audio to be preloaded.
		/// </summary>
		/// <param name="id">
		/// Identifier of the set. Can be acquired through <see cref="TryGetPreloadRequestId"/>.
		/// </param>
		public static void PreloadAudioRequest(AudioId id)
		{
			RequestPreloadAudioRequest(id);
		}
		/// <summary>
		/// Requests a set of audio to be preloaded.
		/// </summary>
		/// <param name="name">Name of the set.</param>
		/// <returns>True, if request object with specified name was found.</returns>
		public static bool PreloadAudioRequest(string name)
		{
			AudioId id;
			if (TryGetPreloadRequestId(name, out id))
			{
				RequestPreloadAudioRequest(id);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Requests a set of audio to be unloaded.
		/// </summary>
		/// <param name="id">
		/// Identifier of the set. Can be acquired through <see cref="TryGetPreloadRequestId"/>.
		/// </param>
		public static void UnloadAudioRequest(AudioId id)
		{
			RequestUnloadAudioRequest(id);
		}
		/// <summary>
		/// Requests a set of audio to be unloaded.
		/// </summary>
		/// <param name="name">Name of the set.</param>
		/// <returns>True, if request object with specified name was found.</returns>
		public static bool UnloadAudioRequest(string name)
		{
			AudioId id;
			if (TryGetPreloadRequestId(name, out id))
			{
				RequestUnloadAudioRequest(id);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Assigns a new value to the Real-Time Parameter Control (RTPC) object.
		/// </summary>
		/// <param name="id">   Identifier of the RTPC object to change.</param>
		/// <param name="value">A new value to set.</param>
		public static void SetRtpcValue(AudioId id, float value)
		{
			RequestSetRtpcValue(id, value);
		}
		/// <summary>
		/// Assigns a new value to the Real-Time Parameter Control (RTPC) object.
		/// </summary>
		/// <param name="name"> Name of the RTPC object to set.</param>
		/// <param name="value">A new value to set.</param>
		/// <returns>True, if RTPC object with specified name was found.</returns>
		public static bool SetRtpcValue(string name, float value)
		{
			AudioId id;
			if (TryGetRtpcId(name, out id))
			{
				RequestSetRtpcValue(id, value);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Changes the state of the audio switch.
		/// </summary>
		/// <param name="switchId">Identifier of the switch object.</param>
		/// <param name="stateId"> Identifier of the state of the switch object.</param>
		public static void SetSwitchState(AudioId switchId, uint stateId)
		{
			RequestSetSwitchState(switchId, stateId);
		}
		/// <summary>
		/// Changes the state of the audio switch.
		/// </summary>
		/// <param name="switchName">Name of the switch object.</param>
		/// <param name="stateName"> Name of the state of the switch object.</param>
		/// <returns>
		/// True, if both <paramref name="switchName"/> and <paramref name="stateName"/> were valid names.
		/// </returns>
		public static bool SetSwitchState(string switchName, string stateName)
		{
			AudioId switchId, stateId;
			if (TryGetSwitchId(switchName, out switchId) &&
				TryGetSwitchStateId(switchId, stateName, out stateId))
			{
				RequestSetSwitchState(switchId, stateId);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Changes the state of the audio switch.
		/// </summary>
		/// <param name="switchId"> Identifier of the switch object.</param>
		/// <param name="stateName">Name of the state of the switch object.</param>
		/// <returns>True, if <paramref name="stateName"/> was a valid name.</returns>
		public static bool SetSwitchState(AudioId switchId, string stateName)
		{
			AudioId stateId;
			if (TryGetSwitchStateId(switchId, stateName, out stateId))
			{
				RequestSetSwitchState(switchId, stateId);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Changes the state of the audio switch.
		/// </summary>
		/// <param name="switchName">Name of the switch object.</param>
		/// <param name="stateId">   Identifier of the state of the switch object.</param>
		/// <returns>True, if <paramref name="switchName"/> was a valid name.</returns>
		public static bool SetSwitchState(string switchName, AudioId stateId)
		{
			AudioId switchId;
			if (TryGetSwitchId(switchName, out switchId))
			{
				RequestSetSwitchState(switchId, stateId);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Executes an audio trigger.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger.</param>
		/// <param name="timeout">  
		/// Time in milliseconds until the trigger execution is stopped (?). If equal to 0 then execution
		/// can only be stopped either by the trigger (?) or calling this function again or by calling
		/// <see cref="o:CryCil.Audio.AudioSystem.StopTrigger"/>.
		/// </param>
		public static void ExecuteTrigger(AudioId triggerId, float timeout = 0)
		{
			RequestExecuteTrigger(triggerId, timeout);
		}
		/// <summary>
		/// Executes an audio trigger.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger.</param>
		/// <param name="timeout">  
		/// Time in milliseconds until the trigger execution is stopped (?). If equal to
		/// <see cref="TimeSpan.Zero"/> then execution can only be stopped either by the trigger (?) or
		/// calling this function again or by calling <see cref="o:CryCil.Audio.AudioSystem.StopTrigger"/>.
		/// </param>
		public static void ExecuteTrigger(AudioId triggerId, TimeSpan timeout = new TimeSpan())
		{
			RequestExecuteTrigger(triggerId, timeout.Milliseconds);
		}
		/// <summary>
		/// Executes an audio trigger.
		/// </summary>
		/// <param name="triggerName">Name of the trigger.</param>
		/// <param name="timeout">    
		/// Time in milliseconds until the trigger execution is stopped (?). If equal to 0 then execution
		/// can only be stopped either by the trigger (?) or calling this function again or by calling
		/// <see cref="o:CryCil.Audio.AudioSystem.StopTrigger"/>.
		/// </param>
		/// <returns>True, if trigger with specified name was found.</returns>
		public static bool ExecuteTrigger(string triggerName, float timeout = 0)
		{
			AudioId id;
			if (TryGetTriggerId(triggerName, out id))
			{
				RequestExecuteTrigger(id, timeout);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Executes an audio trigger.
		/// </summary>
		/// <param name="triggerName">Name of the trigger.</param>
		/// <param name="timeout">    
		/// Time in milliseconds until the trigger execution is stopped (?). If equal to
		/// <see cref="TimeSpan.Zero"/> then execution can only be stopped either by the trigger (?) or
		/// calling this function again or by calling <see cref="o:CryCil.Audio.AudioSystem.StopTrigger"/>.
		/// </param>
		/// <returns>True, if trigger with specified name was found.</returns>
		public static bool ExecuteTrigger(string triggerName, TimeSpan timeout = new TimeSpan())
		{
			AudioId id;
			if (TryGetTriggerId(triggerName, out id))
			{
				RequestExecuteTrigger(id, timeout.Milliseconds);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Stops execution of an audio trigger.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger.</param>
		public static void StopTrigger(AudioId triggerId)
		{
			RequestStopTrigger(triggerId);
		}
		/// <summary>
		/// Stops execution of an audio trigger.
		/// </summary>
		/// <param name="triggerName">Name of the trigger.</param>
		/// <returns>True, if trigger with specified name was found.</returns>
		public static bool StopTrigger(string triggerName)
		{
			AudioId id;
			if (TryGetTriggerId(triggerName, out id))
			{
				RequestStopTrigger(id);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Stops execution of all audio triggers.
		/// </summary>
		public static void StopAllTriggers()
		{
			RequestStopAllTriggers();
		}
		/// <summary>
		/// Changes the strength of the environment effect.
		/// </summary>
		/// <param name="id">    Identifier of the effect.</param>
		/// <param name="amount">Strength of the effect.</param>
		public static void SetEnvironmentAmount(AudioId id, float amount)
		{
			RequestSetEnvironmentAmount(id, amount);
		}
		/// <summary>
		/// Changes the strength of the environment effect.
		/// </summary>
		/// <param name="name">  Name of the effect.</param>
		/// <param name="amount">Strength of the effect.</param>
		/// <returns>True, if the effect with specified name was found.</returns>
		public static bool SetEnvironmentAmount(string name, float amount)
		{
			AudioId id;
			if (TryGetEnvironmentId(name, out id))
			{
				RequestSetEnvironmentAmount(id, amount);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Resets strengths all environmental effects.
		/// </summary>
		public static void ResetEnvironments()
		{
			RequestResetEnvironments();
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetImpl(IntPtr implHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestReserveAudioId(ref AudioId id, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestPreloadAudioRequest(AudioId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestUnloadAudioRequest(AudioId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetRtpcValue(AudioId id, float value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetSwitchState(AudioId switchId, AudioId stateId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestExecuteTrigger(AudioId id, float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestStopTrigger(AudioId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestStopAllTriggers();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetPosition(ref Matrix34 tm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetVolume(float volume);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetEnvironmentAmount(AudioId id, float amount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestResetEnvironments();
		#endregion
	}
}