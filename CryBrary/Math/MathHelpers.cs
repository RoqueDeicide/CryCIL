using System;

using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace CryEngine
{
    public static class MathHelpers
    {
        public static void Interpolate(ref Vec3 actual, Vec3 goal, float speed, float limit = 0)
        {
            var delta = goal - actual;

            if (limit > 0.001f)
            {
                float length = delta.Length;

                if (length > limit)
                {
                    delta /= length;
                    delta *= limit;
                }
            }

            actual += delta * Min(Time.DeltaTime * speed, 1.0f);
        }

        public static void Interpolate(ref Vec2 actual, Vec2 goal, float speed, float limit = 0)
        {
            var delta = goal - actual;

            if (limit > 0.001f)
            {
                float length = delta.Length;

                if (length > limit)
                {
                    delta /= length;
                    delta *= limit;
                }
            }

            actual += delta * Min(Time.DeltaTime * speed, 1.0f);
        }

        public static void Interpolate(ref float actual, float goal, float speed, float limit = 0)
        {
            float delta = goal - actual;

            if (limit > 0.001f)
                delta = Max(Min(delta, limit), -limit);

            actual += delta * Min(Time.DeltaTime * speed, 1.0f);
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0f;
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float DegreesToRadians(float angle)
        {
            return (float)Math.PI * angle / 180.0f;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float RadiansToDegrees(float angle)
        {
            return (float)(angle * (180.0 / Math.PI));
        }

        public static double ISqrt(double d)
        {
            return 1.0 / Math.Sqrt(d);
        }

        public static float ISqrt(float d)
        {
            return (float)(1.0 / Math.Sqrt(d));
        }

        public static void SinCos(double a, out double sinVal, out double cosVal)
        {
            sinVal = Math.Sin(a);

            cosVal = Math.Sqrt(1.0 - sinVal * sinVal);
        }

        public static void SinCos(float a, out float sinVal, out float cosVal)
        {
            sinVal = (float)Math.Sin(a);

            cosVal = (float)Math.Sqrt(1.0f - sinVal * sinVal);
        }

        public static Vec3 Log(Quat q)
        {
            var lensqr = q.V.LengthSquared;
            if (lensqr > 0.0f)

            // Exponent of Quaternion.
            {
                var len = Math.Sqrt(lensqr);
                var angle = Math.Atan2(len, q.W) / len;
                return q.V * (float)angle;
            }

            // logarithm of a quaternion, imaginary part (the real part of the logarithm is always 0)
            return new Vec3(0);
        }

        public static Quat Exp(Vec3 v)
        {
            var lensqr = v.LengthSquared;
            if (lensqr > 0.0f)
            {
                var len = (float)Math.Sqrt(lensqr);
                float s, c; 
                SinCos(len, out s, out c);
                s /= len;
                return new Quat(c, v.X * s, v.Y * s, v.Z * s);
            }
            return Quat.Identity;
        }

        /// <summary>
        /// Determines whether a value is inside the specified range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsInRange<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Clamps a value given a specified range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;

            return value;
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;

            return Clamp(angle, min, max);
        }

        public static T Max<T>(T val1, T val2) where T : IComparable<T>
        {
            if (val1.CompareTo(val2) > 0)
                return val1;

            return val2;
        }

        public static T Min<T>(T val1, T val2) where T : IComparable<T>
        {
            if (val1.CompareTo(val2) < 0)
                return val1;

            return val2;
        }

		public static bool IsPowerOfTwo(int value)
		{
			return (value & (value - 1)) == 0;
		}

		public static bool IsNumberValid(double value)
		{
			var mask = (UInt64)(255 << 55);

			return ((UInt64)value & mask) != mask;
		}

		public static bool IsNumberValid(float value)
		{
			var mask = 0xFF << 23;

			return ((UInt32)value & mask) != mask;
		}

        /// <summary>
        /// The value for which all absolute numbers smaller than are considered equal to zero.
        /// </summary>
        public const float ZeroTolerance = 1e-6f;
    }
}
