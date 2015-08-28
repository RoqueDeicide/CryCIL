namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of values that specify how to remove the physical entity.
	/// </summary>
	public enum PhysicalEntityRemovalMode
	{
		/// <summary>
		/// Specifies the entity to be moved into to-destroy list, delete areas and placeholders.
		/// </summary>
		Destroy,
		/// <summary>
		/// Specifies the entity to be suspended (unregistered from simulation) but not deleted.
		/// </summary>
		Suspend,
		/// <summary>
		/// Specifies the entity to be removed from suspension.
		/// </summary>
		Unsuspend,
		/// <summary>
		/// Specifies the entity removal to be attempted. The entity will only be removed if it's reference
		/// count is 0.
		/// </summary>
		Attempt
	}
}