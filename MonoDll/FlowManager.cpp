#include "StdAfx.h"
#include "FlowManager.h"

#include "MonoFlowNode.h"

#include "MonoCommon.h"
#include <IMonoArray.h>
#include <IMonoAssembly.h>

#include "MonoScriptSystem.h"
#include <IGameFramework.h>

CFlowManager::CFlowManager()
: m_refs(0)
{
	REGISTER_METHOD(RegisterNode);

	REGISTER_METHOD(IsPortActive);

	REGISTER_METHOD(ActivateOutput);
	REGISTER_METHOD(ActivateOutputInt);
	REGISTER_METHOD(ActivateOutputFloat);
	REGISTER_METHOD(ActivateOutputEntityId);
	REGISTER_METHOD(ActivateOutputString);
	REGISTER_METHOD(ActivateOutputBool);
	REGISTER_METHOD(ActivateOutputVec3);

	REGISTER_METHOD(GetPortValueInt);
	REGISTER_METHOD(GetPortValueFloat);
	REGISTER_METHOD(GetPortValueEntityId);
	REGISTER_METHOD(GetPortValueString);
	REGISTER_METHOD(GetPortValueBool);
	REGISTER_METHOD(GetPortValueVec3);

	REGISTER_METHOD(IsOutputConnected);

	REGISTER_METHOD(GetTargetEntity);

	REGISTER_METHOD(SetRegularlyUpdated);
}

CFlowManager::~CFlowManager()
{
	static_cast<CScriptSystem *>(GetMonoScriptSystem())->EraseBinding(this);
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
	return new CMonoFlowNode(pActInfo);
}

void CFlowManager::ActivateOutput(CMonoFlowNode *pNode, int index) { pNode->ActivateOutput(index, 0); }
void CFlowManager::ActivateOutputInt(CMonoFlowNode *pNode, int index, int value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputFloat(CMonoFlowNode *pNode, int index, float value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputEntityId(CMonoFlowNode *pNode, int index, EntityId value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputString(CMonoFlowNode *pNode, int index, mono::string value) { pNode->ActivateOutput(index, string(ToCryString(value))); }
void CFlowManager::ActivateOutputBool(CMonoFlowNode *pNode, int index, bool value) { pNode->ActivateOutput(index, value); }
void CFlowManager::ActivateOutputVec3(CMonoFlowNode *pNode, int index, Vec3 value) { pNode->ActivateOutput(index, value); }

bool CFlowManager::IsPortActive(CMonoFlowNode *pFlowNode, int port)
{
	return pFlowNode->IsPortActive(port);
}

int CFlowManager::GetPortValueInt(CMonoFlowNode *pFlowNode, int index)
{
	return pFlowNode->GetPortInt(index);
}

float CFlowManager::GetPortValueFloat(CMonoFlowNode *pFlowNode, int index)
{
	return pFlowNode->GetPortFloat(index);
}

EntityId CFlowManager::GetPortValueEntityId(CMonoFlowNode *pFlowNode, int index)
{
	return pFlowNode->GetPortEntityId(index);
}

mono::string CFlowManager::GetPortValueString(CMonoFlowNode *pFlowNode, int index)
{
	return (mono::string)ToMonoString(pFlowNode->GetPortString(index));
}

bool CFlowManager::GetPortValueBool(CMonoFlowNode *pFlowNode, int index)
{
	return pFlowNode->GetPortBool(index);
}

Vec3 CFlowManager::GetPortValueVec3(CMonoFlowNode *pFlowNode, int index)
{
	return pFlowNode->GetPortVec3(index);
}

bool CFlowManager::IsOutputConnected(CMonoFlowNode *pNode, int index)
{
	return pNode->IsOutputConnected(index);
}

IEntity *CFlowManager::GetTargetEntity(CMonoFlowNode *pNode, EntityId &id)
{
	if (IEntity *pEntity = pNode->GetTargetEntity())
	{
		id = pEntity->GetId();

		return pEntity;
	}

	MonoWarning("CFlowManager::GetTargetEntity returning nullptr target entity!");
	return nullptr;
}

void CFlowManager::SetRegularlyUpdated(CMonoFlowNode *pNode, bool update)
{
	pNode->SetRegularlyUpdated(update);
}