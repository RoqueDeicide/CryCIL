#pragma once

#include "StdAfx.h"
// CryCommon includes.
#include <IGameObject.h>
#include <IEntityClass.h>
// CryMono specific includes.
#include <MonoRunTime.h>
#include <MonoEntityPropertyHandler.h>
#include <MonoObject.h>

//! @brief Represents a class that binds CryMono entities to CryEngine.
//! 
//! This class mostly just relays all function calls to its CryMono counter-part.
class ManagedEntity : public CGameObjectExtensionHelper < ManagedEntity, IGameObjectExtension >
{
private:
	mono::object managedWrapper;		//!< Managed wrapper of this entity.

	bool initialized;					//!< Indicates whether this entity is initialized.
	bool disposed;						//!< Indicates whether this entity is no longer in use and is set for destruction.
public:
	//! Creates a new instance of this class.
	ManagedEntity() :
		managedWrapper(nullptr),
		initialized(false),
		disposed(false)
	{
		// Not much can be done at this point, because we don't know,
		// which managed class will represent this entity in CryMono.
	}
	//! Disposes of this entity.
	~ManagedEntity()
	{
		if (this->disposed)
		{
			return;
		}
		if (this->managedWrapper)
		{
			// Let the wrapper object release all of the resources.
			this->managedWrapper->ToWrapper()->CallMethod("DisposeInternal");
		}
		this->disposed = true;
	}

	//! Performs preliminary initialization of this object.
	//!
	//! @remark Game object is not fully initialized at this point.
	//!
	//! @param pGameObject Game object that provides access to the CryEngine
	//!					   entity that the CryMono wrapper will manage.
	//!
	//! @result Indication whether initialization of this entity was successful.
	virtual bool Init(IGameObject * pGameObject)
	{
		// Let the underlying extension know, which game object it will work with.
		this->SetGameObject(pGameObject);
		// This is the place where original code had code that enable
		// immediate PostStep physics event and binds this entity to network.

		// This code does not have that, since that should be done on behave of the managed wrapper.

		// Get some data to use to make a wrapper.
		IEntity *entity = GetEntity();					// Hosting entity.
		IEntityClass *entityClass = entity->GetClass();	// Description of entity editor properties.
		EntityId entityId = entity->GetId();			// Hosting entity's identifier.
		// Create the wrapper of appropriate type.
		this->managedWrapper =
			GetMonoRunTime()->CryBrary->
			GetClass("Entity", "CryEngine.Logic")->
			CallMethod("CreateWrapper", ToMonoString(entityClass->GetName()), entity, entityId);
		if (!this->managedWrapper)
		{
			return false;			// Something when wrong.
		}
		// Set the properties.
		int propertyCount;
		auto properties =
			static_cast<CEntityPropertyHandler *>(entityClass->GetPropertyHandler())->
			GetQueuedProperties(entityId, propertyCount);
		for (int i = 0; i < propertyCount; i++)
		{
			auto queuedProperty = properties[i];

			SetPropertyValue(queuedProperty.propertyInfo, queuedProperty.value.c_str());
		}
		// Done with the first stage.
		this->initialized = true;
		// Let the managed wrapper know about completion of the first stage.
		this->managedWrapper->ToWrapper()->CallMethod("OnPreInitializedInternal");
		return true;
	}
	//! Completes initialization of this object.
	//!
	//! @remark Game object is fully initialized at this point.
	//!
	//! @param pGameObject Game object that provides access to the CryEngine
	//!					   entity that the CryMono wrapper will manage.
	virtual void PostInit(IGameObject * pGameObject)
	{
		// Notify the wrapper that initialization is complete.
		this->managedWrapper->ToWrapper()->CallMethod("OnPostInitializedInternal");
	}
	//! Begins initialization of client-side specific resources.
	//!
	//! @param channelId Identifier of the client's channel, that allows to access
	//!                  properties of a specific client and use them for initialization.
	virtual void InitClient(int channelId)
	{
		// Notify the wrapper that initialization is beginning.
		this->managedWrapper->ToWrapper()->CallMethod("OnClientPreInitializedInternal", channelId);
	}
	//! Completes initialization of client-side specific resources.
	//!
	//! @param channelId Identifier of the client's channel, that allows to access
	//!                  properties of a specific client and use them for initialization.
	virtual void PostInitClient(int channelId)
	{
		// Notify the wrapper that initialization is complete.
		this->managedWrapper->ToWrapper()->CallMethod("OnClientPostInitializedInternal", channelId);
	}
	//! Starts reloading of the extension.
	//!
	//! @param pGameObject Game object that owns this entity.
	//! @param params A set of parameters that specifies how to respawn this entity.
	//!
	//! @result Indication whether respawning of this entity is allowed.
	virtual bool ReloadExtension(IGameObject * pGameObject, const SEntitySpawnParams &params)
	{
		// Reset game object.
		this->ResetGameObject();
		// Notify the wrapper about the reload.
		SEntitySpawnParams pars = params;
		IMonoArray *args = CreateMonoArray(1);
		args->Insert(GetMonoRunTime()->CryBrary
			->GetClass("EntitySpawnParameters", "CryEngine.Logic")
			->BoxObject(&pars));
		IMonoObject *result =
			*this->managedWrapper->ToWrapper()->GetClass()
				->GetMethod("ReloadInternal")
				->InvokeArray(this->managedWrapper, args);
		args->Release(false);
		return result->Unbox<bool>();
	}
	//! Finishes reloading of the extension.
	//!
	//! @param pGameObject Game object that owns this entity.
	//! @param params A set of parameters that specifies how to respawn this entity.
	virtual void PostReloadExtension(IGameObject * pGameObject, const SEntitySpawnParams &params)
	{
		// Notify the wrapper about the reload completion.
		SEntitySpawnParams pars = params;
		IMonoArray *args = CreateMonoArray(1);
		args->Insert(GetMonoRunTime()->CryBrary
			->GetClass("EntitySpawnParameters", "CryEngine.Logic")
			->BoxObject(&pars));
		this->managedWrapper->ToWrapper()->GetClass()
			->GetMethod("PostReloadInternal")
			->InvokeArray(this->managedWrapper, args);
		args->Release(false);
	}
	//! Can't say much about this one.
	virtual bool GetEntityPoolSignature(TSerialize signature) { return true; }
	//! Destroys this entity.
	virtual void Release()
	{
		delete this;
	}
	//! Synchronizes this entity.
	virtual void FullSerialize(TSerialize ser)
	{
		if (this->disposed)
		{
			return;
		}
		IEntity *pEntity = GetEntity();
		// Serialize the properties.
		ser.BeginGroup("Properties");
		auto pPropertyHandler = static_cast<CEntityPropertyHandler *>(pEntity->GetClass()->GetPropertyHandler());
		for (int i = 0; i < pPropertyHandler->GetPropertyCount(); i++)
		{
			if (ser.IsWriting())
			{
				IEntityPropertyHandler::SPropertyInfo propertyInfo;
				pPropertyHandler->GetPropertyInfo(i, propertyInfo);

				ser.Value(propertyInfo.name, pPropertyHandler->GetProperty(pEntity, i));
			}
			else
			{
				IEntityPropertyHandler::SPropertyInfo propertyInfo;
				pPropertyHandler->GetPropertyInfo(i, propertyInfo);

				char *propertyValue = nullptr;
				ser.ValueChar(propertyInfo.name, propertyValue, 0);

				pPropertyHandler->SetProperty(pEntity, i, propertyValue);
			}
		}
		ser.EndGroup();				// Properties.
		ser.BeginGroup("ManagedEntity");
		// Tell the managed wrapper to synchronize whatever it needs.
		this->managedWrapper->ToWrapper()->CallMethod("FullSerializeInternal", (void *)&ser);
		ser.EndGroup();				// ManagedEntity.
	}
	//! Synchronizes this entity over network.
	virtual bool NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int flags)
	{
		if (this->disposed)
		{
			return;
		}
		ser.BeginGroup("ManagedEntity");
		// Tell the managed wrapper to synchronize whatever it needs.
		this->managedWrapper->ToWrapper()->CallMethod("NetSerializeInternal", (void *)&ser, aspect, profile, flags);
		ser.EndGroup();				// ManagedEntity.
		return true;
	}
	virtual NetworkAspectType GetNetSerializeAspects() { return eEA_All; }
	virtual void PostSerialize() {}
	virtual void SerializeSpawnInfo(TSerialize ser) {}
	virtual ISerializableInfoPtr GetSpawnInfo() { return 0; }
	//! Updates logical state of the entity.
	//!
	//! @param ctx Data that describes context of the update of this entity.
	//! @param updateSlot Identifier of the entity's slot that has to be updated.
	virtual void Update(SEntityUpdateContext &ctx, int updateSlot)
	{
		if (this->disposed)
		{
			return;
		}
		// Notify the wrapper about the update.
		SEntityUpdateContext context = ctx;

		IMonoArray *args = CreateMonoArray(1);
		args->InsertMonoObject(GetMonoRunTime()->CryBrary
			->GetClass("EntityUpdateContext", "CryEngine.Logic")
			->BoxObject(&context));
		args->Insert(updateSlot);

		this->managedWrapper->ToWrapper()->GetClass()
			->GetMethod("UpdateInternal")
			->InvokeArray(this->managedWrapper, args);

		args->Release(false);
	}
	virtual void HandleEvent(const SGameObjectEvent& event) {}
	//! Processes the event.
	//!
	//! @param event Encapsulates data specific for the event.
	virtual void ProcessEvent(SEntityEvent& entityEvent)
	{
		if (this->disposed)
		{
			return;
		}
		if (entityEvent.event == ENTITY_EVENT_RESET)
		{
			// Delete this entity, if it should not be saved, and the player has just went out of game mode.
			if (!entityEvent.nParam[0] && this->GetEntity()->GetFlags() & ENTITY_FLAG_NO_SAVE)
			{
				this->managedWrapper->ToWrapper()->CallMethod("DisposeInternal");
				gEnv->pEntitySystem->RemoveEntity(this->GetEntityId());
				return;
			}
		}
		if (this->managedWrapper)
		{
			// Let the wrapper process the event first.
			int eventId = entityEvent.event;
			INT_PTR nParam0 = entityEvent.nParam[0];
			INT_PTR nParam1 = entityEvent.nParam[1];
			INT_PTR nParam2 = entityEvent.nParam[2];
			INT_PTR nParam3 = entityEvent.nParam[3];
			float fParam0 = entityEvent.fParam[0];
			float fParam1 = entityEvent.fParam[1];
			this->managedWrapper->ToWrapper()
				->CallMethod
				<int, INT_PTR, INT_PTR, INT_PTR, INT_PTR, float, float>
				("ProcessEntityEventInternal", eventId, nParam0,
				nParam1, nParam2, nParam3, fParam0, fParam1);
		}
		// Mark this entity as disposed, if the entity is about to be removed from the system.
		
		// All disposal on managed side should already be done.
		this->disposed = entityEvent.event == ENTITY_EVENT_DONE;
	}
	virtual void SetChannelId(uint16 id) {}
	virtual void SetAuthority(bool auth) {}
	virtual void GetMemoryUsage(ICrySizer *pSizer) const {}
	virtual void PostUpdate(float frameTime)
	{
		if (!this->disposed && this->managedWrapper)
		{
			this->managedWrapper->ToWrapper()->CallMethod("PostUpdateInternal", frameTime);
		}
	}
	virtual void PostRemoteSpawn()
	{
		if (!this->disposed && this->managedWrapper)
		{
			this->managedWrapper->ToWrapper()->CallMethod("PostRemoteSpawnInternal");
		}
	}

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

private:
	void SetPropertyValue(IEntityPropertyHandler::SPropertyInfo propertyInfo, const char *value)
	{
		if (value != nullptr)
			this->managedWrapper->ToWrapper()->CallMethod("SetPropertyValue", propertyInfo.name, propertyInfo.type, value);
	}
};

///////////////////////////////////////////////////
// Entity RMI's
///////////////////////////////////////////////////
CMonoEntityExtension::RMIParams::RMIParams(mono::object _args, const char *funcName, EntityId target)
	: methodName(funcName)
	, targetId(target)
	, args(_args)
{}

void CMonoEntityExtension::RMIParams::SerializeWith(TSerialize ser)
{
	IMonoArray *pArgs;
	int length;

	if (this->args != nullptr)
	{
		pArgs = GetMonoRunTime()->ToArray(this->args);
		length = pArgs->GetSize();
	}
	else
		length = 0;

	ser.Value("length", length);

	ser.Value("methodName", methodName);
	ser.Value("targetId", targetId, 'eid');

	if (length > 0)
	{
		if (ser.IsWriting())
		{
			for (int i = 0; i < length; i++)
			{
				IMonoObject *pItem = *pArgs->GetItem(i);
				pItem->GetAnyValue().SerializeWith(ser);
				SAFE_RELEASE(pItem);
			}
		}
		else
		{
			pArgs = CreateMonoArray(length);

			for (int i = 0; i < length; i++)
			{
				MonoAnyValue value;
				value.SerializeWith(ser);
				pArgs->InsertAny(value, i);
			}

			args = pArgs->GetManagedObject();
		}

		pArgs->Release();
	}
}

IMPLEMENT_RMI(CMonoEntityExtension, SvScriptRMI)
{
	IMonoClass *pEntityClass = GetMonoRunTime()->CryBrary->GetClass("Entity", "CryEngine.Logic");

	IMonoArray *pNetworkArgs = CreateMonoArray(3);
	pNetworkArgs->Insert(ToMonoString(params.methodName.c_str()));
	pNetworkArgs->InsertMonoObject(params.args);
	pNetworkArgs->Insert(params.targetId);

	pEntityClass->GetMethod("OnRemoteInvocation", 3)->InvokeArray(nullptr, pNetworkArgs);
	pNetworkArgs->Release();

	return true;
}

IMPLEMENT_RMI(CMonoEntityExtension, ClScriptRMI)
{
	IMonoClass *pEntityClass = GetMonoRunTime()->CryBrary->GetClass("Entity", "CryEngine.Logic");

	IMonoArray *pNetworkArgs = CreateMonoArray(3);
	pNetworkArgs->Insert(ToMonoString(params.methodName.c_str()));
	pNetworkArgs->InsertMonoObject(params.args);
	pNetworkArgs->Insert(params.targetId);

	pEntityClass->GetMethod("OnRemoteInvocation", 3)->InvokeArray(nullptr, pNetworkArgs);
	pNetworkArgs->Release();

	return true;
}