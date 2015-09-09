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
	REGISTER_METHOD(PrimitiveIntersectionInternal);
	REGISTER_METHOD(PrimitiveCastInternal);
	REGISTER_METHOD(CreatePhysicalEntity);
	REGISTER_METHOD(CreatePhysicalEntityNoParams);
	REGISTER_METHOD(CreatePhysicalEntityFromHolder);
	REGISTER_METHOD(CreatePhysicalEntityNoParamsFromHolder);
	REGISTER_METHOD(CreatePlaceHolder);
	REGISTER_METHOD(CreatePlaceHolderNoParams);
	REGISTER_METHOD(DestroyPhysicalEntity);

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

int PhysicalWorldInterop::PrimitiveIntersectionInternal(mono::Array *contacts, primitives::primitive *primitive,
														int primitiveType, int queryFlags, int flagsAll, int flagsAny,
														intersection_params *parameters, SCollisionClass collisionClass,
														IPhysicalEntity **entitiesToSkip, int skipCount)
{
	geom_contact *contactsPtr = nullptr;

	IPhysicalWorld::SPWIParams params;
	params.Init(primitiveType, primitive, Vec3(ZERO), queryFlags, &contactsPtr, flagsAll, flagsAny, collisionClass,
				parameters, nullptr, 0, entitiesToSkip, skipCount);
	WriteLockCond _lock;
	float result = gEnv->pPhysicalWorld->PrimitiveWorldIntersection(params, &_lock);

	int count = int(result);
	if (count <= 0)
	{
		*contacts = nullptr;
		return count;
	}

	IMonoClass *klass = MonoEnv->Cryambly->GetClass("CryCil.Engine.Physics", "GeometryContact");
	IMonoArray<geom_contact> _array = MonoEnv->Objects->Arrays->Create(count, klass);
	MonoGCHandle gcHandle = MonoEnv->GC->Pin(_array);

	for (int i = 0; i < count; i++)
	{
		_array[i] = contactsPtr[i];
	}

	*contacts = _array;

	return count;
}

float PhysicalWorldInterop::PrimitiveCastInternal(geom_contact *contact, primitives::primitive *primitive,
												  int primitiveType, Vec3 *sweepDirection, int queryFlags, int flagsAll,
												  int flagsAny, intersection_params *parameters,
												  SCollisionClass collisionClass, IPhysicalEntity **entitiesToSkip,
												  int skipCount)
{
	IPhysicalWorld::SPWIParams params;
	params.Init(primitiveType, primitive, *sweepDirection, queryFlags, &contact, flagsAll, flagsAny, collisionClass,
				parameters, nullptr, 0, entitiesToSkip, skipCount);
	WriteLockCond _lock;
	return gEnv->pPhysicalWorld->PrimitiveWorldIntersection(params, &_lock);
}

IPhysicalEntity *PhysicalWorldInterop::CreatePhysicalEntity(pe_type type, pe_params *initialParameters,
															ForeignData foreignData, int id)
{
	return gEnv->pPhysicalWorld->CreatePhysicalEntity(type, initialParameters, foreignData.handle, foreignData.id, id);
}

IPhysicalEntity *PhysicalWorldInterop::CreatePhysicalEntityNoParams(pe_type type, ForeignData foreignData, int id)
{
	return gEnv->pPhysicalWorld->CreatePhysicalEntity(type, nullptr, foreignData.handle, foreignData.id, id);
}

IPhysicalEntity *PhysicalWorldInterop::CreatePhysicalEntityFromHolder(pe_type type, float lifeTime,
																	  pe_params *initialParameters,
																	  ForeignData foreignData, int id,
																	  IPhysicalEntity *placeHolder)
{
	return gEnv->pPhysicalWorld->CreatePhysicalEntity(type, lifeTime, initialParameters, foreignData.handle,
													  foreignData.id, id, placeHolder);
}

IPhysicalEntity *PhysicalWorldInterop::CreatePhysicalEntityNoParamsFromHolder(pe_type type, float lifeTime,
																			  ForeignData foreignData, int id,
																			  IPhysicalEntity *placeHolder)
{
	return gEnv->pPhysicalWorld->CreatePhysicalEntity(type, lifeTime, nullptr, foreignData.handle, foreignData.id, id,
													  placeHolder);
}

IPhysicalEntity *PhysicalWorldInterop::CreatePlaceHolder(pe_type type, pe_params *initialParameters,
														 ForeignData foreignData, int id)
{
	return gEnv->pPhysicalWorld->CreatePhysicalPlaceholder(type, initialParameters, foreignData.handle, foreignData.id,
														   id);
}

IPhysicalEntity *PhysicalWorldInterop::CreatePlaceHolderNoParams(pe_type type, ForeignData foreignData, int id)
{
	return gEnv->pPhysicalWorld->CreatePhysicalPlaceholder(type, nullptr, foreignData.handle, foreignData.id, id);
}

int PhysicalWorldInterop::DestroyPhysicalEntity(IPhysicalEntity *pent, int mode, int bThreadSafe)
{
	return gEnv->pPhysicalWorld->DestroyPhysicalEntity(pent, mode, bThreadSafe);
}