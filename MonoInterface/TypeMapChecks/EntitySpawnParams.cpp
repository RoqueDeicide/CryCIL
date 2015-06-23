// This file is used to detect changes to the struct EntitySpawnParams at compile time.

#include "stdafx.h"

#include "CheckingBasics.h"

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