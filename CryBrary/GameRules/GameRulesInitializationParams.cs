using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	internal struct GameRulesInitializationParams : IScriptInitializationParams
	{
		public EntityId id;
		public IntPtr entityPtr;
	}
}