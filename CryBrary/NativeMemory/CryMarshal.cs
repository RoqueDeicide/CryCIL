using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Defines methods that allow to allocate and free native memory from Mono.
	/// </summary>
	/// <remarks>This class uses CryEngine-specific functions for memory allocation and release.</remarks>
	public static class CryMarshal
	{
		private static readonly SortedList<IntPtr, ulong> allocatedBlocks;
		/// <summary>
		/// Gets the size of total size of memory that has been allocated by invoking
		/// <see cref="CryMarshal.Allocate"/> .
		/// </summary>
		public static ulong AllocatedMemory { get; private set; }
		/// <summary>
		/// Allocates a native memory block.
		/// </summary>
		/// <param name="size">   Size of the block to allocate.</param>
		/// <param name="dispose">Indicates whether this class should track this memory block.</param>
		/// <returns>Pointer to allocated memory block.</returns>
		public static IntPtr Allocate(ulong size, bool dispose = true)
		{
			IntPtr handle = Native.NativeMemoryHandlingMethods.AllocateMemory(size);
			if (handle == IntPtr.Zero)
			{
				throw new OutOfMemoryException("Unable to allocate native memory block.");
			}
			if (dispose)
			{
				CryMarshal.allocatedBlocks.Add(handle, size);
				CryMarshal.AllocatedMemory += size;
				GC.AddMemoryPressure((long)size);
			}
			return handle;
		}
		/// <summary>
		/// Releases native memory block.
		/// </summary>
		/// <param name="handle">Pointer to the memory block.</param>
		/// <param name="force"> 
		/// Indicates whether the memory block must be released even if it is not tracked by
		/// <see cref="CryMarshal"/> .
		/// </param>
		public static void Free(IntPtr handle, bool force = false)
		{
			if (CryMarshal.allocatedBlocks.ContainsKey(handle))
			{
				Native.NativeMemoryHandlingMethods.FreeMemory(handle);
				CryMarshal.AllocatedMemory -= CryMarshal.allocatedBlocks[handle];
				GC.RemoveMemoryPressure((long)CryMarshal.allocatedBlocks[handle]);
				CryMarshal.allocatedBlocks.Remove(handle);
			}
			else if (force)
			{
				Native.NativeMemoryHandlingMethods.FreeMemory(handle);
			}
		}
	}
}