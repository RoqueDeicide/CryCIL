using System;
using System.Runtime.InteropServices;

namespace CryCil.Specials
{
	/// <summary>
	/// Represents a single-precision floating point number that is clamped into range from 0 to 1.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct UnitSingle
	{
		#region Fields
		private readonly float val;
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="value">A value to be clamped into range from 0 to 1.</param>
		public UnitSingle(float value)
		{
			this.val = value < 0 ? 0 : value > 1 ? 1 : value;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Converts an object of type <see cref="UnitSingle"/> to a normal single-precision floating point
		/// number.
		/// </summary>
		/// <param name="v">An object to convert.</param>
		/// <returns>A value that was stored within the object.</returns>
		public static implicit operator float(UnitSingle v)
		{
			return v.val;
		}
		/// <summary>
		/// Converts a normal single-precision floating point number to an object of type
		/// <see cref="UnitSingle"/>.
		/// </summary>
		/// <remarks>
		/// This operator is implicit for the sake of convenience, it is quite dangerous since the value is
		/// clamped.
		/// </remarks>
		/// <param name="v">An value to convert.</param>
		/// <returns>An object that stores given value.</returns>
		public static implicit operator UnitSingle(float v)
		{
			return new UnitSingle(v);
		}
		#endregion
		#region Utilities
		#endregion
	}
}