using System;
using System.Collections.Generic;
using System.Linq;
using CryCil.Geometry.Csg;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a face in the mesh.
	/// </summary>
	public struct FullFace : ISpatiallySplittable<FullFace>
	{
		/// <summary>
		/// First vertex of this face.
		/// </summary>
		public FullVertex First;
		/// <summary>
		/// Second vertex of this face.
		/// </summary>
		public FullVertex Second;
		/// <summary>
		/// Third vertex of this face.
		/// </summary>
		public FullVertex Third;
		/// <summary>
		/// Gets a plane this face is located on.
		/// </summary>
		public Plane Plane
		{
			get { return new Plane(this.First.Position, this.Second.Position, this.Third.Position); }
		}
		/// <summary>
		/// Gets a normal to the plane this face is located on.
		/// </summary>
		public Vector3 Normal
		{
			get { return (this.Second.Position - this.First.Position) % (this.Third.Position - this.First.Position).Normalized; }
		}
		/// <summary>
		/// Gets a plane this face is located on.
		/// </summary>
		/// <param name="customData">Optional data that can be used in calculations.</param>
		/// <returns>
		/// <see cref="Plane"/> object that describes the plane this object is located on.
		/// </returns>
		public Plane GetPlane(object customData = null)
		{
			return new Plane(this.First.Position, this.Second.Position, this.Third.Position);
		}
		/// <summary>
		/// Gets a normal to the plane this face is located on.
		/// </summary>
		/// <param name="customData">Optional data that can be used in calculations.</param>
		/// <returns>Normalized <see cref="Vector3"/> that represents a normal.</returns>
		public Vector3 GetNormal(object customData = null)
		{
			return (this.Second.Position - this.First.Position) % (this.Third.Position - this.First.Position).Normalized;
		}
		/// <summary>
		/// Gets a list of vertices.
		/// </summary>
		public FullVertex[] Vertices
		{
			get { return new[] {this.First, this.Second, this.Third}; }
		}
		/// <summary>
		/// Creates new instance of type <see cref="FullFace"/> .
		/// </summary>
		/// <param name="vertices">
		/// A collection of vertices first 3 elements of which will define a triangle.
		/// </param>
		public FullFace(IEnumerable<FullVertex> vertices)
			: this()
		{
			this.Init(vertices);
		}
		/// <summary>
		/// Creates new instance of type <see cref="FullFace"/> .
		/// </summary>
		/// <param name="vertices">
		/// A collection of vertices first 3 elements of which will define a triangle.
		/// </param>
		public FullFace(ICollection<FullVertex> vertices)
			: this()
		{
			if (vertices.Count < 3)
			{
				throw new ArgumentException("Not enough vertices to define a triangle.");
			}
			this.Init(vertices);
		}
		/// <summary>
		/// Creates new instance of type <see cref="FullFace"/> .
		/// </summary>
		/// <param name="vertices">
		/// A collection of vertices first 3 elements of which will define a triangle.
		/// </param>
		public FullFace(IList<FullVertex> vertices)
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
		/// <summary>
		/// Splits this face with a plane.
		/// </summary>
		/// <param name="splitter">          Plane that splits this face into parts.</param>
		/// <param name="frontCoplanarFaces">
		/// A collection to add this face to, if it's located on the splitter and is facing the same way.
		/// </param>
		/// <param name="backCoplanarFaces"> 
		/// A collection to add this face to, if it's located on the splitter and is facing the opposite
		/// way.
		/// </param>
		/// <param name="frontFaces">        
		/// A collection to add parts of this face that are located in front of the splitter.
		/// </param>
		/// <param name="backFaces">         
		/// A collection to add parts of this face that are located behind the splitter.
		/// </param>
		/// <param name="customData">        Not used.</param>
		public void Split(Plane splitter,
						  ICollection<FullFace> frontCoplanarFaces, ICollection<FullFace> backCoplanarFaces,
						  ICollection<FullFace> frontFaces, ICollection<FullFace> backFaces, object customData = null)
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
						if (frontCoplanarFaces != null) frontCoplanarFaces.Add(this);
					}
					else
					{
						if (backCoplanarFaces != null) backCoplanarFaces.Add(this);
					}
					break;
				case PlanePosition.Front:
					if (frontFaces != null) frontFaces.Add(this);
					break;
				case PlanePosition.Back:
					if (backFaces != null) backFaces.Add(this);
					break;
				case PlanePosition.Spanning:
					if (frontFaces == null && backFaces == null)
					{
						return; // Any calculations won't be saved anywhere.
					}
					// Prepare to create a split of this triangle.
					// 
					// Cash vertices into an array, so we can loop through it.
					FullVertex[] vertices = this.Vertices;
					// Create lists for vertices on the front and back.
					List<FullVertex> fvs = new List<FullVertex>(4);
					List<FullVertex> bvs = new List<FullVertex>(4);
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
							FullVertex splittingVertex =
								Interpolation.Linear.Create(vertices[i], vertices[j], positionParameter);
							// Add splitting vertex to both lists.
							fvs.Add(splittingVertex);
							bvs.Add(splittingVertex);
						}
						// Create front and back triangle(s) from vertices from corresponding lists.
						if (frontFaces != null) TriangulateLinearly(fvs, false, frontFaces);
						if (backFaces != null) TriangulateLinearly(bvs, false, backFaces);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		/// <summary>
		/// Flips this face.
		/// </summary>
		public void Invert()
		{
			// Reverse order of vertices.
			FullVertex temp = this.Third;
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
		public static FullFace[] TriangulateLinearly(IList<FullVertex> vertices, bool checkForCoplanarity)
		{
			if (vertices.IsNullOrEmpty())
			{
				return null;
			}
			// If we are given a triangle, there is no need to bother about anything.
			if (vertices.Count == 3)
			{
				return new[] {new FullFace(vertices)};
			}
			if (checkForCoplanarity)
			{
				Plane plane = new Plane(vertices[0].Position, vertices[1].Position, vertices[2].Position);
				if (vertices.Any(x => plane.PointPosition(x.Position) != PlanePosition.Coplanar))
				{
					throw new ArgumentException("Vertices that describe a border of a polygon for" +
												" triangulation are not located on the same plane.");
				}
			}
			FullFace[] result = new FullFace[vertices.Count - 2];
			for (int i = 0; i < result.Length; i++)
			{
				result[0] = new FullFace
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
		public static void TriangulateLinearly(IList<FullVertex> vertices, bool checkForCoplanarity,
											   ICollection<FullFace> polygons)
		{
			if (vertices.IsNullOrEmpty())
			{
				return;
			}
			// If we are given a triangle, there is no need to bother about anything.
			if (vertices.Count == 3)
			{
				polygons.Add(new FullFace(vertices));
			}
			if (checkForCoplanarity)
			{
				Plane plane = new Plane(vertices[0].Position, vertices[1].Position, vertices[2].Position);
				if (vertices.Any(x => plane.PointPosition(x.Position) != PlanePosition.Coplanar))
				{
					throw new ArgumentException("Vertices that describe a border of a polygon for" +
												" triangulation are not located on the same plane.");
				}
			}
			int triangleCount = vertices.Count - 2;
			for (int i = 0; i < triangleCount; i++)
			{
				polygons.Add(new FullFace
				{
					First = vertices[0],
					Second = vertices[i + 1],
					Third = vertices[i + 2]
				});
			}
		}
		private void Init(IEnumerable<FullVertex> vertices)
		{
			IEnumerator<FullVertex> enumerator = vertices.GetEnumerator();
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
	}
}