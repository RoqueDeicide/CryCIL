using System;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics.Primitives
{
	public static partial class Primitive
	{
		/// <summary>
		/// Represents a primitive geometric object that represents a box.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		[PrimitiveType]
		public struct Box
		{
			/// <summary>
			/// Identifier of this type of primitives.
			/// </summary>
			public const int Id = 0;
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)] public BasePrimitive Base;
			/// <summary>
			/// A 3x3 matrix that defines orientation of this box.
			/// </summary>
			[FieldOffset(0)] private Matrix33 basis;
			[FieldOffset(36)] private bool oriented;
			/// <summary>
			/// Position of the center of the box in world-space(?) coordinates.
			/// </summary>
			[FieldOffset(40)] public Vector3 Center;
			/// <summary>
			/// Dimensions of this box along respective axes of box's local coordinate space.
			/// </summary>
			[FieldOffset(52)] public Vector3 Size;
			/// <summary>
			/// Gets or sets the matrix which rows form a basis of box's local coordinate system.
			/// </summary>
			public Matrix33 Basis
			{
				get { return this.basis; }
				set
				{
					this.basis = value;
					if (this.basis.IsEquivalent(Matrix33.Identity))
					{
						this.oriented = false;
					}
					else if (!this.basis.IsOrthonormal)
					{
						throw new ArgumentException("Given matrix must be orthonormal.");
					}
					else
					{
						this.oriented = true;
					}
				}
			}
		}
	}
}