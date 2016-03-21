using System;
using System.Linq;
using CryCil.Engine.Physics.Primitives;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that are used when creating objects of type <see cref="GeometryShape"/>.
	/// </summary>
	[Flags]
	public enum PhysicsMeshFlags
	{
		// shared_... flags mean that the mesh will not attempt to free the corresponding array upon
		// deletion

		// shared_ flags don't seem to be useful since the geometry objects are created with C# arrays that
		// are copied every time and are not tracked otherwise.

		//shared_vertexes = 1,
		//shared_indexes = 2,
		//shared_materials = 4,
		//shared_foreign_indexes = 8,
		//shared_normals = 0x10,

		/// <summary>
		/// When set, specifies that the geometry should use an oriented bounding box as a bounding volume.
		/// </summary>
		/// <remarks>
		/// When a combination of multiple flags from the following list is set, then a best fitting
		/// bounding volume will be used: <see cref="OrientedBoundingBox"/>,
		/// <see cref="AxisAlignedBoundingBox"/>, <see cref="SingleBoundingBoxTree"/>,
		/// <see cref="AxisAlignedBoundingBoxRotated"/>, <see cref="VoxelGrid"/>.
		/// </remarks>
		OrientedBoundingBox = 0x20,
		/// <summary>
		/// When set, specifies that the geometry should use an axis-aligned bounding box as a bounding
		/// volume.
		/// </summary>
		/// <remarks>
		/// When a combination of multiple flags from the following list is set, then a best fitting
		/// bounding volume will be used: <see cref="OrientedBoundingBox"/>,
		/// <see cref="AxisAlignedBoundingBox"/>, <see cref="SingleBoundingBoxTree"/>,
		/// <see cref="AxisAlignedBoundingBoxRotated"/>, <see cref="VoxelGrid"/>.
		/// </remarks>
		AxisAlignedBoundingBox = 0x40,
		/// <summary>
		/// When set, specifies that the geometry should use a single box tree (whatever that means) as a
		/// bounding volume.
		/// </summary>
		/// <remarks>
		/// When a combination of multiple flags from the following list is set, then a best fitting
		/// bounding volume will be used: <see cref="OrientedBoundingBox"/>,
		/// <see cref="AxisAlignedBoundingBox"/>, <see cref="SingleBoundingBoxTree"/>,
		/// <see cref="AxisAlignedBoundingBoxRotated"/>, <see cref="VoxelGrid"/>.
		/// </remarks>
		SingleBoundingBoxTree = 0x80,
		/// <summary>
		/// When set, specifies that the geometry should use a rotated axis-aligned bounding box as a
		/// bounding volume.
		/// </summary>
		/// <remarks>
		/// When a combination of multiple flags from the following list is set, then a best fitting
		/// bounding volume will be used: <see cref="OrientedBoundingBox"/>,
		/// <see cref="AxisAlignedBoundingBox"/>, <see cref="SingleBoundingBoxTree"/>,
		/// <see cref="AxisAlignedBoundingBoxRotated"/>, <see cref="VoxelGrid"/>.
		/// </remarks>
		AxisAlignedBoundingBoxRotated = 0x40000,
		/// <summary>
		/// When set, specifies that the geometry should use a voxel grid as a bounding volume.
		/// </summary>
		/// <remarks>
		/// When a combination of multiple flags from the following list is set, then a best fitting
		/// bounding volume will be used: <see cref="OrientedBoundingBox"/>,
		/// <see cref="AxisAlignedBoundingBox"/>, <see cref="SingleBoundingBoxTree"/>,
		/// <see cref="AxisAlignedBoundingBoxRotated"/>, <see cref="VoxelGrid"/>.
		/// </remarks>
		VoxelGrid = 0x80000,
		/// <summary>
		/// When set, specifies that geometry physics simulation algorithms should use optimizations that
		/// assume that the physical body will never have more then 1 contact.
		/// </summary>
		MultiContact0 = 0x100,
		/// <summary>
		/// When set, specifies that geometry physics simulation algorithms should use optimizations that
		/// assume that the physical body will only occasionally have more then 1 contact.
		/// </summary>
		MultiContact1 = 0x200,
		/// <summary>
		/// When set, specifies that geometry physics simulation algorithms should not use any optimizations
		/// that are based on contact count assumptions.
		/// </summary>
		MultiContact2 = 0x400,
		/// <summary>
		/// When set, specifies that geometry creation algorithm needs to try to check whether provided
		/// geometry is similar to the cylinder.
		/// </summary>
		/// <remarks>
		/// If the check is passed, then <see cref="GeometryShape"/> will use the
		/// <see cref="Primitive.Cylinder"/> as an internal representation.
		/// </remarks>
		ApproximateCylinder = 0x800,
		/// <summary>
		/// When set, specifies that geometry creation algorithm needs to try to check whether provided
		/// geometry is similar to the box.
		/// </summary>
		/// <remarks>
		/// If the check is passed, then <see cref="GeometryShape"/> will use the
		/// <see cref="Primitive.Box"/> as an internal representation.
		/// </remarks>
		ApproximateBox = 0x1000,
		/// <summary>
		/// When set, specifies that geometry creation algorithm needs to try to check whether provided
		/// geometry is similar to the sphere.
		/// </summary>
		/// <remarks>
		/// If the check is passed, then <see cref="GeometryShape"/> will use the
		/// <see cref="Primitive.Sphere"/> as an internal representation.
		/// </remarks>
		ApproximateSphere = 0x2000,
		/// <summary>
		/// When set, specifies that geometry creation algorithm needs to try to check whether provided
		/// geometry is similar to the capsule.
		/// </summary>
		/// <remarks>
		/// If the check is passed, then <see cref="GeometryShape"/> will use the
		/// <see cref="Primitive.Capsule"/> as an internal representation.
		/// </remarks>
		ApproximateCapsule = 0x200000,
		/// <summary>
		/// When set, specifies to keep the vertex map (?) after merging adjacent vertices.
		/// </summary>
		KeepVertexMap = 0x8000,
		/// <summary>
		/// When set, specifies to keep the vertex map (?) after merging adjacent vertices, but delete it
		/// after loading the geometry.
		/// </summary>
		KeepVertexMapForSaving = 0x10000,
		/// <summary>
		/// When set, specifies to not attempt to merge the adjacent vertices.
		/// </summary>
		NoVertexMerge = 0x20000,
		/// <summary>
		/// When set, specifies that this geometric object is only used for static objects.
		/// </summary>
		/// <remarks>Setting this flag speeds up the calculation of physics properties.</remarks>
		AlwaysStatic = 0x100000,
		/// <summary>
		/// When set, specifies that all data will be saved unconditionally.
		/// </summary>
		FullSerialization = 0x400000,
		/// <summary>
		/// When set, specifies that all mesh allocations must go to a flushable pool.
		/// </summary>
		Transient = 0x800000,
		/// <summary>
		/// When set, specifies that boolean operations are not allowed for this mesh object.
		/// </summary>
		NoBooleans = 0x1000000,
		/// <summary>
		/// When set, specifies that this geometry object is going to represent a plane.
		/// </summary>
		/// <remarks>
		/// Setting this flag speeds up the calculation of AABB, because all triangles are assumed to be on
		/// the same plane.
		/// </remarks>
		AxisAlignedBoundingBoxPlaneOptimise = 0x4000,
		/// <summary>
		/// When set, specifies to not attempt to filter degenerate triangles.
		/// </summary>
		NoFilter = 0x2000000
	}
}