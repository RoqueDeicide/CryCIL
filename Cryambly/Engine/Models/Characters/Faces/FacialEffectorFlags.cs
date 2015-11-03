using System;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Enumeration of flags that specify facial effectors.
	/// </summary>
	[Flags]
	public enum FacialEffectorFlags
	{
		/// <summary>
		/// When set, species that this facial effector is a root effector in the library.
		/// </summary>
		Root = 0x00001,

		/// <summary>
		/// When set, species that this facial effector must be extended in UI.
		/// </summary>
		UiExtended = 0x01000,
		/// <summary>
		/// When set, species that this facial effector was modified.
		/// </summary>
		UiModified = 0x02000,
		/// <summary>
		/// When set, species that this facial effector is not saved in the library, since it's a
		/// preview-only effector.
		/// </summary>
		UiPreview = 0x04000,
	}
}