using System.Collections.Generic;
using System.Linq;

using CryEngine.Initialization;

namespace CryEngine.Flowgraph.Native
{
	internal struct NodeConfig
	{

		internal FlowNodeFlags Flags;

		internal FlowNodeFilter Filter;

		internal FlowNodeType Type;

		internal string Description;

		internal object[] Inputs;
		internal object[] Outputs;
	}
}