using System;
using CryCil.Graphics;

namespace CryCil
{
	public static partial class Interpolation
	{
		public static partial class Linear
		{
			/// <summary>
			/// Applies linear interpolation to the color.
			/// </summary>
			/// <param name="result">   Result of interpolation.</param>
			/// <param name="first">    First color.</param>
			/// <param name="second">   Second color.</param>
			/// <param name="parameter">
			/// Scalar that describes difference between resultant color value and first color relative to
			/// second color.
			/// </param>
			public static void Apply(out ColorByte result, ColorByte first, ColorByte second, float parameter)
			{
				result = new ColorByte
				{
					Red = (byte)(first.Red + (second.Red - first.Red) * parameter),
					Green = (byte)(first.Green + (second.Green - first.Green) * parameter),
					Blue = (byte)(first.Blue + (second.Blue - first.Blue) * parameter),
					Alpha = (byte)(first.Alpha + (second.Alpha - first.Alpha) * parameter)
				};
			}
			/// <summary>
			/// Creates a new color that is a result of interpolation.
			/// </summary>
			/// <param name="first">    First color.</param>
			/// <param name="second">   Second color.</param>
			/// <param name="parameter">
			/// Scalar that describes difference between resultant color value and first color relative to
			/// second color.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static ColorByte Create(ColorByte first, ColorByte second, float parameter)
			{
				return new ColorByte
				{
					Red = (byte)(first.Red + (second.Red - first.Red) * parameter),
					Green = (byte)(first.Green + (second.Green - first.Green) * parameter),
					Blue = (byte)(first.Blue + (second.Blue - first.Blue) * parameter),
					Alpha = (byte)(first.Alpha + (second.Alpha - first.Alpha) * parameter)
				};
			}
			/// <summary>
			/// Applies linear interpolation to the color.
			/// </summary>
			/// <param name="result">   Result of interpolation.</param>
			/// <param name="first">    First color.</param>
			/// <param name="second">   Second color.</param>
			/// <param name="parameter">
			/// Scalar that describes difference between resultant color value and first color relative to
			/// second color.
			/// </param>
			public static void Apply(out ColorSingle result, ColorSingle first, ColorSingle second, float parameter)
			{
				result = new ColorSingle
				{
					R = first.R + (second.R - first.R) * parameter,
					G = first.G + (second.G - first.G) * parameter,
					B = first.B + (second.B - first.B) * parameter,
					A = first.A + (second.A - first.A) * parameter
				};
			}
			/// <summary>
			/// Creates a new color that is a result of interpolation.
			/// </summary>
			/// <param name="first">    First color.</param>
			/// <param name="second">   Second color.</param>
			/// <param name="parameter">
			/// Scalar that describes difference between resultant color value and first color relative to
			/// second color.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static ColorSingle Create(ColorSingle first, ColorSingle second, float parameter)
			{
				return new ColorSingle
				{
					R = first.R + (second.R - first.R) * parameter,
					G = first.G + (second.G - first.G) * parameter,
					B = first.B + (second.B - first.B) * parameter,
					A = first.A + (second.A - first.A) * parameter
				};
			}
		}
	}
}