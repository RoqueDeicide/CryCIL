using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Graphics;
using CryCil.Utilities;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Defines access to CryEngine IPersistenDebug API.
	/// </summary>
	/// <example>
	/// <code>
	/// static void Example()
	/// {
	///     // Lets say, we have an explosion and a wind.
	/// 
	///     var center = new Vector3(100, 120, 40);			// Center of the explosion.
	///     DebugGraphics.BeginDrawing("Explosion with wind", true);
	/// 
	///     // We gonna represent explosion and its damage radius with a sphere.
	///     DebugGraphics.DrawSphere(center, 4, Colors.DarkRed, 10);
	/// 
	///     // And the direction of the wind that will blow that particle effects away is an arrow:
	///     DebugGraphics.DrawDirection(center, 0.1f, Vector3.Forward, Colors.LightBlue, 10);
	/// }
	/// </code>
	/// </example>
	public static class DebugGraphics
	{
		#region Fields
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Begins drawing. Not invoking this method before any Draw methods may cause a crash.
		/// </summary>
		/// <param name="name"> Name of the object group. Use string literals for this argument.</param>
		/// <param name="clear">
		/// Indicates whether existing group that uses this name should be cleared.
		/// </param>
		public static void BeginDrawing(string name, bool clear)
		{
			Begin(StringPool.Get(name), clear);
		}
		/// <summary>
		/// Draws a sphere.
		/// </summary>
		/// <param name="center"> Coordinates of the center of the sphere.</param>
		/// <param name="radius"> Radius of the sphere in meters.</param>
		/// <param name="color">  Color to use when drawing.</param>
		/// <param name="timeout">Timespan during which the object will be displayed in seconds.</param>
		public static void DrawSphere(Vector3 center, float radius, ColorSingle color, float timeout)
		{
			AddSphere(center, radius, color, timeout);
		}
		/// <summary>
		/// Draws an arrow.
		/// </summary>
		/// <param name="origin">   Starting point of the arrow.</param>
		/// <param name="radius">   Radius of the arrow cone.</param>
		/// <param name="direction">Direction the arrow is point in.</param>
		/// <param name="color">    Color to use when drawing.</param>
		/// <param name="timeout">  Timespan during which the object will be displayed in seconds.</param>
		public static void DrawDirection(Vector3 origin, float radius, Vector3 direction, ColorSingle color, float timeout)
		{
			AddDirection(origin, radius, direction, color, timeout);
		}
		/// <summary>
		/// Draws a line.
		/// </summary>
		/// <param name="start">  First point of the line.</param>
		/// <param name="end">    Second point of the line.</param>
		/// <param name="color">  Color to use when drawing.</param>
		/// <param name="timeout">Timespan during which the object will be displayed in seconds.</param>
		public static void DrawLine(Vector3 start, Vector3 end, ColorSingle color, float timeout)
		{
			AddLine(start, end, color, timeout);
		}
		/// <summary>
		/// Draws a disc.
		/// </summary>
		/// <param name="position">   Coordinates of the center of the disc.</param>
		/// <param name="innerRadius">Inner radius of the disc.</param>
		/// <param name="outerRadius">Outer radius of the disc.</param>
		/// <param name="color">      Color to use when drawing.</param>
		/// <param name="timeout">    
		/// Timespan during which the object will be displayed in seconds.
		/// </param>
		public static void DrawPlanarDisc(Vector3 position, float innerRadius, float outerRadius, ColorSingle color,
										  float timeout)
		{
			AddPlanarDisc(position, innerRadius, outerRadius, color, timeout);
		}
		/// <summary>
		/// Draws a cone.
		/// </summary>
		/// <param name="position">  Coordinates of center of the base circle of the cone.</param>
		/// <param name="direction"> Direction of the cone.</param>
		/// <param name="baseRadius">Radius of the cone's base.</param>
		/// <param name="height">    Height of the cone.</param>
		/// <param name="color">     Color to use when drawing.</param>
		/// <param name="timeout">   Timespan during which the object will be displayed in seconds.</param>
		public static void DrawCone(Vector3 position, Vector3 direction, float baseRadius, float height, ColorSingle color,
									float timeout)
		{
			AddCone(position, direction, baseRadius, height, color, timeout);
		}
		/// <summary>
		/// Draws a cylinder.
		/// </summary>
		/// <param name="position"> Coordinates of center of the base circle of the cylinder.</param>
		/// <param name="direction">Direction of the cylinder.</param>
		/// <param name="radius">   Radius of the cylinder.</param>
		/// <param name="height">   Height of the cylinder.</param>
		/// <param name="color">    Color to use when drawing.</param>
		/// <param name="timeout">  Timespan during which the object will be displayed in seconds.</param>
		public static void DrawCylinder(Vector3 position, Vector3 direction, float radius, float height, ColorSingle color,
										float timeout)
		{
			AddCylinder(position, direction, radius, height, color, timeout);
		}
		/// <summary>
		/// Draws text somewhere on the screen.
		/// </summary>
		/// <param name="size">   Font size.</param>
		/// <param name="color">  Color to use when drawing.</param>
		/// <param name="timeout">Timespan during which the object will be displayed in seconds.</param>
		/// <param name="format"> Formattable text to draw.</param>
		/// <param name="args">   Arguments to insert into text with formatting.</param>
		[StringFormatMethod("format")]
		public static void Draw2DText(float size, ColorSingle color, float timeout, string format, params object[] args)
		{
			Add2DText(string.Format(format, args), size, color, timeout);
		}
		/// <summary>
		/// Draws text at the specified location on the screen.
		/// </summary>
		/// <param name="x">      X-coordinate of the text location.</param>
		/// <param name="y">      Y-coordinate of the text location.</param>
		/// <param name="size">   Font size.</param>
		/// <param name="color">  Color to use when drawing.</param>
		/// <param name="timeout">Timespan during which the object will be displayed in seconds.</param>
		/// <param name="format"> Formattable text to draw.</param>
		/// <param name="args">   Arguments to insert into text with formatting.</param>
		[StringFormatMethod("format")]
		public static void DrawText(float x, float y, float size, ColorSingle color, float timeout, string format,
									params object[] args)
		{
			AddText(x, y, size, color, timeout, string.Format(format, args));
		}
		/// <summary>
		/// Draws a 2D line on the screen.
		/// </summary>
		/// <param name="x1">     X-coordinate of the first point of the line.</param>
		/// <param name="y1">     Y-coordinate of the first point of the line.</param>
		/// <param name="x2">     X-coordinate of the second point of the line.</param>
		/// <param name="y2">     Y-coordinate of the second point of the line.</param>
		/// <param name="color">  Color to use when drawing.</param>
		/// <param name="timeout">Timespan during which the object will be displayed in seconds.</param>
		public static void Draw2DLine(float x1, float y1, float x2, float y2, ColorSingle color, float timeout)
		{
			Add2DLine(x1, y1, x2, y2, color, timeout);
		}
		/// <summary>
		/// Draws Oriented Bounding Box (OBB).
		/// </summary>
		/// <param name="position">  Location of the box.</param>
		/// <param name="quaternion">
		/// <see cref="Quaternion"/> that represents orientation of the OBB.
		/// </param>
		/// <param name="r">         Radius of the box.</param>
		/// <param name="color">     Color to use when drawing.</param>
		/// <param name="timeout">   Timespan during which the object will be displayed in seconds.</param>
		public static void DrawOBB(Vector3 position, Quaternion quaternion, float r, ColorSingle color, float timeout)
		{
			AddQuat(position, quaternion, r, color, timeout);
		}
		/// <summary>
		/// Draws Axis-Aligned Bounding Box (AABB).
		/// </summary>
		/// <param name="min">    The minimum point of the box.</param>
		/// <param name="max">    The maximum point of the box.</param>
		/// <param name="color">  Color to use when drawing.</param>
		/// <param name="timeout">Timespan during which the object will be displayed in seconds.</param>
		public static void DrawAABB(Vector3 min, Vector3 max, ColorSingle color, float timeout)
		{
			AddAABB(min, max, color, timeout);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Begin(IntPtr namePtr, bool clear);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddSphere(Vector3 pos, float radius, ColorSingle color, float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddDirection(Vector3 pos, float radius, Vector3 dir, ColorSingle color, float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddLine(Vector3 pos1, Vector3 pos2, ColorSingle color, float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddPlanarDisc(Vector3 pos, float innerRadius, float outerRadius, ColorSingle color,
												  float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddCone(Vector3 pos, Vector3 dir, float baseRadius, float height, ColorSingle color,
											float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddCylinder(Vector3 pos, Vector3 dir, float radius, float height, ColorSingle color,
												float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Add2DText(string text, float size, ColorSingle color, float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddText(float x, float y, float size, ColorSingle color, float timeout, string fmt);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Add2DLine(float x1, float y1, float x2, float y2, ColorSingle color, float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddQuat(Vector3 pos, Quaternion q, float r, ColorSingle color, float timeout);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddAABB(Vector3 min, Vector3 max, ColorSingle color, float timeout);
		#endregion
	}
}