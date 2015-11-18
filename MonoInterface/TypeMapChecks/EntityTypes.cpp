#include "stdafx.h"

#include "CheckingBasics.h"

#include "IGameObject.h"

TYPE_MIRROR enum EntityAspects
{
	eEA_All_check = NET_ASPECT_ALL,
	// 0x01u                       // aspect 0
	eEA_Script_check = 0x02u, // aspect 1
	// 0x04u                       // aspect 2
	eEA_Physics_check = 0x08u, // aspect 3
	eEA_GameClientStatic_check = 0x10u, // aspect 4
	eEA_GameServerStatic_check = 0x20u, // aspect 5
	eEA_GameClientDynamic_check = 0x40u, // aspect 6
	eEA_GameServerDynamic_check = 0x80u, // aspect 7
	eEA_GameClientA_check = 0x0100u, // aspect 8
	eEA_GameServerA_check = 0x0200u, // aspect 9
	eEA_GameClientB_check = 0x0400u, // aspect 10
	eEA_GameServerB_check = 0x0800u, // aspect 11
	eEA_GameClientC_check = 0x1000u, // aspect 12
	eEA_GameServerC_check = 0x2000u, // aspect 13
	eEA_GameClientD_check = 0x4000u, // aspect 14
	eEA_GameClientE_check = 0x8000u, // aspect 15
	eEA_GameClientF_check = 0x00010000u, // aspect 16
	eEA_GameClientG_check = 0x00020000u, // aspect 17
	eEA_GameClientH_check = 0x00040000u, // aspect 18
	eEA_GameClientI_check = 0x00080000u, // aspect 19
	eEA_GameClientJ_check = 0x00100000u, // aspect 20
	eEA_GameServerD_check = 0x00200000u, // aspect 21
	eEA_GameClientK_check = 0x00400000u, // aspect 22
	eEA_GameClientL_check = 0x00800000u, // aspect 23
	eEA_GameClientM_check = 0x01000000u, // aspect 24
	eEA_GameClientN_check = 0x02000000u, // aspect 25
	eEA_GameClientO_check = 0x04000000u, // aspect 26
	eEA_GameClientP_check = 0x08000000u, // aspect 27
	eEA_GameServerE_check = 0x10000000u, // aspect 28
	eEA_Aspect29_check = 0x20000000u, // aspect 29
	eEA_Aspect30_check = 0x40000000u, // aspect 30
	eEA_Aspect31_check = 0x80000000u, // aspect 31
};

#define CHECK_ENUM(x) static_assert (EntityAspects::x ## _check == EEntityAspects::x, "EEntityAspects enumeration has been changed.")

inline void CheckEntityAspects()
{
	CHECK_ENUM(eEA_All);
	CHECK_ENUM(eEA_Script);
	CHECK_ENUM(eEA_Physics);
	CHECK_ENUM(eEA_GameClientStatic);
	CHECK_ENUM(eEA_GameServerStatic);
	CHECK_ENUM(eEA_GameClientDynamic);
	CHECK_ENUM(eEA_GameServerDynamic);
	CHECK_ENUM(eEA_GameClientA);
	CHECK_ENUM(eEA_GameServerA);
	CHECK_ENUM(eEA_GameClientB);
	CHECK_ENUM(eEA_GameServerB);
	CHECK_ENUM(eEA_GameClientC);
	CHECK_ENUM(eEA_GameServerC);
	CHECK_ENUM(eEA_GameClientD);
	CHECK_ENUM(eEA_GameClientE);
	CHECK_ENUM(eEA_GameClientF);
	CHECK_ENUM(eEA_GameClientG);
	CHECK_ENUM(eEA_GameClientH);
	CHECK_ENUM(eEA_GameClientI);
	CHECK_ENUM(eEA_GameClientJ);
	CHECK_ENUM(eEA_GameServerD);
	CHECK_ENUM(eEA_GameClientK);
	CHECK_ENUM(eEA_GameClientL);
	CHECK_ENUM(eEA_GameClientM);
	CHECK_ENUM(eEA_GameClientN);
	CHECK_ENUM(eEA_GameClientO);
	CHECK_ENUM(eEA_GameClientP);
	CHECK_ENUM(eEA_GameServerE);
	CHECK_ENUM(eEA_Aspect29);
	CHECK_ENUM(eEA_Aspect30);
	CHECK_ENUM(eEA_Aspect31);
}

#include <IEntityClass.h>

TYPE_MIRROR enum EntityClassFlags
{
	ECLF_INVISIBLE_check = BIT(0), // If set this class will not be visible in editor,and entity of this class cannot be placed manually in editor.
	ECLF_DEFAULT_check = BIT(1), // If this is default entity class.
	ECLF_BBOX_SELECTION_check = BIT(2), // If set entity of this class can be selected by bounding box in the editor 3D view.
	ECLF_DO_NOT_SPAWN_AS_STATIC_check = BIT(3), // If set the entity of this class stored as part of the level won't be assigned a static id on creation
	ECLF_MODIFY_EXISTING_check = BIT(4)  // If set modify an existing class with the same name.
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (EntityClassFlags::x ## _check == EEntityClassFlags::x, "EEntityClassFlags enumeration has been changed.")

inline void CheckEntityClassFlags()
{
	CHECK_ENUM(ECLF_INVISIBLE);
	CHECK_ENUM(ECLF_DEFAULT);
	CHECK_ENUM(ECLF_BBOX_SELECTION);
	CHECK_ENUM(ECLF_DO_NOT_SPAWN_AS_STATIC);
	CHECK_ENUM(ECLF_MODIFY_EXISTING);
}

TYPE_MIRROR enum EntityFlags
{
	//////////////////////////////////////////////////////////////////////////
	// Persistent flags (can be set from the editor).
	//////////////////////////////////////////////////////////////////////////

	ENTITY_FLAG_CASTSHADOW_check = BIT(1),
	ENTITY_FLAG_UNREMOVABLE_check = BIT(2),   // This entity cannot be removed using IEntitySystem::RemoveEntity until this flag is cleared.
	ENTITY_FLAG_GOOD_OCCLUDER_check = BIT(3),
	ENTITY_FLAG_NO_DECALNODE_DECALS_check = BIT(4),

	//////////////////////////////////////////////////////////////////////////
	ENTITY_FLAG_WRITE_ONLY_check = BIT(5),
	ENTITY_FLAG_NOT_REGISTER_IN_SECTORS_check = BIT(6),
	ENTITY_FLAG_CALC_PHYSICS_check = BIT(7),
	ENTITY_FLAG_CLIENT_ONLY_check = BIT(8),
	ENTITY_FLAG_SERVER_ONLY_check = BIT(9),
	ENTITY_FLAG_CUSTOM_VIEWDIST_RATIO_check = BIT(10),   // This entity have special custom view distance ratio (AI/Vehicles must have it).
	ENTITY_FLAG_CALCBBOX_USEALL_check = BIT(11),		// use character and objects in BBOx calculations.
	ENTITY_FLAG_VOLUME_SOUND_check = BIT(12),		// Entity is a volume sound (will get moved around by the sound proxy).
	ENTITY_FLAG_HAS_AI_check = BIT(13),		// Entity has an AI object.
	ENTITY_FLAG_TRIGGER_AREAS_check = BIT(14),   // This entity will trigger areas when it enters them.
	ENTITY_FLAG_NO_SAVE_check = BIT(15),   // This entity will not be saved.
	ENTITY_FLAG_CAMERA_SOURCE_check = BIT(16),   // This entity is a camera source.
	ENTITY_FLAG_CLIENTSIDE_STATE_check = BIT(17),   // Prevents error when state changes on the client and does not sync state changes to the client.
	ENTITY_FLAG_SEND_RENDER_EVENT_check = BIT(18),   // When set entity will send ENTITY_EVENT_RENDER every time its rendered.
	ENTITY_FLAG_NO_PROXIMITY_check = BIT(19),   // Entity will not be registered in the partition grid and can not be found by proximity queries.
	ENTITY_FLAG_PROCEDURAL_check = BIT(20),   // Entity has been generated at runtime.
	ENTITY_FLAG_UPDATE_HIDDEN_check = BIT(21),   // Entity will be update even when hidden.  
	ENTITY_FLAG_NEVER_NETWORK_STATIC_check = BIT(22),		// Entity should never be considered a static entity by the network system.
	ENTITY_FLAG_IGNORE_PHYSICS_UPDATE_check = BIT(23),		// Used by Editor only, (don't set).
	ENTITY_FLAG_SPAWNED_check = BIT(24),		// Entity was spawned dynamically without a class.
	ENTITY_FLAG_SLOTS_CHANGED_check = BIT(25),		// Entity's slots were changed dynamically.
	ENTITY_FLAG_MODIFIED_BY_PHYSICS_check = BIT(26),		// Entity was procedurally modified by physics.
	ENTITY_FLAG_OUTDOORONLY_check = BIT(27),		// Same as Brush->Outdoor only.
	ENTITY_FLAG_SEND_NOT_SEEN_TIMEOUT_check = BIT(28),		// Entity will be sent ENTITY_EVENT_NOT_SEEN_TIMEOUT if it is not rendered for 30 seconds.
	ENTITY_FLAG_RECVWIND_check = BIT(29),		// Receives wind.
	ENTITY_FLAG_LOCAL_PLAYER_check = BIT(30),
	ENTITY_FLAG_AI_HIDEABLE_check = BIT(31),  // AI can use the object to calculate automatic hide points.
};

enum EntityFlagsExtended
{
	ENTITY_FLAG_EXTENDED_AUDIO_LISTENER_check = BIT(0),
	ENTITY_FLAG_EXTENDED_NEEDS_MOVEINSIDE_check = BIT(1),
	ENTITY_FLAG_EXTENDED_CAN_COLLIDE_WITH_MERGED_MESHES_check = BIT(2),
};

#define CHECK_ENUM1(x) static_assert (EntityFlags::x ## _check == EEntityFlags::x, "EEntityFlags enumeration has been changed.")
#define CHECK_ENUM2(x) static_assert (EntityFlagsExtended::x ## _check == EEntityFlagsExtended::x, "EEntityFlagsExtended enumeration has been changed.")

inline void CheckEntityFlags()
{
	CHECK_ENUM1(ENTITY_FLAG_CASTSHADOW);
	CHECK_ENUM1(ENTITY_FLAG_UNREMOVABLE);
	CHECK_ENUM1(ENTITY_FLAG_GOOD_OCCLUDER);
	CHECK_ENUM1(ENTITY_FLAG_NO_DECALNODE_DECALS);

	CHECK_ENUM1(ENTITY_FLAG_WRITE_ONLY);
	CHECK_ENUM1(ENTITY_FLAG_NOT_REGISTER_IN_SECTORS);
	CHECK_ENUM1(ENTITY_FLAG_CALC_PHYSICS);
	CHECK_ENUM1(ENTITY_FLAG_CLIENT_ONLY);
	CHECK_ENUM1(ENTITY_FLAG_SERVER_ONLY);
	CHECK_ENUM1(ENTITY_FLAG_CUSTOM_VIEWDIST_RATIO);
	CHECK_ENUM1(ENTITY_FLAG_CALCBBOX_USEALL);
	CHECK_ENUM1(ENTITY_FLAG_VOLUME_SOUND);
	CHECK_ENUM1(ENTITY_FLAG_HAS_AI);
	CHECK_ENUM1(ENTITY_FLAG_TRIGGER_AREAS);
	CHECK_ENUM1(ENTITY_FLAG_NO_SAVE);
	CHECK_ENUM1(ENTITY_FLAG_CAMERA_SOURCE);
	CHECK_ENUM1(ENTITY_FLAG_CLIENTSIDE_STATE);
	CHECK_ENUM1(ENTITY_FLAG_SEND_RENDER_EVENT);
	CHECK_ENUM1(ENTITY_FLAG_NO_PROXIMITY);
	CHECK_ENUM1(ENTITY_FLAG_PROCEDURAL);
	CHECK_ENUM1(ENTITY_FLAG_UPDATE_HIDDEN);
	CHECK_ENUM1(ENTITY_FLAG_NEVER_NETWORK_STATIC);
	CHECK_ENUM1(ENTITY_FLAG_IGNORE_PHYSICS_UPDATE);
	CHECK_ENUM1(ENTITY_FLAG_SPAWNED);
	CHECK_ENUM1(ENTITY_FLAG_SLOTS_CHANGED);
	CHECK_ENUM1(ENTITY_FLAG_MODIFIED_BY_PHYSICS);
	CHECK_ENUM1(ENTITY_FLAG_OUTDOORONLY);
	CHECK_ENUM1(ENTITY_FLAG_SEND_NOT_SEEN_TIMEOUT);
	CHECK_ENUM1(ENTITY_FLAG_RECVWIND);
	CHECK_ENUM1(ENTITY_FLAG_LOCAL_PLAYER);
	CHECK_ENUM1(ENTITY_FLAG_AI_HIDEABLE);

	CHECK_ENUM2(ENTITY_FLAG_EXTENDED_AUDIO_LISTENER);
	CHECK_ENUM2(ENTITY_FLAG_EXTENDED_NEEDS_MOVEINSIDE);
	CHECK_ENUM2(ENTITY_FLAG_EXTENDED_CAN_COLLIDE_WITH_MERGED_MESHES);
}

TYPE_MIRROR enum _entity_query_flags
{
	ent_static_check = 1,
	ent_sleeping_rigid_check = 2,
	ent_rigid_check = 4,
	ent_living_check = 8,
	ent_independent_check = 16,
	ent_deleted_check = 128,
	ent_terrain_check = 0x100,
	ent_all_check = ent_static_check | ent_sleeping_rigid_check | ent_rigid_check | ent_living_check | ent_independent_check | ent_terrain_check,
	ent_flagged_only_check = pef_update,
	ent_skip_flagged_check = pef_update * 2, // "flagged" meas has pef_update set
	ent_areas_check = 32,
	ent_triggers_check = 64,
	ent_ignore_noncolliding_check = 0x10000,
	ent_sort_by_mass_check = 0x20000, // sort by mass in ascending order
	ent_allocate_list_check = 0x40000, // if not set, the function will return an internal pointer
	ent_addref_results_check = 0x100000, // will call AddRef on each entity in the list (expecting the caller call Release)
	ent_water_check = 0x200, // can only be used in RayWorldIntersection
	ent_no_ondemand_activation_check = 0x80000, // can only be used in RayWorldIntersection
	ent_delayed_deformations_check = 0x80000 // queues procedural breakage requests; can only be used in SimulateExplosion
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (_entity_query_flags::x ## _check == entity_query_flags::x, "entity_query_flags enumeration has been changed.")

inline void Check_entity_query_flags()
{
	CHECK_ENUM(ent_static);
	CHECK_ENUM(ent_sleeping_rigid);
	CHECK_ENUM(ent_rigid);
	CHECK_ENUM(ent_living);
	CHECK_ENUM(ent_independent);
	CHECK_ENUM(ent_deleted);
	CHECK_ENUM(ent_terrain);
	CHECK_ENUM(ent_all);
	CHECK_ENUM(ent_flagged_only);
	CHECK_ENUM(ent_skip_flagged);
	CHECK_ENUM(ent_areas);
	CHECK_ENUM(ent_triggers);
	CHECK_ENUM(ent_ignore_noncolliding);
	CHECK_ENUM(ent_sort_by_mass);
	CHECK_ENUM(ent_allocate_list);
	CHECK_ENUM(ent_addref_results);
	CHECK_ENUM(ent_water);
	CHECK_ENUM(ent_no_ondemand_activation);
	CHECK_ENUM(ent_delayed_deformations);
}

TYPE_MIRROR struct EntitySlotInfo
{
	// Slot flags.
	int nFlags;
	// Index of parent slot, (-1 if no parent)
	int nParentSlot;
	// Hide mask used by breakable object to indicate what index of the CStatObj sub-object is hidden.
	uint64 nSubObjHideMask;
	// Slot local transformation matrix.
	const Matrix34 *pLocalTM;
	// Slot world transformation matrix.
	const Matrix34 *pWorldTM;
	// Objects that can binded to the slot.
	EntityId                   entityId;
	struct IStatObj*           pStatObj;
	struct ICharacterInstance*   pCharacter;
	struct IParticleEmitter*   pParticleEmitter;
	struct ILightSource*      pLight;
	struct IRenderNode*      pChildRenderNode;
	struct IGeomCacheRenderNode* pGeomCacheRenderNode;
	// Custom Material used for the slot.
	IMaterial* pMaterial;

	explicit EntitySlotInfo(SEntitySlotInfo other)
	{
		CHECK_TYPE_SIZE(EntitySlotInfo);

		ASSIGN_FIELD(nFlags);
		ASSIGN_FIELD(nParentSlot);
		ASSIGN_FIELD(nSubObjHideMask);
		ASSIGN_FIELD(pLocalTM);
		ASSIGN_FIELD(pWorldTM);
		ASSIGN_FIELD(entityId);
		ASSIGN_FIELD(pStatObj);
		ASSIGN_FIELD(pCharacter);
		ASSIGN_FIELD(pParticleEmitter);
		ASSIGN_FIELD(pLight);
		ASSIGN_FIELD(pChildRenderNode);
		ASSIGN_FIELD(pGeomCacheRenderNode);
		ASSIGN_FIELD(pMaterial);

		CHECK_TYPE(nFlags);
		CHECK_TYPE(nParentSlot);
		CHECK_TYPE(nSubObjHideMask);
		CHECK_TYPE(pLocalTM);
		CHECK_TYPE(pWorldTM);
		CHECK_TYPE(entityId);
		CHECK_TYPE(pStatObj);
		CHECK_TYPE(pCharacter);
		CHECK_TYPE(pParticleEmitter);
		CHECK_TYPE(pLight);
		CHECK_TYPE(pChildRenderNode);
		CHECK_TYPE(pGeomCacheRenderNode);
		CHECK_TYPE(pMaterial);
	}
};

TYPE_MIRROR struct EntitySpawnParams
{
	// The Entity unique identifier (EntityId). If 0 then an ID will be generated
	// automatically (based on the bStaticEntityId parameter).
	EntityId      id;
	EntityId      prevId; // Previously used entityId, in the case of reloading

	// Optional entity guid.
	EntityGUID    guid;
	EntityGUID    prevGuid; // Previously used entityGuid, in the case of reloading

	// Class of entity.
	IEntityClass* pClass;

	/// Entity archetype.
	IEntityArchetype *pArchetype;

	// The name of the layer the entity resides in, when in the Editor.
	const char *sLayerName;

	// Reference to entity's xml node in level data
	XmlNodeRef entityNode;

	//////////////////////////////////////////////////////////////////////////
	// Initial Entity parameters.
	//////////////////////////////////////////////////////////////////////////

	// Note:
	//	 The name of the entity... does not need to be unique.
	const char*   sName;

	// Entity Flags.
	uint32        nFlags;				// e.g. ENTITY_FLAG_CASTSHADOW

	// EEntityFlagsExtended flags.
	uint32        nFlagsExtended;

	// Spawn lock.
	bool					bIgnoreLock;
	// Note:
	//	 To support save games compatible with patched levels (patched levels might use
	//   more EntityIDs and save game might conflict with dynamic ones).
	bool					bStaticEntityId;
	// Entity Pool usage.
	bool					bCreatedThroughPool;
	// Initial entity position (Local space).
	Vec3          vPosition;
	// Initial entity rotation (Local space).
	Quat          qRotation;
	// Initial entity scale (Local space).
	Vec3          vScale;
	// Any user defined data. It will be available for container when it will be created.
	void*         pUserData;
	//////////////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////
	// Optional properties table.
	//////////////////////////////////////////////////////////////////////////
	IScriptTable *pPropertiesTable;
	IScriptTable *pPropertiesInstanceTable;
	//////////////////////////////////////////////////////////////////////////


	EntitySpawnParams(SEntitySpawnParams other)
	{
		static_assert(sizeof(EntitySpawnParams) == sizeof(SEntitySpawnParams), "SEntitySpawnParams has been changed.");

		this->id = other.id;
		this->prevId = other.prevId;
		this->guid = other.guid;
		this->prevGuid = other.prevGuid;
		this->pClass = other.pClass;
		this->pArchetype = other.pArchetype;
		this->sLayerName = other.sLayerName;
		this->entityNode = other.entityNode;
		this->sName = other.sName;
		this->nFlags = other.nFlags;
		this->nFlagsExtended = other.nFlagsExtended;
		this->bIgnoreLock = other.bIgnoreLock;
		this->bStaticEntityId = other.bStaticEntityId;
		this->bCreatedThroughPool = other.bCreatedThroughPool;
		this->vPosition = other.vPosition;
		this->qRotation = other.qRotation;
		this->vScale = other.vScale;
		this->pUserData = other.pUserData;
		this->pPropertiesTable = other.pPropertiesTable;
		this->pPropertiesInstanceTable = other.pPropertiesInstanceTable;

		CHECK_TYPE(id);
		CHECK_TYPE(prevId);
		CHECK_TYPE(guid);
		CHECK_TYPE(prevGuid);
		CHECK_TYPE(pClass);
		CHECK_TYPE(pArchetype);
		CHECK_TYPE(sLayerName);
		CHECK_TYPE(entityNode);
		CHECK_TYPE(sName);
		CHECK_TYPE(nFlags);
		CHECK_TYPE(nFlagsExtended);
		CHECK_TYPE(bIgnoreLock);
		CHECK_TYPE(bStaticEntityId);
		CHECK_TYPE(bCreatedThroughPool);
		CHECK_TYPE(vPosition);
		CHECK_TYPE(qRotation);
		CHECK_TYPE(vScale);
		CHECK_TYPE(pUserData);
		CHECK_TYPE(pPropertiesTable);
		CHECK_TYPE(pPropertiesInstanceTable);
	}
};

TYPE_MIRROR struct EntityUpdateContext
{
	// Current rendering frame id.
	int nFrameID;
	// Current camera.
	CCamera *pCamera;
	// Current system time.
	float fCurrTime;
	// Delta frame time (of last frame).
	float fFrameTime;
	// Indicates if a profile entity must update the log.
	bool bProfileToLog;
	// Number of updated entities.
	int numUpdatedEntities;
	// Number of visible and updated entities.
	int numVisibleEntities;
	// Maximal view distance.
	float fMaxViewDist;
	// Maximal view distance squared.
	float fMaxViewDistSquared;
	// Camera source position.
	Vec3 vCameraPos;
	EntityUpdateContext(SEntityUpdateContext other)
	{
		this->nFrameID = other.nFrameID;
		this->pCamera = other.pCamera;
		this->fCurrTime = other.fCurrTime;
		this->fFrameTime = other.fFrameTime;
		this->bProfileToLog = other.bProfileToLog;
		this->numUpdatedEntities = other.numUpdatedEntities;
		this->numVisibleEntities = other.numVisibleEntities;
		this->fMaxViewDist = other.fMaxViewDist;
		this->fMaxViewDistSquared = other.fMaxViewDistSquared;
		this->vCameraPos = other.vCameraPos;

		CHECK_TYPE(nFrameID);
		CHECK_TYPE(pCamera);
		CHECK_TYPE(fCurrTime);
		CHECK_TYPE(fFrameTime);
		CHECK_TYPE(bProfileToLog);
		CHECK_TYPE(numUpdatedEntities);
		CHECK_TYPE(numVisibleEntities);
		CHECK_TYPE(fMaxViewDist);
		CHECK_TYPE(fMaxViewDistSquared);
		CHECK_TYPE(vCameraPos);
	}
};

enum EntityUpdatePolicy
{
	ENTITY_UPDATE_NEVER_check,			// Never update entity every frame.
	ENTITY_UPDATE_IN_RANGE_check,			// Only update entity if it is in specified range from active camera.
	ENTITY_UPDATE_POT_VISIBLE_check,		// Only update entity if it is potentially visible.
	ENTITY_UPDATE_VISIBLE_check,			// Only update entity if it is visible.
	ENTITY_UPDATE_PHYSICS_check,			// Only update entity if it is need to be updated due to physics.
	ENTITY_UPDATE_PHYSICS_VISIBLE_check,	// Only update entity if it is need to be updated due to physics or if it is visible.
	ENTITY_UPDATE_ALWAYS_check,			// Always update entity every frame.
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (EntityUpdatePolicy::x ## _check == EEntityUpdatePolicy::x, "EEntityUpdatePolicy enumeration has been changed.")

inline void CheckEntityUpdatePolicy()
{
	CHECK_ENUM(ENTITY_UPDATE_NEVER);
	CHECK_ENUM(ENTITY_UPDATE_IN_RANGE);
	CHECK_ENUM(ENTITY_UPDATE_POT_VISIBLE);
	CHECK_ENUM(ENTITY_UPDATE_VISIBLE);
	CHECK_ENUM(ENTITY_UPDATE_PHYSICS);
	CHECK_ENUM(ENTITY_UPDATE_PHYSICS_VISIBLE);
	CHECK_ENUM(ENTITY_UPDATE_ALWAYS);
}

TYPE_MIRROR enum EntityXFormFlags
{
	ENTITY_XFORM_POS_check = BIT(1),
	ENTITY_XFORM_ROT_check = BIT(2),
	ENTITY_XFORM_SCL_check = BIT(3),
	ENTITY_XFORM_NO_PROPOGATE_check = BIT(4),
	ENTITY_XFORM_FROM_PARENT_check = BIT(5),  // When parent changes his transformation.
	ENTITY_XFORM_PHYSICS_STEP_check = BIT(13),
	ENTITY_XFORM_EDITOR_check = BIT(14),
	ENTITY_XFORM_TRACKVIEW_check = BIT(15),
	ENTITY_XFORM_TIMEDEMO_check = BIT(16),
	ENTITY_XFORM_NOT_REREGISTER_check = BIT(17), // An optimization flag, when set object will not be re-registered in 3D engine.
	ENTITY_XFORM_NO_EVENT_check = BIT(18), // suppresses ENTITY_EVENT_XFORM event
	ENTITY_XFORM_NO_SEND_TO_ENTITY_SYSTEM_check = BIT(19),
	ENTITY_XFORM_USER_check = 0x1000000,
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (EntityXFormFlags::x ## _check == EEntityXFormFlags::x, "EEntityXFormFlags enumeration has been changed.")

inline void CheckEntityXFormFlags()
{
	CHECK_ENUM(ENTITY_XFORM_POS);
	CHECK_ENUM(ENTITY_XFORM_ROT);
	CHECK_ENUM(ENTITY_XFORM_SCL);
	CHECK_ENUM(ENTITY_XFORM_NO_PROPOGATE);
	CHECK_ENUM(ENTITY_XFORM_FROM_PARENT);
	CHECK_ENUM(ENTITY_XFORM_PHYSICS_STEP);
	CHECK_ENUM(ENTITY_XFORM_EDITOR);
	CHECK_ENUM(ENTITY_XFORM_TRACKVIEW);
	CHECK_ENUM(ENTITY_XFORM_TIMEDEMO);
	CHECK_ENUM(ENTITY_XFORM_NOT_REREGISTER);
	CHECK_ENUM(ENTITY_XFORM_NO_EVENT);
	CHECK_ENUM(ENTITY_XFORM_NO_SEND_TO_ENTITY_SYSTEM);
	CHECK_ENUM(ENTITY_XFORM_USER);
}

TYPE_MIRROR enum RMInvocation
{
	eRMI_ToClientChannel_check = 0x01,
	eRMI_ToOwnClient_check = 0x02,
	eRMI_ToOtherClients_check = 0x04,
	eRMI_ToAllClients_check = 0x08,

	eRMI_ToServer_check = 0x100,

	eRMI_NoLocalCalls_check = 0x10000,
	eRMI_NoRemoteCalls_check = 0x20000,

	eRMI_ToRemoteClients_check = eRMI_NoLocalCalls_check | eRMI_ToAllClients_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (RMInvocation::x ## _check == ERMInvocation::x, "ERMInvocation enumeration has been changed.")

inline void CheckRMInvocation()
{
	CHECK_ENUM(eRMI_ToClientChannel);
	CHECK_ENUM(eRMI_ToOwnClient);
	CHECK_ENUM(eRMI_ToOtherClients);
	CHECK_ENUM(eRMI_ToAllClients);

	CHECK_ENUM(eRMI_ToServer);

	CHECK_ENUM(eRMI_NoLocalCalls);
	CHECK_ENUM(eRMI_NoRemoteCalls);

	CHECK_ENUM(eRMI_ToRemoteClients);
}

TYPE_MIRROR struct PropertyInfo
{
	const char* name;			// Name of the property.
	IEntityPropertyHandler::EPropertyType type;			// Type of the property value.
	const char* editType;		// Type of edit control to use.
	const char* description;	// Description of the property.
	uint32 flags;				// Property flags.

	struct SLimits				// Limits
	{
		float min;
		float max;
	} limits;

	PropertyInfo(IEntityPropertyHandler::SPropertyInfo other)
	{
		static_assert(sizeof(PropertyInfo) == sizeof(IEntityPropertyHandler::SPropertyInfo),
					  "IEntityPropertyHandler::SPropertyInfo has been changed.");

		this->name = other.name;
		this->type = other.type;
		this->editType = other.editType;
		this->description = other.description;
		this->flags = other.flags;
		this->limits.min = other.limits.min;
		this->limits.max = other.limits.max;

		CHECK_TYPE(name);
		CHECK_TYPE(type);
		CHECK_TYPE(editType);
		CHECK_TYPE(description);
		CHECK_TYPE(flags);
		CHECK_TYPE(limits.min);
		CHECK_TYPE(limits.max);
	}
};