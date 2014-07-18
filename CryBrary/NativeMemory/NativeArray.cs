using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CryEngine;
using CryEngine.Native;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Represents a wrapper object around a native C++ array of data.
	/// </summary>
	public class NativeArray : INativeMemoryWrapper
	{
		#region Properties
		/// <summary>
		/// Gets pointer to the first byte of array inside CryModule memory.
		/// </summary>
		public IntPtr Handle { get; private set; }
		/// <summary>
		/// Indicates whether this array has been disposed of.
		/// </summary>
		public bool Disposed { get; private set; }
		/// <summary>
		/// Gets length of the array in bytes.
		/// </summary>
		public ulong Length { get; private set; }
		/// <summary>
		/// Indicates whether this object is going to free all native memory when disposed.
		/// </summary>
		public bool Managing { get; private set; }
		/// <summary>
		/// Indicates whether native memory was allocated by calling <see
		/// cref="CryMarshal.AllocateMemory" />.
		/// </summary>
		public bool AllocatedByCryMarshal { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of <see cref="NativeArray" /> type.
		/// </summary>
		/// <param name="size">Length of the array.</param>
		public NativeArray(ulong size)
		{
			this.Handle = CryMarshal.AllocateMemory(size);
			this.AllocatedByCryMarshal = true;
			this.Managing = true;
			this.Length = size;
			this.Disposed = false;
		}
		/// <summary>
		/// Creates new instance of <see cref="NativeArray" /> type.
		/// </summary>
		/// <param name="array">Bytes to write to native memory.</param>
		public NativeArray(ICollection<byte> array)
		{
			this.Handle = CryMarshal.AllocateMemory((ulong)array.Count);
			this.Length = (ulong)array.Count;
			this.Managing = true;
			this.AllocatedByCryMarshal = true;
			this.Disposed = false;
			NativeMemoryStream stream = new NativeMemoryStream(this, StreamMode.Write);
			if (array is byte[])
			{
				stream.Write(array as byte[]);
			}
			else
			{
				stream.Write(array.ToArray());
			}
		}
		/// <summary>
		/// Creates new instance of <see cref="NativeArray" /> type.
		/// </summary>
		/// <param name="handle">
		/// Pointer to first byte of memory cluster that is already allocated.
		/// </param>
		/// <param name="size">Size of allocated memory.</param>
		/// <param name="manageMemory">
		/// Indicates whether this <see cref="NativeArray" /> object should free native memory when disposed.
		/// </param>
		public NativeArray(IntPtr handle, ulong size, bool manageMemory, bool allocatedByCryMarshal)
		{
			this.Handle = handle;
			this.Length = size;
			this.Managing = manageMemory;
			this.AllocatedByCryMarshal = allocatedByCryMarshal;
			this.Length = size;
			this.Disposed = false;
		}
		#endregion
		#region Disposal
		~NativeArray()
		{
			if (!this.Disposed)
			{
				this.Dispose();
			}
		}
		/// <summary>
		/// Disposes this array.
		/// </summary>
		public void Dispose()
		{
			if (this.Disposed || !this.Managing)
			{
				return;
			}
			if (this.Managing)
			{
				if (this.AllocatedByCryMarshal)
				{
					CryMarshal.FreeMemory(this.Handle);
				}
				else
				{
					NativeMemoryHandlingMethods.FreeMemory(this.Handle);
				}
			}
			this.Disposed = true;
			GC.SuppressFinalize(this);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets pointer to the first byte of array inside CryModule memory.
		/// </summary>
		/// <param name="index">Ignored.</param>
		/// <returns></returns>
		public IntPtr GetHandle(int index = 0)
		{
			return this.Handle;
		}
		/// <summary>
		/// Indicates whether this array has been disposed of.
		/// </summary>
		/// <param name="index">Ignored.</param>
		/// <returns></returns>
		public bool IsClusterDisposed(int index)
		{
			return this.Disposed;
		}
		/// <summary>
		/// Gets allocation method for this array.
		/// </summary>
		/// <param name="clusterIndex">Unused.</param>
		/// <returns>Method of allocation.</returns>
		public Allocators GetAllocationMethod(int clusterIndex = 0)
		{
			return (this.AllocatedByCryMarshal) ? Allocators.CryMarshal : Allocators.CryModule;
		}
		/// <summary>
		/// Gets one byte from the array.
		/// </summary>
		/// <param name="i">Zero-based index of the byte to get.</param>
		/// <returns></returns>
		public byte GetByte(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetByte: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.GetByte(this.Handle, i).UnsignedByte;
		}
		/// <summary>
		/// Gets one byte from the array as signed 8-bit integer.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public sbyte GetSbyte(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetSbyte: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.GetByte(this.Handle, i).SignedByte;
		}
		/// <summary>
		/// Gets one byte from the array as unsigned 16-bit integer.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public ushort GetUInt16(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetUInt16: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get2Bytes(this.Handle, i).UnsignedShort;
		}
		/// <summary>
		/// Gets one byte from the array as signed 16-bit integer.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public short GetInt16(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetInt16: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get2Bytes(this.Handle, i).SignedShort;
		}
		/// <summary>
		/// Gets one byte from the array as half-precision floating point number.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public Half GetHalf(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetHalf: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get2Bytes(this.Handle, i).HalfFloat;
		}
		/// <summary>
		/// Gets one byte from the array as unsigned 32-bit integer.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public uint GetUInt32(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetUInt32: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get4Bytes(this.Handle, i).UnsignedInt;
		}
		/// <summary>
		/// Gets one byte from the array as signed 32-bit integer.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public int GetInt32(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetInt32: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get4Bytes(this.Handle, i).SignedInt;
		}
		/// <summary>
		/// Gets one byte from the array as single-precision floating point number.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public float GetSingle(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetSingle: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get4Bytes(this.Handle, i).SingleFloat;
		}
		/// <summary>
		/// Gets one byte from the array as unsigned 64-bit integer.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public ulong GetUInt64(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetUInt64: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get8Bytes(this.Handle, i).UnsignedLong;
		}
		/// <summary>
		/// Gets one byte from the array as signed 64-bit integer.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public long GetInt64(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetInt64: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get8Bytes(this.Handle, i).SignedLong;
		}
		/// <summary>
		/// Gets one byte from the array as double-precision floating point number.
		/// </summary>
		/// <param name="i">Zero-based index of the value to get.</param>
		/// <returns></returns>
		public double GetDouble(ulong i)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.GetDouble: This array is already disposed.");
			}
			return NativeMemoryHandlingMethods.Get8Bytes(this.Handle, i).DoubleFloat;
		}
		/// <summary>
		/// Transfers all data from native memory to Mono memory.
		/// </summary>
		/// <returns>An array of elements that were contained in native memory.</returns>
		/// <exception cref="ObjectDisposedException">This array is already disposed.</exception>
		public ElementType[] ToArray<ElementType>(ITransferAgent<ElementType> agent)
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.ToArray: This array is already disposed.");
			}
			// Initialize collection of objects that will hold objects transfered from native memory.
			List<ElementType> elements = new List<ElementType>((int)agent.GetObjectsNumber(this.Handle, 0, this.Length));
			// Transfer data.
			agent.Read(new NativeMemoryStream(this, StreamMode.Read), elements);
			// Return in a form of array.
			return elements.ToArray();
		}
		/// <summary>
		/// Transfers all data from native memory to Mono.
		/// </summary>
		/// <returns>Array of bytes, read from native memory.</returns>
		/// <exception cref="ObjectDisposedException">This array is already disposed.</exception>
		public byte[] ToMonoArray()
		{
			if (this.Disposed)
			{
				throw new ObjectDisposedException("NativeArray.ToMonoArray: This array is already disposed.");
			}
			byte[] array = new byte[this.Length];
			NativeMemoryStream stream = new NativeMemoryStream(this, StreamMode.Read);
			stream.Read(array);
			return array;
		}
		/// <summary>
		/// Writes given collection of objects to native memory using a transfer agent.
		/// </summary>
		/// <typeparam name="ElementType">Type of objects to write.</typeparam>
		/// <param name="objects">Objects to write.</param>
		/// <param name="agent">Agent that will help with transfer.</param>
		/// <param name="offset">
		/// Zero-based index of the first byte with native memory cluster from which to write objects.
		/// </param>
		/// <param name="firstObjectIndex">Zero-based index of the first object to write.</param>
		/// <param name="count">Number of objects to write.</param>
		public void Write<ElementType>(IList<ElementType> objects, ITransferAgent<ElementType> agent, ulong offset, ulong firstObjectIndex, ulong count)
		{
			if (count + firstObjectIndex > (ulong)objects.Count)
			{
				throw new ArgumentOutOfRangeException("count", "Not enough objects in the collection.");
			}
			if (offset > this.Length)
			{
				throw new ArgumentOutOfRangeException("offset", "Offset points at location beyond native memory cluster.");
			}
			if (this.Length < agent.GetBytesNumber(count - firstObjectIndex))
			{
				throw new ArgumentException("Native memory cluster cannot fit given collection of objects.");
			}
			NativeMemoryStream stream = new NativeMemoryStream(this, StreamMode.Write)
			{
				Position = offset
			};
			agent.Write(stream, objects.Skip((int)firstObjectIndex).ToList());
		}
		#endregion
		#region Utilities
		#endregion
	}
}