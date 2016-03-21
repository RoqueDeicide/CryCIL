using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents an object that provides access to one of the rendering shaders.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[SuppressMessage("ReSharper", "ExceptionNotThrown")]
	public struct Shader
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the parameters of this shader.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ShaderParameters Parameters => GetParameters(this.handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderParameters GetParameters(IntPtr handle);
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}