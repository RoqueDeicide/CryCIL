using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates information about the indexed mesh.
	/// </summary>
	public unsafe struct IndexedMeshData
	{
		/// <summary>
		/// Pointer to the array of faces that comprise the mesh.
		/// </summary>
		public CryMeshFace* Faces;
		/// <summary>
		/// Pointer to the array of positions of vertices in 3D space.
		/// </summary>
		public Vector3* VertexPositions;
#pragma warning disable 169
		private Vector3Half* vertsF16;
#pragma warning restore 169
		/// <summary>
		/// Pointer to the array of normals.
		/// </summary>
		public CryMeshNormal* Normals;
		/// <summary>
		/// Pointer to the array of vertex colors.
		/// </summary>
		public CryMeshColor* Colors;
		/// <summary>
		/// Pointer to the array of texture coordinates.
		/// </summary>
		public CryMeshTexturePosition* TextureCoordinates;
		/// <summary>
		/// Pointer to the array of indices that comprise the faces.
		/// </summary>
		public int* Indices;
		/// <summary>
		/// Number of faces that can be accessed via <see cref="Faces"/>.
		/// </summary>
		public int FaceCount;
		/// <summary>
		/// Number of faces that can be accessed via <see cref="VertexPositions"/>, <see cref="Normals"/> and <see cref="Colors"/>.
		/// </summary>
		public int VertexCount;
		/// <summary>
		/// Number of faces that can be accessed via <see cref="TextureCoordinates"/>.
		/// </summary>
		public int TextureCoordinatesCount;
		/// <summary>
		/// Number of faces that can be accessed via <see cref="Indices"/>.
		/// </summary>
		public int IndexCount;
	}
	/// <summary>
	/// Represents a triangular mesh that is hosted by <see cref="StaticObject"/>.
	/// </summary>
	public unsafe struct CryIndexedMesh
	{
		#region Fields
		private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets the object that provides access to data arrays.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public IndexedMeshData Data
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				IndexedMeshData desc;
				GetMeshDescription(this.handle, out desc);
				return desc;
			}
		}
		/// <summary>
		/// Gets or sets the editable object that represents this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryMesh Mesh
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetMesh(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetMesh(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the number of faces that comprise this mesh.
		/// </summary>
		/// <remarks>
		/// Setting this property invalidates <see cref="IndexedMeshData.Faces"/> pointer in <see cref="Data"/>.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int FaceCount
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetFaceCount(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetFaceCount(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the number of vertices that comprise this mesh.
		/// </summary>
		/// <remarks>
		/// Setting this property invalidates <see cref="IndexedMeshData.VertexPositions"/>, <see cref="IndexedMeshData.Normals"/> and <see cref="IndexedMeshData.Colors"/> pointers in <see cref="Data"/>.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int VertexCount
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetVertexCount(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetVertexCount(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the number of texture coordinates that map this mesh onto texture map.
		/// </summary>
		/// <remarks>
		/// Setting this property invalidates <see cref="IndexedMeshData.TextureCoordinates"/> pointer in <see cref="Data"/>.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int TextureCoordinatesCount
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetTexCoordCount(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetTexCoordCount(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the number of indices that form faces that comprise this mesh.
		/// </summary>
		/// <remarks>
		/// Setting this property invalidates <see cref="IndexedMeshData.Indices"/> pointer in <see cref="Data"/>.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexCount
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetIndexCount(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetIndexCount(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the bounding box that encompasses this indexed mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox BoundingBox
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetBBox(this.handle);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SetBBox(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets the collection of mesh subsets.
		/// </summary>
		public CryIndexedMeshSubsets Subsets
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return new CryIndexedMeshSubsets(this.handle);
			}
		}
		#endregion
		#region Construction
		internal CryIndexedMesh(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases and invalidates this object.
		/// </summary>
		public void Release()
		{
			if (!this.IsValid)
			{
				return;
			}

			ReleaseInternal(this.handle);
			this.handle = IntPtr.Zero;
		}
		/// <summary>
		/// Releases internal data. Invalidates pointers in <see cref="Data"/>.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void ReleaseData()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			FreeStreams(this.handle);
		}
		/// <summary>
		/// Calculates bounding box that encompasses this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void CalculateBoundingBox()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			CalcBBox(this.handle);
		}
		/// <summary>
		/// Optimizes this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Optimize()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			OptimizeInternal(this.handle);
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
		private static extern void ReleaseInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMeshDescription(IntPtr handle, out IndexedMeshData meshDesc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryMesh GetMesh(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMesh(IntPtr handle, CryMesh mesh);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FreeStreams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetFaceCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFaceCount(IntPtr handle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetVertexCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVertexCount(IntPtr handle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetTexCoordCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTexCoordCount(IntPtr handle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetIndexCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIndexCount(IntPtr handle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSubSetCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSubSetCount(IntPtr handle, int nSubsets);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern CryMeshSubset* GetSubSet(IntPtr handle, int nIndex);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSubsetBounds(IntPtr handle, int nIndex, ref Vector3 vCenter, float fRadius);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSubsetIndexVertexRanges(IntPtr handle, int nIndex, int nFirstIndexId,
															   int nNumIndices, int nFirstVertId, int nNumVerts);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSubsetMaterialId(IntPtr handle, int nIndex, int nMatID);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSubsetMaterialProperties(IntPtr handle, int nIndex, MaterialFlags nMatFlags,
																PhysicalGeometryType nPhysicalizeType);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBBox(IntPtr handle, ref BoundingBox box);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern BoundingBox GetBBox(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CalcBBox(IntPtr handle);
		//[MethodImpl(MethodImplOptions.InternalCall)]
		//private static extern void RestoreFacesFromIndices(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OptimizeInternal(IntPtr handle);
		#endregion
	}
}