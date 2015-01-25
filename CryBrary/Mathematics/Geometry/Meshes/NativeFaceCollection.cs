using System;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Represents a collection of faces in native memory attached to specific CMesh
	/// object.
	/// </summary>
	public sealed class NativeFaceCollection : NativeMeshDetailsCollection<IndexedTriangleFace>
	{
		#region Properties
		/// <summary>
		/// True.
		/// </summary>
		public override bool Reallocatable
		{
			get { return true; }
		}
		/// <summary>
		/// Gets identifier of the stream used when getting collection handle.
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.Faces; }
		}
		/// <summary>
		/// Gets or sets a face.
		/// </summary>
		/// <param name="index">Index of the face to access.</param>
		/// <exception cref="IndexOutOfRangeException">
		/// Attempt to access face with index that is out of bounds.
		/// </exception>
		public override IndexedTriangleFace this[int index]
		{
			get
			{
				if (index < 0 || index > this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access face with index that is out of bounds.");
				}
				return IndexedTriangleFace.FromNativeMemory(this.CollectionHandle, index * IndexedTriangleFace.ByteCount);
			}
			set
			{
				if (index < 0 || index > this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access face with index that is out of bounds.");
				}
				value.WriteToNativeMemory(this.CollectionHandle, index * IndexedTriangleFace.ByteCount);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeFaceCollection"/>.
		/// </summary>
		/// <param name="meshHandle">Pointer to CMesh object in native memory.</param>
		public NativeFaceCollection(IntPtr meshHandle)
		{
			this.MeshHandle = meshHandle;
			this.UpdateCollection();
		}
		#endregion
		#region Interface
		#endregion
	}
}