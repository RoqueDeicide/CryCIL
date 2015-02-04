namespace CryCil.Geometry
{
	/// <summary>
	/// Provides functionality for reflecting stuff off the surfaces.
	/// </summary>
	public static class Reflection
	{
		/// <summary>
		/// Provides functionality for mirroring vectors off the surfaces.
		/// </summary>
		/// <remarks>
		/// <para>Mirroring formula is:</para>
		/// <para>2*(V dot N)*N - V, where</para>
		/// <para>V - vector and N - normal to the surface.</para>
		/// </remarks>
		public static class Mirror
		{
			/// <summary>
			/// Creates a vector that is a mirror reflection of another vector against a
			/// surface.
			/// </summary>
			/// <param name="vector">Vector that needs to be reflected.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			/// <returns>A new reflected vector.</returns>
			public static Vector2 Apply(Vector2 vector, Vector2 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y;
				return new Vector2(2.0f * dot * normal.X - vector.X, 2.0f * dot * normal.Y - vector.Y);
			}
			/// <summary>
			/// Mirrors given vector over a given surface.
			/// </summary>
			/// <param name="vector">Vector to reflect.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			public static void Apply(ref Vector2 vector, Vector2 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y;
				vector.X = 2.0f * dot * normal.X - vector.X;
				vector.Y = 2.0f * dot * normal.Y - vector.Y;
			}
			/// <summary>
			/// Creates a vector that is a mirror reflection of another vector against a
			/// surface.
			/// </summary>
			/// <param name="vector">Vector that needs to be reflected.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			/// <returns>A new reflected vector.</returns>
			public static Vector3 Apply(Vector3 vector, Vector3 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y;
				return
					new Vector3
					(
						2.0f * dot * normal.X - vector.X,
						2.0f * dot * normal.Y - vector.Y,
						2.0f * dot * normal.Z - vector.Z
					);
			}
			/// <summary>
			/// Mirrors given vector over a given surface.
			/// </summary>
			/// <param name="vector">Vector to reflect.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			public static void Apply(ref Vector3 vector, Vector3 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y;
				vector.X = 2.0f * dot * normal.X - vector.X;
				vector.Y = 2.0f * dot * normal.Y - vector.Y;
				vector.Z = 2.0f * dot * normal.Z - vector.Z;
			}
		}
		/// <summary>
		/// Provides functionality for mirroring vectors off the surfaces.
		/// </summary>
		/// <remarks>
		/// <para>Mirroring formula is:</para>
		/// <para>V - 2*(V dot N)*N, where</para>
		/// <para>V - vector and N - normal to the surface.</para>
		/// </remarks>
		public static class Bounce
		{
			/// <summary>
			/// Creates a vector that is a bouncing reflection of another vector against a surface.
			/// </summary>
			/// <param name="vector">Vector that needs to be reflected.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			/// <returns>A new reflected vector.</returns>
			public static Vector2 Apply(Vector2 vector, Vector2 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y;
				return new Vector2(vector.X - 2.0f * dot * normal.X, vector.Y - 2.0f * dot * normal.Y);
			}
			/// <summary>
			/// Bounces given vector over a given surface.
			/// </summary>
			/// <param name="vector">Vector to reflect.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			public static void Apply(ref Vector2 vector, Vector2 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y;
				vector.X = vector.X - 2.0f * dot * normal.X;
				vector.Y = vector.Y - 2.0f * dot * normal.Y;
			}
			/// <summary>
			/// Creates a vector that is a bouncing reflection of another vector against a surface.
			/// </summary>
			/// <param name="vector">Vector that needs to be reflected.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			/// <returns>A new reflected vector.</returns>
			public static Vector3 Apply(Vector3 vector, Vector3 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
				return
					new Vector3
					(
						vector.X - 2.0f * dot * normal.X,
						vector.Y - 2.0f * dot * normal.Y,
						vector.Z - 2.0f * dot * normal.Z
					);
			}
			/// <summary>
			/// Bounces given vector over a given surface.
			/// </summary>
			/// <param name="vector">Vector to reflect.</param>
			/// <param name="normal">Normal vector to the surface.</param>
			public static void Apply(ref Vector3 vector, Vector3 normal)
			{
				float dot = vector.X * normal.X + vector.Y * normal.Y;
				vector.X = vector.X - 2.0f * dot * normal.X;
				vector.Y = vector.Y - 2.0f * dot * normal.Y;
				vector.Z = vector.Z - 2.0f * dot * normal.Z;
			}
		}
	}
}