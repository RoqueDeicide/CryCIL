using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of flags that define which render resources to refresh.
	/// </summary>
	[Flags]
	public enum ResourceRefreshFlags
	{
		/// <summary>
		/// When set, specifies that shaders must be reloaded.
		/// </summary>
		Shaders = 1,
		/// <summary>
		/// When set, specifies that shader textures must be reloaded.
		/// </summary>
		ShaderTextures = 2,
		/// <summary>
		/// When set, specifies that textures must be reloaded.
		/// </summary>
		Textures = 4,
		/// <summary>
		/// When set, specifies that geometric objects must be reloaded.
		/// </summary>
		Geometry = 8,
		/// <summary>
		/// When set, specifies that reloading process must be enforced.
		/// </summary>
		ForceReload = 0x10
	}
}