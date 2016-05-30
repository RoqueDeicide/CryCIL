#include "stdafx.h"

#include "PhysicalBody.h"
#include <Cry3DEngine/CGF/CryHeaders.h>

void PhysicalBodyInterop::InitializeInterops()
{
	REGISTER_METHOD(RegisterGeometry);
	REGISTER_METHOD(AddRefGeometry);
	REGISTER_METHOD(UnregisterGeometry);
	REGISTER_METHOD(SetMaterialMappings);
}

phys_geometry *PhysicalBodyInterop::RegisterGeometry(IGeometry *shape, int surfaceIdx, IMaterial *material)
{
	int matMap[MAX_SUB_MATERIALS];
	int matCount = material->FillSurfaceTypeIds(matMap);

	return gEnv->pPhysicalWorld->GetGeomManager()->RegisterGeometry(shape, surfaceIdx, matMap, matCount);
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
