using System;

namespace CryCil.Engine.Logic
{
	internal struct FlowNodeConfig
	{
		internal IntPtr Description;
		internal FlowPortConfig[] Inputs;
		internal FlowPortConfig[] Outputs;
		internal FlowNodeFlags Flags;
	}
}