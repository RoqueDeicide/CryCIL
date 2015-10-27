using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Data;
using CryCil.Engine.DebugServices;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a static object - a render mesh that doesn't have any animations.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct StaticObject
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		[FieldOffset(0)] private readonly StaticSubObjects subObjects;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		internal IntPtr Handle
		{
			get { return this.handle; }
		}

		/// <summary>
		/// Gets or sets the flags that are assigned to this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObjectFlags Flags
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetFlags(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetFlags(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the index of the explosion shape used by this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int BreakabilityIndex
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetIdMatBreakable(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the physical body that represents this static object in physical world.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalBody PhysicalBody
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetPhysGeom(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetPhysGeom(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the tetrahedral lattice used by this static object, if it's breakable.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public TetraLattice Lattice
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetTetrLattice(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the material that is used to display this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material Material
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetMaterial(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetMaterial(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the bounding box of this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox BoundingBox
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetBox(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetBBox(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the radius of this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float Radius
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetRadius(this.handle);
			}
		}
		/// <summary>
		/// Gets the lowest LOD object for this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject LowestLod
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetLowestLodInternal(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the path to the file this static object is associated with.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string FilePath
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetFilePath(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetFilePath(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the name of this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string GeometryName
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetGeoName(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetGeoName(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the value that indicates whether this static object is a default one.
		/// </summary>
		/// <remarks>
		/// Probably used to check whether searching/loading of static object was successful.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsDefault
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return IsDefaultObject(this.handle);
			}
		}
		/// <summary>
		/// Gets the value that indicates whether this static object is physicalized.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Physicalized
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return IsPhysicsExist(this.handle);
			}
		}
		/// <summary>
		/// Gets the collection of sub-objects this static object consists of, if it's a compound object.
		/// </summary>
		public StaticSubObjects SubObjects
		{
			get { return this.subObjects; }
		}
		/// <summary>
		/// Gets the value that indicates whether this static object is a sub-object of another static
		/// object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsSubObject
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return IsSubObjectInternal(this.handle);
			}
		}
		/// <summary>
		/// Gets the parent static object if this one is a sub-object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject Parent
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetParentObject(this.handle);
			}
		}
		/// <summary>
		/// Gets the source static object this one was cloned from, if this static object is a clone.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject CloneSource
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetCloneSourceObject(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this static object is deformable.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsDeformable
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return IsDeformableInternal(this.handle);
			}
		}
		/// <summary>
		/// Gets the text that contains the properties of this static object that were loaded from .cgf
		/// file.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Properties
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetProperties(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that encapsulates various statistical data about this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObjectStatistics Statistics
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				StaticObjectStatistics stats;
				GetStatistics(this.handle, out stats);
				return stats;
			}
		}
		#endregion
		#region Construction
		internal StaticObject(IntPtr handle)
		{
			this.subObjects = new StaticSubObjects();
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new static object.
		/// </summary>
		/// <param name="createIndexedMesh">
		/// Indicates whether indexed mesh should be created for this static object.
		/// </param>
		public StaticObject(bool createIndexedMesh)
		{
			this.subObjects = new StaticSubObjects();
			this.handle = CreateStatObjOptionalIndexedMesh(createIndexedMesh);
		}
		/// <summary>
		/// Creates a static object that is an updated version of another static object that had given
		/// geometry created from it.
		/// </summary>
		/// <param name="physicalGeometry">
		/// An object that represents the physical geometry that was created from static object.
		/// </param>
		/// <param name="lastUpdate">      Pointer to the last mesh update to take into account.</param>
		public StaticObject(GeometryShape physicalGeometry, MeshUpdate* lastUpdate = null)
		{
			this.subObjects = new StaticSubObjects();
			this.handle = UpdateDeformableStatObj(physicalGeometry, lastUpdate);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increases the reference count of this static object. Call this when you have multiple
		/// references to the same static object.
		/// </summary>
		/// <returns>Current reference count(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IncrementReferenceCount()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return AddRef(this.handle);
		}
		/// <summary>
		/// Decreases the reference count of this static object. Call this when you destroy an object that
		/// held an extra reference to the this static object.
		/// </summary>
		/// <remarks>When reference count reaches zero, the object is deleted.</remarks>
		/// <returns>Current reference count(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int DecrementReferenceCount()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return Release(this.handle);
		}
		/// <summary>
		/// Updates vertices and normals on the render mesh. Clones this static object, if necessary.
		/// </summary>
		/// <param name="vertices">   An array of new positions of vertices.</param>
		/// <param name="normals">    An array of new normals for vertices.</param>
		/// <param name="firstVertex">Index of the first vertex in render mesh to update.</param>
		/// <param name="vertexCount">Number of vertexes to update.</param>
		/// <returns>
		/// If this static object was cloned, then this object will be returned, otherwise a new cloned
		/// static object will be created and returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">
		/// An array of vertices cannot be null or empty.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// An array of normals cannot be null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">Number of vertices and normals must be equal.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Not enough vertices are provided.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Number of vertices to update cannot be less then 0.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the first vertex to update cannot be less then 0.
		/// </exception>
		public StaticObject UpdateVertices(Vector3[] vertices, Vector3[] normals, int firstVertex, int vertexCount)
		{
			this.AssertInstance();
			if (vertices.IsNullOrEmpty())
			{
				throw new ArgumentNullException("vertices", "An array of vertices cannot be null or empty.");
			}
			if (normals.IsNullOrEmpty())
			{
				throw new ArgumentNullException("normals", "An array of normals cannot be null or empty.");
			}
			if (vertices.Length != normals.Length)
			{
				throw new ArgumentException("Number of vertices and normals must be equal.");
			}
			if (vertexCount > vertices.Length)
			{
				throw new ArgumentOutOfRangeException("vertexCount", "Not enough vertices are provided.");
			}
			if (vertexCount < 0)
			{
				throw new ArgumentOutOfRangeException("vertexCount",
													  "Number of vertices to update cannot be less then 0.");
			}
			if (firstVertex < 0)
			{
				throw new ArgumentOutOfRangeException("firstVertex",
													  "Index of the first vertex to update cannot be less then 0.");
			}
			Contract.EndContractBlock();

			fixed (Vector3* verticesPtr = vertices)
			fixed (Vector3* normalsPtr = normals)
			{
				return UpdateVerticesInternal(this.handle, verticesPtr, normalsPtr, firstVertex, vertexCount, 1);
			}
		}
		/// <summary>
		/// Refreshes parts of this static object.
		/// </summary>
		/// <param name="flags">
		/// A set of flags that specify which aspects of this static object to reload. Acceptable flags are
		/// <see cref="ResourceRefreshFlags.Shaders"/>, <see cref="ResourceRefreshFlags.Textures"/>,
		/// <see cref="ResourceRefreshFlags.Geometry"/>.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Refresh(ResourceRefreshFlags flags)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

#if DEBUG
			ResourceRefreshFlags acceptableFlags = ResourceRefreshFlags.Shaders | ResourceRefreshFlags.Textures |
												   ResourceRefreshFlags.Geometry;
			if ((flags & ~acceptableFlags) != 0)
			{
				Type enumType = typeof(ResourceRefreshFlags);
				string shadersName = Enum.GetName(enumType, ResourceRefreshFlags.Shaders);
				string texturesName = Enum.GetName(enumType, ResourceRefreshFlags.Textures);
				string geometryName = Enum.GetName(enumType, ResourceRefreshFlags.Geometry);
				string message =
					string.Format("Only {0}, {1}, {2} flags from {3} can be accepted when refreshing static object.",
								  shadersName, texturesName, geometryName, enumType.Name);
				Log.Warning(message, false);
			}
#endif
			RefreshInternal(this.handle, flags);
		}
		/// <summary>
		/// Gets the random point on this static object.
		/// </summary>
		/// <param name="aspect">Aspect of this static object's geometry to get the point on.</param>
		/// <returns>A point on the surface and a normal to it.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PositionNormal GetRandomPoint(GeometryFormat aspect)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetRandomPos(this.handle, aspect);
		}
		/// <summary>
		/// Gets the static object that represents a LOD model for this static object.
		/// </summary>
		/// <param name="lodLevel">     Number that specifies which level of detail to get.</param>
		/// <param name="returnNearest">
		/// Optional value that indicates whether nearest valid LOD model should be returned, if provided
		/// <paramref name="lodLevel"/> doesn't exists.
		/// </param>
		/// <returns>The static object that represents a LOD model for this static object.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject GetLodObject(int lodLevel, bool returnNearest = false)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetLodObjectInternal(this.handle, lodLevel, returnNearest);
		}
		/// <summary>
		/// Finds the LOD level that is closest to the provided value.
		/// </summary>
		/// <param name="lodIn">   The value to start the search from.</param>
		/// <param name="searchUp">Indicates whether to search towards higher value.</param>
		/// <returns>A number that specifies the LOD level.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int FindNearestLoadedLod(int lodIn, bool searchUp = false)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return FindNearesLoadedLodInternal(this.handle, lodIn, searchUp);
		}
		/// <summary>
		/// Finds the highest LOD level.
		/// </summary>
		/// <param name="bias">Unknown.</param>
		/// <returns>The highest LOD level.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int FindHighestLod(int bias)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return FindHighestLodInternal(this.handle, bias);
		}
		/// <summary>
		/// Gets the position of the helper.
		/// </summary>
		/// <param name="helperName">Name of the helper that was specified when CGF was exported.</param>
		/// <returns>Coordinates of the helper in unknown coordinate space.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector3 GetHelperPosition(string helperName)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetHelperPos(this.handle, helperName);
		}
		/// <summary>
		/// Gets the transformation matrix of the helper.
		/// </summary>
		/// <param name="helperName">Name of the helper that was specified when CGF was exported.</param>
		/// <returns>Transformation matrix of the helper in unknown coordinate space.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Matrix34 GetHelperTransformation(string helperName)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetHelperTM(this.handle, helperName);
		}
		/// <summary>
		/// Releases the memory that was allocated for the indexed mesh of this static object.
		/// </summary>
		/// <remarks>
		/// Probably can be used when actual mesh is stored in a different format, and so isn't needed once
		/// render mesh is created.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ReleaseIndexedMesh()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			FreeIndexedMesh(this.handle);
		}
		/// <summary>
		/// Marks the render mesh and (optionally) physics mesh and forces them to be re-created from
		/// hosted indexed mesh.
		/// </summary>
		/// <param name="physics">  Indicates whether physics geometry must be recreated.</param>
		/// <param name="tolerance">Unknown.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Invalidate(bool physics = false, float tolerance = 0.05f)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			InvalidateInternal(this.handle, physics, tolerance);
		}
		/// <summary>
		/// Physicalizes this static object and all sub-objects as parts that are then added to physical
		/// entity.
		/// </summary>
		/// <param name="entity">       Physical entity to add parts to.</param>
		/// <param name="pgp">          
		/// Reference to the object that specifies how to physicalize each part.
		/// </param>
		/// <param name="id">           Identifier of the first part.</param>
		/// <param name="propsOverride">Unknown.</param>
		/// <returns>Identifier of the last part.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Physicalize(PhysicalEntity entity, ref GeometryParameters pgp, int id = 0, string propsOverride = null)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return PhysicalizeInternal(this.handle, entity, ref pgp, id, propsOverride);
		}
		/// <summary>
		/// Saves this static object into .cgf file.
		/// </summary>
		/// <param name="file">       Path to the file.</param>
		/// <param name="savePhysics">Indicates whether physical mesh must be saved as well.</param>
		/// <returns>Indication of success.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Save([PathReference] string file, bool savePhysics = false)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return SaveToCGF(this.handle, file, savePhysics);
		}
		/// <summary>
		/// Creates deep copy of this static object.
		/// </summary>
		/// <param name="cloneGeometry">Indicates whether geometry must be cloned.</param>
		/// <param name="cloneChildren">Indicates whether sub-objects must be cloned.</param>
		/// <param name="meshesOnly">   Indicates whether only meshes must be cloned.</param>
		/// <returns>A new static object, changes to that object won't affect this one.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject Clone(bool cloneGeometry, bool cloneChildren, bool meshesOnly)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return CloneInternal(this.handle, cloneGeometry, cloneChildren, meshesOnly);
		}
		/// <summary>
		/// Sets given static object as this one's Morph Buddy and makes sure that both objects have
		/// one-to-one vertex correspondence.
		/// </summary>
		/// <param name="deformed">A static object to have as morph buddy.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetDeformationMorphTarget(StaticObject deformed)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return SetDeformationMorphTargetInternal(this.handle, deformed) != 0;
		}
		/// <summary>
		/// Applies deformational force to this static object.
		/// </summary>
		/// <param name="point">   Point of application of the force.</param>
		/// <param name="radius">  Radius of area of effect.</param>
		/// <param name="strength">Strength of the force.</param>
		/// <returns>Either this static object, or a clone of it.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject Deform(ref Vector3 point, float radius, float strength)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return DeformMorph(this.handle, ref point, radius, strength);
		}
		/// <summary>
		/// Hides non-physicalized geometry.
		/// </summary>
		/// <returns>Either this static object, or a clone of it.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject HideNonPhysicalizedGeometry()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return HideFoliage(this.handle);
		}
		/// <summary>
		/// Synchronizes this static object with its data representation.
		/// </summary>
		/// <param name="sync">An object that handles synchronization.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Synchronize(CrySync sync)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			Serialize(this.handle, sync);
		}
		/// <summary>
		/// Gets mass and density of this static object.
		/// </summary>
		/// <param name="mass">   Mass of this static object.</param>
		/// <param name="density">Density of this static object.</param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetPhysicalProperties(out float mass, out float density)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetPhysicalPropertiesInternal(this.handle, out mass, out density);
		}
		/// <summary>
		/// Gets last shape that was subtracted from this static object.
		/// </summary>
		/// <param name="scale">Relative scale of the shape.</param>
		/// <returns>The static object that represents the shape.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject GetLastCutOut(out float scale)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetLastBooleanOp(this.handle, out scale);
		}
		/// <summary>
		/// Gets the object that contains the mesh data before its converted into render mesh.
		/// </summary>
		/// <param name="createIfNone">
		/// Indicates whether a new mesh needs to be created if it doesn't exist.
		/// </param>
		/// <returns>
		/// An object that contains the triangular mesh. If one didn't exist, then either invalid object
		/// will be returned or a new one depending on <paramref name="createIfNone"/>.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryIndexedMesh GetIndexedMesh(bool createIfNone = false)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			return GetIndexedMeshInternal(this.handle, createIfNone);
		}
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
		private static extern int AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, StaticObjectFlags nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObjectFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetIdMatBreakable(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryIndexedMesh GetIndexedMeshInternal(IntPtr handle, bool bCreateIfNone);

		////! Access to rendering geometry for indoor engine ( optimized vertex arrays, lists of shader pointers )
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern struct IRenderMesh * GetRenderMesh();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalBody GetPhysGeom(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject UpdateVerticesInternal(IntPtr handle, Vector3* vertices, Vector3* normals,
																  int firstVertex, int vertexCount, float scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPhysGeom(IntPtr handle, PhysicalBody pPhysGeom);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern TetraLattice GetTetrLattice(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaterial(IntPtr handle, Material pMaterial);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetMaterial(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern BoundingBox GetBox(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBBox(IntPtr handle, BoundingBox vBBoxMin);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetRadius(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RefreshInternal(IntPtr handle, ResourceRefreshFlags nFlags);

		//// Description:
		////     Registers the object elements into the renderer.
		//// Arguments:
		////     rParams   - Render parameters
		////     nLogLevel - Level of the LOD
		//// Summary:
		////     Renders the object
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern void Render(IntPtr handle, const struct SRendParams &rParams,
		//	const SRenderingPassInfo &passInfo);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PositionNormal GetRandomPos(IntPtr handle, GeometryFormat eForm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject GetLodObjectInternal(IntPtr handle, int nLodLevel, bool bReturnNearest);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject GetLowestLodInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int FindNearesLoadedLodInternal(IntPtr handle, int nLodIn, bool bSearchUp);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int FindHighestLodInternal(IntPtr handle, int nBias);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetFilePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFilePath(IntPtr handle, string szFileName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetGeoName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGeoName(IntPtr handle, string szGeoName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetHelperPos(IntPtr handle, string szHelperName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix34 GetHelperTM(IntPtr handle, string szHelperName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDefaultObject(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FreeIndexedMesh(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsPhysicsExist(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InvalidateInternal(IntPtr handle, bool bPhysics, float tolerance);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSubObjectCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSubObjectCount(IntPtr handle, int nCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern StaticSubObject GetSubObject(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsSubObjectInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject GetParentObject(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject GetCloneSourceObject(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern StaticSubObject FindSubObject(IntPtr handle, string sNodeName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern StaticSubObject FindSubObject_CGA(IntPtr handle, string sNodeName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern StaticSubObject FindSubObject_StrStr(IntPtr handle, string sNodeName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool RemoveSubObject(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool CopySubObject(IntPtr handle, int nToIndex, StaticObject pFromObj, int nFromIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern StaticSubObject AddSubObject(IntPtr handle, StaticObject pStatObj);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int PhysicalizeSubobjects(IntPtr handle, PhysicalEntity pent, ref Matrix34 pMtx, float mass,
														 float density, int id0, string szPropsOverride);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int PhysicalizeInternal(IntPtr handle, PhysicalEntity pent, ref GeometryParameters pgp,
													  int id, string szPropsOverride);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDeformableInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SaveToCGF(IntPtr handle, string sFilename, bool bHavePhysicalProxy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject CloneInternal(IntPtr handle, bool bCloneGeometry, bool bCloneChildren,
														 bool bMeshesOnly);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SetDeformationMorphTargetInternal(IntPtr handle, StaticObject pDeformed);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject DeformMorph(IntPtr handle, ref Vector3 pt, float r, float strength);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject HideFoliage(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Serialize(IntPtr handle, CrySync ser);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetProperties(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetPhysicalPropertiesInternal(IntPtr handle, out float mass, out float density);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern StaticObject GetLastBooleanOp(IntPtr handle, out float scale);

		//// Intersect ray with static object.
		//// Ray must be in object local space.
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern bool RayIntersection(IntPtr handle,  SRayHitInfo &hitInfo,IMaterial *pCustomMtl=0 );
		//// Intersect lineseg with static object. Works on dedi server as well.
		//// Lineseg must be in object local space. Returns the hit position and the surface type id of the point hit.
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern bool LineSegIntersection(IntPtr handle, const Lineseg &lineSeg, Vector3 &hitPos, int &surfaceTypeId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetStatistics(IntPtr handle, out StaticObjectStatistics stats);

		//// Returns initial hide mask
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern ulong GetInitialHideMask(IntPtr handle);
		//// Updates hide mask as new mask = (mask & maskAND) | maskOR
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern ulong UpdateInitialHideMask(IntPtr handle, ulong maskAnd = unchecked (0ul - 1ul),
		//												  ulong maskOr = 0);
		//// Set the filename of the mesh of the next state (for example damaged version)
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern void SetStreamingDependencyFilePath(IntPtr handle, string szFileName);
		////exposes the computelod function from the engine
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern int ComputeLodFromScale(IntPtr handle, float fScale, float fLodRatioNormalized,
		//											  float fEntDistance, bool bFoliage, bool bForPrecache);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateStatObjOptionalIndexedMesh(bool createIndexedMesh);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr UpdateDeformableStatObj(GeometryShape pPhysGeom, MeshUpdate* pLastUpdate);
		#endregion
	}
}