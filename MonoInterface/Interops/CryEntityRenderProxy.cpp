#include "stdafx.h"

#include "CryEntityRenderProxy.h"

void CryEntityRenderProxyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetWorldBounds);
	REGISTER_METHOD(GetLocalBounds);
	REGISTER_METHOD(SetLocalBounds);
	REGISTER_METHOD(InvalidateLocalBounds);
	REGISTER_METHOD(GetRenderMaterialInternal);
	REGISTER_METHOD(SetSlotMaterial);
	REGISTER_METHOD(GetSlotMaterial);
	REGISTER_METHOD(GetRenderNode);
	REGISTER_METHOD(SetMaterialLayersMask);
	REGISTER_METHOD(GetMaterialLayersMask);
	REGISTER_METHOD(SetMaterialLayersBlend);
	REGISTER_METHOD(GetMaterialLayersBlend);
	REGISTER_METHOD(SetCloakInterferenceState);
	REGISTER_METHOD(GetCloakInterferenceState);
	REGISTER_METHOD(SetCloakHighlightStrength);
	REGISTER_METHOD(SetCloakColorChannel);
	REGISTER_METHOD(GetCloakColorChannel);
	REGISTER_METHOD(SetCloakFadeByDistance);
	REGISTER_METHOD(DoesCloakFadeByDistance);
	REGISTER_METHOD(SetCloakBlendTimeScale);
	REGISTER_METHOD(GetCloakBlendTimeScale);
	REGISTER_METHOD(SetIgnoreCloakRefractionColor);
	REGISTER_METHOD(DoesIgnoreCloakRefractionColor);
	REGISTER_METHOD(SetCustomPostEffect);
	REGISTER_METHOD(SetAsPost3dRenderObjectInternal);
	REGISTER_METHOD(SetIgnoreHudInterferenceFilter);
	REGISTER_METHOD(SetHUDRequireDepthTest);
	REGISTER_METHOD(SetHUDDisableBloom);
	REGISTER_METHOD(SetIgnoreHeatAmount);
	REGISTER_METHOD(SetVisionParams);
	REGISTER_METHOD(GetVisionParams);
	REGISTER_METHOD(SetHUDSilhouettesParams);
	REGISTER_METHOD(GetHUDSilhouettesParams);
	REGISTER_METHOD(SetShadowDissolve);
	REGISTER_METHOD(GetShadowDissolve);
	REGISTER_METHOD(SetEffectLayerParams);
	REGISTER_METHOD(SetEffectLayerParamsEnc);
	REGISTER_METHOD(GetEffectLayerParams);
	REGISTER_METHOD(SetOpacity);
	REGISTER_METHOD(GetOpacity);
	REGISTER_METHOD(GetLastSeenTime);
	REGISTER_METHOD(ClearSlotsInternal);
	REGISTER_METHOD(SetMotionBlur);
	REGISTER_METHOD(SetViewDistRatio);
	REGISTER_METHOD(SetLodRatio);
}

void CryEntityRenderProxyInterop::GetWorldBounds(IEntityRenderProxy *handle, AABB &bounds)
{
	handle->GetWorldBounds(bounds);
}

void CryEntityRenderProxyInterop::GetLocalBounds(IEntityRenderProxy *handle, AABB &bounds)
{
	handle->GetLocalBounds(bounds);
}

void CryEntityRenderProxyInterop::SetLocalBounds(IEntityRenderProxy *handle, const AABB &bounds,
												 bool bDoNotRecalculate)
{
	handle->SetLocalBounds(bounds, bDoNotRecalculate);
}

void CryEntityRenderProxyInterop::InvalidateLocalBounds(IEntityRenderProxy *handle)
{
	handle->InvalidateLocalBounds();
}

IMaterial *CryEntityRenderProxyInterop::GetRenderMaterialInternal(IEntityRenderProxy *handle, int nSlot)
{
	return handle->GetRenderMaterial(nSlot);
}

void CryEntityRenderProxyInterop::SetSlotMaterial(IEntityRenderProxy *handle, int nSlot, IMaterial *pMaterial)
{
	handle->SetSlotMaterial(nSlot, pMaterial);
}

IMaterial *CryEntityRenderProxyInterop::GetSlotMaterial(IEntityRenderProxy *handle, int nSlot)
{
	return handle->GetSlotMaterial(nSlot);
}

IRenderNode *CryEntityRenderProxyInterop::GetRenderNode(IEntityRenderProxy *handle)
{
	return handle->GetRenderNode();
}

void CryEntityRenderProxyInterop::SetMaterialLayersMask(IEntityRenderProxy *handle, uint8 nMtlLayersMask)
{
	handle->SetMaterialLayersMask(nMtlLayersMask);
}

uint8 CryEntityRenderProxyInterop::GetMaterialLayersMask(IEntityRenderProxy *handle)
{
	return handle->GetMaterialLayersMask();
}

void CryEntityRenderProxyInterop::SetMaterialLayersBlend(IEntityRenderProxy *handle, uint32 nMtlLayersBlend)
{
	handle->SetMaterialLayersBlend(nMtlLayersBlend);
}

uint8 CryEntityRenderProxyInterop::GetMaterialLayersBlend(IEntityRenderProxy *handle)
{
	return handle->GetMaterialLayersBlend();
}

void CryEntityRenderProxyInterop::SetCloakInterferenceState(IEntityRenderProxy *handle,
															bool bHasCloakInterference)
{
	handle->SetCloakInterferenceState(bHasCloakInterference);
}

bool CryEntityRenderProxyInterop::GetCloakInterferenceState(IEntityRenderProxy *handle)
{
	return handle->GetCloakInterferenceState();
}

void CryEntityRenderProxyInterop::SetCloakHighlightStrength(IEntityRenderProxy *handle, float highlightStrength)
{
	handle->SetCloakHighlightStrength(highlightStrength);
}

void CryEntityRenderProxyInterop::SetCloakColorChannel(IEntityRenderProxy *handle, byte nCloakColorChannel)
{
	handle->SetCloakColorChannel(nCloakColorChannel);
}

byte CryEntityRenderProxyInterop::GetCloakColorChannel(IEntityRenderProxy *handle)
{
	return handle->GetCloakColorChannel();
}

void CryEntityRenderProxyInterop::SetCloakFadeByDistance(IEntityRenderProxy *handle, bool bCloakFadeByDistance)
{
	handle->SetCloakFadeByDistance(bCloakFadeByDistance);
}

bool CryEntityRenderProxyInterop::DoesCloakFadeByDistance(IEntityRenderProxy *handle)
{
	return handle->DoesCloakFadeByDistance();
}

void CryEntityRenderProxyInterop::SetCloakBlendTimeScale(IEntityRenderProxy *handle, float fCloakBlendTimeScale)
{
	handle->SetCloakBlendTimeScale(fCloakBlendTimeScale);
}

float CryEntityRenderProxyInterop::GetCloakBlendTimeScale(IEntityRenderProxy *handle)
{
	return handle->GetCloakBlendTimeScale();
}

void CryEntityRenderProxyInterop::SetIgnoreCloakRefractionColor(IEntityRenderProxy *handle,
																bool bIgnoreCloakRefractionColor)
{
	handle->SetIgnoreCloakRefractionColor(bIgnoreCloakRefractionColor);
}

bool CryEntityRenderProxyInterop::DoesIgnoreCloakRefractionColor(IEntityRenderProxy *handle)
{
	return handle->DoesIgnoreCloakRefractionColor();
}

void CryEntityRenderProxyInterop::SetCustomPostEffect(IEntityRenderProxy *handle, mono::string pPostEffectName)
{
	handle->SetCustomPostEffect(NtText(pPostEffectName));
}

void CryEntityRenderProxyInterop::SetAsPost3dRenderObjectInternal(IEntityRenderProxy *handle,
																  bool bPost3dRenderObject, byte groupId,
																  const Vec4 &groupScreenRect)
{
	handle->SetAsPost3dRenderObject(bPost3dRenderObject, groupId,
									const_cast<float *>(reinterpret_cast<const float *>(&groupScreenRect)));
}

void CryEntityRenderProxyInterop::SetIgnoreHudInterferenceFilter(IEntityRenderProxy *handle, bool bIgnoreFiler)
{
	handle->SetIgnoreHudInterferenceFilter(bIgnoreFiler);
}

void CryEntityRenderProxyInterop::SetHUDRequireDepthTest(IEntityRenderProxy *handle, bool bRequire)
{
	handle->SetHUDRequireDepthTest(bRequire);
}

void CryEntityRenderProxyInterop::SetHUDDisableBloom(IEntityRenderProxy *handle, bool bDisableBloom)
{
	handle->SetHUDDisableBloom(bDisableBloom);
}

void CryEntityRenderProxyInterop::SetIgnoreHeatAmount(IEntityRenderProxy *handle, bool bIgnoreHeat)
{
	handle->SetIgnoreHeatAmount(bIgnoreHeat);
}

void CryEntityRenderProxyInterop::SetVisionParams(IEntityRenderProxy *handle, float r, float g, float b, float a)
{
	handle->SetVisionParams(r, g, b, a);
}

uint CryEntityRenderProxyInterop::GetVisionParams(IEntityRenderProxy *handle)
{
	return handle->GetVisionParams();
}

void CryEntityRenderProxyInterop::SetHUDSilhouettesParams(IEntityRenderProxy *handle, float r, float g, float b,
														  float a)
{
	handle->SetHUDSilhouettesParams(r, g, b, a);
}

uint CryEntityRenderProxyInterop::GetHUDSilhouettesParams(IEntityRenderProxy *handle)
{
	return handle->GetHUDSilhouettesParams();
}

void CryEntityRenderProxyInterop::SetShadowDissolve(IEntityRenderProxy *handle, bool enable)
{
	handle->SetShadowDissolve(enable);
}

bool CryEntityRenderProxyInterop::GetShadowDissolve(IEntityRenderProxy *handle)
{
	return handle->GetShadowDissolve();
}

void CryEntityRenderProxyInterop::SetEffectLayerParams(IEntityRenderProxy *handle, const Vec4 &pParams)
{
	handle->SetEffectLayerParams(pParams);
}

void CryEntityRenderProxyInterop::SetEffectLayerParamsEnc(IEntityRenderProxy *handle, uint nEncodedParams)
{
	handle->SetEffectLayerParams(nEncodedParams);
}

uint CryEntityRenderProxyInterop::GetEffectLayerParams(IEntityRenderProxy *handle)
{
	return handle->GetEffectLayerParams();
}

void CryEntityRenderProxyInterop::SetOpacity(IEntityRenderProxy *handle, float fAmount)
{
	handle->SetOpacity(fAmount);
}

float CryEntityRenderProxyInterop::GetOpacity(IEntityRenderProxy *handle)
{
	return handle->GetOpacity();
}

float CryEntityRenderProxyInterop::GetLastSeenTime(IEntityRenderProxy *handle)
{
	return handle->GetLastSeenTime();
}

void CryEntityRenderProxyInterop::ClearSlotsInternal(IEntityRenderProxy *handle)
{
	handle->ClearSlots();
}

void CryEntityRenderProxyInterop::SetMotionBlur(IEntityRenderProxy *handle, bool enable)
{
	handle->SetMotionBlur(enable);
}

void CryEntityRenderProxyInterop::SetViewDistRatio(IEntityRenderProxy *handle, int nViewDistRatio)
{
	handle->SetViewDistRatio(nViewDistRatio);
}

void CryEntityRenderProxyInterop::SetLodRatio(IEntityRenderProxy *handle, int nLodRatio)
{
	handle->SetLodRatio(nLodRatio);
}
