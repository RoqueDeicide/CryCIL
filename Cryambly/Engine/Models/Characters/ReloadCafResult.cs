using System;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Enumeration of results of reloading a .caf file.
	/// </summary>
	public enum ReloadCafResult
	{
		/// <summary>
		/// Reloading failed.
		/// </summary>
		Failed,
		/// <summary>
		/// Reloading succeeded.
		/// </summary>
		Succeed,
		/// <summary>
		/// Crytek programmer got mad.
		/// </summary>
		GahNotInArray
	}
}