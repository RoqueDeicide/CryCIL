using System;
using CryEngine.Native;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Represents a collection of mesh vertices located native memory.
	/// </summary>
	public unsafe sealed class NativeVertexPositionCollection : NativeMeshDetailsCollection<Vector3>
	{
		private readonly NativeMesh mesh;
		/// <summary>
		/// Creates new instance of class <see cref="NativeVertexPositionCollection"/>.
		/// </summary>
		/// <param name="mesh"><see cref="NativeMesh"/> that hosts this collection.</param>
		public NativeVertexPositionCollection(NativeMesh mesh)
		{
			this.mesh = mesh;
			this.MeshHandle = mesh.CMeshHandle;
			this.UpdateCollection();
		}
		/// <summary>
		/// Gets or sets number of vertices that are defined for this mesh.
		/// </summary>
		/// <remarks>Passing 0 will free the vertex data.</remarks>
		public override int Capacity
		{
			get { return this.Count; }
			set
			{
				if (value < 0 || value == this.Count) return;

				MeshInterop.SetVertexCount(this.MeshHandle, value);
				this.mesh.OnVerticesReallocated(this.Count, value);
				this.Count = value;
			}
		}
		/// <summary>
		/// Gets or sets a position of the vertex at specified index.
		/// </summary>
		/// <param name="index">Index of the vertex to access.</param>
		/// <exception cref="IndexOutOfRangeException">
		/// Attempt to access position of the vertex via vertex that is out of bounds of vertex position
		/// collection.
		/// </exception>
		public override Vector3 this[int index]
		{
			get
			{
				if (index < 0 || index > this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access position of the vertex via" +
													   " vertex that is out of bounds of vertex position" +
													   " collection.");
				}
				return *(Vector3*)(this.CollectionHandle + index * Vector3.ByteCount);
			}
			set
			{
				if (index < 0 || index > this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access position of the vertex via" +
													   " vertex that is out of bounds of vertex position" +
													   " collection.");
				}
				*(Vector3*)(this.CollectionHandle + index * Vector3.ByteCount) = value;
			}
		}
		/// <summary>
		/// <see cref="NativeMeshMemoryRegion.Positions"/>
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.Positions; }
		}
		/// <summary>
		/// True.
		/// </summary>
		public override bool Reallocatable
		{
			get { return true; }
		}
	}
}