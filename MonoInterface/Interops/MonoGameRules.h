#pragma once

#include "IMonoInterface.h"
#include <IGameRulesSystem.h>


class MonoGameRules : public CGameObjectExtensionHelper<MonoGameRules, IGameRules>
{
	MonoGCHandle objHandle;
public:
	MonoGameRules();
	virtual ~MonoGameRules();

	//IGameRules
	virtual bool ShouldKeepClient(int channelId, EDisconnectionCause cause, const char *desc) const override;
	virtual void PrecacheLevel() override;

	virtual void PrecacheLevelResource(const char*, EGameResourceType) override {}
	virtual XmlNodeRef FindPrecachedXmlFile(const char *) override { return XmlNodeRef(); }

	virtual void OnConnect(struct INetChannel *pNetChannel) override;
	virtual void OnDisconnect(EDisconnectionCause cause, const char *desc) override;
	virtual bool OnClientConnect(int channelId, bool isReset) override;
	virtual void OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc,
									bool keepClient) override;
	virtual bool OnClientEnteredGame(int channelId, bool isReset) override;
	virtual void OnEntitySpawn(IEntity *pEntity) override;
	virtual void OnEntityRemoved(IEntity *pEntity) override;
	virtual void OnEntityReused(IEntity *pEntity, SEntitySpawnParams &params, EntityId prevId) override;
	virtual void SendTextMessage(ETextMessageType type, const char *msg, uint32 to = eRMI_ToAllClients,
								 int channelId = -1, const char *p0 = nullptr, const char *p1 = nullptr,
								 const char *p2 = nullptr, const char *p3 = nullptr) override;
	virtual void SendChatMessage(EChatMessageType type, EntityId sourceId, EntityId targetId,
								 const char *msg) override;

	virtual void ClientHit(const HitInfo &) override {}
	virtual void ServerHit(const HitInfo &) override {}
	virtual int GetHitTypeId(const uint32 ) const override { return 0; }
	virtual int GetHitTypeId(const char *) const override { return 0; }

	virtual const char *GetHitType(int) const override { return nullptr; }
	virtual void OnVehicleDestroyed(EntityId) override {}
	virtual void OnVehicleSubmerged(EntityId, float) override {}
	virtual bool CanEnterVehicle(EntityId) override { return false; }
	virtual void CreateEntityRespawnData(EntityId) override {}
	virtual bool HasEntityRespawnData(EntityId) const override { return false; }
	virtual void ScheduleEntityRespawn(EntityId, bool, float) override {}
	virtual void AbortEntityRespawn(EntityId, bool) override {}
	virtual void ScheduleEntityRemoval(EntityId, float, bool) override {}
	virtual void AbortEntityRemoval(EntityId) override {}
	virtual void AddHitListener(IHitListener*) override {}
	virtual void RemoveHitListener(IHitListener*) override {}

	virtual bool OnCollision(const SGameCollision& _event) override;

	virtual void OnCollision_NotifyAI(const EventPhys *) override {}

	virtual void ShowStatus() override;
	virtual bool IsTimeLimited() const override;
	virtual float GetRemainingGameTime() const override;
	virtual void SetRemainingGameTime(float seconds) override;

	virtual void ClearAllMigratingPlayers(void) override {}
	virtual EntityId SetChannelForMigratingPlayer(const char*, uint16) override { return 0; }
	virtual void StoreMigratingPlayer(IActor*) override {}
	virtual const char *GetTeamName(int) const override { return nullptr; }
	virtual void GetMemoryUsage(ICrySizer *) const override {}

	//IGameObjectExtension
	virtual bool Init(IGameObject *pGameObject) override;
	virtual void PostInit(IGameObject *pGameObject) override;
	virtual void ProcessEvent(SEntityEvent& event) override;
	virtual void InitClient(int channelId) override;
	virtual void PostInitClient(int channelId) override;
	virtual bool ReloadExtension(IGameObject *pGameObject, const SEntitySpawnParams &params) override;
	virtual void PostReloadExtension(IGameObject *pGameObject, const SEntitySpawnParams &params) override;
	virtual bool GetEntityPoolSignature(TSerialize signature) override;
	virtual void Release() override { delete this; }
	virtual void FullSerialize(TSerialize ser) override;
	virtual bool NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int pflags) override;
	virtual void PostSerialize() override {}
	virtual void SerializeSpawnInfo(TSerialize) override {}
	virtual ISerializableInfoPtr GetSpawnInfo() override { return nullptr; }
	virtual void Update(SEntityUpdateContext& ctx, int updateSlot) override;
	virtual void HandleEvent(const SGameObjectEvent&) override {}
	virtual void SetChannelId(uint16 id) override;
	virtual void SetAuthority(bool auth) override;
	virtual void PostUpdate(float frameTime) override;
	virtual void PostRemoteSpawn() override {}

	mono::object GetManagedWrapper() const
	{
		return this->objHandle.Object;
	}
	__declspec(property(get = GetManagedWrapper)) mono::object MonoWrapper;
private:
	template<const char *methodName, typename ResultType, typename TArg0, typename TArg1, typename TArg2, typename TArg3, typename TArg4>
	ResultType CallFunc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) const;
	template<const char *methodName, typename ResultType, typename TArg0, typename TArg1, typename TArg2, typename TArg3>
	ResultType CallFunc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3) const;
	template<const char *methodName, typename ResultType, typename TArg0, typename TArg1, typename TArg2>
	ResultType CallFunc(TArg0 arg0, TArg1 arg1, TArg2 arg2) const;
	template<const char *methodName, typename ResultType, typename TArg0, typename TArg1>
	ResultType CallFunc(TArg0 arg0, TArg1 arg1) const;
	template<const char *methodName, typename ResultType, typename TArg0>
	ResultType CallFunc(TArg0 arg0) const;
	template<const char *methodName, typename ResultType>
	ResultType CallFunc() const;
	template<const char *methodName, typename TArg0, typename TArg1, typename TArg2, typename TArg3, typename TArg4>
	void CallProc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) const;
	template<const char *methodName, typename TArg0, typename TArg1, typename TArg2, typename TArg3>
	void CallProc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3) const;
	template<const char *methodName, typename TArg0, typename TArg1, typename TArg2>
	void CallProc(TArg0 arg0, TArg1 arg1, TArg2 arg2) const;
	template<const char *methodName, typename TArg0, typename TArg1>
	void CallProc(TArg0 arg0, TArg1 arg1) const;
	template<const char *methodName, typename TArg0>
	void CallProc(TArg0 arg0) const;
	template<const char *methodName>
	void CallProc() const;
};
