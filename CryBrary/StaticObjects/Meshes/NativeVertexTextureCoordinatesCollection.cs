using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CryEngine.Mathematics.MemoryMapping;
using CryEngine.Native;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Represents a collection of fixed size.
	/// </summary>
	public sealed class NativeVertexTextureCoordinatesCollection : NativeMeshDetailsCollection<Vector2>
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// <see cref="NativeMeshMemoryRegion.TextureCoordinates" />
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.TextureCoordinates; }
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
		public override Vector2 this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access element of the collection" +
													   " through index that is out of bounds.");
				}
				return (Vector2)Marshal.PtrToStructure
					(this.CollectionHandle + index * Vector2.ByteCount, typeof(Vector2));
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access element of the collection" +
													   " through index that is out of bounds.");
				}
				Marshal.StructureToPtr(value, this.CollectionHandle + index * Vector2.ByteCount, false);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeVertexTextureCoordinatesCollection" />.
		/// </summary>
		/// <param name="mesh">Mesh that hosts this collection.</param>
		public NativeVertexTextureCoordinatesCollection(NativeMesh mesh)
		{
			this.MeshHandle = mesh.CMeshHandle;
			mesh.VerticesReallocated += mesh_VerticesReallocated;
			this.UpdateCollection();
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities

		private void mesh_VerticesReallocated(object sender, VertexCollectionEventArgs e)
		{
			NativeMeshMethods.ReallocateStream
				(this.MeshHandle, NativeMeshMemoryRegion.TextureCoordinates, e.NewCount);
			this.UpdateCollection();
		}
		#endregion
	}
}