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
			/// Parameter that determines position of resultant vertex on the line that
			/// goes through the first and second vertex relative to the first one.
			/// </param>
			public static void Apply(out FullVertex result, FullVertex first, FullVertex second, float parameter)
			{
				result = new FullVertex
				{
					Position = Interpolation.Linear.Create(first.Position, second.Position, parameter),
					Normal = Interpolation.Linear.Create(first.Normal, second.Normal, parameter),
					UvPosition = Interpolation.Linear.Create(first.UvPosition, second.UvPosition, parameter),
					PrimaryColor = Interpolation.Linear.Create(first.PrimaryColor, second.PrimaryColor, parameter),
					SecondaryColor = Interpolation.Linear.Create(first.SecondaryColor, second.SecondaryColor, parameter)
				};
			}
			/// <summary>
			/// Creates a new vertex that is a result of interpolation.
			/// </summary>
			/// <param name="first">    First vertex.</param>
			/// <param name="second">   Second vertex.</param>
			/// <param name="parameter">
			/// Parameter that determines position of resultant vertex on the line that
			/// goes through the first and second vertex relative to the first one.
			/// </param>
			/// <returns>Result of interpolation.</returns>
			public static FullVertex Create(FullVertex first, FullVertex second, float parameter)
			{
				return new FullVertex
				{
					Position = Interpolation.Linear.Create(first.Position, second.Position, parameter),
					Normal = Interpolation.Linear.Create(first.Normal, second.Normal, parameter),
					UvPosition = Interpolation.Linear.Create(first.UvPosition, second.UvPosition, parameter),
					PrimaryColor = Interpolation.Linear.Create(first.PrimaryColor, second.PrimaryColor, parameter),
					SecondaryColor = Interpolation.Linear.Create(first.SecondaryColor, second.SecondaryColor, parameter)
				};
			}
		}
	}
}