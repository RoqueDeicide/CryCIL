using CryEngine.Flowgraph;

namespace CryEngine.Native
{
	internal struct NodeConfig
	{

		internal FlowNodeFlags Flags;

		internal FlowNodeFilter Filter;

		internal FlowNodeCloneType Type;

		internal string Description;

		internal object[] Inputs;
		internal object[] Outputs;
	}
}