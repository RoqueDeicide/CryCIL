using System;
using System.Runtime.InteropServices;
using CryEngine.Mathematics.MemoryMapping;
using CryEngine.Native;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Represents a collection of indices that form faces of native mesh.
	/// </summary>
	public unsafe sealed class NativeIndicesCollection : NativeMeshDetailsCollection<uint>
	{
		#region Fields
		private readonly NativeMesh mesh;
		#endregion
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
			get { return NativeMeshMemoryRegion.Indices; }
		}
		/// <summary>
		/// Gets or sets element at specified index.
		/// </summary>
		/// <param name="index">Zero-based index of the element to access.</param>
		/// <exception cref="IndexOutOfRangeException">
		/// Attempt to access element of the collection throw index that is out of bounds.
		/// </exception>
		/// <exception cref="MeshConsistencyException">
		/// Attempt to set face vertex index to value that is out of bounds of the collection of vertices.
		/// </exception>
		/// <exception cref="MeshConsistencyException">
		/// Attempt to set face vertex index to number bigger then biggest value that can be stored by
		/// integer type that is used to store indices.
		/// </exception>
		public override uint this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access element of the collection throw index that is out of bounds.");
				}
				return
					Platform.MeshIndexIs16Bit ?
					new Bytes2(Marshal.ReadInt16(this.CollectionHandle, index * 2)).UnsignedShort
					:
					new Bytes4(Marshal.ReadInt32(this.CollectionHandle, index * 4)).UnsignedInt;
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException("Attempt to access element of the collection throw index that is out of bounds.");
				}
				if (value > this.mesh.Positions.Count)
				{
					throw new MeshConsistencyException
						("Attempt to set face vertex index to value that is out of bounds" +
						 " of the collection of vertices.");
				}
				if (Platform.MeshIndexIs16Bit)
				{
					if (value > UInt16.MaxValue)
					{
						throw new MeshConsistencyException
							("Attempt to set face vertex index to number bigger" +
							 " then biggest value that can be stored by integer" +
							 " type that is used to store indices.");
					}
					Marshal.WriteInt16(this.CollectionHandle, index * 2, new Bytes2((ushort)value).SignedShort);
				}
				else
				{
					Marshal.WriteInt32(this.CollectionHandle, index * 4, *((int*)(&value)));
				}
			}
		}
		/// <summary>
		/// Gets or sets number of elements in the collection.
		/// </summary>
		/// <exception cref="MeshConsistencyException">
		/// Attempt to set the number of indices to one that is not divisible by 3.
		/// </exception>
		/// <exception cref="MeshConsistencyException">
		/// Attempt to set the number of indices to non positive number.
		/// </exception>
		public override int Capacity
		{
			get { return this.Count; }
			set
			{
				if (value <= 0)
				{
					throw new MeshConsistencyException
						("Attempt to set the number of indices to non positive number.");
				}
				if (value % 3 != 0)
				{
					throw new MeshConsistencyException
						("Attempt to set the number of indices to one that is not divisible by 3.");
				}
				MeshInterop.ReallocateStream(this.MeshHandle, NativeMeshMemoryRegion.Indices, value);
				this.UpdateCollection();
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeIndicesCollection"/>.
		/// </summary>
		/// <param name="mesh">Mesh that hosts this collection.</param>
		public NativeIndicesCollection(NativeMesh mesh)
		{
			this.mesh = mesh;
			this.MeshHandle = mesh.CMeshHandle;
			this.UpdateCollection();
		}
		#endregion
		#region Interface
		#endregion
	}
}