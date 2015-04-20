#pragma once

#include "IMonoInterface.h"

struct TerrainInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() { return "Terrain"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine"; }

	virtual void OnRunTimeInitialized();

	static int   get_UnitSize();
	static int   get_Size();
	static int   get_SectorSize();
	static float Elevation(float x, float y);
	static float ElevationInt(int x, int y);
	static bool  IsHole(int x, int y);
	static Vec3  SurfaceNormal(float x, float y);
};