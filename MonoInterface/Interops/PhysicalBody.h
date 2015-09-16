#pragma once

#include "IMonoInterface.h"

struct PhysicalBodyInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PhysicalBody"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized() override;

	static phys_geometry *RegisterGeometry(IGeometry *shape, int surfaceIdx, IMaterial *material);
	static int            AddRefGeometry(phys_geometry *handle);
	static int            UnregisterGeometry(phys_geometry *handle);
	static void           SetMaterialMappings(phys_geometry *handle, IMaterial *material);
};