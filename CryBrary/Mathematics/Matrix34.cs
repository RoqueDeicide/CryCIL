using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryEngine.Mathematics
{
	/// <summary>
	/// Represents 3x4 matrix that can be used to store information about rotation and translation.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 48)]
	public struct Matrix34 : IEnumerable<float>
	{
		#region Static Fields
		/// <summary>
		/// Identity matrix.
		/// </summary>
		/// <remarks>
		/// Identity matrix is a matrix where all elements on main diagonal are equal to 1, and all
		/// others are zeroed.
		/// </remarks>
		public static readonly Matrix34 Identity = new Matrix34
		(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, 0
		);
		#endregion
		#region Fields
		#region Individual Elements
		/// <summary>
		/// First row, First column.
		/// </summary>
		[FieldOffset(0)]
		public float M00;
		/// <summary>
		/// First row, Second column.
		/// </summary>
		[FieldOffset(4)]
		public float M01;
		/// <summary>
		/// First row, Third column.
		/// </summary>
		[FieldOffset(8)]
		public float M02;
		/// <summary>
		/// First row, Fourth column.
		/// </summary>
		[FieldOffset(12)]
		public float M03;
		/// <summary>
		/// Second row, First column.
		/// </summary>
		[FieldOffset(16)]
		public float M10;
		/// <summary>
		/// Second row, Second column.
		/// </summary>
		[FieldOffset(20)]
		public float M11;
		/// <summary>
		/// Second row, Third column.
		/// </summary>
		[FieldOffset(24)]
		public float M12;
		/// <summary>
		/// Second row, Fourth column.
		/// </summary>
		[FieldOffset(28)]
		public float M13;
		/// <summary>
		/// Third row, First column.
		/// </summary>
		[FieldOffset(32)]
		public float M20;
		/// <summary>
		/// Third row, Second column.
		/// </summary>
		[FieldOffset(36)]
		public float M21;
		/// <summary>
		/// Third row, Third column.
		/// </summary>
		[FieldOffset(40)]
		public float M22;
		/// <summary>
		/// Third row, Fourth column.
		/// </summary>
		[FieldOffset(44)]
		public float M23;
		#endregion
		#region Rows
		/// <summary>
		/// First row.
		/// </summary>
		[FieldOffset(0)]
		public Vector4 Row0;
		/// <summary>
		/// Second row.
		/// </summary>
		[FieldOffset(16)]
		public Vector4 Row1;
		/// <summary>
		/// Third row.
		/// </summary>
		[FieldOffset(32)]
		public Vector4 Row2;
		#endregion
		#endregion
		#region Properties
		/// <summary>
		/// Gets angles of rotations around fixed axes that are represented by this matrix.
		/// </summary>
		public Vector3 Angles
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
		/// Gets inverted variation of this matrix.
		/// </summary>
		public Matrix34 Inverted
		{
			get
			{
				var dst = this;

				dst.Invert();

				return dst;
			}
		}
		/// <summary>
		/// Gets inverted variation of this matrix.
		/// </summary>
		public Matrix34 InvertedFast
		{
			get
			{
				return new Matrix34
				{
					M00 = this.M00,
					M01 = this.M10,
					M02 = this.M20,
					M03 = -this.M03 * this.M00 - this.M13 * this.M10 - this.M23 * this.M20,
					M10 = this.M01,
					M11 = this.M11,
					M12 = this.M21,
					M13 = -this.M03 * this.M01 - this.M13 * this.M11 - this.M23 * this.M21,
					M20 = this.M02,
					M21 = this.M12,
					M22 = this.M22,
					M23 = -this.M03 * this.M02 - this.M13 * this.M12 - this.M23 * this.M22
				};
			}
		}
		/// <summary>
		/// Gets first column in a for of a <see cref="Vector3"/> .
		/// </summary>
		public Vector3 ColumnVector0 { get { return new Vector3(this.M00, this.M10, this.M20); } }
		/// <summary>
		/// Gets second column in a for of a <see cref="Vector3"/> .
		/// </summary>
		public Vector3 ColumnVector1 { get { return new Vector3(this.M01, this.M11, this.M21); } }
		/// <summary>
		/// Gets third column in a for of a <see cref="Vector3"/> .
		/// </summary>
		public Vector3 ColumnVector2 { get { return new Vector3(this.M02, this.M12, this.M22); } }
		/// <summary>
		/// Gets fourth column in a for of a <see cref="Vector3"/> .
		/// </summary>
		public Vector3 ColumnVector3 { get { return new Vector3(this.M03, this.M13, this.M23); } }
		/// <summary>
		/// Gets translation vector from this matrix.
		/// </summary>
		public Vector3 Translation { get { return new Vector3(this.M03, this.M13, this.M23); } }
		/// <summary>
		/// Calculates determinant of the matrix composed from first 3 columns of this matrix.
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
		/// Gets or sets first 3 columns of this matrix.
		/// </summary>
		public Matrix33 Matrix33
		{
			get
			{
				return
					new Matrix33
					(
						this.M00, this.M01, this.M02,
						this.M10, this.M11, this.M12,
						this.M20, this.M21, this.M22
					);
			}
			set
			{
				this.M00 = value.M00; this.M01 = value.M01; this.M02 = value.M02;
				this.M10 = value.M10; this.M11 = value.M11; this.M12 = value.M12;
				this.M20 = value.M20; this.M21 = value.M21; this.M22 = value.M22;
			}
		}
		/// <summary>
		/// Determines whether all elements of this matrix are valid numbers.
		/// </summary>
		public bool IsValid
		{
			get
			{
#if DEBUG
				return this.All(MathHelpers.IsNumberValid);
#endif
#if RELEASE
				return
					MathHelpers.IsNumberValid(this.M00) &&
					MathHelpers.IsNumberValid(this.M01) &&
					MathHelpers.IsNumberValid(this.M02) &&
					MathHelpers.IsNumberValid(this.M03) &&
					MathHelpers.IsNumberValid(this.M10) &&
					MathHelpers.IsNumberValid(this.M11) &&
					MathHelpers.IsNumberValid(this.M12) &&
					MathHelpers.IsNumberValid(this.M13) &&
					MathHelpers.IsNumberValid(this.M20) &&
					MathHelpers.IsNumberValid(this.M21) &&
					MathHelpers.IsNumberValid(this.M22) &&
					MathHelpers.IsNumberValid(this.M23);
#endif
			}
		}
		#endregion Properties
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="Matrix34"/> .
		/// </summary>
		/// <param name="v00">First row, First column.</param>
		/// <param name="v01">First row, Second column.</param>
		/// <param name="v02">First row, Third column.</param>
		/// <param name="v03">First row, Fourth column.</param>
		/// <param name="v10">Second row, First column.</param>
		/// <param name="v11">Second row, Second column.</param>
		/// <param name="v12">Second row, Third column.</param>
		/// <param name="v13">Second row, Fourth column.</param>
		/// <param name="v20">Third row, First column.</param>
		/// <param name="v21">Third row, Second column.</param>
		/// <param name="v22">Third row, Third column.</param>
		/// <param name="v23">Third row, Fourth column.</param>
		public Matrix34(float v00, float v01, float v02, float v03, float v10, float v11, float v12, float v13, float v20, float v21, float v22, float v23)
			: this()
		{
			this.M00 = v00; this.M01 = v01; this.M02 = v02; this.M03 = v03;
			this.M10 = v10; this.M11 = v11; this.M12 = v12; this.M13 = v13;
			this.M20 = v20; this.M21 = v21; this.M22 = v22; this.M23 = v23;
		}
		/// <summary>
		/// Creates new instance of type <see cref="Matrix34"/> .
		/// </summary>
		/// <param name="s"><see cref="Vector3"/> that contains scaling factors.</param>
		/// <param name="q">
		/// <see cref="Quaternion"/> that represents rotation that new matrix will represent.
		/// </param>
		/// <param name="t"><see cref="Vector3"/> that represents translation.</param>
		public Matrix34(Vector3 s, Quaternion q, Vector3 t = default(Vector3))
			: this()
		{
			this.Set(s, q, t);
		}
		/// <summary>
		/// Creates new instance of type <see cref="Matrix34"/> .
		/// </summary>
		/// <param name="m33"><see cref="Mathematics.Matrix33"/> to use to fill first 3 columns.</param>
		public Matrix34(Matrix33 m33)
			: this()
		{
			this.Row0 = new Vector4(m33.Row0, 0);
			this.Row1 = new Vector4(m33.Row1, 0);
			this.Row2 = new Vector4(m33.Row2, 1);
		}
		#endregion
		#region Interface
		#region Basics
		/// <summary>
		/// Sets the value of this matrix to the identity matrix.
		/// </summary>
		public void SetIdentity()
		{
			this.M00 = 1.0f; this.M01 = 0.0f; this.M02 = 0.0f; this.M03 = 0.0f;
			this.M10 = 0.0f; this.M11 = 1.0f; this.M12 = 0.0f; this.M13 = 0.0f;
			this.M20 = 0.0f; this.M21 = 0.0f; this.M22 = 1.0f; this.M23 = 0.0f;
		}
		/// <summary>
		/// Sets the value of the matrix to one that represents transformations represented by given arguments.
		/// </summary>
		/// <param name="s"><see cref="Vector3"/> object that represents scale.</param>
		/// <param name="q"><see cref="Quaternion"/> object that represents rotation.</param>
		/// <param name="t">Optional <see cref="Vector3"/> object that represents translation.</param>
		public void Set(Vector3 s, Quaternion q, Vector3 t = default(Vector3))
		{
			float vxvx = q.V.X * q.V.X; float vzvz = q.V.Z * q.V.Z; float vyvy = q.V.Y * q.V.Y;
			float vxvy = q.V.X * q.V.Y; float vxvz = q.V.X * q.V.Z; float vyvz = q.V.Y * q.V.Z;
			float svx = q.W * q.V.X; float svy = q.W * q.V.Y; float svz = q.W * q.V.Z;
			this.M00 = (1 - (vyvy + vzvz) * 2) * s.X; this.M01 = (vxvy - svz) * 2 * s.Y; this.M02 = (vxvz + svy) * 2 * s.Z; this.M03 = t.X;
			this.M10 = (vxvy + svz) * 2 * s.X; this.M11 = (1 - (vxvx + vzvz) * 2) * s.Y; this.M12 = (vyvz - svx) * 2 * s.Z; this.M13 = t.Y;
			this.M20 = (vxvz - svy) * 2 * s.X; this.M21 = (vyvz + svx) * 2 * s.Y; this.M22 = (1 - (vxvx + vyvy) * 2) * s.Z; this.M23 = t.Z;
		}
		/// <summary>
		/// Creates a matrix that represents transformations represented by given arguments.
		/// </summary>
		/// <param name="s"><see cref="Vector3"/> object that represents scale.</param>
		/// <param name="q"><see cref="Quaternion"/> object that represents rotation.</param>
		/// <param name="t">Optional <see cref="Vector3"/> object that represents translation.</param>
		/// <returns>Matrix that represents transformations represented by given arguments.</returns>
		public static Matrix34 Create(Vector3 s, Quaternion q, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.Set(s, q, t);

			return matrix;
		}
		/// <summary>
		/// Sets values of all columns of this matrix to one supplied by given vectors.
		/// </summary>
		/// <param name="vx"><see cref="Vector3"/> object that provides values for the first column.</param>
		/// <param name="vy">
		/// <see cref="Vector3"/> object that provides values for the second column.
		/// </param>
		/// <param name="vz"><see cref="Vector3"/> object that provides values for the third column.</param>
		/// <param name="vw">
		/// <see cref="Vector3"/> object that provides values for the fourth column.
		/// </param>
		public void SetColumns(Vector3 vx, Vector3 vy, Vector3 vz, Vector3 vw)
		{
			this.M00 = vx.X; this.M01 = vy.X; this.M02 = vz.X; this.M03 = vw.X;
			this.M10 = vx.Y; this.M11 = vy.Y; this.M12 = vz.Y; this.M13 = vw.Y;
			this.M20 = vx.Z; this.M21 = vy.Z; this.M22 = vz.Z; this.M23 = vw.Z;
		}
		/// <summary>
		/// Creates a new matrix which columns are initialized with values supplied by given vectors.
		/// </summary>
		/// <param name="vx"><see cref="Vector3"/> object that provides values for the first column.</param>
		/// <param name="vy">
		/// <see cref="Vector3"/> object that provides values for the second column.
		/// </param>
		/// <param name="vz"><see cref="Vector3"/> object that provides values for the third column.</param>
		/// <param name="vw">
		/// <see cref="Vector3"/> object that provides values for the fourth column.
		/// </param>
		/// <returns>
		/// New matrix which columns are initialized with values supplied by given vectors.
		/// </returns>
		public static Matrix34 CreateFromColumns(Vector3 vx, Vector3 vy, Vector3 vz, Vector3 vw)
		{
			var matrix = new Matrix34();
			matrix.SetColumns(vx, vy, vz, vw);

			return matrix;
		}
		/// <summary>
		/// Sets value of this matrix to one that undoes all transformations this matrix represents.
		/// </summary>
		public void InvertFast()
		{
			var v = new Vector3(this.M03, this.M13, this.M23);
			float t = this.M01; this.M01 = this.M10; this.M10 = t; this.M03 = -v.X * this.M00 - v.Y * this.M01 - v.Z * this.M20;
			t = this.M02; this.M02 = this.M20; this.M20 = t; this.M13 = -v.X * this.M10 - v.Y * this.M11 - v.Z * this.M21;
			t = this.M12; this.M12 = this.M21; this.M21 = t; this.M23 = -v.X * this.M20 - v.Y * this.M21 - v.Z * this.M22;
		}
		/// <summary>
		/// Sets value of this matrix to one that undoes all transformations this matrix represents.
		/// </summary>
		public void Invert()
		{
			// Store original values of the matrix.
			var m = this;

			// Calculate 12 cofactors.
			this.M00 = m.M22 * m.M11 - m.M12 * m.M21;
			this.M10 = m.M12 * m.M20 - m.M22 * m.M10;
			this.M20 = m.M10 * m.M21 - m.M20 * m.M11;
			this.M01 = m.M02 * m.M21 - m.M22 * m.M01;
			this.M11 = m.M22 * m.M00 - m.M02 * m.M20;
			this.M21 = m.M20 * m.M01 - m.M00 * m.M21;
			this.M02 = m.M12 * m.M01 - m.M02 * m.M11;
			this.M12 = m.M02 * m.M10 - m.M12 * m.M00;
			this.M22 = m.M00 * m.M11 - m.M10 * m.M01;
			this.M03 = (m.M22 * m.M13 * m.M01 + m.M02 * m.M23 * m.M11 + m.M12 * m.M03 * m.M21) - (m.M12 * m.M23 * m.M01 + m.M22 * m.M03 * m.M11 + m.M02 * m.M13 * m.M21);
			this.M13 = (m.M12 * m.M23 * m.M00 + m.M22 * m.M03 * m.M10 + m.M02 * m.M13 * m.M20) - (m.M22 * m.M13 * m.M00 + m.M02 * m.M23 * m.M10 + m.M12 * m.M03 * m.M20);
			this.M23 = (m.M20 * m.M11 * m.M03 + m.M00 * m.M21 * m.M13 + m.M10 * m.M01 * m.M23) - (m.M10 * m.M21 * m.M03 + m.M20 * m.M01 * m.M13 + m.M00 * m.M11 * m.M23);

			// Calculate "determinant".
			float det = 1.0f / (m.M00 * this.M00 + m.M10 * this.M01 + m.M20 * this.M02);

			// Calculate matrix inverse.
			this.M00 *= det; this.M01 *= det; this.M02 *= det; this.M03 *= det;
			this.M10 *= det; this.M11 *= det; this.M12 *= det; this.M13 *= det;
			this.M20 *= det; this.M21 *= det; this.M22 *= det; this.M23 *= det;
		}
		/// <summary>
		/// Removes scale from this matrix.
		/// </summary>
		public void OrthonormalizeFast()
		{
			var x = new Vector3(this.M00, this.M10, this.M20);
			var y = new Vector3(this.M01, this.M11, this.M21);
			x = x.Normalized;
			var z = (x % y).Normalized;
			y = (z % x).Normalized;
			this.M00 = x.X; this.M10 = x.Y; this.M20 = x.Z;
			this.M01 = y.X; this.M11 = y.Y; this.M21 = y.Z;
			this.M02 = z.X; this.M12 = z.Y; this.M22 = z.Z;
		}

		/// <summary>
		/// Checks if this matrix has an orthonormal-base.
		/// </summary>
		/// <remarks>General case, works even with reflection matrices.</remarks>
		/// <param name="threshold">Precision of the check.</param>
		/// <returns>True, if first 3 columns are orthonormal vectors.</returns>
		public bool IsOrthonormal(float threshold = 0.001f)
		{
			if (Math.Abs(this.ColumnVector0 | this.ColumnVector1) > threshold) return false;
			if (Math.Abs(this.ColumnVector0 | this.ColumnVector2) > threshold) return false;
			if (Math.Abs(this.ColumnVector1 | this.ColumnVector2) > threshold) return false;
			return
				Math.Abs(1 - (this.ColumnVector0 | this.ColumnVector0)) < threshold &&
				Math.Abs(1 - (this.ColumnVector1 | this.ColumnVector1)) < threshold &&
				Math.Abs(1 - (this.ColumnVector2 | this.ColumnVector2)) < threshold;
		}
		/// <summary>
		/// Determines whether this matrix has an orthonormal base in right-handed coordinate system.
		/// </summary>
		/// <param name="threshold">Precision of the check.</param>
		/// <returns>
		/// True, if first 3 columns are orthonormal vectors in right-handed coordinate system.
		/// </returns>
		public bool IsOrthonormalRightHanded(float threshold = 0.001f)
		{
			return
				this.ColumnVector0.IsEquivalent(this.ColumnVector1 % this.ColumnVector2, threshold) &&
				this.ColumnVector1.IsEquivalent(this.ColumnVector2 % this.ColumnVector0, threshold) &&
				this.ColumnVector2.IsEquivalent(this.ColumnVector0 % this.ColumnVector1, threshold);
		}
		/// <summary>
		/// Determines whether this matrix can be considered equal to another one within bounds of
		/// given precision.
		/// </summary>
		/// <param name="m">Another matrix.</param>
		/// <param name="e">
		/// <see cref="Single"/> object that represents maximal difference between two values that
		/// allows to consider them equal.
		/// </param>
		/// <returns>
		/// True, if difference between values of each corresponding pair is less then
		/// <paramref name="e"/> .
		/// </returns>
		public bool IsEquivalent(Matrix34 m, float e = 0.05f)
		{
			return
			(
				(Math.Abs(this.M00 - m.M00) <= e) &&
				(Math.Abs(this.M01 - m.M01) <= e) &&
				(Math.Abs(this.M02 - m.M02) <= e) &&
				(Math.Abs(this.M03 - m.M03) <= e) &&
				(Math.Abs(this.M10 - m.M10) <= e) &&
				(Math.Abs(this.M11 - m.M11) <= e) &&
				(Math.Abs(this.M12 - m.M12) <= e) &&
				(Math.Abs(this.M13 - m.M13) <= e) &&
				(Math.Abs(this.M20 - m.M20) <= e) &&
				(Math.Abs(this.M21 - m.M21) <= e) &&
				(Math.Abs(this.M22 - m.M22) <= e) &&
				(Math.Abs(this.M23 - m.M23) <= e));
		}
		/// <summary>
		/// Calculates a hash code of this matrix.
		/// </summary>
		/// <returns>Hash code of this matrix.</returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + this.M00.GetHashCode();
				hash = hash * 29 + this.M01.GetHashCode();
				hash = hash * 29 + this.M02.GetHashCode();
				hash = hash * 29 + this.M03.GetHashCode();

				hash = hash * 29 + this.M10.GetHashCode();
				hash = hash * 29 + this.M11.GetHashCode();
				hash = hash * 29 + this.M12.GetHashCode();
				hash = hash * 29 + this.M13.GetHashCode();

				hash = hash * 29 + this.M20.GetHashCode();
				hash = hash * 29 + this.M21.GetHashCode();
				hash = hash * 29 + this.M22.GetHashCode();
				hash = hash * 29 + this.M23.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		/// <summary>
		/// Creates the object that enumerates this matrix.
		/// </summary>
		/// <returns>The object that enumerates this matrix.</returns>
		public IEnumerator<float> GetEnumerator()
		{
			yield return this.M00;
			yield return this.M01;
			yield return this.M02;
			yield return this.M03;
			yield return this.M10;
			yield return this.M11;
			yield return this.M12;
			yield return this.M13;
			yield return this.M20;
			yield return this.M21;
			yield return this.M22;
			yield return this.M23;
		}
		#endregion
		#region Scaling
		/// <summary>
		/// Apples scaling to the columns of the matrix.
		/// </summary>
		/// <param name="s">
		/// <see cref="Vector3"/> object which X component will scale first column, Y - second, Z - third.
		/// </param>
		public void ScaleColumns(Vector3 s)
		{
			this.M00 *= s.X; this.M01 *= s.Y; this.M02 *= s.Z;
			this.M10 *= s.X; this.M11 *= s.Y; this.M12 *= s.Z;
			this.M20 *= s.X; this.M21 *= s.Y; this.M22 *= s.Z;
		}
		/// <summary>
		/// Apples scaling to the columns of the matrix.
		/// </summary>
		/// <param name="s">
		/// <see cref="Vector4"/> object which X component will scale first column, Y - second, Z -
		/// third, W - fourth.
		/// </param>
		public void ScaleColumns(Vector4 s)
		{
			this.M00 *= s.X; this.M01 *= s.Y; this.M02 *= s.Z; this.M03 *= s.W;
			this.M10 *= s.X; this.M11 *= s.Y; this.M12 *= s.Z; this.M13 *= s.W;
			this.M20 *= s.X; this.M21 *= s.Y; this.M22 *= s.Z; this.M23 *= s.W;
		}
		/// <summary>
		/// Sets the value of the matrix to one that represents scaling.
		/// </summary>
		/// <param name="s"><see cref="Vector3"/> object that represents scale.</param>
		/// <param name="t">Optional <see cref="Vector3"/> object that represents translation.</param>
		public void SetScale(Vector3 s, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateScale(s));

			this.SetTranslation(t);
		}
		/// <summary>
		/// Creates a matrix that represents scaling.
		/// </summary>
		/// <param name="s"><see cref="Vector3"/> object that represents scale.</param>
		/// <param name="t">Optional <see cref="Vector3"/> object that represents translation.</param>
		/// <returns>Matrix that represents scaling.</returns>
		public static Matrix34 CreateScale(Vector3 s, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetScale(s, t);

			return matrix;
		}
		#endregion
		#region Rotations
		/// <summary>
		/// Sets the values of this matrix to one that represents a rotation around an axis.
		/// </summary>
		/// <param name="rad"> Angle of rotation in radians.</param>
		/// <param name="axis">Normalized vector that represents axis of rotation.</param>
		/// <param name="t">   Optional translation vector.</param>
		public void SetRotationAroundAxis(float rad, Vector3 axis, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationAA(rad, axis));

			this.SetTranslation(t);
		}
		/// <summary>
		/// Creates a rotation matrix around an arbitrary axis (Eulers Theorem).
		/// </summary>
		/// <param name="rad"> Angle of rotation in radians.</param>
		/// <param name="axis">Normalized vector that represents axis of rotation.</param>
		/// <param name="t">   Optional translation vector.</param>
		/// <returns>A new rotation matrix.</returns>
		public static Matrix34 CreateRotationAroundAxis(float rad, Vector3 axis, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAroundAxis(rad, axis, t);

			return matrix;
		}
		/// <summary>
		/// Sets the values of this matrix to one that represents a rotation around an axis.
		/// </summary>
		/// <param name="rot">
		/// <see cref="Vector3"/> object which length represents an angle of rotation and which
		/// direction represents an axis of rotation.
		/// </param>
		/// <param name="t">  Optional translation vector.</param>
		public void SetRotationAroundAxis(Vector3 rot, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationAA(rot));

			this.SetTranslation(t);
		}
		/// <summary>
		/// Creates a rotation matrix around an arbitrary axis (Eulers Theorem).
		/// </summary>
		/// <param name="rot">
		/// <see cref="Vector3"/> object which length represents an angle of rotation and which
		/// direction represents an axis of rotation.
		/// </param>
		/// <param name="t">  Optional translation vector.</param>
		/// <returns>A new rotation matrix.</returns>
		public static Matrix34 CreateRotationAroundAxis(Vector3 rot, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAroundAxis(rot, t);

			return matrix;
		}
		/// <summary>
		/// Sets the values of this matrix to one that represents a rotation around an axis.
		/// </summary>
		/// <param name="c">   Cosine of the angle of the rotation.</param>
		/// <param name="s">   Sine of the angle of the rotation.</param>
		/// <param name="axis"><see cref="Vector3"/> object that represents axis of rotation.</param>
		/// <param name="t">   Optional translation vector.</param>
		public void SetRotationAroundAxis(float c, float s, Vector3 axis, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationAA(c, s, axis))
			{
				M03 = t.X,
				M13 = t.Y,
				M23 = t.Z
			};
		}
		/// <summary>
		/// Creates a rotation matrix around an arbitrary axis (Eulers Theorem).
		/// </summary>
		/// <param name="c">   Cosine of the angle of the rotation.</param>
		/// <param name="s">   Sine of the angle of the rotation.</param>
		/// <param name="axis"><see cref="Vector3"/> object that represents axis of rotation.</param>
		/// <param name="t">   Optional translation vector.</param>
		/// <returns>A new rotation matrix around an arbitrary axis.</returns>
		public static Matrix34 CreateRotationAroundAxis(float c, float s, Vector3 axis, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAroundAxis(c, s, axis, t);

			return matrix;
		}
		/// <summary>
		/// Sets the values of this matrix to one that represents a rotation around X axis.
		/// </summary>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="t">  Optional translation vector.</param>
		public void SetRotationAroundX(float rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationX(rad));

			this.SetTranslation(t);
		}
		/// <summary>
		/// Creates a matrix that represents a rotation around X axis.
		/// </summary>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="t">  Optional translation vector.</param>
		/// <returns>A new matrix that represents a rotation around X axis.</returns>
		public static Matrix34 CreateRotationAroundX(float rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAroundX(rad, t);

			return matrix;
		}
		/// <summary>
		/// Sets the values of this matrix to one that represents a rotation around Y axis.
		/// </summary>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="t">  Optional translation vector.</param>
		public void SetRotationAroundY(float rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationY(rad));

			this.SetTranslation(t);
		}
		/// <summary>
		/// Creates a matrix that represents a rotation around Y axis.
		/// </summary>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="t">  Optional translation vector.</param>
		/// <returns>A new matrix that represents a rotation around Y axis.</returns>
		public static Matrix34 CreateRotationAroundY(float rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAroundY(rad, t);

			return matrix;
		}
		/// <summary>
		/// Sets the values of this matrix to one that represents a rotation around Z axis.
		/// </summary>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="t">  Optional translation vector.</param>
		public void SetRotationAroundZ(float rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationZ(rad));

			this.SetTranslation(t);
		}
		/// <summary>
		/// Creates a matrix that represents a rotation around Z axis.
		/// </summary>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="t">  Optional translation vector.</param>
		/// <returns>A new matrix that represents a rotation around Z axis.</returns>
		public static Matrix34 CreateRotationAroundZ(float rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAroundZ(rad, t);

			return matrix;
		}
		/// <summary>
		/// Convert three Euler angles to 3x3 matrix (rotation order:XYZ)
		/// </summary>
		/// <param name="rad">Angles of rotation.</param>
		/// <param name="t">  Optional translation vector.</param>
		public void SetRotationWithEulerAngles(EulerAngles rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationXYZ((Vector3)rad));

			this.SetTranslation(t);
		}
		/// <summary>
		/// Creates a matrix that represents rotation represented by given Euler angles.
		/// </summary>
		/// <param name="rad">Angles of rotation.</param>
		/// <param name="t">  Optional translation vector.</param>
		/// <returns>A new matrix that represents rotation represented by given Euler angles.</returns>
		public static Matrix34 CreateRotationWithEulerAngles(EulerAngles rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationWithEulerAngles(rad, t);

			return matrix;
		}
		#endregion
		#region Translations
		/// <summary>
		/// Sets the value of the matrix to one that represents a translation without any other transformations.
		/// </summary>
		/// <param name="v"><see cref="Vector3"/> object that represents translation.</param>
		public void SetPureTranslation(Vector3 v)
		{
			this.M00 = 1.0f; this.M01 = 0.0f; this.M02 = 0.0f; this.M03 = v.X;
			this.M10 = 0.0f; this.M11 = 1.0f; this.M12 = 0.0f; this.M13 = v.Y;
			this.M20 = 0.0f; this.M21 = 0.0f; this.M22 = 1.0f; this.M23 = v.Z;
		}
		/// <summary>
		/// Creates a matrix that represents a translation without any other transformations.
		/// </summary>
		/// <param name="v"><see cref="Vector3"/> object that represents translation.</param>
		/// <returns>Matrix that represents a translation without any other transformations.</returns>
		public static Matrix34 CreatePureTranslation(Vector3 v)
		{
			var matrix = new Matrix34();
			matrix.SetPureTranslation(v);

			return matrix;
		}
		/// <summary>
		/// Sets translation that is represented by this matrix to one that is represented by given vector.
		/// </summary>
		/// <param name="t"><see cref="Vector3"/> object that represents translation.</param>
		public void SetTranslation(Vector3 t)
		{
			this.M03 = t.X;
			this.M13 = t.Y;
			this.M23 = t.Z;
		}
		/// <summary>
		/// Multiplies last column of this matrix that represents translation by a given number.
		/// </summary>
		/// <param name="s">Factor of scaling of translation.</param>
		public void ScaleTranslation(float s)
		{
			this.M03 *= s;
			this.M13 *= s;
			this.M23 *= s;
		}
		/// <summary>
		/// Adds values supplied by given vector to the last column of this matrix.
		/// </summary>
		/// <param name="t"><see cref="Vector3"/> object that represents translation.</param>
		public void AddTranslation(Vector3 t)
		{
			this.M03 += t.X;
			this.M13 += t.Y;
			this.M23 += t.Z;
		}
		#endregion
		#region Transformations
		/// <summary>
		/// Applies transformation represented by this matrix (without translation) to given vector.
		/// </summary>
		/// <param name="p"><see cref="Vector3"/> object to apply transformation to.</param>
		/// <returns>
		/// A new <see cref="Vector3"/> object that represents given vector with transformations
		/// applied to it.
		/// </returns>
		public Vector3 TransformVectorNoTranslation(Vector3 p)
		{
			return new Vector3
			(
				this.M00 * p.X + this.M01 * p.Y + this.M02 * p.Z,
				this.M10 * p.X + this.M11 * p.Y + this.M12 * p.Z,
				this.M20 * p.X + this.M21 * p.Y + this.M22 * p.Z
			);
		}
		/// <summary>
		/// Applies transformation represented by this matrix to given vector.
		/// </summary>
		/// <param name="p"><see cref="Vector3"/> object to apply transformation to.</param>
		/// <returns>
		/// A new <see cref="Vector3"/> object that represents given vector with transformations
		/// applied to it.
		/// </returns>
		public Vector3 TransformVector(Vector3 p)
		{
			return new Vector3
			(
				this.M00 * p.X + this.M01 * p.Y + this.M02 * p.Z + this.M03,
				this.M10 * p.X + this.M11 * p.Y + this.M12 * p.Z + this.M13,
				this.M20 * p.X + this.M21 * p.Y + this.M22 * p.Z + this.M23
			);
		}
		#endregion
		#region Interpolations
		/// <summary>
		/// Creates a matrix that represents a spherical linear interpolation from one matrix to another.
		/// </summary>
		/// <param name="m">First matrix.</param>
		/// <param name="n">Second matrix.</param>
		/// <param name="t">Interpolation parameter.</param>
		/// <returns>Matrix which transformations are interpolated between given matrices.</returns>
		public static Matrix34 CreateSlerp(Matrix34 m, Matrix34 n, float t)
		{
			var matrix = new Matrix34();
			matrix.SetSlerp(m, n, t);

			return matrix;
		}
		/// <summary>
		/// Creates a matrix that represents a spherical linear interpolation from one matrix to another.
		/// </summary>
		/// <remarks>
		/// <para>This is an implementation of interpolation without quaternions.</para>
		/// <para>
		/// Given two orthonormal 3x3 matrices this function calculates the shortest possible
		/// interpolation-path between the two rotations. The interpolation curve forms the shortest
		/// great arc on the rotation sphere (geodesic).
		/// </para>
		/// <para>Angular velocity of the interpolation is constant.</para>
		/// <para>Possible stability problems:</para>
		/// <para>
		/// There are two singularities at angle = 0 and angle = <see cref="Math.PI"/> . At 0 the
		/// interpolation-axis is arbitrary, which means any axis will produce the same result
		/// because we have no rotation. (1,0,0) axis is used in this case. At <see cref="Math.PI"/>
		/// the rotations point away from each other and the interpolation-axis is unpredictable. In
		/// this case axis (1,0,0) is used as well. If the angle is ~0 or ~PI, then a very small
		/// vector has to be normalized and this can cause numerical instability.
		/// </para>
		/// </remarks>
		/// <param name="m">First matrix.</param>
		/// <param name="n">Second matrix.</param>
		/// <param name="t">Interpolation parameter.</param>
		public void SetSlerp(Matrix34 m, Matrix34 n, float t)
		{
			// calculate delta-rotation between m and n (=39 flops)
			Matrix33 d = new Matrix33(), i = new Matrix33();
			d.M00 = m.M00 * n.M00 + m.M10 * n.M10 + m.M20 * n.M20; d.M01 = m.M00 * n.M01 + m.M10 * n.M11 + m.M20 * n.M21; d.M02 = m.M00 * n.M02 + m.M10 * n.M12 + m.M20 * n.M22;
			d.M10 = m.M01 * n.M00 + m.M11 * n.M10 + m.M21 * n.M20; d.M11 = m.M01 * n.M01 + m.M11 * n.M11 + m.M21 * n.M21; d.M12 = m.M01 * n.M02 + m.M11 * n.M12 + m.M21 * n.M22;
			d.M20 = d.M01 * d.M12 - d.M02 * d.M11; d.M21 = d.M02 * d.M10 - d.M00 * d.M12; d.M22 = d.M00 * d.M11 - d.M01 * d.M10;

			// extract angle and axis
			double cosine = MathHelpers.Clamp((d.M00 + d.M11 + d.M22 - 1.0) * 0.5, -1.0, +1.0);
			double angle = Math.Atan2(Math.Sqrt(1.0 - cosine * cosine), cosine);
			var axis = new Vector3(d.M21 - d.M12, d.M02 - d.M20, d.M10 - d.M01);
			double l = Math.Sqrt(axis | axis); if (l > 0.00001) axis /= (float)l; else axis = new Vector3(1, 0, 0);
			i.SetRotationAA((float)angle * t, axis); // angle interpolation and calculation of new delta-matrix (=26 flops)

			// final concatenation (=39 flops)
			this.M00 = m.M00 * i.M00 + m.M01 * i.M10 + m.M02 * i.M20; this.M01 = m.M00 * i.M01 + m.M01 * i.M11 + m.M02 * i.M21; this.M02 = m.M00 * i.M02 + m.M01 * i.M12 + m.M02 * i.M22;
			this.M10 = m.M10 * i.M00 + m.M11 * i.M10 + m.M12 * i.M20; this.M11 = m.M10 * i.M01 + m.M11 * i.M11 + m.M12 * i.M21; this.M12 = m.M10 * i.M02 + m.M11 * i.M12 + m.M12 * i.M22;
			this.M20 = this.M01 * this.M12 - this.M02 * this.M11; this.M21 = this.M02 * this.M10 - this.M00 * this.M12; this.M22 = this.M00 * this.M11 - this.M01 * this.M10;

			this.M03 = m.M03 * (1 - t) + n.M03 * t;
			this.M13 = m.M13 * (1 - t) + n.M13 * t;
			this.M23 = m.M23 * (1 - t) + n.M23 * t;
		}
		#endregion
		#region Operators
		/// <summary>
		/// Multiplies two matrices.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Matrix34 operator *(Matrix34 l, Matrix34 r)
		{
			return new Matrix34
			{
				M00 = l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20,
				M10 = l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20,
				M20 = l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20,
				M01 = l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21,
				M11 = l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21,
				M21 = l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21,
				M02 = l.M00 * r.M02 + l.M01 * r.M12 + l.M02 * r.M22,
				M12 = l.M10 * r.M02 + l.M11 * r.M12 + l.M12 * r.M22,
				M22 = l.M20 * r.M02 + l.M21 * r.M12 + l.M22 * r.M22,
				M03 = l.M00 * r.M03 + l.M01 * r.M13 + l.M02 * r.M23 + l.M03,
				M13 = l.M10 * r.M03 + l.M11 * r.M13 + l.M12 * r.M23 + l.M13,
				M23 = l.M20 * r.M03 + l.M21 * r.M13 + l.M22 * r.M23 + l.M23
			};
		}
		#endregion
		#endregion
		#region Utilities
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}