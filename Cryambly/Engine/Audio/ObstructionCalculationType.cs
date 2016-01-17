namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Enumeration of types of calculation of obstruction that is used for audio.
	/// </summary>
	public enum ObstructionCalculationType
	{
		/// <summary>
		/// No obstruction calculation must be done.
		/// </summary>
		None = 0,
		/// <summary>
		/// Obstructions must be ignored.
		/// </summary>
		Ignore = 1,
		/// <summary>
		/// Obstructions must be determined by casting a singular ray.
		/// </summary>
		SingleRay = 2,
		/// <summary>
		/// Obstructions must be determined by casting multiple rays.
		/// </summary>
		MultiRay = 3,

		/// <summary>
		/// Number of obstruction calculation types.
		/// </summary>
		Count
	}
}