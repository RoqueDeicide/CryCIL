using System;
using System.Runtime.CompilerServices;
using CryCil.Geometry;
using CryCil.Utilities;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents an optimized triangular mesh that is stored in video memory for rendering.
	/// </summary>
	/// <example>
	/// Example of updating the colors in VRAM:
	/// <code source="MeshRecoloring.cs"/>
	/// </example>
	public unsafe struct CryRenderMesh
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Indicates whether this render mesh is loaded into video memory for rendering.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Renderable
		{
			get
			{
				this.AssertInstance();

				return CanRender(this.handle);
			}
		}
		/// <summary>
		/// Gets the type name that was given to this render mesh during creation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string TypeName
		{
			get
			{
				this.AssertInstance();

				return GetTypeName(this.handle);
			}
		}
		/// <summary>
		/// Gets the name of the source that was given to this render mesh during creation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string SourceName
		{
			get
			{
				this.AssertInstance();

				return GetSourceName(this.handle);
			}
		}
		/// <summary>
		/// Gets the number of indices that form faces that comprise this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexCount
		{
			get
			{
				this.AssertInstance();

				return GetIndicesCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the number of vertices that comprise this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int VertexCount
		{
			get
			{
				this.AssertInstance();

				return GetVerticesCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the format of vertices that comprise this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public VertexFormat VertexFormat
		{
			get
			{
				this.AssertInstance();

				return GetVertexFormat(this.handle);
			}
		}
		/// <summary>
		/// Gets the type of this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public RenderMeshType MeshType
		{
			get
			{
				this.AssertInstance();

				return GetMeshType(this.handle);
			}
		}
		/// <summary>
		/// Gets geometric mean of areas of all faces that comprise this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float GeometricMeanFaceArea
		{
			get
			{
				this.AssertInstance();

				return GetGeometricMeanFaceArea(this.handle);
			}
		}
		/// <summary>
		/// Sets the skinning data for vegetation simulation.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryMeshBoneMappingByte* VegetationSkinningData
		{
			set
			{
				this.AssertInstance();

				SetSkinningDataVegetation(this.handle, value);
			}
		}
		/// <summary>
		/// Creates <see cref="CryIndexedMesh"/> from this render mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryIndexedMesh IndexedMesh
		{
			get
			{
				this.AssertInstance();

				return GetIndexedMesh(this.handle);
			}
		}
		/// <summary>
		/// Creates <see cref="CryRenderMesh"/> that contains morph weight data generated from this render
		/// mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRenderMesh MorphWeights
		{
			get
			{
				this.AssertInstance();

				return GenerateMorphWeights(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the render mesh that serves as a morph buddy for this one.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRenderMesh MorphBuddy
		{
			get
			{
				this.AssertInstance();

				return GetMorphBuddy(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetMorphBuddy(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the identifier of the custom texture to use.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int CustomTextureId
		{
			set
			{
				this.AssertInstance();

				SetCustomTexID(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the render mesh that contains vertex data(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRenderMesh VertexContainer
		{
			get
			{
				this.AssertInstance();

				return GetVertexContainer(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetVertexContainer(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the bounding box of this render mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox BoundingBox
		{
			get
			{
				this.AssertInstance();

				BoundingBox box;
				GetBBox(this.handle, out box.Minimum, out box.Maximum);
				return box;
			}
			set
			{
				this.AssertInstance();

				SetBBox(this.handle, ref value.Minimum, ref value.Maximum);
			}
		}
		/// <summary>
		/// Indicates whether this render mesh is empty.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsEmpty
		{
			get
			{
				this.AssertInstance();

				return IsEmptyInternal(this.handle);
			}
		}
		#endregion
		#region Construction
		internal CryRenderMesh(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates new render mesh.
		/// </summary>
		/// <param name="type">      Name of the type of render mesh.</param>
		/// <param name="source">    Name of source of render mesh, file name for instance.</param>
		/// <param name="parameters">
		/// Optional pointer to the structure that contains description of the mesh.
		/// </param>
		/// <param name="meshType">  Type of the mesh.</param>
		public CryRenderMesh(string type, string source, RenderMeshInitializationParameters* parameters = null,
							 RenderMeshType meshType = RenderMeshType.Static)
		{
			this.handle = CreateRenderMesh(type, source, parameters, meshType);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increases the reference count of this render mesh. Call this when you have multiple references
		/// to the same render mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			this.AssertInstance();

			AddRef(this.handle);
		}
		/// <summary>
		/// Decreases the reference count of this render mesh. Call this when you destroy an object that
		/// held an extra reference to the this render mesh.
		/// </summary>
		/// <remarks>When reference count reaches zero, the mesh is deleted.</remarks>
		/// <returns>Current reference count(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int DecrementReferenceCount()
		{
			this.AssertInstance();

			return Release(this.handle);
		}
		/// <summary>
		/// Creates new render data buffers from given mesh.
		/// </summary>
		/// <param name="mesh">                   
		/// An object that represents a mesh to create render data buffers from.
		/// </param>
		/// <param name="secondayColorsSetOffset">The offset to shift the secondary colors by(?).</param>
		/// <param name="flags">                  
		/// A set of flags that specifies how to create the render mesh.
		/// </param>
		/// <param name="positionOffset">         
		/// Reference to the vector that represents position of the mesh in the world(?).
		/// </param>
		/// <param name="requiresLock">           
		/// Indicates whether this render mesh should be locked to prevent access from other threads during
		/// conversion process.
		/// </param>
		/// <returns>Size of resultant data if successful, <c>~0U</c> if not.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint SetMesh(CryMesh mesh, int secondayColorsSetOffset, MeshToRenderMeshFlags flags,
							ref Vector3 positionOffset, bool requiresLock)
		{
			this.AssertInstance();

			return SetMeshInternal(this.handle, mesh, secondayColorsSetOffset, flags, ref positionOffset, requiresLock);
		}
		/// <summary>
		/// Copies render data to another render mesh.
		/// </summary>
		/// <param name="destination"> Render mesh to copy data to.</param>
		/// <param name="appendVertex">Unknown.</param>
		/// <param name="dynamic">     Indicates whether render mesh is dynamic.</param>
		/// <param name="fullCopy">    Indicates whether all data buffers should be copied.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void CopyTo(CryRenderMesh destination, int appendVertex = 0, bool dynamic = false, bool fullCopy = true)
		{
			this.AssertInstance();

			CopyToInternal(this.handle, destination, appendVertex, dynamic, fullCopy);
		}
		/// <summary>
		/// Sets character skinning data for this render mesh.
		/// </summary>
		/// <param name="mesh">             Sets character mesh(?).</param>
		/// <param name="pBoneMapping">     Primary bone mapping data array.</param>
		/// <param name="pExtraBoneMapping">Secondary bone mapping data array.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSkinningDataCharacter(CryMesh mesh, CryMeshBoneMappingUint16* pBoneMapping,
											 CryMeshBoneMappingUint16* pExtraBoneMapping)
		{
			this.AssertInstance();

			SetSkinningDataCharacterInternal(this.handle, mesh, pBoneMapping, pExtraBoneMapping);
		}
		/// <summary>
		/// Updates internal array of vertex data.
		/// </summary>
		/// <param name="vertices">    An array of vertex data.</param>
		/// <param name="vertexCount"> Number of elements in the array of vertex data.</param>
		/// <param name="offset">      
		/// Zero-based index of the first vertex data element in the internal array to start update from.
		/// </param>
		/// <param name="stream">      
		/// Identifier of the stream. Use this value and <see cref="CryRenderMesh.VertexFormat"/> to make
		/// sure that <paramref name="vertices"/> array contains data of correct type.
		/// </param>
		/// <param name="copyFlags">   Unknown.</param>
		/// <param name="requiresLock">
		/// Indicates whether <paramref name="stream"/> needs to locked for thread access. If <c>true</c> is
		/// passed, <see cref="CryRenderMesh.UnlockStream"/> will need to be called.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool UpdateVertices(void* vertices, int vertexCount, int offset, int stream, uint copyFlags,
								   bool requiresLock = true)
		{
			this.AssertInstance();

			return UpdateVerticesInternal(this.handle, vertices, vertexCount, offset, stream, copyFlags, requiresLock);
		}
		/// <summary>
		/// Updates internal array of indices.
		/// </summary>
		/// <param name="indices">     An array of indices.</param>
		/// <param name="indexCount">  Number of indices in the array.</param>
		/// <param name="offset">      
		/// Zero-based index of the first index in the internal array to start update from.
		/// </param>
		/// <param name="copyFlags">   Unknown.</param>
		/// <param name="requiresLock">
		/// Indicates whether index stream needs to locked for thread access. If <c>true</c> is passed,
		/// <see cref="CryRenderMesh.UnlockIndexStream"/> will need to be called.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool UpdateIndices(uint* indices, int indexCount, int offset, uint copyFlags, bool requiresLock = true)
		{
			this.AssertInstance();

			return UpdateIndicesInternal(this.handle, indices, indexCount, offset, copyFlags, requiresLock);
		}
		/// <summary>
		/// Generates a data stream that contains tangent-space normals that are stored in quaternions.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void GenerateQTangents()
		{
			this.AssertInstance();

			GenerateQTangentsInternal(this.handle);
		}
		/// <summary>
		/// Calculates bounding box that encompasses this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void CalculateBoundingBox()
		{
			this.AssertInstance();

			UpdateBBoxFromMesh(this.handle);
		}
		/// <summary>
		/// Grants access to the internal array of positions of vertices without caching. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetPositionsPointerNoCache(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetPosPtrNoCache(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of positions of vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetPositionsPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetPosPtr(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of colors of vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetColorsPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetColorPtr(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of normals to vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetNormalsPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetNormPtr(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of texture coordinates of vertices without caching. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetUVPointerNoCache(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetUVPtrNoCache(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of texture coordinates of vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetUVPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetUVPtr(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of tangent-space normals of vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetTangentsPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetTangentPtr(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of quaternion-based tangent space normals of vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetQTangentsPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetQTangentPtr(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of HW skinning objects of vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags">   A set of flags that specifies how to access the array.</param>
		/// <param name="offset">  Zero-based index of the first element in the array to get.</param>
		/// <param name="remapped">Unknown.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetHWSkinPointer(RenderMeshAccessFlags flags, int offset = 0, bool remapped = false)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetHWSkinPtr(this.handle, out stride, flags, offset, remapped);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Grants access to the internal array of velocities of vertices. Check
		/// <see cref="CryRenderMesh.VertexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how to access the array.</param>
		/// <param name="offset">Zero-based index of the first element in the array to get.</param>
		/// <returns>
		/// A strided pointer to the array advanced by <paramref name="offset"/> multiplied by the stride.
		/// Use <see cref="StridedPointer.GetElement"/> method to access individual elements. Check
		/// <see cref="CryRenderMesh.VertexFormat"/> to see which type to cast the returned pointer to.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StridedPointer GetVelocityPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			int stride;
			byte* ptr = GetVelocityPtr(this.handle, out stride, flags, offset);
			return new StridedPointer(ptr, stride);
		}
		/// <summary>
		/// Gets the pointer to the array of indices that form faces that comprise this mesh. Check
		/// <see cref="CryRenderMesh.IndexCount"/> for number of elements in the array.
		/// </summary>
		/// <param name="flags"> A set of flags that specifies how the array must be accessed.</param>
		/// <param name="offset">Optional zero-based index of the first element to access.</param>
		/// <returns>Pointer to the internal index data advanced by <paramref name="offset"/> * 4.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint* GetIndexPointer(RenderMeshAccessFlags flags, int offset = 0)
		{
			this.AssertInstance();

			return GetIndexPtr(this.handle, flags, offset);
		}
		/// <summary>
		/// Unlocks the stream for thread access.
		/// </summary>
		/// <param name="stream">Identifier of the stream to unlock.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void UnlockStream(RenderMeshStreamIds stream)
		{
			this.AssertInstance();

			UnlockStreamInternal(this.handle, stream);
		}
		/// <summary>
		/// Unlocks index stream for thread access.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void UnlockIndexStream()
		{
			this.AssertInstance();

			UnlockIndexStreamInternal(this.handle);
		}
		/// <summary>
		/// Gets number of bytes are being used by the render mesh.
		/// </summary>
		/// <param name="videoMemory">
		/// Indicates whether video memory consumption should be check, rather then system memory.
		/// </param>
		/// <returns>
		/// Number of bytes that are currently allocated for this render mesh in specified memory.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int GetAllocatedBytes(bool videoMemory)
		{
			this.AssertInstance();

			return GetAllocatedBytesInternal(this.handle, videoMemory);
		}
		/// <summary>
		/// Changes the LOD model this render mesh is using.
		/// </summary>
		/// <param name="lod">Index of the LOD model to use.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetMeshLod(int lod)
		{
			this.AssertInstance();

			SetMeshLodInternal(this.handle, lod);
		}
		/// <summary>
		/// Locks this render mesh for thread access.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void LockForThreadAccess()
		{
			this.AssertInstance();

			LockForThreadAccessInternal(this.handle);
		}
		/// <summary>
		/// Unlocks this render mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void UnLockForThreadAccess()
		{
			this.AssertInstance();

			UnLockForThreadAccessInternal(this.handle);
		}
		/// <summary>
		/// Offsets position of this render mesh in the world.
		/// </summary>
		/// <param name="delta">Reference to the coordinates of this render mesh in the world.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void OffsetPosition(ref Vector3 delta)
		{
			this.AssertInstance();

			OffsetPositionInternal(this.handle, ref delta);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanRender(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetTypeName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetSourceName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetIndicesCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetVerticesCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern VertexFormat GetVertexFormat(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderMeshType GetMeshType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetGeometricMeanFaceArea(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint SetMeshInternal(IntPtr handle, CryMesh mesh, int nSecColorsSetOffset,
												   MeshToRenderMeshFlags flags, ref Vector3 pPosOffset, bool requiresLock);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyToInternal(IntPtr handle, CryRenderMesh pDst, int nAppendVtx, bool bDynamic,
												  bool fullCopy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSkinningDataVegetation(IntPtr handle, CryMeshBoneMappingByte* pBoneMapping);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSkinningDataCharacterInternal(IntPtr handle, CryMesh mesh,
																	CryMeshBoneMappingUint16* pBoneMapping,
																	CryMeshBoneMappingUint16* pExtraBoneMapping);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryIndexedMesh GetIndexedMesh(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderMesh GenerateMorphWeights(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderMesh GetMorphBuddy(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMorphBuddy(IntPtr handle, CryRenderMesh pMorph);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UpdateVerticesInternal(IntPtr handle, void* pVertBuffer, int nVertCount, int nOffset,
														  int stream, uint copyFlags, bool requiresLock = true);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UpdateIndicesInternal(IntPtr handle, uint* pNewInds, int nInds, int nOffsInd,
														 uint copyFlags, bool requiresLock = true);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCustomTexID(IntPtr handle, int nCustomTID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GenerateQTangentsInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderMesh GetVertexContainer(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVertexContainer(IntPtr handle, CryRenderMesh pBuf);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBBox(IntPtr handle, ref Vector3 vBoxMin, ref Vector3 vBoxMax);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBBox(IntPtr handle, out Vector3 vBoxMin, out Vector3 vBoxMax);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UpdateBBoxFromMesh(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint* GetPhysVertexMap(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsEmptyInternal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetPosPtrNoCache(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags,
													 int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetPosPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags, int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetColorPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags, int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetNormPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags, int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetUVPtrNoCache(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags,
													int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetUVPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags, int nOffset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetTangentPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags,
												  int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetQTangentPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags,
												   int nOffset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetHWSkinPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags,
												 int nOffset, bool remapped = false);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte* GetVelocityPtr(IntPtr handle, out int nStride, RenderMeshAccessFlags nFlags,
												   int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnlockStreamInternal(IntPtr handle, RenderMeshStreamIds nStream);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnlockIndexStreamInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint* GetIndexPtr(IntPtr handle, RenderMeshAccessFlags nFlags, int nOffset);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetAllocatedBytesInternal(IntPtr handle, bool bVideoMem);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMeshLodInternal(IntPtr handle, int nLod);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LockForThreadAccessInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnLockForThreadAccessInternal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OffsetPositionInternal(IntPtr handle, ref Vector3 delta);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateRenderMesh(string szType, string szSourceName,
													  RenderMeshInitializationParameters* pInitParams,
													  RenderMeshType eBufType);
		#endregion
	}
}