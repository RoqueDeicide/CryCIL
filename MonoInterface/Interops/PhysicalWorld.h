#pragma once

#include "IMonoInterface.h"

struct ExplosionResult;
struct ExplosionParameters;

struct PhysicalWorldInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PhysicalWorld"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized() override;

	static ExplosionResult SimulateExplosion(const ExplosionParameters &parameters, mono::Array entitiesToSkip, int types);
};