using System.Runtime.CompilerServices;

namespace CryCil.Engine.Environment
{
	/// <summary>
	/// Provides access to CryEngine terrain API.
	/// </summary>
	public static class Terrain
	{
		/// <summary>
		/// Gets the size of the height map pixel in meters.
		/// </summary>
		public static extern int UnitSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets the size of the height map in pixels (units).
		/// </summary>
		public static extern int Size
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets the size of the height map sector in pixels (units).
		/// </summary>
		public static extern int SectorSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets interpolated terrain elevation.
		/// </summary>
		/// <param name="point">Point at which the elevation is requested.</param>
		/// <returns>Height of the terrain at the given point.</returns>
		public static float Elevation(Vector2 point)
		{
			return Elevation(point.X, point.Y);
		}
		/// <summary>
		/// Gets interpolated terrain elevation.
		/// </summary>
		/// <param name="x">X-coordinate of the point at which the elevation is requested.</param>
		/// <param name="y">Y-coordinate of the point at which the elevation is requested.</param>
		/// <returns>Height of the terrain at the given point.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Elevation(float x, float y);
		/// <summary>
		/// Gets elevation of the terrain at the specified pixel of the height map.
		/// </summary>
		/// <param name="x">X-coordinate of the point at which the elevation is requested.</param>
		/// <param name="y">Y-coordinate of the point at which the elevation is requested.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Elevation(int x, int y);
		/// <summary>
		/// Determines whether specified pixel of the height map is marked as a hole.
		/// </summary>
		/// <param name="x">X-coordinate of the pixel to check.</param>
		/// <param name="y">Y-coordinate of the pixel to check.</param>
		/// <returns>True, if the pixel is marked as a terrain hole.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsHole(int x, int y);
		/// <summary>
		/// Gets the direction of the normal to the terrain surface at the specified location.
		/// </summary>
		/// <param name="point">Point at which to get the normal.</param>
		/// <returns>
		/// A normalized vector that represents direction of the normal to the terrain surface at the
		/// specified location.
		/// </returns>
		public static Vector3 SurfaceNormal(Vector2 point)
		{
			return SurfaceNormal(point.X, point.Y);
		}
		/// <summary>
		/// Gets the direction of the normal to the terrain surface at the specified location.
		/// </summary>
		/// <param name="x">X-coordinate of the point at which to get the normal.</param>
		/// <param name="y">Y-coordinate of the point at which to get the normal.</param>
		/// <returns>
		/// A normalized vector that represents direction of the normal to the terrain surface at the
		/// specified location.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector3 SurfaceNormal(float x, float y);
	}
}