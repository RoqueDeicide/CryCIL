using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
		[UsedImplicitly] private byte* data;
		[UsedImplicitly] private readonly int stride;
		/// <summary>
		/// Represents a strided pointer that should be ignored by the physics system.
		/// </summary>
		public static readonly StridedPointer Unused = new StridedPointer
		{
			data = (byte*)UnusedValue.Pointer.ToPointer()
		};
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is valid.
		/// </summary>
		public bool IsValid => this.data != null && this.stride > 0;
		internal void* Pointer => this.data;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="data">  Pointer to strided data.</param>
		/// <param name="stride">Length of the stride in bytes.</param>
		public StridedPointer(void* data, int stride)
		{
			this.data = (byte*)data;
			this.stride = stride;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets one of the elements this strided pointer represents.
		/// </summary>
		/// <param name="index">Zero-based index of the element.</param>
		/// <returns>A vector.</returns>
		/// <exception cref="NullReferenceException">
		/// Attempted to dereference null strided pointer.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Attempted to dereference a strided pointer with invalid stride.
		/// </exception>
		[Pure]
		public Vector3 GetVector3(int index)
		{
			this.AssertInstanceValidity();
			return *(Vector3*)(this.data + index * this.stride);
		}
		/// <summary>
		/// Gets the pointer to the element.
		/// </summary>
		/// <param name="index">Zero-based index of the element to get the pointer to.</param>
		/// <returns>Internal pointer advanced by <paramref name="index"/> * stride.</returns>
		/// <exception cref="NullReferenceException">
		/// Attempted to dereference null strided pointer.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Attempted to dereference a strided pointer with invalid stride.
		/// </exception>
		[Pure]
		[SuppressMessage("ReSharper", "PureAttributeOnVoidMethod")]
		public void* GetElement(int index)
		{
			this.AssertInstanceValidity();
			return this.data + index * this.stride;
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">
		/// Attempted to dereference null strided pointer.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Attempted to dereference a strided pointer with invalid stride.
		/// </exception>
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