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
		
		#endregion
		#region Events
		
		#endregion
		#region Construction
		
		#endregion
		#region Interface
		
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateNativeImplementationObject(AudioSystemImplementation managedObject);
		virtual void					PushRequest(SAudioRequest const& rAudioRequestData) = 0;
		virtual void					AddRequestListener(void (*func)(SAudioRequestInfo const* const), void* const pObjectToListenTo, EAudioRequestType const requestType = eART_AUDIO_ALL_REQUESTS, TATLEnumFlagsType const specificRequestMask = ALL_AUDIO_REQUEST_SPECIFIC_TYPE_FLAGS) = 0;
		virtual void					RemoveRequestListener(void (*func)(SAudioRequestInfo const* const), void* const pObjectToListenTo) = 0;
		virtual void					ExternalUpdate() = 0;
		virtual bool					GetAudioTriggerID(char const* const sAudioTriggerName, TAudioControlID& rAudioTriggerID) const = 0;
		virtual bool					GetAudioRtpcID(char const* const sAudioRtpcName, TAudioControlID& rAudioRtpcID) const = 0;
		virtual bool					GetAudioSwitchID(char const* const sAudioSwitchName, TAudioControlID& rAudioSwitchID) const = 0;
		virtual bool					GetAudioSwitchStateID(TAudioControlID const nSwitchID, char const* const sAudioTriggerName, TAudioSwitchStateID& rAudioStateID) const = 0;
		virtual bool					GetAudioPreloadRequestID(char const* const sAudioPreloadRequestName, TAudioPreloadRequestID& rAudioPreloadRequestID) const = 0;
		virtual bool					GetAudioEnvironmentID(char const* const sAudioEnvironmentName, TAudioEnvironmentID& rAudioEnvironmentID) const = 0;
		virtual bool					ReserveAudioListenerID(TAudioObjectID& rAudioObjectID) = 0;
		virtual bool					ReleaseAudioListenerID(TAudioObjectID const nAudioObjectID) = 0;
		virtual void					OnCVarChanged(ICVar* const pCvar) = 0;
		virtual void					GetInfo(SAudioSystemInfo& rAudioSystemInfo) = 0;
		virtual char const*		GetConfigPath() const = 0;
		virtual IAudioProxy*	GetFreeAudioProxy() = 0;
		virtual void					FreeAudioProxy(IAudioProxy* const pIAudioProxy) = 0;
		virtual char const*		GetAudioControlName(EAudioControlType const eAudioEntityType, TATLIDType const nAudioEntityID) = 0;
		virtual char const*		GetAudioControlName(EAudioControlType const eAudioEntityType, TATLIDType const nAudioEntityID1, TATLIDType const nAudioEntityID2) = 0;
		#endregion
	}
}