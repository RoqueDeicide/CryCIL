/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Entity class to forward events to C#
//////////////////////////////////////////////////////////////////////////
// ??/??/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#ifndef __NATIVE_ENTITY__
#define __NATIVE_ENTITY__

#include <MonoCommon.h>

#include <IGameObject.h>
#include <IAnimatedCharacter.h>

struct IMonoObject;
struct IMonoArray;

class CNativeEntity
	: public CGameObjectExtensionHelper<CNativeEntity, IGameObjectExtension>
{
public:
	CNativeEntity();
	virtual ~CNativeEntity() {}

	// IGameObjectExtension
	virtual bool Init(IGameObject *pGameObject);
	virtual void InitClient(int channelId) {}
	virtual void PostInit(IGameObject *pGameObject) {}
	virtual void PostInitClient(int channelId) {}
	virtual bool ReloadExtension(IGameObject* pGameObject, const SEntitySpawnParams& params) { return false; }
	virtual void PostReloadExtension(IGameObject* pGameObject, const SEntitySpawnParams& params) {}
	virtual bool GetEntityPoolSignature(TSerialize signature) { return false; }
	virtual void Release() { delete this; }
	virtual void FullSerialize(TSerialize ser) {}
	virtual bool NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int flags) { return true; }
	virtual void PostSerialize() {}
	virtual void SerializeSpawnInfo(TSerialize ser) {}
	virtual ISerializableInfoPtr GetSpawnInfo() { return 0; }
	virtual void Update(SEntityUpdateContext& ctx, int updateSlot) {}
	virtual void PostUpdate(float frameTime) {}
	virtual void PostRemoteSpawn() {}
	virtual void HandleEvent(const SGameObjectEvent& event) {}
	virtual void ProcessEvent(SEntityEvent& event);
	virtual void SetChannelId(uint16 id) {}
	virtual void SetAuthority(bool auth) {}
	virtual void GetMemoryUsage(ICrySizer* s) const { s->Add(*this); }
	// ~IGameObjectExtension

	void Physicalize();

	bool IsInitialized() { return m_bInitialized; }

protected:
	bool m_bInitialized;
};

#endif //__NATIVE_ENTITY__