using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Provides access to mesh data that is specific to vertexes.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct CryVertexCollection
	{
		#region Fields
		[FieldOffset(0)] private readonly CryMesh mesh;
		[FieldOffset(0)] private readonly CMeshInternals* meshHandle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the number of elements in this collection. Setting this to negative value or 0
		/// deallocates the data.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return MathHelpers.Max(this.meshHandle->streamSize[(int)GeneralMeshStreamId.Positions],
									   this.meshHandle->streamSize[(int)GeneralMeshStreamId.PositionsF16],
									   this.meshHandle->streamSize[(int)GeneralMeshStreamId.P3Sc4Bt2S]);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				value = value < 0 ? 0 : value; // Clamp negative values.

				if (this.Count == value)
				{
					return;
				}

				CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.Positions, value);
				CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.PositionsF16, 0);
				CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.Normals, value);

				if (this.meshHandle->pColor0 != null)
				{
					CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.Colors0, value);
				}

				if (this.meshHandle->pColor1 != null)
				{
					CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.Colors1, value);
				}

				if (this.meshHandle->pVertMats != null)
				{
					CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.VertexMaterials, value);
				}
			}
		}
		/// <summary>
		/// Gets the collection of positions of vertices in 3D space. Returned object will be invalidated,
		/// when <see cref="Count"/> is changed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public VertexPositionCollection Positions
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				int count;
				Vector3* ptr = (Vector3*)CryMesh.GetStreamPtr(this.meshHandle, GeneralMeshStreamId.Positions,
																	 out count);
				return new VertexPositionCollection(ptr, count);
			}
		}
		/// <summary>
		/// Gets the collection of normals of vertices. Returned object will be invalidated, when
		/// <see cref="Count"/> is changed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public VertexNormalCollection Normals
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				int count;
				CryMeshNormal* ptr = (CryMeshNormal*)CryMesh.GetStreamPtr(this.meshHandle,
																				 GeneralMeshStreamId.Normals, out count);
				return new VertexNormalCollection(ptr, count);
			}
		}
		/// <summary>
		/// Gets the collection of indices of materials of vertices. Returned object will be invalidated,
		/// when <see cref="Count"/> is changed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public VertexMaterialCollection MaterialIds
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				int count;
				int* ptr = (int*)CryMesh.GetStreamPtr(this.meshHandle, GeneralMeshStreamId.VertexMaterials,
															 out count);
				return new VertexMaterialCollection(ptr, count);
			}
		}
		/// <summary>
		/// Gets the collection of primary colors of vertices. Returned object will be invalidated, when
		/// <see cref="Count"/> is changed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public VertexColorCollection PrimaryColors
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				int count;
				CryMeshColor* ptr = (CryMeshColor*)CryMesh.GetStreamPtr(this.meshHandle,
																			   GeneralMeshStreamId.Colors0, out count);
				return new VertexColorCollection(ptr, count);
			}
		}
		/// <summary>
		/// Gets the collection of secondary colors of vertices. Returned object will be invalidated, when
		/// <see cref="Count"/> is changed.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public VertexColorCollection SecondaryColors
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				int count;
				CryMeshColor* ptr = (CryMeshColor*)CryMesh.GetStreamPtr(this.meshHandle,
																			   GeneralMeshStreamId.Colors1, out count);
				return new VertexColorCollection(ptr, count);
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Makes internal collection of primary colors of vertices accessible. If this method is not
		/// called on new mesh, <see cref="PrimaryColors"/> will return invalid object. This operation
		/// cannot be done properly, if total number of vertices is 0.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnablePrimaryColors()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			int vertexCount = this.Count;
			if (vertexCount != 0)
			{
				CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.Colors0, vertexCount);
			}
		}
		/// <summary>
		/// Makes internal collection of secondary colors of vertices accessible. If this method is not
		/// called on new mesh, <see cref="SecondaryColors"/> will return invalid object. This operation
		/// cannot be done properly, if total number of vertices is 0.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableSecondaryColors()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			int vertexCount = this.Count;
			if (vertexCount != 0)
			{
				CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.Colors1, vertexCount);
			}
		}
		/// <summary>
		/// Makes internal collection of indices of materials of vertices accessible. If this method is not
		/// called on new mesh, <see cref="MaterialIds"/> will return invalid object. This operation cannot
		/// be done properly, if total number of vertices is 0.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableMaterialIds()
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			int vertexCount = this.Count;
			if (vertexCount != 0)
			{
				CryMesh.ReallocateStream(this.meshHandle, GeneralMeshStreamId.VertexMaterials, vertexCount);
			}
		}
		#endregion
		#region Construction
		internal CryVertexCollection(CryMesh mesh, CMeshInternals* meshHandle)
		{
			this.mesh = mesh;
			this.meshHandle = meshHandle;
		}
		#endregion
		#region Utilities
		private void AssertInstance()
		{
			if (!this.mesh.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}
		#endregion
	}
}