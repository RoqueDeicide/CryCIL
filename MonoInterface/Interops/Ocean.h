#pragma once

#include "IMonoInterface.h"

struct OceanAnimation
{
	float WindDirection;
	float WindSpeed;
	float WavesSpeed;
	float WavesAmount;
	float WavesSize;
};

struct OceanInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Ocean"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Environment"; }
	
	virtual void InitializeInterops() override;

	static byte           get_RenderOptions();
	static void           set_RenderOptions(byte flags);
	static uint           get_VisiblePixelCount();
	static float          get_WaterLevel();
	static Vec4           get_CausticsParameters();
	static Vec4           get_CausticsAnimationParameters();
	static OceanAnimation get_AnimationParameters();

	static bool  IsUnderwater(Vec3 &pos);
	static float GetBottomLevel(Vec3 &position, float maxRelevantDepth, int objectFlags);
	static float GetWaterLevel(Vec3 &position, bool accurate);
};