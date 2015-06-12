using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Data
{
	/// <summary>
	/// Enumeration of contexts in which the synchronization can occur.
	/// </summary>
	public enum SyncContext
	{
		/// <summary>
		/// Data is being synchronized with a save-game file. Occurs when saving/loading the game.
		/// </summary>
		SaveGame,
		/// <summary>
		/// Data is being synchronized via network. Occurs in multiplayer games.
		/// </summary>
		Network,
		/// <summary>
		/// Custom synchronization context, e.g. when reloading scripts.
		/// </summary>
		Custom
	}
}
