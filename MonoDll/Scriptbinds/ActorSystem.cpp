#include "StdAfx.h"
#include "ActorSystem.h"

#include "MonoScriptSystem.h"
#include "Actor.h"

#include "MonoEntity.h"

#include <IGameFramework.h>

CScriptbind_ActorSystem::TActorClasses CScriptbind_ActorSystem::m_monoActorClasses = TActorClasses();

CScriptbind_ActorSystem::CScriptbind_ActorSystem()
{
	REGISTER_METHOD(GetPlayerHealth);
	REGISTER_METHOD(SetPlayerHealth);
	REGISTER_METHOD(GetPlayerMaxHealth);
	REGISTER_METHOD(SetPlayerMaxHealth);

	REGISTER_METHOD(GetActorInfoByChannelId);
	REGISTER_METHOD(GetActorInfoById);

	REGISTER_METHOD(RegisterActorClass);
	REGISTER_METHOD(CreateActor);
	REGISTER_METHOD(RemoveActor);

	REGISTER_METHOD(GetClientActorId);

	REGISTER_METHOD(RemoteInvocation);

	gEnv->pEntitySystem->AddSink(this, IEntitySystem::OnSpawn, 0);
}

CScriptbind_ActorSystem::~CScriptbind_ActorSystem()
{
	if(gEnv->pEntitySystem )
		gEnv->pEntitySystem->RemoveSink(this);
	else
		MonoWarning("Failed to unregister CActorSystem entity sink!");

	m_monoActorClasses.clear();
}

EMonoActorType CScriptbind_ActorSystem::GetMonoActorType(const char *actorClassName)
{
	for each(auto classPair in m_monoActorClasses)
	{
		if(!strcmp(classPair.first, actorClassName))
			return classPair.second;
	}

	return EMonoActorType_None;
}

void CScriptbind_ActorSystem::OnSpawn(IEntity *pEntity,SEntitySpawnParams &params)
{
	EMonoActorType actorType = GetMonoActorType(pEntity->GetClass()->GetName());

	if(actorType != EMonoActorType_None)
	{
		if(IActor *pActor = g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->GetActor(pEntity->GetId()))
		{
			ICryScriptInstance *pScript  = g_pScriptSystem->InstantiateScript(pEntity->GetClass()->GetName(), eScriptFlag_Actor);

			IMonoClass *pActorInfoClass = g_pScriptSystem->GetCryBraryAssembly()->GetClass("ActorInitializationParams", "CryEngine.Native");

			SMonoActorInfo actorInfo(pActor);

			IMonoArray *pArgs = CreateMonoArray(1);
			pArgs->InsertMonoObject(pActorInfoClass->BoxObject(&actorInfo));

			g_pScriptSystem->InitializeScriptInstance(pScript, pArgs);
			SAFE_RELEASE(pArgs);
		}
	}
}

SMonoActorInfo CScriptbind_ActorSystem::GetActorInfoByChannelId(uint16 channelId)
{
	if(IActor *pActor = g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->GetActorByChannelId(channelId))
		return SMonoActorInfo(pActor);

	return SMonoActorInfo();
}

SMonoActorInfo CScriptbind_ActorSystem::GetActorInfoById(EntityId id)
{
	if(IActor *pActor = g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->GetActor(id))
		return SMonoActorInfo(pActor);

	return SMonoActorInfo();
}

void CScriptbind_ActorSystem::RegisterActorClass(mono::string name, bool isNative,  bool isAI)
{
	const char *className = ToCryString(name);

	if(!isNative)
	{
		if(gEnv->pEntitySystem->GetClassRegistry()->FindClass(className))
		{
			MonoWarning("Aborting registration of actor class %s, a class with the same name already exists", className); 
			return;
		}

		g_pScriptSystem->GetIGameFramework()->RegisterFactory(className, (CMonoActor *)0, isAI, (CMonoActor *)0);
	}

	m_monoActorClasses.insert(TActorClasses::value_type(className, isNative ? EMonoActorType_Native : EMonoActorType_Managed));
}

SMonoActorInfo CScriptbind_ActorSystem::CreateActor(int channelId, mono::string name, mono::string className, Vec3 pos, Quat rot, Vec3 scale)
{
	const char *sClassName = ToCryString(className);

	if(IActor *pActor = g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->CreateActor(channelId, ToCryString(name), sClassName, pos, rot, scale))
		return SMonoActorInfo(pActor);

	return SMonoActorInfo();
}

void CScriptbind_ActorSystem::RemoveActor(EntityId id)
{
	g_pScriptSystem->GetIGameFramework()->GetIActorSystem()->RemoveActor(id);
}

EntityId CScriptbind_ActorSystem::GetClientActorId()
{
	return g_pScriptSystem->GetIGameFramework()->GetClientActorId();
}

float CScriptbind_ActorSystem::GetPlayerHealth(IActor *pActor)
{
	return pActor->GetHealth();
}

void CScriptbind_ActorSystem::SetPlayerHealth(IActor *pActor, float newHealth)
{
	pActor->SetHealth(newHealth);
}

float CScriptbind_ActorSystem::GetPlayerMaxHealth(IActor *pActor)
{
	return pActor->GetMaxHealth();
}

void CScriptbind_ActorSystem::SetPlayerMaxHealth(IActor *pActor, float newMaxHealth)
{
	pActor->SetMaxHealth(newMaxHealth);
}

void CScriptbind_ActorSystem::RemoteInvocation(EntityId entityId, EntityId targetId, mono::string methodName, mono::object args, ERMInvocation target, int channelId)
{
	CRY_ASSERT(entityId != 0);

	IGameObject *pGameObject = g_pScriptSystem->GetIGameFramework()->GetGameObject(entityId);
	CRY_ASSERT(pGameObject);

	CMonoEntityExtension::RMIParams params(args, ToCryString(methodName), targetId);

	if(target & eRMI_ToServer)
		pGameObject->InvokeRMI(CMonoActor::SvScriptRMI(), params, target, channelId);
	else
		pGameObject->InvokeRMI(CMonoActor::ClScriptRMI(), params, target, channelId);
}