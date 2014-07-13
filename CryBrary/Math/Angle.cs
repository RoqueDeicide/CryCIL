using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	#region Commented
	///// <summary>
	///// Defines an angle in degrees.
	///// </summary>
	//public struct Degree
	//{
	//	private float v = 0;
	//	/// <summary>
	//	/// Initializes new angle value in degrees.
	//	/// </summary>
	//	/// <param name="degrees">Instance of <see cref="Single"/> that we will use as a starting value.</param>
	//	/// <param name="normalize">Indicates whether we should normalize this angle.</param>
	//	public Degree(float degrees, bool normalize = true)
	//	{
	//		this.v = degrees;
	//		if (normalize)
	//		{
	//			this.Normalize360();
	//		}
	//	}
	//	/// <summary>
	//	/// Creates new instance of <see cref="Degree"/>.
	//	/// </summary>
	//	/// <param name="rads">Angle in radians.</param>
	//	/// <param name="normalize">Indicates whether we should normalize this angle.</param>
	//	/// <returns>
	//	/// New instance of <see cref="Degree"/> with value equal
	//	/// to <paramref name="rads"/> * (180 / <see cref="Math.PI"/>).
	//	/// </returns>
	//	public static Degree FromRadians(float rads, bool normalize = true)
	//	{
	//		return new Degree((float)(rads * (180 / Math.PI)), normalize);
	//	}
	//	/// <summary>
	//	/// Normalizes this angle by putting into 0-360 range.
	//	/// </summary>
	//	public void Normalize360()
	//	{
	//		if (this.v > 360)
	//		{
	//			while (this.v > 360)
	//			{
	//				this.v -= 360;
	//			}
	//		}
	//		else if (this.v < 0)
	//		{
	//			this.v += 360;
	//		}
	//	}
	//	/// <summary>
	//	/// Increments this angle by given value in degrees.
	//	/// </summary>
	//	/// <param name="inc">Angle in degrees to add to</param>
	//	public void Increment(float inc)
	//	{
	//		this.v += inc;
	//		this.Normalize360();
	//	}

	// public void Decrement(float dec) { Increment(-dec); }

	// public override string ToString() { return v.ToString(); }

	// public override int GetHashCode() { return v.GetHashCode(); }

	// #region Operator overloads public static implicit operator
	// float(Degree angleObj) { return angleObj.v; }

	// public static implicit operator Degree(float _angle) { return
	// new Degree(_angle); }

	// public static Degree operator +(Degree lhs, Degree rhs) {
	// Degree angle = new Degree(lhs.v); angle.Increment(rhs.v);
	// return angle; }

	// public static Degree operator -(Degree lhs, Degree rhs) {
	// Degree angle = new Degree(lhs.v); angle.Decrement(rhs.v);
	// return angle; }

	// public static bool operator <(Degree lhs, Degree rhs) { if
	// (lhs.v < rhs.v) return true; return false; }

	// public static bool operator <=(Degree lhs, Degree rhs) { if
	// (lhs.v <= rhs.v) return true; return false; }

	// public static bool operator >(Degree lhs, Degree rhs) { if
	// (lhs.v > rhs.v) return true; return false; }

	// public static bool operator >=(Degree lhs, Degree rhs) { if
	// (lhs.v >= rhs.v) return true; return false; } #endregion
	//
	//}
	#endregion
	/// <summary>
	/// Defines few methods related to angles.
	/// </summary>
	public static class Angle
	{
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">Angle in radians.</param>
		/// <returns>Angle in degrees.</returns>
		public static float Degree(float radians)
		{
			return (float)(radians * (180 / Math.PI));
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">Angle in radians.</param>
		/// <param name="norm">
		/// Indicate whether we should normalize the angle, and if so
		/// then how.
		/// </param>
		/// <returns>Angle in degrees.</returns>
		public static float Degree(float radians, NormalizeType norm)
		{
			float degrees = (float)(radians * (180 / Math.PI));
			switch (norm)
			{
				case NormalizeType.Normalize180:
					while (degrees <= -180) degrees += 360;
					while (degrees > 180) degrees -= 360;
					break;
				case NormalizeType.Normalize360:
					while (degrees <= 0) degrees += 360;
					while (degrees > 360) degrees -= 360;
					break;
				default:
					break;
			}
			return degrees;
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">Angle in radians.</param>
		/// <returns>Angle in degrees.</returns>
		public static double Degree(double radians)
		{
			return radians * (180 / Math.PI);
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">Angle in radians.</param>
		/// <param name="norm">
		/// Indicate whether we should normalize the angle, and if so
		/// then how.
		/// </param>
		/// <returns>Angle in degrees.</returns>
		public static double Degree(double radians, NormalizeType norm)
		{
			double degrees = radians * (180 / Math.PI);
			switch (norm)
			{
				case NormalizeType.Normalize180:
					while (degrees <= -180) degrees += 360;
					while (degrees > 180) degrees -= 360;
					break;
				case NormalizeType.Normalize360:
					while (degrees <= 0) degrees += 360;
					while (degrees > 360) degrees -= 360;
					break;
				default:
					break;
			}
			return degrees;
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">Angle in degrees.</param>
		/// <returns>Angle in radians.</returns>
		public static double Radian(double degrees)
		{
			return degrees * Math.PI / 180;
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">Angle in degrees.</param>
		/// <param name="norm">
		/// Indicate whether we should normalize the angle, and if so
		/// then how.
		/// </param>
		/// <returns>Angle in radians.</returns>
		public static double Radian(double degrees, NormalizeType norm)
		{
			double radians = degrees * Math.PI / 180;
			switch (norm)
			{
				case NormalizeType.Normalize180:
					while (radians <= -Math.PI) radians += MathHelpers.PI2;
					while (radians > Math.PI) radians -= MathHelpers.PI2;
					break;
				case NormalizeType.Normalize360:
					while (radians <= 0) radians += MathHelpers.PI2;
					while (radians > MathHelpers.PI2) radians -= MathHelpers.PI2;
					break;
				default:
					break;
			}
			return radians;
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">Angle in degrees.</param>
		/// <returns>Angle in radians.</returns>
		public static float Radian(float degrees)
		{
			return (float)(degrees * Math.PI / 180);
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">Angle in degrees.</param>
		/// <param name="norm">
		/// Indicate whether we should normalize the angle, and if so
		/// then how.
		/// </param>
		/// <returns>Angle in radians.</returns>
		public static float Radian(float degrees, NormalizeType norm)
		{
			double radians = degrees * Math.PI / 180;
			switch (norm)
			{
				case NormalizeType.Normalize180:
					while (radians <= -Math.PI) radians += MathHelpers.PI2;
					while (radians > Math.PI) radians -= MathHelpers.PI2;
					break;
				case NormalizeType.Normalize360:
					while (radians <= 0) radians += MathHelpers.PI2;
					while (radians > MathHelpers.PI2) radians -= MathHelpers.PI2;
					break;
				default:
					break;
			}
			return (float)radians;
		}
	}
	/// <summary>
	/// Enumeration of types of normalization.
	/// </summary>
	public enum NormalizeType
	{
		/// <summary>
		/// Don't normalize.
		/// </summary>
		No,
		/// <summary>
		/// Normalize into range (-180, 180) or (-PI, PI).
		/// </summary>
		Normalize180,
		/// <summary>
		/// Normalize into range (0, 360) or (0, 2 * PI).
		/// </summary>
		Normalize360
	}
}