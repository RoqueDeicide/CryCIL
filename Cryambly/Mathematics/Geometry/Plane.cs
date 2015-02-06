using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a plane in three dimensional space.
	/// </summary>
	/// <remarks>
	/// <para>This implementation uses equation:</para>
	/// <para>n.x*x + n.y*y + n.z*z + d = 0</para>
	/// <para>where n is a normal to the plane and formula:</para>
	/// <para>n.x*x + n.y*y + n.z*z + d &gt; 0</para>
	/// <para>describes coordinates of vectors in front of the plane.</para>
	/// </remarks>
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Pack = 4)]
	public partial struct Plane : IEquatable<Plane>, IFormattable
	{
		/// <summary>
		/// X-component of the normal to this plane.
		/// </summary>
		[FieldOffset(0)]
		public float X;
		/// <summary>
		/// Y-component of the normal to this plane.
		/// </summary>
		[FieldOffset(4)]
		public float Y;
		/// <summary>
		/// Z-component of the normal to this plane.
		/// </summary>
		[FieldOffset(8)]
		public float Z;
		/// <summary>
		/// The normal vector of the plane.
		/// </summary>
		[FieldOffset(0)]
		public Vector3 Normal;
		/// <summary>
		/// The distance of the plane along its normal from the origin.
		/// </summary>
		[FieldOffset(12)]
		public float D;
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Plane(float value)
			: this()
		{
			this.X = this.Y = this.Z = this.D = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="a">The X component of the</param>
		/// <param name="b">The Y component of the</param>
		/// <param name="c">The Z component of the</param>
		/// <param name="d">
		/// The distance of the plane along its normal from the origin.
		/// </param>
		public Plane(float a, float b, float c, float d)
			: this()
		{
			this.X = a;
			this.Y = b;
			this.Z = c;
			this.D = d;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="value">The normal of the plane.</param>
		/// <param name="d">    
		/// The distance of the plane along its normal from the origin
		/// </param>
		public Plane(Vector3 value, float d)
			: this()
		{
			this.Normal = value;
			this.D = d;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="point"> Any point that lies along the plane.</param>
		/// <param name="normal">The normal of the plane.</param>
		public Plane(Vector3 point, Vector3 normal)
			: this()
		{
			this.Normal = normal;
			this.D = -Vector3.Dot(normal, point);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct using three
		/// points represented by instances of type <see cref="Vector3"/> and using
		/// anti-clockwise winding order.
		/// </summary>
		/// <param name="point1">First point of a triangle defining the plane.</param>
		/// <param name="point2">Second point of a triangle defining the plane.</param>
		/// <param name="point3">Third point of a triangle defining the plane.</param>
		public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
			: this()
		{
#if DEBUG
			this.Normal = ((point2 - point1) % (point3 - point1)).Normalized;
			this.D = -Vector3.Dot(this.Normal, point1);
#endif
#if RELEASE
			float x1 = point2.X - point1.X;
			float y1 = point2.Y - point1.Y;
			float z1 = point2.Z - point1.Z;
			float x2 = point3.X - point1.X;
			float y2 = point3.Y - point1.Y;
			float z2 = point3.Z - point1.Z;
			float yz = (y1 * z2) - (z1 * y2);
			float xz = (z1 * x2) - (x1 * z2);
			float xy = (x1 * y2) - (y1 * x2);
			float invPyth = 1.0f / (float)Math.Sqrt((yz * yz) + (xz * xz) + (xy * xy));

			X = yz * invPyth;
			Y = xz * invPyth;
			Z = xy * invPyth;
			D = -((X * point1.X) + (Y * point1.Y) + (Z * point1.Z));
#endif
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="values">
		/// The values to assign to the A, B, C, and D components of the plane. This must
		/// be an array with four elements.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="values"/> is <c>null</c> .
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when <paramref name="values"/> contains more or less than four
		/// elements.
		/// </exception>
		public Plane(IList<float> values)
			: this()
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Count != 4)
				throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Plane.");
#endif

			this.X = values[0];
			this.Y = values[1];
			this.Z = values[2];
			this.D = values[3];
		}
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>
		/// The value of the A, B, C, or D component, depending on the index.
		/// </value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the A component, 1 for the B
		/// component, 2 for the C component, and 3 for the D component.
		/// </param>
		/// <returns>The value of the component at the specified index.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// Thrown when the <paramref name="index"/> is out of the range [0, 3].
		/// </exception>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0: return this.X;
					case 1: return this.Y;
					case 2: return this.Z;
					case 3: return this.D;
				}

				throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
			}

			set
			{
				switch (index)
				{
					case 0: this.X = value;
						break;
					case 1: this.Y = value;
						break;
					case 2: this.Z = value;
						break;
					case 3: this.D = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
				}
			}
		}
		/// <summary>
		/// Changes the coefficients of the normal vector of the plane to make it of unit
		/// length.
		/// </summary>
		/// <param name="plane"> The source plane.</param>
		/// <param name="result">
		/// When the method completes, contains the normalized plane.
		/// </param>
		public static void Normalize(ref Plane plane, out Plane result)
		{
			float magnitudeInverse =
				1.0f / (float)Math.Sqrt(plane.X * plane.X + plane.Y * plane.Y + plane.Z * plane.Z);

			result = new Plane
			{
				X = plane.X * magnitudeInverse,
				Y = plane.Y * magnitudeInverse,
				Z = plane.Z * magnitudeInverse,
				D = plane.D * magnitudeInverse
			};
		}
		/// <summary>
		/// Changes the coefficients of the normal vector of the plane to make it of unit
		/// length.
		/// </summary>
		/// <param name="plane">The source plane.</param>
		/// <returns>The normalized plane.</returns>
		public static Plane Normalize(Plane plane)
		{
			float magnitude = 1.0f / (float)Math.Sqrt(plane.X * plane.X + plane.Y * plane.Y + plane.Z * plane.Z);
			return new Plane(plane.X * magnitude, plane.Y * magnitude, plane.Z * magnitude, plane.D * magnitude);
		}
		/// <summary>
		/// Transforms a normalized plane by a quaternion rotation.
		/// </summary>
		/// <param name="plane">   The normalized source plane.</param>
		/// <param name="rotation">The quaternion rotation.</param>
		/// <param name="result">  
		/// When the method completes, contains the transformed plane.
		/// </param>
		public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result)
		{
			float x2 = rotation.X + rotation.X;
			float y2 = rotation.Y + rotation.Y;
			float z2 = rotation.Z + rotation.Z;
			float wx = rotation.W * x2;
			float wy = rotation.W * y2;
			float wz = rotation.W * z2;
			float xx = rotation.X * x2;
			float xy = rotation.X * y2;
			float xz = rotation.X * z2;
			float yy = rotation.Y * y2;
			float yz = rotation.Y * z2;
			float zz = rotation.Z * z2;

			float x = plane.X;
			float y = plane.Y;
			float z = plane.Z;

			/*
			 * Note:
			 * Factor common arithmetic out of loop.
			*/
			result = new Plane
			{
				X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy)),
				Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx)),
				Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy)),
				D = plane.D
			};
		}
		/// <summary>
		/// Transforms a normalized plane by a quaternion rotation.
		/// </summary>
		/// <param name="plane">   The normalized source plane.</param>
		/// <param name="rotation">The quaternion rotation.</param>
		/// <returns>The transformed plane.</returns>
		public static Plane Transform(Plane plane, Quaternion rotation)
		{
			float x2 = rotation.X + rotation.X;
			float y2 = rotation.Y + rotation.Y;
			float z2 = rotation.Z + rotation.Z;
			float wx = rotation.W * x2;
			float wy = rotation.W * y2;
			float wz = rotation.W * z2;
			float xx = rotation.X * x2;
			float xy = rotation.X * y2;
			float xz = rotation.X * z2;
			float yy = rotation.Y * y2;
			float yz = rotation.Y * z2;
			float zz = rotation.Z * z2;

			float x = plane.X;
			float y = plane.Y;
			float z = plane.Z;

			/*
			 * Note:
			 * Factor common arithmetic out of loop.
			*/
			return new Plane
			(
				((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy)),
				((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx)),
				((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy)),
				plane.D
			);
		}
		/// <summary>
		/// Transforms an array of normalized planes by a quaternion rotation.
		/// </summary>
		/// <param name="planes">  The array of normalized planes to transform.</param>
		/// <param name="rotation">The quaternion rotation.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="planes"/> is <c>null</c> .
		/// </exception>
		public static void Transform(Plane[] planes, ref Quaternion rotation)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (planes == null)
				throw new ArgumentNullException("planes");
#endif

			float x2 = rotation.X + rotation.X;
			float y2 = rotation.Y + rotation.Y;
			float z2 = rotation.Z + rotation.Z;
			float wx = rotation.W * x2;
			float wy = rotation.W * y2;
			float wz = rotation.W * z2;
			float xx = rotation.X * x2;
			float xy = rotation.X * y2;
			float xz = rotation.X * z2;
			float yy = rotation.Y * y2;
			float yz = rotation.Y * z2;
			float zz = rotation.Z * z2;

			for (int i = 0; i < planes.Length; ++i)
			{
				float x = planes[i].X;
				float y = planes[i].Y;
				float z = planes[i].Z;

				/*
				 * Note:
				 * Factor common arithmetic out of loop.
				*/
				planes[i].X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
				planes[i].Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
				planes[i].Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
			}
		}
		/// <summary>
		/// Changes the coefficients of the normal vector of the plane to make it of unit
		/// length.
		/// </summary>
		public void Normalize()
		{
			float magnitudeInverse =
				1.0f / (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);

			this.X *= magnitudeInverse;
			this.Y *= magnitudeInverse;
			this.Z *= magnitudeInverse;
			this.D *= magnitudeInverse;
		}
		/// <summary>
		/// Calculates signed distance between this plane and a point.
		/// </summary>
		/// <param name="point">
		/// <see cref="Vector3"/> object that describes location of the point in 3D space.
		/// </param>
		/// <returns>
		/// Number of type <see cref="Single"/> which absolute value is a shortest
		/// distance between this plane and a given point, and which sign is negative if
		/// the point is on the backside of this plane.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float SignedDistance(Vector3 point)
		{
			return point.X * this.X + point.Y * this.Y + point.Z * this.Z - this.D;
		}
		/// <summary>
		/// Determines relative position of the point in respect to position of this
		/// plane.
		/// </summary>
		/// <remarks>
		/// This method allows to determine the relative position of the polygon by
		/// continuously invoking it for all of the vertexes and using a dedicated flags
		/// object represented by <see cref="PlanePosition"/> enumeration.
		/// </remarks>
		/// <param name="point">               
		/// <see cref="Vector3"/> object that describes location of the point in 3D space.
		/// </param>
		/// <param name="pointPlanePosition">  
		/// When the call is concluded, this value will indicate relative position of the
		/// point.
		/// </param>
		/// <param name="polygonPlanePosition">
		/// During a call bitwise Or operator will be applied to this argument with second
		/// operand being <paramref name="pointPlanePosition"/> after it is calculated.
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PointPosition(Vector3 point, out PlanePosition pointPlanePosition,
											   ref PlanePosition polygonPlanePosition)
		{
			pointPlanePosition = this.PointPosition(point);
			polygonPlanePosition |= pointPlanePosition;
		}
		/// <summary>
		/// Determines relative position of the point in respect to position of this
		/// plane.
		/// </summary>
		/// <param name="point">
		/// <see cref="Vector3"/> object that describes location of the point in 3D space.
		/// </param>
		/// <returns>
		/// <para><see cref="PlanePosition.Coplanar"/> if the point is on a plane.</para>
		/// <para>
		/// <see cref="PlanePosition.Front"/> if the point is in front of a plane.
		/// </para>
		/// <para><see cref="PlanePosition.Back"/> if the point is behind a plane.</para>
		/// </returns>
		public PlanePosition PointPosition(Vector3 point)
		{
			float signedDistance = this.SignedDistance(point);
			if (signedDistance > MathHelpers.ZeroTolerance)
			{
				return PlanePosition.Front;
			}
			return signedDistance < -MathHelpers.ZeroTolerance
				? PlanePosition.Back : PlanePosition.Coplanar;
		}
		/// <summary>
		/// Flips the orientation of this plane.
		/// </summary>
		public void Negate()
		{
			this.Normal = -this.Normal;
			this.D = -this.D;
		}
	}
	/// <summary>
	/// Enumeration of positions point or a geometric figure can be in relation to a
	/// plane.
	/// </summary>
	[Flags]
	public enum PlanePosition
	{
		/// <summary>
		/// Point or figure occupies the same plane.
		/// </summary>
		Coplanar = 0,
		/// <summary>
		/// Geometric figure is not intersecting the plane and is located inside the part
		/// of the 3D space plane's normal points towards.
		/// </summary>
		Front = 1,
		/// <summary>
		/// Geometric figure is not intersecting the plane and is located inside the part
		/// of the 3D space plane's normal points against.
		/// </summary>
		Back = 2,
		/// <summary>
		/// Geometric figure intersects the plane.
		/// </summary>
		Spanning = 3
	}
}