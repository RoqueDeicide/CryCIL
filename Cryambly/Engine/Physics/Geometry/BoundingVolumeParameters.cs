using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that specifies how to build a bounding volume for a physical mesh.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct BoundingVolumeParameters
	{
		/// <summary>
		/// Minimal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has less then this number of triangles after split, a different one will be attempted.
		/// </summary>
		public int MinTrianglesPerNode;
		/// <summary>
		/// Maximal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has more then this number of triangles, it gets split.
		/// </summary>
		public int MaxTrianglesPerNode;
		/// <summary>
		/// When several BV trees are requested, it selects the one with the smallest volume; This field
		/// scales AABB's volume down.
		/// </summary>
		public float AabbVolumeDivisor;
	}
	/// <summary>
	/// Encapsulates a set of parameters that specifies the how to generate the voxel grid-based bounding
	/// volume tree.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VoxelGridParameters
	{
		/// <summary>
		/// Minimal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has less then this number of triangles after split, a different one will be attempted.
		/// </summary>
		public int MinTrianglesPerNode;
		/// <summary>
		/// Maximal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has more then this number of triangles, it gets split.
		/// </summary>
		public int MaxTrianglesPerNode;
		/// <summary>
		/// When several BV trees are requested, it selects the one with the smallest volume; This field
		/// scales AABB's volume down.
		/// </summary>
		public float AabbVolumeDivisor;
		/// <summary>
		/// Coordinates of the point to use as an origin of coordinates for the voxel grid.
		/// </summary>
		public Vector3 Origin;
		/// <summary>
		/// Dimensions of each cell of the voxel grid.
		/// </summary>
		public Vector3 Step;
		/// <summary>
		/// Number of cells along each axis.
		/// </summary>
		public Vector3Int32 Size;
	}
}