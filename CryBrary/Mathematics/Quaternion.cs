using System;

namespace CryEngine.Mathematics
{
	/// <summary>
	/// Represents a quaternion - object that describes rotation by specific angle around specific axis.
	/// </summary>
	/// <remarks>
	/// Quaternion is basically a rotation. If you use quaternions to define orientation of objects
	/// then in order to get vector that points forward from the object, you need to take <see
	/// cref="Vector3.Forward" /> and rotate it by this quaternion.
	/// </remarks>
	public struct Quaternion
	{
		#region Fields
		/// <summary>
		/// The X, Y and Z components of the quaternion.
		/// </summary>
		public Vector3 V;
		/// <summary>
		/// The W component of the quaternion.
		/// </summary>
		public float W;
		#endregion
		#region Properties
		/// <summary>
		/// Gets inverted version of this quaternion.
		/// </summary>
		public Quaternion Inverted
		{
			get
			{
				var q = this;
				q.Conjugate();
				return q;
			}
		}
		/// <summary>
		/// Gets normalized version of this quaternion.
		/// </summary>
		public Quaternion Normalized { get { var q = this; q.Normalize(); return q; } }
		/// <summary>
		/// Gets safely normalized version of this quaternion.
		/// </summary>
		public Quaternion NormalizedSafe { get { var q = this; q.NormalizeSafe(); return q; } }
		/// <summary>
		/// Gets modulus of this quaternion.
		/// </summary>
		public float Length
		{
			get
			{
				return (float)Math.Sqrt(this.W * this.W + this.V.X * this.V.X + this.V.Y * this.V.Y + this.V.Z * this.V.Z);
			}
		}
		/// <summary>
		/// Gets norm of this quaternion.
		/// </summary>
		public float LengthSquared
		{
			get
			{
				return this.W * this.W + this.V.X * this.V.X + this.V.Y * this.V.Y + this.V.Z * this.V.Z;
			}
		}
		/// <summary>
		/// Converts this quaternion to set of Euler angles.
		/// </summary>
		public Vector3 Angles
		{
			get
			{
				var angles = new Vector3
				{
					Y =
					(float)Math.Asin
					(
						Math.Max
						(
							-1.0,
							Math.Min
							(
								1.0,
								-(this.V.X * this.V.Z - this.W * this.V.Y) * 2
							)
						)
					)
				};

				if (Math.Abs(Math.Abs(angles.Y) - (Math.PI * 0.5)) < 0.01)
				{
					angles.X = 0;
					angles.Z = (float)Math.Atan2(-2 * (this.V.X * this.V.Y - this.W * this.V.Z), 1 - (this.V.X * this.V.X + this.V.Z * this.V.Z) * 2);
				}
				else
				{
					angles.X = (float)Math.Atan2((this.V.Y * this.V.Z + this.W * this.V.X) * 2, 1 - (this.V.X * this.V.X + this.V.Y * this.V.Y) * 2);
					angles.Z = (float)Math.Atan2((this.V.X * this.V.Y + this.W * this.V.Z) * 2, 1 - (this.V.Z * this.V.Z + this.V.Y * this.V.Y) * 2);
				}

				return angles;
			}
		}
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets first column of that matrix.
		/// </summary>
		/// <remarks>
		/// First column of rotation matrix represents a vector that points to the right from the
		/// point that is oriented using this quaternion.
		/// </remarks>
		public Vector3 Column0 { get { return new Vector3(2 * (this.V.X * this.V.X + this.W * this.W) - 1, 2 * (this.V.Y * this.V.X + this.V.Z * this.W), 2 * (this.V.Z * this.V.X - this.V.Y * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets second column of that matrix.
		/// </summary>
		/// <remarks>
		/// Second column of rotation matrix represents a vector that points forward from the point
		/// that is oriented using this quaternion.
		/// </remarks>
		public Vector3 Column1 { get { return new Vector3(2 * (this.V.X * this.V.Y - this.V.Z * this.W), 2 * (this.V.Y * this.V.Y + this.W * this.W) - 1, 2 * (this.V.Z * this.V.Y + this.V.X * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets third column of that matrix.
		/// </summary>
		/// <remarks>
		/// Third column of rotation matrix represents a vector that points up from the point that
		/// is oriented using this quaternion.
		/// </remarks>
		public Vector3 Column2 { get { return new Vector3(2 * (this.V.X * this.V.Z + this.V.Y * this.W), 2 * (this.V.Y * this.V.Z - this.V.X * this.W), 2 * (this.V.Z * this.V.Z + this.W * this.W) - 1); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets first row of that matrix.
		/// </summary>
		/// <remarks> First row of rotation matrix contains X-coordinates of rotation axes. </remarks>
		public Vector3 Row0 { get { return new Vector3(2 * (this.V.X * this.V.X + this.W * this.W) - 1, 2 * (this.V.X * this.V.Y - this.V.Z * this.W), 2 * (this.V.X * this.V.Z + this.V.Y * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets second row of that matrix.
		/// </summary>
		/// <remarks> Second row of rotation matrix contains Y-coordinates of rotation axes. </remarks>
		public Vector3 Row1 { get { return new Vector3(2 * (this.V.Y * this.V.X + this.V.Z * this.W), 2 * (this.V.Y * this.V.Y + this.W * this.W) - 1, 2 * (this.V.Y * this.V.Z - this.V.X * this.W)); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets third row of that matrix.
		/// </summary>
		/// <remarks> Third row of rotation matrix contains Z-coordinates of rotation axes. </remarks>
		public Vector3 Row2 { get { return new Vector3(2 * (this.V.Z * this.V.X - this.V.Y * this.W), 2 * (this.V.Z * this.V.Y + this.V.X * this.W), 2 * (this.V.Z * this.V.Z + this.W * this.W) - 1); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets value in first column and
		/// first row.
		/// </summary>
		public float ForwardX { get { return 2 * (this.V.X * this.V.Y - this.V.Z * this.W); } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets value in first column and
		/// second row.
		/// </summary>
		public float ForwardY { get { return 2 * (this.V.Y * this.V.Y + this.W * this.W) - 1; } }
		/// <summary>
		/// Treating this quaternion as a compressed rotation matrix, gets value in first column and
		/// third row.
		/// </summary>
		public float ForwardZ { get { return 2 * (this.V.Z * this.V.Y + this.V.X * this.W); } }
		/// <summary>
		/// Gets yaw rotation from this quaternion.
		/// </summary>
		public float RotationZ { get { return (float)Math.Atan2(-this.ForwardX, this.ForwardY); } }
		/// <summary>
		/// Checks whether the quaternion is valid.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return this.V.IsValid && MathHelpers.IsNumberValid(this.W);
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of <see cref="Quaternion" /> struct.
		/// </summary>
		/// <param name="w"> The W component of the quaternion. </param>
		/// <param name="x"> The X component of the quaternion. </param>
		/// <param name="y"> The Y component of the quaternion. </param>
		/// <param name="z"> The Z component of the quaternion. </param>
		public Quaternion(float w, float x, float y, float z)
			: this(w, new Vector3(x, y, z)) { }
		/// <summary>
		/// Creates new instance of <see cref="Quaternion" /> struct.
		/// </summary>
		/// <param name="angle"> Supposedly an angle of rotation. </param>
		/// <param name="axis">  Supposedly a vector that represents axis of rotation. </param>
		public Quaternion(float angle, Vector3 axis)
		{
			this.W = angle;
			this.V = axis;
		}
		/// <summary>
		/// Creates new instance of <see cref="Quaternion" /> struct.
		/// </summary>
		/// <param name="matrix"> 3x3 matrix that represents rotation. </param>
		public Quaternion(Matrix33 matrix)
		{
			this = FromMatrix33(matrix);
		}
		/// <summary>
		/// Creates new instance of <see cref="Quaternion" /> struct.
		/// </summary>
		/// <param name="matrix"> 3x4 matrix that contains representation of rotation. </param>
		public Quaternion(Matrix34 matrix)
			: this(new Matrix33(matrix))
		{
		}
		#endregion
		#region Static Interface
		/// <summary>
		/// The identity <see cref="Quaternion" /> (0, 0, 0, 1).
		/// </summary>
		/// <remarks> Identity quaternion has W equal to 1, and vector is equal to 0. </remarks>
		public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		#region Operators
		/// <summary>
		/// Multiplies quaternion by given number.
		/// </summary>
		/// <param name="value"> Left operand. </param>
		/// <param name="scale"> Right operand. </param>
		/// <returns> Result of scaling. </returns>
		public static Quaternion operator *(Quaternion value, float scale)
		{
			return new Quaternion(value.W * scale, value.V * scale);
		}
		/// <summary>
		/// Determines whether two quaternions are equal.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns> True, if two quaternions are equal. </returns>
		public static bool operator ==(Quaternion left, Quaternion right)
		{
			return left.IsEquivalent(right, 0.0000001f);
		}
		/// <summary>
		/// Determines whether two quaternions are not equal.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns> True, if two quaternions are not equal. </returns>
		public static bool operator !=(Quaternion left, Quaternion right)
		{
			return !left.IsEquivalent(right, 0.0000001f);
		}
		/// <summary>
		/// Negates given quaternion.
		/// </summary>
		/// <param name="q"> Quaternion for negation. </param>
		/// <returns> Negated quaternion. </returns>
		public static Quaternion operator -(Quaternion q)
		{
			return new Quaternion(-q.W, -q.V);
		}
		/// <summary>
		/// Reverses vector of this quaternion.
		/// </summary>
		/// <param name="q"> Quaternion which vector must be reversed. </param>
		/// <returns> Quaternion with reversed vector. </returns>
		public static Quaternion operator !(Quaternion q)
		{
			return new Quaternion(q.W, -q.V);
		}
		/// <summary>
		/// Calculates dot-product of two quaternions.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns> Dot-product of two quaternions. </returns>
		public static float operator |(Quaternion left, Quaternion right)
		{
			return (left.V.X * right.V.X + left.V.Y * right.V.Y + left.V.Z * right.V.Z + left.W * right.W);
		}
		/// <summary>
		/// Concatenates rotations represented by two quaternions.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns>
		/// Quaternion that represents rotation equivalent to rotation by left quaternion followed
		/// up by rotation by right quaternion.
		/// </returns>
		public static Quaternion operator *(Quaternion left, Quaternion right)
		{
			return new Quaternion(
				left.W * right.W - (left.V.X * right.V.X + left.V.Y * right.V.Y + left.V.Z * right.V.Z),
				left.V.Y * right.V.Z - left.V.Z * right.V.Y + left.W * right.V.X + left.V.X * right.W,
				left.V.Z * right.V.X - left.V.X * right.V.Z + left.W * right.V.Y + left.V.Y * right.W,
				left.V.X * right.V.Y - left.V.Y * right.V.X + left.W * right.V.Z + left.V.Z * right.W);
		}
		/// <summary>
		/// Calculates quaternion that is a result of division of left one by right one.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns>
		/// Quaternion that represents rotation equivalent to rotation by left quaternion with
		/// reversed vector followed up by rotation by right quaternion.
		/// </returns>
		public static Quaternion operator /(Quaternion left, Quaternion right)
		{
			return (!right * left);
		}
		/// <summary>
		/// Calculates sum of two quaternions.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns> Sum of two quaternions. </returns>
		public static Quaternion operator +(Quaternion left, Quaternion right)
		{
			return new Quaternion(left.W + right.W, left.V + right.V);
		}
		/// <summary>
		/// Calculate mod of two quaternions.
		/// </summary>
		/// <param name="left">  Left operand. </param>
		/// <param name="right"> Right operand. </param>
		/// <returns>
		/// If dot-product of two quaternions is less then zero then returns sum of left quaternion
		/// and reverse of right one, otherwise just returns a sum of two.
		/// </returns>
		public static Quaternion operator %(Quaternion left, Quaternion right)
		{
			var p = right;
			if ((p | left) < 0) p = -p;
			return new Quaternion(left.W + p.W, left.V + p.V);
		}

		public static Quaternion operator -(Quaternion left, Quaternion right)
		{
			return new Quaternion(left.W - right.W, left.V - right.V);
		}

		public static Quaternion operator /(Quaternion left, float right)
		{
			return new Quaternion(left.W / right, left.V / right);
		}

		public static Vector3 operator *(Quaternion left, Vector3 right)
		{
			var r2 = new Vector3
			{
				X = (left.V.Y * right.Z - left.V.Z * right.Y) + left.W * right.X,
				Y = (left.V.Z * right.X - left.V.X * right.Z) + left.W * right.Y,
				Z = (left.V.X * right.Y - left.V.Y * right.X) + left.W * right.Z
			};

			var vOut = new Vector3
			{
				X = (r2.Z * left.V.Y - r2.Y * left.V.Z),
				Y = (r2.X * left.V.Z - r2.Z * left.V.X),
				Z = (r2.Y * left.V.X - r2.X * left.V.Y)
			};

			vOut.X += vOut.X + right.X;
			vOut.Y += vOut.Y + right.Y;
			vOut.Z += vOut.Z + right.Z;
			return vOut;
		}

		public static Vector3 operator *(Vector3 left, Quaternion right)
		{
			var r2 = new Vector3
			{
				X = (right.V.Z * left.Y - right.V.Y * left.Z) + right.W * left.X,
				Y = (right.V.X * left.Z - right.V.Z * left.X) + right.W * left.Y,
				Z = (right.V.Y * left.X - right.V.X * left.Y) + right.W * left.Z
			};

			var vOut = new Vector3
			{
				X = (r2.Y * right.V.Z - r2.Z * right.V.Y),
				Y = (r2.Z * right.V.X - r2.X * right.V.Z),
				Z = (r2.X * right.V.Y - r2.Y * right.V.X)
			};
			vOut.X += vOut.X + left.X;
			vOut.Y += vOut.Y + left.Y;
			vOut.Z += vOut.Z + left.Z;
			return vOut;
		}
		/// <summary>
		/// Converts quaternion to 3D-vector.
		/// </summary>
		/// <param name="value"> Quaternion for conversion. </param>
		/// <returns> Result of conversion. </returns>
		public static explicit operator Vector3(Quaternion value)
		{
			return new Vector3(value);
		}
		#endregion
		#region Creation
		/// <summary>
		/// Creates new quaternion that represents rotation defined by given 3x3 matrix.
		/// </summary>
		/// <param name="m"> Matrix that defines the rotation. </param>
		/// <returns> Quaternion that represents rotation defined by given 3x3 matrix. </returns>
		public static Quaternion FromMatrix33(Matrix33 m)
		{
			float s, p, tr = m.M00 + m.M11 + m.M22;

			//check the diagonal
			if (tr > (float)0.0)
			{
				s = (float)Math.Sqrt(tr + 1.0f); p = 0.5f / s;
				return new Quaternion(s * 0.5f, (m.M21 - m.M12) * p, (m.M02 - m.M20) * p, (m.M10 - m.M01) * p);
			}
			//diagonal is negative. now we have to find the biggest element on the diagonal
			//check if "M00" is the biggest element
			if ((m.M00 >= m.M11) && (m.M00 >= m.M22))
			{
				s = (float)Math.Sqrt(m.M00 - m.M11 - m.M22 + 1.0f); p = 0.5f / s;
				return new Quaternion((m.M21 - m.M12) * p, s * 0.5f, (m.M10 + m.M01) * p, (m.M20 + m.M02) * p);
			}
			//check if "M11" is the biggest element
			if ((m.M11 >= m.M00) && (m.M11 >= m.M22))
			{
				s = (float)Math.Sqrt(m.M11 - m.M22 - m.M00 + 1.0f); p = 0.5f / s;
				return new Quaternion((m.M02 - m.M20) * p, (m.M01 + m.M10) * p, s * 0.5f, (m.M21 + m.M12) * p);
			}
			//check if "M22" is the biggest element
			if ((m.M22 >= m.M00) && (m.M22 >= m.M11))
			{
				s = (float)Math.Sqrt(m.M22 - m.M00 - m.M11 + 1.0f); p = 0.5f / s;
				return new Quaternion((m.M10 - m.M01) * p, (m.M02 + m.M20) * p, (m.M12 + m.M21) * p, s * 0.5f);
			}

			return Quaternion.Identity; // if it ends here, then we have no valid rotation matrix
		}
		/// <summary>
		/// Creates new quaternion that represents rotation by specified angle around given axis.
		/// </summary>
		/// <param name="rad">  Angle in radians. </param>
		/// <param name="axis"> <see cref="Vector3" /> that represents an axis of rotation. </param>
		/// <returns> Quaternion that represents rotation by specified angle around given axis. </returns>
		public static Quaternion CreateRotationAngleAxis(float rad, Vector3 axis)
		{
			var q = new Quaternion();
			q.SetRotationAngleAxis(rad, axis);
			return q;
		}
		/// <summary>
		/// Creates new quaternion that represents rotation by specified angle around given axis.
		/// </summary>
		/// <param name="cosha"> Cosine of angle of rotation. </param>
		/// <param name="sinha"> Sine of angle of rotation. </param>
		/// <param name="axis">  <see cref="Vector3" /> that represents an axis of rotation. </param>
		/// <returns> Quaternion that represents rotation by specified angle around given axis. </returns>
		public static Quaternion CreateRotationAngleAxis(float cosha, float sinha, Vector3 axis)
		{
			var q = new Quaternion();
			q.SetRotationAngleAxis(cosha, sinha, axis);
			return q;
		}
		/// <summary>
		/// Creates new quaternion that represents rotation defined by Euler angles.
		/// </summary>
		/// <param name="angle"> Euler angles of rotation in radians. </param>
		/// <returns> Quaternion that represents rotation defined by Euler angles. </returns>
		public static Quaternion CreateRotationAroundXYZAxes(Vector3 angle)
		{
			var q = new Quaternion();
			q.SetRotationAroundXYZAxes(angle);
			return q;
		}
		/// <summary>
		/// Creates new quaternion that represents rotation around X-axis.
		/// </summary>
		/// <param name="r"> Angle of rotation in radians. </param>
		/// <returns> Quaternion that represents rotation around X-axis. </returns>
		public static Quaternion CreateRotationAroundX(float r)
		{
			var q = new Quaternion();
			q.SetRotationAroundX(r);
			return q;
		}
		/// <summary>
		/// Creates new quaternion that represents rotation around Y-axis.
		/// </summary>
		/// <param name="r"> Angle of rotation in radians. </param>
		/// <returns> Quaternion that represents rotation around Y-axis. </returns>
		public static Quaternion CreateRotationAroundY(float r)
		{
			var q = new Quaternion();
			q.SetRotationAroundY(r);
			return q;
		}
		/// <summary>
		/// Creates new quaternion that represents rotation around Z-axis.
		/// </summary>
		/// <param name="r"> Angle of rotation in radians. </param>
		/// <returns> Quaternion that represents rotation around Z-axis. </returns>
		public static Quaternion CreateRotationAroundZ(float r)
		{
			var q = new Quaternion();
			q.SetRotationAroundZ(r);
			return q;
		}
		/// <summary>
		/// Creates quaternion that represents rotation from first vector two the sector via
		/// shortest arc.
		/// </summary>
		/// <param name="one">          First vector. </param>
		/// <param name="two">          Second vector. </param>
		/// <param name="fallbackAxis">
		/// Axis to use to represent 180 degrees rotation when turns out, that two vectors are
		/// collinear about point in opposite directions.
		/// </param>
		/// <returns>
		/// Quaternion that represents rotation from first vector two the sector via shortest arc.
		/// </returns>
		public static Quaternion CreateRotationFrom2Vectors(Vector3 one, Vector3 two, Vector3 fallbackAxis = new Vector3())
		{
			Quaternion q = new Quaternion();

			q.SetRotationFrom2Vectors(one, two, fallbackAxis);

			return q;
		}
		/// <summary>
		/// Creates new quaternion that represents rotation to given vector from <see
		/// cref="Vector3.Up" />(?) vector.
		/// </summary>
		/// <remarks>
		/// This method is used when we need to make an entity to look in a direction described by
		/// the <paramref name="vdir">vector</paramref>.
		/// </remarks>
		/// <param name="vdir">
		/// <see cref="Vector3" /> that represents direction to which we need to find a rotation.
		/// </param>
		public static Quaternion CreateRotationToViewDirection(Vector3 vdir)
		{
			Quaternion q = Quaternion.Identity;
			q.SetRotationToViewDirection(vdir);
			return q;
		}
		/// <summary>
		/// Creates new quaternion that represents rotation to given vector from <see
		/// cref="Vector3.Up" />(?) vector.
		/// </summary>
		/// <param name="vdir">
		/// <see cref="Vector3" /> that represents direction to which we need to find a rotation.
		/// </param>
		/// <param name="r">Angle of rotation around forward axis to apply to new quaternion.</param>
		/// <seealso cref="Quaternion.CreateRotationToViewDirection(Vector3)"/>
		public static Quaternion CreateRotationToViewDirection(Vector3 vdir, float r)
		{
			Quaternion q = Quaternion.Identity;
			q.SetRotationToViewDirection(vdir, r);
			return q;
		}
		/// <summary>
		/// Creates quaternion using normalized linear interpolation algorithm.
		/// </summary>
		/// <param name="start">Starting position for interpolation.</param>
		/// <param name="end">Ending position for interpolation.</param>
		/// <param name="amount">
		/// Value between 0 and 1 that defines "distance" between required quaternion and given two.
		/// </param>
		/// <returns>
		/// Quaternion that represents rotation between rotations defined by given two quaternions.
		/// </returns>
		public static Quaternion CreateNormalizedLinearInterpolation(Quaternion start, Quaternion end, float amount)
		{
			var q = new Quaternion();
			q.NormalizedLinearInterpolation(start, end, amount);
			return q;
		}
		/// <summary>
		/// Creates quaternion using different normalized linear interpolation algorithm.
		/// </summary>
		/// <param name="start">Starting position for interpolation.</param>
		/// <param name="end">Ending position for interpolation.</param>
		/// <param name="amount">
		/// Value between 0 and 1 that defines "distance" between required quaternion and given two.
		/// </param>
		/// <returns>
		/// Quaternion that represents rotation between rotations defined by given two quaternions.
		/// </returns>
		public static Quaternion CreateNormalizedLinearInterpolation2(Quaternion start, Quaternion end, float amount)
		{
			var q = new Quaternion();
			q.NormalizedLinearInterpolation2(start, end, amount);
			return q;
		}
		/// <summary>
		/// Creates quaternion using spherical linear interpolation algorithm.
		/// </summary>
		/// <param name="start">Starting position for interpolation.</param>
		/// <param name="end">Ending position for interpolation.</param>
		/// <param name="amount">
		/// Value between 0 and 1 that defines "distance" between required quaternion and given two.
		/// </param>
		/// <returns>
		/// Quaternion that represents rotation between rotations defined by given two quaternions.
		/// </returns>
		public static Quaternion CreateSphericalLinearInterpolation(Quaternion start, Quaternion end, float amount)
		{
			var q = new Quaternion();
			q.SphericalLinearInterpolation(start, end, amount);
			return q;
		}

		public static Quaternion CreateExpSlerp(Quaternion start, Quaternion end, float amount)
		{
			var q = new Quaternion();
			q.ExpSlerp(start, end, amount);
			return q;
		}
		#endregion
		#region Parsing
		/// <summary>
		/// Parses given text and creates quaternion that represents this text.
		/// </summary>
		/// <remarks>
		/// Correct format = W,X,Y,Z
		/// </remarks>
		/// <param name="value">Text that contains text representation of quaternion.</param>
		/// <returns>Quaternion represented by given text.</returns>
		public static Quaternion Parse(string value)
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

			return new Quaternion(System.Convert.ToSingle(split[0]), System.Convert.ToSingle(split[1]), System.Convert.ToSingle(split[2]), System.Convert.ToSingle(split[3]));
		}
		#endregion
		#endregion
		#region Interface
		#region Predicates
		/// <summary>
		/// Determines whether this quaternion is sufficiently close to another one.
		/// </summary>
		/// <param name="q">Another quaternion.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if this quaternion represents rotation equivalent to one represented by another quaternion.
		/// </returns>
		public bool IsEquivalent(Quaternion q, float epsilon = 0.05f)
		{
			var p = -q;
			bool t0 = (Math.Abs(this.V.X - q.V.X) <= epsilon) && (Math.Abs(this.V.Y - q.V.Y) <= epsilon) && (Math.Abs(this.V.Z - q.V.Z) <= epsilon) && (Math.Abs(this.W - q.W) <= epsilon);
			bool t1 = (Math.Abs(this.V.X - p.V.X) <= epsilon) && (Math.Abs(this.V.Y - p.V.Y) <= epsilon) && (Math.Abs(this.V.Z - p.V.Z) <= epsilon) && (Math.Abs(this.W - p.W) <= epsilon);
			t0 |= t1;
			return t0;
		}
		/// <summary>
		/// Determines whether this quaternion is Identity quaternion.
		/// </summary>
		/// <seealso cref="Quaternion.Identity"/>
		// ReSharper disable CompareOfFloatsByEqualityOperator
		public bool IsIdentity { get { return this.W == 1 && this.V.X == 0 && this.V.Y == 0 && this.V.Z == 0; } }
		// ReSharper restore CompareOfFloatsByEqualityOperator
		/// <summary>
		/// Determines whether modulus of this quaternion is equal 1.
		/// </summary>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>True, if modulus of this quaternion is approximatly equal to 1.</returns>
		public bool IsUnit(float epsilon = 0.05f)
		{
			return Math.Abs(1 - ((this | this))) < epsilon;
		}
		#endregion
		#region Set Rotation
		/// <summary>
		/// Sets value of this quaternion to identity.
		/// </summary>
		/// <seealso cref="Quaternion.Identity"/>
		public void SetIdentity()
		{
			this = Quaternion.Identity;
		}
		/// <summary>
		/// Sets the value of this quaternion to one that represents rotation around given axis by
		/// given angle.
		/// </summary>
		/// <param name="rad">Angle of rotation in radians.</param>
		/// <param name="axis"><see cref="Vector3" /> that defines axis of rotation.</param>
		public void SetRotationAngleAxis(float rad, Vector3 axis)
		{
			float s, c;
			MathHelpers.SinCos(rad * 0.5f, out s, out c);
			this.SetRotationAngleAxis(c, s, axis);
		}
		/// <summary>
		/// Sets the value of this quaternion to one that represents rotation around given axis by
		/// given angle.
		/// </summary>
		/// <param name="cosha">Cosine of angle of rotation.</param>
		/// <param name="sinha">Sine of angle of rotation.</param>
		/// <param name="axis"><see cref="Vector3" /> that defines axis of rotation.</param>
		public void SetRotationAngleAxis(float cosha, float sinha, Vector3 axis)
		{
			this.W = cosha;
			this.V = axis * sinha;
		}
		/// <summary>
		/// Sets the value of this quaternion to one that represents rotation defined by Euler angles.
		/// </summary>
		/// <param name="angle"><see cref="Vector3" /> that defines Euler angles.</param>
		public void SetRotationAroundXYZAxes(Vector3 angle)
		{
			float sx;
			float cx;
			MathHelpers.SinCos(angle.X * 0.5f, out sx, out cx);

			float sy;
			float cy;
			MathHelpers.SinCos(angle.Y * 0.5f, out sy, out cy);

			float sz;
			float cz;
			MathHelpers.SinCos(angle.Z * 0.5f, out sz, out cz);

			this.W = cx * cy * cz + sx * sy * sz;
			this.V.X = cz * cy * sx - sz * sy * cx;
			this.V.Y = cz * sy * cx + sz * cy * sx;
			this.V.Z = sz * cy * cx - cz * sy * sx;
		}
		/// <summary>
		/// Sets the value of this quaternion to one that represents rotation by given angle around X-axis.
		/// </summary>
		/// <param name="r">Angle of rotation in radians.</param>
		public void SetRotationAroundX(float r)
		{
			float s, c;
			MathHelpers.SinCos(r * 0.5f, out s, out c);
			this.W = c;
			this.V.X = s;
			this.V.Y = 0;
			this.V.Z = 0;
		}
		/// <summary>
		/// Sets the value of this quaternion to one that represents rotation by given angle around Y-axis.
		/// </summary>
		/// <param name="r">Angle of rotation in radians.</param>
		public void SetRotationAroundY(float r)
		{
			float s, c;
			MathHelpers.SinCos(r * 0.5f, out s, out c);
			this.W = c;
			this.V.X = 0;
			this.V.Y = s;
			this.V.Z = 0;
		}
		/// <summary>
		/// Sets the value of this quaternion to one that represents rotation by given angle around Z-axis.
		/// </summary>
		/// <param name="r">Angle of rotation in radians.</param>
		public void SetRotationAroundZ(float r)
		{
			float s, c;
			MathHelpers.SinCos(r * 0.5f, out s, out c);
			this.W = c;
			this.V.X = 0;
			this.V.Y = 0;
			this.V.Z = s;
		}
		/// <summary>
		/// Sets the value of this quaternion to one that represents rotation from first vector two
		/// the sector via shortest arc.
		/// </summary>
		/// <param name="one">First vector.</param>
		/// <param name="two">Second vector.</param>
		/// <param name="fallbackAxis">
		/// Axis to use to represent 180 degrees rotation when turns out, that two vectors are
		/// collinear about point in opposite directions.
		/// </param>
		public void SetRotationFrom2Vectors(Vector3 one, Vector3 two, Vector3 fallbackAxis = new Vector3())
		{
			// Based on Stan Melax's article in Game Programming Gems Copy, since cannot modify local
			Vector3 v0 = one;
			Vector3 v1 = two;
			v0.Normalize();
			v1.Normalize();

			float d = v0 | v1;
			// If dot == 1, vectors are the same
			if (d >= 1.0f)
			{
				this.SetIdentity();
			}
			else if (d < (1e-6f - 1.0f))
			{
				if (fallbackAxis != Vector3.Zero)
				{
					// rotate 180 degrees around given fallback axis
					this.SetRotationAngleAxis((float)Math.PI, fallbackAxis);
				}
				else
				{
					// Generate an axis
					Vector3 axis = new Vector3(1, 0, 0).Cross(one);
					if (Math.Abs(axis.Length) < MathHelpers.ZeroTolerance) // pick another if colinear
						axis = new Vector3(0, 0, -1).Cross(one);
					axis.Normalize();
					this.SetRotationAngleAxis((float)Math.PI, axis);
				}
			}
			else
			{
				float s = (float)Math.Sqrt((1 + d) * 2);
				float invs = 1 / s;

				Vector3 c = v0.Cross(v1);

				this.V.X = c.X * invs;
				this.V.Y = c.Y * invs;
				this.V.Z = c.Z * invs;
				this.W = s * 0.5f;
				this.Normalize();
			}
		}
		/// <summary>
		/// Sets value of this quaternion to value that represents rotation to given vector from
		/// <see cref="Vector3.Up" />(?) vector.
		/// </summary>
		/// <remarks>
		/// This method is used when we need to make an entity to look in a direction described by
		/// the <paramref name="vdir">vector</paramref>.
		/// </remarks>
		/// <param name="vdir">
		/// <see cref="Vector3" /> that represents direction to which we need to find a rotation.
		/// </param>
		public void SetRotationToViewDirection(Vector3 vdir)
		{
			if (!vdir.IsUnit(0.01f))
			{
				throw new ArgumentException("To create look-at quaternion we need a vector of unit length.");
			}
			//set default initialization for up-vector.
			this.W = 0.70710676908493042f;
			this.V.X = vdir.Z * 0.70710676908493042f;
			this.V.Y = 0;
			this.V.Z = 0;
			float l = (float)Math.Sqrt(vdir.X * vdir.X + vdir.Y * vdir.Y);
			if (l > 0.00001f)
			{
				//calculate LookAt quaternion
				Vector3 hv = new Vector3(vdir.X / l, vdir.Y / l + 1.0f, l + 1.0f);
				float r = (float)Math.Sqrt(hv.X * hv.X + hv.Y * hv.Y);
				float s = (float)Math.Sqrt(hv.Z * hv.Z + vdir.Z * vdir.Z);
				//generate the half-angle sine&cosine
				float hacos0 = 0;
				float hasin0 = -1;
				if (r > 0.00001f)
				{
					hacos0 = hv.Y / r;
					hasin0 = -hv.X / r;
				}	//yaw
				float hacos1 = hv.Z / s;
				float hasin1 = vdir.Z / s;					//pitch
				this.W = hacos0 * hacos1;
				this.V.X = hacos0 * hasin1;
				this.V.Y = hasin0 * hasin1;
				this.V.Z = hasin0 * hacos1;
			}
		}
		/// <summary>
		/// Sets value of this quaternion to value that represents rotation to given vector from
		/// <see cref="Vector3.Up" />(?) vector.
		/// </summary>
		/// <param name="vdir">
		/// <see cref="Vector3" /> that represents direction to which we need to find a rotation.
		/// </param>
		/// <param name="r">Angle of rotation around forward axis to apply to new quaternion.</param>
		/// <seealso cref="Quaternion.SetRotationToViewDirection(Vector3)"/>
		public void SetRotationToViewDirection(Vector3 vdir, float r)
		{
			this.SetRotationToViewDirection(vdir);
			float sy, cy; MathHelpers.SinCos(r * 0.5f, out sy, out cy);
			float vx = this.V.X, vy = this.V.Y;
			this.V.X = vx * cy - this.V.Z * sy;
			this.V.Y = this.W * sy + vy * cy;
			this.V.Z = this.V.Z * cy + vx * sy;
			this.W = this.W * cy - vy * sy;
		}
		#endregion
		#region Basic Operations
		/// <summary>
		/// Conjugates this quaternion.
		/// </summary>
		public void Conjugate()
		{
			this = !this;
		}
		/// <summary>
		/// Normalizes this quaternion.
		/// </summary>
		public void Normalize()
		{
			float d = MathHelpers.ReciprocalSquareRoot(this.W * this.W + this.V.X * this.V.X + this.V.Y * this.V.Y + this.V.Z * this.V.Z);
			this.W *= d; this.V.X *= d; this.V.Y *= d; this.V.Z *= d;
		}
		/// <summary>
		/// Normalizes this quaternion or just sets it to identity if its too small.
		/// </summary>
		/// <see cref="Quaternion.Identity"/>
		public void NormalizeSafe()
		{
			float d = this.W * this.W + this.V.X * this.V.X + this.V.Y * this.V.Y + this.V.Z * this.V.Z;
			if (d > 1e-8f)
			{
				d = MathHelpers.ReciprocalSquareRoot(d);
				this.W *= d; this.V.X *= d; this.V.Y *= d; this.V.Z *= d;
			}
			else
				this.SetIdentity();
		}
		/// <summary>
		/// Makes sure that angle of rotation is not bigger then given number.
		/// </summary>
		/// <param name="maxAngle">Max angle in radians</param>
		public void ClampAngle(float maxAngle)
		{
			var wMax = Math.Cos(2.0f * maxAngle);
			if (this.W < wMax)
			{
				this.W = (float)wMax;
				this.Normalize();
			}
		}
		#endregion
		#region Interpolations
		/// <summary>
		/// Sets this quaternion to result of normalized linear interpolation.
		/// </summary>
		/// <param name="start">Starting quaternion in interpolation.</param>
		/// <param name="end">Ending quaternion in interpolation.</param>
		/// <param name="amount">
		/// Number between 0 and 1 that sets position of resulting quaternion between <paramref
		/// name="start" /> and <paramref name="end" />.
		/// </param>
		public void NormalizedLinearInterpolation(Quaternion start, Quaternion end, float amount)
		{
			var q = end;
			if ((start | q) < 0) { q = -q; }

			var vDiff = q.V - start.V;

			this.V = start.V + (vDiff * amount);
			this.W = start.W + ((q.W - start.W) * amount);

			this.Normalize();
		}
		/// <summary>
		/// Sets this quaternion to result of normalized linear interpolation using a different algorithm.
		/// </summary>
		/// <param name="start">Starting quaternion in interpolation.</param>
		/// <param name="end">Ending quaternion in interpolation.</param>
		/// <param name="amount">
		/// Number between 0 and 1 that sets position of resulting quaternion between <paramref
		/// name="start" /> and <paramref name="end" />.
		/// </param>
		public void NormalizedLinearInterpolation2(Quaternion start, Quaternion end, float amount)
		{
			var q = end;
			var cosine = (start | q);
			if (cosine < 0) q = -q;
			var k = (1 - Math.Abs(cosine)) * 0.4669269f;
			var s = 2 * k * amount * amount * amount - 3 * k * amount * amount + (1 + k) * amount;
			this.V.X = start.V.X * (1.0f - s) + q.V.X * s;
			this.V.Y = start.V.Y * (1.0f - s) + q.V.Y * s;
			this.V.Z = start.V.Z * (1.0f - s) + q.V.Z * s;
			this.W = start.W * (1.0f - s) + q.W * s;
			this.Normalize();
		}
		/// <summary>
		/// Sets this quaternion to result of spherical linear interpolation.
		/// </summary>
		/// <param name="start">Starting quaternion in interpolation.</param>
		/// <param name="end">Ending quaternion in interpolation.</param>
		/// <param name="amount">
		/// Number between 0 and 1 that sets position of resulting quaternion between <paramref
		/// name="start" /> and <paramref name="end" />.
		/// </param>
		public void SphericalLinearInterpolation(Quaternion start, Quaternion end, float amount)
		{
			var p = start;
			var q = end;
			var q2 = new Quaternion();

			var cosine = (p | q);
			if (cosine < 0.0f) { cosine = -cosine; q = -q; } // take shortest arc
			if (cosine > 0.9999f)
			{
				this.NormalizedLinearInterpolation(p, q, amount);
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
			this.W = (float)(p.W * c + q2.W * s / sine);
			this.V.X = (float)(p.V.X * c + q2.V.X * s / sine);
			this.V.Y = (float)(p.V.Y * c + q2.V.Y * s / sine);
			this.V.Z = (float)(p.V.Z * c + q2.V.Z * s / sine);
		}

		public void ExpSlerp(Quaternion start, Quaternion end, float amount)
		{
			var q = end;
			if ((start | q) < 0) { q = -q; }
			this = start * MathHelpers.Exp(MathHelpers.Log(!start * q) * amount);
		}
		/// <summary>
		/// Gets quaternion that is this quaternion scaled by given factor.
		/// </summary>
		/// <param name="scale">
		/// Number that represents a scale of new quaternion in relation to this one.
		/// </param>
		/// <returns>Quaternion that is this quaternion scaled by given factor.</returns>
		public Quaternion GetScaled(float scale)
		{
			return CreateNormalizedLinearInterpolation(Quaternion.Identity, this, scale);
		}
		#endregion
		#region Overrides
		/// <summary>
		/// Calculates hash code of this quaternion.
		/// </summary>
		/// <returns>Hash code of this quaternion.</returns>
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				// ReSharper disable NonReadonlyFieldInGetHashCode
				hash = hash * 23 + this.W.GetHashCode();
				hash = hash * 23 + this.V.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode

				return hash;
			}
		}
		/// <summary>
		/// Determines whether this quaternion is equal to given object.
		/// </summary>
		/// <param name="obj">Given object.</param>
		/// <returns>True, if object is quaternion equal to this one.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj is Quaternion)
				return this == (Quaternion)obj;

			return false;
		}
		#endregion
		#endregion
		#region Utilities

		#endregion
	}
}