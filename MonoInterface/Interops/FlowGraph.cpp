#include "stdafx.h"

#include "FlowGraph.h"
#include "MonoFlowNodeFactory.h"
#include "MonoFlowNode.h"

void FlowGraphInterop::OnRunTimeInitialized()
{
	MonoEnv->Functions->AddInternalCall("CryCil.RunTime.Registration", "FlowNodeTypeRegistry", "RegisterType", RegisterType);
	MonoEnv->Functions->AddInternalCall("CryCil.Engine.Logic", "OutputPort", "ActivateInternal", ActivatePort);
	MonoEnv->Functions->AddInternalCall("CryCil.Engine.Logic", "FlowNode", "Deactivate", DeactivateNode);
}

ushort FlowGraphInterop::RegisterType(mono::string name)
{
	auto factory = new MonoFlowNodeFactory();
	ushort id = MonoEnv->CryAction->GetIFlowSystem()->RegisterType(NtText(name), factory);
	factory->AssignTypeId(id);
	return id;
}

void FlowGraphInterop::ActivatePort(IFlowGraph *graph, ushort nodeId, byte portId, MonoFlowData data)
{
	SFlowAddress address(nodeId, portId, true);
	graph->ActivatePortAny(address, data.ToInputData());
}

void FlowGraphInterop::DeactivateNode(IFlowGraph *graph, ushort nodeId)
{
	auto generalNode = graph->GetNodeData(nodeId)->GetNode();
	auto node = static_cast<MonoFlowNode *>(generalNode);
	node->Deactivate();
}
