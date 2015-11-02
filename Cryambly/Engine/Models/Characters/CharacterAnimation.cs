using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using CryCil.Engine.Data;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents an animation.
	/// </summary>
	public struct CharacterAnimation
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets the object that handles sampling for animations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public ParametricSampler ParametricSampler
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetParametricSampler(this.handle);
			}
		}
		/// <summary>
		/// Gets the identifier of this animation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public short AnimationId
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetAnimationId(this.handle);
			}
		}
		/// <summary>
		/// Gets the index of the segment this animation is currently on.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public byte CurrentSegmentIndex
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetCurrentSegmentIndex(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that represents the relative position of this animation
		/// in time where 0 represents start of animation and 1 - end of animation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float CurrentNormalizedAnimationTime
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetCurrentSegmentNormalizedTime(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetCurrentSegmentNormalizedTime(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the priority of motions in this animation. Motions with higher priority override
		/// those with lower priority in transition queue.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float TransitionPriority
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetTransitionPriority(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetTransitionPriority(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the percentage of this animation's motions in transition animation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float TransitionWeight
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetTransitionWeight(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetTransitionWeight(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the time it takes to transition into this animation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float TransitionTime
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetTransitionTime(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetTransitionTime(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that specifies how much of this animation layer blends
		/// onto previous ones.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float PlaybackWeight
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetPlaybackWeight(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetPlaybackWeight(this.handle, MathHelpers.Clamp(value, 0.0f, 1.0f));
			}
		}
		/// <summary>
		/// Gets or sets the scale of animation playback speed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float PlaybackScale
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetPlaybackScale(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetPlaybackScale(this.handle, MathHelpers.Clamp(value, 0.0000001f, float.MaxValue));
			}
		}
		/// <summary>
		/// Gets or sets the user token that can be used to identify this animation or carry any user data.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint UserToken
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetUserToken(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetUserToken(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the time this animation is supposed to take to play out in seconds.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float ExpectedTotalDurationSeconds
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetExpectedTotalDurationSeconds(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetExpectedTotalDurationSeconds(this.handle, value);
			}
		}
		/// <summary>
		/// Indicates whether this animation is active.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsActive
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return IsActivated(this.handle) != 0;
			}
		}
		/// <summary>
		/// Indicates whether this animation is looping.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Looping
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetLoop(this.handle) != 0;
			}
		}
		/// <summary>
		/// Indicates whether this animation is at the end of its cycle.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool AtTheEndOfCycle
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetEndOfCycle(this.handle) != 0;
			}
		}
		/// <summary>
		/// Indicates whether this animation uses time warping in transition queue.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool UsesTimeWarping
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetUseTimeWarping(this.handle) != 0;
			}
		}
		#endregion
		#region Construction
		internal CharacterAnimation(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Synchronizes the state of this animation.
		/// </summary>
		/// <param name="sync">An object that handle synchronization.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Synchronize(CrySync sync)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			Serialize(this.handle, sync);
		}
		/// <summary>
		/// Determines whether this animation has a specific set of flags set.
		/// </summary>
		/// <param name="flags">A set of flags that need to be checked.</param>
		/// <returns>True, if all specified flags are set for this instance.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool HasStaticFlags(AnimationFlags flags)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return HasStaticFlagInternal(this.handle, flags);
		}
		/// <summary>
		/// Sets specified flags without clearing others.
		/// </summary>
		/// <param name="flags">Flags to set.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetStaticFlags(AnimationFlags flags)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			SetStaticFlagInternal(this.handle, flags);
		}
		/// <summary>
		/// Clears specified flags without clearing others.
		/// </summary>
		/// <param name="flags">Flags to clear.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ClearStaticFlags(AnimationFlags flags)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			ClearStaticFlagInternal(this.handle, flags);
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
		private static extern void Serialize(IntPtr handle, CrySync ser);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParametricSampler GetParametricSampler(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern short GetAnimationId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte GetCurrentSegmentIndex(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasStaticFlagInternal(IntPtr handle, AnimationFlags animationFlag);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStaticFlagInternal(IntPtr handle, AnimationFlags nStaticFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearStaticFlagInternal(IntPtr handle, AnimationFlags nStaticFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetCurrentSegmentNormalizedTime(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCurrentSegmentNormalizedTime(IntPtr handle, float normalizedSegmentTime);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetTransitionPriority(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTransitionPriority(IntPtr handle, float transitionPriority);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetTransitionWeight(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTransitionWeight(IntPtr handle, float transitionWeight);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetTransitionTime(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTransitionTime(IntPtr handle, float transitionTime);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetPlaybackWeight(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPlaybackWeight(IntPtr handle, float playbackWeight);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetPlaybackScale(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPlaybackScale(IntPtr handle, float playbackScale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetUserToken(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetUserToken(IntPtr handle, uint nUserToken);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetExpectedTotalDurationSeconds(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetExpectedTotalDurationSeconds(IntPtr handle, float expectedDurationSeconds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint IsActivated(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetLoop(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetEndOfCycle(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetUseTimeWarping(IntPtr handle);
		#endregion
	}
}