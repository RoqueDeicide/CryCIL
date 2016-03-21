using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine
{
	/// <summary>
	/// Enumeration of console variable types.
	/// </summary>
	public enum ConsoleVariableType
	{
		/// <summary>
		/// 32-bit integer number.
		/// </summary>
		Int32 = 1,
		/// <summary>
		/// Single precision floating point number.
		/// </summary>
		Single,
		/// <summary>
		/// Text.
		/// </summary>
		String
	}
	/// <summary>
	/// Represents a wrapper for a CryEngine console variable.
	/// </summary>
	public struct ConsoleVariable
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether this console variable object is usable.
		/// </summary>
		public bool Valid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets or sets integer value of this console variable.
		/// </summary>
		public int ValueInt32
		{
			get { return this.GetInt(); }
			set { this.SetInt(value); }
		}
		/// <summary>
		/// Gets or sets single-precision floating-point value of this console variable.
		/// </summary>
		public float ValueSingle
		{
			get { return this.GetFloat(); }
			set { this.SetFloat(value); }
		}
		/// <summary>
		/// Gets or sets text value of this console variable.
		/// </summary>
		public string ValueString
		{
			get { return this.GetString(); }
			set { this.SetString(value); }
		}
		/// <summary>
		/// Gets primary type of this console variable.
		/// </summary>
		public ConsoleVariableType Type => this.GetVariableType();
		/// <summary>
		/// Gets the name of this console variable.
		/// </summary>
		public string Name => this.GetNameVar();
		/// <summary>
		/// Gets helpful description of this console variable if it was provided on registration.
		/// </summary>
		/// <returns>Null, if no help is available.</returns>
		public string Help => this.GetHelp();
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="handle">Pointer to internal representation of the console variable.</param>
		public ConsoleVariable(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Unregisters and deletes this console variable.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Release();
		/// <summary>
		/// Clears the specified bits in the flag field.
		/// </summary>
		/// <param name="flags">Flags to clear.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearFlags(ConsoleFlags flags);
		/// <summary>
		/// Gets flags assigned to this console variable.
		/// </summary>
		/// <returns>Flags assigned to this console variable.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ConsoleFlags GetFlags();
		/// <summary>
		/// Sets given flags for this console variable.
		/// </summary>
		/// <remarks>It's unknown whether setting a simple assignment or a bitwise OR operation.</remarks>
		/// <param name="flags">Flags to set.</param>
		/// <returns>New set of flags.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ConsoleFlags SetFlags(ConsoleFlags flags);
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetInt();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float GetFloat();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetString();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetString(string s);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetFloat(float f);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetInt(int i);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ConsoleVariableType GetVariableType();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetNameVar();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetHelp();
		#endregion
	}
}