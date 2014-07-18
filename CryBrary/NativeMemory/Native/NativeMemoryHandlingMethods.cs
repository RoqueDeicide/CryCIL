using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CryEngine.Initialization;
using CryEngine.NativeMemory;

namespace CryEngine.Native
{
	internal static class NativeMemoryHandlingMethods
	{
		/// <summary>
		/// Allocates <paramref name="size" /> of bytes of memory.
		/// </summary>
		/// <param name="size">Number of bytes to allocate.</param>
		/// <returns>Pointer to the first byte.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static IntPtr AllocateMemory(ulong size);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void FreeMemory(IntPtr pointer);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Byte1 GetByte(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Bytes2 Get2Bytes(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Bytes4 Get4Bytes(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Bytes8 Get8Bytes(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Buffer32 Get32Bytes(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Buffer64 Get64Bytes(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Buffer128 Get128Bytes(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Buffer256 Get256Bytes(IntPtr pointer, ulong shift);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static Buffer512 Get512Bytes(IntPtr pointer, ulong shift);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void SetByte(IntPtr pointer, ulong shift, byte value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set2Bytes(IntPtr pointer, ulong shift, ushort value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set4Bytes(IntPtr pointer, ulong shift, uint value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set8Bytes(IntPtr pointer, ulong shift, ulong value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set32Bytes(IntPtr pointer, ulong shift, Buffer32 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set64Bytes(IntPtr pointer, ulong shift, Buffer64 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set128Bytes(IntPtr pointer, ulong shift, Buffer128 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set256Bytes(IntPtr pointer, ulong shift, Buffer256 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set512Bytes(IntPtr pointer, ulong shift, Buffer512 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set4BytesPartial(IntPtr pointer, ulong shift, ulong count, Bytes4 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set8BytesPartial(IntPtr pointer, ulong shift, ulong count, Bytes8 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set32BytesPartial(IntPtr pointer, ulong shift, ulong count, Buffer32 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set64BytesPartial(IntPtr pointer, ulong shift, ulong count, Buffer64 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set128BytesPartial(IntPtr pointer, ulong shift, ulong count, Buffer128 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set256BytesPartial(IntPtr pointer, ulong shift, ulong count, Buffer256 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static void Set512BytesPartial(IntPtr pointer, ulong shift, ulong count, Buffer512 value);
	}
}