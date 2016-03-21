using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates information about a triangle face in <see cref="CryMesh"/> topology.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CryMeshFace
	{
		/// <summary>
		/// Zero-based index that can be used to access information about first vertex that forms this face.
		/// </summary>
		public int First;
		/// <summary>
		/// Zero-based index that can be used to access information about second vertex that forms this
		/// face.
		/// </summary>
		public int Second;
		/// <summary>
		/// Zero-based index that can be used to access information about third vertex that forms this face.
		/// </summary>
		public int Third;
		/// <summary>
		/// Zero-based index of the mesh subset this face belongs to.
		/// </summary>
		public byte Subset;
	}
}