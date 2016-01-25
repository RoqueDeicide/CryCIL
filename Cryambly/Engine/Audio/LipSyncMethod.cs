using System;
using System.Linq;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Enumeration of ways the lip-sync can be done.
	/// </summary>
	public enum LipSyncMethod
	{
		/// <summary>
		/// No lip-syncing.
		/// </summary>
		None,
		/// <summary>
		/// Match the facial animation with the sound.
		/// </summary>
		MatchAnimationToSoundName
	}
}