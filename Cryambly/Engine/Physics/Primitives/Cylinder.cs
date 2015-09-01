using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics.Primitives
{
	public static partial class Primitive
	{
		/// <summary>
		/// Represents a primitive geometric object that represents a cylinder.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		[PrimitiveType]
		public struct Cylinder
		{
			/// <summary>
			/// Identifier of this type of primitives.
			/// </summary>
			public const int Id = 5;
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)] public BasePrimitive Base;
			/// <summary>
			/// Coordinates of the center of the cylinder.
			/// </summary>
			[FieldOffset(0)] public Vector3 Center;
			/// <summary>
			/// Direction of the axis of this cylinder.
			/// </summary>
			[FieldOffset(12)] public Vector3 Axis;
			/// <summary>
			/// Radius of the cylinder.
			/// </summary>
			[FieldOffset(24)] public float Radius;
			/// <summary>
			/// Height of the cylinder.
			/// </summary>
			[FieldOffset(28)] public float Height;
		}
		/// <summary>
		/// Represents a primitive geometric object that represents a capsule.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		[PrimitiveType]
		public struct Capsule
		{
			/// <summary>
			/// Identifier of this type of primitives.
			/// </summary>
			public const int Id = 6;
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)] public BasePrimitive Base;
			/// <summary>
			/// Coordinates of the center of the capsule.
			/// </summary>
			[FieldOffset(0)] public Vector3 Center;
			/// <summary>
			/// Direction of the axis of this capsule.
			/// </summary>
			[FieldOffset(12)] public Vector3 Axis;
			/// <summary>
			/// Radius of the capsule.
			/// </summary>
			[FieldOffset(24)] public float Radius;
			/// <summary>
			/// Height of the capsule.
			/// </summary>
			[FieldOffset(28)] public float Height;
		}
	}
}