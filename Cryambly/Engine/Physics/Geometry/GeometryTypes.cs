using CryCil.Engine.Physics.Primitives;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of physical geometry.
	/// </summary>
	public enum GeometryTypes
	{
		/// <summary>
		/// Specifies that this geometric object is a mesh composed of triangles. This value corresponds to
		/// <see cref="Primitive.Triangle.Id"/>.
		/// </summary>
		TriangleMesh = Primitive.Triangle.Id,
		/// <summary>
		/// Specifies that this geometric object is a physical representation of a height map. This value
		/// corresponds to <see cref="Primitive.HeightField.Id"/>.
		/// </summary>
		HeightField = Primitive.HeightField.Id,
		/// <summary>
		/// Specifies that this geometric object is a cylinder. This value corresponds to
		/// <see cref="Primitive.Cylinder.Id"/>.
		/// </summary>
		Cylinder = Primitive.Cylinder.Id,
		/// <summary>
		/// Specifies that this geometric object is a capsule. This value corresponds to
		/// <see cref="Primitive.Capsule.Id"/>.
		/// </summary>
		Capsule = Primitive.Capsule.Id,
		/// <summary>
		/// Specifies that this geometric object is a ray. This value corresponds to
		/// <see cref="Primitive.Ray.Id"/>.
		/// </summary>
		Ray = Primitive.Ray.Id,
		/// <summary>
		/// Specifies that this geometric object is a sphere. This value corresponds to
		/// <see cref="Primitive.Sphere.Id"/>.
		/// </summary>
		Sphere = Primitive.Sphere.Id,
		/// <summary>
		/// Specifies that this geometric object is a box. This value corresponds to
		/// <see cref="Primitive.Box.Id"/>.
		/// </summary>
		Box = Primitive.Box.Id,
		/// <summary>
		/// Specifies that this geometric object is a voxel grid. This value corresponds to
		/// <see cref="Primitive.VoxelGrid.Id"/>.
		/// </summary>
		VoxelGrid = Primitive.VoxelGrid.Id
	}
}