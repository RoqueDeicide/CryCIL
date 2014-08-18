/*
 * Created by SharpDevelop.
 * User: Deicide
 * Date: 08.08.2014
 * Time: 18:27
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CryEngine.Mathematics.Geometry.Meshes.Solids
{
	/// <summary>
	/// Defines boolean operations for CSG editing technique.
	/// </summary>
	/// <remarks>
	/// <para> Constructive Solid Geometry (CSG) is a modeling technique that uses Boolean
	/// operations like union and intersection to combine 3D solids. This library implements CSG
	/// operations on meshes elegantly and concisely using BSP trees, and is meant to serve as an
	/// easily understandable implementation of the algorithm. All edge cases involving overlapping
	/// coplanar polygons in both solids are correctly handled.
	/// </para>
	///
	/// <para>Implementation Details</para><para></para>
	///
	/// <para> All CSG
	/// operations are implemented in terms of two functions, `clipTo()` and `invert()`, which
	/// remove parts of a BSP tree inside another BSP tree and swap solid and empty space,
	/// respectively. To find the union of `a` and `b`, we want to remove everything in `a` inside
	/// `b` and everything in `b` inside `a`, then combine polygons from `a` and `b` into one solid:
	/// </para><para></para>
	///
	/// <para>
	/// <code>
	/// a.ClipTo(b);
	/// b.ClipTo(a);
	/// a.Build(b.AllPolygons);
	/// </code>
	/// </para>
	/// <para></para>
	///
	/// <para> The only tricky part is handling overlapping coplanar
	/// polygons in both trees. The code above keeps both copies, but we need to keep them in one
	/// tree and remove them in the other tree. To remove them from `b` we can clip the inverse of
	/// `b` against `a`. The code for union now looks like this:
	/// </para><para></para>
	///
	/// <para>
	/// <code>
	/// a.ClipTo(b);
	/// b.ClipTo(a);
	/// b.Invert();
	/// b.ClipTo(a);
	/// b.Invert();
	/// a.Build(b.AllPolygons);
	/// </code>
	/// </para>
	/// <para></para>
	///
	/// <para> Subtraction and intersection
	/// naturally follow from set operations. If union is `A | B`, subtraction is `A - B = ~(~A |
	/// B) ` and intersection is `A &amp; B = ~(~A | ~B)` where `~` is the complement operator.
	/// </para>
	///
	/// <para>License</para><para></para>
	///
	/// <para>Copyright (c) 2011 Evan Wallace
	/// (http://madebyevan.com/), under the MIT license.</para>
	/// </remarks>
	/// <example>
	/// <code>
	/// Example illustrated in Wikipedia: http://en.wikipedia.org/wiki/File:Csg_tree.png
	///
	/// // Prepare cylinders.
	/// const float exampleCylinderLength = 10;
	/// const float exampleCylinderRadius = 2;
	///
	/// Vector3 cylinderRight = new Vector3(exampleCylinderLength, 0, 0);
	/// Vector3 cylinderFront = new Vector3(0, exampleCylinderLength, 0);
	/// Vector3 cylinderTop = new Vector3(0, 0, exampleCylinderLength);
	/// Vector3 cylinderLeft = new Vector3(-exampleCylinderLength, 0, 0);
	/// Vector3 cylinderBack = new Vector3(0, -exampleCylinderLength, 0);
	/// Vector3 cylinderBottom = new Vector3(0, 0, -exampleCylinderLength);
	///
	/// Solid cylinderX = Solid.Cylinder(cylinderLeft, cylinderRight, exampleCylinderRadius);
	/// Solid cylinderY = Solid.Cylinder(cylinderBack, cylinderFront, exampleCylinderRadius);
	/// Solid cylinderZ = Solid.Cylinder(cylinderBottom, cylinderTop, exampleCylinderRadius);
	/// // Prepare cube.
	/// Solid exampleCube = Solid.Cuboid(null, new Vector3(4));
	/// // Prepare sphere.
	/// Solid exampleSphere = Solid.Sphere(null, 2.6f);
	/// // Construct a complicated object.
	/// Solid complexGeomtery =
	///     ConstructiveSolidGeometry.Subtract
	///     (
	///         ConstructiveSolidGeometry.Intersection(exampleCube, exampleSphere),
	///         ConstructiveSolidGeometry.Union(cylinderX, CSG.Union(cylinderY, cylinderZ))
	///     );
	/// </code>
	/// </example>
	public static class ConstructiveSolidGeometry
	{
		/// <summary>
		/// Combines two solids together into one solid.
		/// </summary>
		/// <remarks>
		/// <code>
		/// +-------+            +-------+
		/// |       |            |       |
		/// |   A   |            |       |
		/// |    +--+----+   =   |       +----+
		/// +----+--+    |       +----+       |
		///      |   B   |            |       |
		///      |       |            |       |
		///      +-------+            +-------+
		/// </code></remarks>
		/// <example>
		/// <code>
		/// // Create 2x1x1 cuboid from two 1x1x1 cubes placed in a row.
		/// const float blockSideLength = 1;
		///
		/// Solid block1 = Solid.Cuboid(new Vector3(), new Vector3(blockSideLength));
		/// Solid block2 = Solid.Cuboid(new Vector3(blockSideLength / 2, 0, 0), new Vector3(blockSideLength));
		///
		/// Solid twoBlocksInARow = ConstructiveSolidGeometry.Union(block1, block2);
		///
		/// // Create capsule from two spheres and a cylinder.
		/// const float capsuleHeight = 4;
		/// const float capsuleRadius = 0.5f;
		///
		/// Vector3 capsuleBodyTop = new Vector3(0, 0, capsuleHeight / 2 - capsuleRadius);
		/// Vector3 capsuleBodyBottom = new Vector3(0, 0, -(capsuleHeight / 2 - capsuleRadius));
		///
		/// Solid topCap = Solid.Sphere(capsuleBodyTop, capsuleRadius);
		/// Solid bottomCap = Solid.Sphere(capsuleBodyBottom, capsuleRadius);
		/// Solid capsuleBody = Solid.Cylinder(capsuleBodyBottom, capsuleBodyTop, capsuleRadius);
		///
		/// Solid capsule =
		///      ConstructiveSolidGeometry.Union(topCap, ConstructiveSolidGeometry.Union(capsuleBody, bottomCap));
		/// </code>
		/// </example>
		/// <param name="a"> First solid. </param>
		/// <param name="b"> Second solid. </param>
		/// <returns> Result of union. </returns>
		public static Solid Union(Solid a, Solid b)
		{
			// Initialize BSP trees.
			Node aNode = new Node(a.Polygons);
			Node bNode = new Node(b.Polygons);

			aNode.ClipTo(bNode);				// Remove B geometry from A.
			bNode.ClipTo(aNode);				// Remove A geometry from B.
			bNode.Invert();						// Invert B, so we can deal with coplanar intersections.
			bNode.ClipTo(aNode);				// Remove coplanars from B.
			bNode.Invert();						// Invert B back into original form.
			aNode.Build(bNode.AllPolygons);		// Rebuild A to incorporate geometry from B.
			return new Solid(aNode.AllPolygons);
		}
		/// <summary>
		/// Creates a solid which contains every point that is within both of given solids.
		/// </summary>
		/// <remarks>
		/// <code>
		/// +-------+
		/// |       |
		/// |   A   |
		/// |    +--+----+   =   +--+
		/// +----+--+    |       +--+
		///      |   B   |
		///      |       |
		///      +-------+
		/// </code>
		/// </remarks>
		/// <example>
		/// <code>
		/// // Get portion of a sphere that intersects a cube.
		/// const float radius = 0.5f;
		/// const float length = 2;
		///
		/// Solid cube = Solid.Cuboid(null, new Vector3(length));
		/// Solid sphere = Solid.Sphere(new Vector3(length, 0, 0), radius);
		///
		/// Solid affectedPortion = ConstructiveSolidGeometry.Intersection(cube, sphere);
		///
		/// // Clamp a cylinder into a sphere.
		/// const float areaRadius = 2;
		/// const float cylinderHeight = 4;
		/// const float cylinderRadius = 0.5f;
		///
		/// Solid longCylinder = Solid.Cylinder(new Vector3(0, 0, -cylinderHeight / 2),
		///                                     new Vector3(0, 0, cylinderHeight / 2),
		///                                     cylinderRadius);
		/// Solid sphericalArea = Solid.Sphere(null, areaRadius);
		///
		/// Solid clampedCylinder = ConstructiveSolidGeometry.Intersection(longCylinder, sphericalArea);
		/// </code>
		/// </example>
		/// <param name="a"> First solid. </param>
		/// <param name="b"> Second solid. </param>
		/// <returns> Result of intersection. </returns>
		public static Solid Intersection(Solid a, Solid b)
		{
			// Initialize BSP trees.
			Node aNode = new Node(a.Polygons);
			Node bNode = new Node(b.Polygons);

			aNode.Invert();						// Invert A.
			bNode.ClipTo(aNode);				// Remove geometry that is not in A from B.
			bNode.Invert();						// Invert B.
			aNode.ClipTo(bNode);				// Remove geometry that is not in B from A.
			bNode.ClipTo(aNode);				// Remove remains of A from B.
			aNode.Build(bNode.AllPolygons);		// Rebuild A to incorporate B.
			aNode.Invert();						// Invert A so it doesn't end up inside out.
			return new Solid(aNode.AllPolygons);
		}
		/// <summary>
		/// Subtracts right solid from left one.
		/// </summary>
		/// <remarks>
		/// <code>
		/// +-------+            +-------+
		/// |       |            |       |
		/// |   A   |            |       |
		/// |    +--+----+   =   |    +--+
		/// +----+--+    |       +----+
		///      |   B   |
		///      |       |
		///      +-------+
		/// </code>
		/// </remarks>
		/// <example>
		/// <code>
		/// // Create a tube.
		/// const float tubeOuterRadius = 2;
		/// const float tubeShellThickness = 0.1f;
		/// const float tubeLength = 100;
		///
		/// Vector3 leftCapPoint = new Vector3();
		/// Vector3 rightCapPoint = new Vector3(tubeLength, 0, 0);
		///
		/// Solid outerCylinder = Solid.Cylinder(leftCapPoint, rightCapPoint, tubeOuterRadius);
		/// Solid innerCylinder = Solid.Cylinder(leftCapPoint, rightCapPoint,
		///                                      tubeOuterRadius - tubeShellThickness);
		///
		/// Solid tube = CSG.Subtract(outerCylinder, innerCylinder);
		/// // Create a frame.
		/// const float frameSize = 10;
		/// const float frameThickness = 0.5f;
		///
		/// const float innerCuboidThickness = frameSize - frameThickness * 2;
		///
		/// Solid outerCube = Solid.Cuboid(null, new Vector3(frameSize));
		/// Solid firstInnerCuboid = Solid.Cuboid
		/// (
		///     null,
		///     new Vector3
		///     (
		///         frameSize + 2,
		///         innerCuboidThickness,
		///         innerCuboidThickness
		///     )
		/// );
		/// Solid secondInnerCuboid = Solid.Cuboid
		/// (
		///     null,
		///     new Vector3
		///     (
		///         innerCuboidThickness,
		///         frameSize + 2,
		///         innerCuboidThickness
		///     )
		/// );
		/// Solid thirdInnerCuboid = Solid.Cuboid
		/// (
		///     null,
		///     new Vector3
		///     (
		///         innerCuboidThickness,
		///         innerCuboidThickness,
		///         frameSize + 2
		///     )
		/// );
		///
		/// Solid cubeFrame =
		///     ConstructiveSolidGeometry.Subtract
		///     (
		///         outerCube,
		///         ConstructiveSolidGeometry.Union
		///         (
		///             firstInnerCuboid,
		///             ConstructiveSolidGeometry.Union
		///             (
		///                 secondInnerCuboid,
		///                 thirdInnerCuboid
		///             )
		///         )
		///     );
		/// </code>
		/// </example>
		/// <param name="a"> First solid. </param>
		/// <param name="b"> Second solid. </param>
		/// <returns> Result of subtraction. </returns>
		public static Solid Subtract(Solid a, Solid b)
		{
			// Initialize BSP trees.
			Node aNode = new Node(a.Polygons);
			Node bNode = new Node(b.Polygons);

			aNode.Invert();					// Invert A.

			aNode.ClipTo(bNode);			//
			bNode.ClipTo(aNode);			//
			bNode.Invert();					// Union of inverted A with B.
			bNode.ClipTo(aNode);			//
			bNode.Invert();					//
			aNode.Build(bNode.AllPolygons);	//

			aNode.Invert()					// Invert everything since what we've got
			;								// above is what we need but it's inside out.
			return new Solid(aNode.AllPolygons);
		}
		/// <summary>
		/// Represents a solid that can participate in CSG operations.
		/// </summary>
		public class Solid
		{
			/// <summary>
			/// A list of polygons that form this solid.
			/// </summary>
			public List<Polygon> Polygons;
			/// <summary>
			/// Creates a deep copy of this solid.
			/// </summary>
			public Solid Clone
			{
				get
				{
					return new Solid(this.Polygons.Select(x => x.Clone).ToList());
				}
			}
			/// <summary>
			/// Creates new solid.
			/// </summary>
			/// <param name="polygons">A list of polygons that form this solid.</param>
			public Solid(List<Polygon> polygons)
			{
				this.Polygons = polygons;
			}
			/// <summary>
			/// Creates an axis-aligned cuboid.
			/// </summary>
			/// <param name="centerVector">
			/// Optional vector that defines center of the cuboid. If null center will be an origin
			/// of coordinates.
			/// </param>
			/// <param name="lengthsSet">  
			/// Optional vector that defines lengths of the cuboid along the corresponding axes.
			/// </param>
			/// <returns>A new cuboid that can participate in CSG operations.</returns>
			public static Solid Cuboid(Vector3? centerVector = null,
									   Vector3? lengthsSet = null)
			{
				Vector3 center = centerVector ?? new Vector3();
				Vector3 lengths = lengthsSet ?? new Vector3(1);
				Vector3 halfs = lengths / 2;
				#region Vertex Positions
				Vector3[] vertexPositions =
				{
					new Vector3(center.X + halfs.X, center.Y + halfs.Y, center.Z + halfs.Z),	// A
					new Vector3(center.X - halfs.X, center.Y + halfs.Y, center.Z + halfs.Z),	// B
					new Vector3(center.X - halfs.X, center.Y - halfs.Y, center.Z + halfs.Z),	// C
					new Vector3(center.X + halfs.X, center.Y - halfs.Y, center.Z + halfs.Z),	// D
					new Vector3(center.X + halfs.X, center.Y + halfs.Y, center.Z - halfs.Z),	// A1
					new Vector3(center.X - halfs.X, center.Y + halfs.Y, center.Z - halfs.Z),	// B1
					new Vector3(center.X - halfs.X, center.Y - halfs.Y, center.Z - halfs.Z),	// C1
					new Vector3(center.X + halfs.X, center.Y - halfs.Y, center.Z - halfs.Z) 	// D1
				};
				#endregion
				// ReSharper disable EmptyStatement

				List<Polygon> polys = new List<Polygon>(6);
				; // Top.		[A  B  C  D ].
				polys.Add
				(
					new Polygon
					(
						new[]
						{
							new Vertex { Position = vertexPositions[0], Normal = Vector3.Up },
							new Vertex { Position = vertexPositions[1], Normal = Vector3.Up },
							new Vertex { Position = vertexPositions[2], Normal = Vector3.Up },
							new Vertex { Position = vertexPositions[3], Normal = Vector3.Up }
						}.ToList(),
						0
					)
				);
				;// Bottom.		[D1 C1 B1 A1].
				polys.Add
				(
					new Polygon
					(
						new[]
						{
							new Vertex { Position = vertexPositions[7], Normal = Vector3.Down },
							new Vertex { Position = vertexPositions[6], Normal = Vector3.Down },
							new Vertex { Position = vertexPositions[5], Normal = Vector3.Down },
							new Vertex { Position = vertexPositions[4], Normal = Vector3.Down }
						}.ToList(),
						0
					)
				);
				; // Right.		[A  D  D1 A1].
				polys.Add
				(
					new Polygon
					(
						new[]
						{
							new Vertex { Position = vertexPositions[0], Normal = Vector3.Right },
							new Vertex { Position = vertexPositions[3], Normal = Vector3.Right },
							new Vertex { Position = vertexPositions[7], Normal = Vector3.Right },
							new Vertex { Position = vertexPositions[4], Normal = Vector3.Right }
						}.ToList(),
						0
					)
				);
				; // Left.		[B1 C1 C  B ].
				polys.Add
				(
					new Polygon
					(
						new[]
						{
							new Vertex { Position = vertexPositions[5], Normal = Vector3.Left },
							new Vertex { Position = vertexPositions[6], Normal = Vector3.Left },
							new Vertex { Position = vertexPositions[2], Normal = Vector3.Left },
							new Vertex { Position = vertexPositions[3], Normal = Vector3.Left }
						}.ToList(),
						0
					)
				);
				; // Front.		[A1 B1 B  A ].
				polys.Add
				(
					new Polygon
					(
						new[]
						{
							new Vertex { Position = vertexPositions[4], Normal = Vector3.Forward },
							new Vertex { Position = vertexPositions[5], Normal = Vector3.Forward },
							new Vertex { Position = vertexPositions[1], Normal = Vector3.Forward },
							new Vertex { Position = vertexPositions[0], Normal = Vector3.Forward }
						}.ToList(),
						0
					)
				);
				; // Back.		[D  C  C1 D1].
				polys.Add
				(
					new Polygon
					(
						new[]
						{
							new Vertex { Position = vertexPositions[3], Normal = Vector3.Backward },
							new Vertex { Position = vertexPositions[2], Normal = Vector3.Backward },
							new Vertex { Position = vertexPositions[6], Normal = Vector3.Backward },
							new Vertex { Position = vertexPositions[7], Normal = Vector3.Backward }
						}.ToList(),
						0
					)
				);
				// ReSharper restore EmptyStatement
				return new Solid(polys);
			}
			/// <summary>
			/// Creates a sphere.
			/// </summary>
			/// <param name="centerVector">
			/// Optional vector that defines center of the sphere. If null center will be an origin
			/// of coordinates.
			/// </param>
			/// <param name="radius">      Radius of the sphere.</param>
			/// <param name="slices">      Tesseltation along the longitude.</param>
			/// <param name="stacks">      Tesseltation along the latitude.</param>
			/// <returns>A new sphere that can participate in CSG operations.</returns>
			public static Solid Sphere(Vector3? centerVector = null, float radius = 1.0f, float slices = 16.0f,
									   float stacks = 8.0f)
			{
				Vector3 center = centerVector ?? new Vector3();

				List<Polygon> polygons = new List<Polygon>();
				// Adds a vertex calculated from given polar coordinates to the list above.
				Func<float, float, Vertex> createVertex = delegate(float theta, float phi)
				{
					theta *= (float)(Math.PI * 2);
					phi *= (float)Math.PI;

					Vector3 direction = new Vector3
					(
						vx: (float)(Math.Cos(theta) * Math.Sin(phi)),
						vy: (float)Math.Cos(phi),
						vz: (float)(Math.Sin(theta) * Math.Sin(phi))
					);
					return new Vertex
					{
						Position = center + direction * radius,
						Normal = direction
					};
				};
				// Create slices.
				for (int i = 0; i < slices; i++)
				{
					// Create stacks.
					for (int j = 0; j < stacks; j++)
					{
						List<Vertex> vertices = new List<Vertex>();
						vertices.Add(createVertex(i / slices, j / stacks));
						if (j > 0)
						{
							vertices.Add(createVertex((i + 1) / slices, j / stacks));
						}
						if (j < stacks - 1)
						{
							vertices.Add(createVertex((i + 1) / slices, (j + 1) / stacks));
						}
						vertices.Add(createVertex(i / slices, (j + 1) / stacks));

						polygons.Add(new Polygon(vertices, 0));
					}
				}
				return new Solid(polygons);
			}
			/// <summary>
			/// Creates a cylinder.
			/// </summary>
			/// <param name="startVector">
			/// Optional vector that defines position of one and of the ends of the cylinder.
			/// </param>
			/// <param name="endVector">  
			/// Optional vector that defines position of one and of the ends of the cylinder.
			/// </param>
			/// <param name="radius">     Radius of the cylinder.</param>
			/// <param name="slices">     Tesselation parameter.</param>
			/// <returns>A new cylinder that can participate in CSG operations.</returns>
			public static Solid Cylinder(Vector3? startVector = null, Vector3? endVector = null, float radius = 1.0f,
										 float slices = 16.0f)
			{
				Vector3 start = startVector ?? new Vector3(0, -1, 0);
				Vector3 end = endVector ?? new Vector3(0, 1, 0);

				Vector3 ray = end - start;

				Vector3 axisZ = ray.Normalized;
				bool isY = Math.Abs(axisZ.Y) > 0.5;
				Vector3 axisX = (new Vector3(Convert.ToInt32(isY), Convert.ToInt32(!isY), 0) % axisZ).Normalized;
				Vector3 axisY = (axisX % axisZ).Normalized;

				Vertex startVertex = new Vertex { Position = start, Normal = axisZ.Flipped };
				Vertex endVertex = new Vertex { Position = end, Normal = axisZ };

				List<Polygon> polygons = new List<Polygon>();
				// Creates a vertex for a cylinder.
				Func<float, float, float, Vertex> createPoint =
					delegate(float stack, float slice, float normalBlend)
					{
						float angle = (float)(slice * Math.PI * 2);
						Vector3 outPoint = axisX * (float)Math.Cos(angle) + axisY * (float)Math.Sin(angle);
						Vector3 position = start + ray * stack + outPoint * radius;
						Vector3 normal = outPoint * (1 - Math.Abs(normalBlend)) + axisZ * normalBlend;
						return new Vertex { Position = position, Normal = normal };
					};
				// Create polygons.
				for (int i = 0; i < slices; i++)
				{
					float t0 = i / slices;
					float t1 = (i + 1) / slices;

					polygons.Add
					(
						new Polygon
						(
							new[]
							{
								startVertex,
								createPoint(0, t0, -1),
								createPoint(0, t1, -1)
							},
							0
						)
					);
					polygons.Add
					(
						new Polygon
						(
							new[]
							{
								createPoint(0, t1, 0),
								createPoint(0, t0, 0),
								createPoint(1, t0, 0),
								createPoint(1, t1, 0)
							},
							0
						)
					);
					polygons.Add
					(
						new Polygon
						(
							new[]
							{
								endVertex,
								createPoint(1, t1, 1),
								createPoint(1, t0, 1)
							},
							0
						)
					);
				}
				return new Solid(polygons);
			}
			/// <summary>
			/// Combines two solids together.
			/// </summary>
			/// <example>
			/// <code>
			/// Solid twoCubes = cube1 | cube2;
			/// </code></example>
			/// <param name="left"> Left operand.</param>
			/// <param name="right">Right operand.</param>
			/// <returns>Result of <see cref="ConstructiveSolidGeometry.Union"/> .</returns>
			public static Solid operator |(Solid left, Solid right)
			{
				return ConstructiveSolidGeometry.Union(left, right);
			}
			/// <summary>
			/// Combines two solids together.
			/// </summary>
			/// <example>
			/// <code>
			/// Solid intersection = cube1 &amp; cube2;
			/// </code></example>
			/// <param name="left"> Left operand.</param>
			/// <param name="right">Right operand.</param>
			/// <returns>Result of <see cref="ConstructiveSolidGeometry.Intersection"/> .</returns>
			public static Solid operator &(Solid left, Solid right)
			{
				return ConstructiveSolidGeometry.Intersection(left, right);
			}
			/// <summary>
			/// Subtracts right solid from left one.
			/// </summary>
			/// <example>
			/// <code>
			/// Solid rightWithoutLeft = cube1 - cube2;
			/// </code></example>
			/// <param name="left"> Left operand.</param>
			/// <param name="right">Right operand.</param>
			/// <returns>Result of <see cref="ConstructiveSolidGeometry.Subtract"/> .</returns>
			public static Solid operator -(Solid left, Solid right)
			{
				return ConstructiveSolidGeometry.Subtract(left, right);
			}
		}
		/// <summary>
		/// Represents a part of surface of the solid.
		/// </summary>
		public class Polygon
		{
			/// <summary>
			/// A list of vertices that define the outline of the polygon.
			/// </summary>
			public List<Vertex> Vertices;
			/// <summary>
			/// An identifier that can be shared between multiple polygons and associated with some
			/// property (like surface color, or subset index).
			/// </summary>
			public int Shared;
			/// <summary>
			/// Plane this polygon lies on.
			/// </summary>
			public Plane Plane;
			/// <summary>
			/// Creates a clone of this polygon.
			/// </summary>
			public Polygon Clone
			{
				get
				{
					return new Polygon
					{
						Vertices = new List<Vertex>(this.Vertices.Select(x => x.Clone)),
						Plane = this.Plane.Clone,
						Shared = this.Shared
					};
				}
			}
			internal Polygon()
			{
				this.Vertices = null;
				this.Shared = -1;
				this.Plane = null;
			}
			/// <summary>
			/// Constructs a new polygon.
			/// </summary>
			/// <remarks>A plane is created using first three vertices.</remarks>
			/// <param name="vertices">A list of vertices that define the outline of the polygon.</param>
			/// <param name="shared">  An identifier of shared properties.</param>
			public Polygon(IList<Vertex> vertices, int shared)
			{
				if (vertices == null || vertices.Count < 3)
				{
					throw new ArgumentException("Not enough vertices.");
				}
#if DEBUG
				if (vertices.Count == 3)			// No need to check triangles.
				{
					// Check if all vertices are on the same plane.
					List<Vector3> edgeVectors = new List<Vector3>(vertices.Count);
					for (int i = 1; i < vertices.Count; i++)
					{
						edgeVectors.Insert(i - 1, vertices[i].Position - vertices[i - 1].Position);
					}
					// Insert last edge.
					edgeVectors.Add(vertices[0].Position - vertices[vertices.Count - 1].Position);
					// Check if all edges are coplanar.
					for (int i = 2; i < edgeVectors.Count; i++)
					{
						if (Vector3.Mixed(edgeVectors[i], edgeVectors[i - 1], edgeVectors[i - 2]) > MathHelpers.ZeroTolerance)
						{
							throw new ArgumentException("Vertices are not coplanar.");
						}
					}
				}
#endif
				this.Plane = new Plane(vertices[0].Position, vertices[1].Position, vertices[2].Position);
#if DEBUG
				Plane.PlaneRelativePositionClass position = Plane.PlaneRelativePositionClass.Coplanar;
				// Check if the polygon is convex.
				for (int i = 0; i < vertices.Count; i++)
				{
					Vector3 previousVertex = vertices[(i == 0) ? vertices.Count - 1 : i - 1].Position;
					Vector3 currentVertex = vertices[i].Position;
					Vector3 nextVertex = vertices[(i + 1) % vertices.Count].Position;
					// Plane formed by edge between previous vertex and current one and a vector
					// that is parallel to polygon's normal and originates from current vertex.
					ConstructiveSolidGeometry.Plane planeBefore =
						new Plane(currentVertex, previousVertex, this.Plane.Normal + currentVertex);

					position |= planeBefore.RelativePosition(nextVertex);
					if (position == Plane.PlaneRelativePositionClass.Spanning)
					{
						throw new ArgumentException("Given list of vertices does not form a convex polygon.");
					}
				}
#endif
				this.Vertices = vertices.ToList();
				this.Shared = shared;
			}
			/// <summary>
			/// Flips this polygon.
			/// </summary>
			public void Flip()
			{
				this.Vertices.Reverse();
				this.Vertices.ForEach(x => x.Flip());
				this.Plane.Flip();
			}
		}
		/// <summary>
		/// Represents a vertex of the solid.
		/// </summary>
		public class Vertex
		{
			public Vector3 Position;
			public Vector3 Normal;
			/// <summary>
			/// Clones this vertex.
			/// </summary>
			/// <returns>New <see cref="Vertex"/> that is a copy of this one.</returns>
			public Vertex Clone
			{
				get
				{
					return new Vertex
					{
						Position = this.Position,
						Normal = this.Normal
					};
				}
			}
			/// <summary>
			/// Flips the normal of this vertex.
			/// </summary>
			public void Flip()
			{
				this.Normal = this.Normal.Flipped;
			}
			/// <summary>
			/// Creates a vertex that is a linear interpolation between this and another vertex.
			/// </summary>
			/// <param name="other">Another vertex.</param>
			/// <param name="t">    Interpolation value.</param>
			public Vertex Interpolate(Vertex other, float t)
			{
				return new Vertex
				{
					Position = Vector3.CreateLinearInterpolation(this.Position, other.Position, t),
					Normal = Vector3.CreateLinearInterpolation(this.Normal, other.Normal, t)
				};
			}
		}
		/// <summary>
		/// Represents a plane.
		/// </summary>
		public class Plane
		{
			/// <summary>
			/// Tolerance value that is used to determine whether point is on plane.
			/// </summary>
			public const float Epsilon = 1e-5f;
			/// <summary>
			/// Normal to this plane. Also defines the front side of the plane.
			/// </summary>
			public Vector3 Normal;
			/// <summary>
			/// Fourth value in a Cartesian equation of the plane.
			/// </summary>
			public float W;
			/// <summary>
			/// Gets a flipped version of this plane.
			/// </summary>
			public Plane Flipped
			{
				get
				{
					return new Plane
					{
						Normal = -this.Normal,
						W = -this.W
					};
				}
			}
			/// <summary>
			/// Gets a deep copy of this plane.
			/// </summary>
			public Plane Clone
			{
				get
				{
					return new Plane
					{
						Normal = this.Normal,
						W = this.W
					};
				}
			}
			/// <summary>
			/// Initializes default instance of this class.
			/// </summary>
			/// <remarks>
			/// Normal is initialized to <see cref="Vector3.Forward"/>.
			///
			/// W is initialized to 0.
			/// </remarks>
			public Plane()
			{
				this.Normal = Vector3.Forward;
				this.W = 0;
			}
			/// <summary>
			/// Creates a plane from three points.
			/// </summary>
			/// <param name="v1">First point.</param>
			/// <param name="v2">Second point.</param>
			/// <param name="v3">Third point.</param>
			public Plane(Vector3 v1, Vector3 v2, Vector3 v3)
			{
				this.Normal = ((v2 - v1) % (v3 - v1)).Normalized;
				this.W = this.Normal * v1;
			}
			/// <summary>
			/// Flips this plane.
			/// </summary>
			public void Flip()
			{
				this.Normal = -this.Normal;
				this.W = -this.W;
			}
			/// <summary>
			/// Determines position of given point relative to this plane.
			/// </summary>
			/// <param name="point"><see cref="Vector3"/> that represents a point.</param>
			/// <returns>
			/// <para><see cref="PlaneRelativePositionClass.Back"/> if the point is behind this plane.</para>
			///
			/// <para><see cref="PlaneRelativePositionClass.Front"/> if the point is in front of this plane.</para>
			///
			/// <para><see cref="PlaneRelativePositionClass.Coplanar"/> if the point is on this plane.</para>
			/// </returns>
			public PlaneRelativePositionClass RelativePosition(Vector3 point)
			{
				float signedDistance = this.Normal * point - this.W;
				return
					signedDistance < -Plane.Epsilon
						? PlaneRelativePositionClass.Back
						: signedDistance > Plane.Epsilon
							? PlaneRelativePositionClass.Front
							: PlaneRelativePositionClass.Coplanar;
			}
			/// <summary>
			/// Splits polygon using this plane.
			/// </summary>
			/// <param name="polygon">              Polygon to slice.</param>
			/// <param name="frontCoplanarPolygons">
			/// The list that is used for storing the <paramref name="polygon"/> it is located on a
			/// plane and its normal points to front side of the plane.
			/// </param>
			/// <param name="backCoplanarPolygons"> 
			/// The list that is used for storing the <paramref name="polygon"/> it is located on a
			/// plane and its normal points to front side of the plane.
			/// </param>
			/// <param name="frontPolygons">        
			/// The list that is used for storing the polygons that don't intersect this plane but
			/// located in front of it.
			/// </param>
			/// <param name="backPolygons">         
			/// The list that is used for storing the polygons that don't intersect this plane but
			/// located behind it.
			/// </param>
			public void SplitPolygon(Polygon polygon,
									 ref List<Polygon> frontCoplanarPolygons,
									 ref List<Polygon> backCoplanarPolygons,
									 ref List<Polygon> frontPolygons,
									 ref List<Polygon> backPolygons)
			{
				// Initialize lists.
				frontCoplanarPolygons = frontCoplanarPolygons ?? new List<Polygon>();
				backCoplanarPolygons = backCoplanarPolygons ?? new List<Polygon>();
				frontPolygons = frontPolygons ?? new List<Polygon>();
				backPolygons = backPolygons ?? new List<Polygon>();
				// Classify each point of the polygon and itself as well into one of the above classes.
				PlaneRelativePositionClass polygonType = 0;
				List<PlaneRelativePositionClass> types =
					new List<PlaneRelativePositionClass>(polygon.Vertices.Count);
				for (int i = 0; i < polygon.Vertices.Count; i++)
				{
					PlaneRelativePositionClass type = this.RelativePosition(polygon.Vertices[i].Position);
					polygonType |= type;
					types.Add(type);
				}
				switch (polygonType)
				{
					case PlaneRelativePositionClass.Coplanar:
						// Just assign the polygon to either back or front coplanar.
						(
							(this.Normal * polygon.Plane.Normal > 0)
							?
							frontCoplanarPolygons
							:
							backCoplanarPolygons
						).Add(polygon);
						return;
					case PlaneRelativePositionClass.Front:
						frontPolygons.Add(polygon);
						return;
					case PlaneRelativePositionClass.Back:
						backPolygons.Add(polygon);
						return;
					case PlaneRelativePositionClass.Spanning:
						// Here comes the hard part.
						List<ConstructiveSolidGeometry.Vertex> f = new List<Vertex>();
						List<ConstructiveSolidGeometry.Vertex> b = new List<Vertex>();
						for (int i = 0; i < polygon.Vertices.Count; i++)
						{
							// Calculate index of the next vertex of the edge. Modulus operation
							// wraps the index to the beginning of the list, when i - is the index
							// of the last vertex.
							int j = (i + 1) % polygon.Vertices.Count;
							// Get the values that indicate position of the edge relative to this plane.
							PlaneRelativePositionClass edgeStartType = types[i];
							PlaneRelativePositionClass edgeEndType = types[j];
							// Get the vertices that form the edge.
							Vertex edgeStartVertex = polygon.Vertices[i];
							Vertex edgeEndVertex = polygon.Vertices[j];
							// Process the edge.
							if (edgeStartType != PlaneRelativePositionClass.Back)
							{
								// Put the start of the edge into the list of front vertices.
								f.Add(edgeStartVertex);
							}
							if (edgeStartType != PlaneRelativePositionClass.Front)
							{
								// Put the start of the edge into the list of back vertices. Clone
								// it if it is on this plane.
								b.Add
								(
									(edgeStartType != PlaneRelativePositionClass.Back)
										? edgeStartVertex.Clone
										: edgeStartVertex
								);
							}
							if ((edgeStartType | edgeEndType) == PlaneRelativePositionClass.Spanning)
							{
								// Split the edge.
								float positionParameter =
									(this.W - this.Normal * edgeStartVertex.Position)
									/
									(this.Normal * (edgeEndVertex.Position - edgeStartVertex.Position));
								// Calculate position of the splitting vertex using the parameter.
								Vertex splittingVertex =
									edgeStartVertex.Interpolate(edgeEndVertex, positionParameter);
								// Put it into front vertices and its clone into back vertices.
								f.Add(splittingVertex);
								b.Add(splittingVertex.Clone);
							}
							// Create new polygons from front and back vertices if they have enough.
							if (f.Count >= 3)
							{
								frontPolygons.Add(new Polygon(f.ToArray(), polygon.Shared));
							}
							if (b.Count >= 3)
							{
								backPolygons.Add(new Polygon(b.ToArray(), polygon.Shared));
							}
						}
						return;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			/// <summary>
			/// Enumeration of classes points and polygons can be classified into when determining
			/// their position relative to the plane.
			/// </summary>
			[Flags]
			public enum PlaneRelativePositionClass
			{
				/// <summary>
				/// Object lies on a plane.
				/// </summary>
				Coplanar = 0,
				/// <summary>
				/// Object does not intersect the plane and vector that describes the shortest
				/// distance between the object and a plane is opposite to plane's normal.
				/// </summary>
				Front = 1,
				/// <summary>
				/// Object does not intersect the plane and vector that describes the shortest
				/// distance between the object and a plane is co-directional to plane's normal.
				/// </summary>
				Back = 2,
				/// <summary>
				/// Object intersects the plane.
				/// </summary>
				Spanning = 3
			}
		}
		/// <summary>
		/// Represents a BSP tree node.
		/// </summary>
		public class Node
		{
			/// <summary>
			/// Plane that divides the BSP tree at this node.
			/// </summary>
			public Plane Plane;
			/// <summary>
			/// Reference to the <see cref="Node"/> that is located in front of this one.
			/// </summary>
			public Node Front;
			/// <summary>
			/// Reference to the <see cref="Node"/> that is located behind this one.
			/// </summary>
			public Node Back;
			/// <summary>
			/// A list of polygons that are located on the <see cref="Node.Plane"/> .
			/// </summary>
			public List<Polygon> Polygons;
			/// <summary>
			/// Gets the list of all polygons in this part of the BSP tree.
			/// </summary>
			public List<Polygon> AllPolygons
			{
				get
				{
					if (this.Polygons == null)
					{
						return null;
					}
					List<Polygon> frontPolygons;
					List<Polygon> backPolygons;

					int frontPolycount;
					int backPolycount;
					// Initialize lists of polygons from branches, so we can minimize memory
					// allocations, since we will have exact capacity immediately.
					if (this.Front != null)
					{
						frontPolygons = this.Front.AllPolygons;
						frontPolycount = frontPolygons.Count;
					}
					else
					{
						frontPolygons = null;
						frontPolycount = 0;
					}
					if (this.Back != null)
					{
						backPolygons = this.Back.AllPolygons;
						backPolycount = backPolygons.Count;
					}
					else
					{
						backPolygons = null;
						backPolycount = 0;
					}
					// Make a list that fits all polygons from this tree.
					List<Polygon> allPolygons = new List<Polygon>
					(
						this.Polygons.Count + frontPolycount + backPolycount
					);
					allPolygons.AddRange(this.Polygons);
					if (frontPolygons != null) allPolygons.AddRange(frontPolygons);
					if (backPolygons != null) allPolygons.AddRange(backPolygons);
					return allPolygons;
				}
			}
			/// <summary>
			/// Creates a deep copy of this BSP tree.
			/// </summary>
			public Node Clone
			{
				get
				{
					return new Node(null)
					{
						Plane = (this.Plane == null) ? null : this.Plane.Clone,
						Front = (this.Front == null) ? null : this.Front.Clone,
						Back = (this.Back == null) ? null : this.Back.Clone,
						Polygons =
							(this.Polygons == null) ? null : new List<Polygon>(this.Polygons.Select(x => x.Clone))
					};
				}
			}
			/// <summary>
			/// Builds this <see cref="Node"/> from the list of polygons.
			/// </summary>
			/// <param name="polygons">A list of polygons to build the new node from.</param>
			public Node(List<Polygon> polygons)
			{
				this.Plane = null;
				this.Front = null;
				this.Back = null;
				this.Polygons = new List<Polygon>();
				if (polygons != null && polygons.Count > 0)
				{
					this.Build(polygons);
				}
			}
			/// <summary>
			/// Builds a part of BSP tree that has this node at root from the list of polygons.
			/// </summary>
			/// <remarks>
			/// When called on an existing tree, the new polygons are filtered down to the bottom of
			/// the tree and become new nodes there. Each set of polygons is partitioned using the
			/// first polygon (no heuristic is used to pick a good split).
			/// </remarks>
			/// <param name="polygons">A list of polygons to build the BSP tree from.</param>
			public void Build(List<Polygon> polygons)
			{
				if (polygons == null || polygons.Count == 0)
				{
					return;
				}
				if (this.Plane == null)
				{
					this.Plane = polygons[0].Plane.Clone;
				}
				// Polygons in these lists will form branches from this node.
				List<Polygon> frontPolygons = new List<Polygon>();
				List<Polygon> backPolygons = new List<Polygon>();
				// Split polygons with the plane.
				for (int i = 0; i < polygons.Count; i++)
				{
					this.Plane.SplitPolygon
					(
						polygons[i],
						// Polys that are on the same plane as this node end up in the node.
						ref this.Polygons, ref this.Polygons,
						// These will form front and back branches resectively.
						ref frontPolygons, ref backPolygons
					);
				}
				// Build front branch from front polygons.
				if (frontPolygons.Count > 0)
				{
					this.Front = new Node(frontPolygons);
				}
				// Build back branch from back polygons.
				if (backPolygons.Count > 0)
				{
					this.Back = new Node(backPolygons);
				}
			}
			/// <summary>
			/// Flips this BSP tree.
			/// </summary>
			public void Invert()
			{
				if (this.Plane != null)
				{
					this.Plane.Flip();
				}
				if (this.Polygons != null && this.Polygons.Count != 0)
				{
					this.Polygons.ForEach(x => x.Flip());
				}
				if (this.Front != null)
				{
					this.Front.Invert();
				}
				if (this.Back != null)
				{
					this.Back.Invert();
				}
				Node temp = this.Front;
				this.Front = this.Back;
				this.Back = temp;
			}
			/// <summary>
			/// Filters all polygons from given list that are inside this BSP tree.
			/// </summary>
			/// <param name="polygons">A list of polygons to filter.</param>
			/// <returns>A list with filtration done.</returns>
			public List<Polygon> ClipPolygons(List<Polygon> polygons)
			{
				if (this.Plane == null)
				{
					return new List<Polygon>(polygons);
				}
				List<Polygon> front = new List<Polygon>();
				List<Polygon> back = new List<Polygon>();
				for (int i = 0; i < polygons.Count; i++)
				{
					this.Plane.SplitPolygon(polygons[i], ref front, ref back, ref front, ref back);
				}
				if (this.Front != null)
				{
					front = this.Front.ClipPolygons(front);
				}
				// If this node has nothing behind it in the tree, then whatever is behind it in the
				// list should be discarded.
				if (this.Back != null)
				{
					front.AddRange(this.Back.ClipPolygons(back));
				}
				return front;
			}
			/// <summary>
			/// Removes all geometry in this BSP tree that is in another one.
			/// </summary>
			/// <param name="node">Root of another BSP tree.</param>
			public void ClipTo(Node node)
			{
				this.Polygons = node.ClipPolygons(this.Polygons);
				if (this.Front != null) this.Front.ClipTo(node);
				if (this.Back != null) this.Back.ClipTo(node);
			}
		}
	}
}