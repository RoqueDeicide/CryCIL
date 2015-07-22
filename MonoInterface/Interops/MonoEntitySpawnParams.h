#pragma once

#include "IMonoInterface.h"

struct MonoEntitySpawnParams
{
	EntityId          id;
	EntityId          prevId;
	EntityGUID        guid;
	EntityGUID        prevGuid;
	mono::string      pClass;
	IEntityArchetype *pArchetype;
	mono::string      layerName;
	IXmlNode         *entityNode;
	mono::string      name;
	uint64            flags;
	bool              ignoreLock;
	bool              staticEntityId;
	bool              createdThroughPool;
	Vec3              position;
	Quat              rotation;
	Vec3              scale;

	MonoEntitySpawnParams(SEntitySpawnParams params)
	{
		this->createdThroughPool = params.bCreatedThroughPool;
		this->entityNode         = params.entityNode;
		this->flags              = 0;
		this->flags             |= params.nFlags;
		this->flags             |= (uint64)(params.nFlagsExtended) << 32;
		this->guid               = params.guid;
		this->prevGuid           = params.prevGuid;
		this->id                 = params.id;
		this->prevId             = params.prevId;
		this->ignoreLock         = params.bIgnoreLock;
		this->layerName          = ToMonoString(params.sLayerName);
		this->name               = ToMonoString(params.sName);
		this->pArchetype         = params.pArchetype;
		this->pClass             = ToMonoString(params.pClass->GetName());
		this->position           = params.vPosition;
		this->rotation           = params.qRotation;
		this->scale              = params.vScale;
		this->staticEntityId     = params.bStaticEntityId;
	}
	SEntitySpawnParams ToNative()
	{
		SEntitySpawnParams params;
		params.bCreatedThroughPool = this->createdThroughPool;
		params.entityNode          = this->entityNode;
		params.nFlags              = (uint32)this->flags;
		params.nFlagsExtended      = (uint32)(this->flags >> 32);
		params.guid                = this->guid;
		params.prevGuid            = this->prevGuid;
		params.id                  = this->id;
		params.prevId              = this->prevId;
		params.bIgnoreLock         = this->ignoreLock;
		params.sLayerName          = ToNativeString(this->layerName);
		params.sName               = ToNativeString(this->name);
		params.pArchetype          = this->pArchetype;
		params.pClass              = gEnv->pEntitySystem->GetClassRegistry()->FindClass(NtText(this->pClass));
		params.vPosition           = this->position;
		params.qRotation           = this->rotation;
		params.vScale              = this->scale;
		params.bStaticEntityId     = this->staticEntityId;
	}
};