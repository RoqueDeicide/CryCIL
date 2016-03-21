using System;
using System.Linq;
using CryCil.Geometry;
using CryCil.MemoryMapping;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Defines values that are recognized as unused and functionality related to them.
	/// </summary>
	public static class UnusedValue
	{
		/// <summary>
		/// Single-precision floating point number that is recognized as unused.
		/// </summary>
		public static readonly float Single;
		/// <summary>
		/// Double-precision floating point number that is recognized as unused.
		/// </summary>
		public static readonly double Double;
		/// <summary>
		/// 32-bit signed integer number that is recognized as unused.
		/// </summary>
		public const int Int32 = 1 << 31;
		/// <summary>
		/// 32-bit unsigned integer number that is recognized as unused.
		/// </summary>
		public const uint UInt32 = 1u << 31;
		/// <summary>
		/// 3D vector that is recognized as unused.
		/// </summary>
		public static readonly Vector3 Vector;
		/// <summary>
		/// Quaternion that is recognized as unused.
		/// </summary>
		public static readonly Quaternion Quaternion;
		/// <summary>
		/// Pointer that is recognized as unused.
		/// </summary>
		public static readonly IntPtr Pointer;

		static UnusedValue()
		{
			Single = new Bytes4(0xFFBFFFFF).SingleFloat;
			Double = new Bytes8(0xFFF7FFFF).DoubleFloat;
			Vector = new Vector3(Single, 0, 0);
			Quaternion = new Quaternion(0, 0, 0, Single);
			Pointer = new IntPtr(-1);
		}

		// Extension methods.

		/// <summary>
		/// Indicates whether this value is used.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>True, if value wasn't marked as unused.</returns>
		public static bool IsUsed(this float value)
		{
			return (new Bytes4(value).SignedInt & 0xFFA00000) != 0xFFA00000;
		}
		/// <summary>
		/// Indicates whether this value is used.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>True, if value wasn't marked as unused.</returns>
		public static bool IsUsed(this double value)
		{
			return ((int)new Bytes8(value).SignedLong & 0xFFF40000) != 0xFFF40000;
		}
		/// <summary>
		/// Indicates whether this value is used.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>True, if value wasn't marked as unused.</returns>
		public static bool IsUsed(this int value)
		{
			return value != Int32;
		}
		/// <summary>
		/// Indicates whether this value is used.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>True, if value wasn't marked as unused.</returns>
		public static bool IsUsed(this uint value)
		{
			return value != UInt32;
		}
		/// <summary>
		/// Indicates whether this value is used.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>True, if value wasn't marked as unused.</returns>
		public static bool IsUsed(this Vector3 value)
		{
			return value.X.IsUsed();
		}
		/// <summary>
		/// Indicates whether this value is used.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>True, if value wasn't marked as unused.</returns>
		public static bool IsUsed(this Quaternion value)
		{
			return value.W.IsUsed();
		}
		/// <summary>
		/// Indicates whether this value is used.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <returns>True, if value wasn't marked as unused.</returns>
		public static bool IsUsed(this IntPtr value)
		{
			return value == Pointer;
		}
	}
}