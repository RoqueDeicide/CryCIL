using System;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Defines a function that converts degrees to radians.
	/// </summary>
	public static class Degree
	{
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">Degrees to convert to radians.</param>
		/// <returns>Radians that were converted from degrees.</returns>
		public static float ToRadian(float degrees)
		{
			return (float)(degrees * Math.PI / 180.0f);
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">Degrees to convert to radians.</param>
		/// <returns>Radians that were converted from degrees.</returns>
		public static double ToRadian(double degrees)
		{
			return degrees * Math.PI / 180.0;
		}
		/// <summary>
		/// Normalizes and clamps given angle.
		/// </summary>
		/// <param name="degrees">Angle to normalize and clamp.</param>
		/// <param name="min">    Minimal border of the range to clamp the angle into.</param>
		/// <param name="max">    Maximal border of the range to clamp the angle into.</param>
		/// <returns>Degrees that were converted from radians.</returns>
		public static float Clamp(float degrees, float min, float max)
		{
			while (degrees < -360)
				degrees += 360;
			while (degrees > 360)
				degrees -= 360;

			return MathHelpers.Clamp(degrees, min, max);
		}
		/// <summary>
		/// Normalizes and clamps given angle.
		/// </summary>
		/// <param name="degrees">Angle to normalize and clamp.</param>
		/// <param name="min">    Minimal border of the range to clamp the angle into.</param>
		/// <param name="max">    Maximal border of the range to clamp the angle into.</param>
		/// <returns>Degrees that were converted from radians.</returns>
		public static double Clamp(double degrees, double min, double max)
		{
			while (degrees < -360)
				degrees += 360;
			while (degrees > 360)
				degrees -= 360;

			return MathHelpers.Clamp(degrees, min, max);
		}
	}
	/// <summary>
	/// Defines a function that converts radians to degrees.
	/// </summary>
	public static class Radian
	{
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">Radians to convert to degrees.</param>
		/// <returns>Degrees that were converted from radians.</returns>
		public static float ToDegree(float radians)
		{
			return (float)(radians * 180.0f / Math.PI);
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">Radians to convert to degrees.</param>
		/// <returns>Degrees that were converted from radians.</returns>
		public static double ToDegree(double radians)
		{
			return radians * 180.0 / Math.PI;
		}
		/// <summary>
		/// Normalizes and clamps given angle.
		/// </summary>
		/// <param name="radians">Angle to normalize and clamp.</param>
		/// <param name="min">    Minimal border of the range to clamp the angle into.</param>
		/// <param name="max">    Maximal border of the range to clamp the angle into.</param>
		/// <returns>Degrees that were converted from radians.</returns>
		public static float Clamp(float radians, float min, float max)
		{
			while (radians < -MathHelpers.PI2)
				radians += (float)MathHelpers.PI2;
			while (radians > MathHelpers.PI2)
				radians -= (float)MathHelpers.PI2;

			return MathHelpers.Clamp(radians, min, max);
		}
		/// <summary>
		/// Normalizes and clamps given angle.
		/// </summary>
		/// <param name="radians">Angle to normalize and clamp.</param>
		/// <param name="min">    Minimal border of the range to clamp the angle into.</param>
		/// <param name="max">    Maximal border of the range to clamp the angle into.</param>
		/// <returns>Degrees that were converted from radians.</returns>
		public static double Clamp(double radians, double min, double max)
		{
			while (radians < -MathHelpers.PI2)
				radians += MathHelpers.PI2;
			while (radians > MathHelpers.PI2)
				radians -= MathHelpers.PI2;

			return MathHelpers.Clamp(radians, min, max);
		}
	}
}