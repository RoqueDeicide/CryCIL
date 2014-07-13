using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Represents a wrapper around a null terminated string stored in
	/// native memory.
	/// </summary>
	public class NTString : INativeMemoryWrapper
	{
		#region Fields
		private IntPtr handle;
		private bool disposed;
		private Allocators allocator;
		private bool managing;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this string is disposed.
		/// </summary>
		public bool Disposed
		{
			get { return this.disposed; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a native null terminated string.
		/// </summary>
		/// <param name="text">Managed string.</param>
		public NTString(string text)
		{
			this.handle = NativeString.GetNativeString(text);
			this.managing = true;
			this.disposed = false;
			this.allocator = Allocators.Mono;
		}
		/// <summary>
		/// Creates a native null terminated string.
		/// </summary>
		/// <param name="pointer">Pointer to first byte.</param>
		/// <param name="allocator">
		/// Method that wasw used to allocate memory.
		/// </param>
		/// <param name="managing">
		/// Indicates whether this object should release native memory
		/// when disposed.
		/// </param>
		public NTString(IntPtr pointer, Allocators allocator, bool managing = false)
		{
			this.handle = pointer;
			this.managing = managing;
			this.allocator = allocator;
			this.disposed = false;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases resources used by this string.
		/// </summary>
		public void Dispose()
		{
			if (this.disposed || !this.managing)
			{
				return;
			}
			switch (this.allocator)
			{
				case Allocators.CryMarshal:
					CryMarshal.FreeMemory(this.handle);
					break;
				case Allocators.CryModule:
					CryModule.FreeMemory(this.handle);
					break;
				case Allocators.Mono:
					MonoMemory.FreeMemory(this.handle);
					break;
				default:
					break;
			}
			this.disposed = true;
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Gets pointer to the string in native memory.
		/// </summary>
		/// <param name="index">Unused.</param>
		/// <returns>Pointer to the string in native memory.</returns>
		public IntPtr GetHandle(int index = 0)
		{
			return this.handle;
		}
		/// <summary>
		/// Indicates whether this text is disposed.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool IsClusterDisposed(int index)
		{
			return this.disposed;
		}
		/// <summary>
		/// Gets allocation method.
		/// </summary>
		/// <param name="clusterIndex">Unused.</param>
		/// <returns></returns>
		public Allocators GetAllocationMethod(int clusterIndex = 0)
		{
			return this.allocator;
		}
		/// <summary>
		/// Converts this null terminated string to standard Mono string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return NativeString.GetMonoString(this.handle);
		}
		#endregion
		#region Utilities
		~NTString()
		{
			this.Dispose();
		}
		#endregion
	}
}