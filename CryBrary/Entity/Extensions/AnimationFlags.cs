using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Flags used when playing animations.
	/// </summary>
	[Flags]
	public enum AnimationFlags
	{
		/// <summary>
		/// Attempt clean animation blending.
		/// </summary>
		CleanBending = 1 << 1,

		/// <summary>
		/// Don't blend with other animations
		/// </summary>
		NoBlend = 1 << 2,

		/// <summary>
		/// Restart animation after finishing.
		/// </summary>
		RestartAnimation = 1 << 4,

		/// <summary>
		/// Repeat the last frame.
		/// </summary>
		RepeatLastFrame = 1 << 8,

		/// <summary>
		/// Loop this animation
		/// </summary>
		Loop = 1 << 16,
	}
}