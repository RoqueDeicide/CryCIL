#include "stdafx.h"

#include "PhysicalEntity.h"

void PhysicalEntityInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetParams);
	REGISTER_METHOD(GetParams);
	REGISTER_METHOD(GetStatusInternal);
	REGISTER_METHOD(Action);
	REGISTER_METHOD(AddGeometry);
	REGISTER_METHOD(RemoveGeometry);
	REGISTER_METHOD(CollideEntityWithBeam);
	REGISTER_METHOD(SetPhysicalEntityId);
	REGISTER_METHOD(GetPhysicalEntityId);
	REGISTER_METHOD(GetPhysicalEntityById);
}

int PhysicalEntityInterop::SetParams(IPhysicalEntity *handle, PhysicsParameters *parameters, bool threadSafe)
{
	auto converter = GetParamConverterToCE(parameters->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto params = converter(parameters);
	int result = handle->SetParams(params, threadSafe);
	// Delete CryEngine object.
	delete params;
	// Dispose the CryCIL object.
	GetParamDisposer(parameters->type)(parameters);
	return result;
}

int PhysicalEntityInterop::GetParams(IPhysicalEntity *handle, PhysicsParameters *parameters)
{
	auto converter = GetParamConverterToCE(parameters->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto params = converter(parameters);
	int result = handle->GetParams(params);
	// Store result in the CryCIL object.
	GetParamConverterToMono(parameters->type)(params, parameters);
	// Delete CryEngine object.
	delete params;
	return result;
}

int PhysicalEntityInterop::GetStatusInternal(IPhysicalEntity *handle, PhysicsStatus *status)
{
	auto converter = GetStatusConverterToCE(status->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto stat = converter(status);
	int result = handle->GetStatus(stat);
	// Store result in the CryCIL object.
	GetStatusConverterToMono(status->type)(stat, status);
	// Delete CryEngine object.
	delete stat;
	return result;
}

int PhysicalEntityInterop::Action(IPhysicalEntity *handle, PhysicsAction *action, bool threadSafe)
{
	auto converter = GetActionConverterToCE(action->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto act = converter(action);
	// Store result in the CryCIL object.
	int result = handle->Action(act, threadSafe);
	// Delete CryEngine object.
	delete act;
	return result;
}

int PhysicalEntityInterop::AddGeometry(IPhysicalEntity *handle, phys_geometry *pgeom, GeometryParameters *parameters, int id, bool threadSafe)
{
	EPE_GeomParams paramsType = EPE_GeomParams(parameters->type);

	switch (paramsType)
	{
	case ePE_geomparams:
	{
		pe_geomparams geomParams;
		parameters->ToGeomParams(geomParams);
		return handle->AddGeometry(pgeom, &geomParams, id, threadSafe ? 1 : 0);
	}
	case ePE_cargeomparams:
	{
		pe_cargeomparams geomParams;
		GeometryParametersVehicle *carParams =
			reinterpret_cast<GeometryParametersVehicle *>(parameters);
		carParams->ToGeomParams(geomParams);
		return handle->AddGeometry(pgeom, &geomParams, id, threadSafe ? 1 : 0);
	}
	case ePE_articgeomparams:
	{
		pe_articgeomparams geomParams;
		GeometryParametersArticulatedBody *artParams =
			reinterpret_cast<GeometryParametersArticulatedBody *>(parameters);
		artParams->ToGeomParams(geomParams);
		return handle->AddGeometry(pgeom, &geomParams, id, threadSafe ? 1 : 0);
	}
	case ePE_GeomParams_Count: break;
	default: break;
	}

	return -1;
}

void PhysicalEntityInterop::RemoveGeometry(IPhysicalEntity *handle, int id, bool threadSafe)
{
	handle->RemoveGeometry(id, threadSafe ? 1 : 0);
}

bool PhysicalEntityInterop::CollideEntityWithBeam(IPhysicalEntity *handle, Vec3 *org, Vec3 *dir, float r, ray_hit *phit)
{
	return gEnv->pPhysicalWorld->CollideEntityWithBeam(handle, *org, *dir, r, phit) != 0;
}

int PhysicalEntityInterop::SetPhysicalEntityId(IPhysicalEntity *pent, int id, int bReplace, int bThreadSafe)
{
	return gEnv->pPhysicalWorld->SetPhysicalEntityId(pent, id, bReplace, bThreadSafe);
}

int PhysicalEntityInterop::GetPhysicalEntityId(IPhysicalEntity *pent)
{
	return gEnv->pPhysicalWorld->GetPhysicalEntityId(pent);
}

IPhysicalEntity* PhysicalEntityInterop::GetPhysicalEntityById(int id)
{
	return gEnv->pPhysicalWorld->GetPhysicalEntityById(id);
}
