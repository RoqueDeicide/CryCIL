using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

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
			#region Fields
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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
		/// <summary>
		/// Represents a primitive geometric object that represents a capsule.
		/// </summary>
		[StructLayout(LayoutKind.Explicit)]
		[PrimitiveType]
		public struct Capsule
		{
			#region Fields
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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
			/// <exception cref="OverflowException">
			/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
			/// elements.
			/// </exception>
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