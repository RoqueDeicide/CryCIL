using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Physics.Primitives;
using CryCil.Geometry;
using CryCil.Utilities;

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
		/// <summary>
		/// Creates a geometry object that represents a primitive geometric object.
		/// </summary>
		/// <param name="type">     
		/// Type of geometric object to create. There is no way to detect a mismatch between a type what is
		/// actually passed with <paramref name="primitive"/>.
		/// </param>
		/// <param name="primitive">
		/// Reference to the base part of the object that provides information about a primitive.
		/// </param>
		public GeometryShape(int type, ref Primitive.BasePrimitive primitive)
		{
			this.handle = IntPtr.Zero;

			if (Array.BinarySearch(Primitive.RegisteredTypes, type) < 0)
			{
				throw new NotSupportedException(string.Format("Primitive type with identifier = {0} is not supported",
															  type));
			}

			Contract.EndContractBlock();

			this.handle = CreatePrimitive(type, ref primitive);
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
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreatePrimitive(int type, ref Primitive.BasePrimitive primitive);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGeometryType(); // see enum geomtypes
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddRef();
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Release();
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Lock(int bWrite=1); // locks the geometry for reading or writing
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Unlock(int bWrite=1); // bWrite should match the preceding Lock
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBBox( out Primitive.Box pbox); // possibly oriented bbox (depends on BV tree type)
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int CalcPhysicalProperties(PhysicalBody pgeom);	// O(num_triangles) for meshes, unless mesh_always_static is set
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int PointInsideStatus(ref Vector3 pt); // for meshes, will create an auxiliary hashgrid for acceleration
	// IntersectLocked - the main function for geomtries. pdata1,pdata2,pparams can be 0 - defaults will be assumed.
	// returns a pointer to an internal thread-specific contact buffer, locked with the lock argument
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int IntersectLocked(GeometryShape pCollider, ref GeometryWorldData pdata1, ref GeometryWorldData pdata2, ref IntersectionParameters pparams, out GeometryContact *pcontacts, WriteLockCond @lock);
	// Intersect - same as Intersect, but doesn't lock pcontacts
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Intersect(GeometryShape pCollider, ref GeometryWorldData pdata1,ref GeometryWorldData pdata2, ref IntersectionParameters pparams, out GeometryContact *pcontacts);
	// FindClosestPoint - for non-convex meshes only does local search, doesn't guarantee global minimum
	// iFeature's format: (feature type: 2-face, 1-edge, 0-vertex)<<9 | feature index
	// if ptdst0 and ptdst1 are different, searches for a closest point on a line segment
	// ptres[0] is the closest point on the geometry, ptres[1] - on the test line segment
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int FindClosestPoint(ref GeometryWorldData pgwd, out int iPrim,out int iFeature, ref Vector3 ptdst0,ref Vector3 ptdst1, Vector3 *ptres, int nMaxIters=10);
	// CalcVolumetricPressure: a fairly correct computation of volumetric pressure with inverse-quadratic falloff (ex: explosions)
	// for a surface fragment dS, impulse is: k*dS*cos(surface_normal,direction to epicenter) / max(rmin, distance to epicenter)^2
	// returns integral impulse and angular impulse
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CalcVolumetricPressure(ref GeometryWorldData gwd, ref Vector3 epicenter,float k,float rmin, ref Vector3 centerOfMass, out Vector3 P,out Vector3 L);
	// CalculateBuoyancy: computes the submerged volume (return value) and the mass center of the submerged part
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float CalculateBuoyancy(ref Primitive.Plane pplane, ref GeometryWorldData pgwd, out Vector3 submergedMassCenter);
	// CalculateMediumResistance: computes medium resistance integral of the surface; self flow of the medium should be baked into pgwd
	// for a surface fragment dS with normal n and velocity v impulse is: -n*max(0,n*v) (can be scaled by the medium resistance coeff. later)
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CalculateMediumResistance(ref Primitive.Plane pplane, ref GeometryWorldData pgwd, out Vector3 dPres,out Vector3 dLres);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPrimitiveId(int iPrim,int iFeature); // get material id for a primitive (iFeature is ignored currently)
	// GetPrimitive: expects a valid pprim pointer, type depends on GetType; meshes return primitives::triangle
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPrimitive(int iPrim, out Primitive.BasePrimitive pprim); 
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetForeignIdx(int iPrim);	// only works for meshes
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetFeature(int iPrim,int iFeature, Vector3 *pt); // returns vertices of face, edge, or vertex; only for boxes and meshes currently
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int IsConvex(float tolerance);
	// PrepareForRayTest: creates an auxiliary hash structure for short rays test acceleration
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PrepareForRayTest(float raylen);	// raylen - 'expected' ray length to optimize the hash for
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPrimitiveCount();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Primitive.BasePrimitive *GetData();	// returns an pointer to an internal structure; for meshes returns mesh_data
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetData(ref Primitive.BasePrimitive primitive);	// not supported by meshes
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetVolume();
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetCenter();
	// Subtract: performs boolean subtraction; if bLogUpdates==1, will create bop_meshupdate inside the mesh
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Subtract(GeometryShape pGeom, ref GeometryWorldData pdata1,ref GeometryWorldData pdata2, int bLogUpdates=1);
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetSubtractionsCount();	// number of Subtract()s the mesh has survived so far
	// GetForeignData: returns a pointer associated with the geometry
	// special: GetForeignData(DATA_MESHUPDATE) returns the internal bop_meshupdate list (does not interfere with the main foreign pointer)
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void *GetForeignData(int iForeignData=0); 
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetiForeignData(); // foreign data type 
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetForeignData(void *pForeignData, int iForeignData);
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetErrorCount(); // for meshes, the number of edges that don't belong to exactly 2 triangles
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyAuxilaryMeshData(int idata); // see meshAuxData enum
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemapForeignIdx(int *pCurForeignIdx, int *pNewForeignIdx, int nTris); // used in rendermesh-physics sync after boolean ops
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AppendVertices(Vector3 *pVtx,int *pVtxMap, int nVtx);	// used in rendermesh-physics sync after boolean ops
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetExtent(GeometryFormat eForm);
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRandomPos( out Vector3 position, out Vector3 normal, GeometryFormat eForm);	
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CompactMemory(); // used only by non-breakable meshes to compact non-shared vertices into same contingous block of memory
	// Boxify: attempts to build a set of boxes covering the geometry's volume (only supported by trimeshes)
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Boxify(Primitive.Box *pboxes,int nMaxBoxes, ref BoxificationParameters parameters);
	// Sanity check the geometry. i.e. its tree doesn't have an excessive depth. returns 0 if fails
	[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SanityCheck();
		#endregion
	}
}