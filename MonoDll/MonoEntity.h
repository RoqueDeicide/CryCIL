/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Entity class to forward events to C#
//////////////////////////////////////////////////////////////////////////
// ??/??/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#ifndef __MONO_ENTITY__
#define __MONO_ENTITY__

#include <MonoCommon.h>

#include <IGameObject.h>
#include <IAnimatedCharacter.h>

#include <ICryScriptInstance.h>

struct SMonoColliderInfo
{
	SMonoColliderInfo(EventPhysCollision *pCollision, int i)
	{
		foreignId = pCollision->iForeignData[i];
		pForeignData = pCollision->pForeignData[i];

		vloc = pCollision->vloc[i];
		mass = pCollision->mass[i];
		partId = pCollision->partid[i];
		idmat = pCollision->idmat[i];
		iPrim = pCollision->iPrim[i];
	}

	int foreignId;
	void *pForeignData;

	Vec3 vloc;
	float mass;
	int partId;
	short idmat;
	short iPrim;
};

struct IMonoObject;
struct IMonoArray;

struct SQueuedProperty
{
	SQueuedProperty() {}

	SQueuedProperty(IEntityPropertyHandler::SPropertyInfo propInfo, const char *val)
		: propertyInfo(propInfo)
		, value(string(val)) {}

	string value;
	IEntityPropertyHandler::SPropertyInfo propertyInfo;
};

class CMonoEntityExtension
	: public CGameObjectExtensionHelper<CMonoEntityExtension, IGameObjectExtension>
{
public:
	CMonoEntityExtension();
	virtual ~CMonoEntityExtension();

	// IGameObjectExtension
	virtual bool Init(IGameObject *pGameObject);
	virtual void InitClient( int channelId ) {}
	virtual void PostInit(IGameObject *pGameObject);
	virtual void PostInitClient( int channelId ) {}
	virtual bool ReloadExtension( IGameObject* pGameObject, const SEntitySpawnParams& params ) { return false; }
	virtual void PostReloadExtension( IGameObject* pGameObject, const SEntitySpawnParams& params ) {}
	virtual bool GetEntityPoolSignature( TSerialize signature ) { return false; }
	virtual void Release() { delete this; }
	virtual void FullSerialize(TSerialize ser);
	virtual bool NetSerialize( TSerialize ser, EEntityAspects aspect, uint8 profile, int flags );
	virtual void PostSerialize();
	virtual void SerializeSpawnInfo( TSerialize ser ) {}
	virtual ISerializableInfoPtr GetSpawnInfo() { return 0; }
	virtual void Update( SEntityUpdateContext& ctx, int updateSlot ) {}
	virtual void PostUpdate(float frameTime);
	virtual void PostRemoteSpawn() {}
	virtual void HandleEvent( const SGameObjectEvent& event ) {}
	virtual void ProcessEvent( SEntityEvent& event );
	virtual void SetChannelId( uint16 id ) {}
	virtual void SetAuthority( bool auth ) {}
	virtual void GetMemoryUsage( ICrySizer* s ) const { s->Add( *this ); }
	// ~IGameObjectExtension

	struct RMIParams
	{
		RMIParams() : args(NULL) {}
		RMIParams(mono::object _args, const char *funcName, EntityId target);

		void SerializeWith(TSerialize ser);

		mono::object args;
		string methodName;
		EntityId targetId;
	};

	DECLARE_SERVER_RMI_NOATTACH(SvScriptRMI, RMIParams, eNRT_ReliableUnordered);
	DECLARE_CLIENT_RMI_NOATTACH(ClScriptRMI, RMIParams, eNRT_ReliableUnordered);

	IMonoObject *GetScript() { return m_pScript; }

	void SetPropertyValue(IEntityPropertyHandler::SPropertyInfo propertyInfo, const char *value);

	bool IsInitialized() { return m_bInitialized; }

	void Reset(bool enteringGamemode);

protected:
	ICryScriptInstance *m_pScript;

	IAnimatedCharacter *m_pAnimatedCharacter;

	bool m_bInitialized;
	bool m_bDestroyed;

public:
	static void CacheMethods();

	static IMonoMethod *m_pOnRemoteInvocation;

private:
	static IMonoMethod *m_pInternalNetSerialize;
	static IMonoMethod *m_pInternalFullSerialize;

	static IMonoMethod *m_pPostSerialize;
};

#endif