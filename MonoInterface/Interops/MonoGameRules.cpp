#include "stdafx.h"

#include "MonoGameRules.h"

#pragma region Helpers
#include "MonoEntitySpawnParams.h"
#include "GameCollisionInfo.h"
#include "EntityThunkDecls.h"

IMonoClass *GetGameRulesClass()
{
	return MonoEnv->Cryambly->GetClass("CryCil.Engine.Logic", "GameRules");
}

template<typename ThunkType>
ThunkType GetGameRulesThunk(const char *funcName)
{
	ThunkType thunk = GetGameRulesClass()->GetFunction(funcName, -1)->UnmanagedThunk;

	return thunk;
}

template<const char *methodName, typename ResultType, typename TArg0, typename TArg1, typename TArg2, typename TArg3, typename TArg4>
ResultType MonoGameRules::CallFunc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) const
{
	static ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, arg4, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<const char *methodName, typename ResultType, typename TArg0, typename TArg1, typename TArg2, typename TArg3>
ResultType MonoGameRules::CallFunc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3) const
{
	static ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<const char *methodName, typename ResultType, typename TArg0, typename TArg1, typename TArg2>
ResultType MonoGameRules::CallFunc(TArg0 arg0, TArg1 arg1, TArg2 arg2) const
{
	static ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, TArg2, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, arg2, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<const char *methodName, typename ResultType, typename TArg0, typename TArg1>
ResultType MonoGameRules::CallFunc(TArg0 arg0, TArg1 arg1) const
{
	static ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<const char *methodName, typename ResultType, typename TArg0>
ResultType MonoGameRules::CallFunc(TArg0 arg0) const
{
	static ResultType(__stdcall *thunk)(mono::object, TArg0, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<const char *methodName, typename ResultType>
ResultType MonoGameRules::CallFunc() const
{
	static ResultType(__stdcall *thunk)(mono::object, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}


template<const char *methodName, typename TArg0, typename TArg1, typename TArg2, typename TArg3, typename TArg4>
void MonoGameRules::CallProc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) const
{
	static void(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, arg4, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<const char *methodName, typename TArg0, typename TArg1, typename TArg2, typename TArg3>
void MonoGameRules::CallProc(TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3) const
{
	static void(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<const char *methodName, typename TArg0, typename TArg1, typename TArg2>
void MonoGameRules::CallProc(TArg0 arg0, TArg1 arg1, TArg2 arg2) const
{
	static void(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, TArg2, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, arg2, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<const char *methodName, typename TArg0, typename TArg1>
void MonoGameRules::CallProc(TArg0 arg0, TArg1 arg1) const
{
	static void(__stdcall *thunk)(mono::object, TArg0, TArg1, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<const char *methodName, typename TArg0>
void MonoGameRules::CallProc(TArg0 arg0) const
{
	static void(__stdcall *thunk)(mono::object, TArg0, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<const char *methodName>
void MonoGameRules::CallProc() const
{
	static void(__stdcall *thunk)(mono::object, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, mono::exception *)>
		(GetGameRulesClass()->GetFunction(methodName, -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

#pragma endregion

MonoGameRules::MonoGameRules()
	: objHandle(-1)
{
	MonoEnv->CryAction->GetIGameRulesSystem()->SetCurrentGameRules(this);
}

MonoGameRules::~MonoGameRules()
{
	static DisposeMonoEntityThunk thunk =
		DisposeMonoEntityThunk(GetGameRulesClass()->GetFunction("DisposeInternal", -1)->UnmanagedThunk);

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

	MonoEnv->CryAction->GetIGameRulesSystem()->SetCurrentGameRules(nullptr);
}
#define define_game_rules_method_name(methodName) extern const char gameRulesMethodName##methodName[] = #methodName ## "Internal"

define_game_rules_method_name(ShouldKeepClient);
define_game_rules_method_name(PrecacheLevel);
define_game_rules_method_name(OnConnect);
define_game_rules_method_name(OnDisconnect);
define_game_rules_method_name(OnClientConnect);
define_game_rules_method_name(OnClientDisconnect);
define_game_rules_method_name(OnClientEnteredGame);
define_game_rules_method_name(OnEntitySpawn);
define_game_rules_method_name(OnEntityRemoved);
define_game_rules_method_name(OnEntityReused);
define_game_rules_method_name(SendTextMessage);
define_game_rules_method_name(SendChatMessage);
define_game_rules_method_name(OnCollision);
define_game_rules_method_name(ShowStatus);
define_game_rules_method_name(IsTimeLimited);
define_game_rules_method_name(GetRemainingGameTime);
define_game_rules_method_name(SetRemainingGameTime);

#undef define_game_rules_method_name

#define define_game_rules_name(eventName) extern const char gameRulesEventName##eventName[] = "On" ## #eventName;

define_game_rules_name(ResetInEditor);
define_game_rules_name(GameStarted);
define_game_rules_name(Synchronizing);
define_game_rules_name(Synchronized);

#undef define_entity_event_name

#define method(name) gameRulesMethodName##name
#define game_rules_event(eventName) gameRulesEventName##eventName

bool MonoGameRules::ShouldKeepClient(int channelId, EDisconnectionCause cause, const char *desc) const
{
	return this->CallFunc<method(ShouldKeepClient), bool, int, EDisconnectionCause, mono::string>
		(channelId, cause, ToMonoString(desc));
}

void MonoGameRules::PrecacheLevel()
{
	this->CallProc<method(PrecacheLevel)>();
}

void MonoGameRules::OnConnect(struct INetChannel *pNetChannel)
{
	this->CallProc<method(OnConnect), INetChannel *>(pNetChannel);
}

void MonoGameRules::OnDisconnect(EDisconnectionCause cause, const char *desc)
{
	this->CallProc<method(OnDisconnect), EDisconnectionCause, mono::string>(cause, ToMonoString(desc));
}

bool MonoGameRules::OnClientConnect(int channelId, bool isReset)
{
	return this->CallFunc<method(OnClientConnect), bool, int, bool>(channelId, isReset);
}

void MonoGameRules::OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc,
									   bool keepClient)
{
	this->CallProc<method(OnClientDisconnect), int, EDisconnectionCause, mono::string, bool>
		(channelId, cause, ToMonoString(desc), keepClient);
}

bool MonoGameRules::OnClientEnteredGame(int channelId, bool isReset)
{
	return this->CallFunc<method(OnClientEnteredGame), bool, int, bool>(channelId, isReset);
}

void MonoGameRules::OnEntitySpawn(IEntity *pEntity)
{
	this->CallProc<method(OnEntitySpawn), IEntity *>(pEntity);
}

void MonoGameRules::OnEntityRemoved(IEntity *pEntity)
{
	this->CallProc<method(OnEntityRemoved), IEntity *>(pEntity);
}

void MonoGameRules::OnEntityReused(IEntity *pEntity, SEntitySpawnParams &params, EntityId prevId)
{
	MonoEntitySpawnParams parameters(params);
	this->CallProc<method(OnEntityReused), IEntity *, MonoEntitySpawnParams &, EntityId>(pEntity, parameters,
																						 prevId);
}

void MonoGameRules::SendTextMessage(ETextMessageType type, const char *msg, uint32 to, int channelId,
									const char *, const char *, const char *, const char *)
{
	this->CallProc<method(SendTextMessage), ETextMessageType, mono::string, uint32, int>(type, ToMonoString(msg),
																						 to, channelId);
}

void MonoGameRules::SendChatMessage(EChatMessageType type, EntityId sourceId, EntityId targetId, const char *msg)
{
	this->CallProc<method(SendChatMessage), EChatMessageType, EntityId, EntityId, mono::string>(type, sourceId,
																	   targetId, ToMonoString(msg));
}

bool MonoGameRules::OnCollision(const SGameCollision& _event)
{
	GameCollisionInfo info(_event);
	return this->CallFunc<method(OnCollision), bool, GameCollisionInfo &>(info);
}

void MonoGameRules::ShowStatus()
{
	this->CallProc<method(ShowStatus)>();
}

bool MonoGameRules::IsTimeLimited() const
{
	return this->CallFunc<method(IsTimeLimited), bool>();
}

float MonoGameRules::GetRemainingGameTime() const
{
	return this->CallFunc<method(GetRemainingGameTime), float>();
}

void MonoGameRules::SetRemainingGameTime(float seconds)
{
	this->CallProc<method(SetRemainingGameTime), float>(seconds);
}

bool MonoGameRules::Init(IGameObject *pGameObject)
{
	static CreateAbstractionLayerThunk create =
		CreateAbstractionLayerThunk(GetGameRulesClass()->GetFunction("CreateAbstractionLayer")->UnmanagedThunk);
	static RaiseOnInitThunk raise =
		RaiseOnInitThunk(GetGameRulesClass()->GetEvent("Initializing")->GetRaise()->UnmanagedThunk);

	this->SetGameObject(pGameObject);

	IEntity *entity = this->GetEntity();
	IEntityClass *entityClass = entity->GetClass();
	EntityId entityId = entity->GetId();
	// Create the abstraction layer.
	mono::exception ex;
	mono::object obj = create(ToMonoString(entityClass->GetName()), entityId, entity, &ex);
	if (!obj)
	{
		return false;
	}
	this->objHandle = MonoEnv->GC->Keep(obj);

	raise(this->objHandle.Object, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
		return false;
	}

	return pGameObject->BindToNetwork();
}

void MonoGameRules::PostInit(IGameObject *)
{
	static RaiseOnInitThunk raise =
		RaiseOnInitThunk(GetGameRulesClass()->GetEvent("Initialized")->GetRaise()->UnmanagedThunk);

	if (this->objHandle.IsValid)
	{
		mono::exception ex;
		raise(this->objHandle.Object, &ex);
	}
}

void MonoGameRules::ProcessEvent(SEntityEvent& _event)
{
	auto _eventType = _event.event;
	switch (_eventType)
	{
	case ENTITY_EVENT_RESET:
	{
		auto enterGameMode = _event.nParam[0] != 0;
		this->CallProc<game_rules_event(ResetInEditor), bool>(enterGameMode);
	}
		break;
	case ENTITY_EVENT_START_GAME:
		this->CallProc<game_rules_event(GameStarted)>();
		break;
	case ENTITY_EVENT_PRE_SERIALIZE:
		this->CallProc<game_rules_event(Synchronizing)>();
		break;
	case ENTITY_EVENT_POST_SERIALIZE:
		this->CallProc<game_rules_event(Synchronized)>();
		break;
	default:
		break;
	}
	
}

void MonoGameRules::InitClient(int channelId)
{
	static ClientInitRaiseThunk thunk =
		ClientInitRaiseThunk(GetGameRulesClass()->GetEvent("ClientInitializing")->Raise->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, channelId, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void MonoGameRules::PostInitClient(int channelId)
{
	static ClientInitRaiseThunk thunk =
		ClientInitRaiseThunk(GetGameRulesClass()->GetEvent("ClientInitialized")->Raise->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, channelId, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

bool MonoGameRules::ReloadExtension(IGameObject *, const SEntitySpawnParams &params)
{
	static ReloadEventThunk thunk =
		ReloadEventThunk(GetGameRulesClass()->GetEvent("Reloading")->GetRaise()->UnmanagedThunk);

	if (mono::object obj = this->objHandle.Object)
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

void MonoGameRules::PostReloadExtension(IGameObject *, const SEntitySpawnParams &params)
{
	static ReloadedEventThunk thunk =
		ReloadedEventThunk(GetGameRulesClass()->GetEvent("Reloaded")->GetRaise()->UnmanagedThunk);

	if (mono::object obj = this->objHandle.Object)
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

bool MonoGameRules::GetEntityPoolSignature(TSerialize signature)
{
	static GetSignatureThunk thunk =
		GetSignatureThunk(GetGameRulesClass()->GetFunction("GetSignature", -1)->UnmanagedThunk);

	if (mono::object obj = this->objHandle.Object)
	{
		mono::exception ex;
		bool result = thunk(obj, *reinterpret_cast<ISerialize **>(&signature), &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
			return false;
		}
		return result;
	}
	return false;
}

void MonoGameRules::FullSerialize(TSerialize ser)
{
	static SyncInternalThunk thunk =
		SyncInternalThunk(GetGameRulesClass()->GetFunction("SyncInternal", -1)->UnmanagedThunk);

	if (mono::object obj = this->objHandle.Object)
	{
		ser.BeginGroup("AbstractionLayer");

		mono::exception ex;
		thunk(obj, *reinterpret_cast<ISerialize **>(&ser), &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}

		ser.EndGroup();
	}
}

bool MonoGameRules::NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int flags)
{
	static NetSyncInternalThunk thunk =
		NetSyncInternalThunk(GetGameRulesClass()->GetFunction("NetSyncInternal", -1)->UnmanagedThunk);

	if (mono::object obj = this->objHandle.Object)
	{
		mono::exception ex;
		bool result = thunk(obj, *reinterpret_cast<ISerialize **>(&ser), aspect, profile, flags, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
			return false;
		}
		return result;
	}
	return false;
}

void MonoGameRules::Update(SEntityUpdateContext& ctx, int)
{
	static UpdateEntityThunk update =
		UpdateEntityThunk(GetGameRulesClass()->GetFunction("UpdateInternal")->UnmanagedThunk);

	if (mono::object o = this->objHandle.Object)
	{
		mono::exception ex;
		update(o, ctx, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}
	}
}

void MonoGameRules::SetChannelId(uint16 id)
{
	static IMonoField *channelIdField = GetGameRulesClass()->GetField("channelId");

	if (mono::object obj = this->objHandle.Object)
	{
		channelIdField->Set(obj, &id);
	}
}

void MonoGameRules::SetAuthority(bool auth)
{
	static OnAuthorizedEntityThunk update =
		OnAuthorizedEntityThunk(GetGameRulesClass()->GetEvent("Authorized")->Raise->UnmanagedThunk);

	if (mono::object o = this->objHandle.Object)
	{
		mono::exception ex;
		update(o, auth, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}
	}
}

void MonoGameRules::PostUpdate(float)
{
	static PostUpdateEntityThunk update =
		PostUpdateEntityThunk(GetGameRulesClass()->GetFunction("PostUpdateInternal")->UnmanagedThunk);

	if (mono::object o = this->objHandle.Object)
	{
		mono::exception ex;
		update(o, &ex);
		if (ex)
		{
			MonoEnv->HandleException(ex);
		}
	}
}