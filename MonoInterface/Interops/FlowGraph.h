#pragma once

#include "IMonoInterface.h"
#include "IFlowSystem.h"

struct FlowGraphInterop : public IMonoInterop<false, true>
{
	virtual const char *GetInteropClassName() override { return ""; }	// No class name is provided here, because this is a multi-class interop.
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic"; }

	virtual void OnRunTimeInitialized() override;

	static ushort RegisterType(mono::string name);

	static void ActivatePort(IFlowGraph *graph, ushort nodeId, byte portId, struct MonoFlowData data);

	static void DeactivateNode(IFlowGraph *graph, ushort nodeId);
};