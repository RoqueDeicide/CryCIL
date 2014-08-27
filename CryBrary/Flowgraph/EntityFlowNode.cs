using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine;
using CryEngine.Initialization;
using CryEngine.Flowgraph.Native;

namespace CryEngine.Flowgraph
{
	public abstract class EntityFlowNode : FlowNode
	{
		internal override NodeConfig GetNodeConfig()
		{
			var registrationParams = (EntityFlowNodeRegistrationParams)Script.RegistrationParams;

			return new NodeConfig(FlowNodeFilter.Approved, "", FlowNodeFlags.HideUI | FlowNodeFlags.TargetEntity,
								  FlowNodeType.Instanced, registrationParams.InputPorts, registrationParams.OutputPorts);
		}
	}
}