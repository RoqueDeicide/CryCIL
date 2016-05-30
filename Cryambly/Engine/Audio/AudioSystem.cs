using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Provides access to CryEngine audio API.
	/// </summary>
	public static partial class AudioSystem
	{
		#region Properties
		/// <summary>
		/// Gets the path to the file (or folder) that contains the data that is used to configure the audio
		/// system.
		/// </summary>
		public static string ConfigurationPath => GetConfigPath();
		#endregion
		#region Construction
		/// <summary>
		/// Creates a default audio proxy object.
		/// </summary>
		/// <returns>A new audio proxy object that can be used until released manually.</returns>
		public static CryAudioProxy Create()
		{
			return GetFreeAudioProxy();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Attempts to acquire the identifier of the audio preload request.
		/// </summary>
		/// <param name="name">Name of the preload request.</param>
		/// <param name="id">  Resultant identifier.</param>
		/// <returns>
		/// True, if preload request exists and valid identifier was assigned to <paramref name="id"/>.
		/// </returns>
		public static bool TryGetPreloadRequestId(string name, out AudioId id)
		{
			id = 0;
			return !name.IsNullOrEmpty() && GetPreloadRequestId(name, out id);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the Real-Time Parameter Control (RTPC) object.
		/// </summary>
		/// <param name="name">Name of the RTPC object which identifier to get.</param>
		/// <param name="id">  
		/// Resultant identifier that is only valid if this method returns <c>true</c>.
		/// </param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetRtpcId(string name, out AudioId id)
		{
			id = 0;
			return !name.IsNullOrEmpty() && GetAudioRtpcId(name, out id);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the audio switch object.
		/// </summary>
		/// <param name="name">Name of the audio switch object.</param>
		/// <param name="id">  
		/// Resultant identifier that is only valid if this method returns <c>true</c>.
		/// </param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetSwitchId(string name, out AudioId id)
		{
			id = 0;
			return !name.IsNullOrEmpty() && GetAudioSwitchId(name, out id);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the state of the audio switch object.
		/// </summary>
		/// <param name="switchId"> Identifier of the switch.</param>
		/// <param name="stateName">Name of the state of the switch.</param>
		/// <param name="stateId">  
		/// Resultant identifier that is only valid if this method returns <c>true</c>.
		/// </param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetSwitchStateId(AudioId switchId, string stateName, out AudioId stateId)
		{
			stateId = 0;
			return !stateName.IsNullOrEmpty() && GetAudioSwitchStateId(switchId, stateName, out stateId);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the state of the audio switch object.
		/// </summary>
		/// <param name="switchName">Name of the switch.</param>
		/// <param name="stateName"> Name of the state of the switch.</param>
		/// <param name="stateId">   
		/// Resultant identifier that is only valid if this method returns <c>true</c>.
		/// </param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetSwitchStateId(string switchName, string stateName, out AudioId stateId)
		{
			stateId = 0;
			AudioId switchId;
			return !stateName.IsNullOrEmpty() && TryGetSwitchId(switchName, out switchId) &&
				   GetAudioSwitchStateId(switchId, stateName, out stateId);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the audio trigger.
		/// </summary>
		/// <param name="triggerName">Name of the trigger.</param>
		/// <param name="id">         
		/// Resultant identifier that is only valid if this method returns <c>true</c>.
		/// </param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetTriggerId(string triggerName, out AudioId id)
		{
			id = 0;
			return !triggerName.IsNullOrEmpty() && GetAudioTriggerId(triggerName, out id);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the environment audio effect.
		/// </summary>
		/// <param name="name">Name of the effect.</param>
		/// <param name="id">  
		/// Resultant identifier that is only valid if this method returns <c>true</c>.
		/// </param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetEnvironmentId(string name, out AudioId id)
		{
			id = 0;
			return !name.IsNullOrEmpty() && GetAudioEnvironmentId(name, out id);
		}
		/// <summary>
		/// Gets the identifier of the audio control.
		/// </summary>
		/// <param name="type">          Type of the audio control.</param>
		/// <param name="name">          Name of the control.</param>
		/// <param name="id">            
		/// Resultant identifier that is only valid if this method returns <c>true</c>.
		/// </param>
		/// <param name="additionalName">
		/// Optional value that needs to be a name of the audio switch, if <paramref name="type"/> is equal
		/// to <see cref="AudioControlType.SwitchState"/>.
		/// </param>
		/// <returns>
		/// False, if <paramref name="name"/> is either <c>null</c> or an empty string, if
		/// <paramref name="type"/> is equal to <see cref="AudioControlType.None"/> or
		/// <see cref="AudioControlType.AudioObject"/> or any value that is not in the enumeration, if
		/// control with specified name is not found, otherwise returns <c>true</c>.
		/// </returns>
		public static bool TryGetAudioControlId(AudioControlType type, string name, out AudioId id,
												string additionalName = null)
		{
			id = 0;

			if (name.IsNullOrEmpty())
			{
				return false;
			}

			switch (type)
			{
				case AudioControlType.None:
				case AudioControlType.AudioObject:
					return false;
				case AudioControlType.Trigger:
					return GetAudioTriggerId(name, out id);
				case AudioControlType.Rtpc:
					return GetAudioRtpcId(name, out id);
				case AudioControlType.Switch:
					return GetAudioSwitchId(name, out id);
				case AudioControlType.SwitchState:
					AudioId switchId;
					return GetAudioSwitchId(name, out switchId) &&
						   GetAudioSwitchStateId(switchId, additionalName, out id);
				case AudioControlType.Preload:
					return GetPreloadRequestId(name, out id);
				case AudioControlType.Environment:
					return GetAudioEnvironmentId(name, out id);
				default:
					return false;
			}
		}
		/// <summary>
		/// Gets the name of the object that is used to control the audio in the game.
		/// </summary>
		/// <param name="type">Type of the audio control object.</param>
		/// <param name="id">  Identifier of the audio control object.</param>
		/// <returns>Name of the control object, or <c>null</c> if it wasn't found.</returns>
		[CanBeNull]
		public static string GetAudioControlName(AudioControlType type, AudioId id)
		{
			return type == AudioControlType.None ? null : GetAudioControlNameInternal(type, id);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateNativeImplementationObject(AudioSystemImplementation managedObject);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetPreloadRequestId(string name, out AudioId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioTriggerId(string sAudioTriggerName, out AudioId rAudioTriggerId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioRtpcId(string audioRtpcName, out AudioId audioRtpcId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioSwitchId(string audioSwitchName, out AudioId audioSwitchId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioSwitchStateId(uint switchId, string audioTriggerName, out AudioId audioStateId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioEnvironmentId(string sAudioEnvironmentName, out AudioId rAudioEnvironmentId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetConfigPath();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryAudioProxy GetFreeAudioProxy();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetAudioControlNameInternal(AudioControlType eAudioEntityType, AudioId nAudioEntityId);
		#endregion
	}
}