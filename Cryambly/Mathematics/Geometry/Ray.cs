using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Memory;
using CryCil.Engine.Physics;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a three dimensional line based on a point in space and a direction.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public unsafe struct Ray : IEquatable<Ray>, IFormattable
	{
		#region Fields
		/// <summary>
		/// The position in three dimensional space where the ray starts.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// The normalized direction in which the ray points.
		/// </summary>
		public Vector3 Direction;
		#endregion
		#region Construction
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
		#endregion
		#region Interface
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
		/// <c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/> ;
		/// otherwise, <c>false</c> .
		/// </returns>
		public static bool operator !=(Ray left, Ray right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="string"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", this.Position, this.Direction);
		}
		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>A <see cref="string"/> that represents this instance.</returns>
		public string ToString(string format)
		{
			return string.Format(CultureInfo.CurrentCulture, "Position:{0} Direction:{1}", this.Position, this.Direction);
		}
		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="string"/> that represents this instance.</returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Position:{0} Direction:{1}", this.Position, this.Direction);
		}
		/// <summary>
		/// Returns a <see cref="string"/> that represents this instance.
		/// </summary>
		/// <param name="format">        The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="string"/> that represents this instance.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Position:{0} Direction:{1}", this.Position, this.Direction);
		}
		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a
		/// hash table.
		/// </returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + this.Position.GetHashCode();
				hash = hash * 29 + this.Direction.GetHashCode();

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
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise,
		/// <c>false</c> .
		/// </returns>
		public override bool Equals(object value)
		{
			return value != null && value.GetType() == this.GetType() && this.Equals((Ray)value);
		}
		/// <summary>
		/// Casts a ray.
		/// </summary>
		/// <param name="hit">           Resultant singular hit.</param>
		/// <param name="query">         
		/// A set of flags that specify which entities to check for collision with ray and how to process
		/// the result.
		/// </param>
		/// <param name="flags">         
		/// A set of flags that specify how to cast the ray. <see cref="RayCastFlags.StopAtPierceable"/>
		/// will be set internally.
		/// </param>
		/// <param name="entitiesToSkip">
		/// An optional array of entities that have to be ignored by the ray.
		/// </param>
		/// <param name="collisionClass">
		/// An optional value that specifies the collision class for the ray.
		/// </param>
		/// <returns>True, if the ray has hit anything.</returns>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		[Annotations.Pure]
		public bool Cast(out RayHit hit, EntityQueryFlags query = EntityQueryFlags.All, RayCastFlags flags = 0,
						 PhysicalEntity[] entitiesToSkip = null, CollisionClass collisionClass = new CollisionClass())
		{
			flags |= RayCastFlags.StopAtPierceable;

			RayHit h;
			int hitCount;
			if (!entitiesToSkip.IsNullOrEmpty())
			{
				fixed (PhysicalEntity* skipped = entitiesToSkip)
				{
					hitCount = CastRay(ref this.Position, ref this.Direction, query, flags, &h, 1, skipped,
									   entitiesToSkip.Length, collisionClass);
				}
			}
			else
			{
				hitCount = CastRay(ref this.Position, ref this.Direction, query, flags, &h, 1, null, 0, collisionClass);
			}

			if (hitCount == 0)
			{
				hit = new RayHit();
				return false;
			}
			hit = h;
			return true;
		}
		/// <summary>
		/// Casts a ray.
		/// </summary>
		/// <param name="query">         
		/// A set of flags that specify which entities to check for collision with ray and how to process
		/// the result.
		/// </param>
		/// <param name="flags">         A set of flags that specify how to cast the ray.</param>
		/// <param name="entitiesToSkip">
		/// An optional array of entities that have to be ignored by the ray.
		/// </param>
		/// <param name="collisionClass">
		/// An optional value that specifies the collision class for the ray.
		/// </param>
		/// <param name="maxHits">       
		/// Maximal number of hits. Larger values can potentially waste RAM.
		/// </param>
		/// <returns>
		/// An array of objects that describe hits that is sorted by distance away from
		/// <see cref="Position"/> in ascending order, unless
		/// <see cref="RayCastFlags.SeparateImportantHits"/> is set in <paramref name="flags"/> in which
		/// case hit on surface that has <see cref="SurfaceFlags.Important"/> flag set will go before other
		/// pierceable hits.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Maximal number of hits cannot be lass or equal to 0.
		/// </exception>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		[CanBeNull]
		[Annotations.Pure]
		public RayHit[] Cast(EntityQueryFlags query = EntityQueryFlags.All, RayCastFlags flags = RayCastFlags.StopAtPierceable,
							 PhysicalEntity[] entitiesToSkip = null, CollisionClass collisionClass = new CollisionClass(),
							 int maxHits = 16)
		{
			if (maxHits <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxHits), "Maximal number of hits cannot be lass or equal to 0.");
			}
			Contract.EndContractBlock();

			RayHit* hitsBuffer = (RayHit*)CryMarshal.Allocate((ulong)(maxHits * sizeof(RayHit))).ToPointer();

			int hitCount;
			if (!entitiesToSkip.IsNullOrEmpty())
			{
				fixed (PhysicalEntity* skipped = entitiesToSkip)
				{
					hitCount = CastRay(ref this.Position, ref this.Direction, query, flags, hitsBuffer, maxHits, skipped,
									   entitiesToSkip.Length, collisionClass);
				}
			}
			else
			{
				hitCount = CastRay(ref this.Position, ref this.Direction, query, flags, hitsBuffer, maxHits, null, 0,
								   collisionClass);
			}

			if (hitCount == 0)
			{
				return null;
			}

			RayHit[] hits = new RayHit[hitCount];

			fixed (RayHit* hitsPtr = hits)
			{
				for (int i = 0; i < hitCount; i++)
				{
					hitsPtr[i] = hitsBuffer[i];
				}
			}

			return hits;
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int CastRay(ref Vector3 origin, ref Vector3 direction, EntityQueryFlags query,
										  RayCastFlags castFlags, RayHit* hits, int nMaxHits,
										  PhysicalEntity* entitiesToSkip, int skipEntityCount,
										  CollisionClass collisionClass);
		#endregion
	}
}