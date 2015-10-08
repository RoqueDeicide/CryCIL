using System;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Enumeration of flags that specify how to load the static object.
	/// </summary>
	[Flags]
	public enum StaticObjectLoadingFlags
	{
		/// <summary>
		/// When set, specifies that static object must be loaded to be used in a preview.
		/// </summary>
		PreviewMode = 1 << 0,
		/// <summary>
		/// When set, specifies that static object must be made breakable even if it's not supposed to be
		/// breakable.
		/// </summary>
		ForceBreakable = 1 << 1,
		/// <summary>
		/// When set, specifies that LOD models must not be loaded.
		/// </summary>
		IgnoreLoDs = 1 << 2,
		/// <summary>
		/// When set, specifies that tessellation must be enable for the static object.
		/// </summary>
		Tessellate = 1 << 3,
		/// <summary>
		/// When set, specifies only geometry data must be loaded.
		/// </summary>
		/// <remarks>Used for streaming, to avoid parsing all chunks</remarks>
		JustGeometry = 1 << 4
	}
}