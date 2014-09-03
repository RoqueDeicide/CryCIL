using System;
using CryEngine.Native;

namespace CryEngine.Entities.Advanced
{
	/// <summary>
	/// Represents a wrapper object for a <see cref="GameObject" /> extension.
	/// </summary>
	public class GameObjectExtension
	{
		/// <summary>
		/// Sets a value that indicates whether this extension needs to receive post-updates.
		/// </summary>
		public bool ReceivePostUpdates
		{
			set { GameObjectInterop.EnablePostUpdates(this.Owner.Handle, this.Handle, value); }
		}
		/// <summary>
		/// <see cref="GameObject" /> that hosts this extension.
		/// </summary>
		public GameObject Owner { get; set; }
		internal IntPtr Handle { get; set; }
	}
}