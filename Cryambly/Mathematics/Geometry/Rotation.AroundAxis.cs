namespace CryCil.Geometry
{
	public partial class Rotation
	{
		/// <summary>
		/// Defines functions for working with rotations around custom axes.
		/// </summary>
		public static class AroundAxis
		{
			/// <summary>
			/// Rotates given vector around specified axis.
			/// </summary>
			/// <param name="vector">   Vector to rotate.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Apply(ref Vector3 vector, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Apply(ref vector, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Rotates given vector around specified axis.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation.</param>
			public static void Apply(ref Vector3 vector, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				Apply(ref vector, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Rotates given vector around specified axis.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void Apply(ref Vector3 vector, ref Vector3 axis, float sine, float cosine)
			{
				double ic = 1 - cosine;
				vector = new Vector3
					(
					(float)
						(
							(ic * axis.X * axis.X + cosine) * vector.X
							+
							(ic * axis.X * axis.Y - axis.Z * sine) * vector.Y
							+
							(ic * axis.X * axis.Z + axis.Y * sine) * vector.Z
							),
					(float)
						(
							(ic * axis.Y * axis.X + axis.Z * sine) * vector.X
							+
							(ic * axis.Y * axis.Y + cosine) * vector.Y
							+
							(ic * axis.Y * axis.Z - axis.X * sine) * vector.Z
							),
					(float)
						(
							(ic * axis.Z * axis.X - axis.Y * sine) * vector.X
							+
							(ic * axis.Z * axis.Y + axis.X * sine) * vector.Y
							+
							(ic * axis.Z * axis.Z + cosine) * vector.Z
							)
					);
			}
			/// <summary>
			/// Rotates given vector around a given axis using specified pivot.
			/// </summary>
			/// <param name="vector">   Vector to rotate.</param>
			/// <param name="pivot">    Point to rotate the vector around.</param>
			/// <param name="angleAxis">Angle and axis of rotation.</param>
			public static void Apply(ref Vector3 vector, ref Vector3 pivot, ref AngleAxis angleAxis)
			{
				// Move point of origin to the pivot.
				Vector3 originToPivot = -pivot;
				Translation.Apply(ref vector, ref originToPivot);
				// Rotate the vector.
				Apply(ref vector, ref angleAxis);
				// Return to original start point for coordinates.
				Translation.Apply(ref vector, ref pivot);
			}
			/// <summary>
			/// Rotates given vector around a given axis using specified pivot.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="pivot"> Point to rotate the vector around.</param>
			/// <param name="axis">  Axis of rotation.</param>
			/// <param name="angle"> Angle of rotation.</param>
			public static void Apply(ref Vector3 vector, ref Vector3 pivot, ref Vector3 axis, float angle)
			{
				// Move point of origin to the pivot.
				Vector3 originToPivot = -pivot;
				Translation.Apply(ref vector, ref originToPivot);
				// Rotate the vector.
				Apply(ref vector, ref axis, angle);
				// Return to original start point for coordinates.
				Translation.Apply(ref vector, ref pivot);
			}
			/// <summary>
			/// Rotates given vector around a given axis using specified pivot.
			/// </summary>
			/// <param name="vector">Vector to rotate.</param>
			/// <param name="pivot"> Point to rotate the vector around.</param>
			/// <param name="axis">  Axis of rotation.</param>
			/// <param name="sine">  A sine of angle of rotation.</param>
			/// <param name="cosine">A cosine of angle of rotation.</param>
			public static void Apply(ref Vector3 vector, ref Vector3 pivot, ref Vector3 axis, float sine, float cosine)
			{
				// Move point of origin to the pivot.
				Vector3 originToPivot = -pivot;
				Translation.Apply(ref vector, ref originToPivot);
				// Rotate the vector.
				Apply(ref vector, ref axis, sine, cosine);
				// Return to original start point for coordinates.
				Translation.Apply(ref vector, ref pivot);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Override(out Matrix33 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Override(out matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix33 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				Override(out matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void Override(out Matrix33 matrix, ref Vector3 axis, float sine, float cosine)
			{
				matrix = Create33(ref axis, sine, cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Override(out Matrix34 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Override(out matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix34 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				Override(out matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void Override(out Matrix34 matrix, ref Vector3 axis, float sine, float cosine)
			{
				matrix = Create34(ref axis, sine, cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Override(out Matrix44 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Override(out matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Override(out Matrix44 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				Override(out matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given matrix with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void Override(out Matrix44 matrix, ref Vector3 axis, float sine, float cosine)
			{
				matrix = Create44(ref axis, sine, cosine);
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="angleAxis"> 
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Override(out Quaternion quaternion, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle / 2, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Override(out quaternion, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="axis">      Normalized vector that represents axis of rotation.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void Override(out Quaternion quaternion, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle / 2, out sine, out cosine);

				Override(out quaternion, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Overrides given quaternion with one that represents rotation around specified axis.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="axis">      Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">      Sine of half-angle of rotation.</param>
			/// <param name="cosine">    Cosine of half-angle of rotation.</param>
			public static void Override(out Quaternion quaternion, ref Vector3 axis, float sine, float cosine)
			{
				quaternion = CreateQuaternion(ref axis, sine, cosine);
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Set(ref Matrix33 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;

				Set(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix33 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				Set(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void Set(ref Matrix33 matrix, ref Vector3 axis, float sine, float cosine)
			{
				float s = sine;
				float c = cosine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Set(ref Matrix34 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Set(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix34 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				Set(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void Set(ref Matrix34 matrix, ref Vector3 axis, float sine, float cosine)
			{
				float s = sine;
				float c = cosine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Set(ref Matrix44 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Set(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void Set(ref Matrix44 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				Set(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given matrix so it represents rotation around specified axis.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void Set(ref Matrix44 matrix, ref Vector3 axis, float sine, float cosine)
			{
				float s = sine;
				float c = cosine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;
			}
			/// <summary>
			/// Modifies given quaternion so it represents rotation around specified axis.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="angleAxis"> 
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void Set(ref Quaternion quaternion, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle / 2, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				Set(ref quaternion, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given quaternion so it represents rotation around specified axis.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="axis">      Normalized vector that represents axis of rotation.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void Set(ref Quaternion quaternion, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle / 2, out sine, out cosine);

				Set(ref quaternion, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Modifies given quaternion so it represents rotation around specified axis.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="axis">      Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">      Sine of half-angle of rotation.</param>
			/// <param name="cosine">    Cosine of half-angle of rotation.</param>
			public static void Set(ref Quaternion quaternion, ref Vector3 axis, float sine, float cosine)
			{
				quaternion.Vector = axis * sine;
				quaternion.W = cosine;
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void StackUp(ref Matrix33 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;

				StackUp(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix33 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				StackUp(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void StackUp(ref Matrix33 matrix, ref Vector3 axis, float sine, float cosine)
			{
				Matrix33 temp = matrix;

				float s = sine;
				float c = cosine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;

				matrix *= temp;
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void StackUp(ref Matrix34 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;

				StackUp(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix34 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				StackUp(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void StackUp(ref Matrix34 matrix, ref Vector3 axis, float sine, float cosine)
			{
				Matrix34 temp = matrix;

				float s = sine;
				float c = cosine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;

				matrix *= temp;
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">   Matrix to modify.</param>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void StackUp(ref Matrix44 matrix, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;

				StackUp(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="angle"> Angle of rotation in radians.</param>
			public static void StackUp(ref Matrix44 matrix, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				StackUp(ref matrix, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given matrix.
			/// </summary>
			/// <param name="matrix">Matrix to modify.</param>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			public static void StackUp(ref Matrix44 matrix, ref Vector3 axis, float sine, float cosine)
			{
				Matrix44 temp = matrix;

				float s = sine;
				float c = cosine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;

				matrix *= temp;
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given quaternion.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="angleAxis"> 
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			public static void StackUp(ref Quaternion quaternion, ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle / 2, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;

				StackUp(ref quaternion, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given quaternion.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="axis">      Normalized vector that represents axis of rotation.</param>
			/// <param name="angle">     Angle of rotation in radians.</param>
			public static void StackUp(ref Quaternion quaternion, ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle / 2, out sine, out cosine);

				StackUp(ref quaternion, ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Stacks a rotation around specified axis onto given quaternion.
			/// </summary>
			/// <param name="quaternion">Quaternion to modify.</param>
			/// <param name="axis">      Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">      Sine of half-angle of rotation.</param>
			/// <param name="cosine">    Cosine of half-angle of rotation.</param>
			public static void StackUp(ref Quaternion quaternion, ref Vector3 axis, float sine, float cosine)
			{
				quaternion = CreateQuaternion(ref axis, sine, cosine) * quaternion;
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix33 Create33(ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;

				return Create33(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis"> Normalized vector that represents axis of rotation.</param>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix33 Create33(ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				return Create33(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix33 Create33(ref Vector3 axis, float sine, float cosine)
			{
				Matrix33 matrix = Matrix33.Identity;

				float c = cosine;
				float s = sine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;

				return matrix;
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix34 Create34(ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				return Create34(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis"> Normalized vector that represents axis of rotation.</param>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix34 Create34(ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				return Create34(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix34 Create34(ref Vector3 axis, float sine, float cosine)
			{
				Matrix34 matrix = Matrix34.Identity;

				float c = cosine;
				float s = sine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;

				return matrix;
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix44 Create44(ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;
				return Create44(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis"> Normalized vector that represents axis of rotation.</param>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix44 Create44(ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle, out sine, out cosine);

				return Create44(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a matrix that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of angle of rotation.</param>
			/// <param name="cosine">Cosine of angle of rotation.</param>
			/// <returns>Matrix that represents rotation around specified axis.</returns>
			public static Matrix44 Create44(ref Vector3 axis, float sine, float cosine)
			{
				Matrix44 matrix = Matrix44.Identity;

				float c = cosine;
				float s = sine;
				float mc = 1 - c;
				matrix.M00 = mc * axis.X * axis.X + c;
				matrix.M01 = mc * axis.X * axis.Y - axis.Z * s;
				matrix.M02 = mc * axis.X * axis.Z + axis.Y * s;
				matrix.M10 = mc * axis.Y * axis.X + axis.Z * s;
				matrix.M11 = mc * axis.Y * axis.Y + c;
				matrix.M12 = mc * axis.Y * axis.Z - axis.X * s;
				matrix.M20 = mc * axis.Z * axis.X - axis.Y * s;
				matrix.M21 = mc * axis.Z * axis.Y + axis.X * s;
				matrix.M22 = mc * axis.Z * axis.Z + c;

				return matrix;
			}
			/// <summary>
			/// Creates a quaternion that represents rotation around specified axis.
			/// </summary>
			/// <param name="angleAxis">
			/// <see cref="AngleAxis"/> that represents axis and angle of rotation.
			/// </param>
			/// <returns>Quaternion that represents rotation around specified axis.</returns>
			public static Quaternion CreateQuaternion(ref AngleAxis angleAxis)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angleAxis.Angle / 2, out sine, out cosine);
				Vector3 axis = angleAxis.Axis;

				return CreateQuaternion(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a quaternion that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis"> Normalized vector that represents axis of rotation.</param>
			/// <param name="angle">Angle of rotation in radians.</param>
			/// <returns>Quaternion that represents rotation around specified axis.</returns>
			public static Quaternion CreateQuaternion(ref Vector3 axis, float angle)
			{
				double sine;
				double cosine;
				MathHelpers.SinCos(angle / 2, out sine, out cosine);

				return CreateQuaternion(ref axis, (float)sine, (float)cosine);
			}
			/// <summary>
			/// Creates a quaternion that represents rotation around specified axis.
			/// </summary>
			/// <param name="axis">  Normalized vector that represents axis of rotation.</param>
			/// <param name="sine">  Sine of half-angle of rotation.</param>
			/// <param name="cosine">Cosine of half-angle of rotation.</param>
			/// <returns>Quaternion that represents rotation around specified axis.</returns>
			public static Quaternion CreateQuaternion(ref Vector3 axis, float sine, float cosine)
			{
				return new Quaternion
					(
					axis * sine,
					cosine
					);
			}
		}
	}
}