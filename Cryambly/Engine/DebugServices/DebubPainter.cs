using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Graphics;
using CryCil.Utilities;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents an object that can be used to place temporary debug objects in the game.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Objects of this class allow complicated groups of objects to be created with less code clutter by
	/// storing some common information (like colors) without constantly requesting specifications.
	/// </para>
	/// <para>
	/// Objects of this class are not supposed to persist beyond being a simple variable. Keeping these
	/// over a long period of time may cause crashes.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// static void Example()
	/// {
	///     // Lets say, we have an explosion and a wind.
	/// 
	///     var center = new Vector3(100, 120, 40);			// Center of the explosion.
	///     var painter = new DebubPainter("Explosion with wind")
	///     {
	///     	Timeout = 10,
	///     	Color = Colors.DarkRed
	///     };
	/// 
	///     // We gonna represent explosion and its damage radius with a sphere.
	///     painter.Sphere(center, 4);
	/// 
	///     // And the direction of the wind that will blow that particle effects away is an arrow:
	///     painter.Color = Colors.LightBlue;
	///     painter.Arrow(center, 0.1f, Vector3.Forward);
	/// }
	/// </code>
	/// </example>
	public class DebubPainter
	{
		#region Fields
		private float timeout;
		private ColorSingle color;
		#endregion
		#region Properties
		/// <summary>
		/// Sets the time after which the drawn object will expire (in seconds).
		/// </summary>
		public float Timeout
		{
			set { this.timeout = value; }
		}
		/// <summary>
		/// Sets color to use for drawn objects.
		/// </summary>
		public ColorSingle Color
		{
			set { this.color = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="name"> Name of the graphics group to put all objects into.</param>
		/// <param name="clear">
		/// Indicates whether existing group that uses this name should be cleared.
		/// </param>
		public DebubPainter(string name, bool clear = true)
		{
			DebugGraphics.Begin(StringPool.Get(name), clear);
			this.timeout = 1;
			this.color = new ColorSingle(0.0f, 1.0f, 0.0f);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Draws a sphere.
		/// </summary>
		/// <param name="center">Coordinates of the center of the sphere.</param>
		/// <param name="radius">Radius of the sphere in meters.</param>
		public void Sphere(Vector3 center, float radius)
		{
			DebugGraphics.AddSphere(center, radius, this.color, this.timeout);
		}
		/// <summary>
		/// Draws an arrow.
		/// </summary>
		/// <param name="origin">   Starting point of the arrow.</param>
		/// <param name="radius">   Radius of the arrow cone.</param>
		/// <param name="direction">Direction the arrow is point in.</param>
		public void Arrow(Vector3 origin, float radius, Vector3 direction)
		{
			DebugGraphics.AddDirection(origin, radius, direction, this.color, this.timeout);
		}
		/// <summary>
		/// Draws a line.
		/// </summary>
		/// <param name="start">First point of the line.</param>
		/// <param name="end">  Second point of the line.</param>
		public void Line(Vector3 start, Vector3 end)
		{
			DebugGraphics.AddLine(start, end, this.color, this.timeout);
		}
		/// <summary>
		/// Draws a 2D line on the screen.
		/// </summary>
		/// <param name="x1">X-coordinate of the first point of the line.</param>
		/// <param name="y1">Y-coordinate of the first point of the line.</param>
		/// <param name="x2">X-coordinate of the second point of the line.</param>
		/// <param name="y2">Y-coordinate of the second point of the line.</param>
		public void Line(float x1, float y1, float x2, float y2)
		{
			DebugGraphics.Add2DLine(x1, y1, x2, y2, this.color, this.timeout);
		}
		/// <summary>
		/// Draws a disc.
		/// </summary>
		/// <param name="position">   Coordinates of the center of the disc.</param>
		/// <param name="innerRadius">Inner radius of the disc.</param>
		/// <param name="outerRadius">Outer radius of the disc.</param>
		public void PlanarDisc(Vector3 position, float innerRadius, float outerRadius)
		{
			DebugGraphics.AddPlanarDisc(position, innerRadius, outerRadius, this.color, this.timeout);
		}
		/// <summary>
		/// Draws a cone.
		/// </summary>
		/// <param name="position">  Coordinates of center of the base circle of the cone.</param>
		/// <param name="direction"> Direction of the cone.</param>
		/// <param name="baseRadius">Radius of the cone's base.</param>
		/// <param name="height">    Height of the cone.</param>
		public void Cone(Vector3 position, Vector3 direction, float baseRadius, float height)
		{
			DebugGraphics.AddCone(position, direction, baseRadius, height, this.color, this.timeout);
		}
		/// <summary>
		/// Draws a cylinder.
		/// </summary>
		/// <param name="position"> Coordinates of center of the base circle of the cylinder.</param>
		/// <param name="direction">Direction of the cylinder.</param>
		/// <param name="radius">   Radius of the cylinder.</param>
		/// <param name="height">   Height of the cylinder.</param>
		public void Cylinder(Vector3 position, Vector3 direction, float radius, float height)
		{
			DebugGraphics.AddCylinder(position, direction, radius, height, this.color, this.timeout);
		}
		/// <summary>
		/// Draws text somewhere on the screen.
		/// </summary>
		/// <param name="size">  Font size.</param>
		/// <param name="format">Formattable text to draw.</param>
		/// <param name="args">  Arguments to insert into text with formatting.</param>
		[StringFormatMethod("format")]
		public void Text(float size, string format, params object[] args)
		{
			DebugGraphics.Add2DText(string.Format(format, args), size, this.color, this.timeout);
		}
		/// <summary>
		/// Draws text at the specified location on the screen.
		/// </summary>
		/// <param name="x">     X-coordinate of the text location.</param>
		/// <param name="y">     Y-coordinate of the text location.</param>
		/// <param name="size">  Font size.</param>
		/// <param name="format">Formattable text to draw.</param>
		/// <param name="args">  Arguments to insert into text with formatting.</param>
		[StringFormatMethod("format")]
		public void Text(float x, float y, float size, string format, params object[] args)
		{
			DebugGraphics.AddText(x, y, size, this.color, this.timeout, string.Format(format, args));
		}
		/// <summary>
		/// Draws Oriented Bounding Box (OBB).
		/// </summary>
		/// <param name="position">  Location of the box.</param>
		/// <param name="quaternion">
		/// <see cref="Quaternion"/> that represents orientation of the OBB.
		/// </param>
		/// <param name="r">         Radius of the box.</param>
		public void OBB(Vector3 position, Quaternion quaternion, float r)
		{
			DebugGraphics.AddQuat(position, quaternion, r, this.color, this.timeout);
		}
		/// <summary>
		/// Draws Axis-Aligned Bounding Box (AABB).
		/// </summary>
		/// <param name="min">The minimum point of the box.</param>
		/// <param name="max">The maximum point of the box.</param>
		public void AABB(Vector3 min, Vector3 max)
		{
			DebugGraphics.AddAABB(min, max, this.color, this.timeout);
		}
		/// <summary>
		/// Draws an offseted Axis-Aligned Bounding Box (AABB).
		/// </summary>
		/// <param name="offset">Translation vector.</param>
		/// <param name="box">   AABB to draw.</param>
		public void AABB(Vector3 offset, BoundingBox box)
		{
			DebugGraphics.AddAABB(box.Minimum + offset, box.Maximum + offset, this.color, this.timeout);
		}
		#endregion
	}
}