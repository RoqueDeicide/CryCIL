using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents an interface with skeleton animations for characters.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct SkeletonAnimation
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to the collection of animation layers.
		/// </summary>
		[FieldOffset(0)] public readonly SkeletonAnimationLayers Layers;
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
		/// Sets the value that indicates whether debug messages need to be displayed for this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool DisplayDebugText
		{
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetDebugging(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether animation playback for this character moves it.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool AnimationDrivenMotion
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetAnimationDrivenMotion(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetAnimationDrivenMotion(this.handle, value);
			}
		}
		/// <summary>
		/// Indicates whether this animation object is participating in the track-view based cutscene.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsInTrackView
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetTrackViewStatus(this.handle);
			}
		}
		/// <summary>
		/// Gets the velocity of this character.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 Velocity
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetCurrentVelocity(this.handle);
			}
		}
		/// <summary>
		/// Gets the relative movement of the character that is caused by the animation.
		/// </summary>
		/// <remarks>
		/// You can update position of the character by combining returned <see cref="Quatvec"/> with
		/// entity's current transformation matrix.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec RelativeMovement
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				Quatvec movement;
				GetRelMovement(this.handle, out movement);
				return movement;
			}
		}
		#endregion
		#region Construction
		internal SkeletonAnimation(IntPtr handle)
			: this()
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Begins animation with specified name.
		/// </summary>
		/// <param name="name">      Name of the animation to play.</param>
		/// <param name="parameters">
		/// Reference to the parameters that specify how to start the animation.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool BeginAnimation(string name, ref CharacterAnimationParameters parameters)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StartAnimation(this.handle, name, ref parameters);
		}
		/// <summary>
		/// Begins animation with specified identifier.
		/// </summary>
		/// <param name="identifier">Identifier of the animation to play.</param>
		/// <param name="parameters">
		/// Reference to the parameters that specify how to start the animation.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool BeginAnimation(int identifier, ref CharacterAnimationParameters parameters)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StartAnimationById(this.handle, identifier, ref parameters);
		}
		/// <summary>
		/// Stops animations that are being played on the layer.
		/// </summary>
		/// <param name="layer">       Zero-based index of the layer to stop the animations on.</param>
		/// <param name="blendOutTime">Time in seconds it takes to blend out of all animations.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool StopAnimation(int layer, float blendOutTime)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StopAnimationInLayer(this.handle, layer, blendOutTime);
		}
		/// <summary>
		/// Stops animations that are being played on the layer.
		/// </summary>
		/// <param name="layer">       Zero-based index of the layer to stop the animations on.</param>
		/// <param name="blendOutTime">Time it takes to blend out of all animations.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool StopAnimation(int layer, TimeSpan blendOutTime)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StopAnimationInLayer(this.handle, layer, blendOutTime.Milliseconds / 1000.0f);
		}
		/// <summary>
		/// Stops all animations.
		/// </summary>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool StopAnimation()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return StopAnimationsAllLayers(this.handle);
		}
		/// <summary>
		/// Sets the desired value to assign to animation's motion parameters.
		/// </summary>
		/// <param name="id">       Identifier of the motion parameter to set the value for.</param>
		/// <param name="value">    Value to set for the parameter.</param>
		/// <param name="frameTime">Latest frame time in seconds (?).</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetDesiredMotionParameter(MotionParameterId id, float value, float frameTime)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			SetDesiredMotionParam(this.handle, id, value, frameTime);
		}
		/// <summary>
		/// Gets the desired value of the motion parameter.
		/// </summary>
		/// <param name="id">   Identifier of the motion parameter.</param>
		/// <param name="value">Current value of the parameter.</param>
		/// <returns>True, if the value was set for the parameter before (?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetDesiredMotionParameter(MotionParameterId id, out float value)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetDesiredMotionParam(this.handle, id, out value);
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
		private static extern void SetDebugging(IntPtr handle, bool flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetAnimationDrivenMotion(IntPtr handle, bool ts);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetAnimationDrivenMotion(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetTrackViewStatus(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StartAnimation(IntPtr handle, string szAnimName0,
												  ref CharacterAnimationParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StartAnimationById(IntPtr handle, int id, ref CharacterAnimationParameters Params);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StopAnimationInLayer(IntPtr handle, int nLayer, float blendOutTime);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StopAnimationsAllLayers(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern CharacterAnimation FindAnimInFifo(IntPtr handle, uint nUserToken, int nLayer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool RemoveAnimFromFifo(IntPtr handle, uint nLayer, uint num, bool forceRemove);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetNumAnimsInFifo(IntPtr handle, uint nLayer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ClearFifoLayer(IntPtr handle, uint nLayer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern CharacterAnimation GetAnimFromFifo(IntPtr handle, uint nLayer, uint num);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ManualSeekAnimationInFifo(IntPtr handle, uint nLayer, uint num, float time,
															  bool triggerAnimEvents);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveTransitionDelayConditions(IntPtr handle, uint nLayer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetLayerBlendWeight(IntPtr handle, int nLayer, float fMult);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetLayerPlaybackScale(IntPtr handle, int nLayer, float fSpeed);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetLayerPlaybackScale(IntPtr handle, uint nLayer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDesiredMotionParam(IntPtr handle, MotionParameterId id, float value, float frametime);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetDesiredMotionParam(IntPtr handle, MotionParameterId id, out float value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetLayerNormalizedTime(IntPtr handle, uint layer, float normalizedTime);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetLayerNormalizedTime(IntPtr handle, uint layer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetCurrentVelocity(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRelMovement(IntPtr handle, out Quatvec movement);
		#endregion
	}
}