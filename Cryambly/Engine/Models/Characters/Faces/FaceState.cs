using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a collection of weights of various effectors.
	/// </summary>
	public struct FaceState
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
		/// Gets or sets the weight of the effector.
		/// </summary>
		/// <param name="index">Zero-based index of the effector.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float this[int index]
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetEffectorWeight(this.handle, index);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetEffectorWeight(this.handle, index, value);
			}
		}
		#endregion
		#region Construction
		internal FaceState(IntPtr handle)
		{
			this.handle = handle;
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
		private static extern float GetEffectorWeight(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEffectorWeight(IntPtr handle, int nIndex, float fWeight);
		#endregion
	}
}