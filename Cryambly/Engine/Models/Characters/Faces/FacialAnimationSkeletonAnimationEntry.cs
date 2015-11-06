using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a skeleton animation that plays during facial animation.
	/// </summary>
	public struct FacialAnimationSkeletonAnimationEntry
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
		/// Gets or sets the name of the file to use the animation from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string File
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetName(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetName(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the moment when this animation starts playing.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float StartTime
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetStartTime(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetStartTime(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the moment when this animation stops playing.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float EndTime
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetEndTime(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetEndTime(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal FacialAnimationSkeletonAnimationEntry(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface

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
		private static extern void SetName(IntPtr handle, string skeletonAnimationFile);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStartTime(IntPtr handle, float time);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetStartTime(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEndTime(IntPtr handle, float time);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetEndTime(IntPtr handle);
		#endregion
	}
}