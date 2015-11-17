using System;
using CryCil.Geometry;

namespace CryCil
{
	public static partial class Interpolation
	{
		public static partial class Linear
		{
			/// <summary>
			/// Applies linear interpolation to the vertex.
			/// </summary>
			/// <param name="result">   Result of interpolation.</param>
			/// <param name="first">    First vertex.</param>
			/// <param name="second">   Second vertex.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vertex on the line that goes through the
			/// first and second vertex relative to the first one.
			/// </param>
			public static void Apply(out FullVertex result, FullVertex first, FullVertex second, float parameter)
			{
				result = new FullVertex
				{
					Position = Create(first.Position, second.Position, parameter),
					Normal = Create(first.Normal, second.Normal, parameter),
					UvPosition = Create(first.UvPosition, second.UvPosition, parameter),
					PrimaryColor = Create(first.PrimaryColor, second.PrimaryColor, parameter),
					SecondaryColor = Create(first.SecondaryColor, second.SecondaryColor, parameter)
				};
			}
			/// <summary>
			/// Creates a new vertex that is a result of interpolation.
			/// </summary>
			/// <param name="first">    First vertex.</param>
			/// <param name="second">   Second vertex.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vertex on the line that goes through the
			/// first and second vertex relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static FullVertex Create(FullVertex first, FullVertex second, float parameter)
			{
				return new FullVertex
				{
					Position = Create(first.Position, second.Position, parameter),
					Normal = Create(first.Normal, second.Normal, parameter),
					UvPosition = Create(first.UvPosition, second.UvPosition, parameter),
					PrimaryColor = Create(first.PrimaryColor, second.PrimaryColor, parameter),
					SecondaryColor = Create(first.SecondaryColor, second.SecondaryColor, parameter)
				};
			}
		}
	}
}