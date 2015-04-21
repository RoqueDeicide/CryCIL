using System.Runtime.CompilerServices;
using CryCil.Graphics;

namespace CryCil
{
	public partial struct Vector3d
	{
		#region Operators
		#region Arithmetic Operators
		/// <summary>
		/// Multiplies vector by given double factor.
		/// </summary>
		/// <param name="v">    Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator *(Vector3d v, double scale)
		{
			return new Vector3d(v.X * scale, v.Y * scale, v.Z * scale);
		}
		/// <summary>
		/// Multiplies vector by given double factor.
		/// </summary>
		/// <param name="v">    Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator *(double scale, Vector3d v)
		{
			return v * scale;
		}
		/// <summary>
		/// Divides vector by given double value.
		/// </summary>
		/// <param name="v">    Left operand.</param>
		/// <param name="scale">Right operand.</param>
		/// <returns>Result of division.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator /(Vector3d v, double scale)
		{
			scale = 1.0f / scale;

			return new Vector3d(v.X * scale, v.Y * scale, v.Z * scale);
		}
		/// <summary>
		/// Adds one vector to another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator +(Vector3d left, Vector3d right)
		{
			return new Vector3d(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		/// <summary>
		/// Adds one vector to another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator +(Vector2 left, Vector3d right)
		{
			return new Vector3d(left.X + right.X, left.Y + right.Y, right.Z);
		}
		/// <summary>
		/// Adds one vector to another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator +(Vector3d left, Vector2 right)
		{
			return new Vector3d(left.X + right.X, left.Y + right.Y, left.Z);
		}
		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator -(Vector3d left, Vector3d right)
		{
			return new Vector3d(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}
		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator -(Vector2 left, Vector3d right)
		{
			return new Vector3d(left.X - right.X, left.Y - right.Y, 0 - right.Z);
		}
		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator -(Vector3d left, Vector2 right)
		{
			return new Vector3d(left.X - right.X, left.Y - right.Y, left.Z);
		}
		#endregion
		#region Product Operators
		/// <summary>
		/// Calculates dot product of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Dot product.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double operator *(Vector3d left, Vector3d right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}
		/// <summary>
		/// Calculates cross product of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Cross product.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator %(Vector3d left, Vector3d right)
		{
			return
				new Vector3d
				(
					left.Y * right.Z - left.Z * right.Y,
					left.Z * right.X - left.X * right.Z,
					left.X * right.Y - left.Y * right.X
				);
		}
		#endregion
		#region Unary Operators
		/// <summary>
		/// Gets flipped version of given vector.
		/// </summary>
		/// <param name="v">Vector to flip.</param>
		/// <returns>Flipped vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d operator -(Vector3d v)
		{
			return new Vector3d(-v.X, -v.Y, -v.Z);
		}
		#endregion
		#region Comparison Operators
		/// <summary>
		/// Checks equality of two given vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if two vectors are equal.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3d left, Vector3d right)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Checks equality of two given vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if two vectors are not equal.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3d left, Vector3d right)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		#endregion
		#region Conversion Operators
		/// <summary>
		/// Implicitly converts given vector to <see cref="ColorSingle"/> instance.
		/// </summary>
		/// <param name="vec">Vector to convert.</param>
		/// <returns>
		/// <see cref="ColorSingle"/> object where R is vector's X-component, G - Y, B - Z.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ColorSingle(Vector3d vec)
		{
			return new ColorSingle((float)vec.X, (float)vec.Y, (float)vec.Z);
		}
		#endregion
		#endregion
		#region Products
		/// <summary>
		/// Calculates cross product of two vectors.
		/// </summary>
		/// <param name="first">First vector.</param>
		/// <param name="v">    Second vector.</param>
		/// <returns>Cross product.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3d Cross(Vector3d first, Vector3d v)
		{
			return
				new Vector3d
				(
					first.Y * v.Z - first.Z * v.Y,
					first.Z * v.X - first.X * v.Z,
					first.X * v.Y - first.Y * v.X
				);
		}
		/// <summary>
		/// Calculates dot product of two vectors.
		/// </summary>
		/// <param name="v0">First vector.</param>
		/// <param name="v1">Second vector.</param>
		/// <returns>Dot product.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Dot(Vector3d v0, Vector3d v1)
		{
			return v0.X * v1.X + v0.Y * v1.Y + v0.Z * v1.Z;
		}
		/// <summary>
		/// Calculates mixed product of three vectors.
		/// </summary>
		/// <remarks>a.Dot(b.Cross(c))</remarks>
		/// <param name="v0">First vector.</param>
		/// <param name="v1">Second vector.</param>
		/// <param name="v2">Third vector.</param>
		/// <returns>Dot product of first vector and cross product of second and third.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Mixed(Vector3d v0, Vector3d v1, Vector3d v2)
		{
			Vector3d cross = new Vector3d
							(
								v1.Y * v2.Z - v1.Z * v2.Y,
								v1.Z * v2.X - v1.X * v2.Z,
								v1.X * v2.Y - v1.Y * v2.X
							);
			return v0.X * cross.X + v0.Y * cross.Y + v0.Z * cross.Z;
		}
		#endregion
	}
}