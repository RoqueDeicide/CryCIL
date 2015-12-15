using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Memory;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Geometry.Csg;
using CryCil.Geometry.Csg.Base;

namespace CryCil.Geometry
{
	// ReSharper disable ExceptionNotDocumentedOptional

	internal enum CsgOpCode
	{
		Combine,
		Intersect,
		Subtract
	}
	/// <summary>
	/// Represents a triangular mesh where each face is a triangle with it's own set of vertices.
	/// </summary>
	public unsafe class FaceMesh
	{
		#region Fields
		/// <summary>
		/// Indicates whether CSG operations must be done natively.
		/// </summary>
		public static readonly bool NativeCsg = true;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the list of faces that comprise this mesh.
		/// </summary>
		public List<FullFace> Faces { get; private set; }
		/// <summary>
		/// Creates a BSP tree from polygons that form this mesh.
		/// </summary>
		public BspNode<FullFace> BspTree
		{
			get { return new BspNode<FullFace>(this.Faces, null); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates empty mesh.
		/// </summary>
		public FaceMesh()
		{
			this.Faces = new List<FullFace>();
		}
		/// <summary>
		/// Creates a new face mesh from native CryEngine mesh.
		/// </summary>
		/// <param name="cryMesh">CryEngine mesh to create this one from.</param>
		public FaceMesh(CryMesh cryMesh)
		{
			this.Faces = new List<FullFace>();

			if (!cryMesh.IsValid)
			{
				return;
			}

			if (cryMesh.Vertexes.Count == 0 || cryMesh.Faces.Count == 0)
			{
				return;
			}

			for (int i = 0; i < cryMesh.Faces.Count; i++)
			{
				var face = cryMesh.Faces[i];

				this.Faces.Add(new FullFace
				{
					First = new FullVertex(cryMesh, face.First),
					Second = new FullVertex(cryMesh, face.Second),
					Third = new FullVertex(cryMesh, face.Third),
					SubsetIndex = face.Subset
				});
			}
		}
		/// <summary>
		/// Creates a mesh from BSP tree.
		/// </summary>
		/// <param name="bspTree">Root of the BSP tree.</param>
		public FaceMesh(BspNode<FullFace> bspTree)
		{
			this.Faces = bspTree.AllElements;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Combine this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Union"/>
		/// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
		public virtual void Combine(FaceMesh anotherMesh)
		{
			if (NativeCsg)
			{
				var theseFaces = this.Faces.ToArray();
				var otherFaces = anotherMesh.Faces.ToArray();
				fixed (FullFace* theseFacesPtr = theseFaces)
				fixed (FullFace* otherFacesPtr = otherFaces)
				{
					int faceCount;
					var facesPtr = CsgOpInternal(theseFacesPtr, theseFaces.Length, otherFacesPtr, otherFaces.Length,
												 CsgOpCode.Combine, out faceCount);
					this.Faces = ToList(facesPtr, faceCount);
					DeleteListItems(facesPtr);
				}
			}
			else
			{
				BspNode<FullFace> a = this.BspTree;
				BspNode<FullFace> b = anotherMesh.BspTree;
				a.Unite(b, null);
				this.Set(a);
			}
		}
		/// <summary>
		/// Intersects this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Intersection"/>
		/// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
		public virtual void Intersect(FaceMesh anotherMesh)
		{
			if (NativeCsg)
			{
				var theseFaces = this.Faces.ToArray();
				var otherFaces = anotherMesh.Faces.ToArray();
				fixed (FullFace* theseFacesPtr = theseFaces)
				fixed (FullFace* otherFacesPtr = otherFaces)
				{
					int faceCount;
					var facesPtr = CsgOpInternal(theseFacesPtr, theseFaces.Length, otherFacesPtr, otherFaces.Length,
												 CsgOpCode.Intersect, out faceCount);
					this.Faces = ToList(facesPtr, faceCount);
					DeleteListItems(facesPtr);
				}
			}
			else
			{
				BspNode<FullFace> a = this.BspTree;
				BspNode<FullFace> b = anotherMesh.BspTree;
				// Cut geometry that is not common for the meshes.
				a.Invert();
				b.CutTreeOut(a, null);
				b.Invert();
				a.CutTreeOut(b, null);
				// Clean up remains.
				b.CutTreeOut(a, null);
				// Combine geometry.
				a.AddElements(b.AllElements, null);
				// Invert everything.
				a.Invert();
				this.Set(a);
			}
		}
		/// <summary>
		/// Subtracts another mesh from this one.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Subtract"/>
		/// <exception cref="OverflowException">The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue" /> elements.</exception>
		public virtual void Subtract(FaceMesh anotherMesh)
		{
			if (NativeCsg)
			{
				var theseFaces = this.Faces.ToArray();
				var otherFaces = anotherMesh.Faces.ToArray();
				fixed (FullFace* theseFacesPtr = theseFaces)
				fixed (FullFace* otherFacesPtr = otherFaces)
				{
					int faceCount;
					var facesPtr = CsgOpInternal(theseFacesPtr, theseFaces.Length, otherFacesPtr, otherFaces.Length,
												 CsgOpCode.Subtract, out faceCount);
					this.Faces = ToList(facesPtr, faceCount);
					DeleteListItems(facesPtr);
				}
			}
			else
			{
				BspNode<FullFace> a = this.BspTree;
				BspNode<FullFace> b = anotherMesh.BspTree;
				a.Invert();
				a.Unite(b, null);
				a.Invert();
				this.Set(a);
			}
		}
		/// <summary>
		/// Sets this mesh to one represented by a BSP tree.
		/// </summary>
		/// <param name="bspTree">Root of the BSP tree.</param>
		public void Set(BspNode<FullFace> bspTree)
		{
			this.Faces.Clear();
			this.Faces.AddRange(bspTree.AllElements);
		}
		/// <summary>
		/// Exports face and vertex data from this mesh to the CryEngine mesh.
		/// </summary>
		/// <param name="mesh">
		/// CryEngine mesh that will host this one.
		/// </param>
		/// <param name="override">
		/// Indicates whether data within <paramref name="mesh"/> must be overridden by data from this one.
		/// </param>
		/// <param name="fast">
		/// Indicates whether duplicate vertices need to be merged together. Process of finding duplicates is quite lengthy.
		/// </param>
		public void Export(CryMesh mesh, bool @override = true, bool fast = true)
		{
			if (!mesh.IsValid)
			{
				return;
			}

			// Cache all parts of the mesh object so compiler doesn't complain about the error that is not the error.
			var facesCollection = mesh.Faces;
			var vertexesCollection = mesh.Vertexes;
			var positionsCollection = mesh.Vertexes.Positions;
			var normalsCollection = mesh.Vertexes.Normals;
			var colors0Collection = mesh.Vertexes.PrimaryColors;
			var colors1Collection = mesh.Vertexes.SecondaryColors;
			var uvPositionsCollection = mesh.TexturePositions;

			if (@override)
			{
				mesh.Faces.Clear();
				mesh.Vertexes.Clear();
				mesh.TexturePositions.Clear();
			}

			//
			// Prepare vertex and face arrays for export.
			//

			List<FullVertex> vertexes = new List<FullVertex>(this.Faces.Count * 3);
			List<CryMeshFace> faces = new List<CryMeshFace>(this.Faces.Count);

			foreach (var face in this.Faces)
			{
				CryMeshFace currentFace;
				if (fast)
				{
					vertexes.Add(face.First);
					currentFace.First = vertexes.Count;
					vertexes.Add(face.Second);
					currentFace.Second = vertexes.Count;
					vertexes.Add(face.Third);
					currentFace.Third = vertexes.Count;
				}
				else
				{
					int currentVertexIndex = vertexes.IndexOf(face.First);
					if (currentVertexIndex < 0)
					{
						vertexes.Add(face.First);
						currentFace.First = vertexes.Count;
					}
					else
					{
						currentFace.First = currentVertexIndex;
					}
					currentVertexIndex = vertexes.IndexOf(face.Second);
					if (currentVertexIndex < 0)
					{
						vertexes.Add(face.Second);
						currentFace.Second = vertexes.Count;
					}
					else
					{
						currentFace.Second = currentVertexIndex;
					}
					currentVertexIndex = vertexes.IndexOf(face.Third);
					if (currentVertexIndex < 0)
					{
						vertexes.Add(face.Third);
						currentFace.Third = vertexes.Count;
					}
					else
					{
						currentFace.Third = currentVertexIndex;
					}
				}
				currentFace.Subset = (byte)face.SubsetIndex;

				faces.Add(currentFace);
			}

			//
			// Export the data.
			//

			// Reallocate data and determine indexes of first slot to put the data in.
			int firstVertexIndex = mesh.Vertexes.Count;
			int firstFaceIndex = mesh.Faces.Count;

			vertexesCollection.Count = firstVertexIndex + vertexes.Count;
			uvPositionsCollection.Count = firstVertexIndex + vertexes.Count;
			facesCollection.Count = firstFaceIndex + faces.Count;

			// Export vertex data.
			for (int i = 0, j = firstVertexIndex; i < vertexes.Count; i++, j++)
			{
				FullVertex vertex = vertexes[i];
				positionsCollection[j] = vertex.Position;
				normalsCollection[j] = new CryMeshNormal {Normal = vertex.Normal};
				colors0Collection[j] = new CryMeshColor(vertex.PrimaryColor);
				colors1Collection[j] = new CryMeshColor(vertex.SecondaryColor);
				uvPositionsCollection[j] = new CryMeshTexturePosition(vertex.UvPosition);
			}

			// Export face data.
			for (int i = 0, j = firstFaceIndex; i < faces.Count; i++, j++)
			{
				facesCollection[j] = faces[i];
			}
		}
		#endregion
		#region Utilities
		private static List<FullFace> ToList(FullFace* facesPtr, int faceCount)
		{
			if (faceCount <= 0 || facesPtr == null)
			{
				return null;
			}

			var faces = new List<FullFace>(faceCount);
			for (int i = 0; i < faceCount; i++)
			{
				faces.Add(facesPtr[i]);
			}

			return faces;
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteListItems(FullFace* facesPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FullFace* CsgOpInternal(FullFace* facesPtr1, int faceCount1, FullFace* facesPtr2,
													   int faceCount2, CsgOpCode op, out int faceCount);
		#endregion
	}
}