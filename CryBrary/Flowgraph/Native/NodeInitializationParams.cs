using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Flowgraph.Native
{
	internal struct NodeInitializationParams : IScriptInitializationParams
	{
		public IntPtr nodePtr;

		public UInt16 nodeId;
		public UInt32 graphId;
	}
}