using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// A set of flags that specify the blending of material layers.
	/// </summary>
	[Flags]
	public enum MaterialLayerBlendFlags : uint
	{
		/// <summary>
		/// When set, specifies that blending should be enabled for material layers that are used when
		/// entity is frozen.
		/// </summary>
		Frozen = 0xff000000,
		/// <summary>
		/// When set, specifies that blending should be enabled for material layers that are used when
		/// entity is wet.
		/// </summary>
		Wet = 0x00fe0000,
		/// <summary>
		/// When set, specifies that blending should be enabled for material layers that are used when
		/// entity is going from cloaked stance to normal(?).
		/// </summary>
		CloakDissolve = 0x00010000,
		/// <summary>
		/// When set, specifies that blending should be enabled for material layers that are used when
		/// entity is cloaked.
		/// </summary>
		Cloak = 0x0000ff00,
		/// <summary>
		/// When set, specifies that blending should be enabled for material layers that are used when
		/// entity is frozen in a special way(?).
		/// </summary>
		DynamicFrozen = 0x000000ff
	}
}