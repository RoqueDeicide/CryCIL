using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Quaternion with a translation vector
	/// </summary>
	public struct QuatT
	{
		/// <summary>
		/// The quaternion
		/// </summary>
		public Quat Q;
		/// <summary>
		/// The translation vector and a scalar (for uniform scaling?)
		/// </summary>
		public Vec3 T;

		public QuatT(Vec3 t, Quat q)
		{
			Q = q;
			T = t;
		}

		public QuatT(Matrix34 m)
		{
			Q = new Quat(m);
			T = m.Translation;
		}

		public void SetIdentity()
		{
			this = Identity;
		}

		public void SetRotationXYZ(Vec3 rad, Vec3? trans = null)
		{
			Q.SetRotationXYZ(rad);

			T = trans.GetValueOrDefault();
		}

		public void SetRotationAA(float cosha, float sinha, Vec3 axis, Vec3? trans = null)
		{
			Q.SetRotationAA(cosha, sinha, axis);
			T = trans.GetValueOrDefault();
		}

		public void Invert()
		{
			T = -T * Q;
			Q = !Q;
		}

		public void SetTranslation(Vec3 trans)
		{
			T = trans;
		}

		public bool IsEquivalent(QuatT p, float epsilon = 0.05f)
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

		public void Nlerp(QuatT start, QuatT end, float amount)
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

		public void SetFromVectors(Vec3 vx, Vec3 vy, Vec3 vz, Vec3 pos)
		{
			var m34 = new Matrix34();
			m34.M00 = vx.X; m34.M01 = vy.X; m34.M02 = vz.X; m34.M03 = pos.X;
			m34.M10 = vx.Y; m34.M11 = vy.Y; m34.M12 = vz.Y; m34.M13 = pos.Y;
			m34.M20 = vx.Z; m34.M21 = vy.Z; m34.M22 = vz.Z; m34.M23 = pos.Z;
			this = new QuatT(m34);
		}

		public void ClampLengthAngle(float maxLength, float maxAngle)
		{
			T.ClampLength(maxLength);
			Q.ClampAngle(maxAngle);
		}

		public QuatT GetScaled(float scale)
		{
			return new QuatT(T * scale, Q.GetScaled(scale));
		}

		public bool IsIdentity { get { return Q.IsIdentity && T.IsZero(); } }

		public QuatT Inverted
		{
			get
			{
				var quatT = this;
				quatT.Invert();
				return quatT;
			}
		}

		public Vec3 Column0 { get { return Q.Column0; } }
		public Vec3 Column1 { get { return Q.Column1; } }
		public Vec3 Column2 { get { return Q.Column2; } }
		public Vec3 Column3 { get { return T; } }

		public Vec3 Row0 { get { return Q.Row0; } }
		public Vec3 Row1 { get { return Q.Row1; } }
		public Vec3 Row2 { get { return Q.Row2; } }

		public override int GetHashCode()
		{
			// Overflow is fine, just wrap
			unchecked
			{
				int hash = 17;

				hash = hash * 29 + T.GetHashCode();
				hash = hash * 29 + Q.GetHashCode();

				return hash;
			}
		}

		#region Statics
		public static readonly QuatT Identity = new QuatT(Vec3.Zero, Quat.Identity);
		#endregion
	}
}