using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Physics;

namespace CryCil.Utilities
{
	/// <summary>
	/// Represents a strided pointer.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct StridedPointer
	{
		#region Fields
		[UsedImplicitly] private void* data;
		[UsedImplicitly] private readonly int stride;
		/// <summary>
		/// Represents a strided pointer that should be ignored by the physics system.
		/// </summary>
		public static readonly StridedPointer Unused = new StridedPointer
		{
			data = UnusedValue.Pointer.ToPointer()
		};
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is valid.
		/// </summary>
		public bool IsValid
		{
			get { return this.data != null && this.stride > 0; }
		}
		internal void* Pointer
		{
			get { return this.data; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets one of the elements this strided pointer represents.
		/// </summary>
		/// <param name="index">Zero-based index of the element.</param>
		/// <returns>A vector.</returns>
		[Pure]
		public Vector3 GetVector3(int index)
		{
			this.AssertInstanceValidity();
			return *(Vector3*)((byte*)this.data + index * this.stride);
		}
		#endregion
		#region Utilities
		private void AssertInstanceValidity()
		{
			if (this.data == null)
			{
				throw new NullReferenceException("Attempted to dereference null strided pointer.");
			}
			if (this.stride <= 0)
			{
				throw new InvalidOperationException("Attempted to dereference a strided pointer with invalid stride.");
			}
		}
		#endregion
	}
}