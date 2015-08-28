#include "stdafx.h"

#include "PhysicalWorld.h"
#include "ExplosionStructs.h"
#include "WaterManagerStructs.h"
#include "PhysicsEventRaisers.h"

void PhysicalWorldInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SimulateExplosion);
	REGISTER_METHOD(AddExplosionShape);
	REGISTER_METHOD(RemoveExplosionShape);
	REGISTER_METHOD(RemoveAllExplosionShapes);
	REGISTER_METHOD(SetWaterManagerParameters);
	REGISTER_METHOD(GetWaterManagerParameters);

	RegisterEventClients();
}

void PhysicalWorldInterop::Shutdown()
{
	UnregisterEventClients();
}

ExplosionResult PhysicalWorldInterop::SimulateExplosion(const ExplosionParameters &parameters, mono::Array entitiesToSkip,
														int types)
{
	MonoGCHandle skipEntsHandle;
	MonoGCHandle affectedEntsHandle;

	IPhysicalEntity **skipEntsPtr = nullptr;
	int skipEntsCount = 0;

	if (entitiesToSkip)
	{
		skipEntsHandle = MonoEnv->GC->Pin(entitiesToSkip);
		IMonoArray<IPhysicalEntity *> skipEnts = entitiesToSkip;
		skipEntsPtr = &skipEnts[0];
		skipEntsCount = skipEnts.Length;
	}

	pe_explosion e;
	parameters.ToExplosion(e);
	gEnv->pPhysicalWorld->SimulateExplosion(&e, skipEntsPtr, skipEntsCount, types);

	ExplosionResult expResult;
	expResult.FromExplosion(e, affectedEntsHandle);

	return expResult;
}

int PhysicalWorldInterop::AddExplosionShape(IGeometry *shape, float size, int index, float probability /*= 1.0f*/)
{
	return gEnv->pPhysicalWorld->AddExplosionShape(shape, size, index, probability);
}

void PhysicalWorldInterop::RemoveExplosionShape(int index)
{
	gEnv->pPhysicalWorld->RemoveExplosionShape(index);
}

void PhysicalWorldInterop::RemoveAllExplosionShapes()
{
	gEnv->pPhysicalWorld->RemoveAllExplosionShapes();
}

void PhysicalWorldInterop::GetWaterManagerParameters(WaterManagerParameters &parameters)
{
	pe_params_waterman pars;
	parameters.ToParams(pars);

	gEnv->pPhysicalWorld->GetWaterManagerParams(&pars);

	parameters.FromParams(pars);
}

void PhysicalWorldInterop::SetWaterManagerParameters(const WaterManagerParameters &parameters)
{
	pe_params_waterman pars;
	parameters.ToParams(pars);

	gEnv->pPhysicalWorld->SetWaterManagerParams(&pars);
}