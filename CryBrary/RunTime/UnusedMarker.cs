using System;
using CryEngine.Mathematics;
using CryEngine.Mathematics.MemoryMapping;

namespace CryEngine.RunTime
{
	/// <summary>
	/// Defines special values that are recognized as unused by CryEngine, and ways to check usability of objects.
	/// </summary>
	public static class UnusedMarker
	{
		#region Fields
		/// <summary>
		/// If a single-precision floating point data field is equal to this value than CryEngine will ignore it.
		/// </summary>
		public static readonly float Float;
		/// <summary>
		/// If a signed integer data field is equal to this value than CryEngine will ignore it.
		/// </summary>
		public static readonly int Integer;
		/// <summary>
		/// If a unsigned integer data field is equal to this value than CryEngine will ignore it.
		/// </summary>
		public static readonly uint UnsignedInteger;
		/// <summary>
		/// If a 3D vector data field is equal to this value than CryEngine will ignore it.
		/// </summary>
		public static readonly Vector3 Vector3;
		/// <summary>
		/// If a quaternion data field is equal to this value than CryEngine will ignore it.
		/// </summary>
		public static readonly Quaternion Quaternion;
		/// <summary>
		/// If a pointer data field is equal to this value than CryEngine will ignore it.
		/// </summary>
		public static readonly IntPtr IntPtr;
		#endregion
		#region Construction
		static UnusedMarker()
		{
			UnusedMarker.Float = new Bytes4(0xFFBFFFFF).SingleFloat;
			UnusedMarker.Integer = 1 << 31;
			UnusedMarker.UnsignedInteger = 1u << 31;
			UnusedMarker.Vector3 = new Vector3(UnusedMarker.Float);
			UnusedMarker.Quaternion = new Quaternion(UnusedMarker.Float, 0, 0, 0);
			UnusedMarker.IntPtr = new IntPtr(-1);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Indicates whether given value will be ignored by CryEngine.
		/// </summary>
		/// <param name="var">Value to check.</param>
		/// <returns>True, if value will be ignored.</returns>
		public static bool IsUnused(float var)
		{
			return (new Bytes4(var).UnsignedInt & 0xFFA00000) == 0xFFA00000;
		}
		/// <summary>
		/// Indicates whether given value will be ignored by CryEngine.
		/// </summary>
		/// <param name="var">Value to check.</param>
		/// <returns>True, if value will be ignored.</returns>
		public static bool IsUnused(int var)
		{
			return var == 1 << 31;
		}
		/// <summary>
		/// Indicates whether given value will be ignored by CryEngine.
		/// </summary>
		/// <param name="var">Value to check.</param>
		/// <returns>True, if value will be ignored.</returns>
		public static bool IsUnused(uint var)
		{
			return var == 1u << 31;
		}
		/// <summary>
		/// Indicates whether given value will be ignored by CryEngine.
		/// </summary>
		/// <param name="var">Value to check.</param>
		/// <returns>True, if value will be ignored.</returns>
		public static bool IsUnused(Vector3 var)
		{
			return IsUnused(var.X);
		}
		/// <summary>
		/// Indicates whether given value will be ignored by CryEngine.
		/// </summary>
		/// <param name="var">Value to check.</param>
		/// <returns>True, if value will be ignored.</returns>
		public static bool IsUnused(Quaternion var)
		{
			return IsUnused(var.W);
		}
		/// <summary>
		/// Indicates whether given value will be ignored by CryEngine.
		/// </summary>
		/// <param name="var">Value to check.</param>
		/// <returns>True, if value will be ignored.</returns>
		public static bool IsUnused(IntPtr var)
		{
			return var.ToInt32() == -1;
		}
		#endregion
	}
}