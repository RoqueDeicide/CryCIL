#include "stdafx.h"

#include "Terrain.h"

void TerrainInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_UnitSize);
	REGISTER_METHOD(get_Size);
	REGISTER_METHOD(get_SectorSize);
	REGISTER_METHOD_N("Elevation(float,float)", Elevation);
	REGISTER_METHOD_N("Elevation(int,int)", ElevationInt);
	REGISTER_METHOD(IsHole);
	REGISTER_METHOD(SurfaceNormal);
}

int TerrainInterop::get_UnitSize()
{
	return gEnv->p3DEngine->GetHeightMapUnitSize();
}

int TerrainInterop::get_Size()
{
	return gEnv->p3DEngine->GetTerrainSize();
}

int TerrainInterop::get_SectorSize()
{
	return gEnv->p3DEngine->GetTerrainSectorSize();
}

float TerrainInterop::Elevation(float x, float y)
{
	return gEnv->p3DEngine->GetTerrainElevation(x, y);
}

float TerrainInterop::ElevationInt(int x, int y)
{
	return gEnv->p3DEngine->GetTerrainZ(x, y);
}

bool TerrainInterop::IsHole(int x, int y)
{
	return gEnv->p3DEngine->GetTerrainHole(x, y);
}

Vec3 TerrainInterop::SurfaceNormal(float x, float y)
{
	return gEnv->p3DEngine->GetTerrainSurfaceNormal(Vec3(x, y, 0));
}
