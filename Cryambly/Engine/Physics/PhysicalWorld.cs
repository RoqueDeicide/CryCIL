﻿using System.Runtime.CompilerServices;
using CryCil.Engine.Physics.Primitives;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Provides access to CryEngine physics API.
	/// </summary>
	public static unsafe partial class PhysicalWorld
	{
		/// <summary>
		/// Simulates an explosion.
		/// </summary>
		/// <param name="parameters">    A set of parameters that describe the explosion.</param>
		/// <param name="entitiesToSkip">
		/// An optional array of physical entities that must be excluded from the simulation.
		/// </param>
		/// <param name="types">         
		/// A set of flags that specify which entities to include into simulation and how to process
		/// results.
		/// </param>
		/// <returns>
		/// An object that contains an array of entities that were affected by the explosion.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ExplosionResult SimulateExplosion(ref ExplosionParameters parameters,
															   PhysicalEntity[] entitiesToSkip = null,
															   EntityQueryFlags types = EntityQueryFlags.Rigid |
																						EntityQueryFlags.SleepingRigid |
																						EntityQueryFlags.Living |
																						EntityQueryFlags.Independent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int AddExplosionShape(GeometryShape shape, float size, int index, float probability = 1.0f);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveExplosionShape(int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveAllExplosionShapes();
		/// <summary>
		/// Fills given object with parameters to that specify global water simulation.
		/// </summary>
		/// <param name="parameters">A reference to the object to fill with parameters.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetWaterManagerParameters(ref WaterManagerParameters parameters);
		/// <summary>
		/// Updates parameters for the water simulation.
		/// </summary>
		/// <param name="parameters">A reference to the object that provides parameters to change.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetWaterManagerParameters(ref WaterManagerParameters parameters);
		internal static int PrimitiveIntersection(out GeometryContact[] contacts,
												  ref Primitive.BasePrimitive primitive, int primitiveType,
												  EntityQueryFlags queryFlags, PhysicsGeometryFlags flagsAll,
												  PhysicsGeometryFlags flagsAny,
												  ref IntersectionParameters parameters,
												  ref CollisionClass collisionClass, PhysicalEntity[] entitiesToSkip)
		{
			int count;
			if (entitiesToSkip == null)
			{
				count = PrimitiveIntersectionInternal(out contacts, ref primitive, primitiveType, queryFlags, flagsAll,
													  flagsAny, ref parameters, ref collisionClass, null, 0);
			}
			else
				fixed (PhysicalEntity* skip = entitiesToSkip)
				{
					count = PrimitiveIntersectionInternal(out contacts, ref primitive, primitiveType, queryFlags, flagsAll,
														  flagsAny, ref parameters, ref collisionClass, skip,
														  entitiesToSkip.Length);
				}
			return count;
		}
		internal static float PrimitiveCast(out GeometryContact contact, ref Primitive.BasePrimitive primitive,
											int primitiveType, ref Vector3 sweepDirection,
											EntityQueryFlags queryFlags, PhysicsGeometryFlags flagsAll,
											PhysicsGeometryFlags flagsAny, ref IntersectionParameters parameters,
											ref CollisionClass collisionClass, PhysicalEntity[] entitiesToSkip)
		{
			float distance;
			if (entitiesToSkip == null)
			{
				distance = PrimitiveCastInternal(out contact, ref primitive, primitiveType, ref sweepDirection, queryFlags, flagsAll,
												 flagsAny, ref parameters, ref collisionClass, null, 0);
			}
			else
				fixed (PhysicalEntity* skip = entitiesToSkip)
				{
					distance = PrimitiveCastInternal(out contact, ref primitive, primitiveType, ref sweepDirection,
													 queryFlags, flagsAll, flagsAny, ref parameters, ref collisionClass,
													 skip, entitiesToSkip.Length);
				}
			return distance;
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int PrimitiveIntersectionInternal
			(out GeometryContact[] contacts,
			 ref Primitive.BasePrimitive primitive, int primitiveType,
			 EntityQueryFlags queryFlags, PhysicsGeometryFlags flagsAll,
			 PhysicsGeometryFlags flagsAny,
			 ref IntersectionParameters parameters,
			 ref CollisionClass collisionClass, PhysicalEntity* entitiesToSkip,
			 int skipCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float PrimitiveCastInternal
			(out GeometryContact contact, ref Primitive.BasePrimitive primitive,
			 int primitiveType, ref Vector3 sweepDirection,
			 EntityQueryFlags queryFlags, PhysicsGeometryFlags flagsAll,
			 PhysicsGeometryFlags flagsAny, ref IntersectionParameters parameters,
			 ref CollisionClass collisionClass, PhysicalEntity* entitiesToSkip,
			 int skipCount);
	}
}