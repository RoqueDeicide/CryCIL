#pragma once
#include "PhysicsParameterStructs.h"
#include "NtText.h"

struct AreaDefinition
{
	int areaType;
	float fRadius;
	Vec3 boxmin, boxmax;
	Vec3* points;
	int pointsCount;
	float zmin, zmax;
	Vec3 center;
	Vec3 axis;
	PhysicsParametersArea* pGravityParams;
	SEntityPhysicalizeParams::AreaDefinition *ToNativeObject() const
	{
		SEntityPhysicalizeParams::AreaDefinition *areaDef = new SEntityPhysicalizeParams::AreaDefinition();

		areaDef->areaType       = SEntityPhysicalizeParams::AreaDefinition::EAreaType(this->areaType);
		areaDef->fRadius        = this->fRadius;
		areaDef->boxmin         = this->boxmin;
		areaDef->boxmax         = this->boxmax;
		areaDef->pPoints        = this->points;
		areaDef->nNumPoints     = this->pointsCount;
		areaDef->zmin           = this->zmin;
		areaDef->zmax           = this->zmax;
		areaDef->center         = this->center;
		areaDef->axis           = this->axis;
		areaDef->pGravityParams = static_cast<pe_params_area *>
			(this->pGravityParams ? this->pGravityParams->ToParams() : nullptr);
	
		return areaDef;
	}
};
struct EntityPhysicalizationParameters
{
	pe_type type;
	int nSlot;
	float density;
	float mass;
	int nFlagsAnd;
	int nFlagsOr;
	int nLod;
	IPhysicalEntity *pAttachToEntity;
	int nAttachToPart;
	float fStiffnessScale;
	bool bCopyJointVelocities;
	PhysicsParametersParticle* pParticle;
	PhysicsParametersBuoyancy* pBuoyancy;
	PhysicsParametersDimensions* pPlayerDimensions;
	PhysicsParametersDynamics* pPlayerDynamics;
	PhysicsParametersVehicle* pCar;

	AreaDefinition* pAreaDef;
	mono::string szPropsOverride;

	void ToParams(SEntityPhysicalizeParams &params) const
	{
		params.szPropsOverride      = NtText(this->szPropsOverride);
		params.type                 = this->type;
		params.nSlot                = this->nSlot;
		params.density              = this->density;
		params.mass                 = this->mass;
		params.nFlagsAND            = this->nFlagsAnd;
		params.nFlagsOR             = this->nFlagsOr;
		params.nLod                 = this->nLod;
		params.pAttachToEntity      = this->pAttachToEntity;
		params.nAttachToPart        = this->nAttachToPart;
		params.fStiffnessScale      = this->fStiffnessScale;
		params.bCopyJointVelocities = this->bCopyJointVelocities;
		params.pParticle            = static_cast<pe_params_particle *>(this->pParticle ? this->pParticle->ToParams() : nullptr);
		params.pBuoyancy            = static_cast<pe_params_buoyancy *>(this->pBuoyancy ? this->pBuoyancy->ToParams() : nullptr);
		params.pPlayerDimensions    = static_cast<pe_player_dimensions *>(this->pPlayerDimensions ? this->pPlayerDimensions->ToParams() : nullptr);
		params.pPlayerDynamics      = static_cast<pe_player_dynamics *>(this->pPlayerDynamics ? this->pPlayerDynamics->ToParams() : nullptr);
		params.pCar                 = static_cast<pe_params_car *>(this->pCar ? this->pCar->ToParams() : nullptr);
		params.pAreaDef             = this->pAreaDef ? this->pAreaDef->ToNativeObject() : nullptr;
	}
	void Dispose(SEntityPhysicalizeParams &params) const
	{
		SAFE_DELETE(params.pParticle);
		SAFE_DELETE(params.pBuoyancy);
		SAFE_DELETE(params.pPlayerDimensions);
		SAFE_DELETE(params.pPlayerDynamics);
		SAFE_DELETE(params.pCar);
		SAFE_DELETE(params.pAreaDef->pGravityParams);
		SAFE_DELETE(params.pAreaDef);
	}
};
