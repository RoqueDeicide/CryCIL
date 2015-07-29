#pragma once

#include "IMonoInterface.h"

#include "IGameObject.h"

//! Serves as an abstraction layer between logic defined in C# and CryEngine.
struct MonoEntityExtension : CGameObjectExtensionHelper<MonoEntityExtension, IGameObjectExtension>
{
private:
	MonoGCHandle objHandle;
	bool networking;
	bool dontSyncProps;
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
	virtual void SetAuthority(bool auth) override;
	virtual void PostUpdate(float frameTime) override;
	virtual void PostRemoteSpawn() override {}
	virtual void GetMemoryUsage(ICrySizer* pSizer) const override {}
	virtual ComponentEventPriority GetEventPriority(const int eventID) const override;
	//~IGameObjectExtension

	const char  *GetPropertyValue(int index);
	void         SetPropertyValue(int index, const char *value);
	bool         IsInitialized();
	mono::object GetManagedWrapper() { return this->objHandle.Object; }
	__declspec(property(get = GetManagedWrapper)) mono::object MonoWrapper;
	//! Encapsulates name of a method that should be invoked remotely, identifier of the entity that is a target of
	//! invocation and a set of arguments that will be passed to the method.
	struct CryCilRMIParameters
	{
		friend MonoEntityExtension;
	private:
		string methodName;
		uint32 arguments;
		string rmiDataType;
	public:
		//! Creates a new object that will receive the arguments for remote invocation from the network.
		CryCilRMIParameters();
		//! Creates a new object that will send the arguments for remote invocation across the network.
		CryCilRMIParameters(const char *methodName, uint32 args, const char *rmiDataType);
		//! Performs synchronization of arguments.
		void SerializeWith(TSerialize ser);
	};

	DECLARE_SERVER_RMI_PREATTACH(svPreAttachCryCilRmi, CryCilRMIParameters);
	DECLARE_CLIENT_RMI_PREATTACH(clPreAttachCryCilRmi, CryCilRMIParameters);

	DECLARE_SERVER_RMI_POSTATTACH(svPostAttachCryCilRmi, CryCilRMIParameters);
	DECLARE_CLIENT_RMI_POSTATTACH(clPostAttachCryCilRmi, CryCilRMIParameters);

	DECLARE_SERVER_RMI_NOATTACH(svReliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);
	DECLARE_CLIENT_RMI_NOATTACH(clReliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);

	DECLARE_SERVER_RMI_NOATTACH(svUnreliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);
	DECLARE_CLIENT_RMI_NOATTACH(clUnreliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);

	DECLARE_SERVER_RMI_PREATTACH_FAST(svFastPreAttachCryCilRmi, CryCilRMIParameters);
	DECLARE_CLIENT_RMI_PREATTACH_FAST(clFastPreAttachCryCilRmi, CryCilRMIParameters);

	DECLARE_SERVER_RMI_POSTATTACH_FAST(svFastPostAttachCryCilRmi, CryCilRMIParameters);
	DECLARE_CLIENT_RMI_POSTATTACH_FAST(clFastPostAttachCryCilRmi, CryCilRMIParameters);

	DECLARE_SERVER_RMI_NOATTACH_FAST(svFastReliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);
	DECLARE_CLIENT_RMI_NOATTACH_FAST(clFastReliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);

	DECLARE_SERVER_RMI_URGENT(svReliableUrgentCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);
	DECLARE_CLIENT_RMI_URGENT(clReliableUrgentCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);

	DECLARE_SERVER_RMI_INDEPENDENT(svReliableIndependentCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);
	DECLARE_CLIENT_RMI_INDEPENDENT(clReliableIndependentCryCilRmi, CryCilRMIParameters, eNRT_ReliableUnordered);

	DECLARE_SERVER_RMI_NOATTACH_FAST(svFastUnreliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);
	DECLARE_CLIENT_RMI_NOATTACH_FAST(clFastUnreliableNoAttachCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);

	DECLARE_SERVER_RMI_URGENT(svUnreliableUrgentCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);
	DECLARE_CLIENT_RMI_URGENT(clUnreliableUrgentCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);

	DECLARE_SERVER_RMI_INDEPENDENT(svUnreliableIndependentCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);
	DECLARE_CLIENT_RMI_INDEPENDENT(clUnreliableIndependentCryCilRmi, CryCilRMIParameters, eNRT_UnreliableUnordered);

	//! Invokes RMI method that is specified by the params object.
	bool ReceiveRmiCall(CryCilRMIParameters *params);


private:
	template<const char *eventName>
	void raiseEntityEvent();
	template<const char *eventName, typename arg0Type>
	void raiseEntityEvent(arg0Type arg0);
	template<const char *eventName, typename arg0Type, typename arg1Type>
	void raiseEntityEvent(arg0Type arg0, arg1Type arg1);
	template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type>
	void raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2);
	template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type, typename arg3Type>
	void raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2, arg3Type arg3);
	template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type, typename arg3Type, typename arg4Type>
	void raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2, arg3Type arg3, arg4Type arg4);
	template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type, typename arg3Type, typename arg4Type, typename arg5Type>
	void raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2, arg3Type arg3, arg4Type arg4, arg5Type arg5);
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