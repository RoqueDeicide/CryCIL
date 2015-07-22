#pragma once

#include "IMonoInterface.h"

#include "IGameObject.h"

//! Serves as an abstraction layer between logic defined in C# and CryEngine.
struct MonoEntityExtension : CGameObjectExtensionHelper<MonoEntityExtension, IGameObjectExtension>
{
private:
	MonoGCHandle objHandle;
	bool networking;
public:
	MonoEntityExtension();
	virtual ~MonoEntityExtension();
	//IGameObjectExtension
	virtual bool Init(IGameObject* pGameObject) override;
	virtual void PostInit(IGameObject* pGameObject) override;
	virtual void HandleEvent(const SGameObjectEvent& event) override {}
	virtual void ProcessEvent(SEntityEvent& event) override;
	virtual void InitClient(int channelId) override;
	virtual void PostInitClient(int channelId) override;
	virtual bool ReloadExtension(IGameObject* pGameObject, const SEntitySpawnParams& params) override;
	virtual void PostReloadExtension(IGameObject* pGameObject, const SEntitySpawnParams& params) override;
	virtual bool GetEntityPoolSignature(TSerialize signature) override;
	virtual void Release() override;
	virtual void FullSerialize(TSerialize ser) override;
	virtual bool NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int pflags) override;
	//! This method is not needed because the same event goes through ProcessEvent method.
	virtual void PostSerialize() override {}
	//! Not used, we don't set the spawn serializer for Mono entities.
	virtual void SerializeSpawnInfo(TSerialize ser) override {}
	//! Not sure how to make it work and whether its need that much.
	virtual ISerializableInfoPtr GetSpawnInfo() override { return nullptr; }
	virtual void Update(SEntityUpdateContext& ctx, int updateSlot) override;
	virtual void SetChannelId(uint16 id) override;
	// Not sure what this really does, maybe will write some event for it later.
	virtual void SetAuthority(bool auth) override {}
	virtual void PostUpdate(float frameTime) override;
	virtual void PostRemoteSpawn() override {}
	virtual void GetMemoryUsage(ICrySizer* pSizer) const override {}
	virtual ComponentEventPriority GetEventPriority(const int eventID) const override;
	//~IGameObjectExtension

	const char  *GetPropertyValue(int index);
	const char  *SetPropertyValue(int index, int type, const char *value);
	bool         IsInitialized();
	mono::object GetManagedWrapper() { return this->objHandle.Object; }
	__declspec(property(get = GetManagedWrapper)) mono::object MonoWrapper;
	//! Encapsulates name of a method that should be invoked remotely, identifier of the entity that is a target of
	//! invocation and a set of arguments that will be passed to the method.
	struct CryCilRMIParameters
	{
	private:
		string methodName;
		IMonoObject arguments;
		EntityId target;
		string rmiDataType;
	public:
		//! Creates a new object that will receive the arguments for remote invocation from the network.
		CryCilRMIParameters();
		//! Creates a new object that will send the arguments for remote invocation across the network.
		CryCilRMIParameters(const char *methodName, mono::object args, EntityId target, const char *rmiDataType);
		//! Performs synchronization of arguments.
		void SerializeWith(TSerialize ser);
	};
};

//! Attempts to acquire an object that allows the entity to communicate with CryCIL.
//!
//! @returns A pointer to the object that handle entity<->CryCIL communication, or null pointer if this entity has no
//!          connection to CryCIL.
inline MonoEntityExtension *QueryMonoEntityExtension(IEntity *entity, EntityId id)
{
	if (IGameObject *gameObj = MonoEnv->CryAction->GetGameObject(id))
	{
		if (auto ext = gameObj->QueryExtension(entity->GetClass()->GetName()))
		{
			return static_cast<MonoEntityExtension *>(ext);
		}
	}

	return nullptr;
}
//! Attempts to acquire an object that allows the entity to communicate with CryCIL.
//!
//! @returns A pointer to the object that handle entity<->CryCIL communication, or null pointer if this entity has no
//!          connection to CryCIL.
inline MonoEntityExtension *QueryMonoEntityExtension(IEntity *entity)
{
	return QueryMonoEntityExtension(entity, entity->GetId());
}
//! Attempts to acquire an object that allows the entity to communicate with CryCIL.
//!
//! @returns A pointer to the object that handle entity<->CryCIL communication, or null pointer if this entity has no
//!          connection to CryCIL.
inline MonoEntityExtension *QueryMonoEntityExtension(EntityId id)
{
	return QueryMonoEntityExtension(gEnv->pEntitySystem->GetEntity(id), id);
}