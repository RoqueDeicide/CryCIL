#include "stdafx.h"

#include "PhysicalBody.h"

void PhysicalBodyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(RegisterGeometry);
	REGISTER_METHOD(AddRefGeometry);
	REGISTER_METHOD(UnregisterGeometry);
	REGISTER_METHOD(SetMaterialMappings);
}

phys_geometry *PhysicalBodyInterop::RegisterGeometry(IGeometry *shape, ISurfaceType *surfaceType, IMaterial *material)
{
	int matMap[MAX_SUB_MATERIALS];
	int matCount = material->FillSurfaceTypeIds(matMap);

	return gEnv->pPhysicalWorld->GetGeomManager()->RegisterGeometry(shape, surfaceType->GetId(), matMap, matCount);
}

int PhysicalBodyInterop::AddRefGeometry(phys_geometry *handle)
{
	return gEnv->pPhysicalWorld->GetGeomManager()->AddRefGeometry(handle);
}

int PhysicalBodyInterop::UnregisterGeometry(phys_geometry *handle)
{
	return gEnv->pPhysicalWorld->GetGeomManager()->UnregisterGeometry(handle);
}

void PhysicalBodyInterop::SetMaterialMappings(phys_geometry *handle, IMaterial *material)
{
	int matMap[MAX_SUB_MATERIALS];
	int matCount = material->FillSurfaceTypeIds(matMap);

	gEnv->pPhysicalWorld->GetGeomManager()->SetGeomMatMapping(handle, matMap, matCount);
}
