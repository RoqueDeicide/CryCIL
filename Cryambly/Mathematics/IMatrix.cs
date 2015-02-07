using System;
using System.Collections.Generic;

namespace CryCil
{
	/// <summary>
	/// Base interface for all matrices.
	/// </summary>
	public interface IMatrix<MatrixType> : IEnumerable<float>, IEquatable<MatrixType>
	{
		/// <summary>
		/// Gets actual determinant.
		/// </summary>
		float Determinant { get; }
		/// <summary>
		/// Gets a 2D array of elements of this matrix.
		/// </summary>
		float[,] Array2D { get; }
		/// <summary>
		/// Gives access to specific element of this matrix.
		/// </summary>
		/// <param name="row">   Zero-based index of the row.</param>
		/// <param name="column">Zero-based index of the column.</param>
		float this[int row, int column] { get; set; }
		/// <summary>
		/// Sets elements of this matrix to values of identity matrix.
		/// </summary>
		void SetIdentity();
		/// <summary>
		/// Swaps rows and columns of this matrix.
		/// </summary>
		void Transpose();
		/// <summary>
		/// Calculates a real inversion of this matrix.
		/// </summary>
		/// <remarks>
		/// Uses Cramer's Rule which is faster (branchless) but numerically more unstable
		/// than other methods like Gaussian Elimination.
		/// </remarks>
		void Invert();
	}
}