using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates information about a subset of CryEngine triangular mesh.
	/// </summary>
	/// <remarks>
	/// A mesh subset is a continuous range vertices and indices that share the same material.
	/// </remarks>
	public struct CryMeshSubset
	{
		#region Fields
		/// <summary>
		/// Center of geometry(?).
		/// </summary>
		public Vector3 Center;
		/// <summary>
		/// Radius of geometry region(?).
		/// </summary>
		public float Radius;
		/// <summary>
		/// Density of texels for lighting(?).
		/// </summary>
		public float TexelDensity;

		/// <summary>
		/// Zero-based index of the first triangle index in range of indices that belong to this subset.
		/// </summary>
		public int FirstIndexId;
		/// <summary>
		/// Total number of triangle indices within this subset.
		/// </summary>
		public int IndexCount;

		/// <summary>
		/// Zero-based index of the first vertex in range of vertices that belong to this subset.
		/// </summary>
		public int FirstVertexId;
		/// <summary>
		/// Total number of vertices within this subset.
		/// </summary>
		public int VertexCount;

		/// <summary>
		/// Identifier of the material that is used to render this mesh subset.
		/// </summary>
		public int MaterialId;
		/// <summary>
		/// A set of flags that specify how to render the material.
		/// </summary>
		public MaterialFlags MaterialFlags;
		/// <summary>
		/// Specifies how to physicalize this subset.
		/// </summary>
		public PhysicalGeometryType PhysicalizationType;
		#endregion
	}
}