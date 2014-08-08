using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Native;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Represents a collection of fixed size.
	/// </summary>
	public unsafe sealed class NativeVertexNormalCollection : NativeMeshDetailsCollection<Vector3>
	{
		#region Fields
		private readonly NativeMesh mesh;
		#endregion
		#region Properties
		/// <summary>
		/// <see cref="NativeMeshMemoryRegion.Normals" />.
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.Normals; }
		}
		/// <summary>
		/// False.
		/// </summary>
		public override bool Reallocatable
		{
			get { return false; }
		}
		/// <summary>
		/// Gets or sets element at specified index.
		/// </summary>
		/// <param name="index">Zero-based index of the element to access.</param>
		/// <exception cref="IndexOutOfRangeException">
		/// Attempt to access element of the collection throw index that is out of bounds.
		/// </exception>
		public override Vector3 this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access element of the collection" +
													   "through index that is out of bounds.");
				}
				return *(Vector3*)(this.CollectionHandle + index * Vector3.ByteCount);
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access element of the collection" +
													   "through index that is out of bounds.");
				}
				*(Vector3*)(this.CollectionHandle + index * Vector3.ByteCount) = value;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeVertexNormalCollection" />.
		/// </summary>
		/// <param name="mesh">Mesh that hosts this collection.</param>
		public NativeVertexNormalCollection(NativeMesh mesh)
		{
			this.mesh = mesh;
			this.MeshHandle = mesh.CMeshHandle;
			this.UpdateCollection();
			this.mesh.VerticesReallocated += (sender, args) => this.UpdateCollection();
		}
		#endregion
	}
}