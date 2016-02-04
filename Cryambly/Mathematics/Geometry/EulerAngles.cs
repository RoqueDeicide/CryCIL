﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents Euler angles.
	/// </summary>
	/// <remarks>
	/// Most functions that use objects of this type apply the rotation in order X-Y-Z (Pitch-Roll-Yaw).
	/// </remarks>
	public struct EulerAngles
	{
		#region Fields
		/// <summary>
		/// Angle of orientation around X axis.
		/// </summary>
		public float Pitch;
		/// <summary>
		/// Angle of orientation around Y axis.
		/// </summary>
		public float Roll;
		/// <summary>
		/// Angle of orientation around Z axis.
		/// </summary>
		public float Yaw;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets component of angles specified by given index.
		/// </summary>
		/// <param name="index">Index of component.</param>
		/// <exception cref="IndexOutOfRangeException">Index out of range.</exception>
		public float this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return this.Pitch;
					case 1:
						return this.Roll;
					case 2:
						return this.Yaw;
					default:
						throw new IndexOutOfRangeException("[Angle3.this[]] Index out of range.");
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						this.Pitch = value;
						break;
					case 1:
						this.Roll = value;
						break;
					case 2:
						this.Yaw = value;
						break;
					default:
						throw new IndexOutOfRangeException("[Angle3.this[]] Index out of range.");
				}
			}
		}
		#endregion
		#region Interface
		#region Constructors
		/// <summary>
		/// Creates new instance of <see cref="EulerAngles"/> struct.
		/// </summary>
		/// <param name="allAngles">A value to assign to all angles.</param>
		public EulerAngles(float allAngles)
		{
			this.Pitch = allAngles;
			this.Roll = allAngles;
			this.Yaw = allAngles;
		}
		/// <summary>
		/// Creates new instance of <see cref="EulerAngles"/> struct.
		/// </summary>
		/// <param name="pitch">Angle of rotation around X-axis.</param>
		/// <param name="roll"> Angle of rotation around Y-axis.</param>
		/// <param name="yaw">  Angle of rotation around Z-axis.</param>
		public EulerAngles(float pitch, float roll, float yaw)
		{
			this.Pitch = pitch;
			this.Roll = roll;
			this.Yaw = yaw;
		}
		/// <summary>
		/// Creates new instance of <see cref="EulerAngles"/> struct.
		/// </summary>
		/// <param name="matrix">Matrix that defines new instance.</param>
		public EulerAngles(Matrix33 matrix)
		{
			// Assert matrix being orthonormal.
			this.Roll = (float)Math.Asin(Math.Max(-1.0f, Math.Min(1.0f, -matrix.M20)));
			if (Math.Abs(Math.Abs(this.Roll) - (float)(Math.PI * 0.5)) < 0.01f)
			{
				this.Pitch = 0;
				this.Yaw = (float)Math.Atan2(-matrix.M01, matrix.M11);
			}
			else
			{
				this.Pitch = (float)Math.Atan2(matrix.M21, matrix.M22);
				this.Yaw = (float)Math.Atan2(matrix.M10, matrix.M00);
			}
		}
		#endregion
		#region Operators
		#region Comparison Operators
		/// <summary>
		/// Determines whether two instances of <see cref="EulerAngles"/> struct are equal.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>True, if objects are equal, otherwise false.</returns>
		public static bool operator ==(EulerAngles l, EulerAngles r)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return l.Pitch == r.Pitch && l.Roll == r.Roll && l.Yaw == r.Yaw;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		/// <summary>
		/// Determines whether two instances of <see cref="EulerAngles"/> struct are not equal.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>True, if objects are not equal, otherwise false.</returns>
		public static bool operator !=(EulerAngles l, EulerAngles r)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return l.Pitch != r.Pitch && l.Roll != r.Roll && l.Yaw != r.Yaw;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		#endregion
		#region Arithmetic Operators
		/// <summary>
		/// Combines 2 angle objects together.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of addition</returns>
		public static EulerAngles operator +(EulerAngles left, EulerAngles right)
		{
			return new EulerAngles(left.Pitch + right.Pitch, left.Roll + right.Roll, left.Yaw + right.Yaw);
		}
		/// <summary>
		/// Subtracts 1 angle object from another.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>Result of subtraction.</returns>
		public static EulerAngles operator -(EulerAngles left, EulerAngles right)
		{
			return new EulerAngles(left.Pitch - right.Pitch, left.Roll - right.Roll, left.Yaw - right.Yaw);
		}
		/// <summary>
		/// Multiplies given instance of <see cref="EulerAngles"/> struct by given amount.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static EulerAngles operator *(EulerAngles l, float r)
		{
			return new EulerAngles(l.Pitch * r, l.Roll * r, l.Yaw * r);
		}
		/// <summary>
		/// Multiplies given instance of <see cref="EulerAngles"/> struct by given amount.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of multiplication.</returns>
		public static EulerAngles operator *(float l, EulerAngles r)
		{
			return new EulerAngles(r.Pitch * l, r.Roll * l, r.Yaw * l);
		}
		/// <summary>
		/// Divides given instance of <see cref="EulerAngles"/> struct by given amount.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of division.</returns>
		public static EulerAngles operator /(EulerAngles l, float r)
		{
			return new EulerAngles(l.Pitch / r, l.Roll / r, l.Yaw / r);
		}
		/// <summary>
		/// Divides given instance of <see cref="EulerAngles"/> struct by given amount.
		/// </summary>
		/// <param name="l">Left operand.</param>
		/// <param name="r">Right operand.</param>
		/// <returns>Result of division.</returns>
		public static EulerAngles operator /(float l, EulerAngles r)
		{
			return new EulerAngles(r.Pitch / l, r.Roll / l, r.Yaw / l);
		}
		/// <summary>
		/// Creates negated <see cref="EulerAngles"/> instance.
		/// </summary>
		/// <param name="angle">Object to negate.</param>
		/// <returns>Result.</returns>
		public static EulerAngles operator -(EulerAngles angle)
		{
			return new EulerAngles(-angle.Pitch, -angle.Roll, -angle.Yaw);
		}
		#endregion
		#region Conversion Operators
		/// <summary>
		/// Converts unit quaternion to angles.
		/// </summary>
		/// <param name="q">Quaternion to convert.</param>
		/// <returns>Result of conversion.</returns>
		public static explicit operator EulerAngles(Quaternion q)
		{
			EulerAngles result = new EulerAngles
			{
				Roll = System.Convert.ToSingle(Math.Asin(Math.Max(-1.0f, Math.Min(1.0f, -(q.X * q.Z - q.W * q.Y) * 2))))
			};
			if (Math.Abs(Math.Abs(result.Roll) - (float)(Math.PI * 0.5)) < 0.01f)
			{
				result.Pitch = 0;
				result.Yaw = (float)Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), 1 - (q.X * q.X + q.Z * q.Z) * 2);
			}
			else
			{
				result.Pitch = (float)Math.Atan2(-2 * (q.Y * q.Z - q.W * q.X), 1 - (q.X * q.X + q.Y * q.Y) * 2);
				result.Yaw = (float)Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), 1 - (q.Z * q.Z + q.Y * q.Y) * 2);
			}
			return result;
		}
		/// <summary>
		/// Creates vector representation of given Euler angle.
		/// </summary>
		/// <param name="angle">Angle to convert.</param>
		/// <returns>Equivalent of Euler angle.</returns>
		public static explicit operator Vector3(EulerAngles angle)
		{
			return new Vector3(angle.Pitch, angle.Roll, angle.Yaw);
		}
		#endregion
		#endregion
		#region Overrides
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			return this.Pitch.GetHashCode() * 11 + this.Roll.GetHashCode() * 37 + this.Yaw.GetHashCode() * 23;
		}
		/// <summary>
		/// Checks equality of this object and given one.
		/// </summary>
		/// <param name="obj">Given object.</param>
		/// <returns>
		/// True, if given object is <see cref="EulerAngles"/> equal to this instance, otherwise false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (!(obj is EulerAngles)) return false;
			EulerAngles o = (EulerAngles)obj;
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return o.Pitch == this.Pitch && this.Roll == o.Roll && this.Yaw == o.Yaw;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		#endregion
		#region Comparison
		/// <summary>
		/// Determines whether this instance of type <see cref="EulerAngles"/> can be considered equal to
		/// another.
		/// </summary>
		/// <param name="other">    Another set of angles.</param>
		/// <param name="precision">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between components of the angles is less then <paramref name="precision"/>.
		/// </returns>
		public bool IsEquivalent(EulerAngles other, float precision = MathHelpers.ZeroTolerance)
		{
			return
				Math.Abs(this.Pitch - other.Pitch) < precision &&
				Math.Abs(this.Roll - other.Roll) < precision &&
				Math.Abs(this.Yaw - other.Yaw) < precision;
		}
		/// <summary>
		/// Indicates whether this set of Euler angles is represented by a unit vector.
		/// </summary>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between this vector and unit vector is within specified precision bounds.
		/// </returns>
		public bool IsUnit(float epsilon = 0.05f)
		{
			return Math.Abs(1 - (this.Roll * this.Roll + this.Pitch * this.Pitch + this.Yaw * this.Yaw)) <= epsilon;
		}
		#endregion
		#region Angle Operations
		/// <summary>
		/// Normalizes angles encapsulated by this object into [0, 2 * PI] range.
		/// </summary>
		public void Normalize2PI()
		{
			this.Normalize(0, (float)MathHelpers.PI2, (float)MathHelpers.PI2);
		}
		/// <summary>
		/// Normalizes angles encapsulated by this object into [-PI, PI] range.
		/// </summary>
		public void NormalizePI()
		{
			this.Normalize(-(float)Math.PI, (float)Math.PI, (float)MathHelpers.PI2);
		}
		/// <summary>
		/// Normalizes angles encapsulated by this object into [0, 360] range.
		/// </summary>
		public void Normalize360()
		{
			this.Normalize(0, 360, 360);
		}
		/// <summary>
		/// Normalizes angles encapsulated by this object into [-180, 180] range.
		/// </summary>
		public void Normalize180()
		{
			this.Normalize(-180, 180, 360);
		}
		#endregion
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Normalize(float low, float high, float step)
		{
			while (this.Pitch < low)
				this.Pitch += step;
			while (this.Pitch > high)
				this.Pitch -= step;
			while (this.Roll < low)
				this.Roll += step;
			while (this.Roll > high)
				this.Roll -= step;
			while (this.Yaw < low)
				this.Yaw += step;
			while (this.Yaw > high)
				this.Yaw -= step;
		}
		#endregion
	}
}