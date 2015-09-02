using System;

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
	}
}