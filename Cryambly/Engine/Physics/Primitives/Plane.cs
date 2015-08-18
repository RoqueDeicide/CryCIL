using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics.Primitives
{
	public static partial class Primitive
	{
		/// <summary>
		/// Represents a primitive geometric object that represents a plane.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct Plane
		{
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)] public BasePrimitive Base;
			/// <summary>
			/// A vector that is perpendicular to this plane.
			/// </summary>
			[FieldOffset(0)] public Vector3 Normal;
			/// <summary>
			/// Coordinates of one point on this plane.
			/// </summary>
			[FieldOffset(12)] public Vector3 Origin;
		}
	}
}