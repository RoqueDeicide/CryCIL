#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum ERType
{
	eERType_NotRenderNode_check,
	eERType_Brush_check,
	eERType_Vegetation_check,
	eERType_Light_check,
	eERType_Cloud_check,
	eERType_Dummy_1_check,
	eERType_FogVolume_check,
	eERType_Decal_check,
	eERType_ParticleEmitter_check,
	eERType_WaterVolume_check,
	eERType_WaterWave_check,
	eERType_Road_check,
	eERType_DistanceCloud_check,
	eERType_VolumeObject_check,
	eERType_Dummy_0_check,
	eERType_Rope_check,
	eERType_PrismObject_check,
	eERType_Dummy_2_check,
	eERType_LightPropagationVolume_check,
	eERType_RenderProxy_check,
	eERType_GameEffect_check,
	eERType_BreakableGlass_check,
	eERType_Dummy_3_check,
	eERType_MergedMesh_check,
	eERType_GeomCache_check,
	eERType_TypesNum_check
};

#define CHECK_ENUM(x) static_assert (ERType::x ## _check == EERType::x, "EERType enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(eERType_NotRenderNode);
	CHECK_ENUM(eERType_Brush);
	CHECK_ENUM(eERType_Vegetation);
	CHECK_ENUM(eERType_Light);
	CHECK_ENUM(eERType_Cloud);
	CHECK_ENUM(eERType_Dummy_1);
	CHECK_ENUM(eERType_FogVolume);
	CHECK_ENUM(eERType_Decal);
	CHECK_ENUM(eERType_ParticleEmitter);
	CHECK_ENUM(eERType_WaterVolume);
	CHECK_ENUM(eERType_WaterWave);
	CHECK_ENUM(eERType_Road);
	CHECK_ENUM(eERType_DistanceCloud);
	CHECK_ENUM(eERType_VolumeObject);
	CHECK_ENUM(eERType_Dummy_0);
	CHECK_ENUM(eERType_Rope);
	CHECK_ENUM(eERType_PrismObject);
	CHECK_ENUM(eERType_Dummy_2);
	CHECK_ENUM(eERType_LightPropagationVolume);
	CHECK_ENUM(eERType_RenderProxy);
	CHECK_ENUM(eERType_GameEffect);
	CHECK_ENUM(eERType_BreakableGlass);
	CHECK_ENUM(eERType_Dummy_3);
	CHECK_ENUM(eERType_MergedMesh);
	CHECK_ENUM(eERType_GeomCache);
	CHECK_ENUM(eERType_TypesNum);
}