#pragma once

#include "IMonoInterface.h"

struct PostProcessingInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PostProcessing"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void InitializeInterops() override;

	static void SetPostEffectParam(mono::string pParam, float fValue, bool bForceValue);
	static void SetPostEffectParamVec4(mono::string pParam, const Vec4 &pValue, bool bForceValue);
	static void SetPostEffectParamString(mono::string pParam, mono::string pszArg);
	static void GetPostEffectParam(mono::string pParam, float &fValue);
	static void GetPostEffectParamVec4(mono::string pParam, Vec4 &pValue);
	static void GetPostEffectParamString(mono::string pParam, mono::string &pszArg);
	static void ResetPostEffects(bool bOnSpecChange);
};