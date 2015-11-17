using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Geometry.Splines;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a collection of splines that specifies how the facial effector changes over the course
	/// of animation.
	/// </summary>
	public struct FacialAnimationChannelCurves
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
		/// Gets the number of splines in this channel.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialAnimationChannel.GetInterpolatorCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the spline in this channel.
		/// </summary>
		/// <param name="index">Zero-based index of the spline to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public CryEngineSpline this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Index is out of range.");
				}

				return FacialAnimationChannel.GetInterpolator(this.handle, index);
			}
		}
		/// <summary>
		/// Gets the last spline in this channel.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryEngineSpline Last
		{
			get
			{
				this.AssertInstance();

				return FacialAnimationChannel.GetLastInterpolator(this.handle);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationChannelCurves(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds a new default spline to this channel.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Add()
		{
			this.AssertInstance();

			FacialAnimationChannel.AddInterpolator(this.handle);
		}
		/// <summary>
		/// Removes a spline.
		/// </summary>
		/// <param name="index">Zero-based index of the spline to remove.</param>
		/// <returns>True, if index was in range.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool RemoveAt(int index)
		{
			this.AssertInstance();

			if (index < 0 || index >= this.Count)
			{
				return false;
			}
			FacialAnimationChannel.DeleteInterpolator(this.handle, index);
			return true;
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
	/// Represents a channel that specifies how the facial effector changes during the animation.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct FacialAnimationChannel
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to the collection of splines that specify this channel.
		/// </summary>
		[FieldOffset(0)] public readonly FacialAnimationChannelCurves Curves;
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
		/// Gets or sets the identifier of this channel.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FaceIdentifier Identifier
		{
			get
			{
				this.AssertInstance();

				return GetIdentifier(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetIdentifier(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the effector this channel controls.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FaceIdentifier EffectorIdentifier
		{
			get
			{
				this.AssertInstance();

				return GetEffectorIdentifier(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetEffectorIdentifier(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the parent channel for this one.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialAnimationChannel Parent
		{
			get
			{
				this.AssertInstance();

				return GetParent(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetParent(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify this channel.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialAnimationChannelFlags Flags
		{
			get
			{
				this.AssertInstance();

				return GetFlags(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetFlags(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets a facial effector this channel controls.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialEffector Effector
		{
			get
			{
				this.AssertInstance();

				return GetEffector(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetEffector(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationChannel(IntPtr handle)
			: this()
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Removes keys that have too much deviation(?).
		/// </summary>
		/// <param name="errorMax">Maximal allowed deviation(?).</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void CleanupKeys(float errorMax)
		{
			this.AssertInstance();

			CleanupKeysInternal(this.handle, errorMax);
		}
		/// <summary>
		/// Smooths the curves in this channel.
		/// </summary>
		/// <param name="sigma">Smoothing factor.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SmoothKeys(float sigma)
		{
			this.AssertInstance();

			SmoothKeysInternal(this.handle, sigma);
		}
		/// <summary>
		/// Removes noise and smooths curves in this channel.
		/// </summary>
		/// <param name="sigma">    Smoothing factor.</param>
		/// <param name="threshold">Some threshold.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveNoise(float sigma, float threshold)
		{
			this.AssertInstance();

			RemoveNoiseInternal(this.handle, sigma, threshold);
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
		private static extern void SetIdentifier(IntPtr handle, FaceIdentifier ident);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FaceIdentifier GetIdentifier(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEffectorIdentifier(IntPtr handle, FaceIdentifier ident);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FaceIdentifier GetEffectorIdentifier(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetParent(IntPtr handle, FacialAnimationChannel pParent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationChannel GetParent(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, FacialAnimationChannelFlags nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationChannelFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEffector(IntPtr handle, FacialEffector pEffector);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffector GetEffector(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern CryEngineSpline GetInterpolator(IntPtr handle, int i);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern CryEngineSpline GetLastInterpolator(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddInterpolator(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DeleteInterpolator(IntPtr handle, int i);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetInterpolatorCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CleanupKeysInternal(IntPtr handle, float fErrorMax);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SmoothKeysInternal(IntPtr handle, float sigma);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveNoiseInternal(IntPtr handle, float sigma, float threshold);
		#endregion
	}
}