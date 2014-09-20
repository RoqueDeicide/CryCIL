#pragma once

#include "StdAfx.h"
#include "FlowSystem/Nodes/FlowBaseNode.h"
#include "MonoRunTime.h"

//! Class that handles creation, configuration and removal of CryMono FlowGraph nodes.
class CFlowNode_Mono
	: public CFlowBaseNodeInternal	// Derive from internal to be able to create
									// both instanced and singleton nodes through this type.
	, public IFlowGraphHook			// Also implement this interface so we can complete
									// node initialization on Mono side when a new node is created.
{
private:
	ENodeCloneType cloneType;					//!< Indicates whether this node is instanced or singleton.
	SActivationInfo *activationInfo;			//!< Extra info passed to the node upon creation.
	IFlowGraph *hookedGraph;					//!< Pointer to the graph that we hooked when creating the node.
	TFlowNodeId nodeId;							//!< Identifier of the node within the graph.
	TFlowGraphId graphId;						//!< Identifier of the FlowGraph this node is located in.
	IMonoObject *managedNode;					//!< Mono object that handles managed side of the node.
	unsigned int flags;							//!< Flags that are set for this node.
public:
	//! Creates a new node and hooks the graph.
	//! The hooking is required to be able to initialize CryMono part of the node, so it can properly react to the events.
	//!
	//! @param pActInfo Information about the node to create.
	CFlowNode_Mono(SActivationInfo* pActInfo)
		: CFlowBaseNodeInternal()
		, activationInfo(pActInfo)
		, cloneType(ENodeCloneType::eNCT_Instanced)
		, managedNode(nullptr)
		, flags(0)
	{
		// This will allow us to continue initialization. Trying to complete it now will make inputs not work.
		// This is because ports are registered upon GetConfiguration call, which happens only after this call.
		this->hookedGraph = this->activationInfo->pGraph;
		this->hookedGraph->RegisterHook(this);
	};
	~CFlowNode_Mono()
	{
		if (this->managedNode)
		{
			this->managedNode->CallMethod("Destroy");
		}
	}
	//! Clones the nodes, if possible.
	//! @param pActInfo Information about the node to clone.
	//! @return A pointer to the new node that is a copy of this one, if it is an instanced node, otherwise returns pointer to this node.
	virtual IFlowNodePtr Clone(SActivationInfo *pActInfo)
	{
		// Only create a new copy, if this node is instanced. I'm not sure about cloned ones.
		return this->cloneType == eNCT_Instanced ? new CFlowNode_Mono(pActInfo) : this;
	};

	virtual void GetMemoryUsage(ICrySizer* s) const
	{
		s->Add(*this);
	}
	//! Gets a description of this node.
	//! This is the method that actually tells the FlowGraph about the specifics of this particular node: name, description, ports etc.
	//! @param config This is the object configuration data must be put into.
	virtual void GetConfiguration(SFlowNodeConfig& config)
	{
		// Throw error, if managed part is not initialized.
		assert(this->managedNode);
		// Call method that fills the configuration data.
		IMonoArray *args = CreateMonoArray(1);
		args->InsertNativePointer(&config);
		bool isSingleton =
			((IMonoObject *)this->managedNode->GetClass()->GetMethod("FillConfiguration", 1)->
						InvokeArray(this->managedNode->GetManagedObject(), args))->Unbox<bool>();
		args->Release(false);
		// Save the flags that were set in Mono.
		this->flags = config.nFlags;
		// Keep the clone type set in Mono.
		this->cloneType = isSingleton ? ENodeCloneType::eNCT_Singleton : ENodeCloneType::eNCT_Instanced;
	}
	//! Invoked by FlowGraph to process event.
	//! @param event Identifier of the event.
	//! @param pActInfo Description of the node that must process the event.
	virtual void ProcessEvent(EFlowEvent event, SActivationInfo* pActInfo)
	{
		// Save in case it will be needed somewhere else.
		this->activationInfo = pActInfo;

		if (this->hookedGraph && this->managedNode)
		{
			// Finally initialization is complete - we can unhook from FlowGraph.
			this->hookedGraph->UnregisterHook(this);
			this->hookedGraph = nullptr;
		}

		switch (event)
		{
		case EFlowEvent::eFE_Update:
			this->managedNode->CallMethod("Think");
			break;
		case EFlowEvent::eFE_Activate:
			// Acquire current status of the node.
			IFlowNodeData *data = pActInfo->pGraph->GetNodeData(pActInfo->myID);
			// Get number of input ports. Decrement number, if this node targets entity.
			int inputsCount =
				(this->flags & EFlowNodeFlags::EFLN_TARGET_ENTITY)
				? data->GetNumInputPorts() - 1
				: data->GetNumInputPorts();
			// Go through the list of ports.
			for (int i = 0; i < inputsCount; i++)
			{
				// Continue, if this port hasn't been activated.
				if (!pActInfo->pInputPorts[i].IsUserFlagSet())
				{
					continue;
				}
				// Otherwise, inform Mono about activation.
				switch (pActInfo->pInputPorts[i].GetType())
				{
				case eFDT_Void:
					this->managedNode->CallMethod("ActivateInputPort", i);
					break;
				case eFDT_Int:
					this->managedNode->CallMethod("ActivateInputPort", i, pActInfo->pInputPorts[i].GetPtr<int>());
					break;
				case eFDT_Float:
					this->managedNode->CallMethod("ActivateInputPort", i, pActInfo->pInputPorts[i].GetPtr<float>());
					break;
				case eFDT_EntityId:
					this->managedNode->CallMethod("ActivateInputPort", i, pActInfo->pInputPorts[i].GetPtr<EntityId>());
					break;
				case eFDT_Vec3:
					this->managedNode->CallMethod("ActivateInputPort", i, pActInfo->pInputPorts[i].GetPtr<Vec3>());
					break;
				case eFDT_String:
					this->managedNode->CallMethod("ActivateInputPort", i, ToMonoString(pActInfo->pInputPorts[i].GetPtr<string>()->data));
					break;
				case eFDT_Bool:
					this->managedNode->CallMethod("ActivateInputPort", i, pActInfo->pInputPorts[i].GetPtr<bool>());
					break;
				default:
					break;
				}
			}
			break;
		case EFlowEvent::eFE_Initialize:
			this->managedNode->CallMethod("Initialize");
			break;
		case EFlowEvent::eFE_SetEntityId:
			if (pActInfo->pEntity)
			{
				// Inform CryMono about the entity.
				IMonoArray *pParams = CreateMonoArray(2);
				pParams->InsertNativePointer(pActInfo->pEntity);
				pParams->Insert(pActInfo->pEntity->GetId());

				this->managedNode->GetClass()->GetMethod("SetTargetEntity", 2)->
					InvokeArray(this->managedNode->GetManagedObject(), pParams);
				pParams->Release();
			}
			break;
		};
	}
	IEntity *GetTargetEntity()
	{
		return this->activationInfo->pEntity;
	}
	template <class T>
	void ActivateOutput(int nPort, const T &value)
	{
		return CFlowBaseNodeInternal::ActivateOutput<T>(this->activationInfo, nPort, value);
	}
	inline void SetRegularlyUpdated(bool update)
	{
		this->activationInfo->pGraph->SetRegularlyUpdated(this->activationInfo->myID, update);
	}
	//! Does not do anything.
	virtual IFlowNodePtr CreateNode(IFlowNode::SActivationInfo*, TFlowNodeTypeId typeId) override { return nullptr; }
	//! Completes the initialization.
	virtual bool CreatedNode(TFlowNodeId id, const char * name, TFlowNodeTypeId typeId, IFlowNodePtr pNode) override
	{
		if (pNode == this)		// Make sure that it's this node that has been created.
		{
			// Get our ids in order.
			this->nodeId = id;
			this->graphId = this->activationInfo->pGraph->GetGraphId();
			// Gather information into an array.
			IMonoArray *args = CreateMonoArray(4);
			// Type name.
			args->Insert(ToMonoString(gEnv->pFlowSystem->GetTypeName(typeId)));
			// Node handle.
			args->InsertNativePointer(this);
			// 2 ids.
			args->Insert(this->nodeId);
			args->Insert(this->graphId);
			// Initialize managed object.
			IMonoObject *nodeObject =
				GetMonoRunTime()->CryBrary->GetClass("FlowGraphNode")->GetMethod("Create")->InvokeArray(nullptr, args);
			args->Release(false);
			// Save reference for later, or tell FlowGraph about an error, if creation has failed for whatever reason.
			if (nodeObject)
			{
				this->managedNode = nodeObject;
				return true;
			}
			return false;
		}
		return true;
	}
	//! Does not do anything.
	virtual void CancelCreatedNode(TFlowNodeId id, const char * name, TFlowNodeTypeId typeId, IFlowNodePtr pNode) override {}

	virtual void AddRef() override { CFlowBaseNodeInternal::AddRef(); }
	virtual void Release() override { CFlowBaseNodeInternal::Release(); }

	//! Does not do anything.
	virtual IFlowGraphHook::EActivation PerformActivation(IFlowGraphPtr pFlowgraph, TFlowNodeId srcNode, TFlowPortId srcPort, TFlowNodeId toNode, TFlowPortId toPort, const TFlowInputData* value) override { return eFGH_Pass; }
};

// The following code would register the flow node class using
// default CryEngine factory. We have our own, so we don't use it.

//REGISTER_FLOW_NODE("FlowNodeGroup:Mono", CFlowNode_Mono);