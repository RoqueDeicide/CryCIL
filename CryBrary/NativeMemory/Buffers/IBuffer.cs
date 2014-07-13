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
	/// Defines common properties of buffer structures.
	/// </summary>
	public interface IBuffer
	{
		/// <summary>
		/// Gets length of the buffer in bytes.
		/// </summary>
		ulong Length { get; }
		/// <summary>
		/// When implemented in derived class or structure, gets or
		/// sets one byte in this buffer.
		/// </summary>
		/// <param name="index">
		/// Zero-based index of the byte to get or set.
		/// </param>
		/// <returns></returns>
		byte this[ulong index] { get; set; }
		/// <summary>
		/// When implemented in derived type, fills this buffer with
		/// data from native memory.
		/// </summary>
		/// <param name="handle">
		/// Address of the memory cluster from which to get the data.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of the first byte to get from native
		/// memory within the cluster.
		/// </param>
		void Get(IntPtr handle, ulong offset);
		/// <summary>
		/// When implemented in derived type, transfers data from this
		/// buffer to native memory.
		/// </summary>
		/// <param name="handle">
		/// Address of the memory cluster to write the data into.
		/// </param>
		/// <param name="offset">
		/// Zero-based index of the first byte within the cluster to write.
		/// </param>
		void Set(IntPtr handle, ulong offset);
	}
}