using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of flags that specify usage of the material layer.
	/// </summary>
	[Flags]
	public enum MaterialLayerUsage : byte
	{
		/// <summary>
		/// Layer is disabled.
		/// </summary>
		NoDraw = 0x0001,
		/// <summary>
		/// Replace base pass rendering with layer - optimization.
		/// </summary>
		ReplaceBase = 0x0002,
		/// <summary>
		/// Layer doesn't render but still causes parent to fade out.
		/// </summary>
		FadeOut = 0x0004
	}
}