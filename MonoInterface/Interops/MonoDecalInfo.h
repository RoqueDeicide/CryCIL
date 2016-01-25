#pragma once

#include "IMonoInterface.h"

struct MonoDecalInfo
{
	IRenderNode *owner;
	Vec3 position;
	Vec3 normal;
	float size;
	float lifeTime;
	float angle;
	IStatObj *staticObject;
	Vec3 hitDirection;
	float growTime, growTimeAlpha;
	uint32 groupId;
	bool skipOverlappingTest;
	bool assemble;
	bool forceEdge;
	bool forceSingleOwner;
	bool deferred;
	byte sortPriority;
	mono::string materialName;
	bool preventDecalOnGround;

	void Export(CryEngineDecalInfo &info) const
	{
		info.ownerInfo.pRenderNode = this->owner;
		info.vPos = this->position;
		info.vNormal = this->normal;
		info.fSize = this->size;
		info.fLifeTime = this->lifeTime;
		info.fAngle = this->angle;
		info.pIStatObj = this->staticObject;
		info.vHitDirection = this->hitDirection;
		info.fGrowTime = this->growTime;
		info.fGrowTimeAlpha = this->growTimeAlpha;
		info.nGroupId = this->groupId;
		info.bSkipOverlappingTest = this->skipOverlappingTest;
		info.bAssemble = this->assemble;
		info.bForceEdge = this->forceEdge;
		info.bForceSingleOwner = this->forceSingleOwner;
		info.bDeferred = this->deferred;
		info.sortPrio = this->sortPriority;
		NtText name = NtText(this->materialName);
		if (name)
		{
			memcpy(info.szMaterialName, static_cast<const char *>(name), name.Length);
		}
		info.preventDecalOnGround = this->preventDecalOnGround;
	}
};