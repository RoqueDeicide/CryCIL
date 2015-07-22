#include "stdafx.h"

#include "Entity.h"
#include "EntityClass.h"
#include "EntityExtension.h"

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
	return MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName());
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
	static OnPoolBookmarkCreatedThunk thunk = (OnPoolBookmarkCreatedThunk)
		this->GetMonoClass()->GetFunction("OnPoolBookmarkCreated", 3)->UnmanagedThunk;

	mono::exception ex;
	thunk(entityId, params, entityNode, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnEntityPreparedFromPool(EntityId entityId, IEntity *pEntity)
{
	static PoolMigrationHandlerThunk thunk = (PoolMigrationHandlerThunk)
		this->GetMonoClass()->GetFunction("OnEntityPrepared", 2)->UnmanagedThunk;

	mono::exception ex;
	thunk(entityId, pEntity, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnEntityReturningToPool(EntityId entityId, IEntity *pEntity)
{
	static PoolMigrationHandlerThunk thunk = (PoolMigrationHandlerThunk)
		this->GetMonoClass()->GetFunction("OnEntityReturning", 2)->UnmanagedThunk;

	mono::exception ex;
	thunk(entityId, pEntity, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnEntityReturnedToPool(EntityId entityId, IEntity *pEntity)
{
	static PoolMigrationHandlerThunk thunk = (PoolMigrationHandlerThunk)
		this->GetMonoClass()->GetFunction("OnEntityReturned", 2)->UnmanagedThunk;

	mono::exception ex;
	thunk(entityId, pEntity, &ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnPoolDefinitionsLoaded(size_t numAI)
{
	static OnPoolDefinitionsLoadedThunk thunk = (OnPoolDefinitionsLoadedThunk)
		this->GetMonoClass()->GetFunction("OnDefinitionsLoaded", 0)->UnmanagedThunk;

	mono::exception ex;
	thunk(&ex);
	if (ex)
	{
		MonoEnv->HandleException(ex);
	}
}

void EntityPoolInterop::OnBookmarkEntitySerialize(TSerialize serialize, void *pVEntity)
{
	static PoolBookmarkSyncHandlerThunk thunk = (PoolBookmarkSyncHandlerThunk)
		this->GetMonoClass()->GetFunction("OnBookmarkSyncing", 2)->UnmanagedThunk;

	mono::exception ex;
	thunk(*(ISerialize **)&serialize, (IEntity *)pVEntity, &ex);
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
	return monoEntityClassNames.Find([className](NtText &name) { name.Equals(className); });
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
	virtual IGameObjectExtensionPtr Create()
	{
		return ComponentCreate_DeleteWithRelease<MonoEntityExtension>();
	}
	virtual void GetGameObjectExtensionRMIData(void **ppRMI, size_t *nCount)
	{
		return MonoEntityExtension::GetGameObjectExtensionRMIData(ppRMI, nCount);
	}
};

bool _true = true;		//!< Used when registering new entity classes to avoid heap-allocating new boolean values.
bool _false = false;	//!< Used when registering new entity classes to avoid heap-allocating new boolean values.

bool EntitySystemInterop::RegisterEntityClass(mono::string name, mono::string category, mono::string editorHelper,
										mono::string editorIcon, EEntityClassFlags flags, mono::Array properties,
										bool networked)
{
	const char *className = ToNativeString(name);

	auto registry = gEnv->pEntitySystem->GetClassRegistry();

	auto nameMatch =
		[className](NtText &name)
		{
			return name.Equals(className);
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
	description.pUserProxyData = networked ? &_true : &_false;

	registry->RegisterStdClass(description);
	static MonoEntityCreator creator;
	MonoEnv->CryAction->GetIGameObjectSystem()->RegisterExtension(className, &creator, nullptr);
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
	if (!*(bool *)entityClass->GetUserProxyData())
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
}

void NetEntityInterop::SetChannelId(EntityId entityId, ushort channelId)
{
	IGameObject *gameObject = MonoEnv->CryAction->GetGameObject(entityId);

	if (gameObject)
	{
		gameObject->SetChannelId(channelId);
	}
}
