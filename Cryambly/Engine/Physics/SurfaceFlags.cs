using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that can be assigned to physical surfaces.
	/// </summary>
	[Flags]
	public enum SurfaceFlags
	{
		//pierceable_mask = 0x0F,
		//max_pierceable = 0x0F,
		/// <summary>
		/// When set, specifies that the surface is "important". It is possible to prioritize surfaces by this flag when casting a ray.
		/// </summary>
		Important = 0x200,
		//manually_breakable = 0x400,
		//matbreakable_bit = 16
	}
}