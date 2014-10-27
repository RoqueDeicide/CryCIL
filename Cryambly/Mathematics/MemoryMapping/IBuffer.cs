using System;
using System.Diagnostics.Contracts;

namespace CryCil.Mathematics.MemoryMapping
{
	/// <summary>
	/// Defines common properties of buffer structures.
	/// </summary>
	[ContractClass(typeof(IBufferContract))]
	public interface IBuffer
	{
		/// <summary>
		/// Gets length of the buffer in bytes.
		/// </summary>
		int Length { get; }
		/// <summary>
		/// When implemented in derived class or structure, gets or sets one byte in this
		/// buffer.
		/// </summary>
		/// <param name="index">Zero-based index of the byte to get or set.</param>
		byte this[int index] { get; set; }
		/// <summary>
		/// When implemented in derived type, fills this buffer with data from native
		/// memory.
		/// </summary>
		/// <param name="handle">
		/// Address of the memory cluster from which to get the data.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of the first byte to get from native memory within the
		/// cluster.
		/// </param>
		void Get(IntPtr handle, int offset);
		/// <summary>
		/// When implemented in derived type, transfers data from this buffer to native
		/// memory.
		/// </summary>
		/// <param name="handle">
		/// Address of the memory cluster to write the data into.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of the first byte within the cluster to write.
		/// </param>
		void Set(IntPtr handle, int offset);
	}
	[ContractClassFor(typeof(IBuffer))]
	internal abstract class IBufferContract : IBuffer
	{
		int IBuffer.Length
		{
			get
			{
				Contract.Ensures(Contract.Result<int>() > 0, "Length of the buffer must be greater then zero.");
				return default(int);
			}
		}
		byte IBuffer.this[int index]
		{
			get
			{
				Contract.Requires(index > -1, "Index of the byte to access must be greater or equal to zero.");
				Contract.Requires(index < ((IBuffer)this).Length, "Index of the byte to access must be less then buffer length.");
				return default(int);
			}
			// ReSharper disable ValueParameterNotUsed
			set
			// ReSharper restore ValueParameterNotUsed
			{
				Contract.Requires(index > -1, "Index of the byte to access must be greater or equal to zero.");
				Contract.Requires(index < ((IBuffer)this).Length, "Index of the byte to access must be less then buffer length.");
			}
		}
		void IBuffer.Get(IntPtr handle, int offset)
		{
			Contract.Requires(handle != IntPtr.Zero, "Pointer to memory block may not be null.");
		}
		void IBuffer.Set(IntPtr handle, int offset)
		{
			Contract.Requires(handle != IntPtr.Zero, "Pointer to memory block may not be null.");
		}
	}
}