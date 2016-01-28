using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Data;
using CryCil.Engine.Physics.Primitives;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Provides access to CryEngine physics API.
	/// </summary>
	public static unsafe partial class PhysicalWorld
	{
		/// <summary>
		/// An object that provides access to a global collection of objects that specify parameters of
		/// surfaces of physical bodies.
		/// </summary>
		public static readonly PhysicalSurfaces Surfaces = new PhysicalSurfaces();
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
		public static extern ExplosionResult SimulateExplosion
			(ref ExplosionParameters parameters,
			 PhysicalEntity[] entitiesToSkip = null,
			 EntityQueryFlags types = EntityQueryFlags.Rigid |
									  EntityQueryFlags.SleepingRigid |
									  EntityQueryFlags.Living |
									  EntityQueryFlags.Independent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int AddExplosionShape(GeometryShape shape, float size, int index,
													 float probability = 1.0f);
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
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		internal static int PrimitiveIntersection(out GeometryContact[] contacts,
												  ref Primitive.BasePrimitive primitive, int primitiveType,
												  EntityQueryFlags queryFlags, PhysicsGeometryFlags flagsAll,
												  PhysicsGeometryFlags flagsAny,
												  ref IntersectionParameters parameters,
												  ref CollisionClass collisionClass,
												  PhysicalEntity[] entitiesToSkip)
		{
			int count;
			if (entitiesToSkip == null)
			{
				count = PrimitiveIntersectionInternal(out contacts, ref primitive, primitiveType, queryFlags,
													  flagsAll, flagsAny, ref parameters, ref collisionClass,
													  null, 0);
			}
			else
				fixed (PhysicalEntity* skip = entitiesToSkip)
				{
					count = PrimitiveIntersectionInternal(out contacts, ref primitive, primitiveType, queryFlags,
														  flagsAll, flagsAny, ref parameters, ref collisionClass,
														  skip, entitiesToSkip.Length);
				}
			return count;
		}
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		internal static float PrimitiveCast(out GeometryContact contact, ref Primitive.BasePrimitive primitive,
											int primitiveType, ref Vector3 sweepDirection,
											EntityQueryFlags queryFlags, PhysicsGeometryFlags flagsAll,
											PhysicsGeometryFlags flagsAny, ref IntersectionParameters parameters,
											ref CollisionClass collisionClass, PhysicalEntity[] entitiesToSkip)
		{
			float distance;
			if (entitiesToSkip == null)
			{
				distance = PrimitiveCastInternal(out contact, ref primitive, primitiveType, ref sweepDirection,
												 queryFlags, flagsAll, flagsAny, ref parameters, ref collisionClass,
												 null, 0);
			}
			else
				fixed (PhysicalEntity* skip = entitiesToSkip)
				{
					distance = PrimitiveCastInternal(out contact, ref primitive, primitiveType, ref sweepDirection,
													 queryFlags, flagsAll, flagsAny, ref parameters,
													 ref collisionClass, skip, entitiesToSkip.Length);
				}
			return distance;
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int PrimitiveIntersectionInternal
			(out GeometryContact[] contacts,
			 ref Primitive.BasePrimitive primitive,
			 int primitiveType,
			 EntityQueryFlags queryFlags, PhysicsGeometryFlags flagsAll,
			 PhysicsGeometryFlags flagsAny,
			 ref IntersectionParameters parameters,
			 ref CollisionClass collisionClass,
			 PhysicalEntity* entitiesToSkip,
			 int skipCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float PrimitiveCastInternal(out GeometryContact contact,
														  ref Primitive.BasePrimitive primitive,
														  int primitiveType, ref Vector3 sweepDirection,
														  EntityQueryFlags queryFlags,
														  PhysicsGeometryFlags flagsAll,
														  PhysicsGeometryFlags flagsAny,
														  ref IntersectionParameters parameters,
														  ref CollisionClass collisionClass,
														  PhysicalEntity* entitiesToSkip,
														  int skipCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreatePhysicalEntity(PhysicalEntityType type,
														   ref PhysicsParameters initialParameters,
														   ForeignData foreignData,
														   int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreatePhysicalEntityNoParams
			(PhysicalEntityType type, ForeignData foreignData, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreatePhysicalEntityFromHolder(PhysicalEntityType type,
																	 float lifeTime,
																	 ref PhysicsParameters initialParameters,
																	 ForeignData foreignData,
																	 int id,
																	 PhysicalEntity placeHolder);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreatePhysicalEntityNoParamsFromHolder(PhysicalEntityType type,
																			 float lifeTime,
																			 ForeignData foreignData,
																			 int id,
																			 PhysicalEntity placeHolder);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreatePlaceHolder(PhysicalEntityType type,
														ref PhysicsParameters initialParameters,
														ForeignData foreignData, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreatePlaceHolderNoParams(PhysicalEntityType type, ForeignData foreignData,
																int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int DestroyPhysicalEntity(IntPtr pent, int mode, int bThreadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int SetSurfaceParameters(int surfaceIdx, float bounciness, float friction,
														SurfaceFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSurfaceParameters(int surfaceIdx, out float bounciness, out float friction,
														out SurfaceFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int SetSurfaceParametersExt(int surfaceIdx, float bounciness, float friction,
														   float damageReduction, float ricAngle,
														   float ricDamReduction, float ricVelReduction,
														   SurfaceFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSurfaceParametersExt(int surfaceIdx, out float bounciness, out float friction,
														   out float damage_reduction, out float ric_angle,
														   out float ric_dam_reduction, out float ric_vel_reduction,
														   out SurfaceFlags flags);
		/// <summary>
		/// Writes a typed snapshot that carries no useful data into network stream.
		/// </summary>
		/// <remarks>
		/// This method is used when synchronizing the entity that uses physics over network when: 1)
		/// writing to the network stream; 2) entity's physical proxy doesn't exist or physical entity that
		/// is hosted by the proxy doesn't exist or when type of hosted physical entity is not equal to
		/// <paramref name="snapshotType"/>.
		/// </remarks>
		/// <example>
		/// <code source="GarbageTypedSnapshotUsage.cs"/>
		/// </example>
		/// <param name="sync">        
		/// An object that represents the network stream the snapshot needs to be written to.
		/// </param>
		/// <param name="snapshotType">Physicalization type to assign to the snapshot.</param>
		/// <param name="flags">       
		/// A set of optional flags that specify how to write the snapshot.
		/// </param>
		public static void WriteGarbageTypedSnapshot(CrySync sync, PhysicalEntityType snapshotType,
													 SnapshotFlags flags = 0)
		{
			SerializeGarbageTypedSnapshot(sync, snapshotType, flags);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SerializeGarbageTypedSnapshot(CrySync sync, PhysicalEntityType snapshotType,
																 SnapshotFlags flags);
	}
}