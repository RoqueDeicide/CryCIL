#include "StdAfx.h"
#include "FlowManager.h"

#include "Interops/FlowGraph/FlowGraphNode_Mono.h"

#include "MonoCommon.h"
#include <IMonoArray.h>
#include <IMonoAssembly.h>

#include "MonoScriptSystem.h"
#include <IGameFramework.h>

CFlowManager::CFlowManager()
: m_refs(0)
{
	REGISTER_METHOD(RegisterNode);

	REGISTER_METHOD(ActivateOutput);
	REGISTER_METHOD(ActivateOutputInt);
	REGISTER_METHOD(ActivateOutputFloat);
	REGISTER_METHOD(ActivateOutputEntityId);
	REGISTER_METHOD(ActivateOutputString);
	REGISTER_METHOD(ActivateOutputBool);
	REGISTER_METHOD(ActivateOutputVec3);

	REGISTER_METHOD(GetTargetEntity);

	REGISTER_METHOD(SetRegularlyUpdated);
}

CFlowManager::~CFlowManager()
{
	// The following code is commented, because flow manager is not located in the list of interops.

	//static_cast<CScriptSystem *>(GetMonoScriptSystem())->EraseBinding(this);
}

void CFlowManager::RegisterNode(mono::string monoTypeName)
{
	IFlowSystem *pFlowSystem = gEnv->pGame->GetIGameFramework()->GetIFlowSystem();
	if (!pFlowSystem)
		return;

	const char *typeName = ToCryString(monoTypeName);

	CFlowManager *pFlowManager = static_cast<CScriptSystem *>(GetMonoScriptSystem())->GetFlowManager();
	if (!pFlowManager)
	{
		MonoWarning("Aborting registration of node type %s, flow manager was null!", typeName);
		return;
	}

	pFlowSystem->RegisterType(typeName, (IFlowNodeFactoryPtr)pFlowManager);
}

IFlowNodePtr CFlowManager::Create(IFlowNode::SActivationInfo *pActInfo)
{
	return new CFlowNode_Mono(pActInfo);
}

void CFlowManager::ActivateOutput(CFlowNode_Mono *pNode, int index) { pNode->ActivateOutput(index, 0); }
void CFlowManager::ActivateOutputInt(CFlowNode_Mono *pNode, int index, int value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputFloat(CFlowNode_Mono *pNode, int index, float value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputEntityId(CFlowNode_Mono *pNode, int index, EntityId value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputString(CFlowNode_Mono *pNode, int index, mono::string value) { pNode->ActivateOutput(index, string(ToCryString(value))); }
void CFlowManager::ActivateOutputBool(CFlowNode_Mono *pNode, int index, bool value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputVec3(CFlowNode_Mono *pNode, int index, Vec3 value) { pNode->ActivateOutput(index, value); }

IEntity *CFlowManager::GetTargetEntity(CFlowNode_Mono *pNode, EntityId &id)
{
	if (IEntity *pEntity = pNode->GetTargetEntity())
	{
		id = pEntity->GetId();

		return pEntity;
	}

	MonoWarning("CFlowManager::GetTargetEntity returning nullptr target entity!");
	return nullptr;
}

void CFlowManager::SetRegularlyUpdated(CFlowNode_Mono *pNode, bool update)
{
	pNode->SetRegularlyUpdated(update);
}