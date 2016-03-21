using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that indicate which types of physical entities can be used in a given context.
	/// </summary>
	[Flags]
	public enum PhysicalEntityTypes
	{
		/// <summary>
		/// When set, instructs static entities to be used in the context.
		/// </summary>
		Static = 1,
		/// <summary>
		/// When set, instructs sleeping rigid bodies to be used in the context.
		/// </summary>
		SleepingRigid = 2,
		/// <summary>
		/// When set, instructs rigid bodies to be used in the context.
		/// </summary>
		Rigid = 4,
		/// <summary>
		/// When set, instructs living entities to be used in the context.
		/// </summary>
		Living = 8,
		/// <summary>
		/// When set, instructs independently simulated entities (particles, ropes and soft bodies) to be
		/// used in the context.
		/// </summary>
		Independent = 16,
		/// <summary>
		/// When set, instructs deleted entities to be used in the context.
		/// </summary>
		Deleted = 128,
		/// <summary>
		/// When set, instructs terrain to be used in the context.
		/// </summary>
		Terrain = 0x100,
		/// <summary>
		/// When set, instructs all supported types of entities to be used in the context.
		/// </summary>
		All = Static | SleepingRigid | Rigid | Living | Independent | Terrain,
		/// <summary>
		/// When set, instructs to only use entities that have <see cref="PhysicalEntityFlags.Update"/> set
		/// in the context.
		/// </summary>
		FlaggedOnly = PhysicalEntityFlags.Update,
		/// <summary>
		/// When set, instructs to not use entities that have <see cref="PhysicalEntityFlags.Update"/> set
		/// in the context.
		/// </summary>
		SkipFlagged = PhysicalEntityFlags.Update * 2,
		/// <summary>
		/// When set, instructs areas to be used in the context.
		/// </summary>
		Areas = 32,
		/// <summary>
		/// When set, instructs triggers to be used in the context.
		/// </summary>
		Triggers = 64,
		/// <summary>
		/// When set, instructs entities without collision proxies to not be used in the context.
		/// </summary>
		IgnoreNonColliding = 0x10000
	}
}