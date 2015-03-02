using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Represents an object that makes management of native memory a little safer.
	/// </summary>
	public class NativeMemoryBlock : IDisposable
	{
		#region Properties
		/// <summary>
		/// Gets pointer to the beginning of the memory block.
		/// </summary>
		public IntPtr Handle { get; private set; }
		/// <summary>
		/// Gets number of bytes in this memory block.
		/// </summary>
		public ulong Size { get; private set; }
		/// <summary>
		/// Indicates whether this memory block is no longer in use.
		/// </summary>
		public bool Disposed { get; private set; }
		/// <summary>
		/// Indicates whether this memory block will release the memory when disposed of.
		/// </summary>
		public bool Managed { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeMemoryBlock"/> while allocating memory for it.
		/// </summary>
		/// <param name="size">Size of the memory block in bytes to allocate.</param>
		public NativeMemoryBlock(ulong size)
		{
			if (size == 0)
			{
				this.Disposed = true;
				this.Size = 0;
				this.Handle = IntPtr.Zero;
				this.Managed = false;
			}
			else
			{
				this.Size = size;
				this.Handle = CryMarshal.Allocate(size);
				if (this.Handle == IntPtr.Zero)
				{
					throw new OutOfMemoryException("Unable to allocate native memory block.");
				}
				this.Managed = true;
				this.Disposed = false;
			}
		}
		/// <summary>
		/// Creates new instance of type <see cref="NativeMemoryBlock"/> while allocating memory for it.
		/// </summary>
		/// <param name="objCount">Number of object this memory block will store.</param>
		/// <param name="type">    Type of object that will be stored in this memory block.</param>
		public NativeMemoryBlock(int objCount, Type type)
			: this((ulong)(Marshal.SizeOf(type) * objCount))
		{
		}
		~NativeMemoryBlock()
		{
			this.Dispose(false);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Marks this memory block as not used anymore.
		/// </summary>
		/// <param name="freeManagedRes">Doesn't really matter.</param>
		public void Dispose(bool freeManagedRes)
		{
			if (this.Disposed)
			{
				return;
			}
			this.Disposed = true;
			if (this.Managed)
			{
				CryMarshal.Free(this.Handle, true);
			}
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Marks this memory block as not used anymore.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}
		#endregion
		#region Utilities

		#endregion
	}
}