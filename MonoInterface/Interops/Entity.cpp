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
