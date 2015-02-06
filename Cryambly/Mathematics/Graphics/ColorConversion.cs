using System;
using CryCil.Geometry;

namespace CryCil.Graphics
{
	/// <summary>
	/// Defines functions that convert colors from one format to others.
	/// </summary>
	public static class ColorConversion
	{
		/// <summary>
		/// Defines functions that convert colors from RGB to other formats.
		/// </summary>
		public static class RGB
		{
			/// <summary>
			/// Converts the color from RGB format to mCIE.
			/// </summary>
			/// <remarks>
			/// <code>
			/// // test gray.
			/// out.r = 10.0f/31.0f;		// 1/3 = 10/30      Red range     0..30, 31 unused
			/// out.g = 21.0f/63.0f;		// 1/3 = 21/63      Green range   0..63
			/// </code>
			/// </remarks>
			/// <param name="color">Color to convert.</param>
			/// <returns>A new color that is more resistant to DXT compression.</returns>
			public static ColorSingle mCIE(ColorSingle color)
			{
				// to get gray chrominance for dark colors.
				float r = color.R + 0.000001f;
				float g = color.G + 0.000001f;
				float b = color.B + 0.000001f;

				float rgbSum = r + g + b;

				float fInv = 1.0f / rgbSum;

				float rNorm = 3 * 10.0f / 31.0f * r * fInv;
				float gNorm = 3 * 21.0f / 63.0f * g * fInv;
				float scale = rgbSum / 3.0f;

				rNorm = Math.Max(0.0f, Math.Min(1.0f, rNorm));
				gNorm = Math.Max(0.0f, Math.Min(1.0f, gNorm));

				return new ColorSingle(rNorm, gNorm, scale);
			}
			/// <summary>
			/// Converts the color from RGB format to mCIE.
			/// </summary>
			/// <param name="color">Color to convert.</param>
			/// <returns>
			/// A new color that is better suited for display on a monitor.
			/// </returns>
			public static unsafe ColorSingle sRGB(ColorSingle color)
			{
				ColorSingle res = new ColorSingle();

				float* s = (float*)&color;
				float* c = (float*)&res;
				for (int i = 0; i < 3; i++)
				{
					if (*s < 0.0031308f)
					{
						*c = 12.92f * *s;
					}
					else
					{
						*c = (float)(1.055f * Math.Pow(*s, 1.0f / 2.4f) - 0.055f);
					}
					c += i * 4;
					s += i * 4;
				}
				return res;
			}
			/// <summary>
			/// Converts given RGB color to HSV format.
			/// </summary>
			/// <param name="color">Color to convert.</param>
			/// <returns><see cref="Vector3"/> object where X = H, Y = S, Z = V.</returns>
			public static Vector3 HSV(ColorSingle color)
			{
				Vector3 hsv = new Vector3();
				if ((color.B > color.G) && (color.B > color.R))
				{
					hsv.Z = color.B;
					if (hsv.Z > MathHelpers.ZeroTolerance)
					{
						float min = color.R < color.G ? color.R : color.G;
						float delta = hsv.Z - min;
						if (delta > MathHelpers.ZeroTolerance)
						{
							hsv.Y = delta / hsv.Z;
							hsv.X = (240.0f / 360.0f) + (color.R - color.G) / delta * (60.0f / 360.0f);
						}
						else
						{
							hsv.Y = 0.0f;
							hsv.X = (240.0f / 360.0f) + (color.R - color.G) * (60.0f / 360.0f);
						}
						if (hsv.X < 0.0f) hsv.X += 1.0f;
					}
					else
					{
						hsv.Y = 0.0f;
						hsv.X = 0.0f;
					}
				}
				else if (color.G > color.R)
				{
					hsv.Z = color.G;
					if (hsv.Z > MathHelpers.ZeroTolerance)
					{
						float min = color.R < color.B ? color.R : color.B;
						float delta = hsv.Z - min;
						if (delta > MathHelpers.ZeroTolerance)
						{
							hsv.Y = delta / hsv.Z;
							hsv.X = (120.0f / 360.0f) + (color.B - color.R) / delta * (60.0f / 360.0f);
						}
						else
						{
							hsv.Y = 0.0f;
							hsv.X = (120.0f / 360.0f) + (color.B - color.R) * (60.0f / 360.0f);
						}
						if (hsv.X < 0.0f) hsv.X += 1.0f;
					}
					else
					{
						hsv.Y = 0.0f;
						hsv.X = 0.0f;
					}
				}
				else
				{
					hsv.Z = color.R;
					if (hsv.Z > MathHelpers.ZeroTolerance)
					{
						float min = color.G < color.B ? color.G : color.B;
						float delta = hsv.Z - min;
						if (delta > MathHelpers.ZeroTolerance)
						{
							hsv.Y = delta / hsv.Z;
							hsv.X = (color.G - color.B) / delta * (60.0f / 360.0f);
						}
						else
						{
							hsv.Y = 0.0f;
							hsv.X = (color.G - color.B) * (60.0f / 360.0f);
						}
						if (hsv.X < 0.0f) hsv.X += 1.0f;
					}
					else
					{
						hsv.Y = 0.0f;
						hsv.X = 0.0f;
					}
				}
				return hsv;
			}
		}
	}
}