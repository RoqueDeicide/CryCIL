using System.Collections.Generic;
using CryCil.Geometry.Csg;
using CryCil.Geometry.Csg.Base;
using CryCil.Graphics;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a triangular mesh where each face is a triangle with it's own set of
	/// vertices.
	/// </summary>
	public class FaceMesh
	{
		/// <summary>
		/// Gets the list of faces that comprise this mesh.
		/// </summary>
		public List<FullFace> Faces { get; private set; }
		/// <summary>
		/// Creates a BSP tree from polygons that form this mesh.
		/// </summary>
		public BspNode<FullFace> BspTree
		{
			get
			{
				return new BspNode<FullFace>(this.Faces, null);
			}
		}
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
		/// <summary>
		/// Combine this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Union"/>
		public virtual void Combine(FaceMesh anotherMesh)
		{
			BspNode<FullFace> a = this.BspTree;
			BspNode<FullFace> b = anotherMesh.BspTree;
			a.Unite(b, null);
			this.Set(a);
		}
		/// <summary>
		/// Intersects this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Intersection"/>
		public virtual void Intersect(FaceMesh anotherMesh)
		{
			BspNode<FullFace> a = this.BspTree;
			BspNode<FullFace> b = anotherMesh.BspTree;
			a.Invert();					// Cut geometry that is not common for the meshes.
			b.CutTreeOut(a, null);		//
			b.Invert();					//
			a.CutTreeOut(b, null);		//
			// Clean up remains.
			b.CutTreeOut(a, null);
			// Combine geometry.
			a.AddElements(b.AllElements, null);
			// Invert everything.
			a.Invert();
			this.Set(a);
		}
		/// <summary>
		/// Subtracts another mesh from this one.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Subtract"/>
		public virtual void Subtract(FaceMesh anotherMesh)
		{
			BspNode<FullFace> a = this.BspTree;
			BspNode<FullFace> b = anotherMesh.BspTree;
			a.Invert();
			a.Unite(b, null);
			a.Invert();
			this.Set(a);
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
	}
}