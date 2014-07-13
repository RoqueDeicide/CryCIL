using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Used to cross over input data for realtime scripting.
	/// </summary>
	internal class SerializableInput
	{
		public Delegate[] KeyEvents { get; set; }
		public Delegate[] MouseEvents { get; set; }

		public ActionmapHandler ActionmapEvents { get; set; }
	}
}