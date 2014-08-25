using System;
using CryEngine.Native;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Represents a collection of tangent space normals located in native memory.
	/// </summary>
	public sealed class NativeVertexTangentCollection : NativeMeshDetailsCollection<ITangent>
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets element at specified index.
		/// </summary>
		/// <param name="index"> Zero-based index of the element to access. </param>
		public override ITangent this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access tangent space normal through" +
													   " index that is out of bounds of the collection.");
				}
				return CommonTangentOperations.FromNativeMemory
					(this.CollectionHandle, index * CommonTangentOperations.ByteCount);
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access tangent space normal through" +
													   " index that is out of bounds of the collection.");
				}
				CommonTangentOperations.ToNativeMemory
					(this.CollectionHandle, index * CommonTangentOperations.ByteCount, value);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeVertexQTangentCollection" />.
		/// </summary>
		/// <param name="mesh"> Mesh that hosts this collection. </param>
		public NativeVertexTangentCollection(NativeMesh mesh)
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
			NativeMeshMethods.ReallocateStream(this.MeshHandle, NativeMeshMemoryRegion.Qtangents, e.NewCount);
			this.UpdateCollection();
		}
		#endregion
		/// <summary>
		/// <see cref="NativeMeshMemoryRegion.Tangents" />
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.Tangents; }
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