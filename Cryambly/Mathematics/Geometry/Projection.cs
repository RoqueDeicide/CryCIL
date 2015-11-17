using System;

namespace CryCil.Geometry
{
	/// <summary>
	/// Provides functionality for projecting vectors onto surfaces.
	/// </summary>
	public static class Projection
	{
		/// <summary>
		/// Creates a vector which is projection of given vector onto plane which is defined by given
		/// normal.
		/// </summary>
		/// <param name="vector">Vector to project.</param>
		/// <param name="normal">A normal of the plane.</param>
		/// <returns>Projection.</returns>
		/// <exception cref="ArgumentException">Normal to a plane must be a unit vector.</exception>
		public static Vector3 Create(Vector3 vector, Vector3 normal)
		{
#if DEBUG
			if (!normal.IsUnit(MathHelpers.ZeroTolerance))
			{
				throw new ArgumentException("Normal to a plane must be a unit vector.");
			}
#endif
			float dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
			return new Vector3(vector.X - normal.X * dot,
							   vector.Y - normal.Y * dot,
							   vector.Z - normal.Z * dot);
		}
		/// <summary>
		/// Projects this vector onto a plane specified by given normal and one of the points on the plane
		/// and that goes through the origin.
		/// </summary>
		/// <param name="vector">A vector that represents a point on the plane.</param>
		/// <param name="normal">A normal of the plane.</param>
		/// <exception cref="ArgumentException">Normal to a plane must be a unit vector.</exception>
		public static void Apply(ref Vector3 vector, Vector3 normal)
		{
#if DEBUG
			if (!normal.IsUnit(MathHelpers.ZeroTolerance))
			{
				throw new ArgumentException("Normal to a plane must be a unit vector.");
			}
#endif
			float dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
			vector.X = vector.X - normal.X * dot;
			vector.Y = vector.Y - normal.Y * dot;
			vector.Z = vector.Z - normal.Z * dot;
		}
	}
}