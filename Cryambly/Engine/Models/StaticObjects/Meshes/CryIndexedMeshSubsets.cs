using System;
using System.Linq;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a collection of mesh subsets within <see cref="CryIndexedMesh"/>.
	/// </summary>
	public unsafe struct CryIndexedMeshSubsets
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
		/// Gets or sets the number of subsets in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return CryIndexedMesh.GetSubSetCount(this.handle);
			}
			set
			{
				this.AssertInstance();

				CryIndexedMesh.SetSubSetCount(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the pointer to the object that describes a subset of indexed mesh.
		/// </summary>
		/// <param name="index">Zero-based index of the subset to get.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryMeshSubset* this[int index]
		{
			get
			{
				this.AssertInstance();

				return CryIndexedMesh.GetSubSet(this.handle, index);
			}
		}
		#endregion
		#region Construction
		internal CryIndexedMeshSubsets(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Sets the bounds of the mesh subset.
		/// </summary>
		/// <param name="index"> Zero-based index of the subset to set the bounds for.</param>
		/// <param name="center">Coordintes of the center of the sphere that encompasses the subset.</param>
		/// <param name="radius">Radius of the sphere that encompasses the subset.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSubsetBounds(int index, ref Vector3 center, float radius)
		{
			this.AssertInstance();

			CryIndexedMesh.SetSubsetBounds(this.handle, index, ref center, radius);
		}
		/// <summary>
		/// Sets the ranges of indices and vertices for the mesh subset.
		/// </summary>
		/// <param name="index">      Zero-based index of the subset to set the ranges for.</param>
		/// <param name="firstIndex"> Zero-based index of the first index in range.</param>
		/// <param name="indexCount"> Number of indices in range.</param>
		/// <param name="firstVertex">Zero-based index of the first vertex in range.</param>
		/// <param name="vertexCount">Number of vertices in range.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSubsetIndexVertexRanges(int index, int firstIndex, int indexCount, int firstVertex,
											   int vertexCount)
		{
			this.AssertInstance();

			CryIndexedMesh.SetSubsetIndexVertexRanges(this.handle, index, firstIndex, indexCount, firstVertex,
													  vertexCount);
		}
		/// <summary>
		/// Assigns the identifier of the material to the subset.
		/// </summary>
		/// <param name="index">     Zero-based index of the subset to set the material for.</param>
		/// <param name="materialId">Index of the sub-material within main material.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSubsetMaterialId(int index, int materialId)
		{
			this.AssertInstance();

			CryIndexedMesh.SetSubsetMaterialId(this.handle, index, materialId);
		}
		/// <summary>
		/// Sets material properties for the mesh subset.
		/// </summary>
		/// <param name="index">          
		/// Zero-based index of the subset to set the material properties for.
		/// </param>
		/// <param name="materialFlags">  Flags to assign to the material.</param>
		/// <param name="physicalizeType">Physicalization type to assign to the subset.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSubsetMaterialProperties(int index, MaterialFlags materialFlags,
												PhysicalGeometryType physicalizeType)
		{
			this.AssertInstance();

			CryIndexedMesh.SetSubsetMaterialProperties(this.handle, index, materialFlags, physicalizeType);
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
		#endregion
	}
}