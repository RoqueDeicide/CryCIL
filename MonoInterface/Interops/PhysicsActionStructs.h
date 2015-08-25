#pragma once

#include "IMonoInterface.h"

struct PhysicsAction
{
	int type;
	bool initialized;
};

struct PhysicsActionImpulse
{
	PhysicsAction Base;
	Vec3 impulse;
	Vec3 angImpulse;
	Vec3 point;
	int partid;
	int ipart;
	int iApplyTime;
	pe_action *ToAction() const
	{
		pe_action_impulse *act = new pe_action_impulse();

		act->impulse    = this->impulse;
		act->angImpulse = this->angImpulse;
		act->point      = this->point;
		act->partid     = this->partid;
		act->ipart      = this->ipart;
		act->iApplyTime = this->iApplyTime;

		return act;
	}
	void Dispose()
	{
	}
};

struct PhysicsActionReset
{
	PhysicsAction Base;
	bool clearContacts;

	pe_action *ToAction() const
	{
		pe_action_reset *act = new pe_action_reset();

		act->bClearContacts = this->clearContacts ? 1 : 0;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionAddConstraint
{
	PhysicsAction Base;
	int id;
	IPhysicalEntity *pBuddy;
	Vec3 pt0;
	Vec3 pt1;
	int partid0;
	int partid1;
	Quat qframe0;
	Quat qframe1;
	float xlimits0;
	float xlimits1;
	float yzlimits0;
	float yzlimits1;
	uint32 flags;
	float damping;
	float sensorRadius;
	float maxPullForce, maxBendTorque;

	pe_action *ToAction() const
	{
		pe_action_add_constraint *act = new pe_action_add_constraint();

		act->id            = this->id;
		act->pBuddy        = this->pBuddy;
		act->pt[0]         = this->pt0;
		act->pt[1]         = this->pt1;
		act->partid[0]     = this->partid0;
		act->partid[1]     = this->partid1;
		act->qframe[0]     = this->qframe0;
		act->qframe[1]     = this->qframe1;
		act->xlimits[0]    = this->xlimits0;
		act->xlimits[1]    = this->xlimits1;
		act->yzlimits[0]   = this->yzlimits0;
		act->yzlimits[1]   = this->yzlimits1;
		act->flags         = this->flags;
		act->damping       = this->damping;
		act->sensorRadius  = this->sensorRadius;
		act->maxPullForce  = this->maxPullForce;
		act->maxBendTorque = this->maxBendTorque;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionUpdateConstraint
{
	PhysicsAction Base;
	int idConstraint;
	uint32 flagsOR;
	uint32 flagsAND;
	int bRemove;
	Vec3 pt0;
	Vec3 pt1;
	Quat qframe0;
	Quat qframe1;
	float maxPullForce, maxBendTorque;
	float damping;
	int flags;

	pe_action *ToAction() const
	{
		pe_action_update_constraint *act = new pe_action_update_constraint();

		act->idConstraint = this->idConstraint;
		act->flagsOR = this->flagsOR;
		act->flagsAND = this->flagsAND;
		act->flags = this->flags;
		act->pt[0] = this->pt0;
		act->pt[1] = this->pt1;
		act->qframe[0] = this->qframe0;
		act->qframe[1] = this->qframe1;
		act->damping = this->damping;
		act->bRemove = this->bRemove;
		act->maxPullForce = this->maxPullForce;
		act->maxBendTorque = this->maxBendTorque;

		return act;
	}
	void Dispose()
	{}
};

struct PhysicsActionRegisterCollisionEvent
{
	PhysicsAction Base;
	Vec3 pt;
	Vec3 n;
	Vec3 v;
	Vec3 vSelf;
	float collMass;
	IPhysicalEntity *pCollider;
	int partid0;
	int partid1;
	int idmat0;
	int idmat1;
	short iPrim0;
	short iPrim1;

	pe_action *ToAction() const
	{
		pe_action_register_coll_event *act = new pe_action_register_coll_event();

		act->pt = this->pt;
		act->n = this->n;
		act->v = this->v;
		act->vSelf = this->vSelf;
		act->collMass = this->collMass;
		act->pCollider = this->pCollider;
		act->partid[0] = this->partid0;
		act->partid[1] = this->partid1;
		act->idmat[0] = this->idmat0;
		act->idmat[1] = this->idmat1;
		act->iPrim[0] = this->iPrim0;
		act->iPrim[1] = this->iPrim1;

		return act;
	}
	void Dispose()
	{}
};