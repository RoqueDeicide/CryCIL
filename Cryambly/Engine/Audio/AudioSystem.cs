﻿using System;
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
		/// Gets the path to the file (or folder) that contains the data that is used to configure the
		/// audio system.
		/// </summary>
		public static string ConfigurationPath
		{
			get { return GetConfigPath(); }
		}
		/// <summary>
		/// Gets information about the current state of the audio system.
		/// </summary>
		public static AudioSystemInfo Information
		{
			get
			{
				AudioSystemInfo info;
				GetInfo(out info);
				return info;
			}
		}
		/// <summary>
		/// Gets current position of the audio system listener.
		/// </summary>
		public static Vector3 ListenerPosition
		{
			get
			{
				AudioSystemInfo info;
				GetInfo(out info);
				return info.ListenerPosition;
			}
		}
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
			return !name.IsNullOrEmpty() && GetAudioRtpcID(name, out id);
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
			return !name.IsNullOrEmpty() && GetAudioSwitchID(name, out id);
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
			return !stateName.IsNullOrEmpty() && GetAudioSwitchStateID(switchId, stateName, out stateId);
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
				   GetAudioSwitchStateID(switchId, stateName, out stateId);
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
			return !triggerName.IsNullOrEmpty() && GetAudioTriggerID(triggerName, out id);
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
			return !name.IsNullOrEmpty() && GetAudioEnvironmentID(name, out id);
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
					return GetAudioTriggerID(name, out id);
				case AudioControlType.Rtpc:
					return GetAudioRtpcID(name, out id);
				case AudioControlType.Switch:
					return GetAudioSwitchID(name, out id);
				case AudioControlType.SwitchState:
					AudioId switchId;
					return GetAudioSwitchID(name, out switchId) &&
						   GetAudioSwitchStateID(switchId, additionalName, out id);
				case AudioControlType.Preload:
					return GetPreloadRequestId(name, out id);
				case AudioControlType.Environment:
					return GetAudioEnvironmentID(name, out id);
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
		private static extern bool GetAudioTriggerID(string sAudioTriggerName, out AudioId rAudioTriggerID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioRtpcID(string audioRtpcName, out AudioId audioRtpcId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioSwitchID(string audioSwitchName, out AudioId audioSwitchId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioSwitchStateID(uint switchID, string audioTriggerName, out AudioId audioStateId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioEnvironmentID(string sAudioEnvironmentName, out AudioId rAudioEnvironmentID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetInfo(out AudioSystemInfo rAudioSystemInfo);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetConfigPath();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryAudioProxy GetFreeAudioProxy();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetAudioControlNameInternal(AudioControlType eAudioEntityType, AudioId nAudioEntityID);
		#endregion
	}
}