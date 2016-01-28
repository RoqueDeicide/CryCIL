using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// A set of flags that specifies which material layers are enabled for rendering.
	/// </summary>
	[Flags]
	public enum MaterialLayerActivityFlags : byte
	{
		/// <summary>
		/// When set, specifies material layers that are used when entity is frozen.
		/// </summary>
		Frozen = 0x0001,
		/// <summary>
		/// When set, specifies material layers that are used when entity is wet.
		/// </summary>
		Wet = 0x0002,
		/// <summary>
		/// When set, specifies material layers that are used when entity is cloaked.
		/// </summary>
		Cloak = 0x0004,
		/// <summary>
		/// When set, specifies material layers that are used when entity is frozen in a special way(?).
		/// </summary>
		DynamicFrozen = 0x0008
	}
}