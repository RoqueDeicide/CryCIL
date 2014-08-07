using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CryEngine.Mathematics.MemoryMapping;

namespace CryEngine
{
	/// <summary>
	/// Encapsulates 32-bit definition of color.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct Color32 : IEquatable<Color32>, IEnumerable<byte>
	{
		/// <summary>
		/// Number of bytes each instance of this structure consists of.
		/// </summary>
		public static readonly ulong ByteCount = (ulong)Marshal.SizeOf(typeof(Color32));
		/// <summary>
		/// All 4 bytes of this structure.
		/// </summary>
		[FieldOffset(0)]
		public Bytes4 Bytes;
		/// <summary>
		/// Red component of the color.
		/// </summary>
		[FieldOffset(0)]
		public byte Red;
		/// <summary>
		/// Green component of the color.
		/// </summary>
		[FieldOffset(1)]
		public byte Green;
		/// <summary>
		/// Blue component of the color.
		/// </summary>
		[FieldOffset(2)]
		public byte Blue;
		/// <summary>
		/// Alpha component of the color.
		/// </summary>
		[FieldOffset(3)]
		public byte Alpha;
		/// <summary>
		/// Determines whether this color is equal to another.
		/// </summary>
		/// <param name="other">Another color.</param>
		/// <returns>True, if they are equal.</returns>
		public bool Equals(Color32 other)
		{
			return
				this.Red == other.Red &&
				this.Green == other.Green &&
				this.Blue == other.Blue &&
				this.Alpha == other.Alpha;
		}
		/// <summary>
		/// Determines whether this color is equal to another object.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if <paramref name="obj" /> is <see cref="Color32" /> and they are equal.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Color32 && Equals((Color32)obj);
		}
		/// <summary>
		/// Calculates hash code of this color object.
		/// </summary>
		/// <returns>Hash code of this color object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				// ReSharper disable NonReadonlyFieldInGetHashCode
				int hashCode = this.Red.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Green.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Blue.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Alpha.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode
				return hashCode;
			}
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="Color32" /> are equal.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are equal.</returns>
		public static bool operator ==(Color32 left, Color32 right)
		{
			return left.Bytes.SignedInt == right.Bytes.SignedInt;
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="Color32" /> are not equal.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are not equal.</returns>
		public static bool operator !=(Color32 left, Color32 right)
		{
			return
				left.Red != right.Red ||
				left.Green != right.Green ||
				left.Blue != right.Blue ||
				left.Alpha != right.Alpha;
		}
		/// <summary>
		/// Converts given color object to structure that allows easy byte manipulation and simple
		/// type conversion.
		/// </summary>
		/// <param name="color">Color to convert.</param>
		/// <returns>
		/// Instance of <see cref="Bytes4" /> type with each byte representing a color component in
		/// the order - RGBA.
		/// </returns>
		public static explicit operator Bytes4(Color32 color)
		{
			return color.Bytes;
		}
		/// <summary>
		/// Creates text representation of this color object.
		/// </summary>
		/// <returns>Object of type <see cref="String" /> that represents this color.</returns>
		public override string ToString()
		{
			return String.Format("[{0} {1} {2} {3}]", this.Red, this.Green, this.Blue, this.Alpha);
		}
		/// <summary>
		/// Converts text representation of the color to type <see cref="Color32" />.
		/// </summary>
		/// <param name="text">
		/// Object of type <see cref="String" /> that is supposed to represent a color.
		/// </param>
		/// <returns>
		/// Object of type <see cref="Color32" /> that is represented by given <paramref name="text" />.
		/// </returns>
		public static Color32 Parse(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("text", "Attempt to parse empty string as Color32.");
			}
			if (text[0] != '[')
			{
				throw new ArgumentException("Attempt to parse text that is not a recognizable Color32 representation as object of that type.");
			}
			string[] componentStrings = text.Substring(1, text.Length - 2).Split(new[] { ' ' });
			try
			{
				Color32 color = new Color32
				{
					Red = Byte.Parse(componentStrings[0]),
					Green = Byte.Parse(componentStrings[1]),
					Blue = Byte.Parse(componentStrings[2]),
					Alpha = Byte.Parse(componentStrings[3])
				};
				return color;
			}
			catch (Exception)
			{
				throw new ArgumentException("Attempt to parse text that is not a recognizable Color32 representation as object of that type.");
			}
		}
		/// <summary>
		/// Attempts to parse given text as object of type <see cref="Color32" />.
		/// </summary>
		/// <param name="text">
		/// Object of type <see cref="String" /> that might be a representation of type <see
		/// cref="Color32" />.
		/// </param>
		/// <param name="color">
		/// If conversion is successful this object will contain the result.
		/// </param>
		/// <returns>
		/// True, if given text is a recognizable representation of type <see cref="Color32" />.
		/// </returns>
		public static bool TryParse(string text, out Color32 color)
		{
			try
			{
				color = Color32.Parse(text);
				return true;
			}
			catch (Exception)
			{
				color = new Color32();
				return false;
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		/// <summary>
		/// Enumerates four components of this color.
		/// </summary>
		/// <returns>Enumerator of color components.</returns>
		public IEnumerator<byte> GetEnumerator()
		{
			yield return this.Red;
			yield return this.Green;
			yield return this.Blue;
			yield return this.Alpha;
		}
	}
}