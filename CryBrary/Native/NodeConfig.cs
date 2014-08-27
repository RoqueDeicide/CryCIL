using System.Collections.Generic;
using System.Linq;

using CryEngine.Initialization;

namespace CryEngine.Flowgraph.Native
{
	internal struct NodeConfig
	{
		public NodeConfig(FlowNodeFilter cat, string desc,
			FlowNodeFlags nodeFlags, FlowNodeType nodeType,
			IEnumerable<InputPortConfig> inputPorts, IEnumerable<OutputPortConfig> outputPorts)
			: this()
		{
			flags = nodeFlags;
			filter = cat;
			description = desc;
			type = nodeType;

			inputs = inputPorts.Cast<object>().ToArray();
			outputs = outputPorts.Cast<object>().ToArray();
		}

		internal FlowNodeFlags flags;

		internal FlowNodeFilter filter;

		internal FlowNodeType type;

		internal string description;

		internal object[] inputs;
		internal object[] outputs;
	}
}