using System.Collections.Generic;
using System.Linq;
using CryEngine.Mathematics.Geometry.Meshes.CSG;
using CryEngine.Mathematics.Geometry.Meshes.Solids;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	public partial class Mesh
	{
		/// <summary>
		/// Combine this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Union"/>
		public virtual void Combine(IMesh anotherMesh)
		{
			BspNode<SplittableTriangle> a = this.ToBspTree();
			BspNode<SplittableTriangle> b = anotherMesh.ToBspTree();
			a.Unite(b);
			this.SetBsp(a);
		}
		/// <summary>
		/// Intersects this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Intersection"/>
		public virtual void Intersect(IMesh anotherMesh)
		{
			BspNode<SplittableTriangle> a = this.ToBspTree();
			BspNode<SplittableTriangle> b = anotherMesh.ToBspTree();
			a.Invert();				// Cut geometry that is not common for the meshes.
			b.CutTreeOut(a);		//
			b.Invert();				//
			a.CutTreeOut(b);		//
			// Clean up remains.
			b.CutTreeOut(a);
			// Combine geometry.
			a.AddElements(b.AllElements);
			// Invert everything.
			a.Invert();
			this.SetBsp(a);
		}
		/// <summary>
		/// Subtracts another mesh from this one.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Subtract"/>
		public virtual void Subtract(IMesh anotherMesh)
		{
			BspNode<SplittableTriangle> a = this.ToBspTree();
			BspNode<SplittableTriangle> b = anotherMesh.ToBspTree();
			a.Invert();
			a.Unite(b);
			a.Invert();
			this.SetBsp(a);
		}
		/// <summary>
		/// Creates BSP tree from this mesh.
		/// </summary>
		/// <returns>BSP tree that is formed by this mesh.</returns>
		public virtual BspNode<SplittableTriangle> ToBspTree()
		{
			List<SplittableTriangle> faces = new List<SplittableTriangle>(this.Faces.Count);

			for (int i = 0; i < this.Faces.Count; i++)
			{
				faces.Add
				(
					new SplittableTriangle
					(
						this.Faces[i].Indices.Select
						(
							x =>
							new MeshVertex
							{
								Position = this.Positions[x],
								Normal = this.Normals[x],
								UvMapPosition = this.TextureCoordinates[x],
								PrimaryColor = this.PrimaryColors[x],
								SecondaryColor = this.SecondaryColors[x]
							}
						)
					)
				);
			}
			return new BspNode<SplittableTriangle>(faces);
		}
		/// <summary>
		/// When implemented, changes this mesh to be perfect representation of the given BSP tree.
		/// </summary>
		/// <param name="tree">BSP tree to convert to this mesh.</param>
		public abstract void SetBsp(BspNode<SplittableTriangle> tree);
	}
}