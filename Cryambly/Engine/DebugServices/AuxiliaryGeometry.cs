using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Geometry;
using CryCil.Graphics;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Provides access to CryEngine auxiliary geometry rendering API.
	/// </summary>
	public static class AuxiliaryGeometry
	{
		#region Properties
		/// <summary>
		/// Gets or sets flags that specify rendering auxiliary geometry objects.
		/// </summary>
		/// <remarks>Only public flags are available for getting/setting.</remarks>
		public static extern AuxiliaryGeometryRenderFlags Flags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Draws a point.
		/// </summary>
		/// <param name="position"> Location of the point.</param>
		/// <param name="color">    Color of the point.</param>
		/// <param name="thickness">Thickness (size) of the point.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawPoint(Vector3 position, ColorByte color, byte thickness = 1);
		/// <summary>
		/// Draws a sequence of points using the same color.
		/// </summary>
		/// <param name="positions">An array of location of points.</param>
		/// <param name="color">    Color to use to draw the points.</param>
		/// <param name="thickness">Thickness of all points.</param>
		/// <exception cref="ArgumentNullException">Array of point positions cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawPoints(Vector3[] positions, ColorByte color, byte thickness = 1);
		/// <summary>
		/// Draws a sequence of points.
		/// </summary>
		/// <param name="positions">An array of location of points.</param>
		/// <param name="colors">   An array of colors of points.</param>
		/// <param name="thickness">Thickness of all points.</param>
		/// <exception cref="ArgumentNullException">Array of point positions cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of point colors cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Number of render components is not equal to number of colors.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawPoints(Vector3[] positions, ColorByte[] colors, byte thickness = 1);
		/// <summary>
		/// Draws a line.
		/// </summary>
		/// <param name="start">    First point of the line.</param>
		/// <param name="end">      Second point of the line.</param>
		/// <param name="color">    Color of the line.</param>
		/// <param name="thickness">Thickness of the line.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawLine(Vector3 start, Vector3 end, ColorByte color,
										   float thickness = 1.0f);
		/// <summary>
		/// Draws a line with a color gradient between the ends.
		/// </summary>
		/// <param name="start">     First point of the line.</param>
		/// <param name="colorStart">First color in the gradient.</param>
		/// <param name="end">       Second point of the line.</param>
		/// <param name="colorEnd">  Second color in the gradient.</param>
		/// <param name="thickness"> Thickness of the line.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawLine(Vector3 start, ColorByte colorStart, Vector3 end,
										   ColorByte colorEnd, float thickness = 1.0f);
		/// <summary>
		/// Draws a set of separate lines.
		/// </summary>
		/// <param name="vertexes"> 
		/// An array of pairs of vertexes. Must contain even number of elements.
		/// </param>
		/// <param name="color">    Color to use for the lines.</param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawLines(Vector3[] vertexes, ColorByte color, float thickness = 1.0f);
		/// <summary>
		/// Draws a set of separate lines.
		/// </summary>
		/// <param name="vertexes"> 
		/// An array of pairs of vertexes. Must contain even number of elements.
		/// </param>
		/// <param name="colors">   An array of colors to use for the vertexes.</param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of vertex colors cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Number of render components is not equal to number of colors.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawLines(Vector3[] vertexes, ColorByte[] colors, float thickness = 1.0f);
		/// <summary>
		/// Draws a set of lines.
		/// </summary>
		/// <param name="vertexes"> An array of vectors that defines a pool of vertexes.</param>
		/// <param name="indexes">  
		/// An array of pairs of indexes of vectors from the pool that form the lines.
		/// </param>
		/// <param name="color">    Color of the lines.</param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of indexes cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawLines(Vector3[] vertexes, uint[] indexes, ColorByte color,
											float thickness = 1.0f);
		/// <summary>
		/// Draws a set of lines.
		/// </summary>
		/// <param name="vertexes"> An array of vectors that defines a pool of vertexes.</param>
		/// <param name="indexes">  
		/// An array of pairs of indexes of vectors from the pool that form the lines.
		/// </param>
		/// <param name="colors">   An array of colors to use for the vertexes.</param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of colors cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Number of render components is not equal to number of colors.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawLines(Vector3[] vertexes, uint[] indexes, ColorByte[] colors,
											float thickness = 1.0f);
		/// <summary>
		/// Draws a polyline.
		/// </summary>
		/// <param name="vertexes"> An array of vertexes that comprise the polyline.</param>
		/// <param name="closed">   Indicates whether last and first vertex should connected.</param>
		/// <param name="color">    Color of the polyline.</param>
		/// <param name="thickness">Thickness of the polyline.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawPolyline(Vector3[] vertexes, bool closed, ColorByte color,
											float thickness = 1.0f);
		/// <summary>
		/// Draws a polyline.
		/// </summary>
		/// <param name="vertexes"> An array of vertexes that comprise the polyline.</param>
		/// <param name="closed">   Indicates whether last and first vertex should connected.</param>
		/// <param name="colors">   An array of vertex colors.</param>
		/// <param name="thickness">Thickness of the polyline.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of colors cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Number of render components is not equal to number of colors.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawPolyline(Vector3[] vertexes, bool closed, ColorByte[] colors,
											float thickness = 1.0f);
		/// <summary>
		/// Draws a triangle.
		/// </summary>
		/// <param name="first">      First vertex of the triangle.</param>
		/// <param name="firstColor"> Color of the first vertex of the triangle.</param>
		/// <param name="second">     Second vertex of the triangle.</param>
		/// <param name="secondColor">Color of the second vertex of the triangle.</param>
		/// <param name="third">      Third vertex of the triangle.</param>
		/// <param name="thirdColor"> Color of the third vertex of the triangle.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawTriangle(Vector3 first, ColorByte firstColor,
											   Vector3 second, ColorByte secondColor,
											   Vector3 third, ColorByte thirdColor);
		/// <summary>
		/// Draws a polygon comprised by triangles.
		/// </summary>
		/// <param name="vertexes">An array of vertexes that comprise the outline of the polygon.</param>
		/// <param name="color">   Color of the polygon.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawTriangles(Vector3[] vertexes, ColorByte color);
		/// <summary>
		/// Draws a polygon comprised by triangles.
		/// </summary>
		/// <param name="vertexes">An array of vertexes that comprise the outline of the polygon.</param>
		/// <param name="colors">  An array of colors of all vertexes.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of colors cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Number of render components is not equal to number of colors.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawTriangles(Vector3[] vertexes, ColorByte[] colors);
		/// <summary>
		/// Draws a set of triangles.
		/// </summary>
		/// <param name="vertexes">An array of vectors that defines pool of vertexes.</param>
		/// <param name="indexes"> An array of triads of vertex indices that compise the set.</param>
		/// <param name="color">   Color of all vertexes.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of indexes cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawTriangles(Vector3[] vertexes, uint[] indexes, ColorByte color);
		/// <summary>
		/// Draws a set of triangles.
		/// </summary>
		/// <param name="vertexes">An array of vectors that defines pool of vertexes.</param>
		/// <param name="indexes"> An array of triads of vertex indices that compise the set.</param>
		/// <param name="colors">  An array of colors of all vertexes.</param>
		/// <exception cref="ArgumentNullException">Array of vertexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of indexes cannot be null.</exception>
		/// <exception cref="ArgumentNullException">Array of colors cannot be null.</exception>
		/// <exception cref="ArgumentException">
		/// Number of render components is not equal to number of colors.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawTriangles(Vector3[] vertexes, uint[] indexes, ColorByte[] colors);
		/// <summary>
		/// Draws an axis-aligned bounding box.
		/// </summary>
		/// <param name="box">  
		/// An object of type <see cref="BoundingBox"/> that describes the bounding box.
		/// </param>
		/// <param name="solid">Indicates whether sides of the box should be rendered as solids.</param>
		/// <param name="color">Color of the box.</param>
		/// <param name="style">Style of rendering the box.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawAABB(ref BoundingBox box, bool solid, ColorByte color,
										   BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted);
		/// <summary>
		/// Draws an axis-aligned bounding box.
		/// </summary>
		/// <param name="box">  
		/// An object of type <see cref="BoundingBox"/> that describes the bounding box.
		/// </param>
		/// <param name="mat">  
		/// <see cref="Matrix34"/> object that describes location and orientation of axes the box is
		/// aligned along.
		/// </param>
		/// <param name="solid">Indicates whether sides of the box should be rendered as solids.</param>
		/// <param name="color">Color of the box.</param>
		/// <param name="style">Style of rendering the box.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawAABB(ref BoundingBox box, ref Matrix34 mat, bool solid, ColorByte color,
										   BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted);
		/// <summary>
		/// Draws a set of axis-aligned bounding boxes.
		/// </summary>
		/// <param name="boxes">An array of bounding boxes.</param>
		/// <param name="solid">Indicates whether sides of all boxes should be rendered as solids.</param>
		/// <param name="color">Color of all boxes.</param>
		/// <param name="style">Style of rendering the boxes.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawAABBs(BoundingBox[] boxes, bool solid, ColorByte color,
										   BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted);
		/// <summary>
		/// Draws an oriented bounding box.
		/// </summary>
		/// <param name="box">  
		/// An object of type <see cref="OBB"/> that describes the bounding box.
		/// </param>
		/// <param name="solid">Indicates whether sides of all boxes should be rendered as solids.</param>
		/// <param name="color">Color of the box.</param>
		/// <param name="style">Style of rendering the box.</param>
		public static void DrawOBB(ref OBB box, bool solid, ColorByte color,
								   BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted)
		{
			DrawOBB(ref box, Vector3.Zero, solid, color, style);
		}
		/// <summary>
		/// Draws an oriented bounding box.
		/// </summary>
		/// <param name="box">        
		/// An object of type <see cref="OBB"/> that describes the bounding box.
		/// </param>
		/// <param name="translation">Extra translation to apply to the box.</param>
		/// <param name="solid">      
		/// Indicates whether sides of all boxes should be rendered as solids.
		/// </param>
		/// <param name="color">      Color of the box.</param>
		/// <param name="style">      Style of rendering the box.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawOBB(ref OBB box, Vector3 translation, bool solid, ColorByte color,
										  BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted);
		/// <summary>
		/// Draws an oriented bounding box.
		/// </summary>
		/// <param name="box">     
		/// An object of type <see cref="OBB"/> that describes the bounding box.
		/// </param>
		/// <param name="matWorld">Extra transformation to apply to the box.</param>
		/// <param name="solid">   
		/// Indicates whether sides of all boxes should be rendered as solids.
		/// </param>
		/// <param name="color">   Color of the box.</param>
		/// <param name="style">   Style of rendering the box.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawOBB(ref OBB box, ref Matrix34 matWorld, bool solid, ColorByte color,
										  BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted);
		/// <summary>
		/// Draws a sphere.
		/// </summary>
		/// <param name="center">Location of the center of the sphere.</param>
		/// <param name="radius">Radius of the sphere.</param>
		/// <param name="color"> Color of the sphere.</param>
		/// <param name="shaded">Indicates the the sphere is shaded.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawSphere(Vector3 center, float radius, ColorByte color, bool shaded = true);
		/// <summary>
		/// Draws a cone.
		/// </summary>
		/// <param name="center">   Location of the center of the cone's base.</param>
		/// <param name="direction">Direction the cone is pointing at.</param>
		/// <param name="radius">   Radius of the cone's base.</param>
		/// <param name="height">   Height of the cone.</param>
		/// <param name="color">    Color of the cone.</param>
		/// <param name="shaded">   Indicates the the cone is shaded.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawCone(Vector3 center, Vector3 direction, float radius,
											   float height, ColorByte color, bool shaded = true);
		/// <summary>
		/// Draws a cylinder.
		/// </summary>
		/// <param name="center">   Location of the center of the cylinder's base.</param>
		/// <param name="direction">Direction the cylinder is pointing at.</param>
		/// <param name="radius">   Radius of the cylinder's base.</param>
		/// <param name="height">   Height of the cylinder.</param>
		/// <param name="color">    Color of the cylinder.</param>
		/// <param name="shaded">   Indicates the the cylinder is shaded.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawCylinder(Vector3 center, Vector3 direction, float radius,
											   float height, ColorByte color, bool shaded = true);
		/// <summary>
		/// Draws a character animation skeleton bone.
		/// </summary>
		/// <param name="parent">Location of the parent bone.</param>
		/// <param name="bone">  Location of this bone.</param>
		/// <param name="color"> Color of the bone.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DrawBone(Vector3 parent, Vector3 bone, ColorByte color);
		/// <summary>
		/// Flushes the internal buffer of objects forcing them to appear on the screen.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Flush();
		#endregion
	}
}