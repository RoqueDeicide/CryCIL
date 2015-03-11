﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Geometry;

namespace CryCil
{
	/// <summary>
	/// Represents a 3x3 matrix.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct Matrix33 : IMatrix<Matrix33>, IFormattable
	{
		#region Static Fields
		/// <summary>
		/// Identity matrix.
		/// </summary>
		public static readonly Matrix33 Identity = new Matrix33
		(
			1, 0, 0,
			0, 1, 0,
			0, 0, 1
		);
		#endregion
		#region Fields
		#region Individual Elements
		/// <summary>
		/// First row, First Column.
		/// </summary>
		[FieldOffset(0)]
		public float M00;
		/// <summary>
		/// First row, Second Column.
		/// </summary>
		[FieldOffset(4)]
		public float M01;
		/// <summary>
		/// First row, Third Column.
		/// </summary>
		[FieldOffset(8)]
		public float M02;
		/// <summary>
		/// Second row, First Column.
		/// </summary>
		[FieldOffset(12)]
		public float M10;
		/// <summary>
		/// Second row, Second Column.
		/// </summary>
		[FieldOffset(16)]
		public float M11;
		/// <summary>
		/// Second row, Third Column.
		/// </summary>
		[FieldOffset(20)]
		public float M12;
		/// <summary>
		/// Third row, First Column.
		/// </summary>
		[FieldOffset(24)]
		public float M20;
		/// <summary>
		/// Third row, Second Column.
		/// </summary>
		[FieldOffset(28)]
		public float M21;
		/// <summary>
		/// Third row, Third Column.
		/// </summary>
		[FieldOffset(32)]
		public float M22;
		#endregion
		#region Rows
		/// <summary>
		/// First row.
		/// </summary>
		[FieldOffset(0)]
		public Vector3 Row0;
		/// <summary>
		/// Second row.
		/// </summary>
		[FieldOffset(12)]
		public Vector3 Row1;
		/// <summary>
		/// Third row.
		/// </summary>
		[FieldOffset(24)]
		public Vector3 Row2;
		#endregion
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets first column of this matrix.
		/// </summary>
		public Vector3 Column0
		{
			get
			{
				return new Vector3(this.M00, this.M10, this.M20);
			}
			set
			{
				this.M00 = value.X;
				this.M10 = value.Y;
				this.M20 = value.Z;
			}
		}
		/// <summary>
		/// Gets or sets first column of this matrix.
		/// </summary>
		public Vector3 Column1
		{
			get
			{
				return new Vector3(this.M01, this.M11, this.M21);
			}
			set
			{
				this.M01 = value.X;
				this.M11 = value.Y;
				this.M21 = value.Z;
			}
		}
		/// <summary>
		/// Gets or sets first column of this matrix.
		/// </summary>
		public Vector3 Column2
		{
			get
			{
				return new Vector3(this.M02, this.M12, this.M22);
			}
			set
			{
				this.M02 = value.X;
				this.M12 = value.Y;
				this.M22 = value.Z;
			}
		}
		/// <summary>
		/// Gets angles that represent rotation this matrix represents.
		/// </summary>
		public Vector3 AnglesVector
		{
			get
			{
				var angles = new Vector3
				{
					Y = (float)Math.Asin(Math.Max(-1.0, Math.Min(1.0, -this.M20)))
				};

				if (Math.Abs(Math.Abs(angles.Y) - (Math.PI * 0.5)) < 0.01)
				{
					angles.X = 0;
					angles.Z = (float)Math.Atan2(-this.M01, this.M11);
				}
				else
				{
					angles.X = (float)Math.Atan2(this.M21, this.M22);
					angles.Z = (float)Math.Atan2(this.M10, this.M00);
				}

				return angles;
			}
		}
		/// <summary>
		/// Gets angles that represent rotation this matrix represents.
		/// </summary>
		public EulerAngles Angles
		{
			get
			{
				var angles = new EulerAngles
				{
					Roll = (float)Math.Asin(Math.Max(-1.0, Math.Min(1.0, -this.M20)))
				};

				if (Math.Abs(Math.Abs(angles.Roll) - (Math.PI * 0.5)) < 0.01)
				{
					angles.Pitch = 0;
					angles.Yaw = (float)Math.Atan2(-this.M01, this.M11);
				}
				else
				{
					angles.Pitch = (float)Math.Atan2(this.M21, this.M22);
					angles.Yaw = (float)Math.Atan2(this.M10, this.M00);
				}

				return angles;
			}
		}
		/// <summary>
		/// Determines whether all elements of this matrix are valid numbers.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return this.All(MathHelpers.IsNumberValid);
			}
		}
		/// <summary>
		/// Determines whether this matrix is orthonormal.
		/// </summary>
		public bool IsOrthonormal
		{
			get
			{
				Vector3 x = this.Column0;
				Vector3 y = this.Column1;
				Vector3 z = this.Column2;

				return
					Math.Abs(x * y) <= MathHelpers.ZeroTolerance &&
					Math.Abs(x * z) <= MathHelpers.ZeroTolerance &&
					Math.Abs(y * z) <= MathHelpers.ZeroTolerance &&
					Math.Abs(1 - x * x) < MathHelpers.ZeroTolerance &&
					Math.Abs(1 - y * y) < MathHelpers.ZeroTolerance &&
					Math.Abs(1 - z * z) < MathHelpers.ZeroTolerance;
			}
		}
		/// <summary>
		/// Determines whether this matrix is orthonormal in right-handed coordinate system.
		/// </summary>
		public bool IsOrthonormalRightHanded
		{
			get
			{
				Vector3 x = this.Column0;
				Vector3 y = this.Column1;
				Vector3 z = this.Column2;

				return
					x.IsEquivalent(y % z) && x.IsUnit(MathHelpers.ZeroTolerance) &&
					y.IsEquivalent(z % x) && y.IsUnit(MathHelpers.ZeroTolerance) &&
					z.IsEquivalent(x % y) && z.IsUnit(MathHelpers.ZeroTolerance);
			}
		}
		/// <summary>
		/// Calculates determinant of this matrix.
		/// </summary>
		public float Determinant
		{
			get
			{
				return
					   (this.M00 * this.M11 * this.M22) +
					   (this.M01 * this.M12 * this.M20) +
					   (this.M02 * this.M10 * this.M21) -
					   (this.M02 * this.M11 * this.M20) -
					   (this.M00 * this.M12 * this.M21) -
					   (this.M01 * this.M10 * this.M22);
			}
		}
		/// <summary>
		/// Gets a 2D array of elements of this matrix.
		/// </summary>
		public float[,] Array2D
		{
			get
			{
				return new[,]
				{
					{this.M00, this.M01, this.M02},
					{this.M10, this.M11, this.M12},
					{this.M20, this.M21, this.M22}
				};
			}
		}
		/// <summary>
		/// Gives access to specific element of this matrix.
		/// </summary>
		/// <param name="row">   Zero-based index of the row.</param>
		/// <param name="column">Zero-based index of the column.</param>
		public float this[int row, int column]
		{
			get
			{
				switch (row)
				{
					case 0:
						return this.Row0[column];
					case 1:
						return this.Row1[column];
					case 2:
						return this.Row2[column];
					default:
						throw new ArgumentOutOfRangeException("row", "Attempt to access matrix row out of range [0, 2].");
				}
			}
			set
			{
				switch (row)
				{
					case 0:
						this.Row0[column] = value;
						break;
					case 1:
						this.Row1[column] = value;
						break;
					case 2:
						this.Row2[column] = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("row", "Attempt to access matrix row out of range [0, 2].");
				}
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of <see cref="Matrix33"/> struct.
		/// </summary>
		/// <param name="m00">First row, First Column.</param>
		/// <param name="m01">First row, Second Column.</param>
		/// <param name="m02">First row, Third Column.</param>
		/// <param name="m10">Second row, First Column.</param>
		/// <param name="m11">Second row, Second Column.</param>
		/// <param name="m12">Second row, Third Column.</param>
		/// <param name="m20">Third row, First Column.</param>
		/// <param name="m21">Third row, Second Column.</param>
		/// <param name="m22">Third row, Third Column.</param>
		public Matrix33
		(
			float m00, float m01, float m02,
			float m10, float m11, float m12,
			float m20, float m21, float m22
		)
			: this()
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
			this.M20 = m20;
			this.M21 = m21;
			this.M22 = m22;
		}
		/// <summary>
		/// Creates new instance of <see cref="Matrix33"/> struct.
		/// </summary>
		/// <param name="m">
		/// <see cref="Matrix34"/> object from which first 3 columns will be copied.
		/// </param>
		public Matrix33(Matrix34 m)
			: this()
		{
			this.M00 = m.M00;
			this.M01 = m.M01;
			this.M02 = m.M02;

			this.M10 = m.M10;
			this.M11 = m.M11;
			this.M12 = m.M12;

			this.M20 = m.M20;
			this.M21 = m.M21;
			this.M22 = m.M22;
		}
		/// <summary>
		/// Creates new instance of <see cref="Matrix33"/> struct.
		/// </summary>
		/// <param name="m">
		/// <see cref="Matrix44"/> object from which first 3 columns will be copied.
		/// </param>
		public Matrix33(Matrix44 m)
			: this()
		{
			this.M00 = m.M00;
			this.M01 = m.M01;
			this.M02 = m.M02;

			this.M10 = m.M10;
			this.M11 = m.M11;
			this.M12 = m.M12;

			this.M20 = m.M20;
			this.M21 = m.M21;
			this.M22 = m.M22;
		}
		/// <summary>
		/// Creates new instance of <see cref="Matrix33"/> struct.
		/// </summary>
		/// <param name="q">Quaternion that represents rotation that new instance should represent.</param>
		/// <returns>New instance of <see cref="Matrix33"/> struct.</returns>
		public Matrix33(Quaternion q)
			: this()
		{
			var v2 = q.Vector + q.Vector;
			var xx = 1 - v2.X * q.X;
			var yy = v2.Y * q.Y;
			var xw = v2.X * q.W;

			var xy = v2.Y * q.X;
			var yz = v2.Z * q.Y;
			var yw = v2.Y * q.W;

			var xz = v2.Z * q.X;
			var zz = v2.Z * q.Z;
			var zw = v2.Z * q.W;

			this.M00 = 1 - yy - zz;
			this.M01 = xy - zw;
			this.M02 = xz + yw;

			this.M10 = xy + zw;
			this.M11 = xx - zz;
			this.M12 = yz - xw;

			this.M20 = xz - yw;
			this.M21 = yz + xw;
			this.M22 = xx - yy;
		}
		#endregion
		#region Interface
		#region Basic Manipulation
		/// <summary>
		/// Turns this matrix into identity matrix.
		/// </summary>
		public void SetIdentity()
		{
			this.M00 = 1;
			this.M01 = 0;
			this.M02 = 0;

			this.M10 = 0;
			this.M11 = 1;
			this.M12 = 0;

			this.M20 = 0;
			this.M21 = 0;
			this.M22 = 1;
		}
		/// <summary>
		/// Sets rows from given vectors.
		/// </summary>
		/// <param name="vx"><see cref="Vector3"/> object that contains first row.</param>
		/// <param name="vy"><see cref="Vector3"/> object that contains second row.</param>
		/// <param name="vz"><see cref="Vector3"/> object that contains third row.</param>
		public void SetFromVectors(Vector3 vx, Vector3 vy, Vector3 vz)
		{
			this.M00 = vx.X; this.M01 = vy.X; this.M02 = vz.X;
			this.M10 = vx.Y; this.M11 = vy.Y; this.M12 = vz.Y;
			this.M20 = vx.Z; this.M21 = vy.Z; this.M22 = vz.Z;
		}
		/// <summary>
		/// Creates new matrix from vectors that represent rows.
		/// </summary>
		/// <param name="vx"><see cref="Vector3"/> object that contains first row.</param>
		/// <param name="vy"><see cref="Vector3"/> object that contains second row.</param>
		/// <param name="vz"><see cref="Vector3"/> object that contains third row.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateFromVectors(Vector3 vx, Vector3 vy, Vector3 vz)
		{
			var matrix = new Matrix33();
			matrix.SetFromVectors(vx, vy, vz);

			return matrix;
		}
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + this.M00.GetHashCode();
				hash = hash * 29 + this.M01.GetHashCode();
				hash = hash * 29 + this.M02.GetHashCode();

				hash = hash * 29 + this.M10.GetHashCode();
				hash = hash * 29 + this.M11.GetHashCode();
				hash = hash * 29 + this.M12.GetHashCode();

				hash = hash * 29 + this.M20.GetHashCode();
				hash = hash * 29 + this.M21.GetHashCode();
				hash = hash * 29 + this.M22.GetHashCode();

				return hash;
			}
		}
		/// <summary>
		/// Determines whether this matrix can be considered equal to another one within bounds of given
		/// precision.
		/// </summary>
		/// <param name="m">Another matrix.</param>
		/// <param name="e">
		/// <see cref="Single"/> object that represents maximal difference between two values that allows
		/// to consider them equal.
		/// </param>
		/// <returns>
		/// True, if difference between values of each corresponding pair is less then <paramref name="e"/>
		/// .
		/// </returns>
		public bool IsEquivalent(Matrix33 m, float e = 0.05f)
		{
			return
			(
				(Math.Abs(this.M00 - m.M00) <= e) &&
				(Math.Abs(this.M01 - m.M01) <= e) &&
				(Math.Abs(this.M02 - m.M02) <= e) &&
				(Math.Abs(this.M10 - m.M10) <= e) &&
				(Math.Abs(this.M11 - m.M11) <= e) &&
				(Math.Abs(this.M12 - m.M12) <= e) &&
				(Math.Abs(this.M20 - m.M20) <= e) &&
				(Math.Abs(this.M21 - m.M21) <= e) &&
				(Math.Abs(this.M22 - m.M22) <= e)
			);
		}
		/// <summary>
		/// Swaps columns and rows of this matrix.
		/// </summary>
		public void Transpose()
		{
			this =
				new Matrix33
				(
					this.M00, this.M10, this.M20,
					this.M01, this.M11, this.M21,
					this.M02, this.M12, this.M22
				);
		}
		/// <summary>
		/// Inverts this matrix.
		/// </summary>
		public void Invert()
		{
			float det = this.Determinant;
			if (det < MathHelpers.ZeroTolerance)
			{
				throw new DivideByZeroException("Cannot invert the matrix which determinant is equal to 0.");
			}
			// Co-factors.
			float m00 = (this.M11 * this.M22 - this.M21 * this.M12) / det;
			float m01 = (this.M12 * this.M20 - this.M22 * this.M10) / det;
			float m02 = (this.M10 * this.M21 - this.M20 * this.M11) / det;
			float m10 = (this.M02 * this.M21 - this.M22 * this.M01) / det;
			float m11 = (this.M22 * this.M00 - this.M02 * this.M20) / det;
			float m12 = (this.M20 * this.M01 - this.M00 * this.M21) / det;
			float m20 = (this.M12 * this.M01 - this.M02 * this.M11) / det;
			float m21 = (this.M02 * this.M10 - this.M12 * this.M00) / det;
			float m22 = (this.M00 * this.M11 - this.M10 * this.M01) / det;
			// Construct the matrix (transposed).
			this =
				new Matrix33
				(
					m00, m10, m20,
					m01, m11, m21,
					m02, m12, m22
				);
		}
		/// <summary>
		/// Determines whether this matrix is equal to another.
		/// </summary>
		/// <param name="other">Another matrix.</param>
		/// <returns>True, if matrices are equal.</returns>
		public bool Equals(Matrix33 other)
		{
			return this.IsEquivalent(other, MathHelpers.ZeroTolerance);
		}
		#endregion
		#region Text Conversions
		/// <summary>
		/// Creates text representation of this matrix.
		/// </summary>
		/// <returns>
		/// Text representation of this matrix where all elements are listed in a line using default format
		/// for <see cref="Single"/> numbers and culture object specified by
		/// <see cref="Defaults.CultureToStringOnly"/>.
		/// </returns>
		public override string ToString()
		{
			return MatrixTextConverter.ToString(this, Defaults.CultureToStringOnly);
		}
		/// <summary>
		/// Creates text representation of this matrix.
		/// </summary>
		/// <param name="format">
		/// A string that describes a format of this matrix. See Remarks section in
		/// <see cref="Matrix44.ToString(string,IFormatProvider)"/> for details.
		/// </param>
		/// <returns>
		/// Text representation of this matrix formatted as specified by <paramref name="format"/> argument
		/// using culture object specified by <see cref="Defaults.CultureToStringOnly"/>.
		/// </returns>
		public string ToString(string format)
		{
			return MatrixTextConverter.ToString(this, format, Defaults.CultureToStringOnly);
		}
		/// <summary>
		/// Creates text representation of this matrix.
		/// </summary>
		/// <param name="formatProvider">
		/// Object that provides culture-specific information to use in formatting.
		/// </param>
		/// <returns>
		/// Text representation of this matrix where all elements are listed in a line using default format
		/// for <see cref="Single"/> numbers and culture-specific information supplied by
		/// <paramref name="formatProvider"/>.
		/// </returns>
		public string ToString(IFormatProvider formatProvider)
		{
			return MatrixTextConverter.ToString(this, formatProvider);
		}
		/// <summary>
		/// Creates text representation of this matrix. <see cref="MatrixTextConverter"/> documentation for
		/// details.
		/// </summary>
		/// <param name="format">        
		/// A string that describes a format of this matrix. See Remarks section for details.
		/// </param>
		/// <param name="formatProvider">
		/// Object that provides culture-specific information on how to create text representations of
		/// numbers.
		/// </param>
		/// <returns>Text representation specified by given arguments.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return MatrixTextConverter.ToString(this, format, formatProvider);
		}
		/// <summary>
		/// Creates text representation of this matrix.
		/// </summary>
		/// <param name="format">Object that provides details on how to format the text.</param>
		/// <returns>Formatted text representation of the matrix.</returns>
		public string ToString(MatrixTextFormat format)
		{
			return MatrixTextConverter.ToString(this, format);
		}
		#endregion
		#region Enumeration
		/// <summary>
		/// Enumerates this matrix.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<float> GetEnumerator()
		{
			yield return this.M00;
			yield return this.M01;
			yield return this.M02;
			yield return this.M10;
			yield return this.M11;
			yield return this.M12;
			yield return this.M10;
			yield return this.M21;
			yield return this.M22;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
		#endregion
		#region Operators
		/// <summary>
		/// Multiplies the matrix by float number.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right opearnd.</param>
		/// <returns>Result of operation.</returns>
		public static Matrix33 operator *(Matrix33 left, float right)
		{
			var m33 = left;
			m33.M00 *= right; m33.M01 *= right; m33.M02 *= right;
			m33.M10 *= right; m33.M11 *= right; m33.M12 *= right;
			m33.M20 *= right; m33.M21 *= right; m33.M22 *= right;
			return m33;
		}
		/// <summary>
		/// Divides the matrix by float number.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right opearnd.</param>
		/// <returns>Result of operation.</returns>
		public static Matrix33 operator /(Matrix33 left, float right)
		{
			var m33 = left;
			var iop = 1.0f / right;
			m33.M00 *= iop; m33.M01 *= iop; m33.M02 *= iop;
			m33.M10 *= iop; m33.M11 *= iop; m33.M12 *= iop;
			m33.M20 *= iop; m33.M21 *= iop; m33.M22 *= iop;
			return m33;
		}
		/// <summary>
		/// Multiplies the matrix by other matrix.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right opearnd.</param>
		/// <returns>Result of operation.</returns>
		public static Matrix33 operator *(Matrix33 left, Matrix33 right)
		{
			var m = new Matrix33
			{
				M00 = left.M00 * right.M00 + left.M01 * right.M10 + left.M02 * right.M20,
				M01 = left.M00 * right.M01 + left.M01 * right.M11 + left.M02 * right.M21,
				M02 = left.M00 * right.M02 + left.M01 * right.M12 + left.M02 * right.M22,
				M10 = left.M10 * right.M00 + left.M11 * right.M10 + left.M12 * right.M20,
				M11 = left.M10 * right.M01 + left.M11 * right.M11 + left.M12 * right.M21,
				M12 = left.M10 * right.M02 + left.M11 * right.M12 + left.M12 * right.M22,
				M20 = left.M20 * right.M00 + left.M21 * right.M10 + left.M22 * right.M20,
				M21 = left.M20 * right.M01 + left.M21 * right.M11 + left.M22 * right.M21,
				M22 = left.M20 * right.M02 + left.M21 * right.M12 + left.M22 * right.M22
			};
			return m;
		}
		/// <summary>
		/// Multiplies the matrix by vector.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right opearnd.</param>
		/// <returns>Result of operation.</returns>
		public static Vector3 operator *(Matrix33 left, Vector3 right)
		{
			return new Vector3(right.X * left.M00 + right.Y * left.M01 + right.Z * left.M02,
				right.X * left.M10 + right.Y * left.M11 + right.Z * left.M12,
				right.X * left.M20 + right.Y * left.M21 + right.Z * left.M22);
		}
		#endregion
	}
}