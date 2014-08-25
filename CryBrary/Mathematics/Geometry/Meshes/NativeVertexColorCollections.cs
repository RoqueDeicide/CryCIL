using System;
using System.Runtime.InteropServices;
using CryEngine.Mathematics.MemoryMapping;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Base for collections of vertex colors located in native memory.
	/// </summary>
	public abstract class NativeVertexColorCollection : NativeMeshDetailsCollection<Color32>
	{
		#region Fields
		protected NativeMesh Mesh;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates that this collection cannot be reallocated on its own as it depends on
		/// collection of vertices.
		/// </summary>
		public override bool Reallocatable
		{
			get { return false; }
		}
		/// <summary>
		/// Gets or sets color instance at specified index.
		/// </summary>
		/// <param name="index"> Index of the color to access. </param>
		public override Color32 this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", "Requested color is out of bounds.");
				}
				return new Color32 { Bytes = new Bytes4(Marshal.ReadInt32(this.CollectionHandle, index)) };
			}
			set
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", "Requested color is out of bounds.");
				}
				Marshal.WriteInt32(this.CollectionHandle, index, value.Bytes.SignedInt);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="NativeVertexColor0Collection" />.
		/// </summary>
		/// <param name="mesh"> Mesh that hosts this collection. </param>
		protected NativeVertexColorCollection(NativeMesh mesh)
		{
			this.Mesh = mesh;
			this.MeshHandle = mesh.CMeshHandle;
			this.Mesh.VerticesReallocated += this.mesh_VerticesReallocated;
			// ReSharper disable DoNotCallOverridableMethodsInConstructor
			this.UpdateCollection();
			// ReSharper restore DoNotCallOverridableMethodsInConstructor
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities

		// ReSharper disable UnusedParameter.Local
		private void mesh_VerticesReallocated(object sender, VertexCollectionEventArgs e)
		// ReSharper restore UnusedParameter.Local
		{
			this.UpdateCollection();
		}
		#endregion
	}
	/// <summary>
	/// Represents a collection of primary colors located in native memory.
	/// </summary>
	public class NativeVertexColor0Collection : NativeVertexColorCollection
	{
		/// <summary>
		/// Creates new instance of class <see cref="NativeVertexColor0Collection" />.
		/// </summary>
		/// <param name="mesh"> <see cref="NativeMesh" /> that hosts this collection. </param>
		public NativeVertexColor0Collection(NativeMesh mesh)
			: base(mesh)
		{
		}
		/// <summary>
		/// Gets memory stream identifier.
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.Colors0; }
		}
	}
	/// <summary>
	/// Represents a collection of secondary colors located in native memory.
	/// </summary>
	public class NativeVertexColor1Collection : NativeVertexColorCollection
	{
		/// <summary>
		/// Creates new instance of class <see cref="NativeVertexColor0Collection" />.
		/// </summary>
		/// <param name="mesh"> <see cref="NativeMesh" /> that hosts this collection. </param>
		public NativeVertexColor1Collection(NativeMesh mesh)
			: base(mesh)
		{
		}
		/// <summary>
		/// Gets memory stream identifier.
		/// </summary>
		public override NativeMeshMemoryRegion MemoryRegionIdentifier
		{
			get { return NativeMeshMemoryRegion.Colors1; }
		}
	}
}