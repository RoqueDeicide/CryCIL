#include "stdafx.h"

#include "Ocean.h"

void OceanInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_RenderOptions);
	REGISTER_METHOD(set_RenderOptions);
	REGISTER_METHOD(get_VisiblePixelCount);
	REGISTER_METHOD(get_WaterLevel);
	REGISTER_METHOD(get_CausticsParameters);
	REGISTER_METHOD(get_CausticsAnimationParameters);
	REGISTER_METHOD(get_AnimationParameters);

	REGISTER_METHOD(IsUnderwater);
	REGISTER_METHOD(GetBottomLevel);
	REGISTER_METHOD(GetWaterLevel);
}

byte OceanInterop::get_RenderOptions()
{
	return gEnv->p3DEngine->GetOceanRenderFlags();
}

void OceanInterop::set_RenderOptions(byte flags)
{
	gEnv->p3DEngine->SetOceanRenderFlags(flags);
}

uint OceanInterop::get_VisiblePixelCount()
{
	return gEnv->p3DEngine->GetOceanVisiblePixelsCount();
}

float OceanInterop::get_WaterLevel()
{
	return gEnv->p3DEngine->GetWaterLevel();
}

Vec4 OceanInterop::get_CausticsParameters()
{
	return gEnv->p3DEngine->GetCausticsParams();
}

Vec4 OceanInterop::get_CausticsAnimationParameters()
{
	return gEnv->p3DEngine->GetOceanAnimationCausticsParams();
}

OceanAnimation OceanInterop::get_AnimationParameters()
{
	Vec4 out1, out2;
	gEnv->p3DEngine->GetOceanAnimationParams(out1, out2);
	OceanAnimation anim;
	anim.WindDirection = out1.x;
	anim.WindSpeed     = out1.y;
	anim.WavesSpeed    = out1.z;
	anim.WavesAmount   = out1.w;
	anim.WavesSize     = out2.x;

	return anim;
}

bool OceanInterop::IsUnderwater(Vec3 &pos)
{
	return gEnv->p3DEngine->IsUnderWater(pos);
}

float OceanInterop::GetBottomLevel(Vec3 &position, float maxRelevantDepth, int objectFlags)
{
	if (objectFlags != 0)
	{
		return gEnv->p3DEngine->GetBottomLevel(position, maxRelevantDepth, objectFlags);
	}
	return gEnv->p3DEngine->GetBottomLevel(position, maxRelevantDepth);
}

float OceanInterop::GetWaterLevel(Vec3 &position, bool accurate)
{
	return gEnv->p3DEngine->GetWaterLevel(&position, nullptr, accurate);
}
