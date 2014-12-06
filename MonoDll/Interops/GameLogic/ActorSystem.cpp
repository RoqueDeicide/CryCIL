#include "StdAfx.h"
#include "ActorSystem.h"

#include "Actor.h"

#include "MonoRunTime.h"
#include "MonoRunTime.h"

#include "MonoEntity.h"

#include <IGameFramework.h>

ActorSystemInterop::TActorClasses ActorSystemInterop::m_monoActorClasses = TActorClasses();

ActorSystemInterop::ActorSystemInterop()
{
	REGISTER_METHOD(GetActorInfoByChannelId);
	REGISTER_METHOD(GetActorInfoById);

	REGISTER_METHOD(RegisterActorClass);
	REGISTER_METHOD(CreateActor);
	REGISTER_METHOD(RemoveActor);

	REGISTER_METHOD(GetClientActorId);

	REGISTER_METHOD(RemoteInvocation);

	gEnv->pEntitySystem->AddSink(this, IEntitySystem::OnSpawn, 0);
}

ActorSystemInterop::~ActorSystemInterop()
{
	if (gEnv->pEntitySystem)
		gEnv->pEntitySystem->RemoveSink(this);
	else
		MonoWarning("Failed to unregister CActorSystem entity sink!");

	m_monoActorClasses.clear();
}

EMonoActorType ActorSystemInterop::GetMonoActorType(const char *actorClassName)
{
	for each(auto classPair in m_monoActorClasses)
	{
		if (!strcmp(classPair.first, actorClassName))
			return classPair.second;
	}

	return EMonoActorType_None;
}

void ActorSystemInterop::OnSpawn(IEntity *pEntity, SEntitySpawnParams &params)
{
	const char *actorName = pEntity->GetClass()->GetName();
	EMonoActorType actorType = GetMonoActorType(actorName);

	if (actorType != EMonoActorType_None)
	{
		// Tell CryMono to create a managed wrapper for this actor.
		if (IActor *pActor = GetMonoRunTime()->GameFramework->GetIActorSystem()->GetActor(pEntity->GetId()))
		{
			// Box the actor information.
			IMonoClass *pActorInfoClass =
				GetMonoRunTime()->CryBrary->GetClass("ActorInitializationParams", "CryEngine.Native");
			SMonoActorInfo actorInfo(pActor);

			IMonoArray *pArgs = CreateMonoArray(2);
			pArgs->InsertMonoString(ToMonoString(actorName));
			pArgs->InsertMonoObject(pActorInfoClass->BoxObject(&actorInfo));
			// Create the wrapper.
			IMonoObject *wrapper = *GetMonoRunTime()->CryBrary->
				GetClass("Actor", "CryEngine.Actors")->
				GetMethod("Create")->InvokeArray(nullptr, pArgs);
			if (actorType == EMonoActorType_Managed)
			{
				CMonoActor *monoActor = static_cast<CMonoActor *>(pActor);
				monoActor->m_pManagedObject = wrapper->GetManagedObject();
			}
			SAFE_RELEASE(pArgs);
		}
	}
}

SMonoActorInfo ActorSystemInterop::GetActorInfoByChannelId(uint16 channelId)
{
	if (IActor *pActor = GetMonoRunTime()->GameFramework->GetIActorSystem()->GetActorByChannelId(channelId))
		return SMonoActorInfo(pActor);

	return SMonoActorInfo();
}

SMonoActorInfo ActorSystemInterop::GetActorInfoById(EntityId id)
{
	if (IActor *pActor = GetMonoRunTime()->GameFramework->GetIActorSystem()->GetActor(id))
		return SMonoActorInfo(pActor);

	return SMonoActorInfo();
}

void ActorSystemInterop::RegisterActorClass(mono::string name, bool isNative)
{
	const char *className = ToCryString(name);

	if (!isNative)
	{
		if (gEnv->pEntitySystem->GetClassRegistry()->FindClass(className))
		{
			MonoWarning("Aborting registration of actor class %s, a class with the same name already exists", className);
			return;
		}
		// Register a factory.
		GetMonoRunTime()->GameFramework->RegisterFactory(className, (CMonoActor *)0, false, (CMonoActor *)0);
	}

	m_monoActorClasses.insert(TActorClasses::value_type(className, isNative ? EMonoActorType_Native : EMonoActorType_Managed));
}

SMonoActorInfo ActorSystemInterop::CreateActor(int channelId, mono::string name, mono::string className, Vec3 pos, Quat rot, Vec3 scale)
{
	const char *sClassName = ToCryString(className);

	if (IGameFramework *pGameFramework = GetMonoRunTime()->GameFramework)
	{
		if (IActorSystem *pActorSystem = pGameFramework->GetIActorSystem())
		{
			if (IActor *pActor = pActorSystem->CreateActor(channelId, ToCryString(name), sClassName, ZERO, IDENTITY, Vec3(1, 1, 1)))
				return SMonoActorInfo(pActor);
		}
	}

	return SMonoActorInfo();
}

void ActorSystemInterop::RemoveActor(EntityId id)
{
	GetMonoRunTime()->GameFramework->GetIActorSystem()->RemoveActor(id);
}

EntityId ActorSystemInterop::GetClientActorId()
{
	return GetMonoRunTime()->GameFramework->GetClientActorId();
}

void ActorSystemInterop::RemoteInvocation(EntityId entityId, EntityId targetId, mono::string methodName, mono::object args, ERMInvocation target, int channelId)
{
	CRY_ASSERT(entityId != 0);

	IGameObject *pGameObject = GetMonoRunTime()->GameFramework->GetGameObject(entityId);
	CRY_ASSERT(pGameObject);

	CMonoEntityExtension::RMIParams params(args, ToCryString(methodName), targetId);

	if (target & eRMI_ToServer)
		pGameObject->InvokeRMI(CMonoActor::SvScriptRMI(), params, target, channelId);
	else
		pGameObject->InvokeRMI(CMonoActor::ClScriptRMI(), params, target, channelId);
}