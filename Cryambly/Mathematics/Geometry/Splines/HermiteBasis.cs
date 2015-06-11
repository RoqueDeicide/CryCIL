using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace CryCil.Geometry.Splines
{
	/// <summary>
	/// Represents a set of cubic Hermite spline basis numbers.
	/// </summary>
	/// <remarks>
	/// Hermite splines use the following formula to describe the coordinates of the point on the curve
	/// between 2 end-points:
	/// <code>
	/// // The following 4 variables are initialized with some value and they describe a portion of a spline.
	/// Vector3 p0;		// This is a starting point of a spline, by interpolating using time parameter = 0 we will
	/// 				// get this point as a result.
	/// Vector3 p1;		// This is a ending point of a spline, by interpolating using time parameter = 1 we will
	/// 				// get this point as a result.
	/// Vector3 m0;		// This is a starting tangent point, it defines how the spline will go towards the ending point.
	/// Vector3 m1;		// This is a ending tangent point, it defines how the spline will go towards the starting point.
	/// 
	/// // This is a function argument.
	/// float t;		// This is a "time" parameter, it describes position of the interpolated point along the spline
	/// 				// relative to the location of the starting point.
	/// 
	/// // We will need these:
	/// float t2 = t * t;		// Squared "time" parameter.
	/// float t3 = t2 * t;		// Cubed "time" parameter.
	/// 
	/// // Now, let's interpolate.
	/// Vector3 result =
	/// 	 (2 * t3 - 3 * t2 + 1) * p0 + (t3 - 2 * t2 + t) * m0 + (-2 * t3 + 3 * t2) * p1 + (t3 - t2) * m1;
	/// 
	/// // As you can see from the formula above calculations in parentheses only use the argument, that is why they
	/// // are usually calculated separately and called "basis numbers". Here they are:
	/// float pb0 = 2 * t3 - 3 * t2 + 1;		// Starting point basis number.
	/// float pb1 = -2 * t3 + 3 * t2;			// Ending point basis number.
	/// float mb0 = t3 - 2 * t2 + t;			// Starting tangent basis number.
	/// float mb1 = t3 - t2;					// Ending tangent basis number.
	/// 
	/// // With the basis calculated the formula now looks a little shorter:
	/// result = pb0 * p0 + mb0 * m0 + pb1 * p1 + mb1 * m1;
	/// </code>
	/// <para>
	/// This structure allows calculation and reuse of basis numbers since they only depend on the "time"
	/// argument.
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct HermiteBasis
	{
		#region Fields
		// The layout of the fields is the same as layout of the fields in spline::HermitBasis type that is
		// defined in CryEngine.

		/// <summary>
		/// Basis number for a starting point.
		/// </summary>
		public float StartingPointBasis;
		/// <summary>
		/// Basis number for an ending point.
		/// </summary>
		public float EndingPointBasis;
		/// <summary>
		/// Basis number for a starting tangent.
		/// </summary>
		public float StartingTangentBasis;
		/// <summary>
		/// Basis number for an ending tangent.
		/// </summary>
		public float EndingTangentBasis;
		#endregion
		#region Properties
		/// <summary>
		/// Extracts the time parameter out of the basis.
		/// </summary>
		/// <remarks>
		/// It is generally recommended to keep the parameter somewhere rather then extracting it.
		/// </remarks>
		public float Time
		{
			get
			{
				return
					(3 * this.EndingTangentBasis + this.EndingPointBasis)
					/
					(this.EndingPointBasis + 2 * this.EndingTangentBasis);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new Hermite basis.
		/// </summary>
		/// <remarks>
		/// The number will be clamped into range [0; 1] in Release mode. In Debug mode an error will
		/// occur.
		/// </remarks>
		/// <param name="t">A number between 0 and 1 that is used to calculate the basis numbers.</param>
		public HermiteBasis(float t)
		{
#if DEBUG
			Contract.Requires(t >= 0 && t <= 1, "The time parameter has to be within range from 0 to 1.");
#else
			t = t < 0 ? 0 : t > 1 ? 1 : t;
#endif
			if (t < MathHelpers.ZeroTolerance)
			{
				this.StartingPointBasis = 1;
				this.EndingPointBasis = 0;
				this.StartingTangentBasis = 0;
				this.EndingTangentBasis = 0;
			}
			else if (t - 1 < MathHelpers.ZeroTolerance)
			{
				this.StartingPointBasis = 0;
				this.EndingPointBasis = 1;
				this.StartingTangentBasis = 0;
				this.EndingTangentBasis = 0;
			}
			else
			{
				float t2 = t * t;
				float t3 = t2 * t;

				float t3_2 = t3 + t3;
				float t2_3 = 3 * t2;
				float t3_t2 = t3 - t2;
				this.StartingPointBasis = t3_2 - t2_3 + 1;
				this.EndingPointBasis = -t3_2 + t2_3;
				this.StartingTangentBasis = t3_t2 - t2 + t;
				this.EndingTangentBasis = t3_t2;
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Interpolates the point along the spline.
		/// </summary>
		/// <typeparam name="VectorType">
		/// Type of the vector that uses <see cref="float"/> to represent its components.
		/// </typeparam>
		/// <param name="p0">Starting point of the spline.</param>
		/// <param name="m0">Starting tangent of the spline.</param>
		/// <param name="p1">Ending point of the spline.</param>
		/// <param name="m1">Ending tangent of the spline.</param>
		/// <returns>
		/// A point along the described spline at the position represented by the <see cref="Time"/>
		/// parameter.
		/// </returns>
		public VectorType Interpolate<VectorType>(VectorType p0, VectorType m0, VectorType p1, VectorType m1)
			where VectorType : IVector<float, VectorType>
		{
			return p0
				.Scaled(this.StartingPointBasis)
				.Added(m0.Scaled(this.StartingTangentBasis))
				.Added(p1.Scaled(this.EndingPointBasis))
				.Added(m1.Scaled(this.EndingTangentBasis));
		}
		/// <summary>
		/// Interpolates the point along the spline.
		/// </summary>
		/// <typeparam name="VectorType">
		/// Type of the vector that uses <see cref="double"/> to represent its components.
		/// </typeparam>
		/// <param name="p0">Starting point of the spline.</param>
		/// <param name="m0">Starting tangent of the spline.</param>
		/// <param name="p1">Ending point of the spline.</param>
		/// <param name="m1">Ending tangent of the spline.</param>
		/// <returns>
		/// A point along the described spline at the position represented by the <see cref="Time"/>
		/// parameter.
		/// </returns>
		public VectorType InterpolatePrecise<VectorType>(VectorType p0, VectorType m0, VectorType p1, VectorType m1)
			where VectorType : IVector<double, VectorType>
		{
			return p0
				.Scaled(this.StartingPointBasis)
				.Added(m0.Scaled(this.StartingTangentBasis))
				.Added(p1.Scaled(this.EndingPointBasis))
				.Added(m1.Scaled(this.EndingTangentBasis));
		}
		#endregion
	}
}