#pragma once

#include "IMonoInterface.h"
#include "MonoEntitySpawnParams.h"
#include <IGameObject.h>

RAW_THUNK typedef void(*DisposeMonoEntityThunk)();

RAW_THUNK typedef mono::object(*CreateAbstractionLayerThunk)(mono::string, EntityId, IEntity *);
RAW_THUNK typedef mono::object(*RaiseOnInitThunk)(mono::object);

RAW_THUNK typedef void(*ClientInitRaiseThunk)(mono::object, ushort);

RAW_THUNK typedef bool(*ReloadEventThunk)(mono::object, MonoEntitySpawnParams *);

RAW_THUNK typedef void(*ReloadedEventThunk)(mono::object, MonoEntitySpawnParams *);

RAW_THUNK typedef bool(*GetSignatureThunk)(mono::object, ISerialize *);

RAW_THUNK typedef bool(*SyncInternalThunk)(mono::object, ISerialize *);

RAW_THUNK typedef bool(*NetSyncInternalThunk)(mono::object, ISerialize *, EEntityAspects, byte, int);

RAW_THUNK typedef void(*UpdateEntityThunk)(mono::object, SEntityUpdateContext&);

RAW_THUNK typedef void(*OnAuthorizedEntityThunk)(mono::object, bool);

RAW_THUNK typedef void(*PostUpdateEntityThunk)(mono::object);