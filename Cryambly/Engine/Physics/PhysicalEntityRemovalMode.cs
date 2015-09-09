using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of values that specify how to remove the physical entity.
	/// </summary>
	[Flags]
	public enum PhysicalEntityRemovalMode
	{
		/// <summary>
		/// Specifies the entity to be moved into to-destroy list, delete areas and placeholders.
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
		/// An optional flag that specifies that operation specified by one of other flags is to be
		/// attempted. The operation won't take place if it's reference count is not 0.
		/// </summary>
		Attempt = 4
	}
}