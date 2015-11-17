/*
* Copyright (c) 2007-2010 SlimDX Group
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents an axis-aligned bounding box in three dimensional space.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BoundingBox : IEquatable<BoundingBox>, IFormattable
	{
		#region Fields
		/// <summary>
		/// The minimum point of the box.
		/// </summary>
		public Vector3 Minimum;
		/// <summary>
		/// The maximum point of the box.
		/// </summary>
		public Vector3 Maximum;
		#endregion
		#region Properties
		/// <summary>
		/// Checks whether the bounding box is valid.
		/// </summary>
		public bool IsValid
		{
			get { return this.Minimum.IsValid && this.Maximum.IsValid; }
		}
		/// <summary>
		/// Gets an array of eight corners of the bounding box.
		/// </summary>
		public Vector3[] Corners
		{
			get
			{
				var results = new Vector3[8];
				results[0] = new Vector3(this.Minimum.X, this.Maximum.Y, this.Maximum.Z);
				results[1] = new Vector3(this.Maximum.X, this.Maximum.Y, this.Maximum.Z);
				results[2] = new Vector3(this.Maximum.X, this.Minimum.Y, this.Maximum.Z);
				results[3] = new Vector3(this.Minimum.X, this.Minimum.Y, this.Maximum.Z);
				results[4] = new Vector3(this.Minimum.X, this.Maximum.Y, this.Minimum.Z);
				results[5] = new Vector3(this.Maximum.X, this.Maximum.Y, this.Minimum.Z);
				results[6] = new Vector3(this.Maximum.X, this.Minimum.Y, this.Minimum.Z);
				results[7] = new Vector3(this.Minimum.X, this.Minimum.Y, this.Minimum.Z);
				return results;
			}
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="BoundingBox"/> struct.
		/// </summary>
		/// <param name="minimum">The minimum vertex of the bounding box.</param>
		/// <param name="maximum">The maximum vertex of the bounding box.</param>
		public BoundingBox(Vector3 minimum, Vector3 maximum)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
		}
		/// <summary>
		/// Creates a new <see cref="BoundingBox"/> that is just big enough to contain all of given points.
		/// </summary>
		/// <param name="points">
		/// An array of <see cref="Vector3"/> objects that represent the points.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="points"/> is <see langword="null"/>.
		/// </exception>
		public BoundingBox(Vector3[] points)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (points == null)
				throw new ArgumentNullException("points");
#endif

			var min = new Vector3(float.MaxValue);
			var max = new Vector3(float.MinValue);

			for (int i = 0; i < points.Length; ++i)
			{
				Vector3.Min(ref min, ref points[i], out min);
				Vector3.Max(ref max, ref points[i], out max);
			}
			this.Minimum = min;
			this.Maximum = max;
		}
		/// <summary>
		/// Creates a new <see cref="BoundingBox"/> that encompasses a sphere.
		/// </summary>
		/// <param name="sphere">A sphere that needs to be encompassed.</param>
		public BoundingBox(ref BoundingSphere sphere)
		{
			this.Minimum = new Vector3(sphere.Center.X - sphere.Radius, sphere.Center.Y - sphere.Radius,
									   sphere.Center.Z - sphere.Radius);
			this.Maximum = new Vector3(sphere.Center.X + sphere.Radius, sphere.Center.Y + sphere.Radius,
									   sphere.Center.Z + sphere.Radius);
		}
		/// <summary>
		/// Creates a new <see cref="BoundingBox"/> that encompasses a sphere.
		/// </summary>
		/// <param name="sphere">A sphere that needs to be encompassed.</param>
		public BoundingBox(BoundingSphere sphere)
		{
			this.Minimum = new Vector3(sphere.Center.X - sphere.Radius, sphere.Center.Y - sphere.Radius,
									   sphere.Center.Z - sphere.Radius);
			this.Maximum = new Vector3(sphere.Center.X + sphere.Radius, sphere.Center.Y + sphere.Radius,
									   sphere.Center.Z + sphere.Radius);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines if there is an intersection between the current object and a <see cref="Ray"/>.
		/// </summary>
		/// <param name="ray">The ray to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Ray ray)
		{
			float distance;
			return Collision.RayIntersectsBox(ref ray, ref this, out distance);
		}
		/// <summary>
		/// Determines if there is an intersection between the current object and a <see cref="Ray"/>.
		/// </summary>
		/// <param name="ray">     The ray to test.</param>
		/// <param name="distance">
		/// When the method completes, contains the distance of the intersection, or 0 if there was no
		/// intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Ray ray, out float distance)
		{
			return Collision.RayIntersectsBox(ref ray, ref this, out distance);
		}
		/// <summary>
		/// Determines if there is an intersection between the current object and a <see cref="Ray"/>.
		/// </summary>
		/// <param name="ray">  The ray to test.</param>
		/// <param name="point">
		/// When the method completes, contains the point of intersection, or <see cref="Vector3"/> if
		/// there was no intersection.
		/// </param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Ray ray, out Vector3 point)
		{
			return Collision.RayIntersectsBox(ref ray, ref this, out point);
		}
		/// <summary>
		/// Determines if there is an intersection between the current object and a <see cref="Plane"/>.
		/// </summary>
		/// <param name="plane">The plane to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public PlaneIntersectionType Intersects(ref Plane plane)
		{
			return Collision.PlaneIntersectsBox(ref plane, ref this);
		}
		/* This implementation is wrong
		/// <summary>
		/// Determines if there is an intersection between the current object and a triangle.
		/// </summary>
		/// <param name="vertex1">The first vertex of the triangle to test.</param>
		/// <param name="vertex2">The second vertex of the triagnle to test.</param>
		/// <param name="vertex3">The third vertex of the triangle to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
		{
			return Collision.BoxIntersectsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
		}
		*/

		/// <summary>
		/// Determines if there is an intersection between the current object and a
		/// <see cref="BoundingBox"/>.
		/// </summary>
		/// <param name="box">The box to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingBox box)
		{
			return Collision.BoxIntersectsBox(ref this, ref box);
		}
		/// <summary>
		/// Determines if there is an intersection between the current object and a
		/// <see cref="BoundingSphere"/>.
		/// </summary>
		/// <param name="sphere">The sphere to test.</param>
		/// <returns>Whether the two objects intersected.</returns>
		public bool Intersects(ref BoundingSphere sphere)
		{
			return Collision.BoxIntersectsSphere(ref this, ref sphere);
		}
		/// <summary>
		/// Determines whether the current objects contains a point.
		/// </summary>
		/// <param name="point">The point to test.</param>
		/// <returns>The type of containment the two objects have.</returns>
		public bool Contains(ref Vector3 point)
		{
			return Collision.BoxContainsPoint(ref this, ref point);
		}
		/* This implementation is wrong
		/// <summary>
		/// Determines whether the current objects contains a triangle.
		/// </summary>
		/// <param name="vertex1">The first vertex of the triangle to test.</param>
		/// <param name="vertex2">The second vertex of the triagnle to test.</param>
		/// <param name="vertex3">The third vertex of the triangle to test.</param>
		/// <returns>The type of containment the two objects have.</returns>
		public ContainmentType Contains(ref Vector3 vertex1, ref Vector3 vertex2, ref Vector3 vertex3)
		{
			return Collision.BoxContainsTriangle(ref this, ref vertex1, ref vertex2, ref vertex3);
		}
		*/

		/// <summary>
		/// Determines whether the current objects contains a <see cref="BoundingBox"/>.
		/// </summary>
		/// <param name="box">The box to test.</param>
		/// <returns>The type of containment the two objects have.</returns>
		public ContainmentType Contains(ref BoundingBox box)
		{
			return Collision.BoxContainsBox(ref this, ref box);
		}
		/// <summary>
		/// Determines whether the current objects contains a <see cref="BoundingSphere"/>.
		/// </summary>
		/// <param name="sphere">The sphere to test.</param>
		/// <returns>The type of containment the two objects have.</returns>
		public ContainmentType Contains(ref BoundingSphere sphere)
		{
			return Collision.BoxContainsSphere(ref this, ref sphere);
		}
		#endregion
		#region Utilities
		#endregion
		/// <summary>
		/// Constructs a <see cref="BoundingBox"/> that is as large as the total combined area of the two
		/// specified boxes.
		/// </summary>
		/// <param name="value1">The first box to merge.</param>
		/// <param name="value2">The second box to merge.</param>
		/// <param name="result">
		/// When the method completes, contains the newly constructed bounding box.
		/// </param>
		public static void Merge(ref BoundingBox value1, ref BoundingBox value2, out BoundingBox result)
		{
			Vector3.Min(ref value1.Minimum, ref value2.Minimum, out result.Minimum);
			Vector3.Max(ref value1.Maximum, ref value2.Maximum, out result.Maximum);
		}
		/// <summary>
		/// Constructs a <see cref="BoundingBox"/> that is as large as the total combined area of the two
		/// specified boxes.
		/// </summary>
		/// <param name="value1">The first box to merge.</param>
		/// <param name="value2">The second box to merge.</param>
		/// <returns>The newly constructed bounding box.</returns>
		public static BoundingBox Merge(BoundingBox value1, BoundingBox value2)
		{
			BoundingBox box;
			Vector3.Min(ref value1.Minimum, ref value2.Minimum, out box.Minimum);
			Vector3.Max(ref value1.Maximum, ref value2.Maximum, out box.Maximum);
			return box;
		}
		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==(BoundingBox left, BoundingBox right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(BoundingBox left, BoundingBox right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture,
								 "Minimum:{0} Maximum:{1}",
								 this.Minimum, this.Maximum);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(string format)
		{
			return format == null
				? this.ToString()
				: string.Format(CultureInfo.CurrentCulture,
								"Minimum:{0} Maximum:{1}",
								this.Minimum, this.Maximum);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return string.Format(formatProvider, "Minimum:{0} Maximum:{1}", this.Minimum, this.Maximum);
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="format">        The format.</param>
		/// <param name="formatProvider">The format provider.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return format == null
				? this.ToString(formatProvider)
				: string.Format(formatProvider, "Minimum:{0} Maximum:{1}", this.Minimum, this.Maximum);
		}
		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like
		/// a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + this.Minimum.GetHashCode();
				hash = hash * 29 + this.Maximum.GetHashCode();

				return hash;
			}
		}
		/// <summary>
		/// Determines whether the specified <see cref="Vector4"/> is equal to this instance.
		/// </summary>
		/// <param name="value">The <see cref="Vector4"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Vector4"/> is equal to this instance; otherwise,
		/// <c>false</c>.
		/// </returns>
		public bool Equals(BoundingBox value)
		{
			return this.Minimum == value.Minimum && this.Maximum == value.Maximum;
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise,
		/// <c>false</c>.
		/// </returns>
		public override bool Equals(object value)
		{
			return value != null
				   && (value.GetType() == this.GetType()
					   && this.Equals((BoundingBox)value));
		}
		/// <summary>
		/// Determines whether this bounding box is a point.
		/// </summary>
		#region Properties
		public bool IsEmpty
		{
			get { return this.Minimum == this.Maximum; }
		}
		/// <summary>
		/// Gets the <see cref="Vector3"/> object that encapsulates coordinates of the center of this
		/// bounding box.
		/// </summary>
		public Vector3 Center
		{
			get { return (this.Minimum + this.Maximum) * 0.5f; }
		}
		#endregion
	}

	/// <summary>
	/// Describes how one bounding volume contains another.
	/// </summary>
	public enum ContainmentType
	{
		/// <summary>
		/// The two bounding volumes don't intersect at all.
		/// </summary>
		Disjoint,
		/// <summary>
		/// One bounding volume completely contains another.
		/// </summary>
		Contains,
		/// <summary>
		/// The two bounding volumes overlap.
		/// </summary>
		Intersects
	}
}