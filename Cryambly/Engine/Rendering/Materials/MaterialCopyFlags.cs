using System;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of flags that specify how to copy a material.
	/// </summary>
	[Flags]
	public enum MaterialCopyFlags
	{
		/// <summary>
		/// Default copy.
		/// </summary>
		Default = 0,
		/// <summary>
		/// Copy the name.
		/// </summary>
		Name = 1,
		/// <summary>
		/// Copy the material flags that can be captured using <see cref="MaterialFlags.TemplateMask"/>.
		/// </summary>
		Template = 2,
		/// <summary>
		/// Copy textures.
		/// </summary>
		Textures = 4
	}
}