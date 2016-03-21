using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CryCil.Geometry
{
	public partial class Rotation
	{
		/// <summary>
		/// Defines functions that work with rotations that use Euler angles applied in order Yaw-Pitch-Roll
		/// (Z-X-Y).
		/// </summary>
		public static class YPR
		{
			/// <summary>
			/// Creates 3x3 matrix that represents rotation that is defined by Euler angles.
			/// </summary>
			/// <param name="angles">A set of Euler angles.</param>
			/// <returns>3x3 matrix that represents the rotation.</returns>
			public static Matrix33 Create33(ref EulerAngles angles)
			{
				double sz, cz;
				MathHelpers.SinCos(angles.Pitch, out sz, out cz);
				double sx, cx;
				MathHelpers.SinCos(angles.Roll, out sx, out cx);
				double sy, cy;
				MathHelpers.SinCos(angles.Yaw, out sy, out cy);
				Matrix33 c = new Matrix33
				{
					M00 = (float)(cy * cz - sy * sz * sx),
					M01 = (float)(-sz * cx),
					M02 = (float)(sy * cz + cy * sz * sx),
					M10 = (float)(cy * sz + sy * sx * cz),
					M11 = (float)(cz * cx),
					M12 = (float)(sy * sz - cy * sx * cz),
					M20 = (float)(-sy * cx),
					M21 = (float)sx,
					M22 = (float)(cy * cx)
				};
				return c;
			}
			/// <summary>
			/// Creates a set Euler angles.
			/// </summary>
			/// <param name="m">3x3 matrix that represents a rotation.</param>
			/// <returns>
			/// A set of Euler angles that represents rotation that is represented by given matrix.
			/// </returns>
			public static EulerAngles CreateAngles(ref Matrix33 m)
			{
				Contract.Assert(m.IsOrthonormal, "Given matrix must be orthonormal to be treated like a rotation matrix.");

				float length = new Vector2(m.M01, m.M11).LengthSquared;

				if (length > MathHelpers.ZeroTolerance)
				{
					return new EulerAngles((float)Math.Atan2(-m.M01 / length, m.M11 / length),
										   (float)Math.Atan2(m.M21, length),
										   (float)Math.Atan2(-m.M20 / length, m.M22 / length));
				}
				return new EulerAngles(0, (float)Math.Atan2(m.M21, length), 0);
			}
			/// <summary>
			/// Creates a set Euler angles.
			/// </summary>
			/// <param name="viewDirection">Forward direction.</param>
			/// <param name="roll">         A roll value.</param>
			/// <returns>
			/// A set of Euler angles that represents rotation that is represented by given vector and roll
			/// value.
			/// </returns>
			public static EulerAngles CreateAngles(Vector3 viewDirection, float roll)
			{
				Contract.Assert(viewDirection.IsUnit(0.001f), "Given vector must be normalized.");

				float length = new Vector2(viewDirection.X, viewDirection.Y).LengthSquared;

				if (length > MathHelpers.ZeroTolerance)
				{
					return new EulerAngles((float)Math.Atan2(-viewDirection.X / length, viewDirection.Y / length),
										   (float)Math.Atan2(viewDirection.Z, length),
										   roll);
				}
				return new EulerAngles(0, (float)Math.Atan2(viewDirection.Z, length), roll);
			}
			/// <summary>
			/// Calculates view direction of the entity which orientation is represented by Euler angles.
			/// </summary>
			/// <param name="angles">A set of Euler angles that represent the orientation.</param>
			/// <returns>A <see cref="Vector3"/> object that represents the view direction.</returns>
			public static Vector3 CreateViewDirection(ref EulerAngles angles)
			{
				angles.NormalizePI();

				float sz, cz;
				MathHelpers.SinCos(angles.Pitch, out sz, out cz); // Yaw.
				float sx, cx;
				MathHelpers.SinCos(angles.Roll, out sx, out cx);
				// Pitch. Yes, they are switched up because of different order of rotations.

				return new Vector3(-sz * cx, cz * cx, sx);
			}
		}
	}
}