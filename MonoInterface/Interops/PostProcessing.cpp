#include "stdafx.h"

#include "PostProcessing.h"

void PostProcessingInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetPostEffectParam);
	REGISTER_METHOD(SetPostEffectParamVec4);
	REGISTER_METHOD(SetPostEffectParamString);
	REGISTER_METHOD(GetPostEffectParam);
	REGISTER_METHOD(GetPostEffectParamVec4);
	REGISTER_METHOD(GetPostEffectParamString);
	REGISTER_METHOD(ResetPostEffects);
}

void PostProcessingInterop::SetPostEffectParam(mono::string pParam, float fValue, bool bForceValue)
{
	gEnv->p3DEngine->SetPostEffectParam(NtText(pParam), fValue, bForceValue);
}

void PostProcessingInterop::SetPostEffectParamVec4(mono::string pParam, const Vec4 &pValue, bool bForceValue)
{
	gEnv->p3DEngine->SetPostEffectParamVec4(NtText(pParam), pValue, bForceValue);
}

void PostProcessingInterop::SetPostEffectParamString(mono::string pParam, mono::string pszArg)
{
	gEnv->p3DEngine->SetPostEffectParamString(NtText(pParam), NtText(pszArg));
}

void PostProcessingInterop::GetPostEffectParam(mono::string pParam, float &fValue)
{
	gEnv->p3DEngine->GetPostEffectParam(NtText(pParam), fValue);
}

void PostProcessingInterop::GetPostEffectParamVec4(mono::string pParam, Vec4 &pValue)
{
	gEnv->p3DEngine->GetPostEffectParamVec4(NtText(pParam), pValue);
}

void PostProcessingInterop::GetPostEffectParamString(mono::string pParam, mono::string &pszArg)
{
	const char *value;
	gEnv->p3DEngine->GetPostEffectParamString(NtText(pParam), value);
	pszArg = ToMonoString(value);
}

void PostProcessingInterop::ResetPostEffects(bool bOnSpecChange)
{
	gEnv->p3DEngine->ResetPostEffects(bOnSpecChange);
}
