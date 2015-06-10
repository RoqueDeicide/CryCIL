using System;

namespace CryCil.Specials
{
	/// <summary>
	/// Defines specials structures that represent different types of angles.
	/// </summary>
	public static class Angle
	{
		/// <summary>
		/// Defines specials structures that represent angles with difference between minimal and maximal
		/// value being equal to <see cref="Math.PI"/> radians (180 degrees).
		/// </summary>
		public static class Half
		{
			/// <summary>
			/// Defines specials structures that represent angles with difference between minimal and
			/// maximal value being equal to 180 degrees.
			/// </summary>
			public static class Degrees
			{
				/// <summary>
				/// Represents an angle within the range from 0 to 180 degrees.
				/// </summary>
				public struct Unsigned
				{
					private float val;
					/// <summary>
					/// Implicitly converts the object to value.
					/// </summary>
					/// <param name="a">An object to convert.</param>
					/// <returns>A value.</returns>
					public static implicit operator float(Unsigned a)
					{
						return a.val;
					}
					/// <summary>
					/// Implicitly converts the value to object.
					/// </summary>
					/// <param name="a">A value to convert.</param>
					/// <returns>An object.</returns>
					public static implicit operator Unsigned(float a)
					{
						return new Unsigned { val = a < 0 ? 0 : a > 180 ? 180 : a };
					}
				}
				/// <summary>
				/// Represents an angle within the range from -90 to 90 degrees.
				/// </summary>
				public struct Signed
				{
					private float val;
					/// <summary>
					/// Implicitly converts the object to value.
					/// </summary>
					/// <param name="a">An object to convert.</param>
					/// <returns>A value.</returns>
					public static implicit operator float(Signed a)
					{
						return a.val;
					}
					/// <summary>
					/// Implicitly converts the value to object.
					/// </summary>
					/// <param name="a">A value to convert.</param>
					/// <returns>An object.</returns>
					public static implicit operator Signed(float a)
					{
						return new Signed { val = a < -90 ? -90 : a > 90 ? 90 : a };
					}
				}
			}
			/// <summary>
			/// Defines specials structures that represent angles with difference between minimal and
			/// maximal value being equal to <see cref="Math.PI"/> radians.
			/// </summary>
			public static class Radians
			{
				/// <summary>
				/// Represents an angle within the range from 0 to <see cref="Math.PI"/> radians.
				/// </summary>
				public struct Unsigned
				{
					private float val;
					/// <summary>
					/// Implicitly converts the object to value.
					/// </summary>
					/// <param name="a">An object to convert.</param>
					/// <returns>A value.</returns>
					public static implicit operator float(Unsigned a)
					{
						return a.val;
					}
					/// <summary>
					/// Implicitly converts the value to object.
					/// </summary>
					/// <param name="a">A value to convert.</param>
					/// <returns>An object.</returns>
					public static implicit operator Unsigned(float a)
					{
						return new Unsigned { val = a < 0 ? 0 : a > Math.PI ? (float)Math.PI : a };
					}
				}
				/// <summary>
				/// Represents an angle within the range from -<see cref="Math.PI"/> / 2 to <see cref="Math.PI"/> / 2 degrees.
				/// </summary>
				public struct Signed
				{
					private float val;
					/// <summary>
					/// Implicitly converts the object to value.
					/// </summary>
					/// <param name="a">An object to convert.</param>
					/// <returns>A value.</returns>
					public static implicit operator float(Signed a)
					{
						return a.val;
					}
					/// <summary>
					/// Implicitly converts the value to object.
					/// </summary>
					/// <param name="a">A value to convert.</param>
					/// <returns>An object.</returns>
					public static implicit operator Signed(float a)
					{
						return new Signed
						{
							val = (float)(a < -Math.PI / 2 ? -Math.PI / 2 : a > Math.PI / 2 ? Math.PI / 2 : a)
						};
					}
				}
			}
		}
	}
}