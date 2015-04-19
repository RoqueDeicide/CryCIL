using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Memory
{
	/// <summary>
	/// Represents a pointer to native memory block that will be released when the pointer is GCed of
	/// disposed of.
	/// </summary>
	public sealed class DisposablePointer
	{
		#region Fields
		private IntPtr handle;
		#endregion
		#region Construction
		/// <summary>
		/// Allocates new disposable memory block.
		/// </summary>
		/// <param name="size">Size of the block to allocate.</param>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public DisposablePointer(ulong size)
		{
			this.handle = CryMarshal.Allocate(size);
		}
		/// <summary>
		/// Releases underlying memory.
		/// </summary>
		~DisposablePointer()
		{
			this.Dispose(false);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Marks this memory block as not used anymore.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}
		/// <summary>
		/// Allows <see cref="DisposablePointer"/> to be used as <see cref="IntPtr"/>.
		/// </summary>
		/// <param name="ptr">An object which underlying pointer is going to be used somewhere.</param>
		/// <returns>Underlying pointer that is a part of given disposable pointer.</returns>
		public static implicit operator IntPtr(DisposablePointer ptr)
		{
			return ptr.handle;
		}
		#endregion
		#region Utilities
		/// <summary>
		/// Marks this memory block as not used anymore.
		/// </summary>
		/// <param name="suppressFinalize">
		/// Indicates whether <see cref="Finalize"/> method should be suppressed..
		/// </param>
		private void Dispose(bool suppressFinalize)
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}

			this.handle = CryMarshal.Reallocate(this.handle, 0);

			if (suppressFinalize)
			{
				GC.SuppressFinalize(this);
			}
		}
		#endregion
	}
}