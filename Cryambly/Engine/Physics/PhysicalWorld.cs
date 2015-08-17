using System.Runtime.CompilerServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Provides access to CryEngine physics API.
	/// </summary>
	public static class PhysicalWorld
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
		internal static extern int AddExplosionShape(PhysicalGeometry shape, float size, int index, float probability = 1.0f);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveExplosionShape(int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveAllExplosionShapes();
	}
}