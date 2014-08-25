using System;

using System.ComponentModel;
using CryEngine.Mathematics;

namespace CryEngine
{
	/// <summary>
	/// Defines a color in terms of its red, green, blue and alpha values.
	/// </summary>
	[TypeConverter(typeof(Misc.TypeConverters.ColorTypeConverter))]
	public struct Color
	{
		/// <summary>
		/// Constructs a new color object specifying the red, green, blue and alpha values.
		/// </summary>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <param name="alpha"></param>
		public Color(float red, float green, float blue, float alpha = 1)
			: this()
		{
			R = red;
			G = green;
			B = blue;
			A = alpha;
		}

		/// <summary>
		/// Constructs a new color object specifying the red, green, blue and alpha values.
		/// </summary>
		/// <param name="rgb"></param>
		/// <param name="a"></param>
		public Color(Vector3 rgb, float a = 1) : this(rgb.X, rgb.Y, rgb.Z, a) { }

		/// <summary>
		/// Constructs a new greyscale color object.
		/// </summary>
		/// <param name="brightness">
		/// The brightness of the object, where 0 is black, and 1 is white.
		/// </param>
		public Color(float brightness) : this(brightness, brightness, brightness) { }

		#region Overrides
		public override bool Equals(object obj)
		{
			if (obj is Color)
				return this == (Color)obj;

			return false;
		}

		public override string ToString()
		{
			return String.Format("R {0} G {1} B {2} A {3}", R, G, B, A);
		}

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + R.GetHashCode();
				hash = hash * 29 + G.GetHashCode();
				hash = hash * 29 + B.GetHashCode();
				hash = hash * 29 + A.GetHashCode();

				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(Color col1, Color col2)
		{
			return col1.R == col2.R && col1.G == col2.G && col1.B == col2.B && col1.A == col2.A;
		}

		public static bool operator !=(Color col1, Color col2)
		{
			return col1.R != col2.R || col1.G != col2.G || col1.B != col2.B || col1.A != col2.A;
		}

		public static implicit operator Color(Vector3 vec)
		{
			return new Color(vec.X, vec.Y, vec.Z);
		}

		public static implicit operator Vector3(Color clr)
		{
			return new Vector3(clr.R, clr.G, clr.B);
		}
		#endregion

		/// <summary>
		/// The red value of the color.
		/// </summary>
		public float R { get; set; }

		/// <summary>
		/// The green value of the color.
		/// </summary>
		public float G { get; set; }

		/// <summary>
		/// The blue value of the color.
		/// </summary>
		public float B { get; set; }

		/// <summary>
		/// The alpha value of the color.
		/// </summary>
		public float A { get; set; }

		#region Statics
		public static Color Red { get { return new Color(1, 0, 0); } }
		public static Color Blue { get { return new Color(0, 0, 1); } }
		public static Color Green { get { return new Color(0, 1, 0); } }

		public static Color Black { get { return new Color(0); } }
		public static Color White { get { return new Color(1); } }
		#endregion
	}
}