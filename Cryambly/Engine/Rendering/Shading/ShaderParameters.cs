using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Encapsulates a pointer to <see cref="ShaderParameter"/> object, because pointers cannot be type
	/// arguments apparently.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ShaderParameterPtr
	{
		/// <summary>
		/// The pointer itself.
		/// </summary>
		public ShaderParameter* Handle;
	}
	/// <summary>
	/// Represents a wrapper for a collection of shader parameters.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ShaderParameters : IEnumerable<ShaderParameterPtr>
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the pointer to the shader parameter.
		/// </summary>
		/// <param name="index">Zero-based index of the parameter to get.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ShaderParameter* this[int index]
		{
			get { return GetItemInt(this.handle, index).Handle; }
		}
		/// <summary>
		/// Gets the pointer to the shader parameter.
		/// </summary>
		/// <param name="name">Name of the parameter to get.</param>
		/// <returns>Null if not found.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentNullException">
		/// The name of the parameter to get cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the parameter cannot contain more then 32 ASCII symbols.
		/// </exception>
		public ShaderParameter* this[string name]
		{
			get { return GetItemString(this.handle, name); }
		}
		/// <summary>
		/// Gets the number of shader parameters accessible through this position.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public int Count
		{
			get { return GetCount(this.handle); }
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		internal ShaderParameters(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>Object that does the enumeration.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public IEnumerator<ShaderParameterPtr> GetEnumerator()
		{
			int count = GetCount(this.handle);

			for (int i = 0; i < count; i++)
			{
				yield return GetItemInt(this.handle, i);
			}
		}

		#endregion
		#region Utilities
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderParameterPtr GetItemInt(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderParameter* GetItemString(IntPtr handle, string name);
		#endregion
	}
}