using System;

namespace CryEngine.Native
{
	internal struct NodeInitializationParams : IScriptInitializationParams
	{
		public IntPtr nodePtr;

		public UInt16 nodeId;
		public UInt32 graphId;
	}
}