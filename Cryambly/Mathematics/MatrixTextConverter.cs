using System;
using System.Text;

namespace CryCil
{
	/// <summary>
	/// Defines functions for converting matrices into text.
	/// </summary>
	public static class MatrixTextConverter
	{
		/// <summary>
		/// Creates text representation of the matrix.
		/// </summary>
		/// <typeparam name="MatrixType">Type of the marix.</typeparam>
		/// <param name="matrix">        Matrix to convert into text.</param>
		/// <param name="formatProvider">
		/// Object that provides culture-specific information to use in formatting.
		/// </param>
		/// <returns>
		/// Text representation of the matrix where all elements are listed in a line using default format
		/// for <see cref="Single"/> numbers and culture-specific information supplied by
		/// <paramref name="formatProvider"/>.
		/// </returns>
		public static string ToString<MatrixType>(IMatrix<MatrixType> matrix, IFormatProvider formatProvider)
		{
			return ToString(matrix, new MatrixTextFormat { FormatProvider = formatProvider });
		}
		/// <summary>
		/// Creates text representation of the matrix.
		/// </summary>
		/// <remarks>
		/// <para><paramref name="format"/> can consist of two parts:</para>
		/// <para>1) Matrix format specification: this affects how the matrix is formatted.</para>
		/// <para>
		/// 2) Number format specification, this affects how numbers in this matrix are formatted.
		/// </para>
		/// <para>
		/// Both of these parts are optional, and when both of them are present, they should separated by
		/// '|' (vertical slash) symbol.
		/// </para>
		/// <para></para>
		/// <para>
		/// Matrix format specification is a string consisting of letters only the following of which can
		/// be recognized:
		/// </para>
		/// <para>1) 'r' - specifies that each row of the matrix must be on a separate line.</para>
		/// <para>
		/// 2) 'o' - when followed up by one the following symbols: '(', ')', '[', ']', '{', '}', specifies
		///    which of those symbols to use to enclose the entire matrix representation.
		/// </para>
		/// <para>3) 'i' - same as 'o' but for enclosing rows.</para>
		/// <para>
		/// 4) 'c' - specifies that all elements should have commas between them in addition to spaces.
		/// </para>
		/// <para>5) 's' - specifies that all rows should have semicolons between them.</para>
		/// <para></para>
		/// <para>
		/// Refer to https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx for documentation on
		/// formatting single-precision numbers.
		/// </para>
		/// <para></para>
		/// <para>
		/// When only one part is provided, put '|' either at the left side of it or right one to specify
		/// whether that part specifies a number or matrix format respectively. If '|' is not in the
		/// string, default format will be used.
		/// </para>
		/// </remarks>
		/// <example>
		/// <code>
		/// Matrix44 iden = Matrix44.Identity;
		/// string text = iden.ToString("o(i{s|f0");
		/// // Text stored in text variable:
		/// 
		/// // ({1 0 0 0};{0 1 0 0};{0 0 1 0};{0 0 0 1})
		/// text = iden.ToString("ri]sc|f0");
		/// // Text stored in text variable:
		/// 
		/// // [1, 0, 0, 0];
		/// // [0, 1, 0, 0];
		/// // [0, 0, 1, 0];
		/// // [0, 0, 0, 1]
		/// </code>
		/// </example>
		/// <typeparam name="MatrixType">Type of the marix.</typeparam>
		/// <param name="matrix">        Matrix to convert into text.</param>
		/// <param name="format">        
		/// A string that describes a format of the matrix. See Remarks section for details.
		/// </param>
		/// <param name="formatProvider">
		/// Object that provides culture-specific information on how to create text representations of
		/// numbers.
		/// </param>
		/// <returns>Text representation specified by given arguments.</returns>
		public static string ToString<MatrixType>(IMatrix<MatrixType> matrix, string format, IFormatProvider formatProvider)
		{
			if (!format.Contains("|"))
			{
				return ToString(matrix, formatProvider);
			}
			string[] parts = format.Split(new[] { '|' });
			int outerEncloserIndex = parts[0].IndexOf('o');
			int innerEncloserIndex = parts[0].IndexOf('i');
			MatrixTextFormat formatObject = new MatrixTextFormat
			{
				CommaElementDelimitation = parts[0].Contains("c"),
				SemicolonRowDelimitation = parts[0].Contains("s"),
				SeparateIntoRows = parts[0].Contains("r"),
				OuterEnclosers = (outerEncloserIndex != -1)
					? ExtractEnclosers(parts[0], outerEncloserIndex)
					: null,
				RowEnclosers = (innerEncloserIndex != -1)
					? ExtractEnclosers(parts[0], innerEncloserIndex)
					: null,
				NumberFormat = parts[1],
				FormatProvider = formatProvider
			};
			return ToString(matrix, formatObject);
		}
		private static Tuple<char, char> ExtractEnclosers(string formatString, int formatSymbolIndex)
		{
			char encloserSymbol = formatString[formatSymbolIndex + 1];
			switch (encloserSymbol)
			{
				case '(':
				case ')':
					return new Tuple<char, char>('(', ')');
				case '{':
				case '}':
					return new Tuple<char, char>('{', '}');
				case '[':
				case ']':
					return new Tuple<char, char>('[', ']');
				default:
					return null;
			}
		}
		/// <summary>
		/// Creates text representation of the matrix.
		/// </summary>
		/// <typeparam name="MatrixType">Type of the marix.</typeparam>
		/// <param name="matrix">Matrix to convert into text.</param>
		/// <param name="format">Object that provides information of how to format the text.</param>
		/// <returns>A formatted text.</returns>
		public static string ToString<MatrixType>(IMatrix<MatrixType> matrix, MatrixTextFormat format)
		{
			float[,] array = matrix.Array2D;

			StringBuilder builder = new StringBuilder(200);
			if (format.OuterEnclosers != null)
			{
				builder.Append(format.OuterEnclosers.Item1);
			}
			for (int i = 0; i < array.GetLength(0); i++)
			{
				if (format.RowEnclosers != null)
				{
					builder.Append(format.RowEnclosers.Item1);
				}
				for (int j = 0; j < array.GetLength(1); j++)
				{
					builder.AppendFormat(format.FormatProvider, format.NumberFormat, array[i, j]);
					if (j != 3)
					{
						if (format.CommaElementDelimitation)
						{
							builder.Append(',');
						}
						builder.Append(' ');
					}
				}
				if (format.RowEnclosers != null)
				{
					builder.Append(format.RowEnclosers.Item2);
				}
				if (i != 3)
				{
					if (format.SemicolonRowDelimitation)
					{
						builder.Append(';');
					}
					if (format.SeparateIntoRows)
					{
						builder.Append(Environment.NewLine);
					}
					else
					{
						builder.Append(' ');
					}
				}
			}
			if (format.OuterEnclosers != null)
			{
				builder.Append(format.OuterEnclosers.Item2);
			}
			return builder.ToString();
		}
	}
}