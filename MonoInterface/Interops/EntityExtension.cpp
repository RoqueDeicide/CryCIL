#include "stdafx.h"

#include "EntityExtension.h"
#include "IComponent.h"
#include "EntityClass.h"
#include "MonoEntitySpawnParams.h"

IMonoClass *GetMonoEntityClass()
{
	return MonoEnv->Cryambly->GetClass("CryCil.Engine.Logic", "MonoEntity");
}
IMonoClass *GetMonoNetEntityClass()
{
	return MonoEnv->Cryambly->GetClass("CryCil.Engine.Logic", "MonoNetEntity");
}

MonoEntityExtension::MonoEntityExtension()
	: objHandle(-1)
	, networking(false)
{

}

typedef void(__stdcall *DisposeMonoEntityThunk)(mono::exception *);

MonoEntityExtension::~MonoEntityExtension()
{
	static DisposeMonoEntityThunk thunk = (DisposeMonoEntityThunk)
		GetMonoEntityClass()->GetFunction("DisposeInternal", -1)->UnmanagedThunk;

	if (!this->objHandle.IsValid)
	{
		return;
	}
	// Release the abstraction layer.
	mono::exception ex;
	thunk(&ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

typedef mono::object(__stdcall *CreateAbstractionLayerThunk)(mono::string, EntityId, IEntity *, mono::exception *);
typedef mono::object(__stdcall *RaiseOnInitThunk)(mono::object, mono::exception *);

bool MonoEntityExtension::Init(IGameObject* pGameObject)
{
	static CreateAbstractionLayerThunk create = (CreateAbstractionLayerThunk)
		GetMonoEntityClass()->GetFunction("CreateAbstractionLayer", -1)->UnmanagedThunk;
	static RaiseOnInitThunk raise = (RaiseOnInitThunk)
		GetMonoEntityClass()->GetEvent("Initializing")->GetRaise()->UnmanagedThunk;

	this->SetGameObject(pGameObject);

	IEntity *entity           = this->GetEntity();
	IEntityClass *entityClass = entity->GetClass();
	EntityId entityId         = entity->GetId();
	// Create the abstraction layer.
	mono::exception ex;
	mono::object obj = create(ToMonoString(entityClass->GetName()), entityId, entity, &ex);
	if (!obj)
	{
		return false;
	}
	this->objHandle = MonoEnv->GC->Keep(obj);

	this->networking = *(bool *)pGameObject->GetUserData();
	
	// Set all queued properties.
	auto propHandler = static_cast<MonoEntityPropertyHandler *>(entityClass->GetPropertyHandler());
	List<QueuedProperty> &queuedProps = propHandler->GetQueuedProperties()->At(entityId);
	for (int i = 0; i < queuedProps.Length; i++)
	{
		auto prop = queuedProps[i];
		this->SetPropertyValue(prop.index, prop.type, prop.value);
	}

	raise(this->objHandle.Object, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return false;
	}

	return this->networking ? pGameObject->BindToNetwork() : true;
}

void MonoEntityExtension::PostInit(IGameObject* pGameObject)
{
	static RaiseOnInitThunk raise = (RaiseOnInitThunk)
		GetMonoEntityClass()->GetEvent("Initialized")->GetRaise()->UnmanagedThunk;

	if (this->objHandle.IsValid)
	{
		mono::exception ex;
		raise(this->objHandle.Object, &ex);
	}
}

void MonoEntityExtension::ProcessEvent(SEntityEvent& event)
{

}

typedef void(__stdcall *ClientInitRaiseThunk)(mono::object, ushort, mono::exception *);

void MonoEntityExtension::InitClient(int channelId)
{
	ClientInitRaiseThunk thunk = (ClientInitRaiseThunk)
		GetMonoNetEntityClass()->GetEvent("ClientInitializing")->Raise->UnmanagedThunk;

	if (!this->networking)
	{
		return;
	}

	mono::exception ex;
	thunk(this->objHandle.Object, channelId, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void MonoEntityExtension::PostInitClient(int channelId)
{
	ClientInitRaiseThunk thunk = (ClientInitRaiseThunk)
		GetMonoNetEntityClass()->GetEvent("ClientInitialized")->Raise->UnmanagedThunk;

	if (!this->networking)
	{
		return;
	}

	mono::exception ex;
	thunk(this->objHandle.Object, channelId, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

typedef bool(__stdcall *ReloadEventThunk)(mono::object, MonoEntitySpawnParams *, mono::exception *);

bool MonoEntityExtension::ReloadExtension(IGameObject* pGameObject, const SEntitySpawnParams& params)
{
	static ReloadEventThunk thunk = (ReloadEventThunk)
		GetMonoEntityClass()->GetEvent("Reloading")->GetRaise()->UnmanagedThunk;

	if (mono::object obj = this->MonoWrapper)
	{
		MonoEntitySpawnParams parameters(params);
		mono::exception ex;
		bool success = thunk(obj, &parameters, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
			return false;
		}
		return success;
	}
	return false;
}

typedef void(__stdcall *ReloadedEventThunk)(mono::object, MonoEntitySpawnParams *, mono::exception *);

void MonoEntityExtension::PostReloadExtension(IGameObject* pGameObject, const SEntitySpawnParams& params)
{
	static ReloadedEventThunk thunk = (ReloadedEventThunk)
		GetMonoEntityClass()->GetEvent("Reloaded")->GetRaise()->UnmanagedThunk;

	if (mono::object obj = this->MonoWrapper)
	{
		MonoEntitySpawnParams parameters(params);
		mono::exception ex;
		thunk(obj, &parameters, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}
	}
}

typedef bool(__stdcall *GetSignatureThunk)(mono::object, ISerialize *, mono::exception *);

bool MonoEntityExtension::GetEntityPoolSignature(TSerialize signature)
{
	static GetSignatureThunk thunk = (GetSignatureThunk)
		GetMonoEntityClass()->GetFunction("GetSignature", -1)->UnmanagedThunk;

	if (mono::object obj = this->MonoWrapper)
	{
		mono::exception ex;
		bool result = thunk(obj, *(ISerialize **)&signature, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
			return false;
		}
		return result;
	}
	return false;
}

void MonoEntityExtension::Release()
{
	delete this;
}

typedef bool(__stdcall *SyncInternalThunk)(mono::object, ISerialize *, mono::exception *);

void MonoEntityExtension::FullSerialize(TSerialize ser)
{
	static SyncInternalThunk thunk = (SyncInternalThunk)
		GetMonoEntityClass()->GetFunction("SyncInternal", -1)->UnmanagedThunk;

	if (mono::object obj = this->MonoWrapper)
	{
		mono::exception ex;
		thunk(obj, *(ISerialize **)&ser, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}
	}
}

typedef bool(__stdcall *NetSyncInternalThunk)(mono::object, ISerialize *, EEntityAspects, byte, int, mono::exception *);

bool MonoEntityExtension::NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int pflags)
{
	if (!this->networking)
	{
		return true;
	}

	static NetSyncInternalThunk thunk = (NetSyncInternalThunk)
		GetMonoNetEntityClass()->GetFunction("NetSyncInternal", -1)->UnmanagedThunk;

	if (mono::object obj = this->MonoWrapper)
	{
		mono::exception ex;
		bool result = thunk(obj, *(ISerialize **)&ser, aspect, profile, pflags, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
			return false;
		}
		return result;
	}
	return false;
}

typedef void(__stdcall *UpdateEntityThunk)(mono::object, SEntityUpdateContext&, mono::exception *);

void MonoEntityExtension::Update(SEntityUpdateContext& ctx, int updateSlot)
{
	static UpdateEntityThunk update = (UpdateEntityThunk)
		GetMonoEntityClass()->GetFunction("UpdateInternal")->UnmanagedThunk;

	if (mono::object o = this->MonoWrapper)
	{
		mono::exception ex;
		update(o, ctx, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}
	}
}

void MonoEntityExtension::SetChannelId(uint16 id)
{
	if (!this->networking)
	{
		return;
	}

	static IMonoField *channelIdField = GetMonoNetEntityClass()->GetField("channerlId");

	if (mono::object obj = this->MonoWrapper)
	{
		channelIdField->Set(obj, &id);
	}
}

typedef void(__stdcall *PostUpdateEntityThunk)(mono::object, mono::exception *);

void MonoEntityExtension::PostUpdate(float frameTime)
{
	static PostUpdateEntityThunk update = (PostUpdateEntityThunk)
		GetMonoEntityClass()->GetFunction("PostUpdateInternal")->UnmanagedThunk;

	if (mono::object o = this->MonoWrapper)
	{
		mono::exception ex;
		update(o, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}
	}
}

typedef int ComponentEventPriority;

ComponentEventPriority MonoEntityExtension::GetEventPriority(const int eventID) const
{
	return EEntityEventPriority::EEntityEventPriority_GameObject;
}

IMonoClass *GetRmiParamsClass()
{
	return MonoEnv->Cryambly->GetClass("CryCil.Engine.Logic", "RmiParameters");
}

MonoEntityExtension::CryCilRMIParameters::CryCilRMIParameters()
	: methodName(nullptr)
	, arguments(-1)
	, rmiDataType(nullptr)
{}

MonoEntityExtension::CryCilRMIParameters::CryCilRMIParameters
(const char *methodName, uint32 args, const char *rmiDataType)
: methodName(methodName)
, arguments(args)
, rmiDataType(rmiDataType)
{}

typedef mono::object(__stdcall *AcquireArgumentsReceptorThunk)(mono::string, mono::exception *);

void MonoEntityExtension::CryCilRMIParameters::SerializeWith(TSerialize ser)
{
	static AcquireArgumentsReceptorThunk acquireReceptor = (AcquireArgumentsReceptorThunk)
		GetRmiParamsClass()->GetFunction("AcquireReceptor", 2)->UnmanagedThunk;

	// Synchronize the name and identifier of the target entity so we can identify the type of argument object on reception.
	ser.Value("method", this->methodName);
	ser.Value("rmiData", this->rmiDataType);

	if (this->rmiDataType.length() == 0)
	{
		// No arguments here.
		return;
	}

	if (this->arguments == -1)
	{
		// We are receiving the data, therefore we need to get the object that will hold received data.
		mono::exception ex;
		this->arguments = MonoEnv->GC->Keep(acquireReceptor(ToMonoString(this->rmiDataType.c_str()), &ex));
		if (ex)
		{
			MonoEnv->HandleException(ex);
			return;
		}
	}

	// Synchronize the arguments.
	void *param = *(ISerialize **)&ser;
	GetRmiParamsClass()->GetFunction("Synchronize", 1)->ToInstance()->Invoke
		(MonoEnv->GC->GetGCHandleTarget(this->arguments), &param, nullptr, true);

	if (ser.IsWriting())
	{
		// We won't need this anymore.
		MonoEnv->GC->ReleaseGCHandle(this->arguments);
	}
}

typedef bool(__stdcall *ReceiveRMICallThunk)(mono::object, mono::string, mono::object, mono::exception *);

bool MonoEntityExtension::ReceiveRmiCall(CryCilRMIParameters *params)
{
	static ReceiveRMICallThunk receiveCall = (ReceiveRMICallThunk)
		GetMonoNetEntityClass()->GetFunction("ReceiveRmi", -1)->UnmanagedThunk;

	mono::exception ex;
	bool success = receiveCall(this->objHandle.Object, ToMonoString(params->methodName),
							   MonoEnv->GC->GetGCHandleTarget(params->arguments), &ex);
	MonoEnv->GC->ReleaseGCHandle(params->arguments);			// We won't need this anymore.
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return false;
	}
	return success;
}

#define IMPLEMENT_CRYCIL_RMI(name) IMPLEMENT_RMI(MonoEntityExtension, name) \
{ \
	return this->ReceiveRmiCall(const_cast<Params_##name *>(&params)); \
}

IMPLEMENT_CRYCIL_RMI(svPreAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clPreAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svPostAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clPostAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svReliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clReliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svUnreliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clUnreliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svFastPreAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clFastPreAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svFastPostAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clFastPostAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svFastReliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clFastReliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svReliableUrgentCryCilRmi);
IMPLEMENT_CRYCIL_RMI(clReliableUrgentCryCilRmi);
IMPLEMENT_CRYCIL_RMI(svReliableIndependentCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clReliableIndependentCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svFastUnreliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clFastUnreliableNoAttachCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svUnreliableUrgentCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clUnreliableUrgentCryCilRmi)
IMPLEMENT_CRYCIL_RMI(svUnreliableIndependentCryCilRmi)
IMPLEMENT_CRYCIL_RMI(clUnreliableIndependentCryCilRmi)

#undef IMPLEMENT_CRYCIL_RMI