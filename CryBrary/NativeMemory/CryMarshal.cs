using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CryEngine.Native;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Defines methods that allow to handle native memory.
	/// </summary>
	public static partial class CryMarshal
	{
		private static SortedList<long, ulong> allocatedClusters;
		/// <summary>
		/// Gets a size of native memory that has been allocated by this
		/// </summary>
		public static ulong AllocatedMemory { get; private set; }
		/// <summary>
		/// Allocate memory cluster of certain size.
		/// </summary>
		/// <param name="size">Number of bytes to allocate.</param>
		/// <returns>Pointer to first byte in the memory cluster.</returns>
		public static IntPtr AllocateMemory(ulong size)
		{
			if (size == 0)
			{
				throw new ArgumentOutOfRangeException
					("size", "Attempt to allocate a native memory cluster of 0 length.");
			}
			IntPtr pointer = NativeMemoryHandlingMethods.AllocateMemory(size);
			if (pointer == null)
			{
				throw new NativeMemoryException("CryMarshal.AllocateMemory: Unable to allocate memory.");
			}
			GC.AddMemoryPressure((long)size);
			allocatedClusters.Add(pointer.ToInt64(), size);
			AllocatedMemory += size;
			return pointer;
		}
		/// <summary>
		/// Frees memory cluster.
		/// </summary>
		/// <remarks>
		/// This function won't release memory that has not been allocated by <see
		/// cref="CryMarshal.AllocateMemory" />.
		/// </remarks>
		/// <param name="pointer">Pointer to memory cluster to free.</param>
		public static void FreeMemory(IntPtr pointer)
		{
			if (!allocatedClusters.ContainsKey(pointer.ToInt64())) return;

			NativeMemoryHandlingMethods.FreeMemory(pointer);
			ulong freedBytes = allocatedClusters[pointer.ToInt64()];
			GC.RemoveMemoryPressure((long)freedBytes);
			AllocatedMemory -= freedBytes;
		}
	}
	/// <summary>
	/// Represents exception that is thrown whenever there is a problem with native memory.
	/// </summary>
	[SerializableAttribute]
	public class NativeMemoryException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="NativeMemoryException" /> class.
		/// </summary>
		public NativeMemoryException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="NativeMemoryException" /> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public NativeMemoryException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="NativeMemoryException" /> class with specified
		/// message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">Exception that caused a new one to be created.</param>
		public NativeMemoryException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="NativeMemoryException" /> class with
		/// serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected NativeMemoryException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}