namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of values that specify how to remove the physical entity.
	/// </summary>
	public enum PhysicalEntityRemovalMode
	{
		/// <summary>
		/// Specifies the entity to be moved into to-destroy list, delete areas and placeholders. If the
		/// entity has non-zero reference count, then the entity will be scheduled for deletion as soon as
		/// the the reference count is 0.
		/// </summary>
		Destroy = 0,
		/// <summary>
		/// Specifies the entity to be suspended (unregistered from simulation) but not deleted.
		/// </summary>
		Suspend = 1,
		/// <summary>
		/// Specifies the entity to be removed from suspension.
		/// </summary>
		Unsuspend = 2,
		/// <summary>
		/// Specifies the entity to be deleted but deletion must be canceled if the entity has non-zero
		/// reference count.
		/// </summary>
		AttemptDeletion = 4
	}
}