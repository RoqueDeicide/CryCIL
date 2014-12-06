/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// ActorSystem scriptbind; used to get and set actor C++ properties.
//////////////////////////////////////////////////////////////////////////
// 06/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_ACTORSYSTEM_H__
#define __SCRIPTBIND_ACTORSYSTEM_H__

#include <IMonoInterop.h>

#include <MonoCommon.h>

#include <IActorSystem.h>

struct SMonoActorInfo
{
	SMonoActorInfo() : id(0) {}

	SMonoActorInfo(IActor *pIActor)
		: pActor(pIActor)
	{
		pEntity = pIActor->GetEntity();
		id = pEntity->GetId();
		channelId = pActor->GetChannelId();
	}

	SMonoActorInfo(EntityId entId, int chId)
		: id(entId)
		, channelId(chId)
	{}

	IEntity *pEntity;
	IActor *pActor;
	EntityId id;
	int channelId;
};

enum EMonoActorType
{
	EMonoActorType_Managed,
	EMonoActorType_Native,
	EMonoActorType_None,
};

class ActorSystemInterop
	: public IMonoInterop
	, public IEntitySystemSink
{
public:
	ActorSystemInterop();
	~ActorSystemInterop();

	// IEntitySystemSink
	virtual bool OnBeforeSpawn(SEntitySpawnParams &params) { return true; }
	virtual void OnSpawn(IEntity *pEntity, SEntitySpawnParams &params);
	virtual bool OnRemove(IEntity *pEntity) { return true; }
	virtual void OnReused(IEntity *pEntity, SEntitySpawnParams &params) {}
	virtual void OnEvent(IEntity *pEntity, SEntityEvent &event) {}
	// ~IEntitySystemSink

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() { return "ActorInterop"; }
	// ~IMonoScriptBind

	static EMonoActorType GetMonoActorType(const char *actorClassName);

	// externals
	static SMonoActorInfo GetActorInfoByChannelId(uint16 channelId);
	static SMonoActorInfo GetActorInfoById(EntityId id);

	static void RegisterActorClass(mono::string name, bool isNative);
	static SMonoActorInfo CreateActor(int channelId, mono::string name, mono::string className, Vec3 pos, Quat rot, Vec3 scale);
	static void RemoveActor(EntityId id);

	static EntityId GetClientActorId();

	static void RemoteInvocation(EntityId entityId, EntityId targetId, mono::string methodName, mono::object args, ERMInvocation target, int channelId);

	typedef std::map<const char *, EMonoActorType> TActorClasses;
	static TActorClasses m_monoActorClasses;
};

#endif //__SCRIPTBIND_ACTORSYSTEM_H__