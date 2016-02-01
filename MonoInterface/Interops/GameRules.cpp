#include "stdafx.h"

#include "GameRules.h"

#pragma region Helpers
#include "MonoEntitySpawnParams.h"
#include "GameCollisionInfo.h"

// TODO: Use the method names as part of the template.

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

inline NtText CreateFuncName(const char *funcName)
{
	return NtText(2, funcName, "Internal");
}

template<typename ResultType, typename TArg0, typename TArg1, typename TArg2, typename TArg3, typename TArg4>
ResultType MonoGameRules::CallFunc(const char *funcName, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) const
{
	ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, arg4, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<typename ResultType, typename TArg0, typename TArg1, typename TArg2, typename TArg3>
ResultType MonoGameRules::CallFunc(const char *funcName, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3) const
{
	ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *)>
		(GetGameRulesClass()->GetFunction(funcName, -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<typename ResultType, typename TArg0, typename TArg1, typename TArg2>
ResultType MonoGameRules::CallFunc(const char *funcName, TArg0 arg0, TArg1 arg1, TArg2 arg2) const
{
	ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, TArg2, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, arg2, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<typename ResultType, typename TArg0, typename TArg1>
ResultType MonoGameRules::CallFunc(const char *funcName, TArg0 arg0, TArg1 arg1) const
{
	ResultType(__stdcall *thunk)(mono::object, TArg0, TArg1, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, TArg1, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, arg1, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<typename ResultType, typename TArg0>
ResultType MonoGameRules::CallFunc(const char *funcName, TArg0 arg0) const
{
	ResultType(__stdcall *thunk)(mono::object, TArg0, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, TArg0, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, arg0, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}

template<typename ResultType>
ResultType MonoGameRules::CallFunc(const char *funcName) const
{
	ResultType(__stdcall *thunk)(mono::object, mono::exception *) =
		reinterpret_cast<ResultType(__stdcall *)(mono::object, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	ResultType result = thunk(this->objHandle.Object, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
	return result;
}


template<typename TArg0, typename TArg1, typename TArg2, typename TArg3, typename TArg4>
void MonoGameRules::CallProc(const char *funcName, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) const
{
	void(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, TArg4, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, arg4, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<typename TArg0, typename TArg1, typename TArg2, typename TArg3>
void MonoGameRules::CallProc(const char *funcName, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3) const
{
	void(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, TArg2, TArg3, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, arg2, arg3, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<typename TArg0, typename TArg1, typename TArg2>
void MonoGameRules::CallProc(const char *funcName, TArg0 arg0, TArg1 arg1, TArg2 arg2) const
{
	void(__stdcall *thunk)(mono::object, TArg0, TArg1, TArg2, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, TArg2, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, arg2, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<typename TArg0, typename TArg1>
void MonoGameRules::CallProc(const char *funcName, TArg0 arg0, TArg1 arg1) const
{
	void(__stdcall *thunk)(mono::object, TArg0, TArg1, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, TArg1, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, arg1, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

template<typename TArg0>
void MonoGameRules::CallProc(const char *funcName, TArg0 arg0) const
{
	void(__stdcall *thunk)(mono::object, TArg0, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, TArg0, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

	mono::exception ex;
	thunk(this->objHandle.Object, arg0, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void MonoGameRules::CallProc(const char *funcName) const
{
	void(__stdcall *thunk)(mono::object, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(mono::object, mono::exception *)>
		(GetGameRulesClass()->GetFunction(CreateFuncName(funcName), -1)->UnmanagedThunk);

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

bool MonoGameRules::ShouldKeepClient(int channelId, EDisconnectionCause cause, const char *desc) const
{
	return this->CallFunc<bool, int, EDisconnectionCause, mono::string>
		("ShouldKeepClient", channelId, cause, ToMonoString(desc));
}

void MonoGameRules::PrecacheLevel()
{
	this->CallProc("PrecacheLevel");
}

void MonoGameRules::OnConnect(struct INetChannel *pNetChannel)
{
	this->CallProc<INetChannel *>("OnConnect", pNetChannel);
}

void MonoGameRules::OnDisconnect(EDisconnectionCause cause, const char *desc)
{
	this->CallProc<EDisconnectionCause, mono::string>("OnDisconnect", cause, ToMonoString(desc));
}

bool MonoGameRules::OnClientConnect(int channelId, bool isReset)
{
	return this->CallFunc<bool, int, bool>("OnClientConnect", channelId, isReset);
}

void MonoGameRules::OnClientDisconnect(int channelId, EDisconnectionCause cause, const char *desc,
									   bool keepClient)
{
	this->CallProc<int, EDisconnectionCause, mono::string, bool>("OnClientDisconnect", channelId, cause,
																 ToMonoString(desc), keepClient);
}

bool MonoGameRules::OnClientEnteredGame(int channelId, bool isReset)
{
	return this->CallFunc<bool, int, bool>("OnClientEnteredGame", channelId, isReset);
}

void MonoGameRules::OnEntitySpawn(IEntity *pEntity)
{
	this->CallProc<IEntity *>("OnEntitySpawn", pEntity);
}

void MonoGameRules::OnEntityRemoved(IEntity *pEntity)
{
	this->CallProc<IEntity *>("OnEntityRemoved", pEntity);
}

void MonoGameRules::OnEntityReused(IEntity *pEntity, SEntitySpawnParams &params, EntityId prevId)
{
	MonoEntitySpawnParams parameters(params);
	this->CallProc<IEntity *, MonoEntitySpawnParams &, EntityId>("", pEntity, parameters, prevId);
}

void MonoGameRules::SendTextMessage(ETextMessageType type, const char *msg, uint32 to, int channelId,
									const char *, const char *, const char *, const char *)
{
	this->CallProc<ETextMessageType, mono::string, uint32, int>("SendTextMessage", type, ToMonoString(msg), to,
																channelId);
}

void MonoGameRules::SendChatMessage(EChatMessageType type, EntityId sourceId, EntityId targetId, const char *msg)
{
	this->CallProc<EChatMessageType, EntityId, EntityId, mono::string>("SendChatMessage", type, sourceId,
																	   targetId, ToMonoString(msg));
}

bool MonoGameRules::OnCollision(const SGameCollision& _event)
{
	GameCollisionInfo info(_event);
	return this->CallFunc<bool, GameCollisionInfo &>("OnCollision", info);
}

void MonoGameRules::ShowStatus()
{
	this->CallProc("ShowStatus");
}

bool MonoGameRules::IsTimeLimited() const
{
	return this->CallFunc<bool>("IsTimeLimited");
}

float MonoGameRules::GetRemainingGameTime() const
{
	return this->CallFunc<float>("GetRemainingGameTime");
}

void MonoGameRules::SetRemainingGameTime(float seconds)
{
	this->CallProc<float>("SetRemainingGameTime", seconds);
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
