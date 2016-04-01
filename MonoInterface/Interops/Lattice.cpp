#include "stdafx.h"

#include "Lattice.h"

void LatticeInterop::InitializeInterops()
{
	REGISTER_METHOD(GetParams);
	REGISTER_METHOD(SetParams);
	REGISTER_METHOD(CreateSkinMesh);
	REGISTER_METHOD(CreateTetraLattice);
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

ITetrLattice *LatticeInterop::CreateTetraLattice(Vec3* pt, int npt, int* pTets, int nTets)
{
	return gEnv->pPhysicalWorld->GetGeomManager()->CreateTetrLattice(pt, npt, pTets, nTets);
}
