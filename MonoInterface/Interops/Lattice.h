#pragma once

#include "IMonoInterface.h"

struct PhysParamsLattice
{
	bool initialized;
	int nMaxCracks;
	float maxForcePush, maxForcePull, maxForceShift;
	float maxTorqueTwist, maxTorqueBend;
	float crackWeaken;
	float density;
};

struct LatticeInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Lattice"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized() override;

	static PhysParamsLattice GetParams(ITetrLattice *handle);
	static void              SetParams(ITetrLattice *handle, PhysParamsLattice &parameters);
	static IGeometry        *CreateSkinMesh(ITetrLattice *handle, int maxTrisPerBvNode);
};