using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Audio;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents an audio object that is bound to the entity.
	/// </summary>
	public struct CryEntityAudioProxy
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets or sets the fading distance for this audio object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float FadeDistance
		{
			get
			{
				this.AssertInstance();

				return GetFadeDistance(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetFadeDistance(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the fading distance for environmental effects of this audio object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float EnvironmentFadeDistance
		{
			get
			{
				this.AssertInstance();

				return GetEnvironmentFadeDistance(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetEnvironmentFadeDistance(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the environmental effect for this audio object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AudioId EnvironmentId
		{
			get
			{
				this.AssertInstance();

				return GetEnvironmentID(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetEnvironmentID(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the type of algorithm that is used to determine whether sound emitted by this proxy is
		/// obstructed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ObstructionCalculationType ObstructionDetection
		{
			set
			{
				this.AssertInstance();

				SetObstructionCalcTypeInternal(this.handle, value, AudioId.Default);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether auxiliary audio objects must move with the entity.
		/// </summary>
		public bool AuxAudioMovesWithEntity
		{
			set
			{
				this.AssertInstance();

				AuxAudioProxiesMoveWithEntity(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal CryEntityAudioProxy(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates auxiliary audio object that moves with this entity.
		/// </summary>
		/// <returns>Identifier of created audio proxy.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AudioId CreateAuxAudio()
		{
			this.AssertInstance();

			return CreateAuxAudioProxy(this.handle);
		}
		/// <summary>
		/// Destroys auxiliary audio object that moved with this entity.
		/// </summary>
		/// <param name="id">
		/// Identifier that was preveiously returned by <see cref="CreateAuxAudio"/>.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DestroyAuxAudio(AudioId id)
		{
			this.AssertInstance();

			RemoveAuxAudioProxy(this.handle, id);
		}
		/// <summary>
		/// Sets the transformation of the auxiliary audio object in entity-space.
		/// </summary>
		/// <param name="id">            
		/// Identifier of the auxiliary object to change the offset for.
		/// </param>
		/// <param name="transformation">Reference to 3x4 matrix that represents the tranformation.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetAuxAudioOffset(AudioId id, ref Matrix34 transformation)
		{
			this.AssertInstance();

			SetAuxAudioProxyOffset(this.handle, ref transformation, id);
		}
		/// <summary>
		/// Gets the transformation of the auxiliary audio object in entity-space.
		/// </summary>
		/// <param name="id">            Identifier of the auxiliary object to get the offset of.</param>
		/// <param name="transformation">Resultant 3x4 matrix that represents the tranformation.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void GetAuxAudioOffset(AudioId id, out Matrix34 transformation)
		{
			this.AssertInstance();

			GetAuxAudioProxyOffset(this.handle, out transformation, id);
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger to execute.</param>
		/// <param name="method">   A lip-sync method to use.</param>
		/// <param name="auxAudio"> Identifier of auxiliary audio object to act on.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ExecuteTrigger(AudioId triggerId, AudioId auxAudio, LipSyncMethod method = LipSyncMethod.None)
		{
			this.AssertInstance();

			ExecuteTriggerInternal(this.handle, triggerId, method, auxAudio);
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerName">Name of the trigger to execute.</param>
		/// <param name="method">     A lip-sync method to use.</param>
		/// <param name="auxAudio">   Identifier of auxiliary audio object to act on.</param>
		/// <returns>Indication whether the trigger of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool ExecuteTrigger(string triggerName, AudioId auxAudio, LipSyncMethod method = LipSyncMethod.None)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetTriggerId(triggerName, out id))
			{
				ExecuteTriggerInternal(this.handle, id, method, auxAudio);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger to execute.</param>
		/// <param name="method">   A lip-sync method to use.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ExecuteTrigger(AudioId triggerId, LipSyncMethod method = LipSyncMethod.None)
		{
			this.AssertInstance();

			ExecuteTriggerInternal(this.handle, triggerId, method, AudioId.Default);
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerName">Name of the trigger to execute.</param>
		/// <param name="method">     A lip-sync method to use.</param>
		/// <returns>Indication whether the trigger of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool ExecuteTrigger(string triggerName, LipSyncMethod method = LipSyncMethod.None)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetTriggerId(triggerName, out id))
			{
				ExecuteTriggerInternal(this.handle, id, method, AudioId.Default);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Halts execution of an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger to execute.</param>
		/// <param name="auxAudio"> Identifier of auxiliary audio object to act on.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void StopTrigger(AudioId triggerId, AudioId auxAudio)
		{
			this.AssertInstance();

			StopTriggerInternal(this.handle, triggerId, auxAudio);
		}
		/// <summary>
		/// Halts execution of an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerName">Name of the trigger to execute.</param>
		/// <param name="auxAudio">   Identifier of auxiliary audio object to act on.</param>
		/// <returns>Indication whether the trigger of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool StopTrigger(string triggerName, AudioId auxAudio)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetTriggerId(triggerName, out id))
			{
				StopTriggerInternal(this.handle, id, auxAudio);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Halts execution of an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger to execute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void StopTrigger(AudioId triggerId)
		{
			this.AssertInstance();

			StopTriggerInternal(this.handle, triggerId, AudioId.Default);
		}
		/// <summary>
		/// Halts execution of an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerName">Name of the trigger to execute.</param>
		/// <returns>Indication whether the trigger of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool StopTrigger(string triggerName)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetTriggerId(triggerName, out id))
			{
				StopTriggerInternal(this.handle, id, AudioId.Default);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Changes the state of the switch on this proxy.
		/// </summary>
		/// <param name="switchId">Identifier of the switch to set the state for.</param>
		/// <param name="stateId"> Identifier of the state to set.</param>
		/// <param name="auxAudio">Identifier of auxiliary audio object to act on.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSwitchState(AudioId switchId, AudioId stateId, AudioId auxAudio)
		{
			this.AssertInstance();

			SetSwitchStateInternal(this.handle, switchId, stateId, auxAudio);
		}
		/// <summary>
		/// Changes the state of the switch on this proxy.
		/// </summary>
		/// <param name="switchName">Identifier of the switch to set the state for.</param>
		/// <param name="stateName"> Identifier of the state to set.</param>
		/// <param name="auxAudio">  Identifier of auxiliary audio object to act on.</param>
		/// <returns>Indication whether switch and state with specified names were found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetSwitchState(string switchName, string stateName, AudioId auxAudio)
		{
			this.AssertInstance();

			AudioId switchId, stateId;
			if (AudioSystem.TryGetSwitchId(switchName, out switchId) &&
				AudioSystem.TryGetSwitchStateId(switchId, stateName, out stateId))
			{
				SetSwitchStateInternal(this.handle, switchId, stateId, auxAudio);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Changes the state of the switch on this proxy.
		/// </summary>
		/// <param name="switchId">Identifier of the switch to set the state for.</param>
		/// <param name="stateId"> Identifier of the state to set.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSwitchState(AudioId switchId, AudioId stateId)
		{
			this.AssertInstance();

			SetSwitchStateInternal(this.handle, switchId, stateId, AudioId.Default);
		}
		/// <summary>
		/// Changes the state of the switch on this proxy.
		/// </summary>
		/// <param name="switchName">Identifier of the switch to set the state for.</param>
		/// <param name="stateName"> Identifier of the state to set.</param>
		/// <returns>Indication whether switch and state with specified names were found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetSwitchState(string switchName, string stateName)
		{
			this.AssertInstance();

			AudioId switchId, stateId;
			if (AudioSystem.TryGetSwitchId(switchName, out switchId) &&
				AudioSystem.TryGetSwitchStateId(switchId, stateName, out stateId))
			{
				SetSwitchStateInternal(this.handle, switchId, stateId, AudioId.Default);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Changes the RTPC (Real-Time Parameter Control) value on this proxy.
		/// </summary>
		/// <param name="rtpcId">  Identifier of the RTPC to set.</param>
		/// <param name="value">   A value to set.</param>
		/// <param name="auxAudio">Identifier of auxiliary audio object to act on.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetRtpcValue(AudioId rtpcId, float value, AudioId auxAudio)
		{
			this.AssertInstance();

			SetRtpcValueInternal(this.handle, rtpcId, value, auxAudio);
		}
		/// <summary>
		/// Changes the RTPC (Real-Time Parameter Control) value on this proxy.
		/// </summary>
		/// <param name="rtpcName">Name of the RTPC to set.</param>
		/// <param name="value">   A value to set.</param>
		/// <param name="auxAudio">Identifier of auxiliary audio object to act on.</param>
		/// <returns>Indication whether the RTPC of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetRtpcValue(string rtpcName, float value, AudioId auxAudio)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetRtpcId(rtpcName, out id))
			{
				SetRtpcValueInternal(this.handle, id, value, auxAudio);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Changes the RTPC (Real-Time Parameter Control) value on this proxy.
		/// </summary>
		/// <param name="rtpcId">Identifier of the RTPC to set.</param>
		/// <param name="value"> A value to set.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetRtpcValue(AudioId rtpcId, float value)
		{
			this.AssertInstance();

			SetRtpcValueInternal(this.handle, rtpcId, value, AudioId.Default);
		}
		/// <summary>
		/// Changes the RTPC (Real-Time Parameter Control) value on this proxy.
		/// </summary>
		/// <param name="rtpcName">Name of the RTPC to set.</param>
		/// <param name="value">   A value to set.</param>
		/// <returns>Indication whether the RTPC of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetRtpcValue(string rtpcName, float value)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetRtpcId(rtpcName, out id))
			{
				SetRtpcValueInternal(this.handle, id, value, AudioId.Default);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Sets the strength of the environment effect on this proxy.
		/// </summary>
		/// <param name="environmentId">Identifier of the environment effect to change.</param>
		/// <param name="amount">       Strength to set.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetEnvironmentAmount(AudioId environmentId, float amount)
		{
			this.AssertInstance();

			SetEnvironmentAmountInternal(this.handle, environmentId, amount, AudioId.Default);
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="environmentName">Name of the environment effect to change.</param>
		/// <param name="amount">         Strength to set.</param>
		/// <param name="auxAudio">       Identifier of auxiliary audio object to act on.</param>
		/// <returns>Indication whether the environment effect of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetEnvironmentAmount(string environmentName, float amount, AudioId auxAudio)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetEnvironmentId(environmentName, out id))
			{
				SetEnvironmentAmountInternal(this.handle, id, amount, auxAudio);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="environmentName">Name of the environment effect to change.</param>
		/// <param name="amount">         Strength to set.</param>
		/// <returns>Indication whether the environment effect of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetEnvironmentAmount(string environmentName, float amount)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetEnvironmentId(environmentName, out id))
			{
				SetEnvironmentAmountInternal(this.handle, id, amount, AudioId.Default);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Makes environment effects around this proxy affect it(?).
		/// </summary>
		/// <param name="auxAudio">Identifier of auxiliary audio object to act on.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetCurrentEnvironments(AudioId auxAudio)
		{
			this.AssertInstance();

			SetCurrentEnvironmentsInternal(this.handle, auxAudio);
		}
		/// <summary>
		/// Makes environment effects around this proxy affect it(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetCurrentEnvironments()
		{
			this.AssertInstance();

			SetCurrentEnvironmentsInternal(this.handle, AudioId.Default);
		}
		/// <summary>
		/// Sets the type of algorithm that is used to determine whether sound emitted by this proxy is
		/// obstructed.
		/// </summary>
		/// <param name="type">    Type of algorithm.</param>
		/// <param name="auxAudio">Identifier of auxiliary audio object to act on.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetObstructionCalculationType(ObstructionCalculationType type, AudioId auxAudio)
		{
			this.AssertInstance();

			SetObstructionCalcTypeInternal(this.handle, type, auxAudio);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFadeDistance(IntPtr handle, float fFadeDistance);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetFadeDistance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEnvironmentFadeDistance(IntPtr handle, float fEnvironmentFadeDistance);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetEnvironmentFadeDistance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEnvironmentID(IntPtr handle, AudioId nEnvironmentID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioId GetEnvironmentID(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioId CreateAuxAudioProxy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool RemoveAuxAudioProxy(IntPtr handle, AudioId nAudioProxyLocalID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAuxAudioProxyOffset(IntPtr handle, ref Matrix34 rOffset, AudioId nAudioProxyLocalID
			/* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetAuxAudioProxyOffset(IntPtr handle, out Matrix34 offset, AudioId nAudioProxyLocalID
			/* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ExecuteTriggerInternal(IntPtr handle, AudioId nTriggerID, LipSyncMethod eLipSyncMethod,
														  AudioId nAudioProxyLocalID /* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StopTriggerInternal(IntPtr handle, AudioId nTriggerID, AudioId nAudioProxyLocalID
			/* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSwitchStateInternal(IntPtr handle, AudioId nSwitchID, AudioId nStateID,
														  AudioId nAudioProxyLocalID /* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRtpcValueInternal(IntPtr handle, AudioId nRtpcID, float fValue,
														AudioId nAudioProxyLocalID /* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetObstructionCalcTypeInternal(IntPtr handle, ObstructionCalculationType eObstructionType,
																  AudioId nAudioProxyLocalID /* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEnvironmentAmountInternal(IntPtr handle, AudioId nEnvironmentID, float fAmount,
																AudioId nAudioProxyLocalID /* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCurrentEnvironmentsInternal(IntPtr handle, AudioId nAudioProxyLocalID
			/* = AudioId.Default*/);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AuxAudioProxiesMoveWithEntity(IntPtr handle, bool bCanMoveWithEntity);
		#endregion
	}
}