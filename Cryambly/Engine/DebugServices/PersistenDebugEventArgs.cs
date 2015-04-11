using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Provides information about events raised <see cref="PersistentDebug"/>.
	/// </summary>
	public class PersistenDebugEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the object related to the event.
		/// </summary>
		public PersistentDebugObject Object { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="obj">Object related to the event.</param>
		public PersistenDebugEventArgs(PersistentDebugObject obj)
		{
			this.Object = obj;
		}
	}
}
