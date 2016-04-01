#pragma once

#include "IMonoInterface.h"

struct TerrainInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "Terrain"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Environment"; }

	virtual void InitializeInterops() override;

	static int   get_UnitSize();
	static int   get_Size();
	static int   get_SectorSize();
	static float Elevation(float x, float y);
	static float ElevationInt(int x, int y);
	static bool  IsHole(int x, int y);
	static Vec3  SurfaceNormal(float x, float y);
};