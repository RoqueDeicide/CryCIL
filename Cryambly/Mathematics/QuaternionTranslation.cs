using System;

namespace CryCil.Mathematics
{
	/// <summary>
	/// Quaternion with a translation vector.
	/// </summary>
	public struct QuaternionTranslation
	{
		/// <summary>
		/// The quaternion
		/// </summary>
		public Quaternion Q;
		/// <summary>
		/// The translation vector and a scalar (for uniform scaling?)
		/// </summary>
		public Vector3 T;
		/// <summary>
		/// Creates new instance of type <see cref="QuaternionTranslation"/>.
		/// </summary>
		/// <param name="t"><see cref="Vector3"/> that represents translation.</param>
		/// <param name="q"><see cref="Quaternion"/> that represents orientation.</param>
		public QuaternionTranslation(Vector3 t, Quaternion q)
		{
			this.Q = q;
			this.T = t;
		}
		/// <summary>
		/// Creates new instance of type <see cref="QuaternionTranslation"/>.
		/// </summary>
		/// <param name="m">
		/// <see cref="Matrix34"/> that represents both translation and orientation.
		/// </param>
		public QuaternionTranslation(Matrix34 m)
		{
			this.Q = new Quaternion(m);
			this.T = m.Translation;
		}
		/// <summary>
		/// Resets this instance into identity state.
		/// </summary>
		public void SetIdentity()
		{
			this = Identity;
		}
		/// <summary>
		/// Sets this instance to be rotation around XYZ axes represented by a vector.
		/// </summary>
		/// <param name="rad">  Euler angles measured in radians.</param>
		/// <param name="trans">Optional translation vector.</param>
		public void SetRotationXYZ(Vector3 rad, Vector3? trans = null)
		{
			this.Q.SetRotationAroundXYZAxes(rad);

			this.T = trans.GetValueOrDefault();
		}
		/// <summary>
		/// Sets this instance to be rotation around XYZ axes represented by a vector.
		/// </summary>
		/// <param name="cosha">Cosine of angle of rotation around axis.</param>
		/// <param name="sinha">Sine of angle of rotation around axis.</param>
		/// <param name="axis"> Axis of rotation.</param>
		/// <param name="trans"></param>
		public void SetRotationAA(float cosha, float sinha, Vector3 axis, Vector3? trans = null)
		{
			this.Q.SetRotationAngleAxis(cosha, sinha, axis);
			this.T = trans.GetValueOrDefault();
		}
		/// <summary>
		/// Inverts rotation and translation this object represents.
		/// </summary>
		public void Invert()
		{
			this.T = -this.T * this.Q;
			this.Q = !this.Q;
		}
		/// <summary>
		/// Determines whether this instance can be considered equal to another.
		/// </summary>
		/// <param name="p">      Another object.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>True, if this instance can be considered equal to another.</returns>
		public bool IsEquivalent(QuaternionTranslation p, float epsilon = 0.05f)
		{
			var q0 = p.Q;
			var q1 = -p.Q;
			bool t0 = (Math.Abs(this.Q.V.X - q0.V.X) <= epsilon) && (Math.Abs(this.Q.V.Y - q0.V.Y) <= epsilon) && (Math.Abs(this.Q.V.Z - q0.V.Z) <= epsilon) && (Math.Abs(this.Q.W - q0.W) <= epsilon);
			bool t1 = (Math.Abs(this.Q.V.X - q1.V.X) <= epsilon) && (Math.Abs(this.Q.V.Y - q1.V.Y) <= epsilon) && (Math.Abs(this.Q.V.Z - q1.V.Z) <= epsilon) && (Math.Abs(this.Q.W - q1.W) <= epsilon);
			return ((t0 | t1) && (Math.Abs(this.T.X - p.T.X) <= epsilon) && (Math.Abs(this.T.Y - p.T.Y) <= epsilon) && (Math.Abs(this.T.Z - p.T.Z) <= epsilon));
		}
		/// <summary>
		/// Checks whether the transformation is valid.
		/// </summary>
		/// <returns></returns>
		public bool IsValid
		{
			get
			{
				return this.T.IsValid && this.Q.IsValid;
			}
		}
		/// <summary>
		/// Calculates normalized linear interpolation.
		/// </summary>
		/// <param name="start"> Starting point.</param>
		/// <param name="end">   Ending point.</param>
		/// <param name="amount">Amount of interpolation.</param>
		public void NormalizedLinearInterpolation(QuaternionTranslation start, QuaternionTranslation end, float amount)
		{
			var d = end.Q;
			if ((start.Q | d) < 0) { d = -d; }

			var vDiff = d.V - start.Q.V;

			this.Q.V = start.Q.V + (vDiff * amount);
			this.Q.W = start.Q.W + ((d.W - start.Q.W) * amount);

			this.Q.Normalize();

			vDiff = end.T - start.T;
			this.T = start.T + (vDiff * amount);
		}
		/// <summary>
		/// </summary>
		/// <param name="vx"> </param>
		/// <param name="vy"> </param>
		/// <param name="vz"> </param>
		/// <param name="pos"></param>
		public void SetFromVectors(Vector3 vx, Vector3 vy, Vector3 vz, Vector3 pos)
		{
			var m34 = new Matrix34
			{
				M00 = vx.X,
				M01 = vy.X,
				M02 = vz.X,
				M03 = pos.X,
				M10 = vx.Y,
				M11 = vy.Y,
				M12 = vz.Y,
				M13 = pos.Y,
				M20 = vx.Z,
				M21 = vy.Z,
				M22 = vz.Z,
				M23 = pos.Z
			};
			this = new QuaternionTranslation(m34);
		}
		/// <summary>
		/// </summary>
		/// <param name="maxLength"></param>
		/// <param name="maxAngle"> </param>
		public void ClampLengthAngle(float maxLength, float maxAngle)
		{
			this.T.ClampLength(maxLength);
			this.Q.ClampAngle(maxAngle);
		}
		/// <summary>
		/// </summary>
		/// <param name="scale"></param>
		/// <returns></returns>
		public QuaternionTranslation GetScaled(float scale)
		{
			return new QuaternionTranslation(this.T * scale, this.Q.GetScaled(scale));
		}
		/// <summary>
		/// </summary>
		public bool IsIdentity { get { return this.Q.IsIdentity && this.T.IsZero(); } }
		/// <summary>
		/// </summary>
		public QuaternionTranslation Inverted
		{
			get
			{
				var quatT = this;
				quatT.Invert();
				return quatT;
			}
		}
		/// <summary>
		/// </summary>
		public Vector3 Column0 { get { return this.Q.Column0; } }
		/// <summary>
		/// </summary>
		public Vector3 Column1 { get { return this.Q.Column1; } }
		/// <summary>
		/// </summary>
		public Vector3 Column2 { get { return this.Q.Column2; } }
		/// <summary>
		/// </summary>
		public Vector3 Column3 { get { return this.T; } }
		/// <summary>
		/// </summary>
		public Vector3 Row0 { get { return this.Q.Row0; } }
		/// <summary>
		/// </summary>
		public Vector3 Row1 { get { return this.Q.Row1; } }
		/// <summary>
		/// </summary>
		public Vector3 Row2 { get { return this.Q.Row2; } }
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + this.T.GetHashCode();
				hash = hash * 29 + this.Q.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}

		#region Statics
		/// <summary>
		/// </summary>
		public static readonly QuaternionTranslation Identity = new QuaternionTranslation(Vector3.Zero, Quaternion.Identity);
		#endregion
	}
}