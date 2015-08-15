#include "stdafx.h"

#include "Lattice.h"

void LatticeInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetParams);
	REGISTER_METHOD(SetParams);
	REGISTER_METHOD(CreateSkinMesh);
}

PhysParamsLattice LatticeInterop::GetParams(ITetrLattice *handle)
{
	pe_tetrlattice_params params;
	handle->GetParams(&params);
	PhysParamsLattice p;
	p.crackWeaken    = params.crackWeaken;
	p.density        = params.density;
	p.maxForcePull   = params.maxForcePull;
	p.maxForcePush   = params.maxForcePush;
	p.maxForceShift  = params.maxForceShift;
	p.maxTorqueBend  = params.maxTorqueBend;
	p.maxTorqueTwist = params.maxTorqueTwist;
	p.nMaxCracks     = params.nMaxCracks;
	p.initialized    = true;
	return p;
}

void LatticeInterop::SetParams(ITetrLattice *handle, PhysParamsLattice &params)
{
	pe_tetrlattice_params p;
	
	p.crackWeaken    = params.crackWeaken;
	p.density        = params.density;
	p.maxForcePull   = params.maxForcePull;
	p.maxForcePush   = params.maxForcePush;
	p.maxForceShift  = params.maxForceShift;
	p.maxTorqueBend  = params.maxTorqueBend;
	p.maxTorqueTwist = params.maxTorqueTwist;
	p.nMaxCracks     = params.nMaxCracks;

	handle->SetParams(&p);
}

IGeometry *LatticeInterop::CreateSkinMesh(ITetrLattice *handle, int maxTrisPerBvNode)
{
	return handle->CreateSkinMesh(maxTrisPerBvNode);
}
