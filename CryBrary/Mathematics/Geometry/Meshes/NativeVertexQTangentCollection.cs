using System;
using CryEngine.Native;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Represents a collection of compressed tangent space normals.
	/// </summary>
	public sealed class NativeVertexQTangentCollection : NativeMeshDetailsCollection<IQTangent>
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets element at specified index.
		/// </summary>
		/// <param name="index"> Zero-based index of the element to access. </param>
		public override IQTangent this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access tangent space normal through" +
													   " index that is out of bounds of the collection.");
				}
				return CommonQTangentOperations.FromNativeMemory
					(this.CollectionHandle, index * CommonQTangentOperations.ByteCount);
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access tangent space normal through" +
													   " index that is out of bounds of the collection.");
				}
				CommonQTangentOperations.ToNativeMemory
					(this.CollectionHandle, index * CommonQTangentOperations.ByteCount, value);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeVertexQTangentCollection" />.
		/// </summary>
		/// <param name="mesh"> Mesh that hosts this collection. </param>
		public NativeVertexQTangentCollection(NativeMesh mesh)
		{
			this.MeshHandle = mesh.CMeshHandle;
			mesh.VerticesReallocated += this.mesh_VerticesReallocated;
			this.UpdateCollection();
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities

		private void mesh_VerticesReallocated(object sender, VertexCollectionEventArgs e)
		{
			MeshInterop.ReallocateStream(this.MeshHandle, NativeMeshMemoryRegion.Qtangents, e.NewCount);
			this.UpdateCollection();
		}
		#endregion
		/// <summary>
		/// <see cref="NativeMeshMemoryRegion.Qtangents" />
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.Qtangents; }
		}
		/// <summary>
		/// False.
		/// </summary>
		public override bool Reallocatable
		{
			get { return false; }
		}
	}
}