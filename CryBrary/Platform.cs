using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Defines properties that indicate how CryEngine platform was compiled.
	/// </summary>
	public static class Platform
	{
		static Platform()
		{
			MeshIndexIs16Bit = Native.PlatformMethods.AreMeshIndicesInt16();
			MeshTangentsUseSingle = Native.PlatformMethods.AreMeshTangentsSingle();
		}
		#region Mesh Configuration
		/// <summary>
		/// Indicates whether mesh indices are configured to use <see cref="UInt16" /> instead of
		/// <see cref="UInt32" />.
		/// </summary>
		public static readonly bool MeshIndexIs16Bit;
		/// <summary>
		/// Indicates whether tangents are configured to be represented by single precision floating
		/// point numbers instead of 16-bit unsigned integers.
		/// </summary>
		public static readonly bool MeshTangentsUseSingle;
		#endregion
	}
}