#pragma once

#include "IMonoInterface.h"

struct PhysicsStatus
{
	int type;
	bool initialized;
};

struct PhysicsStatusLocation
{
	PhysicsStatus Base;
	int partid;
	int ipart;
	bool localSpace;

	QuatTS loc;
	AABB BoundingBox;
	int SimulationClass;
	Matrix34 Transformation;
	uint32 flags;
	IGeometry *geom;
	IGeometry *geomProxy;
	pe_status *ToStatus()
	{
		pe_status_pos *stat = new pe_status_pos();

		stat->partid = this->partid;
		stat->ipart = this->ipart;
		stat->flags = this->localSpace ? status_local : 0;
		stat->pMtx3x4 = &this->Transformation;

		return stat;
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_pos *stat = static_cast<const pe_status_pos *>(status);

		this->loc.t           = stat->pos;
		this->BoundingBox     = AABB(stat->BBox[0], stat->BBox[1]);
		this->loc.q           = stat->q;
		this->loc.s           = stat->scale;
		this->SimulationClass = stat->iSimClass;
		this->Transformation  = *stat->pMtx3x4;
		this->flags           = stat->flagsOR;
		this->geom            = stat->pGeom;
		this->geomProxy       = stat->pGeomProxy;
	}
};

struct PhysicsStatusNetworkLocation
{
	PhysicsStatus Base;
	Vec3 Position;
	Quat Orientation;
	Vec3 Velocity;
	Ang3 AngularVelocity;
	float TimeOffset;

	pe_status *ToStatus()
	{
		return new pe_status_netpos();
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_netpos *stat = static_cast<const pe_status_netpos *>(status);

		this->Position        = stat->pos;
		this->Orientation     = stat->rot;
		this->Velocity        = stat->vel;
		this->AngularVelocity = Ang3(stat->angvel);
		this->TimeOffset      = stat->timeOffset;
	}
};

struct PhysicsStatusSensors
{
	PhysicsStatus Base;
	Vec3* points;
	Vec3* normals;
	int flags;
	int sensorCount;

	pe_status *ToStatus()
	{
		return new pe_status_sensors();
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_sensors *stat = static_cast<const pe_status_sensors *>(status);

		this->points  = stat->pPoints;
		this->normals = stat->pNormals;
		this->flags   = stat->flags;
	}
};

struct PhysicsStatusDynamics
{
	PhysicsStatus Base;
	int partid;
	int ipart;
	Vec3 v;
	Vec3 w;
	Vec3 a;
	Vec3 wa;
	Vec3 centerOfMass;
	float submergedFraction;
	float mass;
	float energy;
	int nContacts;
	float time_interval;

	pe_status *ToStatus()
	{
		pe_status_dynamics *stat = new pe_status_dynamics();

		stat->partid = this->partid;
		stat->ipart  = this->ipart;

		return stat;
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_dynamics *stat = static_cast<const pe_status_dynamics *>(status);

		this->partid            = stat->partid;
		this->ipart             = stat->ipart;
		this->v                 = stat->v;
		this->w                 = stat->w;
		this->a                 = stat->a;
		this->wa                = stat->wa;
		this->centerOfMass      = stat->centerOfMass;
		this->submergedFraction = stat->submergedFraction;
		this->mass              = stat->mass;
		this->energy            = stat->energy;
		this->nContacts         = stat->nContacts;
		this->time_interval     = stat->time_interval;
	}
};

struct PhysicsStatusSurfaceId
{
	PhysicsStatus Base;
	int ipart;
	int partid;
	int iPrim; // primitive index (only makes sense for meshes)
	int iFeature;	// feature id inside the primitive; doesn't affect the result currently
	int bUseProxy; // use pPhysGeomProxy or pPhysGeom
	int id; // surface id

	pe_status *ToStatus()
	{
		pe_status_id *stat = new pe_status_id();

		stat->partid = this->partid;
		stat->ipart  = this->ipart;

		return stat;
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_id *stat = static_cast<const pe_status_id *>(status);

		this->partid    = stat->partid;
		this->ipart     = stat->ipart;
		this->iPrim     = stat->iPrim;
		this->iFeature  = stat->iFeature;
		this->bUseProxy = stat->bUseProxy;
		this->id        = stat->id;
	}
};

struct PhysicsStatusPartCount
{
	PhysicsStatus Base;

	pe_status *ToStatus()
	{
		return new pe_status_nparts();
	}
	void FromStatus(const pe_status *)
	{
	}
};

struct PhysicsStatusAwake
{
	PhysicsStatus Base;

	pe_status *ToStatus()
	{
		return new pe_status_awake();
	}
	void FromStatus(const pe_status *)
	{}
};

struct PhysicsStatusContainsPoint
{
	PhysicsStatus Base;
	Vec3 pt;

	pe_status *ToStatus()
	{
		pe_status_contains_point *stat = new pe_status_contains_point();

		stat->pt = this->pt;

		return stat;
	}
	void FromStatus(const pe_status *)
	{
	}
};

struct PhysicsStatusPlaceHolder
{
	PhysicsStatus Base;
	IPhysicalEntity *pFullEntity;

	pe_status *ToStatus()
	{
		return new pe_status_placeholder();
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_placeholder *stat = static_cast<const pe_status_placeholder *>(status);

		this->pFullEntity = stat->pFullEntity;
	}
};

struct PhysicsStatusSampleContactArea
{
	PhysicsStatus Base;
	Vec3 ptTest;
	Vec3 dirTest;

	pe_status *ToStatus()
	{
		pe_status_sample_contact_area *stat = new pe_status_sample_contact_area();

		stat->ptTest = this->ptTest;
		stat->dirTest = this->dirTest;

		return stat;
	}
	void FromStatus(const pe_status *)
	{}
};

struct PhysicsStatusConstraint
{
	PhysicsStatus Base;
	int id;
	int idx;
	int flags;
	Vec3 pt[2];
	Vec3 n;
	IPhysicalEntity *pBuddyEntity;
	IPhysicalEntity *pConstraintEntity;

	pe_status *ToStatus()
	{
		pe_status_constraint *stat = new pe_status_constraint();

		stat->id = this->id;

		return stat;
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_constraint *stat = static_cast<const pe_status_constraint *>(status);

		this->id = stat->id;
		this->idx = stat->idx;
		this->flags = stat->flags;
		this->pt[0] = stat->pt[0];
		this->pt[1] = stat->pt[1];
		this->n = stat->n;
		this->pBuddyEntity = stat->pBuddyEntity;
		this->pConstraintEntity = stat->pConstraintEntity;
	}
};

struct PhysicsStatusLiving
{
	PhysicsStatus Base;
	int bFlying;
	float timeFlying;
	Vec3 camOffset;
	Vec3 vel;
	Vec3 velUnconstrained;
	Vec3 velRequested;
	Vec3 velGround;
	float groundHeight;
	Vec3 groundSlope;
	int groundSurfaceIdx;
	int groundSurfaceIdxAux;
	IPhysicalEntity *pGroundCollider;
	int iGroundColliderPart;
	float timeSinceStanceChange;
	int bStuck;
	int bSquashed;

	pe_status *ToStatus()
	{
		return new pe_status_living();
	}
	void FromStatus(const pe_status *status)
	{
		const pe_status_living *stat = static_cast<const pe_status_living *>(status);

		this->bFlying               = stat->bFlying;
		this->timeFlying            = stat->timeFlying;
		this->camOffset             = stat->camOffset;
		this->vel                   = stat->vel;
		this->velUnconstrained      = stat->velUnconstrained;
		this->velRequested          = stat->velRequested;
		this->velGround             = stat->velGround;
		this->groundHeight          = stat->groundHeight;
		this->groundSlope           = stat->groundSlope;
		this->groundSurfaceIdx      = stat->groundSurfaceIdx;
		this->groundSurfaceIdxAux   = stat->groundSurfaceIdxAux;
		this->pGroundCollider       = stat->pGroundCollider;
		this->iGroundColliderPart   = stat->iGroundColliderPart;
		this->timeSinceStanceChange = stat->timeSinceStanceChange;
		this->bStuck                = stat->bStuck;
		this->bSquashed             = stat->bSquashed;
	}
};

struct PhysicsStatusCheckStance
{
	PhysicsStatus Base;
	Vec3 pos;
	Quat q;
	Vec3 sizeCollider;
	float heightCollider;
	Vec3 dirUnproj;
	float unproj;
	int bUseCapsule;

	pe_status *ToStatus()
	{
		pe_status_check_stance *stat = new pe_status_check_stance();

		stat->pos = this->pos;
		stat->q = this->q;
		stat->sizeCollider = this->sizeCollider;
		stat->heightCollider = this->heightCollider;
		stat->dirUnproj = this->dirUnproj;
		stat->unproj = this->unproj;
		stat->bUseCapsule = this->bUseCapsule;

		return stat;
	}
	void FromStatus(const pe_status *)
	{}
};