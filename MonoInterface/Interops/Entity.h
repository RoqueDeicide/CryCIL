#pragma once

#include "IMonoInterface.h"
#include "IEntitySystem.h"
#include "IEntityPoolManager.h"

struct EntityIdInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "EntityId"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Logic"; }

	virtual void OnRunTimeInitialized() override;

	static IEntity     *GetEntity(uint id);
	static bool         PrepareInternal(EntityId id, bool prepareNow);
	static bool         ReturnInternal(EntityId id, bool saveState = true);
	static void         ResetBookmarkInternal(EntityId id);
	static bool         IsEntityBookmarkedInternal(EntityId entityId);
	static mono::string GetBookmarkedClassNameInternal(EntityId entityId);
	static mono::string GetBookmarkedEntityNameInternal(EntityId entityId);
};

struct EntityPoolInterop : public IMonoInterop<false, true>, public IEntityPoolListener
{
	virtual const char *GetName() override { return "CryEntityPool"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Logic"; }
	IMonoClass *GetMonoClass();

	virtual void OnRunTimeInitialized() override;

	virtual void OnPoolBookmarkCreated(EntityId entityId, const SEntitySpawnParams& params, XmlNodeRef entityNode) override;
	virtual void OnEntityPreparedFromPool(EntityId entityId, IEntity *pEntity) override;
	virtual void OnEntityReturningToPool(EntityId entityId, IEntity *pEntity) override;
	virtual void OnEntityReturnedToPool(EntityId entityId, IEntity *pEntity) override;
	virtual void OnPoolDefinitionsLoaded(size_t numAI) override;
	virtual void OnBookmarkEntitySerialize(TSerialize serialize, void *pVEntity) override;

	static void Enable();
	static void Disable();
	static void ResetPools(bool saveState);
	static bool IsDefaultBookmarked(mono::string className);
	static bool IsPreparingEntity(EntityId *entityId);
};
