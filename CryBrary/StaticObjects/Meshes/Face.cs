using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Encapsulates data that describe a mesh face.
	/// </summary>
	public struct Face
	{
		/// <summary>
		/// Array of indices of vertices that form this face.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 3)]
		public int[] Indices;
		/// <summary>
		/// Index of mesh subset this face is assigned to.
		/// </summary>
		public byte SubsetIndex;
	}
}