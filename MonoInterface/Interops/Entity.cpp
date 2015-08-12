#include "stdafx.h"

#include "Entity.h"
#include "EntityClass.h"
#include "EntityExtension.h"
#include "MonoLightProperties.h"

IEntityPoolManager *poolManager;
IEntitySystem *entitySystem;

void EntityIdInterop::OnRunTimeInitialized()
{
	// Hopefully this function will be called before any other ones in this file.
	entitySystem = gEnv->pEntitySystem;
	poolManager = entitySystem->GetIEntityPoolManager();

	REGISTER_METHOD(GetEntity);
	REGISTER_METHOD(PrepareInternal);
	REGISTER_METHOD(ReturnInternal);
	REGISTER_METHOD(ResetBookmarkInternal);
	REGISTER_METHOD(IsEntityBookmarkedInternal);
	REGISTER_METHOD(GetBookmarkedClassNameInternal);
	REGISTER_METHOD(GetBookmarkedEntityNameInternal);
}

IEntity *EntityIdInterop::GetEntity(uint id)
{
	return entitySystem->GetEntity(id);
}

bool EntityIdInterop::PrepareInternal(EntityId id, bool prepareNow)
{
	return poolManager->PrepareFromPool(id, prepareNow);
}

bool EntityIdInterop::ReturnInternal(EntityId id, bool saveState /*= true*/)
{
	return poolManager->ReturnToPool(id, saveState);
}

void EntityIdInterop::ResetBookmarkInternal(EntityId id)
{
	poolManager->ResetBookmark(id);
}

bool EntityIdInterop::IsEntityBookmarkedInternal(EntityId entityId)
{
	return poolManager->IsEntityBookmarked(entityId);
}

mono::string EntityIdInterop::GetBookmarkedClassNameInternal(EntityId entityId)
{
	return ToMonoString(poolManager->GetBookmarkedClassName(entityId));
}

mono::string EntityIdInterop::GetBookmarkedEntityNameInternal(EntityId entityId)
{
	return ToMonoString(poolManager->GetBookmarkedEntityName(entityId));
}

IMonoClass *EntityPoolInterop::GetMonoClass()
{
	return MonoEnv->Cryambly->GetClass(this->GetInteropNameSpace(), this->GetInteropClassName());
}

void EntityPoolInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Enable);
	REGISTER_METHOD(Disable);
	REGISTER_METHOD(ResetPools);
	REGISTER_METHOD(IsDefaultBookmarked);
	REGISTER_METHOD(IsPreparingEntity);
}

#include "MonoEntitySpawnParams.h"

typedef void(__stdcall *OnPoolBookmarkCreatedThunk)(EntityId, MonoEntitySpawnParams, IXmlNode *, mono::exception *);
typedef void(__stdcall *PoolMigrationHandlerThunk)(EntityId, IEntity *, mono::exception *);
typedef void(__stdcall *PoolBookmarkSyncHandlerThunk)(ISerialize *, IEntity *, mono::exception *);
typedef void(__stdcall *OnPoolDefinitionsLoadedThunk)(mono::exception *);

void EntityPoolInterop::OnPoolBookmarkCreated(EntityId entityId, const SEntitySpawnParams& params, XmlNodeRef entityNode)
{
	static OnPoolBookmarkCreatedThunk thunk =
		OnPoolBookmarkCreatedThunk(this->GetMonoClass()->GetFunction("OnPoolBookmarkCreated", 3)->UnmanagedThunk);

	mono::exception ex;
	thunk(entityId, params, entityNode, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnEntityPreparedFromPool(EntityId entityId, IEntity *pEntity)
{
	static PoolMigrationHandlerThunk thunk =
		PoolMigrationHandlerThunk(this->GetMonoClass()->GetFunction("OnEntityPrepared", 2)->UnmanagedThunk);

	mono::exception ex;
	thunk(entityId, pEntity, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnEntityReturningToPool(EntityId entityId, IEntity *pEntity)
{
	static PoolMigrationHandlerThunk thunk =
		PoolMigrationHandlerThunk(this->GetMonoClass()->GetFunction("OnEntityReturning", 2)->UnmanagedThunk);

	mono::exception ex;
	thunk(entityId, pEntity, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnEntityReturnedToPool(EntityId entityId, IEntity *pEntity)
{
	static PoolMigrationHandlerThunk thunk =
		PoolMigrationHandlerThunk(this->GetMonoClass()->GetFunction("OnEntityReturned", 2)->UnmanagedThunk);

	mono::exception ex;
	thunk(entityId, pEntity, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnPoolDefinitionsLoaded(size_t numAI)
{
	static OnPoolDefinitionsLoadedThunk thunk =
		OnPoolDefinitionsLoadedThunk(this->GetMonoClass()->GetFunction("OnDefinitionsLoaded", 0)->UnmanagedThunk);

	mono::exception ex;
	thunk(&ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnBookmarkEntitySerialize(TSerialize serialize, void *pVEntity)
{
	static PoolBookmarkSyncHandlerThunk thunk =
		PoolBookmarkSyncHandlerThunk(this->GetMonoClass()->GetFunction("OnBookmarkSyncing", 2)->UnmanagedThunk);

	mono::exception ex;
	thunk(*reinterpret_cast<ISerialize **>(&serialize), static_cast<IEntity *>(pVEntity), &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::Enable()
{
	poolManager->Enable(true);
}

void EntityPoolInterop::Disable()
{
	poolManager->Enable(false);
}

void EntityPoolInterop::ResetPools(bool saveState)
{
	poolManager->ResetPools(saveState);
}

bool EntityPoolInterop::IsDefaultBookmarked(mono::string className)
{
	return poolManager->IsClassDefaultBookmarked(NtText(className));
}

bool EntityPoolInterop::IsPreparingEntity(EntityId *entityId)
{
	IEntityPoolManager::SPreparingParams pars;
	bool b = poolManager->IsPreparingEntity(pars);
	*entityId = pars.entityId;
	return b;
}

List<NtText> EntitySystemInterop::monoEntityClassNames;

bool EntitySystemInterop::IsMonoEntity(const char *className)
{
	return monoEntityClassNames.Find([className](NtText &name) { return name.Equals(className); }) != nullptr;
}

IEntityProxyPtr EntitySystemInterop::CreateGameObjectForCryCilEntity(IEntity *pEntity, SEntitySpawnParams &params,
																	 void *pUserData)
{
	auto className = pEntity->GetClass()->GetName();

	IGameObject *gameObject;
	auto entityProxy = MonoEnv->CryAction->GetIGameObjectSystem()->CreateGameObjectEntityProxy(*pEntity, &gameObject);

	if (!entityProxy)
	{
		MonoWarning("Unable to create a game object for an entity of class %s", className);
		return IEntityProxyPtr();
	}

	if (!gameObject->ActivateExtension(className))
	{
		MonoWarning("Unable to activate abstraction layer between CreEngine entity of class %s and logic that is defined for it in CryCIL", className);
		return IEntityProxyPtr();
	}

	gameObject->SetUserData(pUserData);

	return entityProxy;
}

void EntitySystemInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(RegisterEntityClass);
	REGISTER_METHOD(RemoveEntity);
	REGISTER_METHOD(SpawnMonoEntity);
	REGISTER_METHOD(SpawnNetEntity);
	REGISTER_METHOD(SpawnCryEntity);
	throw std::logic_error("The method or operation is not implemented.");
}

struct EntityPropertyInfo
{
	mono::string name;
	IEntityPropertyHandler::EPropertyType type;
	mono::string editType;
	mono::string description;
	uint flags;

	Vec2 limits;

	mono::string defaultValue;

	MonoEntityProperty ToNative() const
	{
		MonoEntityProperty prop;
		prop.info.description = ToNativeString(this->description);
		prop.info.editType = ToNativeString(this->editType);
		prop.info.name = ToNativeString(this->name);
		prop.info.flags = this->flags;
		prop.info.type = this->type;
		prop.info.limits.min = this->limits.x;
		prop.info.limits.max = this->limits.y;
		prop.defaultValue = ToNativeString(this->defaultValue);
		return prop;
	}
};

struct MonoEntityCreator : public IGameObjectExtensionCreatorBase
{
	virtual IGameObjectExtensionPtr Create() override
	{
		return ComponentCreate_DeleteWithRelease<MonoEntityExtension>();
	}
	virtual void GetGameObjectExtensionRMIData(void **ppRMI, size_t *nCount) override
	{
		return MonoEntityExtension::GetGameObjectExtensionRMIData(ppRMI, nCount);
	}
};

bool EntitySystemInterop::RegisterEntityClass(mono::string name, mono::string category, mono::string editorHelper,
										mono::string editorIcon, EEntityClassFlags flags, mono::Array properties,
										bool networked, bool dontSyncProps)
{
	const char *className = ToNativeString(name);

	auto registry = gEnv->pEntitySystem->GetClassRegistry();

	auto nameMatch =
		[className](NtText &registeredName)
		{
			return registeredName.Equals(className);
		};

	if ((flags && EEntityClassFlags::ECLF_MODIFY_EXISTING) == 0)
	{
		// If we are not modifying anything, then gotta make sure that the class wasn't registered before.
		if (monoEntityClassNames.Find(nameMatch))
		{
			MonoWarning("%s class is already registered as a CryCIL entity class.", className);
			return false;
		}
		if (registry->FindClass(className) != nullptr)
		{
			MonoWarning("%s class is already registered as a native CryEngine entity class.", className);
			return false;
		}
	}

	// Pin stuff just in case.
	MonoGCHandle nameHandle         = MonoEnv->GC->Pin(name);
	MonoGCHandle categoryHandle     = MonoEnv->GC->Pin(category);
	MonoGCHandle editorHelperHandle = MonoEnv->GC->Pin(editorHelper);
	MonoGCHandle editorIconHandle   = MonoEnv->GC->Pin(editorIcon);
	MonoGCHandle propertiesHandle   = MonoEnv->GC->Pin(properties);

	// Copy the properties to the list.
	IMonoArray<EntityPropertyInfo> propInfos = properties;
	List<MonoEntityProperty> props(propInfos.Length);
	for (int i = 0; i < props.Length; i++)
	{
		props[i] = propInfos[i].ToNative();
	}

	monoEntityClassNames.AddOverride(className, nameMatch);

	IEntityClassRegistry::SEntityClassDesc description;
	// Flags and a name.
	description.sName = className;
	description.flags = flags;
	// Information for the editor.
	description.editorClassInfo.sCategory = ToNativeString(category);
	description.editorClassInfo.sHelper   = ToNativeString(editorHelper);
	description.editorClassInfo.sIcon     = ToNativeString(editorIcon);
	// Handlers.
	static NullEntityEventHandler eventHandler;
	description.pEventHandler = &eventHandler;
	description.pPropertyHandler = new MonoEntityPropertyHandler(props);
	// For now this all user data that will come with a class.
	description.pUserProxyCreateFunc = CreateGameObjectForCryCilEntity;
	description.pUserProxyData = new MonoEntityClassUserData(networked, dontSyncProps);

	registry->RegisterStdClass(description);
	static MonoEntityCreator creator;
	MonoEnv->CryAction->GetIGameObjectSystem()->RegisterExtension(className, &creator, nullptr);
	return true;
}

void EntitySystemInterop::RemoveEntity(EntityId id, bool now)
{
	// Check identifier validity.
	if (gEnv->pEntitySystem->GetEntity(id) == nullptr)
	{
		return;
	}

	gEnv->pEntitySystem->RemoveEntity(id, now);
}

mono::object EntitySystemInterop::SpawnMonoEntity(MonoEntitySpawnParams parameters)
{
	auto params = parameters.ToNative();

	IEntityClass *entityClass = params.pClass;
	if (!entityClass)
	{
		ArgumentException("Cannot create an entity without a valid class.").Throw();
		return nullptr;
	}

	const char *className = entityClass->GetName();
	auto nameMatch =
		[className](NtText &name)
	{
		return name.Equals(className);
	};

	if (monoEntityClassNames.Find(nameMatch) == nullptr)
	{
		ArgumentException("EntitySystem.SpawnMonoEntity cannot be used to spawn entities that are not defined in CryCIL.").Throw();
		return nullptr;
	}

	IEntity *entity = gEnv->pEntitySystem->SpawnEntity(params, true);

	auto ext = QueryMonoEntityExtension(entity);

	if (!ext)
	{
		MonoWarning("Abstraction layer between entity of class %s named %s was not created.", className, params.sName);
		return nullptr;
	}

	return ext->MonoWrapper;
}

mono::object EntitySystemInterop::SpawnNetEntity(MonoEntitySpawnParams parameters, ushort channelId)
{
	auto params = parameters.ToNative();

	IEntityClass *entityClass = params.pClass;
	if (!entityClass)
	{
		ArgumentException("Cannot create an entity without a valid class.").Throw();
		return nullptr;
	}

	const char *className = entityClass->GetName();
	auto nameMatch =
		[className](NtText &name)
	{
		return name.Equals(className);
	};

	if (monoEntityClassNames.Find(nameMatch) == nullptr)
	{
		ArgumentException("EntitySystem.SpawnNetEntity cannot be used to spawn entities that are not defined in CryCIL.").Throw();
		return nullptr;
	}
	// Check if this entity class represents entities that are bound to network.
	if (!static_cast<MonoEntityClassUserData *>(entityClass->GetUserProxyData())->networked)
	{
		ArgumentException("EntitySystem.SpawnNetEntity cannot be used to spawn entities that are not bound to network.").Throw();
		return nullptr;
	}

	IEntity *entity = gEnv->pEntitySystem->SpawnEntity(params, false);

	auto gameObject = MonoEnv->CryAction->GetGameObject(entity->GetId());
	// Channel id is one of those things that should be set before initialization.
	gameObject->SetChannelId(channelId);

	gEnv->pEntitySystem->InitEntity(entity, params);

	auto ext = QueryMonoEntityExtension(entity);

	if (!ext)
	{
		MonoWarning("Abstraction layer between entity of class %s named %s was not created.", className, params.sName);
		return nullptr;
	}

	return ext->MonoWrapper;
}

IEntity *EntitySystemInterop::SpawnCryEntity(MonoEntitySpawnParams parameters)
{
	auto params = parameters.ToNative();

	IEntityClass *entityClass = params.pClass;
	if (!entityClass)
	{
		ArgumentException("Cannot create an entity without a valid class.").Throw();
		return nullptr;
	}

	return gEnv->pEntitySystem->SpawnEntity(params, true);
}

void NetEntityInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetChannelId);
	REGISTER_METHOD(InvokeRmi);
	REGISTER_METHOD(ChangeNetworkStateInternal);
}

void NetEntityInterop::SetChannelId(EntityId entityId, ushort channelId)
{
	IGameObject *gameObject = MonoEnv->CryAction->GetGameObject(entityId);

	if (gameObject)
	{
		gameObject->SetChannelId(channelId);
	}
}

enum CryCilRmiType
{
	// Attachment flags.
	crycil_PreAttach = 1,
	crycil_PostAttach = 2,
	crycil_NoAttach = 3,
	crycil_Urgent = 4,
	crycil_Independent = 5,

	crycil_AttachmentMask = crycil_PreAttach | crycil_PostAttach | crycil_NoAttach | crycil_Urgent | crycil_Independent,
	// Reliability flags.
	crycil_Reliable = 1 << 3,
	// Is Server.
	crycil_ToServer = 1 << 4,
	// Is Fast
	crycil_LowDelay = 1 << 5,

	crycil_PreAttachServer = crycil_PreAttach | crycil_ToServer,
	crycil_PreAttachClient = crycil_PreAttach,
	crycil_PostAttachServer = crycil_PostAttach | crycil_ToServer,
	crycil_PostAttachClient = crycil_PostAttach,
	crycil_ReliableNoAttachServer = crycil_Reliable | crycil_NoAttach | crycil_ToServer,
	crycil_ReliableNoAttachClient = crycil_Reliable | crycil_NoAttach,
	crycil_UnreliableNoAttachServer = crycil_NoAttach | crycil_ToServer,
	crycil_UnreliableNoAttachClient = crycil_NoAttach,

	crycil_FastPreAttachServer = crycil_LowDelay | crycil_PreAttach | crycil_ToServer,
	crycil_FastPreAttachClient = crycil_LowDelay | crycil_PreAttach,
	crycil_FastPostAttachServer = crycil_LowDelay | crycil_PostAttach | crycil_ToServer,
	crycil_FastPostAttachClient = crycil_LowDelay | crycil_PostAttach,
	crycil_FastReliableNoAttachServer = crycil_LowDelay | crycil_Reliable | crycil_NoAttach | crycil_ToServer,
	crycil_FastReliableNoAttachClient = crycil_LowDelay | crycil_Reliable | crycil_NoAttach,
	crycil_FastUnreliableNoAttachServer = crycil_LowDelay | crycil_NoAttach | crycil_ToServer,
	crycil_FastUnreliableNoAttachClient = crycil_LowDelay | crycil_NoAttach,

	crycil_ReliableUrgentServer = crycil_Reliable | crycil_Urgent | crycil_ToServer,
	crycil_ReliableUrgentClient = crycil_Reliable | crycil_Urgent,
	crycil_UnreliableUrgentServer = crycil_Urgent | crycil_ToServer,
	crycil_UnreliableUrgentClient = crycil_Urgent,

	crycil_ReliableIndependentServer = crycil_Reliable | crycil_Independent | crycil_ToServer,
	crycil_ReliableIndependentClient = crycil_Reliable | crycil_Independent,
	crycil_UnreliableIndependentServer = crycil_Independent | crycil_ToServer,
	crycil_UnreliableIndependentClient = crycil_Independent
};

#define INVOKE_CRYCIL_RMI(name) gameObject->InvokeRMI(MonoEntityExtension::name(), params, _where, channel);

void NetEntityInterop::InvokeRmi(EntityId sender, mono::string methodName, mono::object parameters, uint32 _where, int channel, int rmiType)
{
	MonoEntityExtension::CryCilRMIParameters params
		(NtText(methodName), MonoEnv->GC->Keep(parameters), IMonoObject(parameters).Class->FullName);

	IGameObject *gameObject = MonoEnv->CryAction->GetGameObject(sender);
	CryCilRmiType type = CryCilRmiType(rmiType);

	switch (type)
	{
	case crycil_PreAttachServer:
		INVOKE_CRYCIL_RMI(svPreAttachCryCilRmi);
		break;
	case crycil_PreAttachClient:
		INVOKE_CRYCIL_RMI(clPreAttachCryCilRmi);
		break;
	case crycil_PostAttachServer:
		INVOKE_CRYCIL_RMI(svPostAttachCryCilRmi);
		break;
	case crycil_PostAttachClient:
		INVOKE_CRYCIL_RMI(clPostAttachCryCilRmi);
		break;
	case crycil_ReliableNoAttachServer:
		INVOKE_CRYCIL_RMI(svReliableNoAttachCryCilRmi);
		break;
	case crycil_ReliableNoAttachClient:
		INVOKE_CRYCIL_RMI(clReliableNoAttachCryCilRmi);
		break;
	case crycil_UnreliableNoAttachServer:
		INVOKE_CRYCIL_RMI(svUnreliableNoAttachCryCilRmi);
		break;
	case crycil_UnreliableNoAttachClient:
		INVOKE_CRYCIL_RMI(clUnreliableNoAttachCryCilRmi);
		break;
	case crycil_FastPreAttachServer:
		INVOKE_CRYCIL_RMI(svFastPreAttachCryCilRmi);
		break;
	case crycil_FastPreAttachClient:
		INVOKE_CRYCIL_RMI(clFastPreAttachCryCilRmi);
		break;
	case crycil_FastPostAttachServer:
		INVOKE_CRYCIL_RMI(svFastPostAttachCryCilRmi);
		break;
	case crycil_FastPostAttachClient:
		INVOKE_CRYCIL_RMI(clFastPostAttachCryCilRmi);
		break;
	case crycil_FastReliableNoAttachServer:
		INVOKE_CRYCIL_RMI(svFastReliableNoAttachCryCilRmi);
		break;
	case crycil_FastReliableNoAttachClient:
		INVOKE_CRYCIL_RMI(clFastReliableNoAttachCryCilRmi);
		break;
	case crycil_FastUnreliableNoAttachServer:
		INVOKE_CRYCIL_RMI(svFastUnreliableNoAttachCryCilRmi);
		break;
	case crycil_FastUnreliableNoAttachClient:
		INVOKE_CRYCIL_RMI(clFastUnreliableNoAttachCryCilRmi);
		break;
	case crycil_ReliableUrgentServer:
		INVOKE_CRYCIL_RMI(svReliableUrgentCryCilRmi);
		break;
	case crycil_ReliableUrgentClient:
		INVOKE_CRYCIL_RMI(clReliableUrgentCryCilRmi);
		break;
	case crycil_UnreliableUrgentServer:
		INVOKE_CRYCIL_RMI(svUnreliableUrgentCryCilRmi);
		break;
	case crycil_UnreliableUrgentClient:
		INVOKE_CRYCIL_RMI(clUnreliableUrgentCryCilRmi);
		break;
	case crycil_ReliableIndependentServer:
		INVOKE_CRYCIL_RMI(svReliableIndependentCryCilRmi);
		break;
	case crycil_ReliableIndependentClient:
		INVOKE_CRYCIL_RMI(clReliableIndependentCryCilRmi);
		break;
	case crycil_UnreliableIndependentServer:
		INVOKE_CRYCIL_RMI(svUnreliableIndependentCryCilRmi);
		break;
	case crycil_UnreliableIndependentClient:
		INVOKE_CRYCIL_RMI(clUnreliableIndependentCryCilRmi);
		break;
	default:
		break;
	}
}

#undef INVOKE_CRYCIL_RMI

void NetEntityInterop::ChangeNetworkStateInternal(EntityId id, uint32 aspects)
{
	auto gameObject = MonoEnv->CryAction->GetGameObject(id);

	gameObject->ChangedNetworkState(aspects);
}

void CryEntityInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(AddFlagsInternal);
	REGISTER_METHOD(ClearFlagsInternal);
	REGISTER_METHOD(CheckFlagsInternal);
	REGISTER_METHOD(GetIsGarbage);
	REGISTER_METHOD(SetNameInternal);
	REGISTER_METHOD(GetNameInternal);
	REGISTER_METHOD(GetIsLoadedFromLevelFile);
	REGISTER_METHOD(GetIsFromPool);
	REGISTER_METHOD(AttachChildInternal);
	REGISTER_METHOD(DetachAllInternal);
	REGISTER_METHOD(DetachThisInternal);
	REGISTER_METHOD(GetChildCount);
	REGISTER_METHOD(GetChildInternal);
	REGISTER_METHOD(GetParent);
	REGISTER_METHOD(GetParentAttachPointWorldTM);
	REGISTER_METHOD(GetIsParentAttachmentValid);
	REGISTER_METHOD(SetWorldTM);
	REGISTER_METHOD(SetLocalTM);
	REGISTER_METHOD(GetWorldTM);
	REGISTER_METHOD(GetLocalTM);
	REGISTER_METHOD(GetWorldBounds);
	REGISTER_METHOD(GetLocalBounds);
	REGISTER_METHOD(SetPos);
	REGISTER_METHOD(GetPos);
	REGISTER_METHOD(SetRotation);
	REGISTER_METHOD(GetRotation);
	REGISTER_METHOD(SetScale);
	REGISTER_METHOD(GetScale);
	REGISTER_METHOD(GetPosRotScale);
	REGISTER_METHOD(SetPosRotScale);
	REGISTER_METHOD(GetWorldPos);
	REGISTER_METHOD(GetWorldAngles);
	REGISTER_METHOD(GetWorldRotation);
	REGISTER_METHOD(GetForwardDir);
	REGISTER_METHOD(Activate);
	REGISTER_METHOD(IsActive);
	REGISTER_METHOD(PrePhysicsActivate);
	REGISTER_METHOD(IsPrePhysicsActive);
	REGISTER_METHOD(SetTimerInternal);
	REGISTER_METHOD(KillTimerInternal);
	REGISTER_METHOD(Hide);
	REGISTER_METHOD(IsHidden);
	REGISTER_METHOD(MakeInvisible);
	REGISTER_METHOD(IsInvisible);
	REGISTER_METHOD(GetUpdatePolicy);
	REGISTER_METHOD(SetUpdatePolicy);
	REGISTER_METHOD(SetMaterial);
	REGISTER_METHOD(GetMaterial);
	REGISTER_METHOD(GetEntityLinks);
	REGISTER_METHOD(AddEntityLink);
	REGISTER_METHOD(RemoveEntityLink);
	REGISTER_METHOD(RemoveAllEntityLinks);
	REGISTER_METHOD(GetNextLink);
	REGISTER_METHOD(GetLinkName);
	REGISTER_METHOD(GetLinkedEntityId);
	REGISTER_METHOD(GetLinkedEntityGuid);
}

void CryEntityInterop::SetFlags(IEntity *handle, uint64 flags)
{
	handle->SetFlags(*reinterpret_cast<uint32 *>(&flags));
	handle->SetFlagsExtended(uint32(flags >> 32));
}

uint64 CryEntityInterop::GetFlags(IEntity *handle)
{
	return uint64(handle->GetFlags()) | (uint64(handle->GetFlagsExtended()) << 32);
}

void CryEntityInterop::AddFlagsInternal(IEntity *handle, uint64 flagsToAdd)
{
	uint32 normalFlags = *reinterpret_cast<uint32 *>(&flagsToAdd);
	uint32 extendedFlags = uint32(flagsToAdd >> 32);

	if (normalFlags != 0)
	{
		handle->AddFlags(normalFlags);
	}
	if (extendedFlags != 0)
	{
		handle->SetFlagsExtended(handle->GetFlagsExtended() | extendedFlags);
	}
}

void CryEntityInterop::ClearFlagsInternal(IEntity *handle, uint64 flagsToClear)
{
	uint32 normalFlags = *reinterpret_cast<uint32 *>(&flagsToClear);
	uint32 extendedFlags = uint32(flagsToClear >> 32);

	if (normalFlags != 0)
	{
		handle->ClearFlags(normalFlags);
	}
	if (extendedFlags != 0)
	{
		handle->SetFlagsExtended(handle->GetFlagsExtended() & ~extendedFlags);
	}
}

bool CryEntityInterop::CheckFlagsInternal(IEntity *handle, uint64 flagsToCheck, bool all)
{
	uint64 currentFlags = GetFlags(handle);

	if (all)
	{
		return (currentFlags & flagsToCheck) == flagsToCheck;
	}
	else
	{
		return (currentFlags & flagsToCheck) != 0;
	}
}

bool CryEntityInterop::GetIsGarbage(IEntity *handle)
{
	return handle->IsGarbage();
}

void CryEntityInterop::SetNameInternal(IEntity *handle, mono::string sName)
{
	handle->SetName(NtText(sName));
}

mono::string CryEntityInterop::GetNameInternal(IEntity *handle)
{
	return ToMonoString(handle->GetName());
}

bool CryEntityInterop::GetIsLoadedFromLevelFile(IEntity *handle)
{
	return handle->IsLoadedFromLevelFile();
}

bool CryEntityInterop::GetIsFromPool(IEntity *handle)
{
	return handle->IsFromPool();
}

void CryEntityInterop::AttachChildInternal(IEntity *handle, IEntity *child, int flags, mono::string target)
{
	// Check for recursive attachment.
	for (IEntity *parent = handle->GetParent(); parent; parent = parent->GetParent())
	{
		if (parent != child)
		{
			ArgumentException("Cannot create a recursive attachment.").Throw();
		}
	}
	// Actually attach.
	handle->AttachChild(child, SChildAttachParams(flags, NtText(target)));
}

void CryEntityInterop::DetachAllInternal(IEntity *handle, bool keepWorldTM)
{
	handle->DetachAll(keepWorldTM ? IEntity::EAttachmentFlags::ATTACHMENT_KEEP_TRANSFORMATION : 0);
}

void CryEntityInterop::DetachThisInternal(IEntity *handle, bool keepWorldTM)
{
	handle->DetachThis(keepWorldTM ? IEntity::EAttachmentFlags::ATTACHMENT_KEEP_TRANSFORMATION : 0);
}

int CryEntityInterop::GetChildCount(IEntity *handle)
{
	return handle->GetChildCount();
}

IEntity *CryEntityInterop::GetChildInternal(IEntity *handle, int nIndex)
{
	if (nIndex < 0)
	{
		ArgumentOutOfRangeException("Index of the child entity cannot be less then 0.").Throw();
	}
	if (nIndex >= handle->GetChildCount())
	{
		ArgumentOutOfRangeException("Index of the child entity cannot be greater then total number of children.").Throw();
	}

	return handle->GetChild(nIndex);
}

IEntity *CryEntityInterop::GetParent(IEntity *handle)
{
	return handle->GetParent();
}

Matrix34 CryEntityInterop::GetParentAttachPointWorldTM(IEntity *handle)
{
	return handle->GetParentAttachPointWorldTM();
}

bool CryEntityInterop::GetIsParentAttachmentValid(IEntity *handle)
{
	return handle->IsParentAttachmentValid();
}

void CryEntityInterop::SetWorldTM(IEntity *handle, Matrix34 *tm)
{
	handle->SetWorldTM(*tm);
}

void CryEntityInterop::SetLocalTM(IEntity *handle, Matrix34 *tm)
{
	handle->SetLocalTM(*tm);
}

Matrix34 CryEntityInterop::GetWorldTM(IEntity *handle)
{
	return handle->GetWorldTM();
}

Matrix34 CryEntityInterop::GetLocalTM(IEntity *handle)
{
	return handle->GetLocalTM();
}

void CryEntityInterop::GetWorldBounds(IEntity *handle, AABB *bbox)
{
	handle->GetWorldBounds(*bbox);
}

void CryEntityInterop::GetLocalBounds(IEntity *handle, AABB *bbox)
{
	handle->GetLocalBounds(*bbox);
}

void CryEntityInterop::SetPos(IEntity *handle, Vec3 *vPos, bool bRecalcPhyBounds)
{
	handle->SetPos(*vPos, bRecalcPhyBounds);
}

Vec3 CryEntityInterop::GetPos(IEntity *handle)
{
	return handle->GetPos();
}

void CryEntityInterop::SetRotation(IEntity *handle, Quat *qRotation)
{
	handle->SetRotation(*qRotation);
}

Quat CryEntityInterop::GetRotation(IEntity *handle)
{
	return handle->GetRotation();
}

void CryEntityInterop::SetScale(IEntity *handle, Vec3 *vScale)
{
	handle->SetScale(*vScale);
}

Vec3 CryEntityInterop::GetScale(IEntity *handle)
{
	return handle->GetScale();
}

void CryEntityInterop::GetPosRotScale(IEntity *handle, Vec3 *pos, Quat *rotation, Vec3 *scale)
{
	*pos = handle->GetPos();
	*rotation = handle->GetRotation();
	*scale = handle->GetScale();
}

void CryEntityInterop::SetPosRotScale(IEntity *handle, Vec3 *pos, Quat *rotation, Vec3 *scale)
{
	handle->SetPosRotScale(*pos, *rotation, *scale);
}

Vec3 CryEntityInterop::GetWorldPos(IEntity *handle)
{
	return handle->GetWorldPos();
}

Ang3 CryEntityInterop::GetWorldAngles(IEntity *handle)
{
	return handle->GetWorldAngles();
}

Quat CryEntityInterop::GetWorldRotation(IEntity *handle)
{
	return handle->GetWorldRotation();
}

Vec3 CryEntityInterop::GetForwardDir(IEntity *handle)
{
	return handle->GetForwardDir();
}

void CryEntityInterop::Activate(IEntity *handle, bool bActive)
{
	handle->Activate(bActive);
}

bool CryEntityInterop::IsActive(IEntity *handle)
{
	return handle->IsActive();
}

void CryEntityInterop::PrePhysicsActivate(IEntity *handle, bool bActive)
{
	handle->PrePhysicsActivate(bActive);
}

bool CryEntityInterop::IsPrePhysicsActive(IEntity *handle)
{
	return handle->IsPrePhysicsActive();
}

void CryEntityInterop::SetTimerInternal(IEntity *handle, int nTimerId, int nMilliSeconds)
{
	handle->SetTimer(nTimerId, nMilliSeconds);
}

void CryEntityInterop::KillTimerInternal(IEntity *handle, int nTimerId)
{
	handle->KillTimer(nTimerId);
}

void CryEntityInterop::Hide(IEntity *handle, bool bHide)
{
	handle->Hide(bHide);
}

bool CryEntityInterop::IsHidden(IEntity *handle)
{
	return handle->IsHidden();
}

void CryEntityInterop::MakeInvisible(IEntity *handle, bool bInvisible)
{
	handle->Invisible(bInvisible);
}

bool CryEntityInterop::IsInvisible(IEntity *handle)
{
	return handle->IsInvisible();
}

void CryEntityInterop::SetUpdatePolicy(IEntity *handle, int eUpdatePolicy)
{
	handle->SetUpdatePolicy(EEntityUpdatePolicy(eUpdatePolicy));
}

int CryEntityInterop::GetUpdatePolicy(IEntity *handle)
{
	return handle->GetUpdatePolicy();
}

void CryEntityInterop::SetMaterial(IEntity *handle, IMaterial *pMaterial)
{
	handle->SetMaterial(pMaterial);
}

IMaterial *CryEntityInterop::GetMaterial(IEntity *handle)
{
	return handle->GetMaterial();
}

IEntityLink *CryEntityInterop::GetEntityLinks(IEntity *handle)
{
	return handle->GetEntityLinks();
}

IEntityLink *CryEntityInterop::AddEntityLink(IEntity *handle, mono::string linkName, EntityId entityId, EntityGUID entityGuid)
{
	return handle->AddEntityLink(NtText(linkName), entityId, entityGuid);
}

void CryEntityInterop::RemoveEntityLink(IEntity *handle, IEntityLink *pLink)
{
	handle->RemoveEntityLink(pLink);
}

void CryEntityInterop::RemoveAllEntityLinks(IEntity *handle)
{
	handle->RemoveAllEntityLinks();
}

IEntityLink *CryEntityInterop::GetNextLink(IEntityLink *linkHandle)
{
	return linkHandle->next;
}

mono::string CryEntityInterop::GetLinkName(IEntityLink *linkHandle)
{
	return ToMonoString(linkHandle->name);
}

EntityId CryEntityInterop::GetLinkedEntityId(IEntityLink *linkHandle)
{
	return linkHandle->entityId;
}

EntityGUID CryEntityInterop::GetLinkedEntityGuid(IEntityLink *linkHandle)
{
	return linkHandle->entityGuid;
}

void EntitySlotsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(IsSlotValid);
	REGISTER_METHOD(FreeSlot);
	REGISTER_METHOD(GetSlotInfo);
	REGISTER_METHOD(GetSlotWorldTM);
	REGISTER_METHOD(GetSlotLocalTM);
	REGISTER_METHOD(SetSlotLocalTM);
	REGISTER_METHOD(SetSlotCameraSpacePos);
	REGISTER_METHOD(GetSlotCameraSpacePos);
	REGISTER_METHOD(SetParentSlot);
	REGISTER_METHOD(SetSlotMaterial);
	REGISTER_METHOD(SetSlotFlags);
	REGISTER_METHOD(GetSlotFlags);
	REGISTER_METHOD(ShouldUpdateCharacter);
	REGISTER_METHOD(GetCharacter);
	REGISTER_METHOD(SetCharacter);
	REGISTER_METHOD(GetStatObj);
	REGISTER_METHOD(GetParticleEmitter);
	REGISTER_METHOD(GetGeomCacheRenderNode);
	REGISTER_METHOD(MoveSlot);
	REGISTER_METHOD(SetStatObj);
	REGISTER_METHOD(LoadGeometry);
	REGISTER_METHOD(LoadCharacter);
	REGISTER_METHOD(LoadGeomCache);
	REGISTER_METHOD(LoadParticleEmitterDefault);
	REGISTER_METHOD(LoadParticleEmitter);
	REGISTER_METHOD(SetParticleEmitter);
	REGISTER_METHOD(LoadLight);
	REGISTER_METHOD(GetSlotCount);
}

bool EntitySlotsInterop::IsSlotValid(IEntity *handle, int nIndex)
{
	return handle->IsSlotValid(nIndex);
}

void EntitySlotsInterop::FreeSlot(IEntity *handle, int nIndex)
{
	handle->FreeSlot(nIndex);
}

bool EntitySlotsInterop::GetSlotInfo(IEntity *handle, int nIndex, SEntitySlotInfo *slotInfo)
{
	return handle->GetSlotInfo(nIndex, *slotInfo);
}

void EntitySlotsInterop::GetSlotWorldTM(IEntity *handle, int slot, Matrix34 *matrix)
{
	*matrix = handle->GetSlotWorldTM(slot);
}

void EntitySlotsInterop::GetSlotLocalTM(IEntity *handle, int slot, bool bRelativeToParent, Matrix34 *matrix)
{
	*matrix = handle->GetSlotLocalTM(slot, bRelativeToParent);
}

void EntitySlotsInterop::SetSlotLocalTM(IEntity *handle, int slot, Matrix34 *localTM)
{
	handle->SetSlotLocalTM(slot, *localTM);
}

void EntitySlotsInterop::SetSlotCameraSpacePos(IEntity *handle, int slot, Vec3 *cameraSpacePos)
{
	handle->SetSlotCameraSpacePos(slot, *cameraSpacePos);
}

void EntitySlotsInterop::GetSlotCameraSpacePos(IEntity *handle, int slot, Vec3 *cameraSpacePos)
{
	handle->GetSlotCameraSpacePos(slot, *cameraSpacePos);
}

bool EntitySlotsInterop::SetParentSlot(IEntity *handle, int nParentIndex, int nChildIndex)
{
	return handle->SetParentSlot(nParentIndex, nChildIndex);
}

void EntitySlotsInterop::SetSlotMaterial(IEntity *handle, int slot, IMaterial *pMaterial)
{
	handle->SetSlotMaterial(slot, pMaterial);
}

void EntitySlotsInterop::SetSlotFlags(IEntity *handle, int slot, int nFlags)
{
	handle->SetSlotFlags(slot, nFlags);
}

int EntitySlotsInterop::GetSlotFlags(IEntity *handle, int slot)
{
	return handle->GetSlotFlags(slot);
}

bool EntitySlotsInterop::ShouldUpdateCharacter(IEntity *handle, int slot)
{
	return handle->ShouldUpdateCharacter(slot);
}

ICharacterInstance *EntitySlotsInterop::GetCharacter(IEntity *handle, int slot)
{
	return handle->GetCharacter(slot);
}

int EntitySlotsInterop::SetCharacter(IEntity *handle, ICharacterInstance *pCharacter, int slot)
{
	return handle->SetCharacter(pCharacter, slot);
}

IStatObj *EntitySlotsInterop::GetStatObj(IEntity *handle, int slot)
{
	return handle->GetStatObj(slot);
}

IParticleEmitter *EntitySlotsInterop::GetParticleEmitter(IEntity *handle, int slot)
{
	return handle->GetParticleEmitter(slot);
}

IGeomCacheRenderNode *EntitySlotsInterop::GetGeomCacheRenderNode(IEntity *handle, int slot)
{
	return handle->GetGeomCacheRenderNode(slot);
}

void EntitySlotsInterop::MoveSlot(IEntity *handle, IEntity *targetIEnt, int slot)
{
	handle->MoveSlot(targetIEnt, slot);
}

int EntitySlotsInterop::SetStatObj(IEntity *handle, IStatObj *pStatObj, int slot, bool bUpdatePhysics, float mass)
{
	return handle->SetStatObj(pStatObj, slot, bUpdatePhysics, mass);
}

int EntitySlotsInterop::LoadGeometry(IEntity *handle, int slot, mono::string sFilename, mono::string sGeomName, int nLoadFlags)
{
	return handle->LoadGeometry(slot, NtText(sFilename), NtText(sGeomName), nLoadFlags);
}

int EntitySlotsInterop::LoadCharacter(IEntity *handle, int slot, mono::string sFilename, int nLoadFlags)
{
	return handle->LoadCharacter(slot, NtText(sFilename), nLoadFlags);
}

int EntitySlotsInterop::LoadGeomCache(IEntity *handle, int slot, mono::string sFilename)
{
	return handle->LoadGeomCache(slot, NtText(sFilename));
}

int EntitySlotsInterop::LoadParticleEmitterDefault(IEntity *handle, int slot, IParticleEffect *pEffect, bool bPrime, bool bSerialize)
{
	return handle->LoadParticleEmitter(slot, pEffect, nullptr, bPrime, bSerialize);
}

int EntitySlotsInterop::LoadParticleEmitter(IEntity *handle, int slot, IParticleEffect *pEffect, SpawnParams *parameters, bool bPrime, bool bSerialize)
{
	return handle->LoadParticleEmitter(slot, pEffect, parameters, bPrime, bSerialize);
}

int EntitySlotsInterop::SetParticleEmitter(IEntity *handle, int slot, IParticleEmitter *pEmitter, bool bSerialize)
{
	return handle->SetParticleEmitter(slot, pEmitter, bSerialize);
}

int EntitySlotsInterop::LoadLight(IEntity *handle, int slot, const LightProperties &pLight)
{
	CDLight light;
	pLight.ToCDLight(light);
	return handle->LoadLight(slot, &light);
}

int EntitySlotsInterop::GetSlotCount(IEntity *handle)
{
	return handle->GetSlotCount();
}
