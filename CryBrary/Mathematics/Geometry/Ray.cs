using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryEngine.Mathematics.Geometry
{
	/// <summary>
	/// Represents a three dimensional line based on a point in space and a direction.
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
		/// Initializes a new instance of the <see cref="Ray" /> struct.
		/// </summary>
		/// <param name="position"> 
		/// The position in three dimensional space of the origin of the ray.
		/// </param>
		/// <param name="direction"> The normalized direction of the ray. </param>
		public Ray(Vector3 position, Vector3 direction)
		{
			this.Position = position;
			this.Direction = direction;
		}
		/// <summary>
		/// Steps through the entity grid and raytraces entities traces a finite ray from org along dir
		/// </summary>
		/// <param name="objectTypes">  </param>
		/// <param name="flags">        </param>
		/// <param name="maxHits">      </param>
		/// <param name="skipEntities">
		/// an array of IPhysicalEntity handles. <see
		/// cref="CryEngine.Native.NativeHandleExtensions.GetIPhysicalEntity" />
		/// </param>
		/// <returns> Detected hits (solid and pierceable) </returns>
		public IEnumerable<RaycastHit> Cast(EntityQueryFlags objectTypes = EntityQueryFlags.All, RayWorldIntersectionFlags flags = RayWorldIntersectionFlags.AnyHit, int maxHits = 1, IntPtr[] skipEntities = null)
		{
			if (skipEntities != null && skipEntities.Length == 0)
				skipEntities = null;

			object[] hits;
			var numHits = Native.NativePhysicsMethods.RayWorldIntersection(this.Position, this.Direction, objectTypes, flags, maxHits, skipEntities, out hits);
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
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left">  The first value to compare. </param>
		/// <param name="right"> The second value to compare. </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> has the same value as <paramref name="right" />;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(Ray left, Ray right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left">  The first value to compare. </param>
		/// <param name="right"> The second value to compare. </param>
		/// <returns>
		/// <c>true</c> if <paramref name="left" /> has a different value than <paramref
		/// name="right" />; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(Ray left, Ray right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns> A <see cref="System.String" /> that represents this instance. </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", this.Position.ToString(), this.Direction.ToString());
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format"> The format. </param>
		/// <returns> A <see cref="System.String" /> that represents this instance. </returns>
		public string ToString(string format)
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", this.Position.ToString(), this.Direction.ToString());
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="formatProvider"> The format provider. </param>
		/// <returns> A <see cref="System.String" /> that represents this instance. </returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Position:{0} Direction:{1}", this.Position.ToString(), this.Direction.ToString());
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">         The format. </param>
		/// <param name="formatProvider"> The format provider. </param>
		/// <returns> A <see cref="System.String" /> that represents this instance. </returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Position:{0} Direction:{1}", this.Position.ToString(), this.Direction.ToString());
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data
		/// structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + this.Position.GetHashCode();
				hash = hash * 29 + this.Direction.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="Vector4" /> is equal to this instance.
		/// </summary>
		/// <param name="value"> The <see cref="Vector4" /> to compare with this instance. </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(Ray value)
		{
			return this.Position == value.Position && this.Direction == value.Direction;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="value"> The <see cref="System.Object" /> to compare with this instance. </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance;
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object value)
		{
			if (value == null)
				return false;

			if (value.GetType() != this.GetType())
				return false;

			return this.Equals((Ray)value);
		}
	}
}