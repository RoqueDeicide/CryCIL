using System;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a vector which length is an angle of rotation and its normalized version is an axis of
	/// rotation.
	/// </summary>
	public struct AngleAxis
	{
		#region Fields
		/// <summary>
		/// <see cref="Vector3"/> which normalized version can be used as axis of rotation and which length
		/// represents angle of rotation. Use this field if other two properties are too costly
		/// performance-wise.
		/// </summary>
		public Vector3 Vector;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets axis of rotation.
		/// </summary>
		public Vector3 Axis
		{
			get { return this.Vector.Normalized; }
			set { this.Vector = value.Normalized * this.Vector.Length; }
		}
		/// <summary>
		/// Gets or sets angle of rotation.
		/// </summary>
		public float Angle
		{
			get { return this.Vector.Length; }
			set
			{
				this.Vector.Normalize();
				Scale.Apply(ref this.Vector, value);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of type <see cref="AngleAxis"/>.
		/// </summary>
		/// <param name="axis"> Axis of rotation.</param>
		/// <param name="angle">Angle of rotation in radians.</param>
		public AngleAxis(Vector3 axis, float angle)
		{
			this.Vector = axis.Normalized * angle;
		}
		/// <summary>
		/// Creates new instance of type <see cref="AngleAxis"/>.
		/// </summary>
		/// <param name="angles">
		/// A set of Euler angles that represents rotation that new instance needs to represent.
		/// </param>
		public AngleAxis(EulerAngles angles)
		{
			float sx, cx;
			MathHelpers.SinCos(angles.Pitch * 0.5f, out sx, out cx);
			float sy, cy;
			MathHelpers.SinCos(angles.Roll * 0.5f, out sy, out cy);
			float sz, cz;
			MathHelpers.SinCos(angles.Yaw * 0.5f, out sz, out cz);

			float w = cx * cy * cz + sx * sy * sz;
			if (Math.Abs(w - 1) < MathHelpers.ZeroTolerance)
			{
				this.Vector = new Vector3();
			}
			else
			{
				this.Vector = new Vector3(cz * cy * sx - sz * sy * cx,
										  cz * sy * cx + sz * cy * sx,
										  sz * cy * cx - cz * sy * sx
					) * (float)(Math.Acos(w) * 2);
			}
		}
		/// <summary>
		/// Creates new instance of type <see cref="AngleAxis"/>.
		/// </summary>
		/// <param name="quaternion">
		/// <see cref="Quaternion"/> that represents rotation that new instance needs to represent.
		/// </param>
		public AngleAxis(Quaternion quaternion)
		{
			if (Math.Abs(quaternion.W - 1) < MathHelpers.ZeroTolerance)
			{
				this.Vector = new Vector3();
			}
			else
			{
				this.Vector = quaternion.Vector.Normalized * (float)(Math.Acos(quaternion.W) * 2);
			}
		}
		#endregion
	}
}