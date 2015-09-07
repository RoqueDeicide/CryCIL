﻿using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
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
			[FieldOffset(0)] public BasePrimitive Base;
			[FieldOffset(0)] private Grid3DDescription desc;
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
			#region Interface
			/// <summary>
			/// Checks whether this primitive intersects with anything in the physical world.
			/// </summary>
			/// <param name="parameters">    
			/// Reference to object that specifies parameters of intersection.
			/// </param>
			/// <param name="queryFlags">    
			/// A set of flags that specifies which objects to test against and how.
			/// </param>
			/// <param name="flagsAll">      
			/// A set of flags all of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="flagsAny">      
			/// A set of flags any of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="collisionClass">
			/// An object that represents collision class of this primitive.
			/// </param>
			/// <param name="entitiesToSkip">
			/// An optional array of entities to ignore during the test.
			/// </param>
			/// <returns>An array of contacts this primitive has with other entities.</returns>
			[CanBeNull]
			[Pure]
			public GeometryContact[] Intersect(ref IntersectionParameters parameters,
											   EntityQueryFlags queryFlags = EntityQueryFlags.All,
											   PhysicsGeometryFlags flagsAll = (PhysicsGeometryFlags)0,
											   PhysicsGeometryFlags flagsAny = PhysicsGeometryFlags.CollisionTypeDefault |
																			   PhysicsGeometryFlags.CollisionTypePlayer,
											   CollisionClass collisionClass = new CollisionClass(),
											   PhysicalEntity[] entitiesToSkip = null)
			{
				GeometryContact[] contacts;
				int count = PhysicalWorld.PrimitiveIntersection(out contacts, ref this.Base, Id, queryFlags, flagsAll,
																flagsAny, ref parameters, ref collisionClass,
																entitiesToSkip);
				return count == 0 ? null : contacts;
			}
			/// <summary>
			/// Checks whether this primitive intersects with anything in the physical world.
			/// </summary>
			/// <param name="queryFlags">    
			/// A set of flags that specifies which objects to test against and how.
			/// </param>
			/// <param name="flagsAll">      
			/// A set of flags all of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="flagsAny">      
			/// A set of flags any of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="collisionClass">
			/// An object that represents collision class of this primitive.
			/// </param>
			/// <param name="entitiesToSkip">
			/// An optional array of entities to ignore during the test.
			/// </param>
			/// <returns>An array of contacts this primitive has with other entities.</returns>
			[CanBeNull]
			[Pure]
			public GeometryContact[] Intersect(EntityQueryFlags queryFlags = EntityQueryFlags.All,
											   PhysicsGeometryFlags flagsAll = (PhysicsGeometryFlags)0,
											   PhysicsGeometryFlags flagsAny = PhysicsGeometryFlags.CollisionTypeDefault |
																			   PhysicsGeometryFlags.CollisionTypePlayer,
											   CollisionClass collisionClass = new CollisionClass(),
											   PhysicalEntity[] entitiesToSkip = null)
			{
				GeometryContact[] contacts;
				int count =
					PhysicalWorld.PrimitiveIntersection(out contacts, ref this.Base, Id, queryFlags,
														flagsAll, flagsAny, ref IntersectionParameters.Default,
														ref collisionClass, entitiesToSkip);
				return count == 0 ? null : contacts;
			}
			/// <summary>
			/// Casts this primitive along <paramref name="direction"/> from its current position.
			/// </summary>
			/// <param name="contact">       
			/// Resultant contact. Only assigned if returned value is not less then 0.
			/// </param>
			/// <param name="parameters">    
			/// Reference to object that specifies parameters of intersection.
			/// </param>
			/// <param name="direction">     Direction of sweep.</param>
			/// <param name="queryFlags">    
			/// A set of flags that specifies which objects to test against and how.
			/// </param>
			/// <param name="flagsAll">      
			/// A set of flags all of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="flagsAny">      
			/// A set of flags any of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="collisionClass">
			/// An object that represents collision class of this primitive.
			/// </param>
			/// <param name="entitiesToSkip">
			/// An optional array of entities to ignore during the test.
			/// </param>
			/// <returns>Distance to the contact. If less then 0, then there is no contact.</returns>
			[Pure]
			public float Cast(out GeometryContact contact, ref IntersectionParameters parameters, ref Vector3 direction,
							  EntityQueryFlags queryFlags = EntityQueryFlags.All,
							  PhysicsGeometryFlags flagsAll = (PhysicsGeometryFlags)0,
							  PhysicsGeometryFlags flagsAny = PhysicsGeometryFlags.CollisionTypeDefault |
															  PhysicsGeometryFlags.CollisionTypePlayer,
							  CollisionClass collisionClass = new CollisionClass(),
							  PhysicalEntity[] entitiesToSkip = null)
			{
				return PhysicalWorld.PrimitiveCast(out contact, ref this.Base, Id, ref direction, queryFlags, flagsAll,
												   flagsAny, ref parameters, ref collisionClass, entitiesToSkip);
			}
			/// <summary>
			/// Casts this primitive along <paramref name="direction"/> from its current position.
			/// </summary>
			/// <param name="contact">       
			/// Resultant contact. Only assigned if returned value is not less then 0.
			/// </param>
			/// <param name="direction">     Direction of sweep.</param>
			/// <param name="queryFlags">    
			/// A set of flags that specifies which objects to test against and how.
			/// </param>
			/// <param name="flagsAll">      
			/// A set of flags all of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="flagsAny">      
			/// A set of flags any of which must be set on entity/part for it to be tested against this
			/// primitive.
			/// </param>
			/// <param name="collisionClass">
			/// An object that represents collision class of this primitive.
			/// </param>
			/// <param name="entitiesToSkip">
			/// An optional array of entities to ignore during the test.
			/// </param>
			/// <returns>Distance to the contact. If less then 0, then there is no contact.</returns>
			[Pure]
			public float Cast(out GeometryContact contact, ref Vector3 direction,
							  EntityQueryFlags queryFlags = EntityQueryFlags.All,
							  PhysicsGeometryFlags flagsAll = (PhysicsGeometryFlags)0,
							  PhysicsGeometryFlags flagsAny = PhysicsGeometryFlags.CollisionTypeDefault |
															  PhysicsGeometryFlags.CollisionTypePlayer,
							  CollisionClass collisionClass = new CollisionClass(),
							  PhysicalEntity[] entitiesToSkip = null)
			{
				return PhysicalWorld.PrimitiveCast(out contact, ref this.Base, Id, ref direction, queryFlags, flagsAll,
												   flagsAny, ref IntersectionParameters.Default, ref collisionClass,
												   entitiesToSkip);
			}
			#endregion
		}
	}
}