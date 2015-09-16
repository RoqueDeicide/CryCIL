namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of simulation classes that can be applied to entities.
	/// </summary>
	public enum PhysicsSimulationClass
	{
		/// <summary>
		/// Used in situations where simulation class can be changed to indicate that class should remain
		/// the same.
		/// </summary>
		Unused = -1,
		/// <summary>
		/// Static simulation class. Used by physical entities of type
		/// <see cref="PhysicalEntityType.Static"/>.
		/// </summary>
		Static = 0,
		/// <summary>
		/// Sleeping rigid body simulation class. Used by physical entities of type
		/// <see cref="PhysicalEntityType.Rigid"/>.
		/// </summary>
		SleepingRigid = 1,
		/// <summary>
		/// Active rigid body simulation class. Used by physical entities of type
		/// <see cref="PhysicalEntityType.Rigid"/>.
		/// </summary>
		ActiveRigid = 2,
		/// <summary>
		/// Living entity simulation class. Used by physical entities of type
		/// <see cref="PhysicalEntityType.Living"/>.
		/// </summary>
		Living = 3,
		/// <summary>
		/// Independent simulation class. Used by physical entities of type
		/// <see cref="PhysicalEntityType.Particle"/>, <see cref="PhysicalEntityType.Rope"/>,
		/// <see cref="PhysicalEntityType.Soft"/>.
		/// </summary>
		Independent = 4,
		/// <summary>
		/// Trigger simulation class. Used by physical entities of type
		/// <see cref="PhysicalEntityType.Area"/>.
		/// </summary>
		Trigger = 6,
		/// <summary>
		/// Used by physical entities of any type that were deleted.
		/// </summary>
		Deleted = 7
	}
}