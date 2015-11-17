using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Utilities;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about neighbors of each edge of the triangle.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TriangleTopologyInfo
	{
		private readonly Vector3Int32 buddies;
		/// <summary>
		/// Gets the vector that contains 3 indexes of triangles that are neighbors of this one.
		/// </summary>
		public Vector3Int32 NeighborIndexes
		{
			get { return this.buddies; }
		}
	}
	/// <summary>
	/// Represents an array of objects that encapsulate information about neighbors of each edge of the
	/// triangle.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct TriangleTopologyInfos
	{
		#region Fields
		[UsedImplicitly] private TriangleTopologyInfo* pTopology;
		[UsedImplicitly] private int nTris;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that encapsulates information about neighbors of each edge of the triangle.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public TriangleTopologyInfo this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nTris)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pTopology[index];
			}
		}
		/// <summary>
		/// Gets the number of triangles.
		/// </summary>
		public int Count
		{
			get { return this.nTris; }
		}
		#endregion
		#region Construction
		internal TriangleTopologyInfos(TriangleTopologyInfo* pTopology, int nTris)
		{
			this.pTopology = pTopology;
			this.nTris = nTris;
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates information about a group of connected triangles.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MeshIsland
	{
		#region Fields
		[UsedImplicitly] private int itri;
		[UsedImplicitly] private int nTris;
		[UsedImplicitly] private int iParent;
		[UsedImplicitly] private int iChild;
		[UsedImplicitly] private int iNext;
		[UsedImplicitly] private float V;
		[UsedImplicitly] private Vector3 center;
		[UsedImplicitly] private int bProcessed;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the geometrical center of this island.
		/// </summary>
		public Vector3 Center
		{
			get { return this.center; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of objects that encapsulate information about neighbors of each edge of the
	/// triangle.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MeshIslands
	{
		#region Fields
		[UsedImplicitly] private MeshIsland* pIslands;
		[UsedImplicitly] private int nIslands;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents mesh island.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public MeshIsland this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nIslands)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pIslands[index];
			}
		}
		/// <summary>
		/// Gets the number of islands.
		/// </summary>
		public int Count
		{
			get { return this.nIslands; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of indexes of vertices that form triangles that form the mesh.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct TriangleIndexes
	{
		#region Fields
		private readonly int* pIndices;
		private readonly int indexCount;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the index of one of the vertexes that forms one of the triangles.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public int this[int index]
		{
			get
			{
				if (index < 0 || index >= this.indexCount)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pIndices[index];
			}
		}
		/// <summary>
		/// Gets the index of one of the vertexes that forms one of the triangles.
		/// </summary>
		/// <param name="triangleIndex">Zero-based index of the triangle which vertex index to get.</param>
		/// <param name="vertexIndex">  Zero-based index of the vertex of a triangle to get.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Index of the triangle was out of range.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">Index of the vertex was out of range.</exception>
		public int this[int triangleIndex, int vertexIndex]
		{
			get
			{
				if (triangleIndex < 0 || triangleIndex >= this.indexCount / 3)
				{
					throw new ArgumentOutOfRangeException("triangleIndex");
				}
				if (vertexIndex < 0 || vertexIndex > 2)
				{
					throw new ArgumentOutOfRangeException("vertexIndex");
				}

				return this.pIndices[triangleIndex * 3 + vertexIndex];
			}
		}
		/// <summary>
		/// Gets the number of indexes.
		/// </summary>
		public int Count
		{
			get { return this.indexCount; }
		}
		#endregion
		#region Construction
		internal TriangleIndexes(int* indexes, int indexCount)
		{
			this.pIndices = indexes;
			this.indexCount = indexCount;
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of indexes of materials from material mapping that are assigned to
	/// corresponding triangles.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct TriangleMaterials
	{
		#region Fields
		private readonly byte* pMats;
		private readonly int triangleCount;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the index of the material that is assigned to the triangle with this index.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public int this[int index]
		{
			get
			{
				if (index < 0 || index >= this.triangleCount)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pMats[index];
			}
		}
		/// <summary>
		/// Gets the number of triangles.
		/// </summary>
		public int Count
		{
			get { return this.triangleCount; }
		}
		#endregion
		#region Construction
		internal TriangleMaterials(byte* pMats, int triangleCount)
		{
			this.pMats = pMats;
			this.triangleCount = triangleCount;
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of vertices that form triangles that form the mesh.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MeshVertexes
	{
		#region Fields
		private readonly StridedPointer vertices;
		private readonly int vertexCount;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the coordinates of the vertex.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		/// <exception cref="NullReferenceException">
		/// Attempted to dereference null strided pointer.
		/// </exception>
		public Vector3 this[int index]
		{
			get
			{
				if (index < 0 || index >= this.vertexCount)
				{
					throw new IndexOutOfRangeException();
				}

				return this.vertices.GetVector3(index);
			}
		}
		/// <summary>
		/// Gets the number of vertexes.
		/// </summary>
		public int Count
		{
			get { return this.vertexCount; }
		}
		#endregion
		#region Construction
		internal MeshVertexes(StridedPointer vertices, int vertexCount)
		{
			this.vertices = vertices;
			this.vertexCount = vertexCount;
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of vectors that represents normals to each vertex in the mesh.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MeshVertexNormals
	{
		#region Fields
		private readonly Vector3* normals;
		private readonly int normalCount;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the normal to the respective vertex.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public Vector3 this[int index]
		{
			get
			{
				if (index < 0 || index >= this.normalCount)
				{
					throw new IndexOutOfRangeException();
				}

				return this.normals[index];
			}
		}
		/// <summary>
		/// Gets the number of normals.
		/// </summary>
		public int Count
		{
			get { return this.normalCount; }
		}
		#endregion
		#region Construction
		internal MeshVertexNormals(Vector3* normals, int normalCount)
		{
			this.normals = normals;
			this.normalCount = normalCount;
		}
		#endregion
	}
	/// <summary>
	/// Represents an array that maps indexes of original vertices to new vertices after merging the
	/// vertexes.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct VertexMap
	{
		#region Fields
		private readonly int* vertexes;
		private readonly int vertexCount;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the new index of the respective vertex.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public int this[int index]
		{
			get
			{
				if (index < 0 || index >= this.vertexCount)
				{
					throw new IndexOutOfRangeException();
				}

				return this.vertexes[index];
			}
		}
		/// <summary>
		/// Gets the number of vertexes.
		/// </summary>
		public int Count
		{
			get { return this.vertexCount; }
		}
		#endregion
		#region Construction
		internal VertexMap(int* vertexes, int vertexCount)
		{
			this.vertexes = vertexes;
			this.vertexCount = vertexCount;
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates the data about a triangular mesh.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MeshData
	{
		#region Fields
		[UsedImplicitly] private int* pIndices;
		[UsedImplicitly] private byte* pMats;
		[UsedImplicitly] private int* pForeignIdx; // an int associated with each face
		[UsedImplicitly] private StridedPointer pVertices;
		[UsedImplicitly] private Vector3* pNormals;
		[UsedImplicitly] private int* pVtxMap;
		[UsedImplicitly] private TriangleTopologyInfo* pTopology;
		[UsedImplicitly] private int nTris, nVertices;
		[UsedImplicitly] private MeshIslands meshIslands;
		[UsedImplicitly] private IntPtr pTri2Island;
		[UsedImplicitly] private PhysicsMeshFlags flags;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the array of indices of vertexes that form triangles that form this mesh.
		/// </summary>
		public TriangleIndexes TriangleIndexes
		{
			get { return new TriangleIndexes(this.pIndices, this.nTris * 3); }
		}
		/// <summary>
		/// Gets the array of mapped indexes of materials for each triangle in this mesh.
		/// </summary>
		public TriangleMaterials TriangleMaterials
		{
			get { return new TriangleMaterials(this.pMats, this.nTris); }
		}
		/// <summary>
		/// Gets the array of vertexes that form this mesh.
		/// </summary>
		public MeshVertexes Vertexes
		{
			get { return new MeshVertexes(this.pVertices, this.nVertices); }
		}
		/// <summary>
		/// Gets the array of normals for vertexes that form this mesh.
		/// </summary>
		public MeshVertexNormals Normals
		{
			get { return new MeshVertexNormals(this.pNormals, this.nVertices); }
		}
		/// <summary>
		/// Gets the object that maps original vertex indices to merged vertex indices.
		/// </summary>
		public VertexMap VertexMap
		{
			get { return new VertexMap(this.pVtxMap, this.nVertices); }
		}
		/// <summary>
		/// Gets the array of objects that provide indexes of triangles that are neighbors for the triangle
		/// with respective index.
		/// </summary>
		public TriangleTopologyInfos Topology
		{
			get { return new TriangleTopologyInfos(this.pTopology, this.nTris); }
		}
		/// <summary>
		/// Gets the array of groups of connected triangles.
		/// </summary>
		public MeshIslands MeshIslands
		{
			get { return this.meshIslands; }
		}
		#endregion
	}
}