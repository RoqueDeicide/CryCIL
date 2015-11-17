using System;
using System.Runtime.InteropServices;

namespace CryCil
{
	/// <summary>
	/// Represents a 4D vector with components represented by <see cref="short"/> type.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4Int16 : IEquatable<Vector4Int16>
	{
		#region Fields
		/// <summary>
		/// One of the components of this vector.
		/// </summary>
		public short X, Y, Z, W;
		#endregion
		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="Vector4Int16"/> struct.
		/// </summary>
		/// <param name="x">Initial value for the X component of the vector.</param>
		/// <param name="y">Initial value for the Y component of the vector.</param>
		/// <param name="z">Initial value for the Z component of the vector.</param>
		/// <param name="w">Initial value for the W component of the vector.</param>
		public Vector4Int16(short x, short y, short z, short w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if objects are equal.</returns>
		public bool Equals(Vector4Int16 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if objects are of the same type and are equal.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Vector4Int16 && this.Equals((Vector4Int16)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = this.X.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
				hashCode = (hashCode * 397) ^ this.W.GetHashCode();
				return hashCode;
			}
		}
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are equal.</returns>
		public static bool operator ==(Vector4Int16 left, Vector4Int16 right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are not equal.</returns>
		public static bool operator !=(Vector4Int16 left, Vector4Int16 right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}