using System;

namespace CryCil.Geometry
{
	/// <summary>
	/// Describes the result of an intersection with a plane in three dimensions.
	/// </summary>
	[Flags]
	public enum PlaneIntersectionType
	{
		/// <summary>
		/// The object is on the plane.
		/// </summary>
		Coplanar = 0,
		/// <summary>
		/// The object is behind the plane.
		/// </summary>
		Back = 1,
		/// <summary>
		/// The object is in front of the plane.
		/// </summary>
		Front = 2,
		/// <summary>
		/// The object is intersecting the plane.
		/// </summary>
		Intersecting = 3
	}
}