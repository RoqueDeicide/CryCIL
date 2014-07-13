/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// The FlowManager handles registering of Mono flownodes.
//////////////////////////////////////////////////////////////////////////
// 23/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __FLOW_MANAGER__
#define __FLOW_MANAGER__

#include "MonoCommon.h"
#include <IMonoScriptBind.h>

#include <IFlowSystem.h>

struct IMonoArray;
struct IMonoScript;

class CMonoFlowNode;

// Passed down to node script when initializing
struct SMonoNodeInfo
{
	SMonoNodeInfo(CMonoFlowNode *node, TFlowNodeId _id, TFlowGraphId _graphId)
	: pNode(node)
		, id(_id)
		, graphId(_graphId)
	{}

	CMonoFlowNode *pNode;
	TFlowNodeId id;
	TFlowGraphId graphId;
};

class CFlowManager
	: public IMonoScriptBind
	, public IFlowNodeFactory
{
public:
	CFlowManager();
	~CFlowManager();

	// IFlowNodeFactory
	virtual void AddRef() override { ++m_refs; }
	// We want to manually kill this off, since it's used so often.
	virtual void Release() override { if (0 >= --m_refs) delete this; }
	IFlowNodePtr Create(IFlowNode::SActivationInfo *pActInfo) override;

	virtual void GetMemoryUsage(ICrySizer * s) const override
	{
		SIZER_SUBCOMPONENT_NAME(s, "CFlowManager");
		s->Add(*this);
	}
	virtual void Reset() override {}
	// ~IFlowNodeFactory

	// IMonoScriptSystemListener
	virtual void OnPreScriptCompilation(bool isReload) {}
	virtual void OnPostScriptCompilation(bool isReload, bool compilationSuccess) {}

	virtual void OnPreScriptReload(bool initialLoad) {}
	virtual void OnPostScriptReload(bool initialLoad) { if (initialLoad) Reset(); }
	// ~IMonoScriptSystemListener

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() { return "NativeFlowNodeMethods"; }
	virtual const char *GetNamespace() { return "CryEngine.Flowgraph.Native"; }
	// ~IMonoScriptBind

	static void RegisterNode(mono::string typeName);

	static bool IsPortActive(CMonoFlowNode *pNode, int);

	static int GetPortValueInt(CMonoFlowNode *pNode, int);
	static float GetPortValueFloat(CMonoFlowNode *pNode, int);
	static EntityId GetPortValueEntityId(CMonoFlowNode *pNode, int);
	static mono::string GetPortValueString(CMonoFlowNode *pNode, int);
	static bool GetPortValueBool(CMonoFlowNode *pNode, int);
	static Vec3 GetPortValueVec3(CMonoFlowNode *pNode, int);

	static void ActivateOutput(CMonoFlowNode *pNode, int);
	static void ActivateOutputInt(CMonoFlowNode *pNode, int, int);
	static void ActivateOutputFloat(CMonoFlowNode *pNode, int, float);
	static void ActivateOutputEntityId(CMonoFlowNode *pNode, int, EntityId);
	static void ActivateOutputString(CMonoFlowNode *pNode, int, mono::string);
	static void ActivateOutputBool(CMonoFlowNode *pNode, int, bool);
	static void ActivateOutputVec3(CMonoFlowNode *pNode, int, Vec3);

	static bool IsOutputConnected(CMonoFlowNode *pNode, int);

	static IEntity *GetTargetEntity(CMonoFlowNode *pNode, EntityId &id);

	static void SetRegularlyUpdated(CMonoFlowNode *pNode, bool update);

	int m_refs;
};

#endif //__FLOW_MANAGER__