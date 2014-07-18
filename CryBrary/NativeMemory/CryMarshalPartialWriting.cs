using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine.NativeMemory
{
	public static partial class CryMarshal
	{
		/// <summary>
		/// Writes a portion of buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Buffer that provides data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		public static void Set(IntPtr pointer, Bytes4 value, ulong shift, ulong count)
		{
			CryMarshal.Set<Bytes4>(pointer, value, NativeMemoryHandlingMethods.Set4BytesPartial, shift, count);
		}
		/// <summary>
		/// Writes a portion of buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Buffer that provides data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		public static void Set(IntPtr pointer, Bytes8 value, ulong shift, ulong count)
		{
			CryMarshal.Set<Bytes8>(pointer, value, NativeMemoryHandlingMethods.Set8BytesPartial, shift, count);
		}
		/// <summary>
		/// Writes a portion of buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Buffer that provides data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		public static void Set(IntPtr pointer, Buffer32 value, ulong shift, ulong count)
		{
			CryMarshal.Set<Buffer32>(pointer, value, NativeMemoryHandlingMethods.Set32BytesPartial, shift, count);
		}
		/// <summary>
		/// Writes a portion of buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Buffer that provides data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		public static void Set(IntPtr pointer, Buffer64 value, ulong shift, ulong count)
		{
			CryMarshal.Set<Buffer64>(pointer, value, NativeMemoryHandlingMethods.Set64BytesPartial, shift, count);
		}
		/// <summary>
		/// Writes a portion of buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Buffer that provides data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		public static void Set(IntPtr pointer, Buffer128 value, ulong shift, ulong count)
		{
			CryMarshal.Set<Buffer128>(pointer, value, NativeMemoryHandlingMethods.Set128BytesPartial, shift, count);
		}
		/// <summary>
		/// Writes a portion of buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Buffer that provides data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		public static void Set(IntPtr pointer, Buffer256 value, ulong shift, ulong count)
		{
			CryMarshal.Set<Buffer256>(pointer, value, NativeMemoryHandlingMethods.Set256BytesPartial, shift, count);
		}
		/// <summary>
		/// Writes a portion of buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Buffer that provides data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		/// <param name="count">Number of bytes to write.</param>
		public static void Set(IntPtr pointer, Buffer512 value, ulong shift, ulong count)
		{
			CryMarshal.Set<Buffer512>(pointer, value, NativeMemoryHandlingMethods.Set512BytesPartial, shift, count);
		}

		private delegate void Setter<in BufferType>(IntPtr pointer, ulong shift, ulong count, BufferType value)
			where BufferType : IBuffer;
		private static void Set<BufferType>(IntPtr pointer, IBuffer value, Setter<BufferType> setter, ulong shift, ulong count)
			where BufferType : IBuffer
		{
			if (pointer == IntPtr.Zero)
			{
				throw new NullPointerException("Attempt to write to a null pointer.");
			}
			if (count >= value.Length)
			{
				throw new ArgumentOutOfRangeException
					("count", "Number of bytes to write should not be greater then length of buffer.");
			}
			setter(pointer, shift, count, (BufferType)value);
		}
	}
}