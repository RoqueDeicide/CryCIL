using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Entities;

namespace CryEngine
{
	internal struct GameRulesInitializationParams : IScriptInitializationParams
	{
		public EntityId id;
		public IntPtr entityPtr;
	}
}