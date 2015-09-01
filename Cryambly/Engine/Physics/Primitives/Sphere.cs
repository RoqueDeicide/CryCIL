using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics.Primitives
{
	public static partial class Primitive
	{
		/// <summary>
		/// Represents a primitive geometric object that represents a sphere.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		[PrimitiveType]
		public struct Sphere
		{
			/// <summary>
			/// Identifier of this type of primitives.
			/// </summary>
			public const int Id = 4;
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)] public BasePrimitive Base;
			/// <summary>
			/// Coordinates of the center of the sphere.
			/// </summary>
			[FieldOffset(0)] public Vector3 Center;
			/// <summary>
			/// Radius of the sphere.
			/// </summary>
			[FieldOffset(12)] public float Radius;
		}
	}
}