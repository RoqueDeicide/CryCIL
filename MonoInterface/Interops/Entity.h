#pragma once

#include "IMonoInterface.h"
#include "IEntitySystem.h"
#include "IEntityPoolManager.h"

struct MonoEntitySpawnParams;

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

struct EntitySystemInterop : public IMonoInterop<false, true>
{
private:
	static List<NtText> monoEntityClassNames;		//!< A list of registered classes of entities that interact with CryCIL.
public:
	virtual const char *GetName() override { return "EntitySystem"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Logic"; }

	static bool            IsMonoEntity(const char *className);
	static IEntityProxyPtr CreateGameObjectForCryCilEntity(IEntity *pEntity, SEntitySpawnParams &params, void *pUserData);

	virtual void OnRunTimeInitialized() override;

	static bool RegisterEntityClass(mono::string name, mono::string category, mono::string editorHelper,
									mono::string editorIcon, enum EEntityClassFlags flags, mono::object properties,
									bool networked, bool dontSyncProps);

	static void         RemoveEntity(EntityId id, bool now);
	static mono::object SpawnMonoEntity(MonoEntitySpawnParams parameters);
	static mono::object SpawnNetEntity(MonoEntitySpawnParams parameters, ushort channelId);
	static IEntity     *SpawnCryEntity(MonoEntitySpawnParams parameters);
};

struct NetEntityInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "MonoNetEntity"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Logic"; }

	virtual void OnRunTimeInitialized() override;

	static void SetChannelId(EntityId entityId, ushort channelId);
	static void InvokeRmi(EntityId sender, mono::string methodName, mono::object parameters, uint32 _where, int channel,
						  int rmiType);
	static void ChangeNetworkStateInternal(EntityId id, uint32 aspects);
};

struct CryEntityInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() override { return "CryEntity"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Logic"; }

	virtual void OnRunTimeInitialized() override;

	static void SetFlags(IEntity *handle, uint64 flags);
	static uint64 GetFlags(IEntity *handle);
	static void AddFlagsInternal(IEntity *handle, uint64 flagsToAdd);
	static void ClearFlagsInternal(IEntity *handle, uint64 flagsToClear);
	static bool CheckFlagsInternal(IEntity *handle, uint64 flagsToCheck, bool all);
};