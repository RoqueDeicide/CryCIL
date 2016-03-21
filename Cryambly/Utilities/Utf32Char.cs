using System;
using System.Linq;

namespace CryCil.Utilities
{
	/// <summary>
	/// Encapsulates UTF-32 encoded character.
	/// </summary>
	public struct Utf32Char
	{
		private readonly int value;
		/// <summary>
		/// Gets a string that contains a surrogate pair that represents the character that is encoded using
		/// UTF-32 format.
		/// </summary>
		public string SurrogatePair => char.ConvertFromUtf32(this.value);
		/// <summary>
		/// Gets the low surrogate character of this UTF-32 encoded one.
		/// </summary>
		public char Character => (char)this.value;
		/// <summary>
		/// Initializes new object of this type.
		/// </summary>
		/// <param name="value">A character that is encoded using UTF-32 format.</param>
		public unsafe Utf32Char(uint value)
		{
			this.value = *(int*)&value;
		}
		/// <summary>
		/// Initializes new object of this type.
		/// </summary>
		/// <param name="value">A character that is encoded using UTF-32 format.</param>
		public Utf32Char(int value)
		{
			this.value = value;
		}
	}
}