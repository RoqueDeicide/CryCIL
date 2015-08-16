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