using System;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Encapsulates character manager statistics.
	/// </summary>
	public struct CharacterManagerStatistics
	{
		/// <summary>
		/// Number of character instances.
		/// </summary>
		public uint CharacterCount;
		/// <summary>
		/// Number of character models (CGF).
		/// </summary>
		public uint CharacterModelCount;
		/// <summary>
		/// Number of animated objects.
		/// </summary>
		public uint AnimatedObjectCount;
		/// <summary>
		/// Number of models used by animated objects.
		/// </summary>
		public uint AnimatedObjectModelCount;
	}
}