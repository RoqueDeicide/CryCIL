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
		/// Gets a byte from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the byte we need.
		/// </param>
		/// <returns>
		/// 1-byte long struct that can be interpreted as a signed or unsigned 8-bit integer.
		/// </returns>
		public static Byte1 GetByte(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.GetByte(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.GetByte: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets two bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 2-byte long struct that can be interpreted as a signed or unsigned 16-bit integer, a
		/// half-precision floating point number or as a Unicode character.
		/// </returns>
		public static Bytes2 Get2Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get2Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get2Bytes: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets four bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 4-byte long struct that can be interpreted as a signed or unsigned 32-bit integer or a
		/// single-precision floating point number.
		/// </returns>
		public static Bytes4 Get4Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get4Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get4Bytes: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets eight bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 8-byte long struct that can be interpreted as a signed or unsigned 64-bit integer or a
		/// double-precision floating point number.
		/// </returns>
		public static Bytes8 Get8Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get8Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get8Bytes: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets 32 bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 32-byte long struct that can be interpreted as an array of a number of basic types.
		/// </returns>
		public static Buffer32 Get32Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get32Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get32Bytes: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets 64 bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 64-byte long struct that can be interpreted as an array of a number of basic types.
		/// </returns>
		public static Buffer64 Get64Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get64Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get64Bytes: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets 128 bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 128-byte long struct that can be interpreted as an array of a number of basic types.
		/// </returns>
		public static Buffer128 Get128Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get128Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get128Bytes: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets 256 bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 256-byte long struct that can be interpreted as an array of a number of basic types.
		/// </returns>
		public static Buffer256 Get256Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get256Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get256Bytes: Attempt to access native memory with zero-pointer.");
		}
		/// <summary>
		/// Gets 512 bytes from native memory cluster.
		/// </summary>
		/// <param name="pointer">Pointer that points to the beginning of the cluster.</param>
		/// <param name="shift">
		/// A number that should be added to address, contained in the <paramref name="pointer" />
		/// to get address of the bytes we need.
		/// </param>
		/// <returns>
		/// 512-byte long struct that can be interpreted as an array of a number of basic types.
		/// </returns>
		public static Buffer512 Get512Bytes(IntPtr pointer, ulong shift)
		{
			if (pointer.ToInt64() != 0)
			{
				return NativeMemoryHandlingMethods.Get512Bytes(pointer, shift);
			}
			throw new NullPointerException("CryMarshal.Get512Bytes: Attempt to access native memory with zero-pointer.");
		}
	}
}