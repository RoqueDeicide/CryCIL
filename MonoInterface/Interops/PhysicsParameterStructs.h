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
		if (this->nMats != 0 && this->pMatMapping && !is_unused(this->pMatMapping))
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

struct PhysicsParametersFlags
{
	PhysicsParameters Base;
	uint32 flags;
	uint32 flagsOR;
	uint32 flagsAND;
	pe_params *ToParams() const
	{
		pe_params_flags *params = new pe_params_flags();

		params->flags = this->flags;
		params->flagsOR = this->flagsOR;
		params->flagsAND = this->flagsAND;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_flags *params = static_cast<const pe_params_flags *>(pars);

		this->flags = params->flags;
		this->flagsOR = params->flagsOR;
		this->flagsAND = params->flagsAND;
	}
	void Dispose()
	{}
};

struct PhysicsParametersCollisionClass
{
	PhysicsParameters Base;
	SCollisionClass or;
	SCollisionClass and;
	pe_params *ToParams() const
	{
		pe_params_collision_class *params = new pe_params_collision_class();

		params->collisionClassOR = this->or;
		params->collisionClassAND = this->and;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_collision_class *params = static_cast<const pe_params_collision_class *>(pars);

		this->or = params->collisionClassOR;
		this->and = params->collisionClassAND;
	}
	void Dispose()
	{}
};

struct PhysicsParametersStructuralJoint
{
	PhysicsParameters Base;
	int id;
	int idx;
	int bReplaceExisting;
	int partid[2];
	Vec3 pt;
	Vec3 n;
	Vec3 axisx;
	float maxForcePush, maxForcePull, maxForceShift;
	float maxTorqueBend, maxTorqueTwist;
	float damageAccum, damageAccumThresh;
	Vec3 limitConstraint;
	int bBreakable;
	int bConstraintWillIgnoreCollisions;
	int bDirectBreaksOnly;
	float dampingConstraint;
	float szSensor;
	int bBroken;
	int partidEpicenter;

	pe_params *ToParams() const
	{
		pe_params_structural_joint *params = new pe_params_structural_joint();

		params->id                              = this->id;
		params->idx                             = this->idx;
		params->bReplaceExisting                = this->bReplaceExisting;
		params->partid[0]                       = this->partid[0];
		params->partid[1]                       = this->partid[1];
		params->pt                              = this->pt;
		params->n                               = this->n;
		params->axisx                           = this->axisx;
		params->maxForcePush                    = this->maxForcePush;
		params->maxForcePull                    = this->maxForcePull;
		params->maxForceShift                   = this->maxForceShift;
		params->maxTorqueBend                   = this->maxTorqueBend;
		params->maxTorqueTwist                  = this->maxTorqueTwist;
		params->damageAccum                     = this->damageAccum;
		params->damageAccumThresh               = this->damageAccumThresh;
		params->limitConstraint                 = this->limitConstraint;
		params->bBreakable                      = this->bBreakable;
		params->bConstraintWillIgnoreCollisions = this->bConstraintWillIgnoreCollisions;
		params->bDirectBreaksOnly               = this->bDirectBreaksOnly;
		params->dampingConstraint               = this->dampingConstraint;
		params->szSensor                        = this->szSensor;
		params->bBroken                         = this->bBroken;
		params->partidEpicenter                 = this->partidEpicenter;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_structural_joint *params = static_cast<const pe_params_structural_joint *>(pars);

		this->id                              = params->id;
		this->idx                             = params->idx;
		this->bReplaceExisting                = params->bReplaceExisting;
		this->partid[0]                       = params->partid[0];
		this->partid[1]                       = params->partid[1];
		this->pt                              = params->pt;
		this->n                               = params->n;
		this->axisx                           = params->axisx;
		this->maxForcePush                    = params->maxForcePush;
		this->maxForcePull                    = params->maxForcePull;
		this->maxForceShift                   = params->maxForceShift;
		this->maxTorqueBend                   = params->maxTorqueBend;
		this->maxTorqueTwist                  = params->maxTorqueTwist;
		this->damageAccum                     = params->damageAccum;
		this->damageAccumThresh               = params->damageAccumThresh;
		this->limitConstraint                 = params->limitConstraint;
		this->bBreakable                      = params->bBreakable;
		this->bConstraintWillIgnoreCollisions = params->bConstraintWillIgnoreCollisions;
		this->bDirectBreaksOnly               = params->bDirectBreaksOnly;
		this->dampingConstraint               = params->dampingConstraint;
		this->szSensor                        = params->szSensor;
		this->bBroken                         = params->bBroken;
		this->partidEpicenter                 = params->partidEpicenter;
	}
	void Dispose()
	{}
};

struct PhysicsParametersStructuralInitialVelocity
{
	PhysicsParameters Base;
	int partid;
	Vec3 v;
	Vec3 w;

	pe_params *ToParams() const
	{
		pe_params_structural_initial_velocity *params = new pe_params_structural_initial_velocity();

		params->partid = this->partid;
		params->v = this->v;
		params->w = this->w;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_structural_initial_velocity *params = static_cast<const pe_params_structural_initial_velocity *>(pars);

		this->partid = params->partid;
		this->v = params->v;
		this->w = params->w;
	}
	void Dispose()
	{}
};

struct PhysicsParametersTimeout
{
	PhysicsParameters Base;
	float timeIdle;
	float maxTimeIdle;

	pe_params *ToParams() const
	{
		pe_params_timeout *params = new pe_params_timeout();

		params->timeIdle = this->timeIdle;
		params->maxTimeIdle = this->maxTimeIdle;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_timeout *params = static_cast<const pe_params_timeout *>(pars);

		this->timeIdle = params->timeIdle;
		this->maxTimeIdle = params->maxTimeIdle;
	}
	void Dispose()
	{}
};

struct PhysicsParametersSkeleton
{
	PhysicsParameters Base;
	int partid;
	int ipart;
	float stiffness;
	float thickness;
	float maxStretch;
	float maxImpulse;
	float timeStep;
	int nSteps;
	float hardness;
	float explosionScale;
	int bReset;

	pe_params *ToParams() const
	{
		pe_params_skeleton *params = new pe_params_skeleton();

		params->partid         = this->partid;
		params->ipart          = this->ipart;
		params->stiffness      = this->stiffness;
		params->thickness      = this->thickness;
		params->maxStretch     = this->maxStretch;
		params->maxImpulse     = this->maxImpulse;
		params->timeStep       = this->timeStep;
		params->nSteps         = this->nSteps;
		params->hardness       = this->hardness;
		params->explosionScale = this->explosionScale;
		params->bReset         = this->bReset;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_skeleton *params = static_cast<const pe_params_skeleton *>(pars);

		this->partid         = params->partid;
		this->ipart          = params->ipart;
		this->stiffness      = params->stiffness;
		this->thickness      = params->thickness;
		this->maxStretch     = params->maxStretch;
		this->maxImpulse     = params->maxImpulse;
		this->timeStep       = params->timeStep;
		this->nSteps         = params->nSteps;
		this->hardness       = params->hardness;
		this->explosionScale = params->explosionScale;
		this->bReset         = params->bReset;
	}
	void Dispose()
	{}
};

struct PhysicsParametersJoint
{
	PhysicsParameters Base;
	uint32 flags;
	int flagsPivot;
	Vec3 pivot;
	Quat q0;
	Ang3 limits1;
	Ang3 limits2;
	Ang3 bounciness;
	Ang3 ks, kd;
	Ang3 qdashpot;
	Ang3 kdashpot;
	Ang3 q;
	Ang3 qext;
	Ang3 qtarget;
	int op1;
	int op2;
	mono::Array selfCollidingParts;
	int nSelfCollidingParts;
	int* pSelfCollidingParts;
	int bNoUpdate;
	float animationTimeStep;
	float ranimationTimeStep;

	pe_params *ToParams() const
	{
		pe_params_joint *params = new pe_params_joint();

		params->flags               = this->flags;
		params->flagsPivot          = this->flagsPivot;
		params->pivot               = this->pivot;
		params->q0                  = this->q0;
		params->limits[0]           = Vec3(this->limits1);
		params->limits[1]           = Vec3(this->limits2);
		params->bounciness          = Vec3(this->bounciness);
		params->ks                  = Vec3(this->ks);
		params->kd                  = Vec3(this->kd);
		params->qdashpot            = Vec3(this->qdashpot);
		params->kdashpot            = Vec3(this->kdashpot);
		params->q                   = this->q;
		params->qext                = this->qext;
		params->qtarget             = this->qtarget;
		params->op[0]               = this->op1;
		params->op[1]               = this->op2;
		params->pSelfCollidingParts = this->pSelfCollidingParts;
		params->nSelfCollidingParts = this->nSelfCollidingParts;
		params->bNoUpdate           = this->bNoUpdate;
		params->animationTimeStep   = this->animationTimeStep;
		params->ranimationTimeStep  = this->ranimationTimeStep;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_joint *params = static_cast<const pe_params_joint *>(pars);

		this->flags               = params->flags;
		this->flagsPivot          = params->flagsPivot;
		this->pivot               = params->pivot;
		this->q0                  = params->q0;
		this->limits1             = Ang3(params->limits[0]);
		this->limits2             = Ang3(params->limits[1]);
		this->bounciness          = Ang3(params->bounciness);
		this->ks                  = Ang3(params->ks);
		this->kd                  = Ang3(params->kd);
		this->qdashpot            = Ang3(params->qdashpot);
		this->kdashpot            = Ang3(params->kdashpot);
		this->q                   = params->q;
		this->qext                = params->qext;
		this->qtarget             = params->qtarget;
		this->op1                 = params->op[0];
		this->op2                 = params->op[1];
		this->bNoUpdate           = params->bNoUpdate;
		this->animationTimeStep   = params->animationTimeStep;
		this->ranimationTimeStep  = params->ranimationTimeStep;
		this->pSelfCollidingParts = nullptr;
		this->nSelfCollidingParts = 0;

		if (params->nSelfCollidingParts > 0)
		{
			IMonoArray<int> idArray = MonoEnv->Objects->Arrays->Create(nSelfCollidingParts, MonoEnv->CoreLibrary->Int32);
			MonoGCHandle gcHandle = MonoEnv->GC->Pin(idArray);

			for (int i = 0; i < params->nSelfCollidingParts; i++)
			{
				idArray[i] = this->pSelfCollidingParts[i];
			}

			this->selfCollidingParts = idArray;
		}
	}
	void Dispose()
	{
		if (this->nSelfCollidingParts > 0 && !is_unused(this->pSelfCollidingParts) && this->pSelfCollidingParts)
		{
			free(this->pSelfCollidingParts);
		}
	}
};

struct PhysicsParametersArticulatedBody
{
	PhysicsParameters Base;
	int bGrounded;
	int bCheckCollisions;
	int bCollisionResp;
	Vec3 pivot;
	Vec3 a;
	Vec3 wa;
	Vec3 w;
	Vec3 v;
	float scaleBounceResponse;
	int bApply_dqext;
	int bAwake;
	IPhysicalEntity *pHost;
	Vec3 posHostPivot; // attachment position inside pHost
	Quat qHostPivot;
	int bInheritVel;
	int nCollLyingMode;
	Vec3 gravityLyingMode;
	float dampingLyingMode;
	float minEnergyLyingMode;
	int iSimType;
	int iSimTypeLyingMode;
	int nRoots; // only used in GetParams
	int nJointsAlloc;
	int bRecalcJoints;

	pe_params *ToParams() const
	{
		pe_params_articulated_body *params = new pe_params_articulated_body();

		params->bGrounded           = this->bGrounded;
		params->bCheckCollisions    = this->bCheckCollisions;
		params->bCollisionResp      = this->bCollisionResp;
		params->pivot               = this->pivot;
		params->a                   = this->a;
		params->wa                  = this->wa;
		params->w                   = this->w;
		params->v                   = this->v;
		params->scaleBounceResponse = this->scaleBounceResponse;
		params->bApply_dqext        = this->bApply_dqext;
		params->bAwake              = this->bAwake;
		params->pHost               = this->pHost;
		params->posHostPivot        = this->posHostPivot;
		params->qHostPivot          = this->qHostPivot;
		params->bInheritVel         = this->bInheritVel;
		params->nCollLyingMode      = this->nCollLyingMode;
		params->gravityLyingMode    = this->gravityLyingMode;
		params->dampingLyingMode    = this->dampingLyingMode;
		params->minEnergyLyingMode  = this->minEnergyLyingMode;
		params->iSimType            = this->iSimType;
		params->iSimTypeLyingMode   = this->iSimTypeLyingMode;
		params->nRoots              = this->nRoots;
		params->nJointsAlloc        = this->nJointsAlloc;
		params->bRecalcJoints       = this->bRecalcJoints;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_articulated_body *params = static_cast<const pe_params_articulated_body *>(pars);

		this->bGrounded           = params->bGrounded;
		this->bCheckCollisions    = params->bCheckCollisions;
		this->bCollisionResp      = params->bCollisionResp;
		this->pivot               = params->pivot;
		this->a                   = params->a;
		this->wa                  = params->wa;
		this->w                   = params->w;
		this->v                   = params->v;
		this->scaleBounceResponse = params->scaleBounceResponse;
		this->bApply_dqext        = params->bApply_dqext;
		this->bAwake              = params->bAwake;
		this->pHost               = params->pHost;
		this->posHostPivot        = params->posHostPivot;
		this->qHostPivot          = params->qHostPivot;
		this->bInheritVel         = params->bInheritVel;
		this->nCollLyingMode      = params->nCollLyingMode;
		this->gravityLyingMode    = params->gravityLyingMode;
		this->dampingLyingMode    = params->dampingLyingMode;
		this->minEnergyLyingMode  = params->minEnergyLyingMode;
		this->iSimType            = params->iSimType;
		this->iSimTypeLyingMode   = params->iSimTypeLyingMode;
		this->nRoots              = params->nRoots;
		this->nJointsAlloc        = params->nJointsAlloc;
		this->bRecalcJoints       = params->bRecalcJoints;
	}
	void Dispose()
	{}
};

struct PhysicsParametersDimensions
{
	PhysicsParameters Base;
	float heightPivot;
	float heightEye;
	Vec3 sizeCollider;
	float heightCollider;
	float headRadius;
	float heightHead;
	Vec3 dirUnproj;
	float maxUnproj;
	int bUseCapsule;
	float groundContactEps;

	pe_params *ToParams() const
	{
		pe_player_dimensions *params = new pe_player_dimensions();

		params->heightPivot      = this->heightPivot;
		params->heightEye        = this->heightEye;
		params->sizeCollider     = this->sizeCollider;
		params->heightCollider   = this->heightCollider;
		params->headRadius       = this->headRadius;
		params->heightHead       = this->heightHead;
		params->dirUnproj        = this->dirUnproj;
		params->maxUnproj        = this->maxUnproj;
		params->bUseCapsule      = this->bUseCapsule;
		params->groundContactEps = this->groundContactEps;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_player_dimensions *params = static_cast<const pe_player_dimensions *>(pars);

		this->heightPivot      = params->heightPivot;
		this->heightEye        = params->heightEye;
		this->sizeCollider     = params->sizeCollider;
		this->heightCollider   = params->heightCollider;
		this->headRadius       = params->headRadius;
		this->heightHead       = params->heightHead;
		this->dirUnproj        = params->dirUnproj;
		this->maxUnproj        = params->maxUnproj;
		this->bUseCapsule      = params->bUseCapsule;
		this->groundContactEps = params->groundContactEps;
	}
	void Dispose()
	{}
};

struct PhysicsParametersDynamics
{
	PhysicsParameters Base;
	float kInertia;
	float kInertiaAccel;
	float kAirControl;
	float kAirResistance;
	Vec3 gravity;
	float nodSpeed;
	int bSwimming;
	float mass;
	int surface_idx;
	float minSlideAngle;
	float maxClimbAngle;
	float maxJumpAngle;
	float minFallAngle;
	float maxVelGround;
	float timeImpulseRecover;
	int collTypes;
	IPhysicalEntity *pLivingEntToIgnore;
	int bNetwork;
	int bActive;
	int iRequestedTime;
	int bReleaseGroundColliderWhenNotActive;

	pe_params *ToParams() const
	{
		pe_player_dynamics *params = new pe_player_dynamics();

		params->kInertia                            = this->kInertia;
		params->kInertiaAccel                       = this->kInertiaAccel;
		params->kAirControl                         = this->kAirControl;
		params->kAirResistance                      = this->kAirResistance;
		params->gravity                             = this->gravity;
		params->nodSpeed                            = this->nodSpeed;
		params->bSwimming                           = this->bSwimming;
		params->mass                                = this->mass;
		params->surface_idx                         = this->surface_idx;
		params->minSlideAngle                       = this->minSlideAngle;
		params->maxClimbAngle                       = this->maxClimbAngle;
		params->maxJumpAngle                        = this->maxJumpAngle;
		params->minFallAngle                        = this->minFallAngle;
		params->maxVelGround                        = this->maxVelGround;
		params->timeImpulseRecover                  = this->timeImpulseRecover;
		params->collTypes                           = this->collTypes;
		params->pLivingEntToIgnore                  = this->pLivingEntToIgnore;
		params->bNetwork                            = this->bNetwork;
		params->bActive                             = this->bActive;
		params->iRequestedTime                      = this->iRequestedTime;
		params->bReleaseGroundColliderWhenNotActive = this->bReleaseGroundColliderWhenNotActive;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_player_dynamics *params = static_cast<const pe_player_dynamics *>(pars);

		this->kInertia                            = params->kInertia;
		this->kInertiaAccel                       = params->kInertiaAccel;
		this->kAirControl                         = params->kAirControl;
		this->kAirResistance                      = params->kAirResistance;
		this->gravity                             = params->gravity;
		this->nodSpeed                            = params->nodSpeed;
		this->bSwimming                           = params->bSwimming;
		this->mass                                = params->mass;
		this->surface_idx                         = params->surface_idx;
		this->minSlideAngle                       = params->minSlideAngle;
		this->maxClimbAngle                       = params->maxClimbAngle;
		this->maxJumpAngle                        = params->maxJumpAngle;
		this->minFallAngle                        = params->minFallAngle;
		this->maxVelGround                        = params->maxVelGround;
		this->timeImpulseRecover                  = params->timeImpulseRecover;
		this->collTypes                           = params->collTypes;
		this->pLivingEntToIgnore                  = params->pLivingEntToIgnore;
		this->bNetwork                            = params->bNetwork;
		this->bActive                             = params->bActive;
		this->iRequestedTime                      = params->iRequestedTime;
		this->bReleaseGroundColliderWhenNotActive = params->bReleaseGroundColliderWhenNotActive;
	}
	void Dispose()
	{}
};

struct PhysicsParametersParticle
{
	PhysicsParameters Base;
	uint32 flags;
	float mass;
	float size;
	float thickness;
	Vec3 heading;
	float velocity;
	float kAirResistance;
	float kWaterResistance;
	float accThrust;
	float accLift;
	int surface_idx;
	Ang3 wspin;
	Vec3 gravity;
	Vec3 waterGravity;
	Vec3 normal;
	Vec3 rollAxis;
	Quat q0;
	float minBounceVel;
	float minVel;
	IPhysicalEntity *pColliderToIgnore;
	int iPierceability;
	int collTypes;
	int areaCheckPeriod;
	int dontPlayHitEffect;

	pe_params *ToParams() const
	{
		pe_params_particle *params = new pe_params_particle();

		params->flags = this->flags;
		params->mass = this->mass;
		params->size = this->size;
		params->thickness = this->thickness;
		params->heading = this->heading;
		params->velocity = this->velocity;
		params->kAirResistance = this->kAirResistance;
		params->kWaterResistance = this->kWaterResistance;
		params->accThrust = this->accThrust;
		params->accLift = this->accLift;
		params->surface_idx = this->surface_idx;
		params->wspin = Vec3(this->wspin);
		params->gravity = this->gravity;
		params->waterGravity = this->waterGravity;
		params->normal = this->normal;
		params->collTypes = this->collTypes;
		params->rollAxis = this->rollAxis;
		params->q0 = this->q0;
		params->minBounceVel = this->minBounceVel;
		params->minVel = this->minVel;
		params->pColliderToIgnore = this->pColliderToIgnore;
		params->iPierceability = this->iPierceability;
		params->areaCheckPeriod = this->areaCheckPeriod;
		params->dontPlayHitEffect = this->dontPlayHitEffect;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_particle *params = static_cast<const pe_params_particle *>(pars);

		this->flags = params->flags;
		this->mass = params->mass;
		this->size = params->size;
		this->thickness = params->thickness;
		this->heading = params->heading;
		this->velocity = params->velocity;
		this->kAirResistance = params->kAirResistance;
		this->kWaterResistance = params->kWaterResistance;
		this->accThrust = params->accThrust;
		this->accLift = params->accLift;
		this->surface_idx = params->surface_idx;
		this->wspin = Ang3(params->wspin);
		this->gravity = params->gravity;
		this->waterGravity = params->waterGravity;
		this->normal = params->normal;
		this->collTypes = params->collTypes;
		this->rollAxis = params->rollAxis;
		this->q0 = params->q0;
		this->minBounceVel = params->minBounceVel;
		this->minVel = params->minVel;
		this->pColliderToIgnore = params->pColliderToIgnore;
		this->iPierceability = params->iPierceability;
		this->areaCheckPeriod = params->areaCheckPeriod;
		this->dontPlayHitEffect = params->dontPlayHitEffect;
	}
	void Dispose()
	{}
};

struct PhysicsParametersVehicle
{
	PhysicsParameters Base;
	float axleFriction;
	float enginePower;
	float maxSteer;
	float engineMaxRPM;
	float brakeTorque;
	int iIntegrationType;
	float maxTimeStep;
	float minEnergy;
	float damping;
	float minBrakingFriction;
	float maxBrakingFriction;
	float kStabilizer;
	int nWheels;
	float engineMinRPM;
	float engineShiftUpRPM;
	float engineShiftDownRPM;
	float engineIdleRPM;
	float engineStartRPM;
	float clutchSpeed;
	int nGears;
	float* gearRatios;
	int maxGear, minGear;
	float slipThreshold;
	float gearDirSwitchRPM;
	float kDynFriction;
	float steerTrackNeutralTurn;
	float pullTilt;
	float maxTilt;
	int bKeepTractionWhenTilted;

	pe_params *ToParams() const
	{
		pe_params_car *params = new pe_params_car();

		params->axleFriction            = this->axleFriction;
		params->enginePower             = this->enginePower;
		params->maxSteer                = this->maxSteer;
		params->engineMaxRPM            = this->engineMaxRPM;
		params->brakeTorque             = this->brakeTorque;
		params->iIntegrationType        = this->iIntegrationType;
		params->maxTimeStep             = this->maxTimeStep;
		params->minEnergy               = this->minEnergy;
		params->damping                 = this->damping;
		params->minBrakingFriction      = this->minBrakingFriction;
		params->maxBrakingFriction      = this->maxBrakingFriction;
		params->kStabilizer             = this->kStabilizer;
		params->nWheels                 = this->nWheels;
		params->engineMinRPM            = this->engineMinRPM;
		params->engineShiftUpRPM        = this->engineShiftUpRPM;
		params->engineShiftDownRPM      = this->engineShiftDownRPM;
		params->engineIdleRPM           = this->engineIdleRPM;
		params->engineStartRPM          = this->engineStartRPM;
		params->clutchSpeed             = this->clutchSpeed;
		params->nGears                  = this->nGears;
		params->gearRatios              = this->gearRatios;
		params->maxGear                 = this->maxGear;
		params->minGear                 = this->minGear;
		params->slipThreshold           = this->slipThreshold;
		params->gearDirSwitchRPM        = this->gearDirSwitchRPM;
		params->kDynFriction            = this->kDynFriction;
		params->steerTrackNeutralTurn   = this->steerTrackNeutralTurn;
		params->pullTilt                = this->pullTilt;
		params->maxTilt                 = this->maxTilt;
		params->bKeepTractionWhenTilted = this->bKeepTractionWhenTilted;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_car *params = static_cast<const pe_params_car *>(pars);

		this->axleFriction            = params->axleFriction;
		this->enginePower             = params->enginePower;
		this->maxSteer                = params->maxSteer;
		this->engineMaxRPM            = params->engineMaxRPM;
		this->brakeTorque             = params->brakeTorque;
		this->iIntegrationType        = params->iIntegrationType;
		this->maxTimeStep             = params->maxTimeStep;
		this->minEnergy               = params->minEnergy;
		this->damping                 = params->damping;
		this->minBrakingFriction      = params->minBrakingFriction;
		this->maxBrakingFriction      = params->maxBrakingFriction;
		this->kStabilizer             = params->kStabilizer;
		this->nWheels                 = params->nWheels;
		this->engineMinRPM            = params->engineMinRPM;
		this->engineShiftUpRPM        = params->engineShiftUpRPM;
		this->engineShiftDownRPM      = params->engineShiftDownRPM;
		this->engineIdleRPM           = params->engineIdleRPM;
		this->engineStartRPM          = params->engineStartRPM;
		this->clutchSpeed             = params->clutchSpeed;
		this->nGears                  = params->nGears;
		this->gearRatios              = params->gearRatios;
		this->maxGear                 = params->maxGear;
		this->minGear                 = params->minGear;
		this->slipThreshold           = params->slipThreshold;
		this->gearDirSwitchRPM        = params->gearDirSwitchRPM;
		this->kDynFriction            = params->kDynFriction;
		this->steerTrackNeutralTurn   = params->steerTrackNeutralTurn;
		this->pullTilt                = params->pullTilt;
		this->maxTilt                 = params->maxTilt;
		this->bKeepTractionWhenTilted = params->bKeepTractionWhenTilted;
	}
	void Dispose()
	{
		if (this->nGears > 0 && this->gearRatios != nullptr && !is_unused(this->gearRatios))
		{
			free(this->gearRatios);
			this->gearRatios = nullptr;
			this->nGears = 0;
		}
	}
};

struct PhysicsParametersWheel
{
	PhysicsParameters Base;
	int iWheel;
	int bDriving;
	int iAxle;
	int bCanBrake;
	int bBlocked;
	int bCanSteer;
	float suspLenMax;
	float suspLenInitial;
	float minFriction;
	float maxFriction;
	int surface_idx;
	int bRayCast;
	float kStiffness;
	float kStiffnessWeight;
	float kDamping;
	float kLatFriction;
	float Tscale;
	float w;

	pe_params *ToParams() const
	{
		pe_params_wheel *params = new pe_params_wheel();

		params->iWheel           = this->iWheel;
		params->bDriving         = this->bDriving;
		params->iAxle            = this->iAxle;
		params->bCanBrake        = this->bCanBrake;
		params->bBlocked         = this->bBlocked;
		params->bCanSteer        = this->bCanSteer;
		params->suspLenMax       = this->suspLenMax;
		params->suspLenInitial   = this->suspLenInitial;
		params->minFriction      = this->minFriction;
		params->maxFriction      = this->maxFriction;
		params->surface_idx      = this->surface_idx;
		params->bRayCast         = this->bRayCast;
		params->kStiffness       = this->kStiffness;
		params->kStiffnessWeight = this->kStiffnessWeight;
		params->kDamping         = this->kDamping;
		params->kLatFriction     = this->kLatFriction;
		params->Tscale           = this->Tscale;
		params->w                = this->w;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_wheel *params = static_cast<const pe_params_wheel *>(pars);

		this->iWheel           = params->iWheel;
		this->bDriving         = params->bDriving;
		this->iAxle            = params->iAxle;
		this->bCanBrake        = params->bCanBrake;
		this->bBlocked         = params->bBlocked;
		this->bCanSteer        = params->bCanSteer;
		this->suspLenMax       = params->suspLenMax;
		this->suspLenInitial   = params->suspLenInitial;
		this->minFriction      = params->minFriction;
		this->maxFriction      = params->maxFriction;
		this->surface_idx      = params->surface_idx;
		this->bRayCast         = params->bRayCast;
		this->kStiffness       = params->kStiffness;
		this->kStiffnessWeight = params->kStiffnessWeight;
		this->kDamping         = params->kDamping;
		this->kLatFriction     = params->kLatFriction;
		this->Tscale           = params->Tscale;
		this->w                = params->w;
	}
	void Dispose()
	{
	}
};

struct PhysicsParametersRope
{
	PhysicsParameters Base;
	float length;
	float mass;
	float collDist;
	int surface_idx;
	float friction;
	float frictionPull;
	float stiffness;
	float stiffnessAnim;
	float stiffnessDecayAnim;
	float dampingAnim;
	int bTargetPoseActive;
	Vec3 wind;
	float windVariance;
	float airResistance;
	float waterResistance;
	float density;
	float jointLimit;
	float jointLimitDecay;
	float sensorRadius;
	float maxForce;
	float penaltyScale;
	float attachmentZone;
	float minSegLen;
	float unprojLimit;
	float noCollDist;
	int maxIters;
	int nSegments;
	int flagsCollider;
	int collTypes;
	int nMaxSubVtx;
	Vec3 collisionBBox0;
	Vec3 collisionBBox1;
	Vec3 hingeAxis;
	strided_pointer<Vec3> pPoints;
	strided_pointer<Vec3> pVelocities;
	IPhysicalEntity *pEntTiedTo0;
	IPhysicalEntity *pEntTiedTo1;
	int bLocalPtTied;
	Vec3 ptTiedTo0;
	Vec3 ptTiedTo1;
	int idPartTiedTo0;
	int idPartTiedTo1;

	pe_params *ToParams() const
	{
		pe_params_rope *params = new pe_params_rope();

		params->length             = this->length;
		params->mass               = this->mass;
		params->collDist           = this->collDist;
		params->surface_idx        = this->surface_idx;
		params->friction           = this->friction;
		params->frictionPull       = this->frictionPull;
		params->stiffness          = this->stiffness;
		params->stiffnessAnim      = this->stiffnessAnim;
		params->stiffnessDecayAnim = this->stiffnessDecayAnim;
		params->dampingAnim        = this->dampingAnim;
		params->bTargetPoseActive  = this->bTargetPoseActive;
		params->wind               = this->wind;
		params->windVariance       = this->windVariance;
		params->airResistance      = this->airResistance;
		params->waterResistance    = this->waterResistance;
		params->density            = this->density;
		params->jointLimit         = this->jointLimit;
		params->jointLimitDecay    = this->jointLimitDecay;
		params->sensorRadius       = this->sensorRadius;
		params->maxForce           = this->maxForce;
		params->penaltyScale       = this->penaltyScale;
		params->attachmentZone     = this->attachmentZone;
		params->minSegLen          = this->minSegLen;
		params->unprojLimit        = this->unprojLimit;
		params->noCollDist         = this->noCollDist;
		params->maxIters           = this->maxIters;
		params->nSegments          = this->nSegments;
		params->flagsCollider      = this->flagsCollider;
		params->collTypes          = this->collTypes;
		params->nMaxSubVtx         = this->nMaxSubVtx;
		params->collisionBBox[0]   = this->collisionBBox0;
		params->collisionBBox[1]   = this->collisionBBox1;
		params->hingeAxis          = this->hingeAxis;
		params->pPoints            = this->pPoints;
		params->pVelocities        = this->pVelocities;
		params->pEntTiedTo[0]      = this->pEntTiedTo0;
		params->pEntTiedTo[1]      = this->pEntTiedTo1;
		params->bLocalPtTied       = this->bLocalPtTied;
		params->ptTiedTo[0]        = this->ptTiedTo0;
		params->ptTiedTo[1]        = this->ptTiedTo1;
		params->idPartTiedTo[0]    = this->idPartTiedTo0;
		params->idPartTiedTo[1]    = this->idPartTiedTo1;

		return params;
	}
	void FromParams(const pe_params *pars)
	{
		const pe_params_rope *params = static_cast<const pe_params_rope *>(pars);

		this->length             = params->length;
		this->mass               = params->mass;
		this->collDist           = params->collDist;
		this->surface_idx        = params->surface_idx;
		this->friction           = params->friction;
		this->frictionPull       = params->frictionPull;
		this->stiffness          = params->stiffness;
		this->stiffnessAnim      = params->stiffnessAnim;
		this->stiffnessDecayAnim = params->stiffnessDecayAnim;
		this->dampingAnim        = params->dampingAnim;
		this->bTargetPoseActive  = params->bTargetPoseActive;
		this->wind               = params->wind;
		this->windVariance       = params->windVariance;
		this->airResistance      = params->airResistance;
		this->waterResistance    = params->waterResistance;
		this->density            = params->density;
		this->jointLimit         = params->jointLimit;
		this->jointLimitDecay    = params->jointLimitDecay;
		this->sensorRadius       = params->sensorRadius;
		this->maxForce           = params->maxForce;
		this->penaltyScale       = params->penaltyScale;
		this->attachmentZone     = params->attachmentZone;
		this->minSegLen          = params->minSegLen;
		this->unprojLimit        = params->unprojLimit;
		this->noCollDist         = params->noCollDist;
		this->maxIters           = params->maxIters;
		this->nSegments          = params->nSegments;
		this->flagsCollider      = params->flagsCollider;
		this->collTypes          = params->collTypes;
		this->nMaxSubVtx         = params->nMaxSubVtx;
		this->collisionBBox0     = params->collisionBBox[0];
		this->collisionBBox1     = params->collisionBBox[1];
		this->hingeAxis          = params->hingeAxis;
		this->pPoints            = params->pPoints;
		this->pVelocities        = params->pVelocities;
		this->pEntTiedTo0        = params->pEntTiedTo[0];
		this->pEntTiedTo1        = params->pEntTiedTo[1];
		this->bLocalPtTied       = params->bLocalPtTied;
		this->ptTiedTo0          = params->ptTiedTo[0];
		this->ptTiedTo1          = params->ptTiedTo[1];
		this->idPartTiedTo0      = params->idPartTiedTo[0];
		this->idPartTiedTo1      = params->idPartTiedTo[1];
	}
	void Dispose()
	{}
};