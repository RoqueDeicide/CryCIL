using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Thrown when attempting to invoke native methods on a destroyed entity. See <see
	/// cref="CryEngine.CryScriptInstance.IsDestroyed" />
	/// </summary>
	public class ScriptInstanceDestroyedException : Exception
	{
		public ScriptInstanceDestroyedException()
		{
		}

		public ScriptInstanceDestroyedException(string message)
			: base(message)
		{
		}

		public ScriptInstanceDestroyedException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}