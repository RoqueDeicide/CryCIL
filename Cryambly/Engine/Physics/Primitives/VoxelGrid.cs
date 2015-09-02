using System;
using System.Runtime.InteropServices;
using CryCil.Utilities;

namespace CryCil.Engine.Physics.Primitives
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct Grid3DDescription
	{
		internal Matrix33 basis;
		internal int oriented;
		internal Vector3 origin;
		internal Vector3 step;
		internal Vector3 stepr;
		internal Vector3Int32 size;
		internal Vector3Int32 stride;
		internal Matrix33 R;
		internal Vector3 offset;
		internal float scale, rscale;
		internal StridedPointer pVtx;
		internal int* pIndices;
		internal Vector3* pNormals;
		internal char* pIds;
		internal int* pCellTris;
		internal int* pTriBuf;
	}
	public static partial class Primitive
	{
		/// <summary>
		/// Represents a primitive geometric object that represents a height map.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		[PrimitiveType]
		public struct VoxelGrid
		{
			#region Fields
			/// <summary>
			/// Identifier of this type of primitives.
			/// </summary>
			public const int Id = 7;
			/// <summary>
			/// You can pass a reference to this field to any method that accepts reference to
			/// <see cref="BasePrimitive"/> object.
			/// </summary>
			[FieldOffset(0)]
			public BasePrimitive Base;
			[FieldOffset(0)]
			private Grid3DDescription desc;
			#endregion
			#region Properties
			/// <summary>
			/// Gets or sets the matrix which rows form a basis of grid's local coordinate system.
			/// </summary>
			public Matrix33 Basis
			{
				get { return this.desc.basis; }
				set
				{
					this.desc.basis = value;
					if (this.desc.basis.IsEquivalent(Matrix33.Identity))
					{
						this.desc.oriented = 0;
					}
					else if (!this.desc.basis.IsOrthonormal)
					{
						throw new ArgumentException("Given matrix must be orthonormal.");
					}
					else
					{
						this.desc.oriented = 1;
					}
				}
			}
			/// <summary>
			/// Gets or sets the coordinates of the origin point of the grid.
			/// </summary>
			public Vector3 Origin
			{
				get { return this.desc.origin; }
				set { this.desc.origin = value; }
			}
			#endregion
		}
	}
}