using System;
using System.Diagnostics.Contracts;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents a layer of skeleton animation.
	/// </summary>
	public struct SkeletonAnimationLayer
	{
		#region Fields
		private readonly IntPtr handle;
		private readonly uint index;
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
		/// Gets the number of animations in this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonAnimation.GetNumAnimsInFifo(this.handle, this.index);
			}
		}
		/// <summary>
		/// Gets the animation in this layer.
		/// </summary>
		/// <param name="index">Zero-based index of the animation object to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the animation must be less then number of animations in the layer. Only thrown in
		/// Debug buids.
		/// </exception>
		public CharacterAnimation this[uint index]
		{
			get
			{
				this.AssertInstance();
#if DEBUG
				if (index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index of the animation must be less then number of animations in the layer.");
				}
#endif
				Contract.EndContractBlock();

				return SkeletonAnimation.GetAnimFromFifo(this.handle, this.index, index);
			}
		}
		/// <summary>
		/// Sets the blending weight this layer has.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float BlendWeight
		{
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonAnimation.SetLayerBlendWeight(this.handle, (int)this.index, value);
			}
		}
		/// <summary>
		/// Gets or sets the scale of animation playback speed for this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float PlaybackScale
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonAnimation.GetLayerPlaybackScale(this.handle, this.index);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonAnimation.SetLayerPlaybackScale(this.handle, (int)this.index, value);
			}
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that represents the normalized time of all animations on
		/// this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float NormalizedTime
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonAnimation.GetLayerNormalizedTime(this.handle, this.index);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonAnimation.SetLayerNormalizedTime(this.handle, this.index, value);
			}
		}
		#endregion
		#region Construction
		internal SkeletonAnimationLayer(IntPtr handle, uint index)
		{
			this.handle = handle;
			this.index = index;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Looks for an animation in this layer.
		/// </summary>
		/// <param name="userToken">A token that identifies the animation to get.</param>
		/// <returns>
		/// A valid object that represents the animation if found, otherwise an invalid object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CharacterAnimation Find(uint userToken)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return SkeletonAnimation.FindAnimInFifo(this.handle, userToken, (int)this.index);
		}
		/// <summary>
		/// Removes the animation from this layer.
		/// </summary>
		/// <param name="index">Zero-based index of the animation to remove.</param>
		/// <param name="force">
		/// Indicates whether animation must be removed regardless of special conditions.
		/// </param>
		/// <returns>True, if animation was actually removed.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool RemoveAt(uint index, bool force = false)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return SkeletonAnimation.RemoveAnimFromFifo(this.handle, this.index, index, force);
		}
		/// <summary>
		/// Advances the time on the animation. Used when animation has
		/// <see cref="AnimationFlags.ManualUpdate"/> flag set.
		/// </summary>
		/// <param name="index">        Zero-based index of the animation to advance.</param>
		/// <param name="time">         Advancement time in seconds(?).</param>
		/// <param name="triggerEvents">
		/// Indicates whether animation events must be triggered by this advancement.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Advance(uint index, float time, bool triggerEvents)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			SkeletonAnimation.ManualSeekAnimationInFifo(this.handle, this.index, index, time, triggerEvents);
		}
		/// <summary>
		/// Advances the time on the animation. Used when animation has
		/// <see cref="AnimationFlags.ManualUpdate"/> flag set.
		/// </summary>
		/// <param name="index">        Zero-based index of the animation to advance.</param>
		/// <param name="time">         Advancement time.</param>
		/// <param name="triggerEvents">
		/// Indicates whether animation events must be triggered by this advancement.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Advance(uint index, TimeSpan time, bool triggerEvents)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			SkeletonAnimation.ManualSeekAnimationInFifo(this.handle, this.index, index, time.Milliseconds / 1000.0f,
														triggerEvents);
		}
		/// <summary>
		/// Makes sure that there is no animation that can cause a delay in transition. Use it when you
		/// need to have an animation transitioned to immediately.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveTransitionDelayConditions()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			SkeletonAnimation.RemoveTransitionDelayConditions(this.handle, this.index);
		}
		/// <summary>
		/// Clears this layer of all animations.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Clear()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			SkeletonAnimation.ClearFifoLayer(this.handle, this.index);
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
		#endregion
	}
	/// <summary>
	/// Represents a collection of animation layers in objects of type <see cref="SkeletonAnimation"/>.
	/// </summary>
	public struct SkeletonAnimationLayers
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
		/// Gets the object that represents a layer of skeleton animation.
		/// </summary>
		/// <param name="index">Zero-based index of the layer.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException" accessor="get">
		/// Index of the layer must be less then 16.
		/// </exception>
		public SkeletonAnimationLayer this[uint index]
		{
			get
			{
				this.AssertInstance();
				if (index >= 16)
				{
					throw new IndexOutOfRangeException("Index of the layer must be less then 16.");
				}
				Contract.EndContractBlock();

				return new SkeletonAnimationLayer(this.handle, index);
			}
		}
		/// <summary>
		/// Sets the blending weight all layers have.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float BlendWeight
		{
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonAnimation.SetLayerBlendWeight(this.handle, -1, value);
			}
		}
		/// <summary>
		/// Sets the scale of animation playback speed for all layers.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float PlaybackScale
		{
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonAnimation.SetLayerPlaybackScale(this.handle, -1, value);
			}
		}
		#endregion
		#region Construction
		internal SkeletonAnimationLayers(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Looks for an animation in all layers.
		/// </summary>
		/// <param name="userToken">A token that identifies the animation to get.</param>
		/// <returns>
		/// A valid object that represents the animation if found, otherwise an invalid object.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CharacterAnimation Find(uint userToken)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return SkeletonAnimation.FindAnimInFifo(this.handle, userToken, -1);
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
		#endregion
	}
}