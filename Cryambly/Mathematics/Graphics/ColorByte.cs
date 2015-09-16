using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CryCil.MemoryMapping;

namespace CryCil.Graphics
{
	/// <summary>
	/// Encapsulates 32-bit definition of color.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct ColorByte : IEquatable<ColorByte>, IEnumerable<byte>
	{
		/// <summary>
		/// Number of bytes each instance of this structure consists of.
		/// </summary>
		public static readonly ulong ByteCount = (ulong)Marshal.SizeOf(typeof(ColorByte));
		/// <summary>
		/// All 4 bytes of this structure.
		/// </summary>
		[FieldOffset(0)] public Bytes4 Bytes;
		/// <summary>
		/// Red component of the color.
		/// </summary>
		[FieldOffset(0)] public byte Red;
		/// <summary>
		/// Green component of the color.
		/// </summary>
		[FieldOffset(1)] public byte Green;
		/// <summary>
		/// Blue component of the color.
		/// </summary>
		[FieldOffset(2)] public byte Blue;
		/// <summary>
		/// Alpha component of the color.
		/// </summary>
		[FieldOffset(3)] public byte Alpha;
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="red">  Red component of the color.</param>
		/// <param name="green">Green component of the color.</param>
		/// <param name="blue"> Blue component of the color.</param>
		/// <param name="alpha">Alpha component of the color.</param>
		public ColorByte(byte red, byte green, byte blue, byte alpha)
			: this()
		{
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
			this.Alpha = alpha;
		}
		/// <summary>
		/// Creates new color from its 32-bit representation.
		/// </summary>
		/// <param name="colors">32-bit representation of the color.</param>
		public ColorByte(uint colors)
			: this()
		{
			this.Bytes.UnsignedInt = colors;
		}
		/// <summary>
		/// Modifies alpha component of this color.
		/// </summary>
		/// <param name="value">New value to use for the alpha component.</param>
		/// <returns>Modified color.</returns>
		public ColorByte ModifyAlpha(byte value)
		{
			this.Alpha = value;
			return this;
		}
		/// <summary>
		/// Determines whether this color is equal to another.
		/// </summary>
		/// <param name="other">Another color.</param>
		/// <returns>True, if they are equal.</returns>
		public bool Equals(ColorByte other)
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
		/// True, if <paramref name="obj"/> is <see cref="ColorByte"/> and they are equal.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ColorByte && this.Equals((ColorByte)obj);
		}
		/// <summary>
		/// Calculates hash code of this color object.
		/// </summary>
		/// <returns>Hash code of this color object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.Red.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Green.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Blue.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Alpha.GetHashCode();

				return hashCode;
			}
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="ColorByte"/> are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are equal.</returns>
		public static bool operator ==(ColorByte left, ColorByte right)
		{
			return left.Bytes.SignedInt == right.Bytes.SignedInt;
		}
		/// <summary>
		/// Determines whether two instances of type <see cref="ColorByte"/> are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if operands are not equal.</returns>
		public static bool operator !=(ColorByte left, ColorByte right)
		{
			return
				left.Red != right.Red ||
				left.Green != right.Green ||
				left.Blue != right.Blue ||
				left.Alpha != right.Alpha;
		}
		/// <summary>
		/// Converts given color object to structure that allows easy byte manipulation and simple type
		/// conversion.
		/// </summary>
		/// <param name="color">Color to convert.</param>
		/// <returns>
		/// Instance of <see cref="Bytes4"/> type with each byte representing a color component in the
		/// order - RGBA.
		/// </returns>
		public static explicit operator Bytes4(ColorByte color)
		{
			return color.Bytes;
		}
		/// <summary>
		/// Converts given object of type <see cref="ColorByte"/> into object of type
		/// <see cref="ColorSingle"/>.
		/// </summary>
		/// <param name="color">Color to convert to floating-point representation.</param>
		/// <returns>
		/// A new color that is the same as given one, but in a floating point number format.
		/// </returns>
		public static implicit operator ColorSingle(ColorByte color)
		{
			return new ColorSingle(color.Red / 255.0f, color.Green / 255.0f, color.Blue / 255.0f, color.Alpha / 255.0f);
		}
		/// <summary>
		/// Creates text representation of this color object.
		/// </summary>
		/// <returns>Object of type <see cref="String"/> that represents this color.</returns>
		public override string ToString()
		{
			return string.Format("[{0} {1} {2} {3}]", this.Red, this.Green, this.Blue, this.Alpha);
		}
		/// <summary>
		/// Converts text representation of the color to type <see cref="ColorByte"/> .
		/// </summary>
		/// <param name="text">
		/// Object of type <see cref="String"/> that is supposed to represent a color.
		/// </param>
		/// <returns>
		/// Object of type <see cref="ColorByte"/> that is represented by given <paramref name="text"/> .
		/// </returns>
		public static ColorByte Parse(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("text", "Attempt to parse empty string as Color32.");
			}
			if (text[0] != '[')
			{
				throw new ArgumentException(
					"Attempt to parse text that is not a recognizable Color32 representation as object of that type.");
			}
			string[] componentStrings = text.Substring(1, text.Length - 2).Split(' ');
			try
			{
				ColorByte color = new ColorByte
				{
					Red = byte.Parse(componentStrings[0]),
					Green = byte.Parse(componentStrings[1]),
					Blue = byte.Parse(componentStrings[2]),
					Alpha = byte.Parse(componentStrings[3])
				};
				return color;
			}
			catch (Exception)
			{
				throw new ArgumentException(
					"Attempt to parse text that is not a recognizable Color32 representation as object of that type.");
			}
		}
		/// <summary>
		/// Attempts to parse given text as object of type <see cref="ColorByte"/> .
		/// </summary>
		/// <param name="text"> 
		/// Object of type <see cref="String"/> that might be a representation of type
		/// <see cref="ColorByte"/> .
		/// </param>
		/// <param name="color">If conversion is successful this object will contain the result.</param>
		/// <returns>
		/// True, if given text is a recognizable representation of type <see cref="ColorByte"/> .
		/// </returns>
		public static bool TryParse(string text, out ColorByte color)
		{
			try
			{
				color = Parse(text);
				return true;
			}
			catch (Exception)
			{
				color = new ColorByte();
				return false;
			}
		}
		/// <summary>
		/// Creates linear interpolation between two colors.
		/// </summary>
		/// <param name="v1">   First color.</param>
		/// <param name="v2">   Last color.</param>
		/// <param name="value">
		/// Interpolation parameter between 0 and 1 that describes the position of interpolated color in
		/// percentage-like fashion.
		/// </param>
		/// <returns>Interpolated color.</returns>
		public static ColorByte CreateLinearInterpolation(ColorByte v1, ColorByte v2, float value)
		{
			return new ColorByte
			{
				Red = (byte)(v1.Red + (v2.Red - v1.Red) * value),
				Green = (byte)(v1.Green + (v2.Green - v1.Green) * value),
				Blue = (byte)(v1.Blue + (v2.Blue - v1.Blue) * value),
				Alpha = (byte)(v1.Alpha + (v2.Alpha - v1.Alpha) * value)
			};
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