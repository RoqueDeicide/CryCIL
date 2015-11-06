using System;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Enumeration of flags that specifies sequences of facial animations.
	/// </summary>
	[Flags]
	public enum FacialAnimationSequenceFlags
	{
		/// <summary>
		/// When set, specifies that the time range of the sequence is created from the length of sound.
		/// </summary>
		RangeFromSound = 0x00001,
	}
}