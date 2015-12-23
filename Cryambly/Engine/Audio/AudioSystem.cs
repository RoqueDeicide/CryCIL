using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Provides access to CryEngine audio API.
	/// </summary>
	public static partial class AudioSystem
	{
		#region Fields
		
		#endregion
		#region Properties
		/// <summary>
		/// Gets the path to the file (or folder) that contains the data that is used to configure the audio system.
		/// </summary>
		public static string ConfigurationPath
		{
			get { return GetConfigPath(); }
		}
		#endregion
		#region Events
		
		#endregion
		#region Construction
		
		#endregion
		#region Interface
		/// <summary>
		/// Attempts to acquire the identifier of the audio preload request.
		/// </summary>
		/// <param name="name">Name of the preload request.</param>
		/// <param name="id">Resultant identifier.</param>
		/// <returns>True, if preload request exists and valid identifier was assigned to <paramref name="id"/>.</returns>
		public static bool TryGetPreloadRequestId(string name, out uint id)
		{
			id = 0;
			return !name.IsNullOrEmpty() && GetPreloadRequestId(name, out id);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the Real-Time Parameter Control (RTPC) object.
		/// </summary>
		/// <param name="name">Name of the RTPC object which identifier to get.</param>
		/// <param name="id">Resultant identifier that is only valid if this method returns <c>true</c>.</param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetRtpcId(string name, out uint id)
		{
			id = 0;
			return !name.IsNullOrEmpty() && GetAudioRtpcID(name, out id);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the audio switch object.
		/// </summary>
		/// <param name="name">Name of the audio switch object.</param>
		/// <param name="id">Resultant identifier that is only valid if this method returns <c>true</c>.</param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetSwitchId(string name, out uint id)
		{
			id = 0;
			return !name.IsNullOrEmpty() && GetAudioSwitchID(name, out id);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the state of the audio switch object.
		/// </summary>
		/// <param name="switchId">Identifier of the switch.</param>
		/// <param name="stateName">Name of the state of the switch.</param>
		/// <param name="stateId">Resultant identifier that is only valid if this method returns <c>true</c>.</param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetSwitchStateId(uint switchId, string stateName, out uint stateId)
		{
			stateId = 0;
			return !stateName.IsNullOrEmpty() && GetAudioSwitchStateID(switchId, stateName, out stateId);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the state of the audio switch object.
		/// </summary>
		/// <param name="switchName">Name of the switch.</param>
		/// <param name="stateName">Name of the state of the switch.</param>
		/// <param name="stateId">Resultant identifier that is only valid if this method returns <c>true</c>.</param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetSwitchStateId(string switchName, string stateName, out uint stateId)
		{
			stateId = 0;
			uint switchId;
			return !stateName.IsNullOrEmpty() && TryGetSwitchId(switchName, out switchId) && GetAudioSwitchStateID(switchId, stateName, out stateId);
		}
		/// <summary>
		/// Attempts to acquire the identifier of the audio trigger.
		/// </summary>
		/// <param name="triggerName">Name of the trigger.</param>
		/// <param name="id">Resultant identifier that is only valid if this method returns <c>true</c>.</param>
		/// <returns>Indication whether identifier was acquired successfully.</returns>
		public static bool TryGetTriggerId(string triggerName, out uint id)
		{
			id =0;
			return !triggerName.IsNullOrEmpty() && GetAudioTriggerID(triggerName, out id);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateNativeImplementationObject(AudioSystemImplementation managedObject);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetPreloadRequestId(string name, out uint id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void					 PushRequest(SAudioRequest const& rAudioRequestData);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void					 AddRequestListener(void (*func)(SAudioRequestInfo const* const), void* const pObjectToListenTo, EAudioRequestType const requestType = eART_AUDIO_ALL_REQUESTS, TATLEnumFlagsType const specificRequestMask = ALL_AUDIO_REQUEST_SPECIFIC_TYPE_FLAGS);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void					 RemoveRequestListener(void (*func)(SAudioRequestInfo const* const), void* const pObjectToListenTo);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void					 ExternalUpdate();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioTriggerID(string sAudioTriggerName, out uint rAudioTriggerID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioRtpcID(string audioRtpcName, out uint audioRtpcId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioSwitchID(string audioSwitchName, out uint audioSwitchId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAudioSwitchStateID(uint switchID, string audioTriggerName, out uint audioStateId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool					 GetAudioEnvironmentID(string sAudioEnvironmentName, TAudioEnvironmentID& rAudioEnvironmentID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool					 ReserveAudioListenerID(TAudioObjectID& rAudioObjectID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool					 ReleaseAudioListenerID(TAudioObjectID const nAudioObjectID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void					 OnCVarChanged(ICVar* const pCvar);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void					 GetInfo(SAudioSystemInfo& rAudioSystemInfo);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetConfigPath();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IAudioProxy*	 GetFreeAudioProxy();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void					 FreeAudioProxy(IAudioProxy* const pIAudioProxy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern char const*		 GetAudioControlName(EAudioControlType const eAudioEntityType, TATLIDType const nAudioEntityID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern char const*		 GetAudioControlName(EAudioControlType const eAudioEntityType, TATLIDType const nAudioEntityID1, TATLIDType const nAudioEntityID2);
		#endregion
	}
}