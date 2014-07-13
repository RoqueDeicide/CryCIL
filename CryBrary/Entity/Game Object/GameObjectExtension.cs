using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine.Advanced
{
	public class GameObjectExtension
	{
		public bool ReceivePostUpdates
		{
			set { NativeGameObjectMethods.EnablePostUpdates(Owner.Handle, Handle, value); }
		}

		public GameObject Owner { get; set; }
		internal IntPtr Handle { get; set; }
	}
}