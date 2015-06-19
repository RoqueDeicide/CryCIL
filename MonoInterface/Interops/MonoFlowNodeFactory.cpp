#include "stdafx.h"

#include "MonoFlowNodeFactory.h"
#include "MonoFlowNode.h"

IFlowNodePtr MonoFlowNodeFactory::Create(IFlowNode::SActivationInfo *actInfo)
{
	bool cancel;
	auto node = new MonoFlowNode(this->typeId, actInfo, cancel);
	if (!cancel)
	{
		return node;
	}
	return nullptr;
}

MonoFlowNodeFactory::MonoFlowNodeFactory()
	: refCount(0)
	, typeId(0)
{

}

typedef void(*UnregisterTypeThunk)(ushort);

MonoFlowNodeFactory::~MonoFlowNodeFactory()
{
	static UnregisterTypeThunk thunk = (UnregisterTypeThunk)
		MonoEnv->Cryambly->GetClass("CryCil.RunTime.Registration", "FlowNodeTypeRegistry")
						 ->GetFunction("UnregisterType", 1)->RawThunk;

	thunk(this->typeId);
}
