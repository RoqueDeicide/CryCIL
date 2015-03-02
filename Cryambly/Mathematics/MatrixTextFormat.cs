using System;

namespace CryCil
{
	/// <summary>
	/// Encapsulates description of the text format of the matrix.
	/// </summary>
	public class MatrixTextFormat
	{
		/// <summary>
		/// <see cref="String"/> that describes format that is used to format matrix elements.
		/// </summary>
		public string NumberFormat;
		/// <summary>
		/// Provider of culture-specific information to use when formatting numbers.
		/// </summary>
		public IFormatProvider FormatProvider;
		/// <summary>
		/// Optional combination of characters that enclose the entire matrix text representation.
		/// </summary>
		public Tuple<char, char> OuterEnclosers;
		/// <summary>
		/// Optional combination of characters that enclose the text representations of matrix rows.
		/// </summary>
		public Tuple<char, char> RowEnclosers;
		/// <summary>
		/// Indicates if comma must be put between matrix elements.
		/// </summary>
		public bool CommaElementDelimitation;
		/// <summary>
		/// Indicates if semicolon must be put between matrix rows.
		/// </summary>
		public bool SemicolonRowDelimitation;
		/// <summary>
		/// Indicates whether matrix rows must located at different rows in a resultant text.
		/// </summary>
		public bool SeparateIntoRows;
		/// <summary>
		/// Creates a default <see cref="MatrixTextFormat"/> object.
		/// </summary>
		public MatrixTextFormat()
		{
			this.NumberFormat = "";
			this.FormatProvider = Defaults.CultureToStringOnly;
			this.OuterEnclosers = null;
			this.RowEnclosers = null;
			this.CommaElementDelimitation = false;
			this.SemicolonRowDelimitation = false;
		}
	}
}