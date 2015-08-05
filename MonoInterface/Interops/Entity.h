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

	static void         SetFlags(IEntity *handle, uint64 flags);
	static uint64       GetFlags(IEntity *handle);
	static void         AddFlagsInternal(IEntity *handle, uint64 flagsToAdd);
	static void         ClearFlagsInternal(IEntity *handle, uint64 flagsToClear);
	static bool         CheckFlagsInternal(IEntity *handle, uint64 flagsToCheck, bool all);
	static bool         GetIsGarbage(IEntity *handle);
	static void         SetNameInternal(IEntity *handle, mono::string sName);
	static mono::string GetNameInternal(IEntity *handle);
	static bool         GetIsLoadedFromLevelFile(IEntity *handle);
	static bool         GetIsFromPool(IEntity *handle);
	static void         AttachChildInternal(IEntity *handle, IEntity *child, int flags, mono::string target);
	static void         DetachAllInternal(IEntity *handle, bool keepWorldTM);
	static void         DetachThisInternal(IEntity *handle, bool keepWorldTM);
	static int          GetChildCount(IEntity *handle);
	static IEntity     *GetChildInternal(IEntity *handle, int nIndex);
	static IEntity     *GetParent(IEntity *handle);
	static Matrix34     GetParentAttachPointWorldTM(IEntity *handle);
	static bool         GetIsParentAttachmentValid(IEntity *handle);
	static void         SetWorldTM(IEntity *handle, Matrix34 *tm);
	static void         SetLocalTM(IEntity *handle, Matrix34 *tm);
	static Matrix34     GetWorldTM(IEntity *handle);
	static Matrix34     GetLocalTM(IEntity *handle);
	static void         GetWorldBounds(IEntity *handle, AABB *bbox);
	static void         GetLocalBounds(IEntity *handle, AABB *bbox);
	static void         SetPos(IEntity *handle, Vec3 *vPos, bool bRecalcPhyBounds);
	static Vec3         GetPos(IEntity *handle);
	static void         SetRotation(IEntity *handle, Quat *qRotation);
	static Quat         GetRotation(IEntity *handle);
	static void         SetScale(IEntity *handle, Vec3 *vScale);
	static Vec3         GetScale(IEntity *handle);
	static void         GetPosRotScale(IEntity *handle, Vec3 *pos, Quat *rotation, Vec3 *scale);
	static void         SetPosRotScale(IEntity *handle, Vec3 *pos, Quat *rotation, Vec3 *scale);
	static Vec3         GetWorldPos(IEntity *handle);
	static Ang3         GetWorldAngles(IEntity *handle);
	static Quat         GetWorldRotation(IEntity *handle);
	static Vec3         GetForwardDir(IEntity *handle);
	static void         Activate(IEntity *handle, bool bActive);
	static bool         IsActive(IEntity *handle);
	static void         PrePhysicsActivate(IEntity *handle, bool bActive);
	static bool         IsPrePhysicsActive(IEntity *handle);
	static void         SetTimerInternal(IEntity *handle, int nTimerId, int nMilliSeconds);
	static void         KillTimerInternal(IEntity *handle, int nTimerId);
	static void         Hide(IEntity *handle, bool bHide);
	static bool         IsHidden(IEntity *handle);
	static void         MakeInvisible(IEntity *handle, bool bInvisible);
	static bool         IsInvisible(IEntity *handle);
	static void         SetUpdatePolicy(IEntity *handle, int eUpdatePolicy);
	static int          GetUpdatePolicy(IEntity *handle);
	static void         SetMaterial(IEntity *handle, IMaterial *pMaterial);
	static IMaterial   *GetMaterial(IEntity *handle);
	static IEntityLink *GetEntityLinks(IEntity *handle);
	static IEntityLink *AddEntityLink(IEntity *handle, mono::string linkName, EntityId entityId, EntityGUID entityGuid);
	static void         RemoveEntityLink(IEntity *handle, IEntityLink *pLink);
	static void         RemoveAllEntityLinks(IEntity *handle);
	static IEntityLink *GetNextLink(IEntityLink *linkHandle);
	static mono::string GetLinkName(IEntityLink *linkHandle);
	static EntityId     GetLinkedEntityId(IEntityLink *linkHandle);
	static EntityGUID   GetLinkedEntityGuid(IEntityLink *linkHandle);
};

struct EntitySlotsInterop : IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "EntitySlotOps"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Logic"; }

	virtual void OnRunTimeInitialized() override;

	static bool         IsSlotValid(IEntity *entityHandle, int nIndex);
	static void         FreeSlot(IEntity *entityHandle, int nIndex);
	static bool         GetSlotInfo(IEntity *entityHandle, int nIndex, SEntitySlotInfo *slotInfo);
	static void         GetSlotWorldTM(IEntity *entityHandle, int slot, Matrix34 *matrix);
	static void         GetSlotLocalTM(IEntity *entityHandle, int slot, bool bRelativeToParent, Matrix34 *matrix);
	static void         SetSlotLocalTM(IEntity *entityHandle, int slot, Matrix34 *localTM);
	static void         SetSlotCameraSpacePos(IEntity *entityHandle, int slot, Vec3 *cameraSpacePos);
	static void         GetSlotCameraSpacePos(IEntity *entityHandle, int slot, Vec3 *cameraSpacePos);
	static bool         SetParentSlot(IEntity *entityHandle, int nParentIndex, int nChildIndex);
	static void         SetSlotMaterial(IEntity *entityHandle, int slot, IMaterial *pMaterial);
	static void         SetSlotFlags(IEntity *entityHandle, int slot, int nFlags);
	static int          GetSlotFlags(IEntity *entityHandle, int slot);
	static bool         ShouldUpdateCharacter(IEntity *entityHandle, int slot);
	static ICharacterInstance *GetCharacter(IEntity *entityHandle, int slot);
	static int          SetCharacter(IEntity *entityHandle, ICharacterInstance *pCharacter, int slot);
	static IStatObj    *GetStatObj(IEntity *entityHandle, int slot);
	static IParticleEmitter *GetParticleEmitter(IEntity *entityHandle, int slot);
	static IGeomCacheRenderNode *GetGeomCacheRenderNode(IEntity *entityHandle, int slot);
	static void         MoveSlot(IEntity *entityHandle, IEntity *targetIEnt, int slot);
	static int          SetStatObj(IEntity *entityHandle, IStatObj *pStatObj, int slot, bool bUpdatePhysics, float mass);
	static int          LoadGeometry(IEntity *entityHandle, int slot, mono::string sFilename, mono::string sGeomName, int nLoadFlags);
	static int          LoadCharacter(IEntity *entityHandle, int slot, mono::string sFilename, int nLoadFlags);
	static int          LoadGeomCache(IEntity *entityHandle, int slot, mono::string sFilename);
	static int          LoadParticleEmitterDefault(IEntity *entityHandle, int slot, IParticleEffect *pEffect, bool bPrime, bool bSerialize);
	static int          LoadParticleEmitter(IEntity *entityHandle, int slot, IParticleEffect *pEffect, SpawnParams *parameters,
											bool bPrime, bool bSerialize);
	static int          SetParticleEmitter(IEntity *entityHandle, int slot, IParticleEmitter *pEmitter, bool bSerialize);
	static int          LoadLight(IEntity *entityHandle, int slot, CDLight *pLight);
	static int          GetSlotCount(IEntity *entityHandle);
};