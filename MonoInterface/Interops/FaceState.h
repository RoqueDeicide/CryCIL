#pragma once

#include "IMonoInterface.h"

struct IFaceState;

struct FaceStateInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FaceState"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void OnRunTimeInitialized() override;

	static float GetEffectorWeight(IFaceState *handle, int nIndex);
	static void  SetEffectorWeight(IFaceState *handle, int nIndex, float fWeight);
};