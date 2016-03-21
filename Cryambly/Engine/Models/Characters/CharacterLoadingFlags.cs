using System;
using System.Linq;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Enumeration of flags that specify how to load characters.
	/// </summary>
	[Flags]
	public enum CharacterLoadingFlags : uint
	{
		/// <summary>
		/// When set, specifies to not load LOD data.
		/// </summary>
		IgnoreLod = 0x01,
		/// <summary>
		/// When set, specifies that the character will be loaded for editing(?).
		/// </summary>
		CharEditModel = 0x02,
		/// <summary>
		/// When set, specifies that the character will be displayed as a preview(?).
		/// </summary>
		PreviewMode = 0x04,
		/// <summary>
		/// When set, specifies to not stream the static objects that are included into the character.
		/// </summary>
		DoNotStreamStaticObjects = 0x08,
		/// <summary>
		/// When set, specifies to not recreate skeleton(?).
		/// </summary>
		SkipSkeletonRecreation = 0x10,
		/// <summary>
		/// When set, specifies to not post warnings in case of errors.
		/// </summary>
		DisableLogWarnings = 0x20
	}
}