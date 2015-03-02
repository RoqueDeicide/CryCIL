using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.Mathematics.Geometry.Meshes.CSG
{
	/// <summary>
	/// Represents a triangle.
	/// </summary>
	public struct SplittableTriangle : ISpatiallySplittable<SplittableTriangle>
	{
		#region Fields
		/// <summary>
		/// First vertex of the triangle.
		/// </summary>
		public MeshVertex First;
		/// <summary>
		/// Second vertex of the triangle.
		/// </summary>
		public MeshVertex Second;
		/// <summary>
		/// Third vertex of the triangle.
		/// </summary>
		public MeshVertex Third;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a plane this triangle is located on.
		/// </summary>
		public Plane Plane
		{
			get { return new Plane(this.First.Position, this.Second.Position, this.Third.Position); }
		}
		/// <summary>
		/// Gets a normal of this triangle.
		/// </summary>
		public Vector3 Normal
		{
			get
			{
				return (this.Second.Position - this.First.Position)
				  .Cross(this.Third.Position - this.First.Position).Normalized;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="SplittableTriangle"/> .
		/// </summary>
		/// <param name="vertices">
		/// A collection of vertices first 3 elements of which will define a triangle.
		/// </param>
		public SplittableTriangle(IEnumerable<MeshVertex> vertices)
			: this()
		{
			this.Init(vertices);
		}
		/// <summary>
		/// Creates new instance of type <see cref="SplittableTriangle"/> .
		/// </summary>
		/// <param name="vertices">
		/// A collection of vertices first 3 elements of which will define a triangle.
		/// </param>
		public SplittableTriangle(ICollection<MeshVertex> vertices)
			: this()
		{
			if (vertices.Count < 3)
			{
				throw new ArgumentException("Not enough vertices to define a triangle.");
			}
			this.Init(vertices);
		}
		/// <summary>
		/// Creates new instance of type <see cref="SplittableTriangle"/> .
		/// </summary>
		/// <param name="vertices">
		/// A collection of vertices first 3 elements of which will define a triangle.
		/// </param>
		public SplittableTriangle(IList<MeshVertex> vertices)
			: this()
		{
			if (vertices.Count < 3)
			{
				throw new ArgumentException("Not enough vertices to define a triangle.");
			}
			this.First = vertices[0];
			this.Second = vertices[1];
			this.Third = vertices[2];
		}
		#endregion
		#region Interface
		/// <summary>
		/// Splits this triangle with given plane.
		/// </summary>
		/// <param name="splitter">             <see cref="Plane"/> that is used for splitting.</param>
		/// <param name="frontCoplanarElements">
		/// An optional collection for this triangle if it's located on this plane and faces the same way.
		/// </param>
		/// <param name="backCoplanarElements"> 
		/// An optional collection for this triangle if it's located on this plane and faces the opposite
		/// way.
		/// </param>
		/// <param name="frontElements">        
		/// An optional collection for parts of this triangle that are located in front of this plane.
		/// </param>
		/// <param name="backElements">         
		/// An optional collection for parts of this triangle that are located behind this plane.
		/// </param>
		/// <param name="customData">           Not used.</param>
		public void Split(Plane splitter,
						  ICollection<SplittableTriangle> frontCoplanarElements,
						  ICollection<SplittableTriangle> backCoplanarElements,
						  ICollection<SplittableTriangle> frontElements,
						  ICollection<SplittableTriangle> backElements,
						  object customData = null)
		{
			PlanePosition triangleType = 0;
			PlanePosition[] positions = new PlanePosition[3];
			// Determine position of the triangle relative to the plane.
			splitter.PointPosition(this.First.Position, out positions[0], ref triangleType);
			splitter.PointPosition(this.Second.Position, out positions[1], ref triangleType);
			splitter.PointPosition(this.Third.Position, out positions[2], ref triangleType);
			// Process this triangle's data based on its position.
			switch (triangleType)
			{
				case PlanePosition.Coplanar:
					// See where this triangle is looking and it to corresponding list.
					if (this.Normal * splitter.Normal > 0)
					{
						if (frontCoplanarElements != null) frontCoplanarElements.Add(this);
					}
					else
					{
						if (backCoplanarElements != null) backCoplanarElements.Add(this);
					}
					break;
				case PlanePosition.Front:
					if (frontElements != null) frontElements.Add(this);
					break;
				case PlanePosition.Back:
					if (backElements != null) backElements.Add(this);
					break;
				case PlanePosition.Spanning:
					if (frontElements == null && backElements == null)
					{
						return;				// Any calculations won't be saved anywhere.
					}
					// Prepare to create a split of this triangle.
					// 
					// Cash vertices into an array, so we can loop through it.
					MeshVertex[] vertices = this.Vertices;
					// Create lists for vertices on the front and back.
					List<MeshVertex> fvs = new List<MeshVertex>(4);
					List<MeshVertex> bvs = new List<MeshVertex>(4);
					// Process edges.
					// 
					// We go through the polygon edge by edge with i being index of the start of the edge,
					// and j - end.
					for (int i = 0, j = 1; i < 3; i++, j = (j + 1) % 3)
					{
						// If edge doesn't begin behind the plane, add starting vertex to front vertices.
						if (positions[i] != PlanePosition.Back)
						{
							fvs.Add(vertices[i]);
						}
						// Else put the starting vertex to the back vertices.
						else
						{
							bvs.Add(vertices[i]);
						}
						// If this edge intersects the plane, split it.
						if ((positions[i] | positions[j]) == PlanePosition.Spanning)
						{
							// Calculate fraction that describes position of splitting vertex along the
							// line between start and end of the edge.
							float positionParameter =
								(splitter.D - splitter.Normal * vertices[i].Position)
								/
								(splitter.Normal * (vertices[j].Position - vertices[i].Position));
							// Linearly interpolate the vertex that splits the edge.
							MeshVertex splittingVertex =
								(MeshVertex)vertices[i].CreateLinearInterpolation(vertices[j], positionParameter);
							// Add splitting vertex to both lists.
							fvs.Add(splittingVertex);
							bvs.Add(splittingVertex);
						}
						// Create front and back triangle(s) from vertices from corresponding lists.
						if (frontElements != null) SplittableTriangle.TriangulateLinearly(fvs, false, frontElements);
						if (backElements != null) SplittableTriangle.TriangulateLinearly(bvs, false, backElements);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		/// <summary>
		/// Flips this triangle.
		/// </summary>
		public void Invert()
		{
			// Reverse order of vertices.
			MeshVertex temp = this.Third;
			this.Third = this.First;
			this.First = temp;
		}
		/// <summary>
		/// Creates an array of triangular faces that form a polygon from a given list of vertices.
		/// </summary>
		/// <param name="vertices">           
		/// A list of vertices that describes a border of the polygon.
		/// </param>
		/// <param name="checkForCoplanarity">
		/// Indicates whether coplanarity of given polygon must be ensured. Pass false only if know, that
		/// your polygon is on one plane.
		/// </param>
		/// <returns>An array of triangles.</returns>
		public static SplittableTriangle[] TriangulateLinearly(IList<MeshVertex> vertices, bool checkForCoplanarity)
		{
			if (vertices.IsNullOrEmpty())
			{
				return null;
			}
			// If we are given a triangle, there is no need to bother about anything.
			if (vertices.Count == 3)
			{
				return new[] { new SplittableTriangle(vertices) };
			}
			if (checkForCoplanarity)
			{
				Plane plane = new Plane(vertices[0].Position, vertices[1].Position, vertices[2].Position);
				if (vertices.Any(x => plane.PointPosition(x.Position) != PlanePosition.Coplanar))
				{
					throw new ArgumentException("Vertices that describe a border of a polygon for triangulation are not located on the same plane.");
				}
			}
			SplittableTriangle[] result = new SplittableTriangle[vertices.Count - 2];
			for (int i = 0; i < result.Length; i++)
			{
				result[0] = new SplittableTriangle
				{
					First = vertices[0],
					Second = vertices[i + 1],
					Third = vertices[i + 2]
				};
			}
			return result;
		}
		/// <summary>
		/// Triangulates given polygon and adds resultant triangles to the list of polygons.
		/// </summary>
		/// <param name="vertices">           
		/// A list of vertices that represents a border of the polygon that is going to be triangulated.
		/// </param>
		/// <param name="checkForCoplanarity">
		/// Indicates whether coplanarity of given polygon must be ensured. Pass false only if know, that
		/// your polygon is on one plane.
		/// </param>
		/// <param name="polygons">           The list of polygon to which to add the triangles.</param>
		public static void TriangulateLinearly(IList<MeshVertex> vertices, bool checkForCoplanarity,
											   ICollection<SplittableTriangle> polygons)
		{
			if (vertices.IsNullOrEmpty())
			{
				return;
			}
			// If we are given a triangle, there is no need to bother about anything.
			if (vertices.Count == 3)
			{
				polygons.Add(new SplittableTriangle(vertices));
			}
			if (checkForCoplanarity)
			{
				Plane plane = new Plane(vertices[0].Position, vertices[1].Position, vertices[2].Position);
				if (vertices.Any(x => plane.PointPosition(x.Position) != PlanePosition.Coplanar))
				{
					throw new ArgumentException("Vertices that describe a border of a polygon for triangulation are not located on the same plane.");
				}
			}
			int triangleCount = vertices.Count - 2;
			for (int i = 0; i < triangleCount; i++)
			{
				polygons.Add(new SplittableTriangle
				{
					First = vertices[0],
					Second = vertices[i + 1],
					Third = vertices[i + 2]
				});
			}
		}
		#endregion
		#region Utilities
		private void Init(IEnumerable<MeshVertex> vertices)
		{
			IEnumerator<MeshVertex> enumerator = vertices.GetEnumerator();
			enumerator.Reset();
			if (enumerator.MoveNext())
			{
				this.First = enumerator.Current;
			}
			if (enumerator.MoveNext())
			{
				this.Second = enumerator.Current;
			}
			if (!enumerator.MoveNext())
			{
				throw new ArgumentException("Not enough vertices to define a triangle.");
			}
			this.Third = enumerator.Current;
		}
		#endregion
	}
}