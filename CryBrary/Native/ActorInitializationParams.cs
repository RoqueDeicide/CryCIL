using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Entities;

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