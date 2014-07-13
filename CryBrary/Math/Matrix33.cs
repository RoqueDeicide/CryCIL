using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Represents a 3x3 matrix.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct Matrix33 : IEnumerable<float>
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
		#region Columns
		/// <summary>
		/// First column.
		/// </summary>
		[FieldOffset(0)]
		public Column33 Column0;
		/// <summary>
		/// Second column.
		/// </summary>
		[FieldOffset(4)]
		public Column33 Column1;
		/// <summary>
		/// Third column.
		/// </summary>
		[FieldOffset(8)]
		public Column33 Column2;
		#endregion
		#endregion
		#region Properties
		/// <summary>
		/// Gets angles that represent rotation this matrix represents.
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
					angles.Z = (float)Math.Atan2(-M01, M11);
				}
				else
				{
					angles.X = (float)Math.Atan2(M21, M22);
					angles.Z = (float)Math.Atan2(M10, M00);
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
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of <see cref="Matrix33" /> struct.
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
		/// <returns>New instance of <see cref="Matrix33" /> struct.</returns>
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
		/// Creates new instance of <see cref="Matrix33" /> struct.
		/// </summary>
		/// <param name="m">
		/// <see cref="Matrix34" /> object from which first 3 columns will be copied.
		/// </param>
		/// <returns>New instance of <see cref="Matrix33" /> struct.</returns>
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
		/// Creates new instance of <see cref="Matrix33" /> struct.
		/// </summary>
		/// <param name="q">
		/// Quaternion that represents rotation that new instance whould represent.
		/// </param>
		/// <returns>New instance of <see cref="Matrix33" /> struct.</returns>
		public Matrix33(Quaternion q)
			: this()
		{
			var v2 = q.V + q.V;
			var xx = 1 - v2.X * q.V.X;
			var yy = v2.Y * q.V.Y;
			var xw = v2.X * q.W;

			var xy = v2.Y * q.V.X;
			var yz = v2.Z * q.V.Y;
			var yw = v2.Y * q.W;

			var xz = v2.Z * q.V.X;
			var zz = v2.Z * q.V.Z;
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
			M00 = 1;
			M01 = 0;
			M02 = 0;

			M10 = 0;
			M11 = 1;
			M12 = 0;

			M20 = 0;
			M21 = 0;
			M22 = 1;
		}
		/// <summary>
		/// Sets rows from given vectors.
		/// </summary>
		/// <param name="vx"><see cref="Vector3" /> object that contains first row.</param>
		/// <param name="vy"><see cref="Vector3" /> object that contains second row.</param>
		/// <param name="vz"><see cref="Vector3" /> object that contains third row.</param>
		public void SetFromVectors(Vector3 vx, Vector3 vy, Vector3 vz)
		{
			M00 = vx.X; M01 = vy.X; M02 = vz.X;
			M10 = vx.Y; M11 = vy.Y; M12 = vz.Y;
			M20 = vx.Z; M21 = vy.Z; M22 = vz.Z;
		}
		/// <summary>
		/// Creates new matrix from vectors that represent rows.
		/// </summary>
		/// <param name="vx"><see cref="Vector3" /> object that contains first row.</param>
		/// <param name="vy"><see cref="Vector3" /> object that contains second row.</param>
		/// <param name="vz"><see cref="Vector3" /> object that contains third row.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateFromVectors(Vector3 vx, Vector3 vy, Vector3 vz)
		{
			var matrix = new Matrix33();
			matrix.SetFromVectors(vx, vy, vz);

			return matrix;
		}
		/// <summary>
		/// Sets this matrix to represent a scaling operation.
		/// </summary>
		/// <param name="s">Scale to set.</param>
		public void SetScale(Vector3 s)
		{
			M00 = s.X; M01 = 0; M02 = 0;
			M10 = 0; M11 = s.Y; M12 = 0;
			M20 = 0; M21 = 0; M22 = s.Z;
		}
		/// <summary>
		/// Creates new matrix that is set to represent scaling operation.
		/// </summary>
		/// <param name="s">Scale to set.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateScale(Vector3 s)
		{
			var matrix = new Matrix33();
			matrix.SetScale(s);

			return matrix;
		}
		/// <summary></summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + M00.GetHashCode();
				hash = hash * 29 + M01.GetHashCode();
				hash = hash * 29 + M02.GetHashCode();

				hash = hash * 29 + M10.GetHashCode();
				hash = hash * 29 + M11.GetHashCode();
				hash = hash * 29 + M12.GetHashCode();

				hash = hash * 29 + M20.GetHashCode();
				hash = hash * 29 + M21.GetHashCode();
				hash = hash * 29 + M22.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		#endregion
		#region Setting Rotations
		/// <summary>
		/// Sets this matrix to represent rotation around given axis.
		/// </summary>
		/// <param name="angle">Angle of rotation.</param>
		/// <param name="axis">Axis of rotation.</param>
		public void SetRotationAA(float angle, Vector3 axis)
		{
			double s, c;
			MathHelpers.SinCos(angle, out s, out c);
			float mc = 1.0f - (float)c;

			float mcx = mc * axis.X;
			float mcy = mc * axis.Y;
			float mcz = mc * axis.Z;

			float tcx = axis.X * (float)s;
			float tcy = axis.Y * (float)s;
			float tcz = axis.Z * (float)s;

			M00 = mcx * axis.X + (float)c;
			M01 = mcx * axis.Y - tcz;
			M02 = mcx * axis.Z + tcy;

			M10 = mcy * axis.X + tcz;
			M11 = mcy * axis.Y + (float)c;
			M12 = mcy * axis.Z - tcx;

			M20 = mcz * axis.X - tcy;
			M21 = mcz * axis.Y + tcx;
			M22 = mcz * axis.Z + (float)c;
		}
		/// <summary>
		/// Creates new matrix that is set to represent rotation around given axis.
		/// </summary>
		/// <param name="rad">Angle of rotation.</param>
		/// <param name="axis">Axis of rotation.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateRotationAA(float rad, Vector3 axis)
		{
			var matrix = new Matrix33();
			matrix.SetRotationAA(rad, axis);

			return matrix;
		}
		/// <summary>
		/// Sets this matrix to represent rotation around given axis.
		/// </summary>
		/// <param name="c">Cosine of angle of rotation.</param>
		/// <param name="s">Sine of angle of rotation.</param>
		/// <param name="axis">Axis of rotation.</param>
		public void SetRotationAA(float c, float s, Vector3 axis)
		{
			float mc = 1 - c;
			M00 = mc * axis.X * axis.X + c; M01 = mc * axis.X * axis.Y - axis.Z * s; M02 = mc * axis.X * axis.Z + axis.Y * s;
			M10 = mc * axis.Y * axis.X + axis.Z * s; M11 = mc * axis.Y * axis.Y + c; M12 = mc * axis.Y * axis.Z - axis.X * s;
			M20 = mc * axis.Z * axis.X - axis.Y * s; M21 = mc * axis.Z * axis.Y + axis.X * s; M22 = mc * axis.Z * axis.Z + c;
		}
		/// <summary>
		/// Creates new matrix that is set to represent rotation around given axis.
		/// </summary>
		/// <param name="c">Cosine of angle of rotation.</param>
		/// <param name="s">Sine of angle of rotation.</param>
		/// <param name="axis">Axis of rotation.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateRotationAA(float c, float s, Vector3 axis)
		{
			var matrix = new Matrix33();
			matrix.SetRotationAA(c, s, axis);

			return matrix;
		}
		/// <summary>
		/// Sets this matrix to represent rotation around given axis.
		/// </summary>
		/// <param name="rot">
		/// <see cref="Vector3" /> which length represents an angle of rotation, and that, once
		/// normalized, represents an axis of rotation.
		/// </param>
		public void SetRotationAA(Vector3 rot)
		{
			float angle = rot.Length;
			if (Math.Abs(angle) < MathHelpers.ZeroTolerance)
				SetIdentity();
			else
				SetRotationAA(angle, rot / angle);
		}
		/// <summary>
		/// Creates new matrix that is set to represent rotation around given axis.
		/// </summary>
		/// <param name="rot">
		/// <see cref="Vector3" /> which length represents an angle of rotation, and that, once
		/// normalized, represents an axis of rotation.
		/// </param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateRotationAA(Vector3 rot)
		{
			var matrix = new Matrix33();
			matrix.SetRotationAA(rot);

			return matrix;
		}
		/// <summary>
		/// Sets this matrix to represent rotation around X axis.
		/// </summary>
		/// <param name="rad">Angle of rotation around X axis.</param>
		public void SetRotationX(float rad)
		{
			double s, c; MathHelpers.SinCos(rad, out s, out c);
			M00 = 1.0f; M01 = 0.0f; M02 = 0.0f;
			M10 = 0.0f; M11 = (float)c; M12 = (float)-s;
			M20 = 0.0f; M21 = (float)s; M22 = (float)c;
		}
		/// <summary>
		/// Creates new matrix that is set to represent rotation around X axis.
		/// </summary>
		/// <param name="rad">Angle of rotation around X axis.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateRotationX(float rad)
		{
			var matrix = new Matrix33();
			matrix.SetRotationX(rad);

			return matrix;
		}
		/// <summary>
		/// Sets this matrix to represent rotation around Y axis.
		/// </summary>
		/// <param name="rad">Angle of rotation around Y axis.</param>
		public void SetRotationY(float rad)
		{
			double s, c; MathHelpers.SinCos(rad, out s, out c);
			M00 = (float)c; M01 = 0; M02 = (float)s;
			M10 = 0; M11 = 1; M12 = 0;
			M20 = (float)-s; M21 = 0; M22 = (float)c;
		}
		/// <summary>
		/// Creates new matrix that is set to represent rotation around Y axis.
		/// </summary>
		/// <param name="rad">Angle of rotation around Y axis.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateRotationY(float rad)
		{
			var matrix = new Matrix33();
			matrix.SetRotationY(rad);

			return matrix;
		}
		/// <summary>
		/// Sets this matrix to represent rotation around Z axis.
		/// </summary>
		/// <param name="rad">Angle of rotation around Z axis.</param>
		public void SetRotationZ(float rad)
		{
			double s, c; MathHelpers.SinCos(rad, out s, out c);
			M00 = (float)c; M01 = (float)-s; M02 = 0.0f;
			M10 = (float)s; M11 = (float)c; M12 = 0.0f;
			M20 = 0.0f; M21 = 0.0f; M22 = 1.0f;
		}
		/// <summary>
		/// Creates new matrix that is set to represent rotation around Z axis.
		/// </summary>
		/// <param name="rad">Angle of rotation around Z axis.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateRotationZ(float rad)
		{
			var matrix = new Matrix33();
			matrix.SetRotationZ(rad);

			return matrix;
		}
		/// <summary>
		/// Sets this matrix to represent rotation around fixed Axes.
		/// </summary>
		/// <param name="rad"><see cref="Vector3" /> that defines rotations around XYZ.</param>
		public void SetRotationXYZ(Vector3 rad)
		{
			double sx, cx; MathHelpers.SinCos(rad.X, out sx, out cx);
			double sy, cy; MathHelpers.SinCos(rad.Y, out sy, out cy);
			double sz, cz; MathHelpers.SinCos(rad.Z, out sz, out cz);
			double sycz = (sy * cz), sysz = (sy * sz);
			M00 = (float)(cy * cz); M01 = (float)(sycz * sx - cx * sz); M02 = (float)(sycz * cx + sx * sz);
			M10 = (float)(cy * sz); M11 = (float)(sysz * sx + cx * cz); M12 = (float)(sysz * cx - sx * cz);
			M20 = (float)(-sy); M21 = (float)(cy * sx); M22 = (float)(cy * cx);
		}
		/// <summary>
		/// Creates new matrix that is set to represent rotation around fixed Axes.
		/// </summary>
		/// <param name="rad">Angles of rotation.</param>
		/// <returns>New matrix.</returns>
		public static Matrix33 CreateRotationXYZ(Vector3 rad)
		{
			var matrix = new Matrix33();
			matrix.SetRotationXYZ(rad);

			return matrix;
		}
		#endregion
		#region Enumeration
		/// <summary>
		/// Enumerates this matrix.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<float> GetEnumerator()
		{
			yield return M00;
			yield return M01;
			yield return M02;
			yield return M10;
			yield return M11;
			yield return M12;
			yield return M10;
			yield return M21;
			yield return M22;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion
		#endregion
		#region Operators
		/// <summary>
		/// Multiplies the matrix by float number.
		/// </summary>
		/// <param name="left">Left operand.</param>
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
		/// <param name="left">Left operand.</param>
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
		/// <param name="left">Left operand.</param>
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
		/// <param name="left">Left operand.</param>
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