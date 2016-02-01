#include "stdafx.h"

#include "GameRules.h"

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

}

MonoGameRules::~MonoGameRules()
{

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

#define method(name) gameRulesMethodName##name

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
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::PostInit(IGameObject *pGameObject)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::InitClient(int channelId)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::PostInitClient(int channelId)
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool MonoGameRules::ReloadExtension(IGameObject *pGameObject, const SEntitySpawnParams &params)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::PostReloadExtension(IGameObject *pGameObject, const SEntitySpawnParams &params)
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool MonoGameRules::GetEntityPoolSignature(TSerialize signature)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::Release()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::FullSerialize(TSerialize ser)
{
	throw std::logic_error("The method or operation is not implemented.");
}

bool MonoGameRules::NetSerialize(TSerialize ser, EEntityAspects aspect, uint8 profile, int pflags)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::PostSerialize()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::SerializeSpawnInfo(TSerialize ser)
{
	throw std::logic_error("The method or operation is not implemented.");
}

ISerializableInfoPtr MonoGameRules::GetSpawnInfo()
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::Update(SEntityUpdateContext& ctx, int updateSlot)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::HandleEvent(const SGameObjectEvent& event)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::SetChannelId(uint16 id)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::SetAuthority(bool auth)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::PostUpdate(float frameTime)
{
	throw std::logic_error("The method or operation is not implemented.");
}

void MonoGameRules::PostRemoteSpawn()
{
	throw std::logic_error("The method or operation is not implemented.");
}
