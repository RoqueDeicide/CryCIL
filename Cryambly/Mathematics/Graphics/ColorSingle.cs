using System;
using CryCil.Geometry;

namespace CryCil.Graphics
{
	/// <summary>
	/// Defines a color in terms of its red, green, blue and alpha values.
	/// </summary>
	public struct ColorSingle
	{
		#region Fields
		/// <summary>
		/// The red value of the color.
		/// </summary>
		public float R;
		/// <summary>
		/// The green value of the color.
		/// </summary>
		public float G;
		/// <summary>
		/// The blue value of the color.
		/// </summary>
		public float B;
		/// <summary>
		/// The alpha value of the color.
		/// </summary>
		public float A;
		#endregion
		#region Properties
		/// <summary>
		/// Gets luminance value of this color.
		/// </summary>
		/// <remarks>Luminance formula is: r*0.30f + g*0.59f + b*0.11f.</remarks>
		public float Luminance
		{
			get { return this.R * 0.3f + this.G * 0.59f + this.B * 0.11f; }
		}
		/// <summary>
		/// Provides read/write access to the color component.
		/// </summary>
		/// <param name="index">Zero-based index of the color component to access.</param>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return this.R;
					case 1:
						return this.G;
					case 2:
						return this.B;
					case 3:
						return this.A;
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access color" +
																	   " component other then R, G, B or A.");
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						this.R = value;
						break;
					case 1:
						this.G = value;
						break;
					case 2:
						this.B = value;
						break;
					case 3:
						this.A = value;
						break;
					default:
						throw new ArgumentOutOfRangeException("index", "Attempt to access color" +
																	   " component other then R, G, B or A.");
				}
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Constructs a new color object specifying the red, green, blue and alpha
		/// values.
		/// </summary>
		/// <param name="red">  Red component of the color.</param>
		/// <param name="green">Green component of the color.</param>
		/// <param name="blue"> Blue component of the color.</param>
		/// <param name="alpha">Alpha component of the color.</param>
		public ColorSingle(float red, float green, float blue, float alpha = 1)
			: this()
		{
			this.R = red;
			this.G = green;
			this.B = blue;
			this.A = alpha;
		}
		/// <summary>
		/// Constructs a new color object specifying the red, green, blue and alpha
		/// values.
		/// </summary>
		/// <param name="rgb"><see cref="Vector3"/> that specifies RGB values.</param>
		/// <param name="a">  Alpha component of the color.</param>
		public ColorSingle(Vector3 rgb, float a = 1) : this(rgb.X, rgb.Y, rgb.Z, a) { }
		/// <summary>
		/// Constructs a new gray-scale color object.
		/// </summary>
		/// <param name="brightness">
		/// The brightness of the object, where 0 is black, and 1 is white.
		/// </param>
		public ColorSingle(float brightness) : this(brightness, brightness, brightness) { }
		#endregion
		#region Interface
		/// <summary>
		/// Clamps this color into range.
		/// </summary>
		/// <param name="bottom">Lower level.</param>
		/// <param name="top">   Upper level.</param>
		public void Clamp(float bottom = 0.0f, float top = 1.0f)
		{
			this.R = MathHelpers.Clamp(this.R, bottom, top);
			this.G = MathHelpers.Clamp(this.G, bottom, top);
			this.B = MathHelpers.Clamp(this.B, bottom, top);
			this.A = MathHelpers.Clamp(this.A, bottom, top);
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if objects are of the same type and are equal, otherwise false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is ColorSingle)
				return this == (ColorSingle)obj;

			return false;
		}
		/// <summary>
		/// Creates text representation of this color.
		/// </summary>
		/// <returns>
		/// Text representation of this color using R r G g B b A a format with
		/// capitalized letters being present in the output and lower-case letters being
		/// replaced by corresponding color values.
		/// </returns>
		public override string ToString()
		{
			return String.Format("R {0} G {1} B {2} A {3}", this.R, this.G, this.B, this.A);
		}
		/// <summary>
		/// Gets hash code of this color.
		/// </summary>
		/// <returns>Hash code of this color.</returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + this.R.GetHashCode();
				hash = hash * 29 + this.G.GetHashCode();
				hash = hash * 29 + this.B.GetHashCode();
				hash = hash * 29 + this.A.GetHashCode();

				return hash;
			}
		}
		/// <summary>
		/// Determines whether two colors are exactly the same.
		/// </summary>
		/// <param name="col1">Left operand.</param>
		/// <param name="col2">Right operand.</param>
		/// <returns>True, if objects are equal, otherwise false.</returns>
		public static bool operator ==(ColorSingle col1, ColorSingle col2)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return col1.R == col2.R && col1.G == col2.G && col1.B == col2.B && col1.A == col2.A;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether two colors are not the same.
		/// </summary>
		/// <param name="col1">Left operand.</param>
		/// <param name="col2">Right operand.</param>
		/// <returns>True, if objects are not equal, otherwise false.</returns>
		public static bool operator !=(ColorSingle col1, ColorSingle col2)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return col1.R != col2.R || col1.G != col2.G || col1.B != col2.B || col1.A != col2.A;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Creates a color from vector.
		/// </summary>
		/// <param name="vec">Vector to convert into color.</param>
		/// <returns>R = X; G = Y; B = Z;</returns>
		public static implicit operator ColorSingle(Vector3 vec)
		{
			return new ColorSingle(vec.X, vec.Y, vec.Z);
		}
		/// <summary>
		/// Creates a vector out of color.
		/// </summary>
		/// <param name="clr">Color to convert into vector.</param>
		/// <returns>Vector with RGB as XYZ.</returns>
		public static implicit operator Vector3(ColorSingle clr)
		{
			return new Vector3(clr.R, clr.G, clr.B);
		}
		/// <summary>
		/// Inverts the color.
		/// </summary>
		/// <param name="color">Color to invert.</param>
		/// <returns>
		/// Color where each component is a negated corresponding component of the given
		/// color object.
		/// </returns>
		public static ColorSingle operator -(ColorSingle color)
		{
			return new ColorSingle(-color.R, -color.G, -color.B, -color.A);
		}
		/// <summary>
		/// Creates a new color that is more resistant to DXT compression.
		/// </summary>
		/// <remarks>
		/// <code>
		/// // test gray.
		/// out.r = 10.0f/31.0f;		// 1/3 = 10/30      Red range     0..30, 31 unused
		/// out.g = 21.0f/63.0f;		// 1/3 = 21/63      Green range   0..63
		/// </code>
		/// </remarks>
		/// <returns>A new color that is more resistant to DXT compression.</returns>
		public ColorSingle RGB_to_mCIE()
		{
			ColorSingle temp = this;

			// to get gray chrominance for dark colors.
			temp.R += 0.000001f;
			temp.G += 0.000001f;
			temp.B += 0.000001f;

			float rgbSum = temp.R + temp.G + temp.B;

			float fInv = 1.0f / rgbSum;

			float rNorm = 3 * 10.0f / 31.0f * temp.R * fInv;
			float gNorm = 3 * 21.0f / 63.0f * temp.G * fInv;
			float scale = rgbSum / 3.0f;

			rNorm = Math.Max(0.0f, Math.Min(1.0f, rNorm));
			gNorm = Math.Max(0.0f, Math.Min(1.0f, gNorm));

			return new ColorSingle(rNorm, gNorm, scale);
		}
		/// <summary>
		/// Converts color that is resistant to DXT compression to normal color.
		/// </summary>
		/// <remarks>
		/// <code>
		/// // test gray.
		/// 							//                  Blue range    0..31
		/// out.r = 10.0f/31.0f;		// 1/3 = 10/30      Red range     0..30, 31 unused
		/// out.g = 21.0f/63.0f;		// 1/3 = 21/63      Green range   0..63
		/// </code>
		/// </remarks>
		/// <returns>A normal color.</returns>
		public ColorSingle mCIE_to_RGB()
		{
			ColorSingle output = this;

			float fScale = output.B;

			output.R *= 31.0f / 30.0f;
			output.G *= 63.0f / 63.0f;
			output.B = 0.999f - output.R - output.G;

			float s = 3.0f * fScale;

			output.R *= s; output.G *= s; output.B *= s;

			output.Clamp();

			return output;
		}
		#endregion
	}
}