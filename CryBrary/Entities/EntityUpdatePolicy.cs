namespace CryEngine.Entities
{
	public enum EntityUpdatePolicy
	{
		/// <summary>
		/// Never update entity every frame.
		/// </summary>
		Never,
		/// <summary>
		/// Only update entity if it is in specified range from active camera.
		/// </summary>
		InRange,
		/// <summary>
		/// Only update entity if it is potentially visible.
		/// </summary>
		PotentiallyVisible,
		/// <summary>
		/// Only update entity if it is visible.
		/// </summary>
		Visible,
		/// <summary>
		/// Only update entity if it is need to be updated due to physics.
		/// </summary>
		Physics,
		/// <summary>
		/// Only update entity if it is need to be updated due to physics or if it is visible.
		/// </summary>
		PhysicsVisible,
		/// <summary>
		/// Always update entity every frame.
		/// </summary>
		Always,
	}
}