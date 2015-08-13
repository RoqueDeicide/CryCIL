using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that can be used when querying entities.
	/// </summary>
	/// <remarks>Used in simulation of explosions, casting rays etc.</remarks>
	[Flags]
	public enum EntityQueryFlags
	{
		/// <summary>
		/// When set, instructs static entities to be included in results of the query.
		/// </summary>
		Static = 1,
		/// <summary>
		/// When set, instructs sleeping rigid bodies to be included in results of the query.
		/// </summary>
		SleepingRigid = 2,
		/// <summary>
		/// When set, instructs rigid bodies to be included in results of the query.
		/// </summary>
		Rigid = 4,
		/// <summary>
		/// When set, instructs living entities to be included in results of the query.
		/// </summary>
		Living = 8,
		/// <summary>
		/// When set, instructs independently simulated entities (particles, ropes and soft bodies) to be
		/// included in results of the query.
		/// </summary>
		Independent = 16,
		/// <summary>
		/// When set, instructs deleted entities to be included in results of the query.
		/// </summary>
		Deleted = 128,
		/// <summary>
		/// When set, instructs terrain to be included in results of the query.
		/// </summary>
		Terrain = 0x100,
		/// <summary>
		/// When set, instructs all supported types of entities to be included in results of the query.
		/// </summary>
		All = Static | SleepingRigid | Rigid | Living | Independent | Terrain,
		/// <summary>
		/// When set, instructs to only include entities that have <see cref="PhysicalEntityFlags.Update"/>
		/// set in results of the query.
		/// </summary>
		FlaggedOnly = PhysicalEntityFlags.Update,
		/// <summary>
		/// When set, instructs to not include entities that have <see cref="PhysicalEntityFlags.Update"/>
		/// set in results of the query.
		/// </summary>
		SkipFlagged = PhysicalEntityFlags.Update * 2,
		/// <summary>
		/// When set, instructs areas to be included in results of the query.
		/// </summary>
		Areas = 32,
		/// <summary>
		/// When set, instructs triggers to be included in results of the query.
		/// </summary>
		Triggers = 64,
		/// <summary>
		/// When set, instructs entities without collision proxies to not be included in results of the
		/// query.
		/// </summary>
		IgnoreNonColliding = 0x10000,
		/// <summary>
		/// When set, instructs to sort results of the query by mass in ascending order.
		/// </summary>
		SortByMass = 0x20000,
		/// <summary>
		/// When set, instructs to allocated the memory for the results of the query.
		/// </summary>
		/// <remarks>Allocated memory is expected to be removed by the user.</remarks>
		AllocateList = 0x40000,
		/// <summary>
		/// When set, instructs to call AddRef function on each resultant physical entity.
		/// </summary>
		/// <remarks>Release function is expected to be called in this case.</remarks>
		AddRefResults = 0x100000,
		/// <summary>
		/// When set, instructs water to be included in results of the query.
		/// </summary>
		/// <remarks>Can only be used in <see cref="PhysicalWorld.RayWorldIntersection"/>.</remarks>
		Water = 0x200,
		/// <summary>
		/// When set, instruct to not have results activated(?). (Used by small background physical
		/// objects, like fish and insects).
		/// </summary>
		/// <remarks>Can only be used in <see cref="PhysicalWorld.RayWorldIntersection"/>.</remarks>
		NoOnDemandActivation = 0x80000, // can only be used in RayWorldIntersection
		/// <summary>
		/// When set, instructs procedural breakage events to be queued.
		/// </summary>
		/// <remarks>Can only be used in <see cref="PhysicalWorld.SimulateExplosion"/>.</remarks>
		DelayedDeformations = 0x80000
	}
}