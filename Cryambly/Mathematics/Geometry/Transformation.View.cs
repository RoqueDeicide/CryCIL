using System;
using System.Linq;

namespace CryCil.Geometry
{
	public static partial class Transformation
	{
		/// <summary>
		/// Defines functions that assist with projections.
		/// </summary>
		public static class View
		{
			/// <summary>
			/// Creates a look-at transformation matrix.
			/// </summary>
			/// <param name="eye">Position of the eye(camera's origin) in world space.</param>
			/// <param name="at"> Position of the point the eye is looking at in world space.</param>
			/// <param name="up"> A global Up vector.</param>
			/// <returns>A look-at transformation matrix.</returns>
			/// <overloads>
			/// <summary>Creates a look-at transformation matrix that transforms vector from world space to
			/// camera space.</summary><remarks>When global Up vector is not specified, then default
			/// CryEngine Up vector will be used (it is defined in <see cref="Vector3.Up"/>).</remarks>
			/// </overloads>
			public static Matrix44 Create(Vector3 eye, Vector3 at, Vector3 up)
			{
				// Z-axis looks away from the camera in camera space.
				Vector3 zaxis = (at - eye).Normalized;
				// X-axis points to the left from the camera (this is why we need an Up vector).
				Vector3 xaxis = (up % zaxis).Normalized;
				// Y-axis points upwards from the camera.
				Vector3 yaxis = xaxis % zaxis;

				// For nicer formatting of the return statement.
				float xaxisnd = -xaxis * eye;
				float yaxisnd = -yaxis * eye;
				float zaxisnd = -zaxis * eye;

				return new Matrix44(xaxis.X, yaxis.X, zaxis.X, 0,
									xaxis.Y, yaxis.Y, zaxis.Y, 0,
									xaxis.Z, yaxis.Z, zaxis.Z, 0,
									xaxisnd, yaxisnd, zaxisnd, 1);
			}
			/// <summary>
			/// Creates a look-at transformation matrix.
			/// </summary>
			/// <param name="eye">Position of the eye(camera's origin) in world space.</param>
			/// <param name="at"> Position of the point the eye is looking at in world space.</param>
			/// <returns>A look-at transformation matrix.</returns>
			public static Matrix44 Create(Vector3 eye, Vector3 at)
			{
				// Z-axis looks away from the camera in camera space.
				Vector3 zaxis = (at - eye).Normalized;
				// X-axis points to the left from the camera (this is why we need an Up vector).
				Vector3 xaxis = new Vector3(-zaxis.Y, zaxis.X, 0);
				// This is a cross-product of the default Up vector and a Z-axis that we calculated above.
				// Y-axis points upwards from the camera.
				Vector3 yaxis = xaxis % zaxis;

				// For nicer formatting of the return statement.
				float xaxisnd = -xaxis * eye;
				float yaxisnd = -yaxis * eye;
				float zaxisnd = -zaxis * eye;

				return new Matrix44(xaxis.X, yaxis.X, zaxis.X, 0,
									xaxis.Y, yaxis.Y, zaxis.Y, 0,
									xaxis.Z, yaxis.Z, zaxis.Z, 0,
									xaxisnd, yaxisnd, zaxisnd, 1);
			}
		}
	}
}