#include "stdafx.h"

#include "EntityExtension.h"
#include "IComponent.h"
#include "EntityClass.h"
#include "MonoEntitySpawnParams.h"

#include <CryCharAnimationParams.h>
#include "MonoAnimationEvent.h"
#include "EntityThunkDecls.h"

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
	, dontSyncProps(false)
{

}

MonoEntityExtension::~MonoEntityExtension()
{
	static DisposeMonoEntityThunk thunk =
		DisposeMonoEntityThunk(GetMonoEntityClass()->GetFunction("DisposeInternal", -1)->RawThunk);

	if (!this->objHandle.IsValid)
	{
		return;
	}
	// Release the abstraction layer.
	thunk();
}

bool MonoEntityExtension::Init(IGameObject* pGameObject)
{
	static CreateAbstractionLayerThunk create =
		CreateAbstractionLayerThunk(GetMonoEntityClass()->GetFunction("CreateAbstractionLayer", -1)->RawThunk);
	static RaiseOnInitThunk raise =
		RaiseOnInitThunk(GetMonoEntityClass()->GetEvent("Initializing")->GetRaise()->RawThunk);

	this->SetGameObject(pGameObject);

	IEntity *entity           = this->GetEntity();
	IEntityClass *entityClass = entity->GetClass();
	EntityId entityId         = entity->GetId();
	// Create the abstraction layer.
	mono::object obj = create(ToMonoString(entityClass->GetName()), entityId, entity);
	if (!obj)
	{
		return false;
	}
	this->objHandle = MonoEnv->GC->Keep(obj);

	auto userData = static_cast<MonoEntityClassUserData *>(pGameObject->GetUserData());
	this->dontSyncProps = userData->dontSyncProps;
	this->networking = userData->networked;
	
	// Set all queued properties.
	auto propHandler = static_cast<MonoEntityPropertyHandler *>(entityClass->GetPropertyHandler());
	List<QueuedProperty> &queuedProps = propHandler->GetQueuedProperties()->At(entityId);
	for (int i = 0; i < queuedProps.Length; i++)
	{
		auto prop = queuedProps[i];
		this->SetPropertyValue(prop.index, prop.value);
	}

	raise(this->objHandle.Object);

	return this->networking ? pGameObject->BindToNetwork() : true;
}

void MonoEntityExtension::PostInit(IGameObject*)
{
	static RaiseOnInitThunk raise =
		RaiseOnInitThunk(GetMonoEntityClass()->GetEvent("Initialized")->GetRaise()->RawThunk);

	if (this->objHandle.IsValid)
	{
		raise(this->objHandle.Object);
	}
}

#pragma region Entity Event Raisers
template<const char *eventName>
void MonoEntityExtension::raiseEntityEvent() const
{
	static void(*raise)(mono::object) =
		reinterpret_cast<void(*)(mono::object)>
		(GetMonoEntityClass()->GetEvent(eventName)->Raise->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		raise(obj);
	}
}
template<const char *eventName, typename arg0Type>
void MonoEntityExtension::raiseEntityEvent(arg0Type arg0) const
{
	static void(*raise)(mono::object, arg0Type) =
		reinterpret_cast<void(*)(mono::object, arg0Type)>
		(GetMonoEntityClass()->GetEvent(eventName)->Raise->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		raise(obj, arg0);
	}
}
template<const char *eventName, typename arg0Type, typename arg1Type>
void MonoEntityExtension::raiseEntityEvent(arg0Type arg0, arg1Type arg1) const
{
	static void(*raise)(mono::object, arg0Type, arg1Type) =
		reinterpret_cast<void(*)(mono::object, arg0Type, arg1Type)>
		(GetMonoEntityClass()->GetEvent(eventName)->Raise->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		raise(obj, arg0, arg1);
	}
}
template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type>
void MonoEntityExtension::raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2) const
{
	static void(*raise)(mono::object, arg0Type, arg1Type, arg2Type) =
		reinterpret_cast<void(*)(mono::object, arg0Type, arg1Type, arg2Type)>
		(GetMonoEntityClass()->GetEvent(eventName)->Raise->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		raise(obj, arg0, arg1, arg2);
	}
}
template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type, typename arg3Type>
void MonoEntityExtension::raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2, arg3Type arg3) const
{
	static void(*raise)(mono::object, arg0Type, arg1Type, arg2Type, arg3Type) =
		reinterpret_cast<void(*)(mono::object, arg0Type, arg1Type, arg2Type, arg3Type)>
		(GetMonoEntityClass()->GetEvent(eventName)->Raise->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		raise(obj, arg0, arg1, arg2, arg3);
	}
}
template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type, typename arg3Type, typename arg4Type>
void MonoEntityExtension::raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2, arg3Type arg3, arg4Type arg4) const
{
	static void(*raise)(mono::object, arg0Type, arg1Type, arg2Type, arg3Type, arg4Type) =
		reinterpret_cast<void(*)
		(mono::object, arg0Type, arg1Type, arg2Type, arg3Type, arg4Type)>
		(GetMonoEntityClass()->GetEvent(eventName)->Raise->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		raise(obj, arg0, arg1, arg2, arg3, arg4);
	}
}
template<const char *eventName, typename arg0Type, typename arg1Type, typename arg2Type, typename arg3Type, typename arg4Type, typename arg5Type>
void MonoEntityExtension::raiseEntityEvent(arg0Type arg0, arg1Type arg1, arg2Type arg2, arg3Type arg3, arg4Type arg4, arg5Type arg5) const
{
	static void(*raise)(mono::object, arg0Type, arg1Type, arg2Type, arg3Type, arg4Type, arg5Type) =
		reinterpret_cast<void(*)
		(mono::object, arg0Type, arg1Type, arg2Type, arg3Type, arg4Type, arg5Type)>
		(GetMonoEntityClass()->GetEvent(eventName)->Raise->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		raise(obj, arg0, arg1, arg2, arg3, arg4, arg5);
	}
}

#pragma endregion
#pragma region Entity Event Names
// Gotta have these global variables here, cause simple string literals are not compile-time constants even with string
// pooling option on.

#define define_entity_event_name(eventName) extern const char entityEventName##eventName[] = #eventName;

define_entity_event_name(Moved);
define_entity_event_name(MovedInEditor);
define_entity_event_name(TimedOut);
define_entity_event_name(Ressurected);
define_entity_event_name(Done);
define_entity_event_name(ReturningToPool);
define_entity_event_name(ResetInEditor);
define_entity_event_name(Attached);
define_entity_event_name(AttachedTo);
define_entity_event_name(Detached);
define_entity_event_name(DetachedFrom);
define_entity_event_name(Linked);
define_entity_event_name(Unlinked);
define_entity_event_name(Hidden);
define_entity_event_name(NotHidden);
define_entity_event_name(PhysicsEnabled);
define_entity_event_name(Awoken);
define_entity_event_name(AreaEntered);
define_entity_event_name(AreaLeft);
define_entity_event_name(Broken);
define_entity_event_name(NotSeen);
define_entity_event_name(Collided);
define_entity_event_name(Rendered);
define_entity_event_name(BeforePhysicsUpdate);
define_entity_event_name(LevelLoaded);
define_entity_event_name(LevelStarted);
define_entity_event_name(GameStarted);
define_entity_event_name(Synchronizing);
define_entity_event_name(Synchronized);
define_entity_event_name(BecameVisible);
define_entity_event_name(BecameInvisible);
define_entity_event_name(MaterialChanged);
define_entity_event_name(MaterialLayersChanged);
define_entity_event_name(Activated);
define_entity_event_name(Deactivated);
define_entity_event_name(AnimationEvent);

#undef define_entity_event_name

#pragma endregion

#define entity_event(eventName) entityEventName##eventName

void MonoEntityExtension::ProcessEvent(SEntityEvent& _event)
{
	auto _eventType = _event.event;
	switch (_eventType)
	{
	case ENTITY_EVENT_XFORM:
	{
		EEntityXFormFlags flags = EEntityXFormFlags(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(Moved), EEntityXFormFlags>(flags);
	}
		break;
	case ENTITY_EVENT_XFORM_FINISHED_EDITOR:
		this->raiseEntityEvent<entity_event(MovedInEditor)>();
		break;
	case ENTITY_EVENT_TIMER:
	{
		auto timerId = int(_event.nParam[0]);
		auto milliseconds = int(_event.nParam[1]);
		this->raiseEntityEvent<entity_event(TimedOut), int, int>(timerId, milliseconds);
	}
		break;
	case ENTITY_EVENT_INIT:
		this->raiseEntityEvent<entity_event(Ressurected)>();
		break;
	case ENTITY_EVENT_DONE:
		this->raiseEntityEvent<entity_event(Done)>();
		break;
	case ENTITY_EVENT_RETURNING_TO_POOL:
		this->raiseEntityEvent<entity_event(ReturningToPool)>();
		break;
	case ENTITY_EVENT_RESET:
	{
		auto enterGameMode = _event.nParam[0] != 0;
		this->raiseEntityEvent<entity_event(ResetInEditor), bool>(enterGameMode);
	}
		break;
	case ENTITY_EVENT_ATTACH:
	{
		auto entityId = EntityId(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(Attached), EntityId>(entityId);
	}
		break;
	case ENTITY_EVENT_ATTACH_THIS:
	{
		auto entityId = EntityId(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(AttachedTo), EntityId>(entityId);
	}
		break;
	case ENTITY_EVENT_DETACH:
	{
		auto entityId = EntityId(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(Detached), EntityId>(entityId);
	}
		break;
	case ENTITY_EVENT_DETACH_THIS:
	{
		auto entityId = EntityId(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(DetachedFrom), EntityId>(entityId);
	}
		break;
	case ENTITY_EVENT_LINK:
	{
		auto link = reinterpret_cast<IEntityLink *>(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(Linked), mono::string, EntityId, EntityGUID>
			(ToMonoString(link->name), link->entityId, link->entityGuid);
	}
		break;
	case ENTITY_EVENT_DELINK:
	{
		auto link = reinterpret_cast<IEntityLink *>(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(Unlinked), mono::string, EntityId, EntityGUID>
			(ToMonoString(link->name), link->entityId, link->entityGuid);
	}
		break;
	case ENTITY_EVENT_HIDE:
		this->raiseEntityEvent<entity_event(Hidden)>();
		break;
	case ENTITY_EVENT_UNHIDE:
		this->raiseEntityEvent<entity_event(NotHidden)>();
		break;
	case ENTITY_EVENT_ENABLE_PHYSICS:
	{
		auto enablePhysics = _event.nParam[0] != 0;
		this->raiseEntityEvent<entity_event(PhysicsEnabled), bool>(enablePhysics);
	}
		break;
	case ENTITY_EVENT_PHYSICS_CHANGE_STATE:
	{
		auto awake = _event.nParam[0] != 0;
		this->raiseEntityEvent<entity_event(Awoken), bool>(awake);
	}
		break;
	case ENTITY_EVENT_SCRIPT_EVENT:
		break;
	case ENTITY_EVENT_ENTERAREA:
	case ENTITY_EVENT_ENTERNEARAREA:
	{
		auto entityId = EntityId(_event.nParam[0]);
		auto areaId = int(_event.nParam[1]);
		auto areaEntityId = EntityId(_event.nParam[2]);
		auto fadeFactor = _event.fParam[0];
		this->raiseEntityEvent<entity_event(AreaEntered), EntityId, int, EntityId, float>
			(entityId, areaId, areaEntityId, fadeFactor);
	}
		break;
	case ENTITY_EVENT_LEAVEAREA:
	case ENTITY_EVENT_LEAVENEARAREA:
	{
		auto entityId = EntityId(_event.nParam[0]);
		auto areaId = int(_event.nParam[1]);
		auto areaEntityId = EntityId(_event.nParam[2]);
		auto fadeFactor = _event.fParam[0];
		this->raiseEntityEvent<entity_event(AreaLeft), EntityId, int, EntityId, float>
			(entityId, areaId, areaEntityId, fadeFactor);
	}
		break;
	case ENTITY_EVENT_MOVEINSIDEAREA:
		break;
	case ENTITY_EVENT_MOVENEARAREA:
		break;
	case ENTITY_EVENT_CROSS_AREA:
		break;
	case ENTITY_EVENT_PHYS_POSTSTEP:
		break;
	case ENTITY_EVENT_PHYS_BREAK:
		this->raiseEntityEvent<entity_event(Broken)>();
		break;
	case ENTITY_EVENT_AI_DONE:
		break;
	case ENTITY_EVENT_SOUND_DONE:
		break;
	case ENTITY_EVENT_NOT_SEEN_TIMEOUT:
		this->raiseEntityEvent<entity_event(NotSeen)>();
		break;
	case ENTITY_EVENT_COLLISION:
	{
		auto infoPointer = reinterpret_cast<void *>(_event.nParam[0]);
		auto isCollider = _event.nParam[1] != 0;
		this->raiseEntityEvent<entity_event(Collided), void *, bool>(infoPointer, isCollider);
	}
		break;
	case ENTITY_EVENT_RENDER:
	{
		auto infoPointer = reinterpret_cast<void *>(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(Rendered), void *>(infoPointer);
	}
		break;
	case ENTITY_EVENT_PREPHYSICSUPDATE:
	{
		auto frameTime = _event.fParam[0];
		this->raiseEntityEvent<entity_event(BeforePhysicsUpdate), float>(frameTime);
	}
		break;
	case ENTITY_EVENT_LEVEL_LOADED:
		this->raiseEntityEvent<entity_event(LevelLoaded)>();
		break;
	case ENTITY_EVENT_START_LEVEL:
		this->raiseEntityEvent<entity_event(LevelStarted)>();
		break;
	case ENTITY_EVENT_START_GAME:
		this->raiseEntityEvent<entity_event(GameStarted)>();
		break;
	case ENTITY_EVENT_ENTER_SCRIPT_STATE:
		break;
	case ENTITY_EVENT_LEAVE_SCRIPT_STATE:
		break;
	case ENTITY_EVENT_PRE_SERIALIZE:
		this->raiseEntityEvent<entity_event(Synchronizing)>();
		break;
	case ENTITY_EVENT_POST_SERIALIZE:
		this->raiseEntityEvent<entity_event(Synchronized)>();
		break;
	case ENTITY_EVENT_INVISIBLE:
		this->raiseEntityEvent<entity_event(BecameVisible)>();
		break;
	case ENTITY_EVENT_VISIBLE:
		this->raiseEntityEvent<entity_event(BecameInvisible)>();
		break;
	case ENTITY_EVENT_MATERIAL:
	{
		auto newMaterial = reinterpret_cast<void *>(_event.nParam[0]);
		this->raiseEntityEvent<entity_event(MaterialChanged), void *>(newMaterial);
	}
		break;
	case ENTITY_EVENT_MATERIAL_LAYER:
	{
		auto _new = byte(_event.nParam[0]);
		auto _old = byte(_event.nParam[1]);
		this->raiseEntityEvent<entity_event(MaterialLayersChanged), byte, byte>(_new, _old);
	}
		break;
	case ENTITY_EVENT_ONHIT:
		break;
	case ENTITY_EVENT_ANIM_EVENT:
	{
		auto animEvent = MonoAnimationEvent(reinterpret_cast<AnimEventInstance &>(_event.nParam[0]));
		auto character = reinterpret_cast<ICharacterInstance *>(_event.nParam[1]);
		this->raiseEntityEvent<entity_event(MaterialLayersChanged), MonoAnimationEvent, ICharacterInstance *>
			(animEvent, character);
	}
		break;
	case ENTITY_EVENT_SCRIPT_REQUEST_COLLIDERMODE:
		break;
	case ENTITY_EVENT_ACTIVATE_FLOW_NODE_OUTPUT:
		break;
	case ENTITY_EVENT_EDITOR_PROPERTY_CHANGED:
		break;
	case ENTITY_EVENT_RELOAD_SCRIPT:
		break;
	case ENTITY_EVENT_ACTIVATED:
		this->raiseEntityEvent<entity_event(Activated)>();
		break;
	case ENTITY_EVENT_DEACTIVATED:
		this->raiseEntityEvent<entity_event(Deactivated)>();
		break;
	case ENTITY_EVENT_LAST:
		break;
	default:
		break;
	}
}

#undef entity_event

void MonoEntityExtension::InitClient(int channelId)
{
	static ClientInitRaiseThunk thunk =
		ClientInitRaiseThunk(GetMonoNetEntityClass()->GetEvent("ClientInitializing")->Raise->RawThunk);

	if (!this->networking)
	{
		return;
	}

	thunk(this->objHandle.Object, channelId);
}

void MonoEntityExtension::PostInitClient(int channelId)
{
	static ClientInitRaiseThunk thunk =
		ClientInitRaiseThunk(GetMonoNetEntityClass()->GetEvent("ClientInitialized")->Raise->RawThunk);

	if (!this->networking)
	{
		return;
	}

	thunk(this->objHandle.Object, channelId);
}

bool MonoEntityExtension::ReloadExtension(IGameObject*, const SEntitySpawnParams& params)
{
	static ReloadEventThunk thunk =
		ReloadEventThunk(GetMonoEntityClass()->GetEvent("Reloading")->GetRaise()->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		MonoEntitySpawnParams parameters(params);
		return thunk(obj, &parameters);
	}
	return false;
}

void MonoEntityExtension::PostReloadExtension(IGameObject*, const SEntitySpawnParams& params)
{
	static ReloadedEventThunk thunk =
		ReloadedEventThunk(GetMonoEntityClass()->GetEvent("Reloaded")->GetRaise()->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		MonoEntitySpawnParams parameters(params);
		thunk(obj, &parameters);
	}
}

bool MonoEntityExtension::GetEntityPoolSignature(TSerialize signature)
{
	static GetSignatureThunk thunk =
		GetSignatureThunk(GetMonoEntityClass()->GetFunction("GetSignature", -1)->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		return thunk(obj, *reinterpret_cast<ISerialize **>(&signature));
	}
	return false;
}

void MonoEntityExtension::Release()
{
	delete this;
}

void MonoEntityExtension::FullSerialize(TSerialize ser)
{
	static SyncInternalThunk thunk =
		SyncInternalThunk(GetMonoEntityClass()->GetFunction("SyncInternal", -1)->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		ser.BeginGroup("AbstractionLayer");

		thunk(obj, *reinterpret_cast<ISerialize **>(&ser));

		ser.EndGroup();
	}

	if (this->dontSyncProps)
	{
		return;
	}

	ser.BeginGroup("Properties");

	auto entity = this->GetEntity();
	auto propHandler = entity->GetClass()->GetPropertyHandler();

	int propCount = propHandler->GetPropertyCount();

	for (int i = 0; i < propCount; i++)
	{
		IEntityPropertyHandler::SPropertyInfo propInfo;
		propHandler->GetPropertyInfo(i, propInfo);
		string currentValue;
		if (ser.IsWriting())
		{
			currentValue = propHandler->GetProperty(entity, i);
		}
		ser.Value(propInfo.name, currentValue);
		if (ser.IsReading())
		{
			propHandler->SetProperty(entity, i, currentValue.c_str());
		}
	}

	ser.EndGroup();
}

bool MonoEntityExtension::NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int flags)
{
	if (!this->networking)
	{
		return true;
	}

	static NetSyncInternalThunk thunk =
		NetSyncInternalThunk(GetMonoNetEntityClass()->GetFunction("NetSyncInternal", -1)->RawThunk);

	if (mono::object obj = this->MonoWrapper)
	{
		return thunk(obj, *reinterpret_cast<ISerialize **>(&ser), aspect, profile, flags);
	}
	return false;
}

void MonoEntityExtension::Update(SEntityUpdateContext& ctx, int)
{
	static UpdateEntityThunk update =
		UpdateEntityThunk(GetMonoEntityClass()->GetFunction("UpdateInternal")->RawThunk);

	if (mono::object o = this->MonoWrapper)
	{
		update(o, ctx);
	}
}

void MonoEntityExtension::SetChannelId(uint16 id)
{
	if (!this->networking)
	{
		return;
	}

	static const IMonoField *channelIdField = GetMonoNetEntityClass()->GetField("channelId");

	if (mono::object obj = this->MonoWrapper)
	{
		channelIdField->Set(obj, &id);
	}
}

void MonoEntityExtension::SetAuthority(bool auth)
{
	static OnAuthorizedEntityThunk update =
		OnAuthorizedEntityThunk(GetMonoEntityClass()->GetEvent("Authorized")->Raise->RawThunk);

	if (mono::object o = this->MonoWrapper)
	{
		update(o, auth);
	}
}

void MonoEntityExtension::PostUpdate(float)
{
	static PostUpdateEntityThunk update =
		PostUpdateEntityThunk(GetMonoEntityClass()->GetFunction("PostUpdateInternal")->RawThunk);

	if (mono::object o = this->MonoWrapper)
	{
		update(o);
	}
}

typedef int ComponentEventPriority;

ComponentEventPriority MonoEntityExtension::GetEventPriority(const int) const
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

typedef mono::object(*AcquireArgumentsReceptorThunk)(mono::string);

void MonoEntityExtension::CryCilRMIParameters::SerializeWith(TSerialize ser)
{
	static AcquireArgumentsReceptorThunk acquireReceptor =
		AcquireArgumentsReceptorThunk(GetRmiParamsClass()->GetFunction("AcquireReceptor", 2)->RawThunk);

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
		this->arguments = MonoEnv->GC->Keep(acquireReceptor(ToMonoString(this->rmiDataType.c_str())));
	}

	// Synchronize the arguments.
	void *param = *reinterpret_cast<ISerialize **>(&ser);
	GetRmiParamsClass()->GetFunction("Synchronize", 1)->ToInstance()->Invoke
		(MonoEnv->GC->GetGCHandleTarget(this->arguments), &param, nullptr, true);

	if (ser.IsWriting())
	{
		// We won't need this anymore.
		MonoEnv->GC->ReleaseGCHandle(this->arguments);
	}
}

typedef bool(*ReceiveRMICallThunk)(mono::object, mono::string, mono::object);

bool MonoEntityExtension::ReceiveRmiCall(CryCilRMIParameters *params) const
{
	static ReceiveRMICallThunk receiveCall =
		ReceiveRMICallThunk(GetMonoNetEntityClass()->GetFunction("ReceiveRmi", -1)->RawThunk);

	bool success = receiveCall(this->objHandle.Object, ToMonoString(params->methodName),
							   MonoEnv->GC->GetGCHandleTarget(params->arguments));
	MonoEnv->GC->ReleaseGCHandle(params->arguments);			// We won't need this anymore.
	return success;
}

typedef mono::string(*GetEntityPropertyValueThunk)(mono::object, int);

const char *MonoEntityExtension::GetPropertyValue(int index) const
{
	static GetEntityPropertyValueThunk get =
		GetEntityPropertyValueThunk(GetMonoEntityClass()->GetFunction("GetEditableProperty", -1)->RawThunk);

	return ToNativeString(get(this->MonoWrapper, index));
}

typedef void(*SetEntityPropertyValueThunk)(mono::object, int, mono::string);

void MonoEntityExtension::SetPropertyValue(int index, const char *value) const
{
	static SetEntityPropertyValueThunk set =
		SetEntityPropertyValueThunk(GetMonoEntityClass()->GetFunction("SetEditableProperty", -1)->RawThunk);

	set(this->MonoWrapper, index, ToMonoString(value));
}

bool MonoEntityExtension::IsInitialized() const
{
	return this->objHandle.IsValid;
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