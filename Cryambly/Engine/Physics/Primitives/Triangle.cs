using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics.Primitives
{
	public static partial class Primitive
	{
		/// <summary>
		/// Represents a primitive geometric object that represents a triangle.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		public struct Triangle
		{
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)] public BasePrimitive Base;
			/// <summary>
			/// First point of this triangle.
			/// </summary>
			[FieldOffset(0)] public Vector3 First;
			/// <summary>
			/// Second point of this triangle.
			/// </summary>
			[FieldOffset(12)] public Vector3 Second;
			/// <summary>
			/// Third point of this triangle.
			/// </summary>
			[FieldOffset(24)] public Vector3 Third;
			/// <summary>
			/// A normal vector to the surface of this triangle.
			/// </summary>
			[FieldOffset(36)] public Vector3 Normal;
		}
		/// <summary>
		/// Represents a triangle primitive with an index.
		/// </summary>
		public struct IndexedTriangle
		{
			/// <summary>
			/// Base part of this object.
			/// </summary>
			public Triangle Base;
			/// <summary>
			/// Some index.
			/// </summary>
			public int Index;
		}
	}
}