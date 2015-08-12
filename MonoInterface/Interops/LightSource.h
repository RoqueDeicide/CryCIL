#pragma once

#include "IMonoInterface.h"
#include "MonoLightProperties.h"

struct LightSourceInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "LightSource"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering.Lighting"; }

	virtual void OnRunTimeInitialized() override;

	static void          SetLightProperties(ILightSource *handle, const LightProperties &properties);
	static void          GetLightProperties(ILightSource *handle, LightProperties &properties);
	static void          GetMatrix(ILightSource *handle, Matrix34 &matrix);
	static void          SetMatrix(ILightSource *handle, const Matrix34 &mat);
	static ILightSource *CreateLightSource();
	static void          DeleteLightSource(ILightSource *pLightSource);
};