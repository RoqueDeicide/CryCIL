#pragma once

#include "IMonoInterface.h"

struct PhysicsParameters
{
	int type;
	bool initialized;
};

struct PhysicsParametersLocation
{
	PhysicsParameters Base;
	Vec3 position;
	Quat orientation;
	float scale;
	Matrix34* pMtx3x4;
	Matrix33* pMtx3x3;
	int simClass;
	bool bRecalcBounds;
	bool bEntGridUseOBB;
	pe_params *ToParams() const
	{
		pe_params_pos *params = new pe_params_pos();

		params->pos            = this->position;
		params->q              = this->orientation;
		params->scale          = this->scale;
		params->pMtx3x4        = this->pMtx3x4;
		params->pMtx3x3        = this->pMtx3x3;
		params->iSimClass      = this->simClass;
		params->bRecalcBounds  = this->bRecalcBounds ? 1 : 0;
		params->bEntGridUseOBB = this->bEntGridUseOBB;

		return params;
	}
	void FromParams(const pe_params *)
	{
		// We do nothing because this type is not supposed to be used with GetParams.
	}
	void Dispose()
	{
		if (this->pMtx3x3) free(this->pMtx3x3);
		if (this->pMtx3x4) free(this->pMtx3x4);
	}
};

struct PhysicsParametersBoundingBox
{
	PhysicsParameters Base;
	AABB aabb;
	pe_params *ToParams() const
	{
		pe_params_bbox *params = new pe_params_bbox();

		params->BBox[0] = this->aabb.min;
		params->BBox[1] = this->aabb.max;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_bbox *params = static_cast<const pe_params_bbox *>(pars);

		this->aabb = AABB(params->BBox[0], params->BBox[1]);
	}
	void Dispose()
	{
	}
};

struct PhysicsParametersOuterEntity
{
	PhysicsParameters Base;
	IPhysicalEntity *entity;
	IGeometry *geom;
	pe_params *ToParams() const
	{
		pe_params_outer_entity *params = new pe_params_outer_entity();

		params->pOuterEntity      = this->entity;
		params->pBoundingGeometry = this->geom;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_outer_entity *params = static_cast<const pe_params_outer_entity *>(pars);

		this->entity = params->pOuterEntity;
		this->geom   = params->pBoundingGeometry;
	}
	void Dispose()
	{}
};

struct PhysicsParametersSensors
{
	PhysicsParameters Base;
	mono::Array rays;
	Vec3 *origins;
	Vec3 *dirs;
	int32 count;
	pe_params *ToParams()
	{
		pe_params_sensors *params = new pe_params_sensors();

		params->nSensors    = this->count;
		params->pOrigins    = this->origins;
		params->pDirections = this->dirs;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_sensors *params = static_cast<const pe_params_sensors *>(pars);

		if (params->nSensors == 0)
		{
			this->rays = nullptr;
			return;
		}

		IMonoArray<Ray> rays = MonoEnv->Objects->Arrays->Create(params->nSensors, MonoEnv->Cryambly->Ray);
		MonoGCHandle handle = MonoEnv->GC->Pin(rays);

		for (int i = 0; i < params->nSensors; i++)
		{
			rays[i] = Ray(params->pOrigins[i], params->pDirections[i]);
		}

		this->rays = rays;
	}
	void Dispose()
	{
		if (this->count != 0)	// Only possible when we set the sensors to an array that has elements in it.
		{
			free(this->origins);
			free(this->dirs);
		}
	}
};

struct PhysicsParametersSimulation
{
	PhysicsParameters Base;
	int iSimClass;
	float maxTimeStep;
	float minEnergy;
	float damping;
	Vec3 gravity;
	float dampingFreefall;
	Vec3 gravityFreefall;
	float maxRotVel;
	float mass;
	float density;
	int maxLoggedCollisions;
	int disablePreCG;
	float maxFriction;
	int collTypes;
	pe_params *ToParams() const
	{
		pe_simulation_params *params = new pe_simulation_params();

		params->iSimClass           = this->iSimClass;
		params->maxTimeStep         = this->maxTimeStep;
		params->minEnergy           = this->minEnergy;
		params->damping             = this->damping;
		params->gravity             = this->gravity;
		params->dampingFreefall     = this->dampingFreefall;
		params->gravityFreefall     = this->gravityFreefall;
		params->maxRotVel           = this->maxRotVel;
		params->mass                = this->mass;
		params->density             = this->density;
		params->maxLoggedCollisions = this->maxLoggedCollisions;
		params->disablePreCG        = this->disablePreCG;
		params->maxFriction         = this->maxFriction;
		params->collTypes           = this->collTypes;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_simulation_params *params = static_cast<const pe_simulation_params *>(pars);

		this->iSimClass           = params->iSimClass;
		this->maxTimeStep         = params->maxTimeStep;
		this->minEnergy           = params->minEnergy;
		this->damping             = params->damping;
		this->gravity             = params->gravity;
		this->dampingFreefall     = params->dampingFreefall;
		this->gravityFreefall     = params->gravityFreefall;
		this->maxRotVel           = params->maxRotVel;
		this->mass                = params->mass;
		this->density             = params->density;
		this->maxLoggedCollisions = params->maxLoggedCollisions;
		this->disablePreCG        = params->disablePreCG;
		this->maxFriction         = params->maxFriction;
		this->collTypes           = params->collTypes;
	}
	void Dispose()
	{}
};

struct PhysicsParametersPart
{
	PhysicsParameters Base;
	// Indicates whether this object is set up for assignment of parameters.
	bool forAssignment;
	int partid;
	int ipart;
	bool bRecalcBBox;
	QuatTS location;
	Matrix34 pMtx3x4;
	uint flagsCond;
	uint flagsOR, flagsAND;
	uint flagsColliderOR, flagsColliderAND;
	float mass;
	float density;
	float minContactDist;
	phys_geometry *pPhysGeom, *pPhysGeomProxy;
	int idmatBreakable;
	ITetrLattice *pLattice;
	int idSkeleton;
	mono::Array matMappings;
	int* pMatMapping;
	int nMats;
	int idParent;
	pe_params *ToParams()
	{
		pe_params_part *params = new pe_params_part();

		params->partid           = this->partid;
		params->ipart            = this->ipart;
		params->bRecalcBBox      = this->bRecalcBBox ? 1 : 0;
		params->pos              = this->location.t;
		params->q                = this->location.q;
		params->scale            = this->location.s;
		params->pMtx3x4          = &this->pMtx3x4;
		params->flagsCond        = this->flagsCond;
		params->flagsOR          = this->flagsOR;
		params->flagsAND         = this->flagsAND;
		params->flagsColliderOR  = this->flagsColliderOR;
		params->flagsColliderAND = this->flagsColliderAND;
		params->mass             = this->mass;
		params->minContactDist   = this->minContactDist;
		params->pPhysGeom        = this->pPhysGeom;
		params->pPhysGeomProxy   = this->pPhysGeomProxy;
		params->idmatBreakable   = this->idmatBreakable;
		params->pLattice         = this->pLattice;
		params->idSkeleton       = this->idSkeleton;
		params->pMatMapping      = this->pMatMapping;
		params->nMats            = this->nMats;
		params->idParent         = this->idParent;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_part *params = static_cast<const pe_params_part *>(pars);

		this->partid           = params->partid;
		this->ipart            = params->ipart;
		this->bRecalcBBox      = params->bRecalcBBox ? 1 : 0;
		this->location.t       = params->pos;
		this->location.q       = params->q;
		this->location.s       = params->scale;
		this->pMtx3x4          = *params->pMtx3x4;
		this->flagsCond        = params->flagsCond;
		this->flagsOR          = params->flagsOR;
		this->flagsAND         = params->flagsAND;
		this->flagsColliderOR  = params->flagsColliderOR;
		this->flagsColliderAND = params->flagsColliderAND;
		this->mass             = params->mass;
		this->minContactDist   = params->minContactDist;
		this->pPhysGeom        = params->pPhysGeom;
		this->pPhysGeomProxy   = params->pPhysGeomProxy;
		this->idmatBreakable   = params->idmatBreakable;
		this->pLattice         = params->pLattice;
		this->idSkeleton       = params->idSkeleton;
		this->idParent         = params->idParent;
		if (params->nMats == 0)
		{
			this->matMappings = nullptr;
			return;
		}

		IMonoArray<int> mappings = MonoEnv->Objects->Arrays->Create(params->nMats, MonoEnv->CoreLibrary->Int32);
		MonoGCHandle handle = MonoEnv->GC->Pin(mappings);

		for (int i = 0; i < params->nMats; i++)
		{
			mappings[i] = params->pMatMapping[i];
		}

		this->matMappings = mappings;
	}
	void Dispose()
	{
		if (this->nMats != 0)
		{
			free(this->pMatMapping);
		}
	}
};

struct PhysicsParametersForeignData
{
	PhysicsParameters Base;
	void *data;
	int foreignDataId;
	int iForeignFlags;
	int iForeignFlagsAND, iForeignFlagsOR;
	pe_params *ToParams() const
	{
		pe_params_foreign_data *params = new pe_params_foreign_data();

		params->pForeignData     = this->data;
		params->iForeignData     = this->foreignDataId;
		params->iForeignFlags    = this->iForeignFlags;
		params->iForeignFlagsAND = this->iForeignFlagsAND;
		params->iForeignFlagsOR  = this->iForeignFlagsOR;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_foreign_data *params = static_cast<const pe_params_foreign_data *>(pars);

		this->data             = params->pForeignData;
		this->foreignDataId    = params->iForeignData;
		this->iForeignFlags    = params->iForeignFlags;
		this->iForeignFlagsAND = params->iForeignFlagsAND;
		this->iForeignFlagsOR  = params->iForeignFlagsOR;
	}
	void Dispose()
	{}
};

struct PhysicsParametersBuoyancy
{
	PhysicsParameters Base;
	float waterDensity;
	float kwaterDensity;
	float waterDamping;
	float waterResistance, kwaterResistance;
	Vec3 waterFlow;
	float flowVariance;
	primitives::plane waterPlane;
	float waterEmin;
	int iMedium;
	pe_params *ToParams() const
	{
		pe_params_buoyancy *params = new pe_params_buoyancy();

		params->waterDensity     = this->waterDensity;
		params->kwaterDensity    = this->kwaterDensity;
		params->waterDamping     = this->waterDamping;
		params->waterResistance  = this->waterResistance;
		params->kwaterResistance = this->kwaterResistance;
		params->waterFlow        = this->waterFlow;
		params->flowVariance     = this->flowVariance;
		params->waterPlane       = this->waterPlane;
		params->waterEmin        = this->waterEmin;
		params->iMedium          = this->iMedium;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_buoyancy *params = static_cast<const pe_params_buoyancy *>(pars);

		this->waterDensity     = params->waterDensity;
		this->kwaterDensity    = params->kwaterDensity;
		this->waterDamping     = params->waterDamping;
		this->waterResistance  = params->waterResistance;
		this->kwaterResistance = params->kwaterResistance;
		this->waterFlow        = params->waterFlow;
		this->flowVariance     = params->flowVariance;
		this->waterPlane       = params->waterPlane;
		this->waterEmin        = params->waterEmin;
		this->iMedium          = params->iMedium;
	}
	void Dispose()
	{}
};