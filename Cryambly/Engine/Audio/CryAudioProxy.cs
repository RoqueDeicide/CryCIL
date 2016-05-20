using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Logic;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Represents an audio proxy.
	/// </summary>
	public struct CryAudioProxy
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
		/// Gets the identifier of this proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AudioId Id
		{
			get
			{
				this.AssertInstance();

				return GetAudioObjectId(this.handle);
			}
		}
		/// <summary>
		/// Sets the type of algorithm that is used to determine whether sound emitted by this proxy is
		/// obstructed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AudioOcclusionType ObstructionDetection
		{
			set
			{
				this.AssertInstance();

				SetObstructionCalcTypeInternal(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the position of this proxy in world-space(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 Position
		{
			set
			{
				this.AssertInstance();

				SetPosition(this.handle, ref value);
			}
		}
		/// <summary>
		/// Sets the transformation of this proxy in world-space(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Matrix34 Transformation
		{
			set
			{
				this.AssertInstance();

				SetTransformation(this.handle, ref value);
			}
		}
		#endregion
		#region Construction
		internal CryAudioProxy(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Initializes this audio proxy, turning it into a valid audio object that can make sounds.
		/// </summary>
		/// <param name="name">     Name of the object.</param>
		/// <param name="asyncInit">Indicates whether initialization must be done asynchronously.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Initialize(string name, bool asyncInit = true)
		{
			this.AssertInstance();

			Init(this.handle, name, asyncInit);
		}
		/// <summary>
		/// Releases this audio proxy, if it was initialized previously(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Release()
		{
			this.AssertInstance();

			ReleaseInternal(this.handle);
		}
		/// <summary>
		/// Resets this audio proxy, making reinitialization possible(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Reset()
		{
			this.AssertInstance();

			ResetInternal(this.handle);
		}
		/// <summary>
		/// Plays an audio file.
		/// </summary>
		/// <param name="file">Path to the file to play.</param>
		public void PlayFile(string file)
		{
			this.AssertInstance();

			PlayFileInternal(this.handle, file);
		}
		/// <summary>
		/// Stops an audio file.
		/// </summary>
		/// <param name="file">Path to the file to stop playing.</param>
		public void StopFile(string file)
		{
			this.AssertInstance();

			StopFileInternal(this.handle, file);
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerId">Identifier of the trigger to execute.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ExecuteTrigger(AudioId triggerId)
		{
			this.AssertInstance();

			ExecuteTriggerInternal(this.handle, triggerId);
		}
		/// <summary>
		/// Executes an audio trigger on this proxy.
		/// </summary>
		/// <param name="triggerName">Name of the trigger to execute.</param>
		/// <returns>Indication whether the trigger of specified name was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool ExecuteTrigger(string triggerName)
		{
			this.AssertInstance();

			AudioId id;
			if (AudioSystem.TryGetTriggerId(triggerName, out id))
			{
				ExecuteTriggerInternal(this.handle, id);

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

			StopTriggerInternal(this.handle, triggerId);
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
				StopTriggerInternal(this.handle, id);

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

			SetSwitchStateInternal(this.handle, switchId, stateId);
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
				SetSwitchStateInternal(this.handle, switchId, stateId);

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

			SetRtpcValueInternal(this.handle, rtpcId, value);
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
				SetRtpcValueInternal(this.handle, id, value);

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

			SetEnvironmentAmountInternal(this.handle, environmentId, amount);
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
				SetEnvironmentAmountInternal(this.handle, id, amount);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Makes environment effects around this proxy affect this proxy(?).
		/// </summary>
		/// <param name="entityToIgnore">An entity to ignore(?).</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetCurrentEnvironments(EntityId entityToIgnore = new EntityId())
		{
			this.AssertInstance();

			SetCurrentEnvironmentsInternal(this.handle, entityToIgnore);
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
		private static extern void Init(IntPtr handle, string sObjectName, bool bInitAsync);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReleaseInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PlayFileInternal(IntPtr handle, string file);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StopFileInternal(IntPtr handle, string file);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExecuteTriggerInternal(IntPtr handle, AudioId nTriggerID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StopTriggerInternal(IntPtr handle, AudioId nTriggerID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSwitchStateInternal(IntPtr handle, AudioId nSwitchID, AudioId nStateID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRtpcValueInternal(IntPtr handle, AudioId nRtpcID, float fValue);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetObstructionCalcTypeInternal(IntPtr handle, AudioOcclusionType eObstructionType);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTransformation(IntPtr handle, ref Matrix34 rPosition);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPosition(IntPtr handle, ref Vector3 rPosition);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEnvironmentAmountInternal(IntPtr handle, AudioId nEnvironmentID, float fAmount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCurrentEnvironmentsInternal(IntPtr handle, EntityId nEntityToIgnore);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioId GetAudioObjectId(IntPtr handle);
		#endregion
	}
}