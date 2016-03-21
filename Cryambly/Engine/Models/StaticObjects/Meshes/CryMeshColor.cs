using System;
using System.Linq;
using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a color that is used by CryEngine meshes.
	/// </summary>
	public struct CryMeshColor : IEquatable<CryMeshColor>
	{
		#region Fields
		private byte r, g, b, a;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="r">Red component.</param>
		/// <param name="g">Green component.</param>
		/// <param name="b">Blue component.</param>
		/// <param name="a">Alpha component.</param>
		public CryMeshColor(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="colors">
		/// 4D vector which X, Y, Z and W components are used to initialize Red, Green, Blue and Alpha
		/// components respectively. The components of this vector must be in range from 0 to 255.
		/// </param>
		public CryMeshColor(Vector4 colors)
		{
			this.r = (byte)colors.X;
			this.g = (byte)colors.Y;
			this.b = (byte)colors.Z;
			this.a = (byte)colors.W;
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="colors">Another object that provides the color components.</param>
		public CryMeshColor(ColorByte colors)
		{
			this.r = colors.Red;
			this.g = colors.Green;
			this.b = colors.Blue;
			this.a = colors.Alpha;
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="colors">Another object that provides the color components.</param>
		public CryMeshColor(ColorSingle colors)
		{
			this.r = (byte)(colors.R * 255);
			this.g = (byte)(colors.G * 255);
			this.b = (byte)(colors.B * 255);
			this.a = (byte)(colors.A * 255);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Exports Red, Green and Blue components into another color.
		/// </summary>
		/// <param name="other">Another color.</param>
		public void ExportRGB(ref CryMeshColor other)
		{
			other.r = this.r;
			other.g = this.g;
			other.b = this.b;
		}
		/// <summary>
		/// Exports Alpha component into another color.
		/// </summary>
		/// <param name="other">Another color.</param>
		public void ExportA(ref CryMeshColor other)
		{
			other.a = this.a;
		}
		/// <summary>
		/// Exports all components into another color.
		/// </summary>
		/// <param name="other">Another color.</param>
		public void Export(out ColorByte other)
		{
			other = new ColorByte(this.r, this.g, this.b, this.a);
		}
		/// <summary>
		/// Exports all components into a vector.
		/// </summary>
		/// <param name="other">
		/// Vector to export components to. All components will be in range [0; 255].
		/// </param>
		public void Export(out Vector4 other)
		{
			other.X = this.r;
			other.Y = this.g;
			other.Z = this.b;
			other.W = this.a;
		}
		/// <summary>
		/// Masks the alpha component by applying bitwise AND operation to it.
		/// </summary>
		/// <param name="mask">The value to use as a second operand in the AND operation.</param>
		public void MaskAlpha(byte mask)
		{
			this.a &= mask;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if objects are equal.</returns>
		public bool Equals(CryMeshColor other)
		{
			return this.r == other.r && this.g == other.g && this.b == other.b && this.a == other.a;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if objects are of the same type and are equal.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is CryMeshColor && this.Equals((CryMeshColor)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = this.r.GetHashCode();
				hashCode = (hashCode * 397) ^ this.g.GetHashCode();
				hashCode = (hashCode * 397) ^ this.b.GetHashCode();
				hashCode = (hashCode * 397) ^ this.a.GetHashCode();
				return hashCode;
			}
		}
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are equal.</returns>
		public static bool operator ==(CryMeshColor left, CryMeshColor right)
		{
			return left.r == right.r && left.g == right.g && left.b == right.b && left.a == right.a;
		}
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are not equal.</returns>
		public static bool operator !=(CryMeshColor left, CryMeshColor right)
		{
			return left.r != right.r || left.g != right.g || left.b != right.b || left.a != right.a;
		}
		/// <summary>
		/// Determines whether left operand is less then the right one.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is less then the right one.</returns>
		public static bool operator <(CryMeshColor left, CryMeshColor right)
		{
			return
				left.r != right.r
					? left.r < right.r
					: left.g != right.g
						? left.g < right.g
						: left.b != right.b
							? left.b < right.b
							: left.a < right.a;
		}
		/// <summary>
		/// Determines whether left operand is greater then the right one.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is greater then the right one.</returns>
		public static bool operator >(CryMeshColor left, CryMeshColor right)
		{
			return !(left < right) && left != right;
		}
		/// <summary>
		/// Changes this color to be a result of linear interpolation between its current value and a given
		/// one.
		/// </summary>
		/// <param name="endPoint">Other vector that color the interpolation 'end point'.</param>
		/// <param name="position">
		/// Parameter that specifies value of the interpolated value relative to this one and 'end point'.
		/// </param>
		public void LinearInterpolation(ref CryMeshColor endPoint, float position)
		{
			ColorByte start, end;

			this.Export(out start);
			endPoint.Export(out end);

			Interpolation.Linear.Apply(out start, start, end, position);

			this.r = start.Red;
			this.g = start.Green;
			this.b = start.Blue;
			this.a = start.Alpha;
		}
		#endregion
	}
}