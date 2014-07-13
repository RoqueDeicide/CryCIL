using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Native
{
	public struct ActorInitializationParams : IScriptInitializationParams
	{
		public IntPtr EntityPtr;
		public IntPtr ActorPtr;
		public EntityId Id;
		public int ChannelId;
	}
}