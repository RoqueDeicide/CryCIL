using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Represents 3x4 matrix.
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
		#region Columns
		/// <summary>
		/// First column.
		/// </summary>
		[FieldOffset(0)]
		public Column34 Column0;
		/// <summary>
		/// Second column.
		/// </summary>
		[FieldOffset(4)]
		public Column34 Column1;
		/// <summary>
		/// Third column.
		/// </summary>
		[FieldOffset(8)]
		public Column34 Column2;
		/// <summary>
		/// Fourth column.
		/// </summary>
		[FieldOffset(12)]
		public Column34 Column3;
		#endregion
		#endregion
		#region Constructors
		/// <summary>
		/// Creates new instance of type <see cref="Matrix34" />.
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
			M00 = v00; M01 = v01; M02 = v02; M03 = v03;
			M10 = v10; M11 = v11; M12 = v12; M13 = v13;
			M20 = v20; M21 = v21; M22 = v22; M23 = v23;
		}
		/// <summary>
		/// Creates new instance of type <see cref="Matrix34" />.
		/// </summary>
		/// <param name="s"><see cref="Vector3" /> that contains scaling factors.</param>
		/// <param name="q">
		/// <see cref="Quaternion" /> that represents rotation that new matrix will represent.
		/// </param>
		/// <param name="t"><see cref="Vector3" /> that represents translation.</param>
		public Matrix34(Vector3 s, Quaternion q, Vector3 t = default(Vector3))
			: this()
		{
			Set(s, q, t);
		}
		/// <summary>
		/// Creates new instance of type <see cref="Matrix34" />.
		/// </summary>
		/// <param name="m33"><see cref="Matrix33" /> to use to fill first 3 columns.</param>
		public Matrix34(Matrix33 m33)
			: this()
		{
			this.Row0 = new Vector4(m33.Row0, 0);
			this.Row1 = new Vector4(m33.Row1, 0);
			this.Row2 = new Vector4(m33.Row2, 1);
		}
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
		/// Gets first column in a for of a <see cref="Vector3" />.
		/// </summary>
		public Vector3 ColumnVector0 { get { return new Vector3(M00, M10, M20); } }
		/// <summary>
		/// Gets second column in a for of a <see cref="Vector3" />.
		/// </summary>
		public Vector3 ColumnVector1 { get { return new Vector3(M01, M11, M21); } }
		/// <summary>
		/// Gets third column in a for of a <see cref="Vector3" />.
		/// </summary>
		public Vector3 ColumnVector2 { get { return new Vector3(M02, M12, M22); } }
		/// <summary>
		/// Gets fourth column in a for of a <see cref="Vector3" />.
		/// </summary>
		public Vector3 ColumnVector3 { get { return new Vector3(M03, M13, M23); } }
		/// <summary>
		/// Gets translation vector from this matrix.
		/// </summary>
		public Vector3 Translation { get { return new Vector3(M03, M13, M23); } }
		#endregion
		#region Interface
		#region Basics

		public void SetIdentity()
		{
			M00 = 1.0f; M01 = 0.0f; M02 = 0.0f; M03 = 0.0f;
			M10 = 0.0f; M11 = 1.0f; M12 = 0.0f; M13 = 0.0f;
			M20 = 0.0f; M21 = 0.0f; M22 = 1.0f; M23 = 0.0f;
		}
		#endregion
		#region Scaling
		/// <summary>
		/// Apples scaling to the columns of the matrix.
		/// </summary>
		/// <param name="s">
		/// <see cref="Vector3" /> object which X component will scale first column, Y - second, Z - third.
		/// </param>
		public void ScaleColumns(Vector3 s)
		{
			M00 *= s.X; M01 *= s.Y; M02 *= s.Z;
			M10 *= s.X; M11 *= s.Y; M12 *= s.Z;
			M20 *= s.X; M21 *= s.Y; M22 *= s.Z;
		}
		/// <summary>
		/// Apples scaling to the columns of the matrix.
		/// </summary>
		/// <param name="s">
		/// <see cref="Vector4" /> object which X component will scale first column, Y - second, Z -
		/// third, W - fourth.
		/// </param>
		public void ScaleColumns(Vector4 s)
		{
			M00 *= s.X; M01 *= s.Y; M02 *= s.Z; M03 *= s.W;
			M10 *= s.X; M11 *= s.Y; M12 *= s.Z; M13 *= s.W;
			M20 *= s.X; M21 *= s.Y; M22 *= s.Z; M23 *= s.W;
		}
		#endregion
		#region Rotations
		/// <summary>
		/// Create a rotation matrix around an arbitrary axis (Eulers Theorem).
		/// </summary>
		/// <example>
		/// <code>Matrix34 m34; Vector3 axis=GetNormalized( Vector3(-1.0f,-0.3f,0.0f) );
		/// m34.SetRotationAA( 3.14314f, axis, Vector3(5,5,5) );</code>
		/// </example>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="axis">Normalized vector that represents axis of rotation.</param>
		/// <param name="t">Optional translation vector.</param>
		public void SetRotationAA(float rad, Vector3 axis, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationAA(rad, axis));

			SetTranslation(t);
		}

		public static Matrix34 CreateRotationAA(float rad, Vector3 axis, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAA(rad, axis, t);

			return matrix;
		}

		public void SetRotationAA(Vector3 rot, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationAA(rot));

			SetTranslation(t);
		}

		public static Matrix34 CreateRotationAA(Vector3 rot, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAA(rot, t);

			return matrix;
		}

		/*!
		* Create rotation-matrix about X axis using an angle.
		* The angle is assumed to be in radians.
		* The translation-vector is set to zero.
		*
		*  Example:
		*        Matrix34 m34;
		*        m34.SetRotationX(0.5f);
		*/
		public void SetRotationX(float rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationX(rad));

			SetTranslation(t);
		}

		public static Matrix34 CreateRotationX(float rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationX(rad, t);

			return matrix;
		}

		/*!
		* Create rotation-matrix about Y axis using an angle.
		* The angle is assumed to be in radians.
		* The translation-vector is set to zero.
		*
		*  Example:
		*        Matrix34 m34;
		*        m34.SetRotationY(0.5f);
		*/
		public void SetRotationY(float rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationY(rad));

			SetTranslation(t);
		}

		public static Matrix34 CreateRotationY(float rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationY(rad, t);

			return matrix;
		}

		/*!
		* Create rotation-matrix about Z axis using an angle.
		* The angle is assumed to be in radians.
		* The translation-vector is set to zero.
		*
		*  Example:
		*        Matrix34 m34;
		*        m34.SetRotationZ(0.5f);
		*/
		public void SetRotationZ(float rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationZ(rad));

			SetTranslation(t);
		}

		public static Matrix34 CreateRotationZ(float rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationZ(rad, t);

			return matrix;
		}

		/*!
		*
		* Convert three Euler angle to mat33 (rotation order:XYZ)
		* The Euler angles are assumed to be in radians.
		* The translation-vector is set to zero.
		*
		*  Example 1:
		*        Matrix34 m34;
		*        m34.SetRotationXYZ( Ang3(0.5f,0.2f,0.9f), translation );
		*
		*  Example 2:
		*        Matrix34 m34=Matrix34::CreateRotationXYZ( Ang3(0.5f,0.2f,0.9f), translation );
		*/
		/// <summary>
		/// Convert three Euler angle to 3x3 matrix (rotation order:XYZ)
		/// </summary>
		/// <example>
		/// <code>Matrix34 m34; m34.SetRotationXYZ(new Vector3(0.5f,0.2f,0.9f), translation);</code>
		/// </example>
		/// <param name="rad">Angles of rotation.</param>
		/// <param name="t">Optional translation vector.</param>
		public void SetRotationXYZ(Vector3 rad, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationXYZ(rad));

			SetTranslation(t);
		}

		public static Matrix34 CreateRotationXYZ(Vector3 rad, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationXYZ(rad, t);

			return matrix;
		}

		public void SetRotationAA(float c, float s, Vector3 axis, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateRotationAA(c, s, axis));
			M03 = t.X; M13 = t.Y; M23 = t.Z;
		}

		public static Matrix34 CreateRotationAA(float c, float s, Vector3 axis, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetRotationAA(c, s, axis, t);

			return matrix;
		}
		#endregion
		#endregion

		public void Set(Vector3 s, Quaternion q, Vector3 t = default(Vector3))
		{
			float vxvx = q.V.X * q.V.X; float vzvz = q.V.Z * q.V.Z; float vyvy = q.V.Y * q.V.Y;
			float vxvy = q.V.X * q.V.Y; float vxvz = q.V.X * q.V.Z; float vyvz = q.V.Y * q.V.Z;
			float svx = q.W * q.V.X; float svy = q.W * q.V.Y; float svz = q.W * q.V.Z;
			M00 = (1 - (vyvy + vzvz) * 2) * s.X; M01 = (vxvy - svz) * 2 * s.Y; M02 = (vxvz + svy) * 2 * s.Z; M03 = t.X;
			M10 = (vxvy + svz) * 2 * s.X; M11 = (1 - (vxvx + vzvz) * 2) * s.Y; M12 = (vyvz - svx) * 2 * s.Z; M13 = t.Y;
			M20 = (vxvz - svy) * 2 * s.X; M21 = (vyvz + svx) * 2 * s.Y; M22 = (1 - (vxvx + vyvy) * 2) * s.Z; M23 = t.Z;
		}

		public static Matrix34 Create(Vector3 s, Quaternion q, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.Set(s, q, t);

			return matrix;
		}

		public void SetScale(Vector3 s, Vector3 t = default(Vector3))
		{
			this = new Matrix34(Matrix33.CreateScale(s));

			SetTranslation(t);
		}

		public static Matrix34 CreateScale(Vector3 s, Vector3 t = default(Vector3))
		{
			var matrix = new Matrix34();
			matrix.SetScale(s, t);

			return matrix;
		}

		public void SetTranslationMat(Vector3 v)
		{
			M00 = 1.0f; M01 = 0.0f; M02 = 0.0f; M03 = v.X;
			M10 = 0.0f; M11 = 1.0f; M12 = 0.0f; M13 = v.Y;
			M20 = 0.0f; M21 = 0.0f; M22 = 1.0f; M23 = v.Z;
		}

		public static Matrix34 CreateTranslationMat(Vector3 v)
		{
			var matrix = new Matrix34();
			matrix.SetTranslationMat(v);

			return matrix;
		}

		public void SetFromVectors(Vector3 vx, Vector3 vy, Vector3 vz, Vector3 pos)
		{
			M00 = vx.X; M01 = vy.X; M02 = vz.X; M03 = pos.X;
			M10 = vx.Y; M11 = vy.Y; M12 = vz.Y; M13 = pos.Y;
			M20 = vx.Z; M21 = vy.Z; M22 = vz.Z; M23 = pos.Z;
		}

		public static Matrix34 CreateFromVectors(Vector3 vx, Vector3 vy, Vector3 vz, Vector3 pos)
		{
			var matrix = new Matrix34();
			matrix.SetFromVectors(vx, vy, vz, pos);

			return matrix;
		}

		public void InvertFast()
		{
			var v = new Vector3(M03, M13, M23);
			float t = M01; M01 = M10; M10 = t; M03 = -v.X * M00 - v.Y * M01 - v.Z * M20;
			t = M02; M02 = M20; M20 = t; M13 = -v.X * M10 - v.Y * M11 - v.Z * M21;
			t = M12; M12 = M21; M21 = t; M23 = -v.X * M20 - v.Y * M21 - v.Z * M22;
		}

		public void Invert()
		{
			// rescue members
			var m = this;

			// calculate 12 cofactors
			M00 = m.M22 * m.M11 - m.M12 * m.M21;
			M10 = m.M12 * m.M20 - m.M22 * m.M10;
			M20 = m.M10 * m.M21 - m.M20 * m.M11;
			M01 = m.M02 * m.M21 - m.M22 * m.M01;
			M11 = m.M22 * m.M00 - m.M02 * m.M20;
			M21 = m.M20 * m.M01 - m.M00 * m.M21;
			M02 = m.M12 * m.M01 - m.M02 * m.M11;
			M12 = m.M02 * m.M10 - m.M12 * m.M00;
			M22 = m.M00 * m.M11 - m.M10 * m.M01;
			M03 = (m.M22 * m.M13 * m.M01 + m.M02 * m.M23 * m.M11 + m.M12 * m.M03 * m.M21) - (m.M12 * m.M23 * m.M01 + m.M22 * m.M03 * m.M11 + m.M02 * m.M13 * m.M21);
			M13 = (m.M12 * m.M23 * m.M00 + m.M22 * m.M03 * m.M10 + m.M02 * m.M13 * m.M20) - (m.M22 * m.M13 * m.M00 + m.M02 * m.M23 * m.M10 + m.M12 * m.M03 * m.M20);
			M23 = (m.M20 * m.M11 * m.M03 + m.M00 * m.M21 * m.M13 + m.M10 * m.M01 * m.M23) - (m.M10 * m.M21 * m.M03 + m.M20 * m.M01 * m.M13 + m.M00 * m.M11 * m.M23);

			// calculate determinant
			float det = 1.0f / (m.M00 * M00 + m.M10 * M01 + m.M20 * M02);

			// calculate matrix inverse/
			M00 *= det; M01 *= det; M02 *= det; M03 *= det;
			M10 *= det; M11 *= det; M12 *= det; M13 *= det;
			M20 *= det; M21 *= det; M22 *= det; M23 *= det;
		}

		/// <summary>
		/// transforms a vector. the translation is not beeing considered
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector3 TransformVector(Vector3 p)
		{
			return new Vector3(M00 * p.X + M01 * p.Y + M02 * p.Z + M03, M10 * p.X + M11 * p.Y + M12 * p.Z + M13, M20 * p.X + M21 * p.Y + M22 * p.Z + M23);
		}

		/// <summary>
		/// transforms a point and add translation vector
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector3 TransformPoint(Vector3 p)
		{
			return new Vector3(M00 * p.X + M01 * p.Y + M02 * p.Z + M03, M10 * p.X + M11 * p.Y + M12 * p.Z + M13, M20 * p.X + M21 * p.Y + M22 * p.Z + M23);
		}

		/// <summary>
		/// Remove scale from matrix.
		/// </summary>
		public void OrthonormalizeFast()
		{
			var x = new Vector3(M00, M10, M20);
			var y = new Vector3(M01, M11, M21);
			x = x.Normalized;
			var z = (x % y).Normalized;
			y = (z % x).Normalized;
			M00 = x.X; M10 = x.Y; M20 = x.Z;
			M01 = y.X; M11 = y.Y; M21 = y.Z;
			M02 = z.X; M12 = z.Y; M22 = z.Z;
		}

		/// <summary>
		/// determinant is ambiguous: only the upper-left-submatrix's determinant is calculated
		/// </summary>
		/// <returns></returns>
		public float Determinant()
		{
			return (M00 * M11 * M22) + (M01 * M12 * M20) + (M02 * M10 * M21) - (M02 * M11 * M20) - (M00 * M12 * M21) - (M01 * M10 * M22);
		}

		public static Matrix34 CreateSlerp(Matrix34 m, Matrix34 n, float t)
		{
			var matrix = new Matrix34();
			matrix.SetSlerp(m, n, t);

			return matrix;
		}

		/// <summary>
		/// Direct-Matrix-Slerp: for the sake of completeness, I have included the following
		/// expression for Spherical-Linear-Interpolation without using quaternions. This is much
		/// faster then converting both matrices into quaternions in order to do a quaternion slerp
		/// and then converting the slerped quaternion back into a matrix. This is a high-precision
		/// calculation. Given two orthonormal 3x3 matrices this function calculates the shortest
		/// possible interpolation-path between the two rotations. The interpolation curve forms a
		/// great arc on the rotation sphere (geodesic). Not only does Slerp follow a great arc it
		/// follows the shortest great arc. Furthermore Slerp has constant angular velocity. All in
		/// all Slerp is the optimal interpolation curve between two rotations. STABILITY
		/// PROBLEM: There are two singularities at angle=0 and
		/// angle=PI. At 0 the interpolation-axis is arbitrary, which means any axis will produce
		/// the same result because we have no rotation. Thats why I'm using (1,0,0). At PI the
		/// rotations point away from each other and the interpolation-axis is unpredictable. In
		/// this case I'm also using the axis (1,0,0). If the angle is ~0 or ~PI, then we have to
		/// normalize a very small vector and this can cause numerical instability. The
		/// quaternion-slerp has exactly the same problems. Ivo
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <param name="t"></param>
		/// <example>
		/// Matrix33 slerp=Matrix33::CreateSlerp( m,n,0.333f );
		/// </example>
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
			M00 = m.M00 * i.M00 + m.M01 * i.M10 + m.M02 * i.M20; M01 = m.M00 * i.M01 + m.M01 * i.M11 + m.M02 * i.M21; M02 = m.M00 * i.M02 + m.M01 * i.M12 + m.M02 * i.M22;
			M10 = m.M10 * i.M00 + m.M11 * i.M10 + m.M12 * i.M20; M11 = m.M10 * i.M01 + m.M11 * i.M11 + m.M12 * i.M21; M12 = m.M10 * i.M02 + m.M11 * i.M12 + m.M12 * i.M22;
			M20 = M01 * M12 - M02 * M11; M21 = M02 * M10 - M00 * M12; M22 = M00 * M11 - M01 * M10;

			M03 = m.M03 * (1 - t) + n.M03 * t;
			M13 = m.M13 * (1 - t) + n.M13 * t;
			M23 = m.M23 * (1 - t) + n.M23 * t;
		}

		public void SetTranslation(Vector3 t)
		{
			M03 = t.X;
			M13 = t.Y;
			M23 = t.Z;
		}

		public void ScaleTranslation(float s)
		{
			M03 *= s;
			M13 *= s;
			M23 *= s;
		}

		public Matrix34 AddTranslation(Vector3 t)
		{
			M03 += t.X;
			M13 += t.Y;
			M23 += t.Z;

			return this;
		}

		public void SetRotation33(Matrix33 m33)
		{
			M00 = m33.M00; M01 = m33.M01; M02 = m33.M02;
			M10 = m33.M10; M11 = m33.M11; M12 = m33.M12;
			M20 = m33.M20; M21 = m33.M21; M22 = m33.M22;
		}

		/// <summary>
		/// check if we have an orthonormal-base (general case, works even with reflection matrices)
		/// </summary>
		/// <param name="threshold"></param>
		/// <returns></returns>
		public int IsOrthonormal(float threshold = 0.001f)
		{
			var d0 = Math.Abs(ColumnVector0 | ColumnVector1); if (d0 > threshold) return 0;
			var d1 = Math.Abs(ColumnVector0 | ColumnVector2); if (d1 > threshold) return 0;
			var d2 = Math.Abs(ColumnVector1 | ColumnVector2); if (d2 > threshold) return 0;
			var a = (int)System.Convert.ChangeType((Math.Abs(1 - (ColumnVector0 | ColumnVector0))) < threshold, typeof(int));
			var b = (int)System.Convert.ChangeType((Math.Abs(1 - (ColumnVector1 | ColumnVector1))) < threshold, typeof(int));
			var c = (int)System.Convert.ChangeType((Math.Abs(1 - (ColumnVector2 | ColumnVector2))) < threshold, typeof(int));
			return a & b & c;
		}

		public int IsOrthonormalRH(float threshold = 0.001f)
		{
			var a = (int)System.Convert.ChangeType(ColumnVector0.IsEquivalent(ColumnVector1 % ColumnVector2, threshold), typeof(int));
			var b = (int)System.Convert.ChangeType(ColumnVector1.IsEquivalent(ColumnVector2 % ColumnVector0, threshold), typeof(int));
			var c = (int)System.Convert.ChangeType(ColumnVector2.IsEquivalent(ColumnVector0 % ColumnVector1, threshold), typeof(int));
			return a & b & c;
		}

		public bool IsEquivalent(Matrix34 m, float e = 0.05f)
		{
			return ((Math.Abs(M00 - m.M00) <= e) && (Math.Abs(M01 - m.M01) <= e) && (Math.Abs(M02 - m.M02) <= e) && (Math.Abs(M03 - m.M03) <= e) &&
			(Math.Abs(M10 - m.M10) <= e) && (Math.Abs(M11 - m.M11) <= e) && (Math.Abs(M12 - m.M12) <= e) && (Math.Abs(M13 - m.M13) <= e) &&
			(Math.Abs(M20 - m.M20) <= e) && (Math.Abs(M21 - m.M21) <= e) && (Math.Abs(M22 - m.M22) <= e) && (Math.Abs(M23 - m.M23) <= e));
		}

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
				hash = hash * 29 + M03.GetHashCode();

				hash = hash * 29 + M10.GetHashCode();
				hash = hash * 29 + M11.GetHashCode();
				hash = hash * 29 + M12.GetHashCode();
				hash = hash * 29 + M13.GetHashCode();

				hash = hash * 29 + M20.GetHashCode();
				hash = hash * 29 + M21.GetHashCode();
				hash = hash * 29 + M22.GetHashCode();
				hash = hash * 29 + M23.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}

		#region Operators
		/// <summary>
		/// Extracts <see cref="Matrix33" /> submatrix of <see cref="Matrix34" />.
		/// </summary>
		/// <param name="m"><see cref="Matrix34" /> to extract 3x3 matrix from.</param>
		/// <returns>Extracted matrix.</returns>
		public static explicit operator Matrix33(Matrix34 m)
		{
			return new Matrix33(m);
		}

		public static Matrix34 operator *(Matrix34 l, Matrix34 r)
		{
			var m = new Matrix34
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

			return m;
		}
		#endregion
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
		/// <summary>
		/// Enumerates this matrix.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<float> GetEnumerator()
		{
			yield return M00;
			yield return M01;
			yield return M02;
			yield return M03;
			yield return M10;
			yield return M11;
			yield return M12;
			yield return M13;
			yield return M20;
			yield return M21;
			yield return M22;
			yield return M23;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}