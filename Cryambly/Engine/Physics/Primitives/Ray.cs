using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics.Primitives
{
	public static partial class Primitive
	{
		/// <summary>
		/// Represents a primitive geometric object that represents a ray.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		[PrimitiveType]
		public struct Ray
		{
			/// <summary>
			/// Identifier of this type of primitives.
			/// </summary>
			public const int Id = 3;
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)]
			public BasePrimitive Base;
			/// <summary>
			/// Coordinates of the starting point of the ray.
			/// </summary>
			[FieldOffset(0)]
			public Vector3 Origin;
			/// <summary>
			/// Direction of the ray.
			/// </summary>
			[FieldOffset(12)]
			public Vector3 Direction;
		}
	}
}