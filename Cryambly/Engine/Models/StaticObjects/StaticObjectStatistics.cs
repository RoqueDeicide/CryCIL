using System;
using CryCil.Annotations;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates various statistics of the static object.
	/// </summary>
	public unsafe struct StaticObjectStatistics
	{
		#region Fields
		private readonly int vertices;
		/// <summary>
		/// Contains number of vertices in each LOD model.
		/// </summary>
#pragma warning disable 649,169
		public fixed int VerticesPerLod[6];
#pragma warning restore 649,169
		private readonly int indices;
		/// <summary>
		/// Contains number of indices in each LOD model.
		/// </summary>
#pragma warning disable 649,169
		public fixed int IndicesPerLod[6];
#pragma warning restore 649,169
		private readonly int meshSize;
		private readonly int meshSizeLoaded;
		private readonly int physProxySize;
		private readonly int physProxySizeMax;
		private readonly int physPrimitives;
		private readonly int drawCalls;
		private readonly int lods;
		private readonly int subMeshCount;
		private readonly int numRefs;
		private readonly bool splitLods;
#pragma warning disable 649,169
		[UsedImplicitly] private IntPtr pTextureSizer;
		[UsedImplicitly] private IntPtr pTextureSizer2;
#pragma warning restore 649,169
		#endregion
		#region Properties
		/// <summary>
		/// Gets total number of vertices in this static object.
		/// </summary>
		public int Vertices
		{
			get { return this.vertices; }
		}
		/// <summary>
		/// Gets total number of indices that form polygons in this static object.
		/// </summary>
		public int Indices
		{
			get { return this.indices; }
		}
		/// <summary>
		/// Gets the size of mesh data in bytes(?).
		/// </summary>
		public int MeshSize
		{
			get { return this.meshSize; }
		}
		/// <summary>
		/// Gets the size of mesh data that was loaded from .cgf file in bytes(?).
		/// </summary>
		public int MeshSizeLoaded
		{
			get { return this.meshSizeLoaded; }
		}
		/// <summary>
		/// Gets the size of physical proxy data in bytes(?).
		/// </summary>
		public int PhysicalProxySize
		{
			get { return this.physProxySize; }
		}
		/// <summary>
		/// Gets the maximal size of physical proxy data in bytes(?).
		/// </summary>
		public int PhysicalProxySizeMax
		{
			get { return this.physProxySizeMax; }
		}
		/// <summary>
		/// Gets number of physical primitives that comprise this static object.
		/// </summary>
		public int PhysicalPrimitives
		{
			get { return this.physPrimitives; }
		}
		/// <summary>
		/// Gets number of draw calls that are required to render this static object.
		/// </summary>
		public int DrawCalls
		{
			get { return this.drawCalls; }
		}
		/// <summary>
		/// Gets number of LOD models this static object contains.
		/// </summary>
		public int Lods
		{
			get { return this.lods; }
		}
		/// <summary>
		/// Gets number of sub meshes.
		/// </summary>
		public int SubMeshCount
		{
			get { return this.subMeshCount; }
		}
		/// <summary>
		/// Gets reference count.
		/// </summary>
		public int ReferenceCount
		{
			get { return this.numRefs; }
		}
		/// <summary>
		/// Indicates whether LOD models were loaded from separate files.
		/// </summary>
		public bool SplitLods
		{
			get { return this.splitLods; }
		}
		#endregion
		#region Construction
		internal StaticObjectStatistics(int vertices, int indices, int meshSize,
										int meshSizeLoaded, int physProxySize, int physProxySizeMax, int physPrimitives, int drawCalls, int lods,
										int subMeshCount, int numRefs, bool splitLods, IntPtr pTextureSizer, IntPtr pTextureSizer2)
		{
			this.vertices = vertices;
			this.indices = indices;
			this.meshSize = meshSize;
			this.meshSizeLoaded = meshSizeLoaded;
			this.physProxySize = physProxySize;
			this.physProxySizeMax = physProxySizeMax;
			this.physPrimitives = physPrimitives;
			this.drawCalls = drawCalls;
			this.lods = lods;
			this.subMeshCount = subMeshCount;
			this.numRefs = numRefs;
			this.splitLods = splitLods;
			this.pTextureSizer = pTextureSizer;
			this.pTextureSizer2 = pTextureSizer2;
		}
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}