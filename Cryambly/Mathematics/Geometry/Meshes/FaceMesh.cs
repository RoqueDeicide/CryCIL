using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Memory;
using CryCil.Geometry.Csg;
using CryCil.Geometry.Csg.Base;

namespace CryCil.Geometry
{
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