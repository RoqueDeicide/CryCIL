using System;

namespace CryEngine.Mathematics
{
	/// <summary>
	/// Defines few methods related to angles.
	/// </summary>
	public static class Angle
	{
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians"> Angle in radians. </param>
		/// <returns> Angle in degrees. </returns>
		public static float Degree(float radians)
		{
			return (float)(radians * (180 / Math.PI));
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians"> Angle in radians. </param>
		/// <param name="norm">   
		/// Indicate whether we should normalize the angle, and if so then how.
		/// </param>
		/// <returns> Angle in degrees. </returns>
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
			}
			return degrees;
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians"> Angle in radians. </param>
		/// <returns> Angle in degrees. </returns>
		public static double Degree(double radians)
		{
			return radians * (180 / Math.PI);
		}
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians"> Angle in radians. </param>
		/// <param name="norm">   
		/// Indicate whether we should normalize the angle, and if so then how.
		/// </param>
		/// <returns> Angle in degrees. </returns>
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
			}
			return degrees;
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees"> Angle in degrees. </param>
		/// <returns> Angle in radians. </returns>
		public static double Radian(double degrees)
		{
			return degrees * Math.PI / 180;
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees"> Angle in degrees. </param>
		/// <param name="norm">   
		/// Indicate whether we should normalize the angle, and if so then how.
		/// </param>
		/// <returns> Angle in radians. </returns>
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
			}
			return radians;
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees"> Angle in degrees. </param>
		/// <returns> Angle in radians. </returns>
		public static float Radian(float degrees)
		{
			return (float)(degrees * Math.PI / 180);
		}
		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees"> Angle in degrees. </param>
		/// <param name="norm">   
		/// Indicate whether we should normalize the angle, and if so then how.
		/// </param>
		/// <returns> Angle in radians. </returns>
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