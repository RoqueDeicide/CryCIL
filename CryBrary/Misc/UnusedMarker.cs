using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using CryEngine.Mathematics;

namespace CryEngine.Utilities
{
	public static class UnusedMarker
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct f2i
		{
			[FieldOffset(0)]
			public float f;

			[FieldOffset(0)]
			public UInt32 i;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct d2i
		{
			[FieldOffset(0)]
			public double d;

			[FieldOffset(0)]
			public UInt32 i;
		}

		public static float Float
		{
			get
			{
				var u = new f2i();
				u.i = 0xFFBFFFFF;

				return u.f;
			}
		}

		public static bool IsUnused(float var)
		{
			var u = new f2i();
			u.f = var;
			return (u.i & 0xFFA00000) == 0xFFA00000;
		}

		public static int Integer
		{
			get
			{
				return 1 << 31;
			}
		}

		public static bool IsUnused(int var)
		{
			return var == 1 << 31;
		}

		[CLSCompliant(false)]
		public static uint UnsignedInteger
		{
			get
			{
				return 1u << 31;
			}
		}

		[CLSCompliant(false)]
		public static bool IsUnused(uint var)
		{
			return var == 1u << 31;
		}

		public static Vector3 Vector3
		{
			get
			{
				return new Vector3(Float);
			}
		}

		public static bool IsUnused(Vector3 var)
		{
			return IsUnused(var.X);
		}

		public static Quaternion Quaternion
		{
			get
			{
				var q = new Quaternion();

				q.W = Float;
				return q;
			}
		}

		public static bool IsUnused(Quaternion var)
		{
			return IsUnused(var.W);
		}

		public static IntPtr IntPtr
		{
			get
			{
				return new IntPtr(-1);
			}
		}

		public static bool IsUnused(IntPtr var)
		{
			return var.ToInt32() == -1;
		}
	}
}