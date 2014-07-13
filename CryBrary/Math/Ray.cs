using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CryEngine.Physics;

namespace CryEngine
{
	/// <summary>
	/// Represents a three dimensional line based on a point in space
	/// and a direction.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Ray : IEquatable<Ray>, IFormattable
	{
		/// <summary>
		/// The position in three dimensional space where the ray starts.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// The normalized direction in which the ray points.
		/// </summary>
		public Vector3 Direction;
		/// <summary>
		/// Initializes a new instance of the <see
		/// cref="CryEngine.Ray" /> struct.
		/// </summary>
		/// <param name="position">
		/// The position in three dimensional space of the origin of
		/// the ray.
		/// </param>
		/// <param name="direction">
		/// The normalized direction of the ray.
		/// </param>
		public Ray(Vector3 position, Vector3 direction)
		{
			Position = position;
			Direction = direction;
		}
		/// <summary>
		/// Steps through the entity grid and raytraces entities
		/// traces a finite ray from org along dir
		/// </summary>
		/// <param name="objectTypes"></param>
		/// <param name="flags"></param>
		/// <param name="maxHits"></param>
		/// <param name="skipEntities">
		/// an array of IPhysicalEntity handles. <see
		/// cref="CryEngine.Native.NativeHandleExtensions.GetIPhysicalEntity" />
		/// </param>
		/// <returns>Detected hits (solid and pierceable)</returns>
		public IEnumerable<RaycastHit> Cast(EntityQueryFlags objectTypes = EntityQueryFlags.All, RayWorldIntersectionFlags flags = RayWorldIntersectionFlags.AnyHit, int maxHits = 1, IntPtr[] skipEntities = null)
		{
			if (skipEntities != null && skipEntities.Length == 0)
				skipEntities = null;

			object[] hits;
			var numHits = Native.NativePhysicsMethods.RayWorldIntersection(Position, Direction, objectTypes, flags, maxHits, skipEntities, out hits);
			if (numHits > 0)
			{
				return hits.Cast<RaycastHit>();
			}

			return new List<RaycastHit>();
		}

		public static IEnumerable<RaycastHit> Cast(Vector3 pos, Vector3 dir, EntityQueryFlags objectTypes = EntityQueryFlags.All, RayWorldIntersectionFlags flags = RayWorldIntersectionFlags.AnyHit, int maxHits = 1, IntPtr[] skipEntities = null)
		{
			var ray = new Ray(pos, dir);

			return ray.Cast(objectTypes, flags, maxHits, skipEntities);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a point.
		/// </summary>
		/// <param name="point">The point to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Vector3 point)
		{
			return Collision.RayIntersectsPoint(ref this, ref point);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.Ray" />.
		/// </summary>
		/// <param name="ray">The ray to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Ray ray)
		{
			Vector3 point;
			return Collision.RayIntersectsRay(ref this, ref ray, out point);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.Ray" />.
		/// </summary>
		/// <param name="ray">The ray to test.</param>
		/// <param name="point">
		/// When the method completes, contains the point of
		/// intersection, or <see cref="CryEngine.Vector3" /> if there
		/// was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Ray ray, out Vector3 point)
		{
			return Collision.RayIntersectsRay(ref this, ref ray, out point);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.Plane" />.
		/// </summary>
		/// <param name="plane">The plane to test</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Plane plane)
		{
			float distance;
			return Collision.RayIntersectsPlane(ref this, ref plane, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.Plane" />.
		/// </summary>
		/// <param name="plane">The plane to test.</param>
		/// <param name="distance">
		/// When the method completes, contains the distance of the
		/// intersection, or 0 if there was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Plane plane, out float distance)
		{
			return Collision.RayIntersectsPlane(ref this, ref plane, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.Plane" />.
		/// </summary>
		/// <param name="plane">The plane to test.</param>
		/// <param name="point">
		/// When the method completes, contains the point of
		/// intersection, or <see cref="CryEngine.Vector3" /> if there
		/// was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Plane plane, out Vector3 point)
		{
			return Collision.RayIntersectsPlane(ref this, ref plane, out point);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a triangle.
		/// </summary>
		/// <param name="vertex1">
		/// The first vertex of the triangle to test.
		/// </param>
		/// <param name="vertex2">
		/// The second vertex of the triangle to test.
		/// </param>
		/// <param name="vertex3">
		/// The third vertex of the triangle to test.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
		{
			float distance;
			return Collision.RayIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a triangle.
		/// </summary>
		/// <param name="vertex1">
		/// The first vertex of the triangle to test.
		/// </param>
		/// <param name="vertex2">
		/// The second vertex of the triangle to test.
		/// </param>
		/// <param name="vertex3">
		/// The third vertex of the triangle to test.
		/// </param>
		/// <param name="distance">
		/// When the method completes, contains the distance of the
		/// intersection, or 0 if there was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, out float distance)
		{
			return Collision.RayIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a triangle.
		/// </summary>
		/// <param name="vertex1">
		/// The first vertex of the triangle to test.
		/// </param>
		/// <param name="vertex2">
		/// The second vertex of the triangle to test.
		/// </param>
		/// <param name="vertex3">
		/// The third vertex of the triangle to test.
		/// </param>
		/// <param name="point">
		/// When the method completes, contains the point of
		/// intersection, or <see cref="CryEngine.Vector3" /> if there
		/// was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3, out Vector3 point)
		{
			return Collision.RayIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3, out point);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.BoundingBox" />.
		/// </summary>
		/// <param name="box">The box to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingBox box)
		{
			float distance;
			return Collision.RayIntersectsBox(ref this, ref box, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.BoundingBox" />.
		/// </summary>
		/// <param name="box">The box to test.</param>
		/// <param name="distance">
		/// When the method completes, contains the distance of the
		/// intersection, or 0 if there was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingBox box, out float distance)
		{
			return Collision.RayIntersectsBox(ref this, ref box, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.BoundingBox" />.
		/// </summary>
		/// <param name="box">The box to test.</param>
		/// <param name="point">
		/// When the method completes, contains the point of
		/// intersection, or <see cref="CryEngine.Vector3" /> if there
		/// was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingBox box, out Vector3 point)
		{
			return Collision.RayIntersectsBox(ref this, ref box, out point);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.BoundingSphere" />.
		/// </summary>
		/// <param name="sphere">The sphere to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingSphere sphere)
		{
			float distance;
			return Collision.RayIntersectsSphere(ref this, ref sphere, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.BoundingSphere" />.
		/// </summary>
		/// <param name="sphere">The sphere to test.</param>
		/// <param name="distance">
		/// When the method completes, contains the distance of the
		/// intersection, or 0 if there was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingSphere sphere, out float distance)
		{
			return Collision.RayIntersectsSphere(ref this, ref sphere, out distance);
		}

		/// <summary>
		/// Determines if there is an intersection between the current
		/// object and a <see cref="CryEngine.BoundingSphere" />.
		/// </summary>
		/// <param name="sphere">The sphere to test.</param>
		/// <param name="point">
		/// When the method completes, contains the point of
		/// intersection, or <see cref="CryEngine.Vector3" /> if there
		/// was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingSphere sphere, out Vector3 point)
		{
			return Collision.RayIntersectsSphere(ref this, ref sphere, out point);
		}

		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> has the same value
		/// as <paramref name="right" />; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Ray left, Ray right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> has a different
		/// value than <paramref name="right" />; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Ray left, Ray right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents
		/// this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", Position.ToString(), Direction.ToString());
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents
		/// this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString(string format)
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", Position.ToString(), Direction.ToString());
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents
		/// this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Position:{0} Direction:{1}", Position.ToString(), Direction.ToString());
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents
		/// this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Position:{0} Direction:{1}", Position.ToString(), Direction.ToString());
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing
		/// algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + Position.GetHashCode();
				hash = hash * 29 + Direction.GetHashCode();

				return hash;
			}
		}

		/// <summary>
		/// Determines whether the specified <see
		/// cref="CryEngine.Vector4" /> is equal to this instance.
		/// </summary>
		/// <param name="value">
		/// The <see cref="CryEngine.Vector4" /> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="CryEngine.Vector4"
		/// /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Ray value)
		{
			return Position == value.Position && Direction == value.Direction;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"
		/// /> is equal to this instance.
		/// </summary>
		/// <param name="value">
		/// The <see cref="System.Object" /> to compare with this instance.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object" />
		/// is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object value)
		{
			if (value == null)
				return false;

			if (value.GetType() != GetType())
				return false;

			return Equals((Ray)value);
		}
	}
}