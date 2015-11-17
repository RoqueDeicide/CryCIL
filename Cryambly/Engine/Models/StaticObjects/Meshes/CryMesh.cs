using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// This type is a mirror of CryEngine's general purpose mesh class.
	/// </summary>
	public unsafe struct CryMesh : IEquatable<CryMesh>
	{
		#region Fields
		private readonly CMeshInternals* handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != null; }
		}
		/// <summary>
		/// Gets the collection of faces that comprise this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryMeshFaceCollection Faces
		{
			get
			{
				this.AssertInstance();

				return new CryMeshFaceCollection(this, this.handle);
			}
		}
		/// <summary>
		/// Gets the object that provides data that specifies the vertices of this mesh.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryVertexCollection Vertexes
		{
			get
			{
				this.AssertInstance();

				return new CryVertexCollection(this, this.handle);
			}
		}
		/// <summary>
		/// Gets the collection of vectors that define mapping of this mesh onto texture map.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryTexturePositionCollection TexturePositions
		{
			get
			{
				this.AssertInstance();

				return new CryTexturePositionCollection(this, this.handle);
			}
		}

		///// <summary>
		///// Gets or sets the number of tangents that can be stored in the internal array. Setting this to value that is less or equal to 0 deallocates the array. Setting this value also reallocates number of texture positions.
		///// </summary>
		//public int TangentCount
		//{
		//	get
		//	{
		//		this.AssertInstance();
		//	Contract.EndContractBlock();

		//		return this.handle->streamSize[(int)GeneralMeshStreamId.Tangents];
		//	}
		//	set
		//	{
		//		this.AssertInstance();
		//	Contract.EndContractBlock();
		//		value = value < 0 ? 0 : value;	// Clamp negative values.
		//		if (this.TangentCount == value)
		//		{
		//			return;
		//		}
		//		ReallocateStream(this.handle, GeneralMeshStreamId.Tangents, value);
		//		ReallocateStream(this.handle, GeneralMeshStreamId.TextureCoordinates, value);
		//	}
		//}

		/// <summary>
		/// Gets the collection of mesh subsets.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryMeshSubsetCollection Subsets
		{
			get
			{
				this.AssertInstance();

				return new CryMeshSubsetCollection(this.handle);
			}
		}

		///// <summary>
		///// Gets or sets the number of indices that can be stored in the internal array. Setting this to value that is less or equal to 0 deallocates the array.
		///// </summary>
		//public int IndexCount
		//{
		//	get
		//	{
		//		this.AssertInstance();
		//	Contract.EndContractBlock();

		// return this.handle->streamSize[(int)GeneralMeshStreamId.Indices]; } set { this.AssertInstance();
		// Contract.EndContractBlock();

		//		ReallocateStream(this.handle, GeneralMeshStreamId.Indices, value);
		//	}
		//}
		#endregion
		#region Construction
		internal CryMesh(IntPtr handle)
		{
			this.handle = (CMeshInternals*)handle.ToPointer();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Copies mesh data from another mesh to this one.
		/// </summary>
		/// <param name="source">Mesh object to copy the data from.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Copy(CryMesh source)
		{
			this.AssertInstance();

			if (!source.IsValid)
			{
				return;
			}

			CopyMesh(this.handle, source.handle);
		}
		/// <summary>
		/// Determines whether this mesh object contains the same data streams as another one.
		/// </summary>
		/// <param name="other">Another mesh object.</param>
		/// <returns>True, another object is valid and contains the same data.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Equals(CryMesh other)
		{
			this.AssertInstance();

			return other.IsValid && CompareStreams(this.handle, other.handle);
		}
		/// <summary>
		/// Appends data from another mesh object to the end of this one's.
		/// </summary>
		/// <param name="source">    Mesh object to copy data from.</param>
		/// <param name="throwError">
		/// Indicates whether error messages that are returned by internal function must be thrown as
		/// exception objects, rather then posted into log.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="Exception">
		/// An error has occurred when appending data. This exception can only be thrown if
		/// <paramref name="throwError"/> is set to <c>true</c>.
		/// </exception>
		public void Append(CryMesh source, bool throwError = false)
		{
			this.AssertInstance();

			if (!source.IsValid)
			{
				return;
			}

			string error = AppendData(this.handle, source.handle, throwError);

			if (error != null)
			{
				throw new Exception(error);
			}
		}
		/// <summary>
		/// Appends specific parts of another mesh object to this one.
		/// </summary>
		/// <param name="source">     Mesh object to copy data from.</param>
		/// <param name="firstVertex">Zero-based index of the first vertex to append.</param>
		/// <param name="vertexCount">Number of vertices to append.</param>
		/// <param name="firstFace">  Zero-based index of the first face to append.</param>
		/// <param name="faceCount">  Number of faces to append.</param>
		/// <param name="throwError"> 
		/// Indicates whether error messages that are returned by internal function must be thrown as
		/// exception objects, rather then posted into log.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="Exception">
		/// An error has occurred when appending data. This exception can only be thrown if
		/// <paramref name="throwError"/> is set to <c>true</c>.
		/// </exception>
		public void Append(CryMesh source, int firstVertex, int vertexCount, int firstFace, int faceCount,
						   bool throwError = false)
		{
			this.AssertInstance();

			if (!source.IsValid)
			{
				return;
			}

			string error = AppendSpecificData(this.handle, source.handle, firstVertex, vertexCount, firstFace, faceCount,
											  throwError);

			if (error != null)
			{
				throw new Exception(error);
			}
		}
		/// <summary>
		/// Validates this mesh object.
		/// </summary>
		/// <returns>True, if this mesh object can be used to create a valid render mesh.</returns>
		public bool Validate()
		{
			return this.IsValid && ValidateInternal(this.handle);
		}
		/// <summary>
		/// Validates this mesh object.
		/// </summary>
		/// <param name="message">
		/// When this method concludes this parameter will be either <c>null</c> (if method returned
		/// <c>true</c>), or an error message that specifies what caused the validation error.
		/// </param>
		/// <returns>True, if this mesh object can be used to create a valid render mesh.</returns>
		public bool Validate(out string message)
		{
			if (!this.IsValid)
			{
				message = "This instance is not usable.";
				return false;
			}

			return ValidateMessage(this.handle, out message);
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
		internal static extern void* GetStreamPtr(CMeshInternals* handle, GeneralMeshStreamId stream,
												  out int pElementCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ReallocateStream(CMeshInternals* handle, GeneralMeshStreamId stream, int newCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSubsetCount(CMeshInternals* handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSubsetCount(CMeshInternals* handle, int count);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyMesh(CMeshInternals* handle, CMeshInternals* mesh);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CompareStreams(CMeshInternals* handle, CMeshInternals* mesh);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string AppendData(CMeshInternals* handle, CMeshInternals* mesh, bool returnErrorMessage);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string AppendSpecificData(CMeshInternals* handle, CMeshInternals* mesh, int fromVertex,
														int vertexCount, int fromFace, int faceCount,
														bool returnErrorMessage);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveRangeFromStreamInternal(CMeshInternals* handle, GeneralMeshStreamId stream,
																  int nFirst, int nCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ValidateInternal(CMeshInternals* handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ValidateMessage(CMeshInternals* handle, out string ppErrorDescription);
		#endregion
	}
}