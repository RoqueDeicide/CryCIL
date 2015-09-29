using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil
{
	/// <summary>
	/// Represents a 4x4 matrix.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	public struct Matrix44 : IFormattable, IMatrix<Matrix44>
	{
		#region Statics
		/// <summary>
		/// Identity matrix.
		/// </summary>
		public static readonly Matrix44 Identity = new Matrix44(1, 0, 0, 0,
																0, 1, 0, 0,
																0, 0, 1, 0,
																0, 0, 0, 1);
		#endregion
		#region Fields
		#region Individuals
		/// <summary>
		/// First row, First column.
		/// </summary>
		[FieldOffset(0)] public float M00;
		/// <summary>
		/// First row, Second column.
		/// </summary>
		[FieldOffset(4)] public float M01;
		/// <summary>
		/// First row, Third column.
		/// </summary>
		[FieldOffset(8)] public float M02;
		/// <summary>
		/// First row, Fourth column.
		/// </summary>
		[FieldOffset(12)] public float M03;
		/// <summary>
		/// Second row, First column.
		/// </summary>
		[FieldOffset(16)] public float M10;
		/// <summary>
		/// Second row, Second column.
		/// </summary>
		[FieldOffset(20)] public float M11;
		/// <summary>
		/// Second row, Third column.
		/// </summary>
		[FieldOffset(24)] public float M12;
		/// <summary>
		/// Second row, Fourth column.
		/// </summary>
		[FieldOffset(28)] public float M13;
		/// <summary>
		/// Third row, First column.
		/// </summary>
		[FieldOffset(32)] public float M20;
		/// <summary>
		/// Third row, Second column.
		/// </summary>
		[FieldOffset(36)] public float M21;
		/// <summary>
		/// Third row, Third column.
		/// </summary>
		[FieldOffset(40)] public float M22;
		/// <summary>
		/// Third row, Fourth column.
		/// </summary>
		[FieldOffset(44)] public float M23;
		/// <summary>
		/// Fourth row, First column.
		/// </summary>
		[FieldOffset(48)] public float M30;
		/// <summary>
		/// Fourth row, Second column.
		/// </summary>
		[FieldOffset(52)] public float M31;
		/// <summary>
		/// Fourth row, Third column.
		/// </summary>
		[FieldOffset(56)] public float M32;
		/// <summary>
		/// Fourth row, Fourth column.
		/// </summary>
		[FieldOffset(60)] public float M33;
		#endregion
		#region Rows
		/// <summary>
		/// First row.
		/// </summary>
		[FieldOffset(0)] public Vector4 Row0;
		/// <summary>
		/// Second row.
		/// </summary>
		[FieldOffset(16)] public Vector4 Row1;
		/// <summary>
		/// Third row.
		/// </summary>
		[FieldOffset(32)] public Vector4 Row2;
		/// <summary>
		/// Fourth row.
		/// </summary>
		[FieldOffset(48)] public Vector4 Row3;
		#endregion
		#endregion
		#region Properties
		/// <summary>
		/// Gets inverted variation of this matrix.
		/// </summary>
		public Matrix44 Inverted
		{
			get
			{
				Matrix44 dst = this;

				if (!dst.Invert())
				{
					throw new DivideByZeroException("Attempt was made to invert a matrix which determinant is equal to 0.");
				}

				return dst;
			}
		}
		/// <summary>
		/// Gets determinant of this matrix.
		/// </summary>
		/// <remarks>This will only get you determinant of upper 3x3 submatrix.</remarks>
		public float QuickDeterminant
		{
			get
			{
				return this.M00 * this.M11 * this.M22 +
					   this.M01 * this.M12 * this.M20 +
					   this.M02 * this.M10 * this.M21 -
					   this.M02 * this.M11 * this.M20 -
					   this.M00 * this.M12 * this.M21 -
					   this.M01 * this.M10 * this.M22;
			}
		}
		/// <summary>
		/// Gets actual determinant.
		/// </summary>
		public float Determinant
		{
			get
			{
				// 2x2 sub-determinants
				float det20101 = this.M00 * this.M11 - this.M01 * this.M10;
				float det20102 = this.M00 * this.M12 - this.M02 * this.M10;
				float det20103 = this.M00 * this.M13 - this.M03 * this.M10;
				float det20112 = this.M01 * this.M12 - this.M02 * this.M11;
				float det20113 = this.M01 * this.M13 - this.M03 * this.M11;
				float det20123 = this.M02 * this.M13 - this.M03 * this.M12;

				// 3x3 sub-determinants
				float det3201012 = this.M20 * det20112 - this.M21 * det20102 + this.M22 * det20101;
				float det3201013 = this.M20 * det20113 - this.M21 * det20103 + this.M23 * det20101;
				float det3201023 = this.M20 * det20123 - this.M22 * det20103 + this.M23 * det20102;
				float det3201123 = this.M21 * det20123 - this.M22 * det20113 + this.M23 * det20112;

				return -det3201123 * this.M30 + det3201023 * this.M31 - det3201013 * this.M32 + det3201012 * this.M33;
			}
		}
		/// <summary>
		/// Gets new matrix that represents this matrix where rows and columns are swapped.
		/// </summary>
		public Matrix44 Transposed
		{
			get
			{
				return new Matrix44(this.M00, this.M10, this.M20, this.M30,
									this.M01, this.M11, this.M21, this.M31,
									this.M02, this.M12, this.M22, this.M32,
									this.M03, this.M13, this.M23, this.M33);
			}
		}
		/// <summary>
		/// Gets or sets translation transformation that is represented by this matrix.
		/// </summary>
		public Vector3 Translation
		{
			get { return new Vector3(this.M03, this.M13, this.M23); }
			set
			{
				this.M03 = value.X;
				this.M13 = value.Y;
				this.M23 = value.Z;
			}
		}
		/// <summary>
		/// Indicates whether elements of this matrix are valid numbers.
		/// </summary>
		public bool IsValid
		{
			get { return this.All(MathHelpers.IsNumberValid); }
		}
		/// <summary>
		/// Gives access to specific element of this matrix.
		/// </summary>
		/// <param name="index">Zero-based index of the element to get.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Attempt to access matrix component out of range [0, 15].
		/// </exception>
		public unsafe float this[int index]
		{
			get
			{
				if ((index | 0xF) != 0xF) // index < 0 || index > 3
				{
					throw new ArgumentOutOfRangeException("index", "Attempt to access matrix row out of range [0, 2].");
				}
				Contract.EndContractBlock();

				fixed (float* ptr = &this.M00)
				{
					return ptr[index];
				}
			}
			set
			{
				if ((index | 0xF) != 0xF) // index < 0 || index > 3
				{
					throw new ArgumentOutOfRangeException("index", "Attempt to access matrix row out of range [0, 2].");
				}
				Contract.EndContractBlock();

				fixed (float* ptr = &this.M00)
				{
					ptr[index] = value;
				}
			}
		}
		/// <summary>
		/// Gives access to specific element of this matrix.
		/// </summary>
		/// <param name="row">   Zero-based index of the row.</param>
		/// <param name="column">Zero-based index of the column.</param>
		public unsafe float this[int row, int column]
		{
			get
			{
				if ((row | 0x3) != 0x3) // row < 0 || row > 3
				{
					throw new ArgumentOutOfRangeException("row", "Attempt to access matrix row out of range [0, 2].");
				}
				if ((column | 0x3) != 0x3) // column < 0 || column > 3
				{
					throw new ArgumentOutOfRangeException("row", "Attempt to access matrix column out of range [0, 2].");
				}
				Contract.EndContractBlock();

				fixed (float* ptr = &this.M00)
				{
					return ptr[row * 4 + column];
				}
			}
			set
			{
				if ((row | 0x3) != 0x3) // row < 0 || row > 3
				{
					throw new ArgumentOutOfRangeException("row", "Attempt to access matrix row out of range [0, 2].");
				}
				if ((column | 0x3) != 0x3) // column < 0 || column > 3
				{
					throw new ArgumentOutOfRangeException("row", "Attempt to access matrix column out of range [0, 2].");
				}
				Contract.EndContractBlock();

				fixed (float* ptr = &this.M00)
				{
					ptr[row * 4 + column] = value;
				}
			}
		}
		/// <summary>
		/// Gets or sets first 3 columns of this matrix.
		/// </summary>
		public Matrix33 Matrix33
		{
			get
			{
				return new Matrix33(this.M00, this.M01, this.M02,
									this.M10, this.M11, this.M12,
									this.M20, this.M21, this.M22);
			}
			set
			{
				this.M00 = value.M00;
				this.M01 = value.M01;
				this.M02 = value.M02;
				this.M10 = value.M10;
				this.M11 = value.M11;
				this.M12 = value.M12;
				this.M20 = value.M20;
				this.M21 = value.M21;
				this.M22 = value.M22;
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
					{this.M00, this.M01, this.M02, this.M03},
					{this.M10, this.M11, this.M12, this.M13},
					{this.M20, this.M21, this.M22, this.M23},
					{this.M30, this.M31, this.M32, this.M33}
				};
			}
		}
		#endregion
		#region Contruction
		/// <summary>
		/// Creates new instance of <see cref="Matrix44"/> class.
		/// </summary>
		/// <param name="v00">First row, first column.</param>
		/// <param name="v01">First row, second column.</param>
		/// <param name="v02">First row, third column.</param>
		/// <param name="v03">First row, fourth column.</param>
		/// <param name="v10">Second row, first column.</param>
		/// <param name="v11">Second row, second column.</param>
		/// <param name="v12">Second row, third column.</param>
		/// <param name="v13">Second row, fourth column.</param>
		/// <param name="v20">Third row, first column.</param>
		/// <param name="v21">Third row, second column.</param>
		/// <param name="v22">Third row, third column.</param>
		/// <param name="v23">Third row, fourth column.</param>
		/// <param name="v30">Fourth row, first column.</param>
		/// <param name="v31">Fourth row, second column.</param>
		/// <param name="v32">Fourth row, third column.</param>
		/// <param name="v33">Fourth row, fourth column.</param>
		public Matrix44(float v00, float v01, float v02, float v03,
						float v10, float v11, float v12, float v13,
						float v20, float v21, float v22, float v23,
						float v30, float v31, float v32, float v33)
			: this()
		{
			this.M00 = v00;
			this.M01 = v01;
			this.M02 = v02;
			this.M03 = v03;
			this.M10 = v10;
			this.M11 = v11;
			this.M12 = v12;
			this.M13 = v13;
			this.M20 = v20;
			this.M21 = v21;
			this.M22 = v22;
			this.M23 = v23;
			this.M30 = v30;
			this.M31 = v31;
			this.M32 = v32;
			this.M33 = v33;
		}
		/// <summary>
		/// Creates new instance of <see cref="Matrix44"/> class.
		/// </summary>
		/// <param name="m"><see cref="Matrix33"/> to fill new matrix with.</param>
		public Matrix44(ref Matrix33 m)
			: this()
		{
			if (!m.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			this.M00 = m.M10;
			this.M01 = m.M11;
			this.M02 = m.M12;
			this.M03 = 0;
			this.M10 = m.M20;
			this.M11 = m.M21;
			this.M12 = m.M22;
			this.M13 = 0;
			this.M20 = m.M20;
			this.M21 = m.M21;
			this.M22 = m.M22;
			this.M23 = 0;
			this.M30 = 0;
			this.M31 = 0;
			this.M32 = 0;
			this.M33 = 1;
		}
		/// <summary>
		/// Creates new instance of <see cref="Matrix44"/> class.
		/// </summary>
		/// <param name="m"><see cref="Matrix34"/> to fill new matrix with.</param>
		public Matrix44(ref Matrix34 m)
			: this()
		{
			if (!m.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			this.M00 = m.M10;
			this.M01 = m.M11;
			this.M02 = m.M12;
			this.M03 = m.M13;
			this.M10 = m.M20;
			this.M11 = m.M21;
			this.M12 = m.M22;
			this.M13 = m.M23;
			this.M20 = m.M20;
			this.M21 = m.M21;
			this.M22 = m.M22;
			this.M23 = m.M23;
			this.M30 = 0;
			this.M31 = 0;
			this.M32 = 0;
			this.M33 = 1;
		}
		/// <summary>
		/// Copies contents from <see cref="Matrix44"/> to new one.
		/// </summary>
		/// <param name="m"></param>
		public Matrix44(ref Matrix44 m)
			: this()
		{
			if (!m.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			this.Row0 = m.Row0;
			this.Row1 = m.Row1;
			this.Row2 = m.Row2;
			this.Row3 = m.Row3;
		}
		#endregion
		#region Interface
		#region Simple Operations
		/// <summary>
		/// Sets elements of this matrix to values of identity matrix.
		/// </summary>
		public void SetIdentity()
		{
			this.M00 = 1;
			this.M01 = 0;
			this.M02 = 0;
			this.M03 = 0;
			this.M10 = 0;
			this.M11 = 1;
			this.M12 = 0;
			this.M13 = 0;
			this.M20 = 0;
			this.M21 = 0;
			this.M22 = 1;
			this.M23 = 0;
			this.M30 = 0;
			this.M31 = 0;
			this.M32 = 0;
			this.M33 = 1;
		}
		/// <summary>
		/// Swaps rows and columns of this matrix.
		/// </summary>
		public void Transpose()
		{
			Matrix44 tmp = this;
			this.M00 = tmp.M00;
			this.M01 = tmp.M10;
			this.M02 = tmp.M20;
			this.M03 = tmp.M30;
			this.M10 = tmp.M01;
			this.M11 = tmp.M11;
			this.M12 = tmp.M21;
			this.M13 = tmp.M31;
			this.M20 = tmp.M02;
			this.M21 = tmp.M12;
			this.M22 = tmp.M22;
			this.M23 = tmp.M32;
			this.M30 = tmp.M03;
			this.M31 = tmp.M13;
			this.M32 = tmp.M23;
			this.M33 = tmp.M33;
		}
		/// <summary>
		/// Calculate a real inversion of a Matrix44.
		/// </summary>
		/// <remarks>
		/// Uses Cramer's Rule which is faster (branchless) but numerically more unstable than other
		/// methods like Gaussian Elimination.
		/// </remarks>
		/// <returns>False, if this matrix's determinant is equal to zero, otherwise true.</returns>
		public bool Invert()
		{
			// Calculate determinant
			float det = this.Determinant;

			if (Math.Abs(det) < MathHelpers.ZeroTolerance)
			{
				return false;
			}

			Matrix44 m = this;

			// Calculate pairs for first 8 elements (cofactors)
			float f00 = m.M22 * m.M33;
			float f01 = m.M32 * m.M23;
			float f02 = m.M12 * m.M33;
			float f03 = m.M32 * m.M13;
			float f04 = m.M12 * m.M23;
			float f05 = m.M22 * m.M13;
			float f06 = m.M02 * m.M33;
			float f07 = m.M32 * m.M03;
			float f08 = m.M02 * m.M23;
			float f09 = m.M22 * m.M03;
			float f10 = m.M02 * m.M13;
			float f11 = m.M12 * m.M03;

			// Calculate first 8 elements (cofactors)
			this.M00 = f00 * m.M11 + f03 * m.M21 + f04 * m.M31 - f01 * m.M11 - f02 * m.M21 - f05 * m.M31;
			this.M01 = f01 * m.M01 + f06 * m.M21 + f09 * m.M31 - f00 * m.M01 - f07 * m.M21 - f08 * m.M31;
			this.M02 = f02 * m.M01 + f07 * m.M11 + f10 * m.M31 - f03 * m.M01 - f06 * m.M11 - f11 * m.M31;
			this.M03 = f05 * m.M01 + f08 * m.M11 + f11 * m.M21 - f04 * m.M01 - f09 * m.M11 - f10 * m.M21;
			this.M10 = f01 * m.M10 + f02 * m.M20 + f05 * m.M30 - f00 * m.M10 - f03 * m.M20 - f04 * m.M30;
			this.M11 = f00 * m.M00 + f07 * m.M20 + f08 * m.M30 - f01 * m.M00 - f06 * m.M20 - f09 * m.M30;
			this.M12 = f03 * m.M00 + f06 * m.M10 + f11 * m.M30 - f02 * m.M00 - f07 * m.M10 - f10 * m.M30;
			this.M13 = f04 * m.M00 + f09 * m.M10 + f10 * m.M20 - f05 * m.M00 - f08 * m.M10 - f11 * m.M20;

			// Calculate pairs for second 8 elements (cofactors)
			f00 = m.M20 * m.M31;
			f01 = m.M30 * m.M21;
			f02 = m.M10 * m.M31;
			f03 = m.M30 * m.M11;
			f04 = m.M10 * m.M21;
			f05 = m.M20 * m.M11;
			f06 = m.M00 * m.M31;
			f07 = m.M30 * m.M01;
			f08 = m.M00 * m.M21;
			f09 = m.M20 * m.M01;
			f10 = m.M00 * m.M11;
			f11 = m.M10 * m.M01;

			// Calculate second 8 elements (cofactors)
			this.M20 = f00 * m.M13 + f03 * m.M23 + f04 * m.M33 - f01 * m.M13 - f02 * m.M23 - f05 * m.M33;
			this.M21 = f01 * m.M03 + f06 * m.M23 + f09 * m.M33 - f00 * m.M03 - f07 * m.M23 - f08 * m.M33;
			this.M22 = f02 * m.M03 + f07 * m.M13 + f10 * m.M33 - f03 * m.M03 - f06 * m.M13 - f11 * m.M33;
			this.M23 = f05 * m.M03 + f08 * m.M13 + f11 * m.M23 - f04 * m.M03 - f09 * m.M13 - f10 * m.M23;
			this.M30 = f02 * m.M22 + f05 * m.M32 + f01 * m.M12 - f04 * m.M32 - f00 * m.M12 - f03 * m.M22;
			this.M31 = f08 * m.M32 + f00 * m.M02 + f07 * m.M22 - f06 * m.M22 - f09 * m.M32 - f01 * m.M02;
			this.M32 = f06 * m.M12 + f11 * m.M32 + f03 * m.M02 - f10 * m.M32 - f02 * m.M02 - f07 * m.M12;
			this.M33 = f10 * m.M22 + f04 * m.M02 + f09 * m.M12 - f08 * m.M12 - f11 * m.M22 - f05 * m.M02;

			// Divide the cofactor-matrix by the determinant
			float idet = 1.0f / det;
			this.M00 *= idet;
			this.M01 *= idet;
			this.M02 *= idet;
			this.M03 *= idet;
			this.M10 *= idet;
			this.M11 *= idet;
			this.M12 *= idet;
			this.M13 *= idet;
			this.M20 *= idet;
			this.M21 *= idet;
			this.M22 *= idet;
			this.M23 *= idet;
			this.M30 *= idet;
			this.M31 *= idet;
			this.M32 *= idet;
			this.M33 *= idet;
			return true;
		}
		#endregion
		#region Operators
		/// <summary>
		/// Calculates product of one matrix and a number.
		/// </summary>
		/// <param name="m">Left operand.</param>
		/// <param name="f">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Matrix44 operator *(Matrix44 m, float f)
		{
			if (!m.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			Matrix44 r = new Matrix44
			{
				Row0 = {X = m.M00 * f, Y = m.M01 * f, Z = m.M02 * f, W = m.M03 * f},
				Row1 = {X = m.M10 * f, Y = m.M11 * f, Z = m.M12 * f, W = m.M13 * f},
				Row2 = {X = m.M20 * f, Y = m.M21 * f, Z = m.M22 * f, W = m.M23 * f},
				Row3 = {X = m.M30 * f, Y = m.M31 * f, Z = m.M32 * f, W = m.M33 * f}
			};
			return r;
		}
		/// <summary>
		/// Calculates sum of two matrices.
		/// </summary>
		/// <param name="mm0">Left operand.</param>
		/// <param name="mm1">Right operand.</param>
		/// <returns>Result of operation.</returns>
		public static Matrix44 operator +(Matrix44 mm0, Matrix44 mm1)
		{
			if (!mm0.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			if (!mm1.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			Matrix44 r = new Matrix44
			{
				Row0 =
				{
					X = mm0.M00 + mm1.M00,
					Y = mm0.M01 + mm1.M01,
					Z = mm0.M02 + mm1.M02,
					W = mm0.M03 + mm1.M03
				},
				Row1 =
				{
					X = mm0.M10 + mm1.M10,
					Y = mm0.M11 + mm1.M11,
					Z = mm0.M12 + mm1.M12,
					W = mm0.M13 + mm1.M13
				},
				Row2 =
				{
					X = mm0.M20 + mm1.M20,
					Y = mm0.M21 + mm1.M21,
					Z = mm0.M22 + mm1.M22,
					W = mm0.M23 + mm1.M23
				},
				Row3 =
				{
					X = mm0.M30 + mm1.M30,
					Y = mm0.M31 + mm1.M31,
					Z = mm0.M32 + mm1.M32,
					W = mm0.M33 + mm1.M33
				}
			};
			return r;
		}
		/// <summary>
		/// Multiplies two matrices together.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This operation is used to combine transformations represented by these matrices together into
		/// one matrix.
		/// </para>
		/// <para>This operation takes 48 mults and 24 adds.</para>
		/// </remarks>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Matrix44 operator *(Matrix44 l, Matrix33 r)
		{
			if (!l.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			if (!r.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			Matrix44 m = new Matrix44
			{
				Row0 =
				{
					X = l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20,
					Y = l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21,
					Z = l.M00 * r.M02 + l.M01 * r.M12 + l.M02 * r.M22,
					W = l.M03
				},
				Row1 =
				{
					X = l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20,
					Y = l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21,
					Z = l.M10 * r.M02 + l.M11 * r.M12 + l.M12 * r.M22,
					W = l.M13
				},
				Row2 =
				{
					X = l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20,
					Y = l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21,
					Z = l.M20 * r.M02 + l.M21 * r.M12 + l.M22 * r.M22,
					W = l.M23
				},
				Row3 =
				{
					X = l.M30 * r.M00 + l.M31 * r.M10 + l.M32 * r.M20,
					Y = l.M30 * r.M01 + l.M31 * r.M11 + l.M32 * r.M21,
					Z = l.M30 * r.M02 + l.M31 * r.M12 + l.M32 * r.M22,
					W = l.M33
				}
			};
			return m;
		}
		/// <summary>
		/// Multiplies two matrices together.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This operation is used to combine transformations represented by these matrices together into
		/// one matrix.
		/// </para>
		/// <para>This operation takes 48 mults and 36 adds.</para>
		/// </remarks>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Matrix44 operator *(Matrix44 l, Matrix34 r)
		{
			if (!l.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			if (!r.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			// ReSharper disable UseObjectOrCollectionInitializer
			Matrix44 m = new Matrix44();
			// ReSharper restore UseObjectOrCollectionInitializer
			m.M00 = l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20;
			m.M10 = l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20;
			m.M20 = l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20;
			m.M30 = l.M30 * r.M00 + l.M31 * r.M10 + l.M32 * r.M20;
			m.M01 = l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21;
			m.M11 = l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21;
			m.M21 = l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21;
			m.M31 = l.M30 * r.M01 + l.M31 * r.M11 + l.M32 * r.M21;
			m.M02 = l.M00 * r.M02 + l.M01 * r.M12 + l.M02 * r.M22;
			m.M12 = l.M10 * r.M02 + l.M11 * r.M12 + l.M12 * r.M22;
			m.M22 = l.M20 * r.M02 + l.M21 * r.M12 + l.M22 * r.M22;
			m.M32 = l.M30 * r.M02 + l.M31 * r.M12 + l.M32 * r.M22;
			m.M03 = l.M00 * r.M03 + l.M01 * r.M13 + l.M02 * r.M23 + l.M03;
			m.M13 = l.M10 * r.M03 + l.M11 * r.M13 + l.M12 * r.M23 + l.M13;
			m.M23 = l.M20 * r.M03 + l.M21 * r.M13 + l.M22 * r.M23 + l.M23;
			m.M33 = l.M30 * r.M03 + l.M31 * r.M13 + l.M32 * r.M23 + l.M33;
			return m;
		}
		/// <summary>
		/// Multiplies two matrices together.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This operation is used to combine transformations represented by these matrices together into
		/// one matrix.
		/// </para>
		/// <para>This operation takes 48 mults and 36 adds.</para>
		/// </remarks>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Matrix44 operator *(Matrix44 l, Matrix44 r)
		{
			if (!l.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			if (!r.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			// ReSharper disable UseObjectOrCollectionInitializer
			Matrix44 res = new Matrix44();
			// ReSharper restore UseObjectOrCollectionInitializer
			res.M00 = l.M00 * r.M00 + l.M01 * r.M10 + l.M02 * r.M20 + l.M03 * r.M30;
			res.M10 = l.M10 * r.M00 + l.M11 * r.M10 + l.M12 * r.M20 + l.M13 * r.M30;
			res.M20 = l.M20 * r.M00 + l.M21 * r.M10 + l.M22 * r.M20 + l.M23 * r.M30;
			res.M30 = l.M30 * r.M00 + l.M31 * r.M10 + l.M32 * r.M20 + l.M33 * r.M30;
			res.M01 = l.M00 * r.M01 + l.M01 * r.M11 + l.M02 * r.M21 + l.M03 * r.M31;
			res.M11 = l.M10 * r.M01 + l.M11 * r.M11 + l.M12 * r.M21 + l.M13 * r.M31;
			res.M21 = l.M20 * r.M01 + l.M21 * r.M11 + l.M22 * r.M21 + l.M23 * r.M31;
			res.M31 = l.M30 * r.M01 + l.M31 * r.M11 + l.M32 * r.M21 + l.M33 * r.M31;
			res.M02 = l.M00 * r.M02 + l.M01 * r.M12 + l.M02 * r.M22 + l.M03 * r.M32;
			res.M12 = l.M10 * r.M02 + l.M11 * r.M12 + l.M12 * r.M22 + l.M13 * r.M32;
			res.M22 = l.M20 * r.M02 + l.M21 * r.M12 + l.M22 * r.M22 + l.M23 * r.M32;
			res.M32 = l.M30 * r.M02 + l.M31 * r.M12 + l.M32 * r.M22 + l.M33 * r.M32;
			res.M03 = l.M00 * r.M03 + l.M01 * r.M13 + l.M02 * r.M23 + l.M03 * r.M33;
			res.M13 = l.M10 * r.M03 + l.M11 * r.M13 + l.M12 * r.M23 + l.M13 * r.M33;
			res.M23 = l.M20 * r.M03 + l.M21 * r.M13 + l.M22 * r.M23 + l.M23 * r.M33;
			res.M33 = l.M30 * r.M03 + l.M31 * r.M13 + l.M32 * r.M23 + l.M33 * r.M33;
			return res;
		}
		/// <summary>
		/// Multiplies matrix by vector.
		/// </summary>
		/// <param name="m">Left operand.</param>
		/// <param name="v">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Vector4 operator *(Matrix44 m, Vector4 v)
		{
			if (!m.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			if (!v.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid vector.");
			}
			return new Vector4(v.X * m.M00 + v.Y * m.M01 + v.Z * m.M02 + v.W * m.M03,
							   v.X * m.M10 + v.Y * m.M11 + v.Z * m.M12 + v.W * m.M13,
							   v.X * m.M20 + v.Y * m.M21 + v.Z * m.M22 + v.W * m.M23,
							   v.X * m.M30 + v.Y * m.M31 + v.Z * m.M32 + v.W * m.M33);
		}
		/// <summary>
		/// Multiplies vector by matrix.
		/// </summary>
		/// <remarks>This operation is done to transform the vector by the matrix.</remarks>
		/// <param name="v">Left operand.</param>
		/// <param name="m">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static Vector4 operator *(Vector4 v, Matrix44 m)
		{
			if (!m.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid matrix.");
			}
			if (!v.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid vector.");
			}
			return new Vector4(v.X * m.M00 + v.Y * m.M10 + v.Z * m.M20 + v.W * m.M30,
							   v.X * m.M01 + v.Y * m.M11 + v.Z * m.M21 + v.W * m.M31,
							   v.X * m.M02 + v.Y * m.M12 + v.Z * m.M22 + v.W * m.M32,
							   v.X * m.M03 + v.Y * m.M13 + v.Z * m.M23 + v.W * m.M33);
		}
		#endregion
		#region Transformations
		/// <summary>
		/// Applies transformation that is represented by this matrix to the vector.
		/// </summary>
		/// <param name="b">Vector to transform.</param>
		/// <returns>Transformed vector.</returns>
		public Vector3 TransformVector(Vector3 b)
		{
			if (!b.IsValid)
			{
				throw new ArgumentException("Parameter must be a valid vector.");
			}
			return new Vector3
			{
				X = this.M00 * b.X + this.M01 * b.Y + this.M02 * b.Z,
				Y = this.M10 * b.X + this.M11 * b.Y + this.M12 * b.Z,
				Z = this.M20 * b.X + this.M21 * b.Y + this.M22 * b.Z
			};
		}
		#endregion
		#region Get/Set Rows
		/// <summary>
		/// Sets first 3 columns of the row of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the row to set.</param>
		/// <param name="v">Vector that supplies new values.</param>
		public void SetRow(int i, Vector3 v)
		{
			this[i, 0] = v.X;
			this[i, 1] = v.Y;
			this[i, 2] = v.Z;
		}
		/// <summary>
		/// Sets row of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the row to set.</param>
		/// <param name="v">Vector that supplies new values.</param>
		public void SetRow4(int i, Vector4 v)
		{
			this[i, 0] = v.X;
			this[i, 1] = v.Y;
			this[i, 2] = v.Z;
			this[i, 3] = v.W;
		}
		/// <summary>
		/// Gets first 3 columns of the row of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the row to get.</param>
		/// <returns><see cref="Vector3"/> object that contains the contents of the row.</returns>
		public Vector3 GetRow(int i)
		{
			return new Vector3(this[i, 0], this[i, 1], this[i, 2]);
		}
		/// <summary>
		/// Gets first the row of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the row to get.</param>
		/// <returns><see cref="Vector4"/> object that contains the contents of the row.</returns>
		public Vector4 GetRow4(int i)
		{
			return new Vector4(this[i, 0], this[i, 1], this[i, 2], this[i, 3]);
		}
		#endregion
		#region Get/Set Columns
		/// <summary>
		/// Sets first 3 rows of the column of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the column to set.</param>
		/// <param name="v">Vector that supplies new values.</param>
		public void SetColumn(int i, Vector3 v)
		{
			if (i < 0 || i > 3)
			{
				throw new ArgumentOutOfRangeException("i", "Index of the column must be in [0; 3].");
			}
			switch (i)
			{
				case 0:
					this.M00 = v.X;
					this.M10 = v.Y;
					this.M20 = v.Z;
					break;
				case 1:
					this.M01 = v.X;
					this.M11 = v.Y;
					this.M21 = v.Z;
					break;
				case 2:
					this.M02 = v.X;
					this.M12 = v.Y;
					this.M22 = v.Z;
					break;
				case 3:
					this.M03 = v.X;
					this.M13 = v.Y;
					this.M23 = v.Z;
					break;
			}
		}
		/// <summary>
		/// Sets the column of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the column to set.</param>
		/// <param name="v">Vector that supplies new values.</param>
		public void SetColumn(int i, Vector4 v)
		{
			if (i < 0 || i > 3)
			{
				throw new ArgumentOutOfRangeException("i", "Index of the column must be in [0; 3].");
			}
			switch (i)
			{
				case 0:
					this.M00 = v.X;
					this.M10 = v.Y;
					this.M20 = v.Z;
					this.M30 = v.W;
					break;
				case 1:
					this.M01 = v.X;
					this.M11 = v.Y;
					this.M21 = v.Z;
					this.M31 = v.W;
					break;
				case 2:
					this.M02 = v.X;
					this.M12 = v.Y;
					this.M22 = v.Z;
					this.M32 = v.W;
					break;
				case 3:
					this.M03 = v.X;
					this.M13 = v.Y;
					this.M23 = v.Z;
					this.M33 = v.W;
					break;
			}
		}
		/// <summary>
		/// Gets first 3 rows of the column of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the column to get.</param>
		/// <returns>Vector that contains values.</returns>
		public Vector3 GetColumn(int i)
		{
			if (i < 0 || i > 3)
			{
				throw new ArgumentOutOfRangeException("i", "Index of the column must be in [0; 3].");
			}
			switch (i)
			{
				case 0:
					return new Vector3(this.M00, this.M10, this.M20);
				case 1:
					return new Vector3(this.M01, this.M11, this.M21);
				case 2:
					return new Vector3(this.M02, this.M12, this.M22);
				case 3:
					return new Vector3(this.M03, this.M13, this.M23);
				default:
					return Vector3.Zero;
			}
		}
		/// <summary>
		/// Gets the column of this matrix.
		/// </summary>
		/// <param name="i">Zero-based index of the column to get.</param>
		/// <returns>Vector that contains values.</returns>
		public Vector4 GetColumn4(int i)
		{
			if (i < 0 || i > 3)
			{
				throw new ArgumentOutOfRangeException("i", "Index of the column must be in [0; 3].");
			}
			switch (i)
			{
				case 0:
					return new Vector4(this.M00, this.M10, this.M20, this.M30);
				case 1:
					return new Vector4(this.M01, this.M11, this.M21, this.M31);
				case 2:
					return new Vector4(this.M02, this.M12, this.M22, this.M32);
				case 3:
					return new Vector4(this.M03, this.M13, this.M23, this.M33);
				default:
					return Vector4.Zero;
			}
		}
		#endregion
		#region Comparisons
		/// <summary>
		/// Determines whether first matrix can be considered equivalent of second one.
		/// </summary>
		/// <param name="m0">First matrix.</param>
		/// <param name="m1">Second matrix.</param>
		/// <param name="e"> Precision of comparison.</param>
		/// <returns>True, if matrices can be considered equivalents.</returns>
		public static bool IsEquivalent(ref Matrix44 m0, ref Matrix44 m1, float e = MathHelpers.ZeroTolerance)
		{
			return (Math.Abs(m0.M00 - m1.M00) <= e) && (Math.Abs(m0.M01 - m1.M01) <= e) &&
				   (Math.Abs(m0.M02 - m1.M02) <= e) && (Math.Abs(m0.M03 - m1.M03) <= e) &&
				   (Math.Abs(m0.M10 - m1.M10) <= e) && (Math.Abs(m0.M11 - m1.M11) <= e) &&
				   (Math.Abs(m0.M12 - m1.M12) <= e) && (Math.Abs(m0.M13 - m1.M13) <= e) &&
				   (Math.Abs(m0.M20 - m1.M20) <= e) && (Math.Abs(m0.M21 - m1.M21) <= e) &&
				   (Math.Abs(m0.M22 - m1.M22) <= e) && (Math.Abs(m0.M23 - m1.M23) <= e) &&
				   (Math.Abs(m0.M30 - m1.M30) <= e) && (Math.Abs(m0.M31 - m1.M31) <= e) &&
				   (Math.Abs(m0.M32 - m1.M32) <= e) && (Math.Abs(m0.M33 - m1.M33) <= e);
		}
		/// <summary>
		/// Determines whether this matrix is equal to another object.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if another object is a non-null boxed object of type <see cref="Matrix44"/> that is equal
		/// to this one.
		/// </returns>
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(null, obj) && (obj is Matrix44 && this.Equals((Matrix44)obj));
		}
		/// <summary>
		/// Calculates hash code of this matrix.
		/// </summary>
		/// <returns>
		/// Hash code calculated using aggregation of elements of the matrix by consecutive multiplication
		/// and XOR operations.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.M00.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M01.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M02.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M03.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M10.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M11.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M12.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M13.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M20.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M21.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M22.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M23.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M30.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M31.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M32.GetHashCode();
				hashCode = (hashCode * 397) ^ this.M33.GetHashCode();
				return hashCode;
			}
		}
		/// <summary>
		/// Determines whether two matrices are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if matrices are equal, otherwise false.</returns>
		public static bool operator ==(Matrix44 left, Matrix44 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines whether two matrices are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if matrices are not equal, otherwise false.</returns>
		public static bool operator !=(Matrix44 left, Matrix44 right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Determines whether this matrix is equal to another.
		/// </summary>
		/// <param name="other">Another matrix.</param>
		/// <returns>True, if matrices are equal.</returns>
		public bool Equals(Matrix44 other)
		{
			return IsEquivalent(ref this, ref other);
		}
		#endregion
		#region Enumeration
		/// <summary>
		/// Creates an object that enumerates this matrix.
		/// </summary>
		/// <returns>An object that enumerates this matrix.</returns>
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

			yield return this.M30;
			yield return this.M31;
			yield return this.M32;
			yield return this.M33;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
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
		#endregion
	}
}