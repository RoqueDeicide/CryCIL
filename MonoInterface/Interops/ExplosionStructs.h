#pragma once

#include "IMonoInterface.h"

struct ExplosionParameters
{
	Vec3 occlusionEpicenter;
	Vec3 impulseEpicenter;
	float minimalRadius;
	float maximalRadius;
	float radius;
	float impulsivePressureAtRadius;
	int occlusionResolution;
	int grow;
	float minimalOcclusionRadius;
	float holeSize;
	Vec3 explodingDirection;
	int holeType;
	bool forceDeformEntities;

	void ToExplosion(pe_explosion &e) const
	{
		e.epicenter            = this->occlusionEpicenter;
		e.epicenterImp         = this->impulseEpicenter;
		e.rmin                 = this->minimalRadius;
		e.explDir              = this->explodingDirection;
		e.forceDeformEntities  = this->forceDeformEntities;
		e.holeSize             = this->holeSize;
		e.iholeType            = this->holeType;
		e.impulsivePressureAtR = this->impulsivePressureAtRadius;
		e.nGrow                = this->grow;
		e.nOccRes              = this->occlusionResolution;
		e.r                    = this->radius;
		e.rmax                 = this->maximalRadius;
		e.rminOcc              = this->minimalOcclusionRadius;
	}
};

struct ExplodedEntity
{
	IPhysicalEntity *entity;
	float exposure;
};

struct ExplosionResult
{
	mono::Array affectedEntities;
	void FromExplosion(const pe_explosion &e, MonoGCHandle &handle/* a pinning handle that will be released at the end of the parent call. */)
	{
		if (e.nAffectedEnts == 0)
		{
			this->affectedEntities = nullptr;
			return;
		}
		this->affectedEntities = MonoEnv->Objects->Arrays->Create(e.nAffectedEnts, MonoEnv->Cryambly->GetClass("CryCil.Engine.Physics", "ExplodedEntity"));
		handle = MonoEnv->GC->Pin(this->affectedEntities);
		IMonoArray<ExplodedEntity> entities = this->affectedEntities;
		for (int i = 0; i < e.nAffectedEnts; i++)
		{
			entities[i].entity   = e.pAffectedEnts[i];
			entities[i].exposure = e.pAffectedEntsExposure[i];
		}
	}
};