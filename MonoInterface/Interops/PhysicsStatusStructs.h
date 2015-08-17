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

		this->loc.t = stat->pos;
		this->BoundingBox = AABB(stat->BBox[0], stat->BBox[1]);
		this->loc.q = stat->q;
		this->loc.s = stat->scale;
		this->SimulationClass = stat->iSimClass;
		this->Transformation = *stat->pMtx3x4;
		this->flags = stat->flagsOR;
	}
};