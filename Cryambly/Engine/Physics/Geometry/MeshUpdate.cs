using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Physics.Primitives;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates changes that were made to the vertex.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexChange
	{
		#region Fields
		[UsedImplicitly] private int idx;
		[UsedImplicitly] private int iBvtx;
		[UsedImplicitly] private int idxTri0; // intersecting triangles' foreign indices
		[UsedImplicitly] private int idxTri1; // intersecting triangles' foreign indices
		#endregion
		#region Properties
		/// <summary>
		/// Gets the zero-based index of this vertex in resultant mesh.
		/// </summary>
		public int ResultantIndex
		{
			get { return this.idx; }
		}
		/// <summary>
		/// Gets the zero-based index of this vertex in original mesh, if it existed in it.
		/// </summary>
		/// <returns>- 1, if this vertex didn't exist in original mesh.</returns>
		public int OriginalIndex
		{
			get { return this.iBvtx; }
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates changes that were made to the triangle.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TriangleChange
	{
		#region Fields
		[UsedImplicitly] private int idxNew;
		[UsedImplicitly] private int iop;
		[UsedImplicitly] private int idxOrg;
		[UsedImplicitly] private Vector3Int32 iVtx;
		[UsedImplicitly] private float areaOrg;
		[UsedImplicitly] private Vector3 area0;
		[UsedImplicitly] private Vector3 area1;
		[UsedImplicitly] private Vector3 area2;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the zero-based index of this triangle in resultant mesh.
		/// </summary>
		public int ResultantIndex
		{
			get { return this.idxNew; }
		}
		/// <summary>
		/// Gets the zero-based index of this triangle in original mesh.
		/// </summary>
		public int OriginalIndex
		{
			get { return this.idxOrg; }
		}
		/// <summary>
		/// Gets the value that indicates whether this triangle was present in original mesh.
		/// </summary>
		public bool OriginalTriangle
		{
			get { return this.iop == 1; }
		}
		/// <summary>
		/// Gets a vector that contains 3 values that can be used to acquire the indices of this triangle's
		/// vertices. See Remarks section for details.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If the value is greater then or equal to 0 then it represents the zero-based index of the
		/// vertex in resultant mesh.
		/// </para>
		/// <para>
		/// If the value is less then 0 then the index of the vertex must be acquired through the following
		/// code:
		/// </para>
		/// <code>
		/// int GetTriangleVertexIndex(MeshUpdate mu, int triangleIndex)
		/// {
		///     return mu.VertexChanges[-triangleIndex - 1].ResultantIndex;
		/// }
		/// </code>
		/// </remarks>
		public Vector3Int32 VertexIndexes
		{
			get { return this.iVtx; }
		}
		/// <summary>
		/// Gets the original area of the triangle.
		/// </summary>
		public float OriginalArea
		{
			get { return this.areaOrg; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the barycentric coordinates for each vertex of this triangle.
		/// </summary>
		/// <param name="first"> Barycentric coordinates for the first vertex.</param>
		/// <param name="second">Barycentric coordinates for the second vertex.</param>
		/// <param name="third"> Barycentric coordinates for the third vertex.</param>
		public void GetBarycentricCoordinates(out Vector3 first, out Vector3 second, out Vector3 third)
		{
			first = this.area0 / this.areaOrg;
			second = this.area1 / this.areaOrg;
			third = this.area2 / this.areaOrg;
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates information about a fix that was issued to resolve a problem with degenerate triangle.
	/// See Remarks for details.
	/// </summary>
	/// <remarks>
	/// The problem with degenerate triangle that is resolved with this fix:
	/// <code>
	/// A __J___ C   (ACJ is a thin triangle on top of ABC; J is 'the junction vertex')
	///   \ |  /		 in ABC: set A-&gt;Jnew
	///    \| /			 in ACJ: set J-&gt;Jnew, A -&gt; A from original ABC, C -&gt; B from original ABC
	///     \/
	///      B
	/// </code>
	/// In the above picture ACJ is a thin triangle on top of ABC and J is the 'junction vertex'. In order
	/// to fix that problem, J in ACJ is replaced with brand new vertex, A in ACJ is replaced with A from
	/// ABC, C in ACJ is replaced with B from ABC and A from ABC is replaced with new J vertex.
	/// </remarks>
	public struct JunctionVertexFix
	{
		#region Fields
		[UsedImplicitly] private int iABC;
		[UsedImplicitly] private int iACJ;
		[UsedImplicitly] private int iCA;
		[UsedImplicitly] private int iAC;
		[UsedImplicitly] private int iTJvtx;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the zero-based index of the ABC triangle that is mentioned in Remarks section for
		/// <see cref="JunctionVertexFix"/>.
		/// </summary>
		public int BigTriangleIndex
		{
			get { return this.iABC; }
		}
		/// <summary>
		/// Gets the zero-based index of the ACJ triangle that is mentioned in Remarks section for
		/// <see cref="JunctionVertexFix"/>.
		/// </summary>
		public int SmallTriangleIndex
		{
			get { return this.iACJ; }
		}
		/// <summary>
		/// Gets the zero-based index of the CA edge in ABC triangle that is mentioned in Remarks section
		/// for <see cref="JunctionVertexFix"/>.
		/// </summary>
		public int BigTriangleEdgeIndex
		{
			get { return this.iCA; }
		}
		/// <summary>
		/// Gets the zero-based index of the AC edge in ACJ triangle that is mentioned in Remarks section
		/// for <see cref="JunctionVertexFix"/>.
		/// </summary>
		public int SmallTriangleEdgeIndex
		{
			get { return this.iAC; }
		}
		/// <summary>
		/// Gets the zero-based index of the junction vertex that is mentioned in Remarks section for
		/// <see cref="JunctionVertexFix"/>.
		/// </summary>
		public int JunctionVertexIndex
		{
			get { return this.iTJvtx; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of changes that were made to the vertexes of the geometry.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct VertexChanges
	{
		#region Fields
		[UsedImplicitly] private VertexChange* pNewVtx;
		[UsedImplicitly] private int nNewVtx;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents a change to one of the vertexes.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		public VertexChange this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nNewVtx)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pNewVtx[index];
			}
		}
		/// <summary>
		/// Gets the number of changed vertexes.
		/// </summary>
		public int Count
		{
			get { return this.nNewVtx; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of changes that were made to the triangles that comprise the mesh.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct TriangleChanges
	{
		#region Fields
		[UsedImplicitly] private TriangleChange* pNewTri;
		[UsedImplicitly] private int nNewTri;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents a change to one of the triangles.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		public TriangleChange this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nNewTri)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pNewTri[index];
			}
		}
		/// <summary>
		/// Gets the number of changed triangles.
		/// </summary>
		public int Count
		{
			get { return this.nNewTri; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of changes that were made to fix the problem that is described in Remarks
	/// section of <see cref="JunctionVertexFix"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct JunctionVertexFixes
	{
		#region Fields
		[UsedImplicitly] private JunctionVertexFix* pTJFixes;
		[UsedImplicitly] private int nTJFixes;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents a fix.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		public JunctionVertexFix this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nTJFixes)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pTJFixes[index];
			}
		}
		/// <summary>
		/// Gets the number of fixes.
		/// </summary>
		public int Count
		{
			get { return this.nTJFixes; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of new positions of bounding volume boxes.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MovedBoxes
	{
		#region Fields
		[UsedImplicitly] private Primitive.Box* pMovedBoxes;
		[UsedImplicitly] private int nMovedBoxes;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents a moved bounding volume box.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		public Primitive.Box this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nMovedBoxes)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pMovedBoxes[index];
			}
		}
		/// <summary>
		/// Gets the number of moved boxes.
		/// </summary>
		public int Count
		{
			get { return this.nMovedBoxes; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of indexes of vertexes that were present in original geometry and were removed
	/// in the new one.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct RemovedVertexes
	{
		#region Fields
		[UsedImplicitly] private int* pRemovedVtx;
		[UsedImplicitly] private int nRemovedVtx;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents a moved bounding volume box.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		public int this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nRemovedVtx)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pRemovedVtx[index];
			}
		}
		/// <summary>
		/// Gets the number of removed vertexes.
		/// </summary>
		public int Count
		{
			get { return this.nRemovedVtx; }
		}
		#endregion
	}
	/// <summary>
	/// Represents an array of indexes of triangles that were present in original geometry and were removed
	/// in the new one.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct RemovedTriangles
	{
		#region Fields
		[UsedImplicitly] private int* pRemovedTri;
		[UsedImplicitly] private int nRemovedTri;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents a moved bounding volume box.
		/// </summary>
		/// <param name="index">Zero-based index of the object to get.</param>
		public int this[int index]
		{
			get
			{
				if (index < 0 || index >= this.nRemovedTri)
				{
					throw new IndexOutOfRangeException();
				}

				return this.pRemovedTri[index];
			}
		}
		/// <summary>
		/// Gets the number of removed triangles.
		/// </summary>
		public int Count
		{
			get { return this.nRemovedTri; }
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates information about changes that were made to the geometric mesh.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MeshUpdate
	{
		#region Fields
		[UsedImplicitly] private MeshUpdate* prevRef, nextRef;
		[UsedImplicitly] private GeometryShape result;
		[UsedImplicitly] private GeometryShape original;
		[UsedImplicitly] private RemovedVertexes removedVertexes;
		[UsedImplicitly] private RemovedTriangles removedTriangles;
		[UsedImplicitly] private VertexChanges vertexChanges;
		[UsedImplicitly] private TriangleChanges triangleChanges;
		[UsedImplicitly] private IntPtr pWeldedVtx;
		[UsedImplicitly] private int nWeldedVtx;
		[UsedImplicitly] private JunctionVertexFixes fixes;
		[UsedImplicitly] private MeshUpdate* next;
		[UsedImplicitly] private MovedBoxes movedBoxes;
		[UsedImplicitly] private float relScale;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the value that indicates whether this object is the last one in the sequence of updates.
		/// </summary>
		public bool IsLast
		{
			get { return this.next != null; }
		}
		/// <summary>
		/// Gets the next mesh update object in the linked list.
		/// </summary>
		/// <exception cref="NullReferenceException">This object is last in the linked list.</exception>
		public MeshUpdate Next
		{
			get
			{
				if (this.next == null)
				{
					throw new NullReferenceException("This object is last in the linked list.");
				}
				Contract.EndContractBlock();

				return *this.next;
			}
		}
		/// <summary>
		/// Gets the mesh that was created as a result of changes this object represents.
		/// </summary>
		public GeometryShape ResultantMesh
		{
			get { return this.result; }
		}
		/// <summary>
		/// Gets the mesh that changed.
		/// </summary>
		public GeometryShape OriginalMesh
		{
			get { return this.original; }
		}
		/// <summary>
		/// Gets the array of indexes of vertexes that were removed from <see cref="OriginalMesh"/>.
		/// </summary>
		public RemovedVertexes RemovedVertexes
		{
			get { return this.removedVertexes; }
		}
		/// <summary>
		/// Gets the array of indexes of triangles that were removed from <see cref="OriginalMesh"/>.
		/// </summary>
		public RemovedTriangles RemovedTriangles
		{
			get { return this.removedTriangles; }
		}
		/// <summary>
		/// Gets the array of objects that represent changes that were made to vertexes.
		/// </summary>
		public VertexChanges VertexChanges
		{
			get { return this.vertexChanges; }
		}
		/// <summary>
		/// Gets the array of objects that represent changes that were made to triangles.
		/// </summary>
		public TriangleChanges TriangleChanges
		{
			get { return this.triangleChanges; }
		}
		/// <summary>
		/// Gets the array of objects that describe fixes that were applied to solve problems that are
		/// described in Remarks section of <see cref="JunctionVertexFix"/>.
		/// </summary>
		public JunctionVertexFixes JunctionVertexFixes
		{
			get { return this.fixes; }
		}
		/// <summary>
		/// Gets the array of new objects that represent the bounding volume boxes.
		/// </summary>
		public MovedBoxes MovedBoxes
		{
			get { return this.movedBoxes; }
		}
		/// <summary>
		/// Gets the scale of resultant mesh relative to scale of the original mesh.
		/// </summary>
		public float RelativeScale
		{
			get { return this.relScale; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Attempts to acquire the next mesh update object in the delineated part of the linked list of
		/// updates.
		/// </summary>
		/// <param name="next">      
		/// Resultant object that will not be valid, if this object returns <c>false</c>.
		/// </param>
		/// <param name="lastUpdate">
		/// An optional pointer that can be specified to make sure that we don't proceed beyond this
		/// particular update.
		/// </param>
		/// <returns>True, if <paramref name="next"/> was properly assigned and can be used.</returns>
		public bool GetNext(out MeshUpdate next, MeshUpdate* lastUpdate = null)
		{
			next = new MeshUpdate();

			if (this.next == null || (lastUpdate != null && this.next == lastUpdate->next))
			{
				return false;
			}

			next = *this.next;
			return true;
		}
		#endregion
	}
}