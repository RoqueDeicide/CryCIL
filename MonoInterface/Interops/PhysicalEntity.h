#pragma once

#include "IMonoInterface.h"

struct PhysicalEntityInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PhysicalEntity"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized() override;

	static int SetParams(IPhysicalEntity *handle, PhysicsParameters *parameters, bool threadSafe);
	static int GetParams(IPhysicalEntity *handle, PhysicsParameters *parameters);
};