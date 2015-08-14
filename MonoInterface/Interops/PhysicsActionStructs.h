#pragma once

#include "IMonoInterface.h"

struct PhysicsAction
{
	int type;
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