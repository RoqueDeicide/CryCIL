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
		/// Writes <see cref="Byte" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, byte value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.SetByte(pointer, shift, value);
		}
		/// <summary>
		/// Writes <see cref="SByte" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, sbyte value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			Byte1 val = new Byte1
			{
				SignedByte = value
			};
			NativeMemoryHandlingMethods.SetByte(pointer, shift, val.UnsignedByte);
		}
		/// <summary>
		/// Writes <see cref="Char" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, char value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			Bytes2 val = new Bytes2
			{
				Character = value
			};
			NativeMemoryHandlingMethods.Set2Bytes(pointer, shift, val.UnsignedShort);
		}
		/// <summary>
		/// Writes <see cref="UInt16" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, ushort value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set2Bytes(pointer, shift, value);
		}
		/// <summary>
		/// Writes <see cref="Int16" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, short value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			Bytes2 val = new Bytes2
			{
				SignedShort = value
			};
			NativeMemoryHandlingMethods.Set2Bytes(pointer, shift, val.UnsignedShort);
		}
		/// <summary>
		/// Writes <see cref="UInt32" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, uint value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set4Bytes(pointer, shift, value);
		}
		/// <summary>
		/// Writes <see cref="Int32" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, int value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			Bytes4 val = new Bytes4
			{
				SignedInt = value
			};
			NativeMemoryHandlingMethods.Set4Bytes(pointer, shift, val.UnsignedInt);
		}
		/// <summary>
		/// Writes <see cref="Single" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, float value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			Bytes4 val = new Bytes4
			{
				SingleFloat = value
			};
			NativeMemoryHandlingMethods.Set4Bytes(pointer, shift, val.UnsignedInt);
		}
		/// <summary>
		/// Writes <see cref="UInt64" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, ulong value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set8Bytes(pointer, shift, value);
		}
		/// <summary>
		/// Writes <see cref="Int64" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, long value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			Bytes8 val = new Bytes8
			{
				SignedLong = value
			};
			NativeMemoryHandlingMethods.Set8Bytes(pointer, shift, val.UnsignedLong);
		}
		/// <summary>
		/// Writes <see cref="Double" /> value to the native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="value">Value to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the value to.
		/// </param>
		public static void Set(IntPtr pointer, double value, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			Bytes8 val = new Bytes8
			{
				DoubleFloat = value
			};
			NativeMemoryHandlingMethods.Set8Bytes(pointer, shift, val.UnsignedLong);
		}
		/// <summary>
		/// Writes 32-byte long buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="buffer">Data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the data to.
		/// </param>
		public static void Set(IntPtr pointer, ref Buffer32 buffer, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set32Bytes(pointer, shift, buffer);
		}
		/// <summary>
		/// Writes 64-byte long buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="buffer">Data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the data to.
		/// </param>
		public static void Set(IntPtr pointer, ref Buffer64 buffer, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set64Bytes(pointer, shift, buffer);
		}
		/// <summary>
		/// Writes 128-byte long buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="buffer">Data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the data to.
		/// </param>
		public static void Set(IntPtr pointer, ref Buffer128 buffer, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set128Bytes(pointer, shift, buffer);
		}
		/// <summary>
		/// Writes 256-byte long buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="buffer">Data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the data to.
		/// </param>
		public static void Set(IntPtr pointer, ref Buffer256 buffer, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set256Bytes(pointer, shift, buffer);
		}
		/// <summary>
		/// Writes 512-byte long buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="buffer">Data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the data to.
		/// </param>
		public static void Set(IntPtr pointer, ref Buffer512 buffer, ulong shift)
		{
			if (pointer.ToInt64() == 0)
			{
				return;
			}
			NativeMemoryHandlingMethods.Set512Bytes(pointer, shift, buffer);
		}
		/// <summary>
		/// Writes a buffer to native memory.
		/// </summary>
		/// <param name="pointer">Pointer to beginning of native memory cluster.</param>
		/// <param name="buffer">Data to write.</param>
		/// <param name="shift">
		/// Address of the byte within the cluster to which to write the data to.
		/// </param>
		public static void Set(IntPtr pointer, IBuffer buffer, ulong shift)
		{
			if (pointer == IntPtr.Zero)
			{
				return;
			}
			switch (buffer.Length)
			{
				case 1:
					NativeMemoryHandlingMethods.SetByte(pointer, shift, ((Byte1)buffer).UnsignedByte);
					break;
				case 2:
					NativeMemoryHandlingMethods.Set2Bytes(pointer, shift, ((Bytes2)buffer).UnsignedShort);
					break;
				case 4:
					NativeMemoryHandlingMethods.Set4Bytes(pointer, shift, ((Bytes4)buffer).UnsignedInt);
					break;
				case 8:
					NativeMemoryHandlingMethods.Set8Bytes(pointer, shift, ((Bytes8)buffer).UnsignedLong);
					break;
				case 32:
					NativeMemoryHandlingMethods.Set32Bytes(pointer, shift, (Buffer32)buffer);
					break;
				case 64:
					NativeMemoryHandlingMethods.Set64Bytes(pointer, shift, (Buffer64)buffer);
					break;
				case 128:
					NativeMemoryHandlingMethods.Set128Bytes(pointer, shift, (Buffer128)buffer);
					break;
				case 256:
					NativeMemoryHandlingMethods.Set256Bytes(pointer, shift, (Buffer256)buffer);
					break;
				case 512:
					NativeMemoryHandlingMethods.Set512Bytes(pointer, shift, (Buffer512)buffer);
					break;
			}
		}
	}
}