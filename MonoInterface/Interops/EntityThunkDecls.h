#pragma once

#include "IMonoInterface.h"
#include "MonoEntitySpawnParams.h"
#include <IGameObject.h>

typedef void(__stdcall *DisposeMonoEntityThunk)(mono::exception *);

typedef mono::object(__stdcall *CreateAbstractionLayerThunk)(mono::string, EntityId, IEntity *, mono::exception *);
typedef mono::object(__stdcall *RaiseOnInitThunk)(mono::object, mono::exception *);

typedef void(__stdcall *ClientInitRaiseThunk)(mono::object, ushort, mono::exception *);

typedef bool(__stdcall *ReloadEventThunk)(mono::object, MonoEntitySpawnParams *, mono::exception *);

typedef void(__stdcall *ReloadedEventThunk)(mono::object, MonoEntitySpawnParams *, mono::exception *);

typedef bool(__stdcall *GetSignatureThunk)(mono::object, ISerialize *, mono::exception *);

typedef bool(__stdcall *SyncInternalThunk)(mono::object, ISerialize *, mono::exception *);

typedef bool(__stdcall *NetSyncInternalThunk)(mono::object, ISerialize *, EEntityAspects, byte, int, mono::exception *);

typedef void(__stdcall *UpdateEntityThunk)(mono::object, SEntityUpdateContext&, mono::exception *);

typedef void(__stdcall *OnAuthorizedEntityThunk)(mono::object, bool, mono::exception *);

typedef void(__stdcall *PostUpdateEntityThunk)(mono::object, mono::exception *);