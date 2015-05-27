using System;

namespace CryCil.Geometry
{
	public static partial class Transformation
	{
		/// <summary>
		/// Defines functions that assist with projections.
		/// </summary>
		public static class Projection
		{
			/// <summary>
			/// Creates a projection transformation matrix.
			/// </summary>
			/// <param name="horizontalFov">    Horizontal field of view in radians.</param>
			/// <param name="verticalFov">      Vertical field of view in radians.</param>
			/// <param name="nearPlaneDistance">Depth value of the near clipping plane.</param>
			/// <param name="farPlaneDistance"> Depth value of the far clipping plane.</param>
			/// <returns>
			/// An instance of type <see cref="Matrix44"/> that represents projection transformation.
			/// </returns>
			/// <overloads>
			/// <summary>
			/// <para>
			/// All overloads of this method create projection transformation matrices using various
			/// parameters.
			/// </para>
			/// <para>
			/// To apply the transformation to the vector use
			/// <see cref="Transformation.Apply(ref Vector4,ref Matrix44)"/> function after applying the
			/// view transformation using a matrix created by <see cref="o:Transformation.View.Create()"/>.
			/// The view transformation is needed because all provided arguments are only valid in camera
			/// space.
			/// </para></summary>
			/// </overloads>
			public static Matrix44 Create(float horizontalFov, float verticalFov, float nearPlaneDistance,
										  float farPlaneDistance)
			{
				float w = (float)(1.0f / Math.Tan(horizontalFov / 2.0f));
				float h = (float)(1.0f / Math.Tan(verticalFov / 2.0f));
				float q = farPlaneDistance / (farPlaneDistance - nearPlaneDistance);
				float p = -q * nearPlaneDistance;

				return new Matrix44
				(
					w, 0, 0, 0,
					0, h, 0, 0,
					0, 0, q, 1,
					0, 0, p, 0
				);
			}
			/// <summary>
			/// Creates a projection transformation matrix.
			/// </summary>
			/// <param name="width">            Width of the camera surface at near plane.</param>
			/// <param name="height">           Height of the camera surface at near plane.</param>
			/// <param name="nearPlaneDistance">Depth value of the near clipping plane.</param>
			/// <param name="farPlaneDistance"> Depth value of the far clipping plane.</param>
			/// <param name="holder">           Differenciates the overload from others. Ignored.</param>
			/// <returns>
			/// An instance of type <see cref="Matrix44"/> that represents projection transformation.
			/// </returns>
			public static Matrix44 Create(float width, float height, float nearPlaneDistance,
										  float farPlaneDistance, bool holder)
			{
				float w = 2 * nearPlaneDistance * width;
				float h = 2 * nearPlaneDistance * height;
				float q = farPlaneDistance / (farPlaneDistance - nearPlaneDistance);
				float p = -q * nearPlaneDistance;

				return new Matrix44
				(
					w, 0, 0, 0,
					0, h, 0, 0,
					0, 0, q, 1,
					0, 0, p, 0
				);
			}
			/// <summary>
			/// Creates a projection transformation matrix.
			/// </summary>
			/// <param name="left">  Minimal X-coordinate of the view volume.</param>
			/// <param name="right"> Maximal X-coordinate of the view volume.</param>
			/// <param name="bottom">Minimal Y-coordinate of the view volume.</param>
			/// <param name="top">   Maximal Y-coordinate of the view volume.</param>
			/// <param name="near">  Minimal Z-coordinate of the view volume.</param>
			/// <param name="far">   Maximal Z-coordinate of the view volume.</param>
			/// <returns>
			/// An instance of type <see cref="Matrix44"/> that represents projection transformation.
			/// </returns>
			public static Matrix44 Create(float left, float right, float bottom, float top, float near,
										  float far)
			{
				float w = 2 * near / (right - left);
				float a = (left + right) / (left - right);

				float h = 2 * near / (top - bottom);
				float b = (bottom + top) / (bottom - top);

				float q = far / (far - near);
				float p = -q * near;

				return new Matrix44
				(
					w, 0, 0, 0,
					0, h, 0, 0,
					a, b, q, 1,
					0, 0, p, 0
				);
			}
		}
	}
}