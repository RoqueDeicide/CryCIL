using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Entities;

namespace CryEngine
{
	internal struct GameRulesInitializationParams : IScriptInitializationParams
	{
		internal EntityId Id;
		internal IntPtr EntityPointer;
	}
}