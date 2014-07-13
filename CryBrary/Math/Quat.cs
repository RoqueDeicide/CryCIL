using System;

namespace CryEngine
{
	public struct Quat
	{
		/// <summary>
		/// The X, Y and Z components of the quaternion.
		/// </summary>
		public Vec3 V;

		/// <summary>
		/// The W component of the quaternion.
		/// </summary>
		public float W;

		public Quat(float w, float x, float y, float z)
			: this(w, new Vec3(x, y, z)) { }

		public Quat(float angle, Vec3 axis)
		{
			W = angle;
			V = axis;
		}

		public Quat(Matrix33 matrix)
		{
			this = FromMatrix33(matrix);
		}

		public Quat(Matrix34 matrix)
			: this(new Matrix33(matrix))
		{
		}

		public bool IsEquivalent(Quat q, float epsilon = 0.05f)
		{
			var p = -q;
			bool t0 = (Math.Abs(V.X - q.V.X) <= epsilon) && (Math.Abs(V.Y - q.V.Y) <= epsilon) && (Math.Abs(V.Z - q.V.Z) <= epsilon) && (Math.Abs(W - q.W) <= epsilon);
			bool t1 = (Math.Abs(V.X - p.V.X) <= epsilon) && (Math.Abs(V.Y - p.V.Y) <= epsilon) && (Math.Abs(V.Z - p.V.Z) <= epsilon) && (Math.Abs(W - p.W) <= epsilon);
			t0 |= t1;
			return t0;
		}

		public bool IsIdentity { get { return W == 1 && V.X == 0 && V.Y == 0 && V.Z == 0; } }

		public void SetIdentity()
		{
			this = Quat.Identity;
		}

		public bool IsUnit(float epsilon = 0.05f)
		{
			return Math.Abs(1 - ((this | this))) < epsilon;
		}

		public void SetRotationAA(float rad, Vec3 axis)
		{
			float s, c;
			MathHelpers.SinCos(rad * 0.5f, out s, out c);
			SetRotationAA(c, s, axis);
		}

		public void SetRotationAA(float cosha, float sinha, Vec3 axis)
		{
			W = cosha;
			V = axis * sinha;
		}

		public void SetRotationXYZ(Vec3 angle)
		{
			float sx;
			float cx;
			MathHelpers.SinCos((float)(angle.X * (float)0.5), out sx, out cx);

			float sy;
			float cy;
			MathHelpers.SinCos((float)(angle.Y * (float)0.5), out sy, out cy);

			float sz;
			float cz;
			MathHelpers.SinCos((float)(angle.Z * (float)0.5), out sz, out cz);

			W = cx * cy * cz + sx * sy * sz;
			V.X = cz * cy * sx - sz * sy * cx;
			V.Y = cz * sy * cx + sz * cy * sx;
			V.Z = sz * cy * cx - cz * sy * sx;
		}

		public void SetRotationX(float r)
		{
			float s, c;
			MathHelpers.SinCos((float)(r * (float)0.5), out s, out c); W = c; V.X = s; V.Y = 0; V.Z = 0;
		}

		public void SetRotationY(float r)
		{
			float s, c; MathHelpers.SinCos((float)(r * (float)0.5), out s, out c); W = c; V.X = 0; V.Y = s; V.Z = 0;
		}

		public void SetRotationZ(float r)
		{
			float s, c; MathHelpers.SinCos((float)(r * (float)0.5), out s, out c); W = c; V.X = 0; V.Y = 0; V.Z = s;
		}

		/*public void SetRotationV0V1(Vec3 v0, Vec3 v1)
		{
			var dot = v0.X * v1.X + v0.Y * v1.Y + v0.Z * v1.Z + 1.0;
			if (dot > 0.0001)
			{
				var vx = v0.Y * v1.Z - v0.Z * v1.Y;
				var vy = v0.Z * v1.X - v0.X * v1.Z;
				var vz = v0.X * v1.Y - v0.Y * v1.X;
				var d = Math.ISqrt(dot * dot + vx * vx + vy * vy + vz * vz);
				W = (float)(dot * d); V.X = (float)(vx * d); V.Y = (float)(vy * d); V.Z = (float)(vz * d);
				return;
			}
			W = 0; V = v0.GetOrthogonal().GetNormalized();
		}*/

		public void SetRotationVDir(Vec3 vDir)
		{
			// set default initialisation for up-vector
			W = 0.70710676908493042f; V.X = (vDir.Z + vDir.Z) * 0.35355338454246521f; V.Y = 0.0f; V.Z = 0.0f;
			var l = Math.Sqrt(vDir.X * vDir.X + vDir.Y * vDir.Y);
			if (l > 0.00001)
			{
				// calculate LookAt quaternion
				var hvX = vDir.X / l;
				var hvY = vDir.Y / l + 1.0f;
				var hvZ = l + 1.0f;

				var r = Math.Sqrt(hvX * hvX + hvY * hvY);
				var s = Math.Sqrt(hvZ * hvZ + vDir.Z * vDir.Z);
				// generate the half-angle sine&cosine
				var hacos0 = 0.0; var hasin0 = -1.0;
				if (r > 0.00001) { hacos0 = hvY / r; hasin0 = -hvX / r; }    // yaw
				var hacos1 = hvZ / s; var hasin1 = vDir.Z / s;                    // pitch
				W = (float)(hacos0 * hacos1); V.X = (float)(hacos0 * hasin1); V.Y = (float)(hasin0 * hasin1); V.Z = (float)(hasin0 * hacos1);
			}
		}

		public void SetRotationVDir(Vec3 vDir, float r)
		{
			SetRotationVDir(vDir);
			double sy, cy; MathHelpers.SinCos(r * 0.5f, out sy, out cy);
			var vx = V.X;
			var vy = V.Y;
			V.X = (float)(vx * cy - V.Z * sy); V.Y = (float)(W * sy + vy * cy); V.Z = (float)(V.Z * cy + vx * sy); W = (float)(W * cy - vy * sy);
		}

		public void Invert()
		{
			this = !this;
		}

		public void Normalize()
		{
			float d = MathHelpers.ISqrt(W * W + V.X * V.X + V.Y * V.Y + V.Z * V.Z);
			W *= d; V.X *= d; V.Y *= d; V.Z *= d;
		}

		public void NormalizeSafe()
		{
			float d = W * W + V.X * V.X + V.Y * V.Y + V.Z * V.Z;
			if (d > 1e-8f)
			{
				d = MathHelpers.ISqrt(d);
				W *= d; V.X *= d; V.Y *= d; V.Z *= d;
			}
			else
				SetIdentity();
		}

		public void Nlerp(Quat start, Quat end, float amount)
		{
			var q = end;
			if ((start | q) < 0) { q = -q; }

			var vDiff = q.V - start.V;

			V = start.V + (vDiff * amount);
			W = start.W + ((q.W - start.W) * amount);

			Normalize();
		}

		public void Nlerp2(Quat start, Quat end, float amount)
		{
			var q = end;
			var cosine = (start | q);
			if (cosine < 0) q = -q;
			var k = (1 - Math.Abs(cosine)) * 0.4669269f;
			var s = 2 * k * amount * amount * amount - 3 * k * amount * amount + (1 + k) * amount;
			V.X = start.V.X * (1.0f - s) + q.V.X * s;
			V.Y = start.V.Y * (1.0f - s) + q.V.Y * s;
			V.Z = start.V.Z * (1.0f - s) + q.V.Z * s;
			W = start.W * (1.0f - s) + q.W * s;
			Normalize();
		}

		public void Slerp(Quat start, Quat end, float amount)
		{
			var p = start;
			var q = end;
			var q2 = new Quat();

			var cosine = (p | q);
			if (cosine < 0.0f) { cosine = -cosine; q = -q; } // take shortest arc
			if (cosine > 0.9999f)
			{
				Nlerp(p, q, amount);
				return;
			}
			// from now on, a division by 0 is not possible any more
			q2.W = q.W - p.W * cosine;
			q2.V.X = q.V.X - p.V.X * cosine;
			q2.V.Y = q.V.Y - p.V.Y * cosine;
			q2.V.Z = q.V.Z - p.V.Z * cosine;
			var sine = Math.Sqrt(q2 | q2);
			double s, c;

			MathHelpers.SinCos(Math.Atan2(sine, cosine) * amount, out s, out c);
			W = (float)(p.W * c + q2.W * s / sine);
			V.X = (float)(p.V.X * c + q2.V.X * s / sine);
			V.Y = (float)(p.V.Y * c + q2.V.Y * s / sine);
			V.Z = (float)(p.V.Z * c + q2.V.Z * s / sine);
		}

		public void ExpSlerp(Quat start, Quat end, float amount)
		{
			var q = end;
			if ((start | q) < 0) { q = -q; }
			this = start * MathHelpers.Exp(MathHelpers.Log(!start * q) * amount);
		}

		public Quat GetScaled(float scale)
		{
			return CreateNlerp(Quat.Identity, this, scale);
		}

		/// <summary></summary>
		/// <param name="maxAngle">Max angle in radians</param>
		public void ClampAngle(float maxAngle)
		{
			var wMax = Math.Cos(2.0f * maxAngle);
			if (W < wMax)
			{
				W = (float)wMax;
				Normalize();
			}
		}

		public Quat Inverted { get { var q = this; q.Invert(); return q; } }

		public Quat Normalized { get { var q = this; q.Normalize(); return q; } }

		public Quat NormalizedSafe { get { var q = this; q.NormalizeSafe(); return q; } }

		public float Length
		{
			get
			{
				return (float)Math.Sqrt(W * W + V.X * V.X + V.Y * V.Y + V.Z * V.Z);
			}
		}

		public float LengthSquared
		{
			get
			{
				return (float)W * W + V.X * V.X + V.Y * V.Y + V.Z * V.Z;
			}
		}

		public Vec3 Angles
		{
			get
			{
				var angles = new Vec3();

				angles.Y = (float)Math.Asin(Math.Max(-1.0, Math.Min(1.0, -(V.X * V.Z - W * V.Y) * 2)));
				if (Math.Abs(Math.Abs(angles.Y) - (Math.PI * 0.5)) < 0.01)
				{
					angles.X = 0;
					angles.Z = (float)Math.Atan2(-2 * (V.X * V.Y - W * V.Z), 1 - (V.X * V.X + V.Z * V.Z) * 2);
				}
				else
				{
					angles.X = (float)Math.Atan2((V.Y * V.Z + W * V.X) * 2, 1 - (V.X * V.X + V.Y * V.Y) * 2);
					angles.Z = (float)Math.Atan2((V.X * V.Y + W * V.Z) * 2, 1 - (V.Z * V.Z + V.Y * V.Y) * 2);
				}

				return angles;
			}
		}

		public Vec3 Column0 { get { return new Vec3(2 * (V.X * V.X + W * W) - 1, 2 * (V.Y * V.X + V.Z * W), 2 * (V.Z * V.X - V.Y * W)); } }

		public Vec3 Column1 { get { return new Vec3(2 * (V.X * V.Y - V.Z * W), 2 * (V.Y * V.Y + W * W) - 1, 2 * (V.Z * V.Y + V.X * W)); } }

		public Vec3 Column2 { get { return new Vec3(2 * (V.X * V.Z + V.Y * W), 2 * (V.Y * V.Z - V.X * W), 2 * (V.Z * V.Z + W * W) - 1); } }

		public Vec3 Row0 { get { return new Vec3(2 * (V.X * V.X + W * W) - 1, 2 * (V.X * V.Y - V.Z * W), 2 * (V.X * V.Z + V.Y * W)); } }

		public Vec3 Row1 { get { return new Vec3(2 * (V.Y * V.X + V.Z * W), 2 * (V.Y * V.Y + W * W) - 1, 2 * (V.Y * V.Z - V.X * W)); } }

		public Vec3 Row2 { get { return new Vec3(2 * (V.Z * V.X - V.Y * W), 2 * (V.Z * V.Y + V.X * W), 2 * (V.Z * V.Z + W * W) - 1); } }

		public float ForwardX { get { return 2 * (V.X * V.Y - V.Z * W); } }
		public float ForwardY { get { return 2 * (V.Y * V.Y + W * W) - 1; } }
		public float ForwardZ { get { return 2 * (V.Z * V.Y + V.X * W); } }

		public float RotationZ { get { return (float)Math.Atan2(-ForwardX, ForwardY); } }

		/// <summary>
		/// Checks whether the quaternion is valid.
		/// </summary>
		/// <returns></returns>
		public bool IsValid
		{
			get
			{
				return V.IsValid && MathHelpers.IsNumberValid(W);
			}
		}

		#region Operators
		public static Quat operator *(Quat value, float scale)
		{
			return new Quat(value.W * scale, value.V * scale);
		}

		public static bool operator ==(Quat left, Quat right)
		{
			return left.IsEquivalent(right, 0.0000001f);
		}

		public static bool operator !=(Quat left, Quat right)
		{
			return !(left == right);
		}

		public static Quat operator -(Quat q)
		{
			return new Quat(-q.W, -q.V);
		}

		public static Quat operator !(Quat q)
		{
			return new Quat(q.W, -q.V);
		}

		public static float operator |(Quat left, Quat right)
		{
			return (left.V.X * right.V.X + left.V.Y * right.V.Y + left.V.Z * right.V.Z + left.W * right.W);
		}

		public static Quat operator *(Quat left, Quat right)
		{
			return new Quat(
				left.W * right.W - (left.V.X * right.V.X + left.V.Y * right.V.Y + left.V.Z * right.V.Z),
				left.V.Y * right.V.Z - left.V.Z * right.V.Y + left.W * right.V.X + left.V.X * right.W,
				left.V.Z * right.V.X - left.V.X * right.V.Z + left.W * right.V.Y + left.V.Y * right.W,
				left.V.X * right.V.Y - left.V.Y * right.V.X + left.W * right.V.Z + left.V.Z * right.W);
		}

		public static Quat operator /(Quat left, Quat right)
		{
			return (!right * left);
		}

		public static Quat operator +(Quat left, Quat right)
		{
			return new Quat(left.W + right.W, left.V + right.V);
		}

		public static Quat operator %(Quat left, Quat right)
		{
			var p = right;
			if ((p | left) < 0) p = -p;
			return new Quat(left.W + p.W, left.V + p.V);
		}

		public static Quat operator -(Quat left, Quat right)
		{
			return new Quat(left.W - right.W, left.V - right.V);
		}

		public static Quat operator /(Quat left, float right)
		{
			return new Quat(left.W / right, left.V / right);
		}

		public static Vec3 operator *(Quat left, Vec3 right)
		{
			var vOut = new Vec3();
			var r2 = new Vec3();

			r2.X = (left.V.Y * right.Z - left.V.Z * right.Y) + left.W * right.X;
			r2.Y = (left.V.Z * right.X - left.V.X * right.Z) + left.W * right.Y;
			r2.Z = (left.V.X * right.Y - left.V.Y * right.X) + left.W * right.Z;
			vOut.X = (r2.Z * left.V.Y - r2.Y * left.V.Z); vOut.X += vOut.X + right.X;
			vOut.Y = (r2.X * left.V.Z - r2.Z * left.V.X); vOut.Y += vOut.Y + right.Y;
			vOut.Z = (r2.Y * left.V.X - r2.X * left.V.Y); vOut.Z += vOut.Z + right.Z;
			return vOut;
		}

		public static Vec3 operator *(Vec3 left, Quat right)
		{
			var vOut = new Vec3();
			var r2 = new Vec3();

			r2.X = (right.V.Z * left.Y - right.V.Y * left.Z) + right.W * left.X;
			r2.Y = (right.V.X * left.Z - right.V.Z * left.X) + right.W * left.Y;
			r2.Z = (right.V.Y * left.X - right.V.X * left.Y) + right.W * left.Z;
			vOut.X = (r2.Y * right.V.Z - r2.Z * right.V.Y); vOut.X += vOut.X + left.X;
			vOut.Y = (r2.Z * right.V.X - r2.X * right.V.Z); vOut.Y += vOut.Y + left.Y;
			vOut.Z = (r2.X * right.V.Y - r2.Y * right.V.X); vOut.Z += vOut.Z + left.Z;
			return vOut;
		}

		public static explicit operator Vec3(Quat value)
		{
			return new Vec3(value);
		}
		#endregion

		#region Overrides
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 23 + W.GetHashCode();
				hash = hash * 23 + V.GetHashCode();

				return hash;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj is Quat)
				return this == (Quat)obj;

			return false;
		}
		#endregion

		#region Statics
		/// <summary>
		/// The identity <see cref="CryEngine.Quat" /> (0, 0, 0, 1).
		/// </summary>
		public static readonly Quat Identity = new Quat(0.0f, 0.0f, 0.0f, 1.0f);

		public static Quat FromMatrix33(Matrix33 m)
		{
			float s, p, tr = m.M00 + m.M11 + m.M22;

			//check the diagonal
			if (tr > (float)0.0)
			{
				s = (float)Math.Sqrt(tr + 1.0f); p = 0.5f / s;
				return new Quat(s * 0.5f, (m.M21 - m.M12) * p, (m.M02 - m.M20) * p, (m.M10 - m.M01) * p);
			}
			//diagonal is negative. now we have to find the biggest element on the diagonal
			//check if "M00" is the biggest element
			if ((m.M00 >= m.M11) && (m.M00 >= m.M22))
			{
				s = (float)Math.Sqrt(m.M00 - m.M11 - m.M22 + 1.0f); p = 0.5f / s;
				return new Quat((m.M21 - m.M12) * p, s * 0.5f, (m.M10 + m.M01) * p, (m.M20 + m.M02) * p);
			}
			//check if "M11" is the biggest element
			if ((m.M11 >= m.M00) && (m.M11 >= m.M22))
			{
				s = (float)Math.Sqrt(m.M11 - m.M22 - m.M00 + 1.0f); p = 0.5f / s;
				return new Quat((m.M02 - m.M20) * p, (m.M01 + m.M10) * p, s * 0.5f, (m.M21 + m.M12) * p);
			}
			//check if "M22" is the biggest element
			if ((m.M22 >= m.M00) && (m.M22 >= m.M11))
			{
				s = (float)Math.Sqrt(m.M22 - m.M00 - m.M11 + 1.0f); p = 0.5f / s;
				return new Quat((m.M10 - m.M01) * p, (m.M02 + m.M20) * p, (m.M12 + m.M21) * p, s * 0.5f);
			}

			return Quat.Identity; // if it ends here, then we have no valid rotation matrix
		}

		public static Quat CreateRotationAA(float rad, Vec3 axis)
		{
			var q = new Quat();
			q.SetRotationAA(rad, axis);
			return q;
		}

		public static Quat CreateRotationAA(float cosha, float sinha, Vec3 axis)
		{
			var q = new Quat();
			q.SetRotationAA(cosha, sinha, axis);
			return q;
		}

		public static Quat CreateRotationXYZ(Vec3 angle)
		{
			var q = new Quat();
			q.SetRotationXYZ(angle);
			return q;
		}

		public static Quat CreateRotationX(float r)
		{
			var q = new Quat();
			q.SetRotationX(r);
			return q;
		}

		public static Quat CreateRotationY(float r)
		{
			var q = new Quat();
			q.SetRotationY(r);
			return q;
		}

		public static Quat CreateRotationZ(float r)
		{
			var q = new Quat();
			q.SetRotationZ(r);
			return q;
		}

		/*public static Quat CreateRotationV0V1(Vec3 v0, Vec3 v1)
		{
			var q = new Quat();
			q.SetRotationV0V1(v0, v1);
			return q;
		}*/

		public static Quat CreateRotationVDir(Vec3 vDir)
		{
			var q = new Quat();
			q.SetRotationVDir(vDir);
			return q;
		}

		public static Quat CreateRotationVDir(Vec3 vDir, float r)
		{
			var q = new Quat();
			q.SetRotationVDir(vDir, r);
			return q;
		}

		public static Quat CreateNlerp(Quat start, Quat end, float amount)
		{
			var q = new Quat();
			q.Nlerp(start, end, amount);
			return q;
		}

		public static Quat CreateNlerp2(Quat start, Quat end, float amount)
		{
			var q = new Quat();
			q.Nlerp2(start, end, amount);
			return q;
		}

		public static Quat CreateSlerp(Quat start, Quat end, float amount)
		{
			var q = new Quat();
			q.Slerp(start, end, amount);
			return q;
		}

		public static Quat CreateExpSlerp(Quat start, Quat end, float amount)
		{
			var q = new Quat();
			q.ExpSlerp(start, end, amount);
			return q;
		}

		public static Quat Parse(string value)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length < 1)
				throw new ArgumentException("value string was empty");
#endif

			string[] split = value.Split(',');

#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (split.Length != 4)
				throw new ArgumentException("value string was invalid");
#endif

			return new Quat(System.Convert.ToSingle(split[0]), System.Convert.ToSingle(split[1]), System.Convert.ToSingle(split[2]), System.Convert.ToSingle(split[3]));
		}
		#endregion
	}
}