using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Memory
{
	/// <summary>
	/// Provides access to CryEngine memory management API.
	/// </summary>
	public static class CryMarshal
	{
		#region Fields
		private static readonly SortedList<IntPtr, ulong> allocatedBlocks;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the size of total size of memory that has been allocated by invoking
		/// <see cref="CryMarshal.Allocate"/> .
		/// </summary>
		public static ulong AllocatedMemory { get; private set; }
		#endregion
		#region Construction
		static CryMarshal()
		{
			allocatedBlocks = new SortedList<IntPtr, ulong>();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Allocates a native memory block.
		/// </summary>
		/// <param name="size"> Size of the block to allocate.</param>
		/// <param name="track">Indicates whether this class should track this memory block.</param>
		/// <returns>Pointer to allocated memory block.</returns>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public static IntPtr Allocate(ulong size, bool track = true)
		{
			IntPtr handle = AllocateMemory(size);
			if (handle == IntPtr.Zero)
			{
				throw new OutOfMemoryException("Unable to allocate native memory block.");
			}
			if (track)
			{
				allocatedBlocks.Add(handle, size);
				AllocatedMemory += size;
				GC.AddMemoryPressure((long)size);
			}
			return handle;
		}
		/// <summary>
		/// Reallocates a native memory block.
		/// </summary>
		/// <param name="handle">
		/// Pointer to the block that needs reallocation. If null is passed, then memory will be allocated.
		/// </param>
		/// <param name="size">  
		/// Size of the block to reallocate. If 0 is passed then memory will be released.
		/// </param>
		/// <param name="track"> 
		/// Indicates whether given memory block should be tracked by this class, if it wasn't already.
		/// </param>
		/// <returns>Pointer to new memory block, or <see cref="IntPtr.Zero"/> if it was released.</returns>
		/// <exception cref="OutOfMemoryException">Unable to reallocate native memory block.</exception>
		public static IntPtr Reallocate(IntPtr handle, ulong size, bool track = true)
		{
			if (handle == IntPtr.Zero && size == 0)
			{
				return IntPtr.Zero;
			}

			IntPtr reallocedHandle = ReallocateMemory(handle, size);

			if (reallocedHandle == IntPtr.Zero && size > 0)
			{
				throw new OutOfMemoryException("Unable to reallocate native memory block.");
			}

			int index = allocatedBlocks.IndexOfKey(handle);

			if (index > 0)
			{
				ulong oldSize = allocatedBlocks.Values[index];
				GC.RemoveMemoryPressure((long)oldSize);
				AllocatedMemory -= oldSize;
				// Update the registration info.
				if (handle == reallocedHandle)
				{
					allocatedBlocks.Values[index] = size;
					AllocatedMemory += size;
					GC.AddMemoryPressure((long)size);
					track = false;
				}
				else
				{
					allocatedBlocks.RemoveAt(index);
					track = true;
				}
			}
			if (track && size != 0)
			{
				allocatedBlocks.Add(reallocedHandle, size);
				GC.AddMemoryPressure((long)size);
				AllocatedMemory += size;
			}

			return reallocedHandle;
		}
		/// <summary>
		/// Releases native memory block.
		/// </summary>
		/// <param name="handle">Pointer to the memory block.</param>
		/// <param name="force"> 
		/// Indicates whether the memory block must be released even if it is not tracked by
		/// <see cref="CryMarshal"/> .
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Pointer to memory block to be released cannot be null.
		/// </exception>
		public static void Free(IntPtr handle, bool force = false)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentNullException(nameof(handle),
												"Pointer to memory block to be released cannot be null.");
			}

			int index = allocatedBlocks.IndexOfKey(handle);

			if (index > 0)
			{
				FreeMemory(handle);

				ulong size = allocatedBlocks.Values[index];

				AllocatedMemory -= size;
				GC.RemoveMemoryPressure((long)size);

				allocatedBlocks.RemoveAt(index);
			}
			else if (force)
			{
				FreeMemory(handle);
			}
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr AllocateMemory(ulong size);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr ReallocateMemory(IntPtr ptr, ulong size);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FreeMemory(IntPtr handle);
		#endregion
	}
}