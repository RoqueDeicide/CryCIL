using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CryCil.Geometry;

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
		public static readonly Matrix44 Identity = new Matrix44
		(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1
		);
		#endregion
		#region Fields
		#region Individuals
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
		/// <summary>
		/// Fourth row, First column.
		/// </summary>
		[FieldOffset(48)]
		public float M30;
		/// <summary>
		/// Fourth row, Second column.
		/// </summary>
		[FieldOffset(52)]
		public float M31;
		/// <summary>
		/// Fourth row, Third column.
		/// </summary>
		[FieldOffset(56)]
		public float M32;
		/// <summary>
		/// Fourth row, Fourth column.
		/// </summary>
		[FieldOffset(60)]
		public float M33;
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
		/// <summary>
		/// Fourth row.
		/// </summary>
		[FieldOffset(48)]
		public Vector4 Row3;
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
				return
					(this.Row0.X * this.Row1.Y * this.Row2.Z) +
					(this.Row0.Y * this.Row1.Z * this.Row2.X) +
					(this.Row0.Z * this.Row1.X * this.Row2.Y) -
					(this.Row0.Z * this.Row1.Y * this.Row2.X) -
					(this.Row0.X * this.Row1.Z * this.Row2.Y) -
					(this.Row0.Y * this.Row1.X * this.Row2.Z);
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
				float det2_01_01 = this.M00 * this.M11 - this.M01 * this.M10;
				float det2_01_02 = this.M00 * this.M12 - this.M02 * this.M10;
				float det2_01_03 = this.M00 * this.M13 - this.M03 * this.M10;
				float det2_01_12 = this.M01 * this.M12 - this.M02 * this.M11;
				float det2_01_13 = this.M01 * this.M13 - this.M03 * this.M11;
				float det2_01_23 = this.M02 * this.M13 - this.M03 * this.M12;

				// 3x3 sub-determinants
				float det3_201_012 = this.M20 * det2_01_12 - this.M21 * det2_01_02 + this.M22 * det2_01_01;
				float det3_201_013 = this.M20 * det2_01_13 - this.M21 * det2_01_03 + this.M23 * det2_01_01;
				float det3_201_023 = this.M20 * det2_01_23 - this.M22 * det2_01_03 + this.M23 * det2_01_02;
				float det3_201_123 = this.M21 * det2_01_23 - this.M22 * det2_01_13 + this.M23 * det2_01_12;

				return (-det3_201_123 * this.M30 + det3_201_023 * this.M31 - det3_201_013 * this.M32 + det3_201_012 * this.M33);
			}
		}
		/// <summary>
		/// Gets new matrix that represents this matrix where rows and columns are swapped.
		/// </summary>
		public Matrix44 Transposed
		{
			get
			{
				return new Matrix44
							(
								this.Row0.X, this.Row1.X, this.Row2.X, this.Row3.X,
								this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.Y,
								this.Row0.Z, this.Row1.Z, this.Row2.Z, this.Row3.Z,
								this.Row0.W, this.Row1.W, this.Row2.W, this.Row3.W
							);
			}
		}
		/// <summary>
		/// Gets or sets translation transformation that is represented by this matrix.
		/// </summary>
		public Vector3 Translation
		{
			get
			{
				return new Vector3(this.Row0.W, this.Row1.W, this.Row2.W);
			}
			set
			{
				this.Row0.W = value.X;
				this.Row1.W = value.Y;
				this.Row2.W = value.Z;
			}
		}
		/// <summary>
		/// Indicates whether elements of this matrix are valid numbers.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return this.All(MathHelpers.IsNumberValid);
			}
		}
		/// <summary>
		/// Gives access to specific element of this matrix.
		/// </summary>
		/// <param name="index">Zero-based index of the element to get.</param>
		public float this[int index]
		{
			get { return this[index / 4, index % 4]; }
			set { this[index / 4, index % 4] = value; }
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
					case 3:
						return this.Row3[column];
					default:
						throw new ArgumentOutOfRangeException("row", "Attempt to access matrix row out of range [0, 3].");
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
					case 3:
						this.Row3[column] = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("row", "Attempt to access matrix row out of range [0, 3].");
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
			this.Row0.X = v00; this.Row0.Y = v01; this.Row0.Z = v02; this.Row0.W = v03;
			this.Row1.X = v10; this.Row1.Y = v11; this.Row1.Z = v12; this.Row1.W = v13;
			this.Row2.X = v20; this.Row2.Y = v21; this.Row2.Z = v22; this.Row2.W = v23;
			this.Row3.X = v30; this.Row3.Y = v31; this.Row3.Z = v32; this.Row3.W = v33;
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
			this.Row0.X = m.Row1.X; this.Row0.Y = m.Row1.Y; this.Row0.Z = m.Row1.Z; this.Row0.W = 0;
			this.Row1.X = m.Row2.X; this.Row1.Y = m.Row2.Y; this.Row1.Z = m.Row2.Z; this.Row1.W = 0;
			this.Row2.X = m.Row2.X; this.Row2.Y = m.Row2.Y; this.Row2.Z = m.Row2.Z; this.Row2.W = 0;
			this.Row3.X = 0; this.Row3.Y = 0; this.Row3.Z = 0; this.Row3.W = 1;
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
			this.Row0.X = m.Row1.X; this.Row0.Y = m.Row1.Y; this.Row0.Z = m.Row1.Z; this.Row0.W = m.Row1.W;
			this.Row1.X = m.Row2.X; this.Row1.Y = m.Row2.Y; this.Row1.Z = m.Row2.Z; this.Row1.W = m.Row2.W;
			this.Row2.X = m.Row2.X; this.Row2.Y = m.Row2.Y; this.Row2.Z = m.Row2.Z; this.Row2.W = m.Row2.W;
			this.Row3.X = 0; this.Row3.Y = 0; this.Row3.Z = 0; this.Row3.W = 1;
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
			this.Row0.X = 1; this.Row0.Y = 0; this.Row0.Z = 0; this.Row0.W = 0;
			this.Row1.X = 0; this.Row1.Y = 1; this.Row1.Z = 0; this.Row1.W = 0;
			this.Row2.X = 0; this.Row2.Y = 0; this.Row2.Z = 1; this.Row2.W = 0;
			this.Row3.X = 0; this.Row3.Y = 0; this.Row3.Z = 0; this.Row3.W = 1;
		}
		/// <summary>
		/// Swaps rows and columns of this matrix.
		/// </summary>
		public void Transpose()
		{
			Matrix44 tmp = this;
			this.Row0.X = tmp.Row0.X; this.Row0.Y = tmp.Row1.X; this.Row0.Z = tmp.Row2.X; this.Row0.W = tmp.Row3.X;
			this.Row1.X = tmp.Row0.Y; this.Row1.Y = tmp.Row1.Y; this.Row1.Z = tmp.Row2.Y; this.Row1.W = tmp.Row3.Y;
			this.Row2.X = tmp.Row0.Z; this.Row2.Y = tmp.Row1.Z; this.Row2.Z = tmp.Row2.Z; this.Row2.W = tmp.Row3.Z;
			this.Row3.X = tmp.Row0.W; this.Row3.Y = tmp.Row1.W; this.Row3.Z = tmp.Row2.W; this.Row3.W = tmp.Row3.W;
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
			float[] tmp = new float[12];
			// Calculate determinant
			float det = this.Determinant;

			if (Math.Abs(det) < MathHelpers.ZeroTolerance)
			{
				return false;
			}
			Matrix44 m = this;

			// Calculate pairs for first 8 elements (cofactors)
			tmp[0] = m.Row2.Z * m.Row3.W;
			tmp[1] = m.Row3.Z * m.Row2.W;
			tmp[2] = m.Row1.Z * m.Row3.W;
			tmp[3] = m.Row3.Z * m.Row1.W;
			tmp[4] = m.Row1.Z * m.Row2.W;
			tmp[5] = m.Row2.Z * m.Row1.W;
			tmp[6] = m.Row0.Z * m.Row3.W;
			tmp[7] = m.Row3.Z * m.Row0.W;
			tmp[8] = m.Row0.Z * m.Row2.W;
			tmp[9] = m.Row2.Z * m.Row0.W;
			tmp[10] = m.Row0.Z * m.Row1.W;
			tmp[11] = m.Row1.Z * m.Row0.W;

			// Calculate first 8 elements (cofactors)
			this.Row0.X = tmp[0] * m.Row1.Y + tmp[3] * m.Row2.Y + tmp[4] * m.Row3.Y;
			this.Row0.X -= tmp[1] * m.Row1.Y + tmp[2] * m.Row2.Y + tmp[5] * m.Row3.Y;
			this.Row0.Y = tmp[1] * m.Row0.Y + tmp[6] * m.Row2.Y + tmp[9] * m.Row3.Y;
			this.Row0.Y -= tmp[0] * m.Row0.Y + tmp[7] * m.Row2.Y + tmp[8] * m.Row3.Y;
			this.Row0.Z = tmp[2] * m.Row0.Y + tmp[7] * m.Row1.Y + tmp[10] * m.Row3.Y;
			this.Row0.Z -= tmp[3] * m.Row0.Y + tmp[6] * m.Row1.Y + tmp[11] * m.Row3.Y;
			this.Row0.W = tmp[5] * m.Row0.Y + tmp[8] * m.Row1.Y + tmp[11] * m.Row2.Y;
			this.Row0.W -= tmp[4] * m.Row0.Y + tmp[9] * m.Row1.Y + tmp[10] * m.Row2.Y;
			this.Row1.X = tmp[1] * m.Row1.X + tmp[2] * m.Row2.X + tmp[5] * m.Row3.X;
			this.Row1.X -= tmp[0] * m.Row1.X + tmp[3] * m.Row2.X + tmp[4] * m.Row3.X;
			this.Row1.Y = tmp[0] * m.Row0.X + tmp[7] * m.Row2.X + tmp[8] * m.Row3.X;
			this.Row1.Y -= tmp[1] * m.Row0.X + tmp[6] * m.Row2.X + tmp[9] * m.Row3.X;
			this.Row1.Z = tmp[3] * m.Row0.X + tmp[6] * m.Row1.X + tmp[11] * m.Row3.X;
			this.Row1.Z -= tmp[2] * m.Row0.X + tmp[7] * m.Row1.X + tmp[10] * m.Row3.X;
			this.Row1.W = tmp[4] * m.Row0.X + tmp[9] * m.Row1.X + tmp[10] * m.Row2.X;
			this.Row1.W -= tmp[5] * m.Row0.X + tmp[8] * m.Row1.X + tmp[11] * m.Row2.X;

			// Calculate pairs for second 8 elements (cofactors)
			tmp[0] = m.Row2.X * m.Row3.Y;
			tmp[1] = m.Row3.X * m.Row2.Y;
			tmp[2] = m.Row1.X * m.Row3.Y;
			tmp[3] = m.Row3.X * m.Row1.Y;
			tmp[4] = m.Row1.X * m.Row2.Y;
			tmp[5] = m.Row2.X * m.Row1.Y;
			tmp[6] = m.Row0.X * m.Row3.Y;
			tmp[7] = m.Row3.X * m.Row0.Y;
			tmp[8] = m.Row0.X * m.Row2.Y;
			tmp[9] = m.Row2.X * m.Row0.Y;
			tmp[10] = m.Row0.X * m.Row1.Y;
			tmp[11] = m.Row1.X * m.Row0.Y;

			// Calculate second 8 elements (cofactors)
			this.Row2.X = tmp[0] * m.Row1.W + tmp[3] * m.Row2.W + tmp[4] * m.Row3.W;
			this.Row2.X -= tmp[1] * m.Row1.W + tmp[2] * m.Row2.W + tmp[5] * m.Row3.W;
			this.Row2.Y = tmp[1] * m.Row0.W + tmp[6] * m.Row2.W + tmp[9] * m.Row3.W;
			this.Row2.Y -= tmp[0] * m.Row0.W + tmp[7] * m.Row2.W + tmp[8] * m.Row3.W;
			this.Row2.Z = tmp[2] * m.Row0.W + tmp[7] * m.Row1.W + tmp[10] * m.Row3.W;
			this.Row2.Z -= tmp[3] * m.Row0.W + tmp[6] * m.Row1.W + tmp[11] * m.Row3.W;
			this.Row2.W = tmp[5] * m.Row0.W + tmp[8] * m.Row1.W + tmp[11] * m.Row2.W;
			this.Row2.W -= tmp[4] * m.Row0.W + tmp[9] * m.Row1.W + tmp[10] * m.Row2.W;
			this.Row3.X = tmp[2] * m.Row2.Z + tmp[5] * m.Row3.Z + tmp[1] * m.Row1.Z;
			this.Row3.X -= tmp[4] * m.Row3.Z + tmp[0] * m.Row1.Z + tmp[3] * m.Row2.Z;
			this.Row3.Y = tmp[8] * m.Row3.Z + tmp[0] * m.Row0.Z + tmp[7] * m.Row2.Z;
			this.Row3.Y -= tmp[6] * m.Row2.Z + tmp[9] * m.Row3.Z + tmp[1] * m.Row0.Z;
			this.Row3.Z = tmp[6] * m.Row1.Z + tmp[11] * m.Row3.Z + tmp[3] * m.Row0.Z;
			this.Row3.Z -= tmp[10] * m.Row3.Z + tmp[2] * m.Row0.Z + tmp[7] * m.Row1.Z;
			this.Row3.W = tmp[10] * m.Row2.Z + tmp[4] * m.Row0.Z + tmp[9] * m.Row1.Z;
			this.Row3.W -= tmp[8] * m.Row1.Z + tmp[11] * m.Row2.Z + tmp[5] * m.Row0.Z;


			// Divide the cofactor-matrix by the determinant
			float idet = 1.0f / det;
			this.Row0.X *= idet; this.Row0.Y *= idet; this.Row0.Z *= idet; this.Row0.W *= idet;
			this.Row1.X *= idet; this.Row1.Y *= idet; this.Row1.Z *= idet; this.Row1.W *= idet;
			this.Row2.X *= idet; this.Row2.Y *= idet; this.Row2.Z *= idet; this.Row2.W *= idet;
			this.Row3.X *= idet; this.Row3.Y *= idet; this.Row3.Z *= idet; this.Row3.W *= idet;
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
				Row0 = { X = m.Row0.X * f, Y = m.Row0.Y * f, Z = m.Row0.Z * f, W = m.Row0.W * f },
				Row1 = { X = m.Row1.X * f, Y = m.Row1.Y * f, Z = m.Row1.Z * f, W = m.Row1.W * f },
				Row2 = { X = m.Row2.X * f, Y = m.Row2.Y * f, Z = m.Row2.Z * f, W = m.Row2.W * f },
				Row3 = { X = m.Row3.X * f, Y = m.Row3.Y * f, Z = m.Row3.Z * f, W = m.Row3.W * f }
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
					X = mm0.Row0.X + mm1.Row0.X,
					Y = mm0.Row0.Y + mm1.Row0.Y,
					Z = mm0.Row0.Z + mm1.Row0.Z,
					W = mm0.Row0.W + mm1.Row0.W
				},
				Row1 =
				{
					X = mm0.Row1.X + mm1.Row1.X,
					Y = mm0.Row1.Y + mm1.Row1.Y,
					Z = mm0.Row1.Z + mm1.Row1.Z,
					W = mm0.Row1.W + mm1.Row1.W
				},
				Row2 =
				{
					X = mm0.Row2.X + mm1.Row2.X,
					Y = mm0.Row2.Y + mm1.Row2.Y,
					Z = mm0.Row2.Z + mm1.Row2.Z,
					W = mm0.Row2.W + mm1.Row2.W
				},
				Row3 =
				{
					X = mm0.Row3.X + mm1.Row3.X,
					Y = mm0.Row3.Y + mm1.Row3.Y,
					Z = mm0.Row3.Z + mm1.Row3.Z,
					W = mm0.Row3.W + mm1.Row3.W
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
					X = l.Row0.X * r.Row0.X + l.Row0.Y * r.Row1.X + l.Row0.Z * r.Row2.X,
					Y = l.Row0.X * r.Row0.Y + l.Row0.Y * r.Row1.Y + l.Row0.Z * r.Row2.Y,
					Z = l.Row0.X * r.Row0.Z + l.Row0.Y * r.Row1.Z + l.Row0.Z * r.Row2.Z,
					W = l.Row0.W
				},
				Row1 =
				{
					X = l.Row1.X * r.Row0.X + l.Row1.Y * r.Row1.X + l.Row1.Z * r.Row2.X,
					Y = l.Row1.X * r.Row0.Y + l.Row1.Y * r.Row1.Y + l.Row1.Z * r.Row2.Y,
					Z = l.Row1.X * r.Row0.Z + l.Row1.Y * r.Row1.Z + l.Row1.Z * r.Row2.Z,
					W = l.Row1.W
				},
				Row2 =
				{
					X = l.Row2.X * r.Row0.X + l.Row2.Y * r.Row1.X + l.Row2.Z * r.Row2.X,
					Y = l.Row2.X * r.Row0.Y + l.Row2.Y * r.Row1.Y + l.Row2.Z * r.Row2.Y,
					Z = l.Row2.X * r.Row0.Z + l.Row2.Y * r.Row1.Z + l.Row2.Z * r.Row2.Z,
					W = l.Row2.W
				},
				Row3 =
				{
					X = l.Row3.X * r.Row0.X + l.Row3.Y * r.Row1.X + l.Row3.Z * r.Row2.X,
					Y = l.Row3.X * r.Row0.Y + l.Row3.Y * r.Row1.Y + l.Row3.Z * r.Row2.Y,
					Z = l.Row3.X * r.Row0.Z + l.Row3.Y * r.Row1.Z + l.Row3.Z * r.Row2.Z,
					W = l.Row3.W
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
			m.Row0.X = l.Row0.X * r.Row0.X + l.Row0.Y * r.Row1.X + l.Row0.Z * r.Row2.X;
			m.Row1.X = l.Row1.X * r.Row0.X + l.Row1.Y * r.Row1.X + l.Row1.Z * r.Row2.X;
			m.Row2.X = l.Row2.X * r.Row0.X + l.Row2.Y * r.Row1.X + l.Row2.Z * r.Row2.X;
			m.Row3.X = l.Row3.X * r.Row0.X + l.Row3.Y * r.Row1.X + l.Row3.Z * r.Row2.X;
			m.Row0.Y = l.Row0.X * r.Row0.Y + l.Row0.Y * r.Row1.Y + l.Row0.Z * r.Row2.Y;
			m.Row1.Y = l.Row1.X * r.Row0.Y + l.Row1.Y * r.Row1.Y + l.Row1.Z * r.Row2.Y;
			m.Row2.Y = l.Row2.X * r.Row0.Y + l.Row2.Y * r.Row1.Y + l.Row2.Z * r.Row2.Y;
			m.Row3.Y = l.Row3.X * r.Row0.Y + l.Row3.Y * r.Row1.Y + l.Row3.Z * r.Row2.Y;
			m.Row0.Z = l.Row0.X * r.Row0.Z + l.Row0.Y * r.Row1.Z + l.Row0.Z * r.Row2.Z;
			m.Row1.Z = l.Row1.X * r.Row0.Z + l.Row1.Y * r.Row1.Z + l.Row1.Z * r.Row2.Z;
			m.Row2.Z = l.Row2.X * r.Row0.Z + l.Row2.Y * r.Row1.Z + l.Row2.Z * r.Row2.Z;
			m.Row3.Z = l.Row3.X * r.Row0.Z + l.Row3.Y * r.Row1.Z + l.Row3.Z * r.Row2.Z;
			m.Row0.W = l.Row0.X * r.Row0.W + l.Row0.Y * r.Row1.W + l.Row0.Z * r.Row2.W + l.Row0.W;
			m.Row1.W = l.Row1.X * r.Row0.W + l.Row1.Y * r.Row1.W + l.Row1.Z * r.Row2.W + l.Row1.W;
			m.Row2.W = l.Row2.X * r.Row0.W + l.Row2.Y * r.Row1.W + l.Row2.Z * r.Row2.W + l.Row2.W;
			m.Row3.W = l.Row3.X * r.Row0.W + l.Row3.Y * r.Row1.W + l.Row3.Z * r.Row2.W + l.Row3.W;
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
			res.Row0.X = l.Row0.X * r.Row0.X + l.Row0.Y * r.Row1.X + l.Row0.Z * r.Row2.X + l.Row0.W * r.Row3.X;
			res.Row1.X = l.Row1.X * r.Row0.X + l.Row1.Y * r.Row1.X + l.Row1.Z * r.Row2.X + l.Row1.W * r.Row3.X;
			res.Row2.X = l.Row2.X * r.Row0.X + l.Row2.Y * r.Row1.X + l.Row2.Z * r.Row2.X + l.Row2.W * r.Row3.X;
			res.Row3.X = l.Row3.X * r.Row0.X + l.Row3.Y * r.Row1.X + l.Row3.Z * r.Row2.X + l.Row3.W * r.Row3.X;
			res.Row0.Y = l.Row0.X * r.Row0.Y + l.Row0.Y * r.Row1.Y + l.Row0.Z * r.Row2.Y + l.Row0.W * r.Row3.Y;
			res.Row1.Y = l.Row1.X * r.Row0.Y + l.Row1.Y * r.Row1.Y + l.Row1.Z * r.Row2.Y + l.Row1.W * r.Row3.Y;
			res.Row2.Y = l.Row2.X * r.Row0.Y + l.Row2.Y * r.Row1.Y + l.Row2.Z * r.Row2.Y + l.Row2.W * r.Row3.Y;
			res.Row3.Y = l.Row3.X * r.Row0.Y + l.Row3.Y * r.Row1.Y + l.Row3.Z * r.Row2.Y + l.Row3.W * r.Row3.Y;
			res.Row0.Z = l.Row0.X * r.Row0.Z + l.Row0.Y * r.Row1.Z + l.Row0.Z * r.Row2.Z + l.Row0.W * r.Row3.Z;
			res.Row1.Z = l.Row1.X * r.Row0.Z + l.Row1.Y * r.Row1.Z + l.Row1.Z * r.Row2.Z + l.Row1.W * r.Row3.Z;
			res.Row2.Z = l.Row2.X * r.Row0.Z + l.Row2.Y * r.Row1.Z + l.Row2.Z * r.Row2.Z + l.Row2.W * r.Row3.Z;
			res.Row3.Z = l.Row3.X * r.Row0.Z + l.Row3.Y * r.Row1.Z + l.Row3.Z * r.Row2.Z + l.Row3.W * r.Row3.Z;
			res.Row0.W = l.Row0.X * r.Row0.W + l.Row0.Y * r.Row1.W + l.Row0.Z * r.Row2.W + l.Row0.W * r.Row3.W;
			res.Row1.W = l.Row1.X * r.Row0.W + l.Row1.Y * r.Row1.W + l.Row1.Z * r.Row2.W + l.Row1.W * r.Row3.W;
			res.Row2.W = l.Row2.X * r.Row0.W + l.Row2.Y * r.Row1.W + l.Row2.Z * r.Row2.W + l.Row2.W * r.Row3.W;
			res.Row3.W = l.Row3.X * r.Row0.W + l.Row3.Y * r.Row1.W + l.Row3.Z * r.Row2.W + l.Row3.W * r.Row3.W;
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
			return new Vector4(v.X * m.Row0.X + v.Y * m.Row0.Y + v.Z * m.Row0.Z + v.W * m.Row0.W,
							  v.X * m.Row1.X + v.Y * m.Row1.Y + v.Z * m.Row1.Z + v.W * m.Row1.W,
							  v.X * m.Row2.X + v.Y * m.Row2.Y + v.Z * m.Row2.Z + v.W * m.Row2.W,
							  v.X * m.Row3.X + v.Y * m.Row3.Y + v.Z * m.Row3.Z + v.W * m.Row3.W);
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
			return new Vector4(v.X * m.Row0.X + v.Y * m.Row1.X + v.Z * m.Row2.X + v.W * m.Row3.X,
							  v.X * m.Row0.Y + v.Y * m.Row1.Y + v.Z * m.Row2.Y + v.W * m.Row3.Y,
							  v.X * m.Row0.Z + v.Y * m.Row1.Z + v.Z * m.Row2.Z + v.W * m.Row3.Z,
							  v.X * m.Row0.W + v.Y * m.Row1.W + v.Z * m.Row2.W + v.W * m.Row3.W);
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
				X = this.Row0.X * b.X + this.Row0.Y * b.Y + this.Row0.Z * b.Z,
				Y = this.Row1.X * b.X + this.Row1.Y * b.Y + this.Row1.Z * b.Z,
				Z = this.Row2.X * b.X + this.Row2.Y * b.Y + this.Row2.Z * b.Z
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
					this.Row0.X = v.X;
					this.Row1.X = v.Y;
					this.Row2.X = v.Z;
					break;
				case 1:
					this.Row0.Y = v.X;
					this.Row1.Y = v.Y;
					this.Row2.Y = v.Z;
					break;
				case 2:
					this.Row0.Z = v.X;
					this.Row1.Z = v.Y;
					this.Row2.Z = v.Z;
					break;
				case 3:
					this.Row0.W = v.X;
					this.Row1.W = v.Y;
					this.Row2.W = v.Z;
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
					this.Row0.X = v.X;
					this.Row1.X = v.Y;
					this.Row2.X = v.Z;
					this.Row3.X = v.W;
					break;
				case 1:
					this.Row0.Y = v.X;
					this.Row1.Y = v.Y;
					this.Row2.Y = v.Z;
					this.Row3.Y = v.W;
					break;
				case 2:
					this.Row0.Z = v.X;
					this.Row1.Z = v.Y;
					this.Row2.Z = v.Z;
					this.Row3.Z = v.W;
					break;
				case 3:
					this.Row0.W = v.X;
					this.Row1.W = v.Y;
					this.Row2.W = v.Z;
					this.Row3.W = v.W;
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
					return new Vector3(this.Row0.X, this.Row1.X, this.Row2.X);
				case 1:
					return new Vector3(this.Row0.Y, this.Row1.Y, this.Row2.Y);
				case 2:
					return new Vector3(this.Row0.Z, this.Row1.Z, this.Row2.Z);
				case 3:
					return new Vector3(this.Row0.W, this.Row1.W, this.Row2.W);
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
					return new Vector4(this.Row0.X, this.Row1.X, this.Row2.X, this.Row3.X);
				case 1:
					return new Vector4(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.Y);
				case 2:
					return new Vector4(this.Row0.Z, this.Row1.Z, this.Row2.Z, this.Row3.Z);
				case 3:
					return new Vector4(this.Row0.W, this.Row1.W, this.Row2.W, this.Row3.W);
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
		public static bool IsEquivalent(ref  Matrix44 m0, ref Matrix44 m1, float e = MathHelpers.ZeroTolerance)
		{
			return
			(
				(Math.Abs(m0.Row0.X - m1.Row0.X) <= e) && (Math.Abs(m0.Row0.Y - m1.Row0.Y) <= e) && (Math.Abs(m0.Row0.Z - m1.Row0.Z) <= e) && (Math.Abs(m0.Row0.W - m1.Row0.W) <= e) &&
				(Math.Abs(m0.Row1.X - m1.Row1.X) <= e) && (Math.Abs(m0.Row1.Y - m1.Row1.Y) <= e) && (Math.Abs(m0.Row1.Z - m1.Row1.Z) <= e) && (Math.Abs(m0.Row1.W - m1.Row1.W) <= e) &&
				(Math.Abs(m0.Row2.X - m1.Row2.X) <= e) && (Math.Abs(m0.Row2.Y - m1.Row2.Y) <= e) && (Math.Abs(m0.Row2.Z - m1.Row2.Z) <= e) && (Math.Abs(m0.Row2.W - m1.Row2.W) <= e) &&
				(Math.Abs(m0.Row3.X - m1.Row3.X) <= e) && (Math.Abs(m0.Row3.Y - m1.Row3.Y) <= e) && (Math.Abs(m0.Row3.Z - m1.Row3.Z) <= e) && (Math.Abs(m0.Row3.W - m1.Row3.W) <= e)
			);
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
			if (ReferenceEquals(null, obj)) return false;
			return obj is Matrix44 && Equals((Matrix44)obj);
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
			return Matrix44.IsEquivalent(ref this, ref other);
		}
		#endregion
		#region Enumeration
		/// <summary>
		/// </summary>
		/// <returns></returns>
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