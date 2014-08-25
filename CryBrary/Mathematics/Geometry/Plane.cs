using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryEngine.Mathematics.Geometry
{
	/// <summary>
	/// Represents a plane in three dimensional space.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public partial struct Plane : IEquatable<Plane>, IFormattable
	{
		/// <summary>
		/// The normal vector of the plane.
		/// </summary>
		public Vector3 Normal;
		/// <summary>
		/// The distance of the plane along its normal from the origin.
		/// </summary>
		public float D;
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="value">The value that will be assigned to all components.</param>
		public Plane(float value)
		{
			this.Normal.X = this.Normal.Y = this.Normal.Z = this.D = value;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="a">The X component of the normal.</param>
		/// <param name="b">The Y component of the normal.</param>
		/// <param name="c">The Z component of the normal.</param>
		/// <param name="d">The distance of the plane along its normal from the origin.</param>
		public Plane(float a, float b, float c, float d)
		{
			this.Normal.X = a;
			this.Normal.Y = b;
			this.Normal.Z = c;
			this.D = d;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="value">The normal of the plane.</param>
		/// <param name="d">    The distance of the plane along its normal from the origin</param>
		public Plane(Vector3 value, float d)
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
		{
			this.Normal = normal;
			this.D = -Vector3.Dot(normal, point);
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="point1">First point of a triangle defining the plane.</param>
		/// <param name="point2">Second point of a triangle defining the plane.</param>
		/// <param name="point3">Third point of a triangle defining the plane.</param>
		public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
		{
#if DEBUG
			this.Normal = ((point2 - point1) % (point3 - point1)).Normalized;
			this.D = this.Normal * point1;
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

			Normal.X = yz * invPyth;
			Normal.Y = xz * invPyth;
			Normal.Z = xy * invPyth;
			D = -((Normal.X * point1.X) + (Normal.Y * point1.Y) + (Normal.Z * point1.Z));
#endif
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Plane"/> struct.
		/// </summary>
		/// <param name="values">
		/// The values to assign to the A, B, C, and D components of the plane. This must be an
		/// array with four elements.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="values"/> is <c>null</c> .
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when <paramref name="values"/> contains more or less than four elements.
		/// </exception>
		public Plane(IList<float> values)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Count != 4)
				throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for Plane.");
#endif

			this.Normal.X = values[0];
			this.Normal.Y = values[1];
			this.Normal.Z = values[2];
			this.D = values[3];
		}
		/// <summary>
		/// Gets or sets the component at the specified index.
		/// </summary>
		/// <value>The value of the A, B, C, or D component, depending on the index.</value>
		/// <param name="index">
		/// The index of the component to access. Use 0 for the A component, 1 for the B component,
		/// 2 for the C component, and 3 for the D component.
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
					case 0: return this.Normal.X;
					case 1: return this.Normal.Y;
					case 2: return this.Normal.Z;
					case 3: return this.D;
				}

				throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
			}

			set
			{
				switch (index)
				{
					case 0: this.Normal.X = value;
						break;
					case 1: this.Normal.Y = value;
						break;
					case 2: this.Normal.Z = value;
						break;
					case 3: this.D = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("index", "Indices for Plane run from 0 to 3, inclusive.");
				}
			}
		}
		/// <summary>
		/// Changes the coefficients of the normal vector of the plane to make it of unit length.
		/// </summary>
		/// <param name="plane"> The source plane.</param>
		/// <param name="result">When the method completes, contains the normalized plane.</param>
		public static void Normalize(ref Plane plane, out Plane result)
		{
			float magnitude = 1.0f / (float)Math.Sqrt((plane.Normal.X * plane.Normal.X) + (plane.Normal.Y * plane.Normal.Y) + (plane.Normal.Z * plane.Normal.Z));

			result.Normal.X = plane.Normal.X * magnitude;
			result.Normal.Y = plane.Normal.Y * magnitude;
			result.Normal.Z = plane.Normal.Z * magnitude;
			result.D = plane.D * magnitude;
		}
		/// <summary>
		/// Changes the coefficients of the normal vector of the plane to make it of unit length.
		/// </summary>
		/// <param name="plane">The source plane.</param>
		/// <returns>The normalized plane.</returns>
		public static Plane Normalize(Plane plane)
		{
			float magnitude = 1.0f / (float)Math.Sqrt((plane.Normal.X * plane.Normal.X) + (plane.Normal.Y * plane.Normal.Y) + (plane.Normal.Z * plane.Normal.Z));
			return new Plane(plane.Normal.X * magnitude, plane.Normal.Y * magnitude, plane.Normal.Z * magnitude, plane.D * magnitude);
		}
		/// <summary>
		/// Transforms a normalized plane by a quaternion rotation.
		/// </summary>
		/// <param name="plane">   The normalized source plane.</param>
		/// <param name="rotation">The quaternion rotation.</param>
		/// <param name="result">  When the method completes, contains the transformed plane.</param>
		public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result)
		{
			float x2 = rotation.V.X + rotation.V.X;
			float y2 = rotation.V.Y + rotation.V.Y;
			float z2 = rotation.V.Z + rotation.V.Z;
			float wx = rotation.W * x2;
			float wy = rotation.W * y2;
			float wz = rotation.W * z2;
			float xx = rotation.V.X * x2;
			float xy = rotation.V.X * y2;
			float xz = rotation.V.X * z2;
			float yy = rotation.V.Y * y2;
			float yz = rotation.V.Y * z2;
			float zz = rotation.V.Z * z2;

			float x = plane.Normal.X;
			float y = plane.Normal.Y;
			float z = plane.Normal.Z;

			/*
			 * Note:
			 * Factor common arithmetic out of loop.
			*/
			result.Normal.X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
			result.Normal.Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
			result.Normal.Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
			result.D = plane.D;
		}
		/// <summary>
		/// Transforms a normalized plane by a quaternion rotation.
		/// </summary>
		/// <param name="plane">   The normalized source plane.</param>
		/// <param name="rotation">The quaternion rotation.</param>
		/// <returns>The transformed plane.</returns>
		public static Plane Transform(Plane plane, Quaternion rotation)
		{
			Plane result;
			float x2 = rotation.V.X + rotation.V.X;
			float y2 = rotation.V.Y + rotation.V.Y;
			float z2 = rotation.V.Z + rotation.V.Z;
			float wx = rotation.W * x2;
			float wy = rotation.W * y2;
			float wz = rotation.W * z2;
			float xx = rotation.V.X * x2;
			float xy = rotation.V.X * y2;
			float xz = rotation.V.X * z2;
			float yy = rotation.V.Y * y2;
			float yz = rotation.V.Y * z2;
			float zz = rotation.V.Z * z2;

			float x = plane.Normal.X;
			float y = plane.Normal.Y;
			float z = plane.Normal.Z;

			/*
			 * Note:
			 * Factor common arithmetic out of loop.
			*/
			result.Normal.X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
			result.Normal.Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
			result.Normal.Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
			result.D = plane.D;

			return result;
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

			float x2 = rotation.V.X + rotation.V.X;
			float y2 = rotation.V.Y + rotation.V.Y;
			float z2 = rotation.V.Z + rotation.V.Z;
			float wx = rotation.W * x2;
			float wy = rotation.W * y2;
			float wz = rotation.W * z2;
			float xx = rotation.V.X * x2;
			float xy = rotation.V.X * y2;
			float xz = rotation.V.X * z2;
			float yy = rotation.V.Y * y2;
			float yz = rotation.V.Y * z2;
			float zz = rotation.V.Z * z2;

			for (int i = 0; i < planes.Length; ++i)
			{
				float x = planes[i].Normal.X;
				float y = planes[i].Normal.Y;
				float z = planes[i].Normal.Z;

				/*
				 * Note:
				 * Factor common arithmetic out of loop.
				*/
				planes[i].Normal.X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
				planes[i].Normal.Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
				planes[i].Normal.Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
			}
		}
		/// <summary>
		/// Changes the coefficients of the normal vector of the plane to make it of unit length.
		/// </summary>
		public void Normalize()
		{
			float magnitude = 1.0f / (float)Math.Sqrt((this.Normal.X * this.Normal.X) + (this.Normal.Y * this.Normal.Y) + (this.Normal.Z * this.Normal.Z));

			this.Normal.X *= magnitude;
			this.Normal.Y *= magnitude;
			this.Normal.Z *= magnitude;
			this.D *= magnitude;
		}
		/// <summary>
		/// Calculates signed distance between this plane and a point.
		/// </summary>
		/// <param name="point">
		/// <see cref="Vector3"/> object that describes location of the point in 3D space.
		/// </param>
		/// <returns>
		/// Number of type <see cref="Single"/> which absolute value is a shortest distance between
		/// this plane and a given point, and which sign is negative if the point is on the backside
		/// of this plane.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float SignedDistance(Vector3 point)
		{
			return point * this.Normal - this.D;
		}
		/// <summary>
		/// Determines relative position of the point in respect to position of this plane.
		/// </summary>
		/// <param name="point">          
		/// <see cref="Vector3"/> object that describes location of the point in 3D space.
		/// </param>
		/// <param name="pointPlanePosition">  
		/// When the call is concluded, this value will indicate relative position of the point.
		/// </param>
		/// <param name="polygonPlanePosition">
		/// During a call bitwise Or operator will be applied to this argument with second operand
		/// being <paramref name="pointPlanePosition"/> after it is calculated.
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PointPosition(Vector3 point, out PlanePosition pointPlanePosition,
											   ref PlanePosition polygonPlanePosition)
		{
			pointPlanePosition = this.PointPosition(point);
			polygonPlanePosition |= pointPlanePosition;
		}
		/// <summary>
		/// Determines relative position of the point in respect to position of this plane.
		/// </summary>
		/// <param name="point">
		/// <see cref="Vector3"/> object that describes location of the point in 3D space.
		/// </param>
		/// <returns>
		/// <para><see cref="PlanePosition.Coplanar"/> if the point is on a plane.</para>
		/// <para><see cref="PlanePosition.Front"/> if the point is in front of a plane.</para>
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
		public void Flip()
		{
			this.Normal = -this.Normal;
			this.D = -this.D;
		}
	}
	/// <summary>
	/// Enumeration of positions point or a geometric figure can be in relation to a plane.
	/// </summary>
	[Flags]
	public enum PlanePosition
	{
		/// <summary>
		/// Point or figure occupies the same plane.
		/// </summary>
		Coplanar = 0,
		/// <summary>
		/// Geometric figure is not intersecting the plane and is located inside the part of the 3D
		/// space plane's normal points towards.
		/// </summary>
		Front = 1,
		/// <summary>
		/// Geometric figure is not intersecting the plane and is located inside the part of the 3D
		/// space plane's normal points against.
		/// </summary>
		Back = 2,
		/// <summary>
		/// Geometric figure intersects the plane.
		/// </summary>
		Spanning = 3
	}
}