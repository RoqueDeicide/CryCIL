#pragma once

#include "IMonoInterface.h"
#include "IFlowSystem.h"

//! This factory is separated from interop class unlike in CryMono in order to be able to get the identifier of the node
//! type without having to register a hook every time CryCIL-based flow node is created.
struct MonoFlowNodeFactory : public IFlowNodeFactory
{
private:
	int refCount;
	TFlowNodeTypeId typeId;
public:
	MonoFlowNodeFactory();
	~MonoFlowNodeFactory();

	virtual void AddRef() override { this->refCount++; }
	virtual void Release() override { if (--this->refCount <= 0) delete this; }

	virtual IFlowNodePtr Create(IFlowNode::SActivationInfo *actInfo) override;

	virtual void GetMemoryUsage(ICrySizer *) const override {}
	virtual void Reset() override {}

	//! Invoked from the interop class to assign the type identifier.
	void AssignTypeId(TFlowNodeTypeId id) { this->typeId = id; }
};