using CryCil.Geometry;

namespace CryCil
{
	public static partial class Interpolations
	{
		/// <summary>
		/// Defines functions that allow user to work with linear interpolations.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Linear interpolation is a process of calculating a value of N-dimensional
		/// vector that is located on the line defined by two vectors (the order of which
		/// is important) and position of that vector relative to the first line-defining
		/// one is denoted by a scalar value (a.k.a. parameter).
		/// </para>
		/// <para>
		/// The formula for calculation is the same for any vectors and only requires
		/// definition of: summation, negation and scaling (multiplication by scalar
		/// value) .
		/// </para>
		/// <para>result = first + (second - first) * parameter;</para>
		/// </remarks>
		public static partial class Linear
		{
			internal static void Apply(out Quaternion result, Quaternion first, Quaternion second, float parameter)
			{
				result =
					new Quaternion
					(
						first.X + (first.X - second.X) * parameter,
						first.Y + (first.Y - second.Y) * parameter,
						first.Z + (first.Z - second.Z) * parameter,
						first.W + (first.W - second.W) * parameter
					);
			}
			internal static Quaternion Create(Quaternion first, Quaternion second, float parameter)
			{
				return
					new Quaternion
					(
						first.X + (first.X - second.X) * parameter,
						first.Y + (first.Y - second.Y) * parameter,
						first.Z + (first.Z - second.Z) * parameter,
						first.W + (first.W - second.W) * parameter
					);
			}
		}
	}
}