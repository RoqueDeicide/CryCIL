﻿using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil
{
	public partial struct Vector2d
	{
		#region Arithmetic Operators
		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="left"> The first vector to add.</param>
		/// <param name="right">The second vector to add.</param>
		/// <returns>
		/// A new vector where each coordinate is a sum of respective coordinates of given 2 vectors.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator +(Vector2d left, Vector2d right)
		{
			return new Vector2d(left.X + right.X, left.Y + right.Y);
		}
		/// <summary>
		/// Subtracts one vector from another.
		/// </summary>
		/// <param name="left"> The vector to subtract from.</param>
		/// <param name="right">The vector to subtract.</param>
		/// <returns>
		/// A new vector where each coordinate is a result of subtraction of respective coordinates of given
		/// 2 vectors.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d left, Vector2d right)
		{
			return new Vector2d(left.X - right.X, left.Y - right.Y);
		}
		/// <summary>
		/// Reverses the direction of a given vector.
		/// </summary>
		/// <param name="value">The vector to negate.</param>
		/// <returns>A vector facing in the opposite direction.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d value)
		{
			return new Vector2d(-value.X, -value.Y);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>
		/// A new vector where each coordinate is a multiple of respective coordinate of given vector by
		/// given factor.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator *(double scale, Vector2d value)
		{
			return new Vector2d(value.X * scale, value.Y * scale);
		}
		/// <summary>
		/// Scales a vector by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>
		/// A new vector where each coordinate is a multiple of respective coordinate of given vector by
		/// given factor.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator *(Vector2d value, double scale)
		{
			return new Vector2d(value.X * scale, value.Y * scale);
		}
		/// <summary>
		/// Scales a vector down by the given value.
		/// </summary>
		/// <param name="value">The vector to scale.</param>
		/// <param name="scale">The amount by which to scale the vector.</param>
		/// <returns>
		/// A new vector where each coordinate is a multiple of respective coordinate of given vector by
		/// given factor.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator /(Vector2d value, double scale)
		{
			return new Vector2d(value.X / scale, value.Y / scale);
		}
		/// <summary>
		/// Calculates dot product of two vectors.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Dot product of two vectors.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double operator *(Vector2d left, Vector2d right)
		{
			return left.X * right.X + left.Y * right.Y;
		}
		/// <summary>
		/// Modulates a vector with another by performing component-wise multiplication.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>The modulated vector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator |(Vector2d left, Vector2d right)
		{
			return new Vector2d(left.X * right.X, left.Y * right.Y);
		}
		#endregion
		#region Comparison Operators
		/// <summary>
		/// Tests for equality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has exactly the same value as <paramref name="right"/> ;
		/// otherwise, <c>false</c> .
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2d left, Vector2d right)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return left.X == right.X && left.Y == right.Y;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Tests for inequality between two objects.
		/// </summary>
		/// <param name="left"> The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		/// <c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/> ;
		/// otherwise, <c>false</c> .
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2d left, Vector2d right)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return left.X != right.X || left.Y != right.Y;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		#endregion
		#region Conversion Operators
		/// <summary>
		/// Performs an implicit conversion from <see cref="PointF"/> to <see cref="Vector2d"/> .
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// A new instance of type <see cref="Vector2d"/> which has X and Y coordinates taken from
		/// <paramref name="value"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2d(PointF value)
		{
			return new Vector2d(value.X, value.Y);
		}
		/// <summary>
		/// Performs an implicit conversion from <see cref="SizeF"/> to <see cref="Vector2d"/> .
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// A new instance of type <see cref="Vector2d"/> which has X and Y coordinates taken from
		/// <paramref name="value"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2d(SizeF value)
		{
			return new Vector2d(value.Width, value.Height);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2d"/> to <see cref="Vector3d"/> .
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// A new instance of type <see cref="Vector3d"/> which has X and Y coordinates taken from
		/// <paramref name="value"/> and Z coordinate equal to 0.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3d(Vector2d value)
		{
			return new Vector3d(value);
		}
		/// <summary>
		/// Performs an explicit conversion from <see cref="Vector2d"/> to <see cref="Vector4d"/> .
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// A new instance of type <see cref="Vector4d"/> which has X and Y coordinates taken from
		/// <paramref name="value"/> and Z and W coordinates equal to 0.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector4d(Vector2d value)
		{
			return new Vector4d(value, 0.0f, 0.0f);
		}
		#endregion
	}
}