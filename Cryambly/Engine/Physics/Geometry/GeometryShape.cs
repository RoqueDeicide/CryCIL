using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents an object that defines the shape of geometric object that is used by the physics
	/// subsystem.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct GeometryShape
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		#endregion
		#region Construction
		internal GeometryShape(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new geometry object.
		/// </summary>
		/// <param name="vertices">              An array of vertices the mesh consists of.</param>
		/// <param name="indices">               
		/// An array of indices of vertices that form the triangles that form the mesh.
		/// </param>
		/// <param name="flags">                 A set of flags that specify the geometry.</param>
		/// <param name="approximationTolerance">
		/// A tolerance value to use when checking whether provided geometry resembles a primitive
		/// geometric object.
		/// </param>
		/// <param name="minTrianglesPerNode">   
		/// Minimal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has less then this number of triangles after split, a different one will be
		/// attempted.
		/// </param>
		/// <param name="maxTrianglesPerNode">   
		/// Maximal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has more then this number of triangles, it gets split.
		/// </param>
		/// <param name="favorAabb">             
		/// When several BV trees are requested, it selects the one with the smallest volume; This field
		/// scales AABB's volume down.
		/// </param>
		/// <exception cref="ArgumentException">
		/// An array of vertices needs to have at least 3 vertices.
		/// </exception>
		/// <exception cref="ArgumentNullException">An array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Length of the array of indexes must be divisible by 3.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Minimal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// An array of vertices cannot be longer then 65535.
		/// </exception>
		public GeometryShape(Vector3[] vertices, ushort[] indices, PhysicsMeshFlags flags,
							 float approximationTolerance = 0.05f, int minTrianglesPerNode = 2, int maxTrianglesPerNode = 4,
							 float favorAabb = 1)
		{
			this.handle = IntPtr.Zero;

			this.AssertInstance();
			if (vertices.IsNullOrTooSmall(3))
			{
				throw new ArgumentException("An array of vertices needs to have at least 3 vertices.", "vertices");
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices", "An array of indexes cannot be null.");
			}
			if (indices.Length % 3 != 0)
			{
				throw new ArgumentException("Length of the array of indexes must be divisible by 3.", "indices");
			}
			if (minTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("minTrianglesPerNode",
													  "Minimal number of triangles per node must be greater then 0.");
			}
			if (maxTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("maxTrianglesPerNode",
													  "Maximal number of triangles per node must be greater then 0.");
			}
			if (vertices.Length > ushort.MaxValue)
			{
				throw new ArgumentOutOfRangeException("vertices", "An array of vertices cannot be longer then 65535.");
			}
			Contract.EndContractBlock();

			fixed (Vector3* verticesPtr = vertices)
			fixed (ushort* indexesPtr = indices)
			{
				this.handle = CreateMesh(verticesPtr, indexesPtr, null, null, indices.Length / 3, flags,
										 approximationTolerance, minTrianglesPerNode, maxTrianglesPerNode, favorAabb);
			}
		}
		/// <summary>
		/// Creates a new geometry object.
		/// </summary>
		/// <param name="vertices">              An array of vertices the mesh consists of.</param>
		/// <param name="indices">               
		/// An array of indices of vertices that form the triangles that form the mesh.
		/// </param>
		/// <param name="flags">                 A set of flags that specify the geometry.</param>
		/// <param name="bvParams">              
		/// An object that provides the parameters that specify how to build the bounding volume tree.
		/// </param>
		/// <param name="approximationTolerance">
		/// A tolerance value to use when checking whether provided geometry resembles a primitive
		/// geometric object.
		/// </param>
		/// <exception cref="ArgumentException">
		/// An array of vertices needs to have at least 3 vertices.
		/// </exception>
		/// <exception cref="ArgumentNullException">An array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Length of the array of indexes must be divisible by 3.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Minimal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// An array of vertices cannot be longer then 65535.
		/// </exception>
		public GeometryShape(Vector3[] vertices, ushort[] indices, PhysicsMeshFlags flags,
							 BoundingVolumeParameters bvParams, float approximationTolerance = 0.05f)
		{
			this.handle = IntPtr.Zero;

			this.AssertInstance();
			if (vertices.IsNullOrTooSmall(3))
			{
				throw new ArgumentException("An array of vertices needs to have at least 3 vertices.", "vertices");
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices", "An array of indexes cannot be null.");
			}
			if (indices.Length % 3 != 0)
			{
				throw new ArgumentException("Length of the array of indexes must be divisible by 3.", "indices");
			}
			if (bvParams.MinTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("bvParams", "Minimal number of triangles per node must be greater then 0.");
			}
			if (bvParams.MaxTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("bvParams", "Maximal number of triangles per node must be greater then 0.");
			}
			if (vertices.Length > ushort.MaxValue)
			{
				throw new ArgumentOutOfRangeException("vertices", "An array of vertices cannot be longer then 65535.");
			}
			Contract.EndContractBlock();

			fixed (Vector3* verticesPtr = vertices)
			fixed (ushort* indexesPtr = indices)
			{
				this.handle = CreateMeshBv(verticesPtr, indexesPtr, null, null, indices.Length / 3, flags,
										 ref bvParams, approximationTolerance);
			}
		}
		/// <summary>
		/// Creates a new geometry object.
		/// </summary>
		/// <param name="vertices">              An array of vertices the mesh consists of.</param>
		/// <param name="indices">               
		/// An array of indices of vertices that form the triangles that form the mesh.
		/// </param>
		/// <param name="flags">                 
		/// A set of flags that specify the geometry. <see cref="PhysicsMeshFlags.VoxelGrid"/> flag doesn't
		/// need to be set.
		/// </param>
		/// <param name="vgParams">              
		/// An object that provides the parameters that specify how to build the bounding volume tree.
		/// </param>
		/// <param name="approximationTolerance">
		/// A tolerance value to use when checking whether provided geometry resembles a primitive
		/// geometric object.
		/// </param>
		/// <exception cref="ArgumentException">
		/// An array of vertices needs to have at least 3 vertices.
		/// </exception>
		/// <exception cref="ArgumentNullException">An array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Length of the array of indexes must be divisible by 3.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Minimal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// An array of vertices cannot be longer then 65535.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Dimensions of the voxel grid cannot be less then 0.
		/// </exception>
		public GeometryShape(Vector3[] vertices, ushort[] indices, PhysicsMeshFlags flags,
							 VoxelGridParameters vgParams, float approximationTolerance = 0.05f)
		{
			this.handle = IntPtr.Zero;

			this.AssertInstance();
			if (vertices.IsNullOrTooSmall(3))
			{
				throw new ArgumentException("An array of vertices needs to have at least 3 vertices.", "vertices");
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices", "An array of indexes cannot be null.");
			}
			if (indices.Length % 3 != 0)
			{
				throw new ArgumentException("Length of the array of indexes must be divisible by 3.", "indices");
			}
			if (vgParams.MinTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("vgParams", "Minimal number of triangles per node must be greater then 0.");
			}
			if (vgParams.MaxTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("vgParams", "Maximal number of triangles per node must be greater then 0.");
			}
			if (vertices.Length > ushort.MaxValue)
			{
				throw new ArgumentOutOfRangeException("vertices", "An array of vertices cannot be longer then 65535.");
			}
			if (vgParams.Size.X <= 0 || vgParams.Size.Y <= 0 || vgParams.Size.Z <= 0)
			{
				throw new ArgumentException("Dimensions of the voxel grid cannot be less then 0.", "vgParams");
			}
			Contract.EndContractBlock();

			fixed (Vector3* verticesPtr = vertices)
			fixed (ushort* indexesPtr = indices)
			{
				this.handle = CreateMeshVg(verticesPtr, indexesPtr, null, null, indices.Length / 3,
										 flags | PhysicsMeshFlags.VoxelGrid, ref vgParams, approximationTolerance);
			}
		}
		/// <summary>
		/// Creates a new geometry object.
		/// </summary>
		/// <param name="vertices">              An array of vertices the mesh consists of.</param>
		/// <param name="indices">               
		/// An array of indices of vertices that form the triangles that form the mesh.
		/// </param>
		/// <param name="materialIds">           An array of indexes of materials per face.</param>
		/// <param name="flags">                 A set of flags that specify the geometry.</param>
		/// <param name="approximationTolerance">
		/// A tolerance value to use when checking whether provided geometry resembles a primitive
		/// geometric object.
		/// </param>
		/// <param name="minTrianglesPerNode">   
		/// Minimal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has less then this number of triangles after split, a different one will be
		/// attempted.
		/// </param>
		/// <param name="maxTrianglesPerNode">   
		/// Maximal number of triangle that can be present in a single bounding volume tree node. If one of
		/// the nodes has more then this number of triangles, it gets split.
		/// </param>
		/// <param name="favorAabb">             
		/// When several BV trees are requested, it selects the one with the smallest volume; This field
		/// scales AABB's volume down.
		/// </param>
		/// <exception cref="ArgumentException">
		/// An array of vertices needs to have at least 3 vertices.
		/// </exception>
		/// <exception cref="ArgumentNullException">An array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Length of the array of indexes must be divisible by 3.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Minimal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// An array of vertices cannot be longer then 65535.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The number of ids for materials must be equal to number of triangles.
		/// </exception>
		public GeometryShape(Vector3[] vertices, ushort[] indices, byte[] materialIds, PhysicsMeshFlags flags,
							 float approximationTolerance = 0.05f, int minTrianglesPerNode = 2, int maxTrianglesPerNode = 4,
							 float favorAabb = 1)
		{
			this.handle = IntPtr.Zero;

			this.AssertInstance();
			if (vertices.IsNullOrTooSmall(3))
			{
				throw new ArgumentException("An array of vertices needs to have at least 3 vertices.", "vertices");
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices", "An array of indexes cannot be null.");
			}
			if (indices.Length % 3 != 0)
			{
				throw new ArgumentException("Length of the array of indexes must be divisible by 3.", "indices");
			}
			if (minTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("minTrianglesPerNode",
													  "Minimal number of triangles per node must be greater then 0.");
			}
			if (maxTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("maxTrianglesPerNode",
													  "Maximal number of triangles per node must be greater then 0.");
			}
			if (vertices.Length > ushort.MaxValue)
			{
				throw new ArgumentOutOfRangeException("vertices", "An array of vertices cannot be longer then 65535.");
			}
			int triangleCount = indices.Length / 3;
			if (triangleCount != materialIds.Length)
			{
				throw new ArgumentException("The number of ids for materials must be equal to number of triangles.");
			}
			Contract.EndContractBlock();

			fixed (Vector3* verticesPtr = vertices)
			fixed (ushort* indexesPtr = indices)
			fixed (byte* matPtr = materialIds)
			{
				this.handle = CreateMesh(verticesPtr, indexesPtr, matPtr, null, indices.Length / 3, flags,
										 approximationTolerance, minTrianglesPerNode, maxTrianglesPerNode, favorAabb);
			}
		}
		/// <summary>
		/// Creates a new geometry object.
		/// </summary>
		/// <param name="vertices">              An array of vertices the mesh consists of.</param>
		/// <param name="indices">               
		/// An array of indices of vertices that form the triangles that form the mesh.
		/// </param>
		/// <param name="materialIds">           An array of indexes of materials per face.</param>
		/// <param name="flags">                 A set of flags that specify the geometry.</param>
		/// <param name="bvParams">              
		/// An object that provides the parameters that specify how to build the bounding volume tree.
		/// </param>
		/// <param name="approximationTolerance">
		/// A tolerance value to use when checking whether provided geometry resembles a primitive
		/// geometric object.
		/// </param>
		/// <exception cref="ArgumentException">
		/// An array of vertices needs to have at least 3 vertices.
		/// </exception>
		/// <exception cref="ArgumentNullException">An array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Length of the array of indexes must be divisible by 3.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Minimal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// An array of vertices cannot be longer then 65535.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The number of ids for materials must be equal to number of triangles.
		/// </exception>
		public GeometryShape(Vector3[] vertices, ushort[] indices, byte[] materialIds, PhysicsMeshFlags flags,
							 BoundingVolumeParameters bvParams, float approximationTolerance = 0.05f)
		{
			this.handle = IntPtr.Zero;

			this.AssertInstance();
			if (vertices.IsNullOrTooSmall(3))
			{
				throw new ArgumentException("An array of vertices needs to have at least 3 vertices.", "vertices");
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices", "An array of indexes cannot be null.");
			}
			if (indices.Length % 3 != 0)
			{
				throw new ArgumentException("Length of the array of indexes must be divisible by 3.", "indices");
			}
			if (bvParams.MinTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("bvParams", "Minimal number of triangles per node must be greater then 0.");
			}
			if (bvParams.MaxTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("bvParams", "Maximal number of triangles per node must be greater then 0.");
			}
			if (vertices.Length > ushort.MaxValue)
			{
				throw new ArgumentOutOfRangeException("vertices", "An array of vertices cannot be longer then 65535.");
			}
			int triangleCount = indices.Length / 3;
			if (triangleCount != materialIds.Length)
			{
				throw new ArgumentException("The number of ids for materials must be equal to number of triangles.");
			}
			Contract.EndContractBlock();

			fixed (Vector3* verticesPtr = vertices)
			fixed (ushort* indexesPtr = indices)
			fixed (byte* matPtr = materialIds)
			{
				this.handle = CreateMeshBv(verticesPtr, indexesPtr, matPtr, null, indices.Length / 3, flags,
										 ref bvParams, approximationTolerance);
			}
		}
		/// <summary>
		/// Creates a new geometry object.
		/// </summary>
		/// <param name="vertices">              An array of vertices the mesh consists of.</param>
		/// <param name="indices">               
		/// An array of indices of vertices that form the triangles that form the mesh.
		/// </param>
		/// <param name="materialIds">           An array of indexes of materials per face.</param>
		/// <param name="flags">                 
		/// A set of flags that specify the geometry. <see cref="PhysicsMeshFlags.VoxelGrid"/> flag doesn't
		/// need to be set.
		/// </param>
		/// <param name="vgParams">              
		/// An object that provides the parameters that specify how to build the bounding volume tree.
		/// </param>
		/// <param name="approximationTolerance">
		/// A tolerance value to use when checking whether provided geometry resembles a primitive
		/// geometric object.
		/// </param>
		/// <exception cref="ArgumentException">
		/// An array of vertices needs to have at least 3 vertices.
		/// </exception>
		/// <exception cref="ArgumentNullException">An array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Length of the array of indexes must be divisible by 3.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Minimal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of triangles per node must be greater then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// An array of vertices cannot be longer then 65535.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The number of ids for materials must be equal to number of triangles.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Dimensions of the voxel grid cannot be less then 0.
		/// </exception>
		public GeometryShape(Vector3[] vertices, ushort[] indices, byte[] materialIds, PhysicsMeshFlags flags,
							 VoxelGridParameters vgParams, float approximationTolerance = 0.05f)
		{
			this.handle = IntPtr.Zero;

			this.AssertInstance();
			if (vertices.IsNullOrTooSmall(3))
			{
				throw new ArgumentException("An array of vertices needs to have at least 3 vertices.", "vertices");
			}
			if (indices == null)
			{
				throw new ArgumentNullException("indices", "An array of indexes cannot be null.");
			}
			if (indices.Length % 3 != 0)
			{
				throw new ArgumentException("Length of the array of indexes must be divisible by 3.", "indices");
			}
			if (vgParams.MinTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("vgParams", "Minimal number of triangles per node must be greater then 0.");
			}
			if (vgParams.MaxTrianglesPerNode <= 0)
			{
				throw new ArgumentOutOfRangeException("vgParams", "Maximal number of triangles per node must be greater then 0.");
			}
			if (vertices.Length > ushort.MaxValue)
			{
				throw new ArgumentOutOfRangeException("vertices", "An array of vertices cannot be longer then 65535.");
			}
			int triangleCount = indices.Length / 3;
			if (triangleCount != materialIds.Length)
			{
				throw new ArgumentException("The number of ids for materials must be equal to number of triangles.");
			}
			if (vgParams.Size.X <= 0 || vgParams.Size.Y <= 0 || vgParams.Size.Z <= 0)
			{
				throw new ArgumentException("Dimensions of the voxel grid cannot be less then 0.", "vgParams");
			}
			Contract.EndContractBlock();

			fixed (Vector3* verticesPtr = vertices)
			fixed (ushort* indexesPtr = indices)
			fixed (byte* matPtr = materialIds)
			{
				this.handle = CreateMeshVg(verticesPtr, indexesPtr, matPtr, null, indices.Length / 3,
										 flags | PhysicsMeshFlags.VoxelGrid, ref vgParams, approximationTolerance);
			}
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateMesh(Vector3* vertices, ushort* indices, byte* materialIds, int* foreignIds,
												int triangleCount, PhysicsMeshFlags flags,
												float approximationTolerance = 0.05f, int minTrianglesPerNode = 2,
												int maxTrianglesPerNode = 4, float favorAabb = 1);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateMeshBv(Vector3* vertices, ushort* indices, byte* materialIds, int* foreignIds,
												int triangleCount, PhysicsMeshFlags flags,
												ref BoundingVolumeParameters bvParams, float approximationTolerance = 0.05f);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateMeshVg(Vector3* vertices, ushort* indices, byte* materialIds, int* foreignIds,
												int triangleCount, PhysicsMeshFlags flags, ref VoxelGridParameters vgParams,
												float approximationTolerance = 0.05f);
		#endregion
	}
}