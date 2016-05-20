using System;
using System.Linq;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Enumeration of types of occlusion algorithms that are used for audio.
	/// </summary>
	public enum AudioOcclusionType
	{
		/// <summary>
		/// No occlusion calculation must be done.
		/// </summary>
		None = 0,
		/// <summary>
		/// Obstructions must be ignored.
		/// </summary>
		Ignore = 1,
		/// <summary>
		/// Occluders must be detected by casting a singular ray.
		/// </summary>
		SingleRay = 2,
		/// <summary>
		/// Occluders must be detected by casting multiple rays.
		/// </summary>
		MultiRay = 3,

		/// <summary>
		/// Number of occlusion algorithm types.
		/// </summary>
		Count
	}
}