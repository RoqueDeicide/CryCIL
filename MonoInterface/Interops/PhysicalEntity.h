#pragma once

#include "IMonoInterface.h"
#include "PhysicsStructConversionUtilities.h"

struct PhysicalEntityInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PhysicalEntity"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void InitializeInterops() override;

	static pe_type GetPhysicalType(IPhysicalEntity *handle);
	static int  SetParams(IPhysicalEntity *handle, PhysicsParameters *parameters, bool threadSafe);
	static int  GetParams(IPhysicalEntity *handle, PhysicsParameters *parameters);
	static int  GetStatusInternal(IPhysicalEntity *handle, PhysicsStatus *status);
	static int  Action(IPhysicalEntity *handle, PhysicsAction *action, bool threadSafe);
	static int  AddGeometry(IPhysicalEntity *handle, phys_geometry *pgeom, GeometryParameters *parameters, int id,
							bool threadSafe);
	static void RemoveGeometry(IPhysicalEntity *handle, int id, bool threadSafe);
	static bool CollideEntityWithBeam(IPhysicalEntity *handle, Vec3 *org, Vec3 *dir, float r, ray_hit *phit);
	static int  SetPhysicalEntityId(IPhysicalEntity *pent, int id, int bReplace, int bThreadSafe);
	static int  GetPhysicalEntityId(IPhysicalEntity *pent);
	static IPhysicalEntity* GetPhysicalEntityById(int id);
};