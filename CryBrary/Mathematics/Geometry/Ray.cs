using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using CryEngine.NativeMemory;

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
		/// Initializes a new instance of the <see cref="Ray"/> struct.
		/// </summary>
		/// <param name="position"> 
		/// The position in three dimensional space of the origin of the ray.
		/// </param>
		/// <param name="direction">The normalized direction of the ray.</param>
		public Ray(Vector3 position, Vector3 direction)
		{
			this.Position = position;
			this.Direction = direction;
		}
		/// <summary>
		/// Casts a ray through the world and returns all intersections.
		/// </summary>
		/// <param name="objectTypes"> 
		/// Flags that define which objects to query for intersection with this ray.
		/// </param>
		/// <param name="flags">       Flags that define how to detect intersections.</param>
		/// <param name="maxHits">     Maximal number of hits to return.</param>
		/// <param name="skipEntities">An array of IPhysicalEntity handles.</param>
		/// <returns>Detected hits (solid and pierceable).</returns>
		public IEnumerable<RaycastHit> Cast
		(
			EntityQueryFlags objectTypes = EntityQueryFlags.All,
			RayWorldIntersectionFlags flags = RayWorldIntersectionFlags.AnyHit,
			int maxHits = 1,
			IntPtr[] skipEntities = null
		)
		{
			return Ray.Cast(this.Position, this.Direction, objectTypes, flags, maxHits, skipEntities);
		}
		/// <summary>
		/// Casts a ray through the world and returns all intersections.
		/// </summary>
		/// <param name="pos">         Position from which to cast a ray.</param>
		/// <param name="dir">         Direction of the ray to cast.</param>
		/// <param name="objectTypes"> 
		/// Flags that define which objects to query for intersection with this ray.
		/// </param>
		/// <param name="flags">       Flags that define how to detect intersections.</param>
		/// <param name="maxHits">     Maximal number of hits to return.</param>
		/// <param name="skipEntities">An array of IPhysicalEntity handles.</param>
		/// <returns>Detected hits (solid and pierceable).</returns>
		public unsafe static List<RaycastHit> Cast
		(
			Vector3 pos,
			Vector3 dir,
			EntityQueryFlags objectTypes = EntityQueryFlags.All,
			RayWorldIntersectionFlags flags = RayWorldIntersectionFlags.AnyHit,
			int maxHits = 1,
			IntPtr[] skipEntities = null
		)
		{
			if (maxHits < 1)
			{
				return null;
			}
			List<RaycastHit> hits;
			// Allocate memory for entities to skip.
			using (NativeMemoryBlock skipEntitiesArray =
				new NativeMemoryBlock(skipEntities == null ? 0 : skipEntities.Length, typeof(IntPtr)))
			// Allocate memory for ray-cast hits.
			using (NativeMemoryBlock hitsArray = new NativeMemoryBlock(maxHits, typeof(RaycastHit)))
			{
				// Transfer handles of entities to skip to native memory.
				if (!skipEntitiesArray.Disposed)
				{
					IntPtr* skipEntsPointer = (IntPtr*)skipEntitiesArray.Handle.ToPointer();
					for (int i = 0; i < skipEntities.Length; i++)		// Ignore that ReSharper warning.
					{
						skipEntsPointer[i] = skipEntities[i];
					}
				}
				// Cast the ray.
				int numberOfHits =
					Native.PhysicsInterop.RayWorldIntersection
					(
						pos,
						dir,
						objectTypes,
						flags,
						maxHits,
						skipEntitiesArray.Handle,
						skipEntities == null ? 0 : skipEntities.Length,
						hitsArray.Handle
					);
				// Transfer ray-cast hits from native memory.
				RaycastHit* hitPointer = (RaycastHit*)hitsArray.Handle.ToPointer();
				hits = new List<RaycastHit>(numberOfHits);
				for (int i = 0; i < numberOfHits; i++)
				{
					hits.Add(hitPointer[i]);
				}
			}
			return hits;
		}
		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/> ;
		/// otherwise, <c>false</c> .
		/// </returns>
		public static bool operator ==(Ray left, Ray right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has a different value than
		/// <paramref name="right"/> ; otherwise, <c>false</c> .
		/// </returns>
		public static bool operator !=(Ray left, Ray right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", this.Position.ToString(), this.Direction.ToString());
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(string format)
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", this.Position.ToString(), this.Direction.ToString());
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Position:{0} Direction:{1}", this.Position.ToString(), this.Direction.ToString());
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">        The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
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
		/// Determines whether the specified <see cref="Vector4"/> is equal to this instance.
		/// </summary>
		/// <param name="value">The <see cref="Vector4"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public bool Equals(Ray value)
		{
			return this.Position == value.Position && this.Direction == value.Direction;
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
		/// otherwise, <c>false</c> .
		/// </returns>
		public override bool Equals(object value)
		{
			return value != null && value.GetType() == this.GetType() && this.Equals((Ray)value);
		}
	}
}