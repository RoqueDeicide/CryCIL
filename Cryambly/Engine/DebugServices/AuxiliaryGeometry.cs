using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CryCil.Geometry;
using CryCil.Graphics;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Provides access to CryEngine auxiliary geometry rendering API.
	/// </summary>
	public static unsafe class AuxiliaryGeometry
	{
		#region Properties
		/// <summary>
		/// Gets or sets flags that specify rendering auxiliary geometry objects.
		/// </summary>
		/// <remarks>Only public flags are available for getting/setting.</remarks>
		public static AuxiliaryGeometryRenderFlags Flags
		{
			get { return GetFlags(); }
			set { SetFlags(value); }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Draws a point.
		/// </summary>
		/// <param name="position"> Location of the point.</param>
		/// <param name="color">    Color of the point.</param>
		/// <param name="thickness">Thickness (size) of the point.</param>
		public static void DrawPoint(Vector3 position, ColorByte color, byte thickness = 1)
		{
			DrawPointInternal(position, color, thickness);
		}
		/// <summary>
		/// Draws a sequence of points using the same color.
		/// </summary>
		/// <param name="positions">An array of location of points.</param>
		/// <param name="color">    Color to use to draw the points.</param>
		/// <param name="thickness">Thickness of all points.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public static void DrawPoints(Vector3[] positions, ColorByte color, byte thickness = 1)
		{
			if (positions.IsNullOrEmpty())
			{
				return;
			}

			fixed (Vector3* ptr = positions)
			{
				DrawPointsInternal(ptr, positions.Length, color, thickness);
			}
		}
		/// <summary>
		/// Draws a sequence of points.
		/// </summary>
		/// <param name="positions">
		/// An array of location of points. Points won't drawn if this array is <c>null</c> or empty.
		/// </param>
		/// <param name="colors">   
		/// An array of colors of points. Points won't drawn if this array is <c>null</c> or empty.
		/// </param>
		/// <param name="thickness">Thickness of all points.</param>
		/// <exception cref="ArgumentException">
		/// Number of positions is not equal to number of colors.
		/// </exception>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public static void DrawPoints(Vector3[] positions, ColorByte[] colors, byte thickness = 1)
		{
			if (positions.IsNullOrEmpty() || colors.IsNullOrEmpty())
			{
				return;
			}

			int positionCount = positions.Length;
			int colorCount = colors.Length;

			if (positionCount != colorCount)
			{
				throw new ArgumentException("Number of positions is not equal to number of colors.");
			}

			fixed (Vector3* positionsPtr = positions)
			fixed (ColorByte* colorsPtr = colors)
			{
				DrawPointsColorsInternal(positionsPtr, positionCount, colorsPtr, colorCount, thickness);
			}
		}
		/// <summary>
		/// Draws a line.
		/// </summary>
		/// <param name="start">    First point of the line.</param>
		/// <param name="end">      Second point of the line.</param>
		/// <param name="color">    Color of the line.</param>
		/// <param name="thickness">Thickness of the line.</param>
		public static void DrawLine(Vector3 start, Vector3 end, ColorByte color, float thickness = 1.0f)
		{
			DrawLineInternal(start, end, color, thickness);
		}
		/// <summary>
		/// Draws a line with a color gradient between the ends.
		/// </summary>
		/// <param name="start">     First point of the line.</param>
		/// <param name="colorStart">First color in the gradient.</param>
		/// <param name="end">       Second point of the line.</param>
		/// <param name="colorEnd">  Second color in the gradient.</param>
		/// <param name="thickness"> Thickness of the line.</param>
		public static void DrawLine(Vector3 start, ColorByte colorStart, Vector3 end, ColorByte colorEnd,
									float thickness = 1.0f)
		{
			DrawLineColorsInternal(start, colorStart, end, colorEnd, thickness);
		}
		/// <summary>
		/// Draws a set of separate lines.
		/// </summary>
		/// <param name="vertexes"> 
		/// An array of pairs of vertexes. Must contain even number of elements.
		/// </param>
		/// <param name="color">    Color to use for the lines.</param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Number of vertexes in the array must be an even number.
		/// </exception>
		public static void DrawLines(Vector3[] vertexes, ColorByte color, float thickness = 1.0f)
		{
			if (vertexes.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;

			if (vertexCount % 2 != 0)
			{
				throw new ArgumentException("Number of vertexes in the array must be an even number.");
			}

			fixed (Vector3* vertexPtr = vertexes)
			{
				DrawLinesInternal(vertexPtr, vertexCount, color, thickness);
			}
		}
		/// <summary>
		/// Draws a set of separate lines.
		/// </summary>
		/// <param name="vertexes"> 
		/// An array of pairs of vertexes. Must contain even number of elements. Lines won't be drawn if
		/// this array is <c>null</c> or empty.
		/// </param>
		/// <param name="colors">   
		/// An array of colors to use for the vertexes. Lines won't be drawn if this array is <c>null</c> or
		/// empty.
		/// </param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Number of vertexes in the array must be an even number.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Number vertices must be equal to number of colors.
		/// </exception>
		public static void DrawLines(Vector3[] vertexes, ColorByte[] colors, float thickness = 1.0f)
		{
			if (vertexes.IsNullOrEmpty() || colors.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;
			int colorCount = colors.Length;

			if (vertexCount % 2 != 0)
			{
				throw new ArgumentException("Number of vertexes in the array must be an even number.");
			}
			if (colorCount != vertexCount)
			{
				throw new ArgumentException("Number vertices must be equal to number of colors.");
			}

			fixed (Vector3* vertexPtr = vertexes)
			fixed (ColorByte* colorsPtr = colors)
			{
				DrawLinesColorsInternal(vertexPtr, vertexCount, colorsPtr, colorCount, thickness);
			}
		}
		/// <summary>
		/// Draws a set of lines.
		/// </summary>
		/// <param name="vertexes"> 
		/// An array of vectors that defines a pool of vertexes. Lines won't be drawn if this array is
		/// <c>null</c> or empty.
		/// </param>
		/// <param name="indexes">  
		/// An array of pairs of indexes of vectors from the pool that form the lines. Lines won't be drawn
		/// if this array is <c>null</c> or empty.
		/// </param>
		/// <param name="color">    Color of the lines.</param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">Number of indexes must be even.</exception>
		/// <exception cref="ArgumentException">There are indexes that are out of range.</exception>
		public static void DrawLines(Vector3[] vertexes, uint[] indexes, ColorByte color, float thickness = 1.0f)
		{
			if (vertexes.IsNullOrEmpty() || indexes.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;
			int indexCount = indexes.Length;

			if (indexCount % 2 != 0)
			{
				throw new ArgumentException("Number of indexes must be even.");
			}

#if DEBUG
			ValidateIndexes(indexes, vertexCount, indexCount);
#endif

			fixed (Vector3* vertexPtr = vertexes)
			fixed (uint* indexesPtr = indexes)
			{
				DrawLinesIndexesInternal(vertexPtr, vertexCount, indexesPtr, indexCount, color, thickness);
			}
		}
		/// <summary>
		/// Draws a set of lines.
		/// </summary>
		/// <param name="vertexes"> 
		/// An array of vectors that defines a pool of vertexes. Lines won't be drawn if this array is
		/// <c>null</c> or empty.
		/// </param>
		/// <param name="indexes">  
		/// An array of pairs of indexes of vectors from the pool that form the lines. Lines won't be drawn
		/// if this array is <c>null</c> or empty.
		/// </param>
		/// <param name="colors">   
		/// An array of colors to use for the vertexes. Lines won't be drawn if this array is <c>null</c> or
		/// empty.
		/// </param>
		/// <param name="thickness">Thickness of the lines.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">Number of indexes must be even.</exception>
		/// <exception cref="ArgumentException">
		/// Number of vertexes must be equal to number of colors.
		/// </exception>
		/// <exception cref="ArgumentException">There are indexes that are out of range.</exception>
		public static void DrawLines(Vector3[] vertexes, uint[] indexes, ColorByte[] colors, float thickness = 1.0f)
		{
			if (vertexes.IsNullOrEmpty() || indexes.IsNullOrEmpty() || colors.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;
			int indexCount = indexes.Length;
			int colorCount = colors.Length;

			if (indexCount % 2 != 0)
			{
				throw new ArgumentException("Number of indexes must be even.");
			}
			if (vertexCount != colorCount)
			{
				throw new ArgumentException("Number of vertexes must be equal to number of colors.");
			}

#if DEBUG
			ValidateIndexes(indexes, vertexCount, indexCount);
#endif

			fixed (Vector3* vertexPtr = vertexes)
			fixed (uint* indexesPtr = indexes)
			fixed (ColorByte* colorsPtr = colors)
			{
				DrawLinesIndexesColorsInternal(vertexPtr, vertexCount, indexesPtr, indexCount, colorsPtr, thickness);
			}
		}
		/// <summary>
		/// Draws a polyline.
		/// </summary>
		/// <param name="vertexes"> An array of vertexes that comprise the polyline.</param>
		/// <param name="closed">   Indicates whether last and first vertex should connected.</param>
		/// <param name="color">    Color of the polyline.</param>
		/// <param name="thickness">Thickness of the polyline.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public static void DrawPolyline(Vector3[] vertexes, bool closed, ColorByte color, float thickness = 1.0f)
		{
			if (vertexes.IsNullOrEmpty())
			{
				return;
			}

			fixed (Vector3* vertexesPtr = vertexes)
			{
				DrawPolylineInternal(vertexesPtr, vertexes.Length, closed, color, thickness);
			}
		}
		/// <summary>
		/// Draws a polyline.
		/// </summary>
		/// <param name="vertexes"> 
		/// An array of vertexes that comprise the polyline. Polyline won't be drawn if this array is
		/// <c>null</c> or empty.
		/// </param>
		/// <param name="closed">   Indicates whether last and first vertex should connected.</param>
		/// <param name="colors">   
		/// An array of vertex colors. Polyline won't be drawn if this array is <c>null</c> or empty.
		/// </param>
		/// <param name="thickness">Thickness of the polyline.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Number of vertexes is not equal to number of colors.
		/// </exception>
		public static void DrawPolyline(Vector3[] vertexes, bool closed, ColorByte[] colors, float thickness = 1.0f)
		{
			if (vertexes.IsNullOrEmpty() || colors.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;
			int colorCount = colors.Length;

			if (vertexCount != colorCount)
			{
				throw new ArgumentException("Number of vertexes is not equal to number of colors.");
			}

			fixed (Vector3* vertexesPtr = vertexes)
			fixed (ColorByte* colorsPtr = colors)
			{
				DrawPolylineColorsInternal(vertexesPtr, vertexCount, closed, colorsPtr, thickness);
			}
		}
		/// <summary>
		/// Draws a triangle.
		/// </summary>
		/// <param name="first"> First vertex of the triangle.</param>
		/// <param name="second">Second vertex of the triangle.</param>
		/// <param name="third"> Third vertex of the triangle.</param>
		/// <param name="color"> Color of the triangle.</param>
		public static void DrawTriangle(Vector3 first, Vector3 second, Vector3 third, ColorByte color)
		{
			DrawTriangleInternal(first, color, second, color, third, color);
		}
		/// <summary>
		/// Draws a triangle.
		/// </summary>
		/// <param name="first">      First vertex of the triangle.</param>
		/// <param name="firstColor"> Color of the first vertex of the triangle.</param>
		/// <param name="second">     Second vertex of the triangle.</param>
		/// <param name="secondColor">Color of the second vertex of the triangle.</param>
		/// <param name="third">      Third vertex of the triangle.</param>
		/// <param name="thirdColor"> Color of the third vertex of the triangle.</param>
		public static void DrawTriangle(Vector3 first, ColorByte firstColor, Vector3 second, ColorByte secondColor,
										Vector3 third, ColorByte thirdColor)
		{
			DrawTriangleInternal(first, firstColor, second, secondColor, third, thirdColor);
		}
		/// <summary>
		/// Draws a polygon comprised by triangles.
		/// </summary>
		/// <param name="vertexes">An array of vertexes that comprise the outline of the polygon.</param>
		/// <param name="color">   Color of the polygon.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public static void DrawTriangles(Vector3[] vertexes, ColorByte color)
		{
			if (vertexes.IsNullOrEmpty())
			{
				return;
			}

			fixed (Vector3* vertexesPtr = vertexes)
			{
				DrawTrianglesInternal(vertexesPtr, vertexes.Length, color);
			}
		}
		/// <summary>
		/// Draws a polygon comprised by triangles.
		/// </summary>
		/// <param name="vertexes">
		/// An array of vertexes that comprise the outline of the polygon. Polygon won't be drawn if this
		/// array is <c>null</c> or empty.
		/// </param>
		/// <param name="colors">  
		/// An array of colors of all vertexes. Polygon won't be drawn if this array is <c>null</c> or
		/// empty.
		/// </param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Number of vertexes is not equal to number of colors.
		/// </exception>
		public static void DrawTriangles(Vector3[] vertexes, ColorByte[] colors)
		{
			if (vertexes.IsNullOrEmpty() || colors.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;
			int colorCount = colors.Length;

			if (vertexCount != colorCount)
			{
				throw new ArgumentException("Number of vertexes is not equal to number of colors.");
			}

			fixed (Vector3* vertexesPtr = vertexes)
			fixed (ColorByte* colorsPtr = colors)
			{
				DrawTrianglesColorsInternal(vertexesPtr, vertexCount, colorsPtr, colorCount);
			}
		}
		/// <summary>
		/// Draws a set of triangles.
		/// </summary>
		/// <param name="vertexes">
		/// An array of vectors that defines pool of vertexes. Triangles won't be drawn if this array is
		/// <c>null</c> or empty.
		/// </param>
		/// <param name="indexes"> 
		/// An array of triads of vertex indices that comprise the set. Triangles won't be drawn if this
		/// array is <c>null</c> or empty.
		/// </param>
		/// <param name="color">   Color of all vertexes.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">Number of indexes must be divisible by 3.</exception>
		/// <exception cref="ArgumentException">There are indexes that are out of range.</exception>
		public static void DrawTriangles(Vector3[] vertexes, uint[] indexes, ColorByte color)
		{
			if (vertexes.IsNullOrEmpty() || indexes.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;
			int indexCount = indexes.Length;

			if (indexCount % 3 != 0)
			{
				throw new ArgumentException("Number of indexes must be divisible by 3.");
			}

#if DEBUG
			ValidateIndexes(indexes, vertexCount, indexCount);
#endif

			fixed (Vector3* vertexPtr = vertexes)
			fixed (uint* indexesPtr = indexes)
			{
				DrawTrianglesIndexesInternal(vertexPtr, vertexCount, indexesPtr, indexCount, color);
			}
		}
		/// <summary>
		/// Draws a set of triangles.
		/// </summary>
		/// <param name="vertexes">
		/// An array of vectors that defines pool of vertexes. Triangles won't be drawn if this array is
		/// <c>null</c> or empty.
		/// </param>
		/// <param name="indexes"> 
		/// An array of triads of vertex indices that comprise the set. Triangles won't be drawn if this
		/// array is <c>null</c> or empty.
		/// </param>
		/// <param name="colors">  
		/// An array of colors of all vertexes. Triangles won't be drawn if this array is <c>null</c> or
		/// empty.
		/// </param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="ArgumentException">Number of indexes must be divisible by 3.</exception>
		/// <exception cref="ArgumentException">
		/// Number of vertexes must be equal to number of colors.
		/// </exception>
		/// <exception cref="ArgumentException">There are indexes that are out of range.</exception>
		public static void DrawTriangles(Vector3[] vertexes, uint[] indexes, ColorByte[] colors)
		{
			if (vertexes.IsNullOrEmpty() || indexes.IsNullOrEmpty() || colors.IsNullOrEmpty())
			{
				return;
			}

			int vertexCount = vertexes.Length;
			int indexCount = indexes.Length;
			int colorCount = colors.Length;

			if (indexCount % 3 != 0)
			{
				throw new ArgumentException("Number of indexes must be divisible by 3.");
			}
			if (vertexCount != colorCount)
			{
				throw new ArgumentException("Number of vertexes must be equal to number of colors.");
			}

#if DEBUG
			ValidateIndexes(indexes, vertexCount, indexCount);
#endif

			fixed (Vector3* vertexPtr = vertexes)
			fixed (uint* indexesPtr = indexes)
			fixed (ColorByte* colorsPtr = colors)
			{
				DrawTrianglesIndexesColorsInternal(vertexPtr, vertexCount, indexesPtr, indexCount, colorsPtr);
			}
		}
		/// <summary>
		/// Draws an axis-aligned bounding box.
		/// </summary>
		/// <param name="box">  
		/// An object of type <see cref="BoundingBox"/> that describes the bounding box.
		/// </param>
		/// <param name="solid">Indicates whether sides of the box should be rendered as solids.</param>
		/// <param name="color">Color of the box.</param>
		/// <param name="style">Style of rendering the box.</param>
		public static void DrawAABB(ref BoundingBox box, bool solid, ColorByte color,
									BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted)
		{
			DrawAABBInternal(ref box, solid, color, style);
		}
		/// <summary>
		/// Draws an axis-aligned bounding box.
		/// </summary>
		/// <param name="box">  
		/// An object of type <see cref="BoundingBox"/> that describes the bounding box.
		/// </param>
		/// <param name="mat">  
		/// <see cref="Matrix34"/> object that describes location and orientation of axes the box is aligned
		/// along.
		/// </param>
		/// <param name="solid">Indicates whether sides of the box should be rendered as solids.</param>
		/// <param name="color">Color of the box.</param>
		/// <param name="style">Style of rendering the box.</param>
		public static void DrawAABB(ref BoundingBox box, ref Matrix34 mat, bool solid, ColorByte color,
									BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted)
		{
			DrawAABBMatrixInternal(ref box, ref mat, solid, color, style);
		}
		/// <summary>
		/// Draws a set of axis-aligned bounding boxes.
		/// </summary>
		/// <param name="boxes">An array of bounding boxes.</param>
		/// <param name="solid">Indicates whether sides of all boxes should be rendered as solids.</param>
		/// <param name="color">Color of all boxes.</param>
		/// <param name="style">Style of rendering the boxes.</param>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public static void DrawAABBs(BoundingBox[] boxes, bool solid, ColorByte color,
									 BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted)
		{
			if (boxes.IsNullOrEmpty())
			{
				return;
			}

			fixed (BoundingBox* boxesPtr = boxes)
			{
				DrawAABBsInternal(boxesPtr, boxes.Length, solid, color, style);
			}
		}
		/// <summary>
		/// Draws an oriented bounding box.
		/// </summary>
		/// <param name="box">  An object of type <see cref="OBB"/> that describes the bounding box.</param>
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
		public static void DrawOBB(ref OBB box, Vector3 translation, bool solid, ColorByte color,
								   BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted)
		{
			DrawOBBInternal(ref box, translation, solid, color, style);
		}
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
		public static void DrawOBB(ref OBB box, ref Matrix34 matWorld, bool solid, ColorByte color,
								   BoundingBoxRenderStyle style = BoundingBoxRenderStyle.Faceted)
		{
			DrawOBBMatrixInternal(ref box, ref matWorld, solid, color, style);
		}
		/// <summary>
		/// Draws a sphere.
		/// </summary>
		/// <param name="center">Location of the center of the sphere.</param>
		/// <param name="radius">Radius of the sphere.</param>
		/// <param name="color"> Color of the sphere.</param>
		/// <param name="shaded">Indicates the the sphere is shaded.</param>
		public static void DrawSphere(Vector3 center, float radius, ColorByte color, bool shaded = true)
		{
			DrawSphereInternal(center, radius, color, shaded);
		}
		/// <summary>
		/// Draws a cone.
		/// </summary>
		/// <param name="center">   Location of the center of the cone's base.</param>
		/// <param name="direction">Direction the cone is pointing at.</param>
		/// <param name="radius">   Radius of the cone's base.</param>
		/// <param name="height">   Height of the cone.</param>
		/// <param name="color">    Color of the cone.</param>
		/// <param name="shaded">   Indicates the the cone is shaded.</param>
		public static void DrawCone(Vector3 center, Vector3 direction, float radius, float height, ColorByte color,
									bool shaded = true)
		{
			DrawConeInternal(center, direction, radius, height, color, shaded);
		}
		/// <summary>
		/// Draws a cylinder.
		/// </summary>
		/// <param name="center">   Location of the center of the cylinder's base.</param>
		/// <param name="direction">Direction the cylinder is pointing at.</param>
		/// <param name="radius">   Radius of the cylinder's base.</param>
		/// <param name="height">   Height of the cylinder.</param>
		/// <param name="color">    Color of the cylinder.</param>
		/// <param name="shaded">   Indicates the the cylinder is shaded.</param>
		public static void DrawCylinder(Vector3 center, Vector3 direction, float radius, float height, ColorByte color,
										bool shaded = true)
		{
			DrawCylinderInternal(center, direction, radius, height, color, shaded);
		}
		/// <summary>
		/// Draws a character animation skeleton bone.
		/// </summary>
		/// <param name="parent">Location of the parent bone.</param>
		/// <param name="bone">  Location of this bone.</param>
		/// <param name="color"> Color of the bone.</param>
		public static void DrawBone(Vector3 parent, Vector3 bone, ColorByte color)
		{
			DrawBoneInternal(parent, bone, color);
		}
		/// <summary>
		/// Flushes the internal buffer of objects forcing them to appear on the screen.
		/// </summary>
		public static void Flush()
		{
			FlushInternal();
		}
		#endregion
		#region Utilities
		/// <exception cref="ArgumentException">There are indexes that are out of range.</exception>
		private static void ValidateIndexes(uint[] indexes, int vertexCount, int indexCount)
		{
			StringBuilder erroneousIndexes = null;
			for (int i = 0; i < indexCount; i++)
			{
				// Check the index.
				if (indexes[i] > vertexCount)
				{
					// Lazy initialization.
					if (erroneousIndexes == null)
					{
						erroneousIndexes = new StringBuilder(200);
						erroneousIndexes.Append("There are indexes that are out of range: ");
					}
					else
					{
						erroneousIndexes.Append(", ");
					}

					// Add the index.
					erroneousIndexes.AppendFormat("[{0}] = {1}", i, indexes[i]);
				}
			}

			if (erroneousIndexes != null)
			{
				// Complete the message and throw it.
				erroneousIndexes.Append(";");
				throw new ArgumentException(erroneousIndexes.ToString());
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AuxiliaryGeometryRenderFlags GetFlags();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(AuxiliaryGeometryRenderFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawPointInternal(Vector3 position, ColorByte color, byte thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawPointsInternal(Vector3* positions, int positionCount, ColorByte color,
													  byte thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawPointsColorsInternal(Vector3* positions, int positionCount, ColorByte* colors,
															int colorCount, byte thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLineInternal(Vector3 start, Vector3 end, ColorByte color, float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLineColorsInternal(Vector3 start, ColorByte colorStart, Vector3 end,
														  ColorByte colorEnd, float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLinesInternal(Vector3* vertexes, int vertexCount, ColorByte color, float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLinesColorsInternal(Vector3* vertexes, int vertexCount, ColorByte* colors,
														   int colorCount, float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLinesIndexesInternal(Vector3* vertexes, int vertexCount, uint* indexes,
															int indexCount, ColorByte color, float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLinesIndexesColorsInternal(Vector3* vertexes, int vertexCount, uint* indexes,
																  int indexCount, ColorByte* colors,
																  float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawPolylineInternal(Vector3* vertexes, int vertexCount, bool closed, ColorByte color,
														float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawPolylineColorsInternal(Vector3* vertexes, int vertexCount, bool closed,
															  ColorByte* colors, float thickness);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTriangleInternal(Vector3 first, ColorByte firstColor, Vector3 second,
														ColorByte secondColor, Vector3 third, ColorByte thirdColor);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTrianglesInternal(Vector3* vertexes, int vertexCount, ColorByte color);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTrianglesColorsInternal(Vector3* vertexes, int vertexCount, ColorByte* colors,
															   int colorCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTrianglesIndexesInternal(Vector3* vertexes, int vertexCount, uint* indexes,
																int indexCount, ColorByte color);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawTrianglesIndexesColorsInternal(Vector3* vertexes, int vertexCount, uint* indexes,
																	  int indexCount, ColorByte* colors);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawAABBInternal(ref BoundingBox box, bool solid, ColorByte color,
													BoundingBoxRenderStyle style);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawAABBMatrixInternal(ref BoundingBox box, ref Matrix34 mat, bool solid,
														  ColorByte color, BoundingBoxRenderStyle style);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawAABBsInternal(BoundingBox* boxes, int boxCount, bool solid, ColorByte color,
													 BoundingBoxRenderStyle style);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawOBBInternal(ref OBB box, Vector3 translation, bool solid, ColorByte color,
												   BoundingBoxRenderStyle style);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawOBBMatrixInternal(ref OBB box, ref Matrix34 matWorld, bool solid, ColorByte color,
														 BoundingBoxRenderStyle style);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawSphereInternal(Vector3 center, float radius, ColorByte color, bool shaded);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawConeInternal(Vector3 center, Vector3 direction, float radius, float height,
													ColorByte color, bool shaded);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawCylinderInternal(Vector3 center, Vector3 direction, float radius, float height,
														ColorByte color, bool shaded);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawBoneInternal(Vector3 parent, Vector3 bone, ColorByte color);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FlushInternal();
		#endregion
	}
}