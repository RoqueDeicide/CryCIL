using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Quaternion with a translation vector
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

		public QuaternionTranslation(Vector3 t, Quaternion q)
		{
			Q = q;
			T = t;
		}

		public QuaternionTranslation(Matrix34 m)
		{
			Q = new Quaternion(m);
			T = m.Translation;
		}

		public void SetIdentity()
		{
			this = Identity;
		}

		public void SetRotationXYZ(Vector3 rad, Vector3? trans = null)
		{
			Q.SetRotationAroundXYZAxes(rad);

			T = trans.GetValueOrDefault();
		}

		public void SetRotationAA(float cosha, float sinha, Vector3 axis, Vector3? trans = null)
		{
			Q.SetRotationAngleAxis(cosha, sinha, axis);
			T = trans.GetValueOrDefault();
		}

		public void Invert()
		{
			T = -T * Q;
			Q = !Q;
		}

		public void SetTranslation(Vector3 trans)
		{
			T = trans;
		}

		public bool IsEquivalent(QuaternionTranslation p, float epsilon = 0.05f)
		{
			var q0 = p.Q;
			var q1 = -p.Q;
			bool t0 = (Math.Abs(Q.V.X - q0.V.X) <= epsilon) && (Math.Abs(Q.V.Y - q0.V.Y) <= epsilon) && (Math.Abs(Q.V.Z - q0.V.Z) <= epsilon) && (Math.Abs(Q.W - q0.W) <= epsilon);
			bool t1 = (Math.Abs(Q.V.X - q1.V.X) <= epsilon) && (Math.Abs(Q.V.Y - q1.V.Y) <= epsilon) && (Math.Abs(Q.V.Z - q1.V.Z) <= epsilon) && (Math.Abs(Q.W - q1.W) <= epsilon);
			return ((t0 | t1) && (Math.Abs(T.X - p.T.X) <= epsilon) && (Math.Abs(T.Y - p.T.Y) <= epsilon) && (Math.Abs(T.Z - p.T.Z) <= epsilon));
		}

		/// <summary>
		/// Checks whether the transformation is valid.
		/// </summary>
		/// <returns></returns>
		public bool IsValid
		{
			get
			{
				return T.IsValid && Q.IsValid;
			}
		}

		public void Nlerp(QuaternionTranslation start, QuaternionTranslation end, float amount)
		{
			var d = end.Q;
			if ((start.Q | d) < 0) { d = -d; }

			var vDiff = d.V - start.Q.V;

			Q.V = start.Q.V + (vDiff * amount);
			Q.W = start.Q.W + ((d.W - start.Q.W) * amount);

			Q.Normalize();

			vDiff = end.T - start.T;
			T = start.T + (vDiff * amount);
		}

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

		public void ClampLengthAngle(float maxLength, float maxAngle)
		{
			T.ClampLength(maxLength);
			Q.ClampAngle(maxAngle);
		}

		public QuaternionTranslation GetScaled(float scale)
		{
			return new QuaternionTranslation(T * scale, Q.GetScaled(scale));
		}

		public bool IsIdentity { get { return Q.IsIdentity && T.IsZero(); } }

		public QuaternionTranslation Inverted
		{
			get
			{
				var quatT = this;
				quatT.Invert();
				return quatT;
			}
		}

		public Vector3 Column0 { get { return Q.Column0; } }
		public Vector3 Column1 { get { return Q.Column1; } }
		public Vector3 Column2 { get { return Q.Column2; } }
		public Vector3 Column3 { get { return T; } }

		public Vector3 Row0 { get { return Q.Row0; } }
		public Vector3 Row1 { get { return Q.Row1; } }
		public Vector3 Row2 { get { return Q.Row2; } }

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 29 + T.GetHashCode();
				hash = hash * 29 + Q.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}

		#region Statics
		public static readonly QuaternionTranslation Identity = new QuaternionTranslation(Vector3.Zero, Quaternion.Identity);
		#endregion
	}
}