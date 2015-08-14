#pragma once

#include "IMonoInterface.h"

struct PhysicsParameters
{
	int type;

	void ToParams(pe_params *p) const
	{
		p->type = this->type;
	}
};