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
		#region Construction
		/// <summary>
		/// Constructs a new color object specifying the red, green, blue and alpha
		/// values.
		/// </summary>
		/// <param name="red">  </param>
		/// <param name="green"></param>
		/// <param name="blue"> </param>
		/// <param name="alpha"></param>
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
		/// <param name="rgb"></param>
		/// <param name="a">  </param>
		public ColorSingle(Vector3 rgb, float a = 1) : this(rgb.X, rgb.Y, rgb.Z, a) { }
		/// <summary>
		/// Constructs a new greyscale color object.
		/// </summary>
		/// <param name="brightness">
		/// The brightness of the object, where 0 is black, and 1 is white.
		/// </param>
		public ColorSingle(float brightness) : this(brightness, brightness, brightness) { }
		#endregion

		#region Overrides
		/// <summary>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is ColorSingle)
				return this == (ColorSingle)obj;

			return false;
		}
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("R {0} G {1} B {2} A {3}", this.R, this.G, this.B, this.A);
		}
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + this.R.GetHashCode();
				hash = hash * 29 + this.G.GetHashCode();
				hash = hash * 29 + this.B.GetHashCode();
				hash = hash * 29 + this.A.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		#endregion

		#region Operators
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
		#endregion
	}
}