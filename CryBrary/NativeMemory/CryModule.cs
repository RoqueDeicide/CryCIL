using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CryEngine.Native;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Defines even less safe ways of working with native memory.
	/// </summary>
	public static class CryModule
	{
		/// <summary>
		/// Allocates an array of bytes of given size within native memory.
		/// </summary>
		/// <remarks>
		/// No checks are done by this method.
		/// </remarks>
		/// <param name="size">Number of bytes to allocate.</param>
		/// <returns>Pointer to the first byte.</returns>
		public static IntPtr AllocateMemory(ulong size)
		{
			return NativeMemoryHandlingMethods.AllocateMemory(size);
		}
		/// <summary>
		/// Frees memory.
		/// </summary>
		/// <remarks>
		/// No checks are done by this method.
		/// </remarks>
		/// <param name="handle">Pointer to first byte of memory cluster to release.</param>
		public static void FreeMemory(IntPtr handle)
		{
			NativeMemoryHandlingMethods.FreeMemory(handle);
		}
	}
}