#pragma once

#include "IMonoInterface.h"

struct CryEntityRenderProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryEntityRenderProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Logic.EntityProxies"; }

	virtual void OnRunTimeInitialized() override;

	static void GetWorldBounds(IEntityRenderProxy *handle, AABB &bounds);
	static void GetLocalBounds(IEntityRenderProxy *handle, AABB &bounds);
	static void SetLocalBounds(IEntityRenderProxy *handle, const AABB &bounds, bool bDoNotRecalculate);
	static void InvalidateLocalBounds(IEntityRenderProxy *handle);
	static IMaterial *GetRenderMaterialInternal(IEntityRenderProxy *handle, int nSlot);
	static void SetSlotMaterial(IEntityRenderProxy *handle, int nSlot, IMaterial *pMaterial);
	static IMaterial *GetSlotMaterial(IEntityRenderProxy *handle, int nSlot);
	static IRenderNode *GetRenderNode(IEntityRenderProxy *handle);
	static void SetMaterialLayersMask(IEntityRenderProxy *handle, uint8 nMtlLayersMask);
	static uint8 GetMaterialLayersMask(IEntityRenderProxy *handle);
	static void SetMaterialLayersBlend(IEntityRenderProxy *handle, uint32 nMtlLayersBlend);
	static uint8 GetMaterialLayersBlend(IEntityRenderProxy *handle);
	static void SetCloakInterferenceState(IEntityRenderProxy *handle, bool bHasCloakInterference);
	static bool GetCloakInterferenceState(IEntityRenderProxy *handle);
	static void SetCloakHighlightStrength(IEntityRenderProxy *handle, float highlightStrength);
	static void SetCloakColorChannel(IEntityRenderProxy *handle, byte nCloakColorChannel);
	static byte GetCloakColorChannel(IEntityRenderProxy *handle);
	static void SetCloakFadeByDistance(IEntityRenderProxy *handle, bool bCloakFadeByDistance);
	static bool DoesCloakFadeByDistance(IEntityRenderProxy *handle);
	static void SetCloakBlendTimeScale(IEntityRenderProxy *handle, float fCloakBlendTimeScale);
	static float GetCloakBlendTimeScale(IEntityRenderProxy *handle);
	static void SetIgnoreCloakRefractionColor(IEntityRenderProxy *handle, bool bIgnoreCloakRefractionColor);
	static bool DoesIgnoreCloakRefractionColor(IEntityRenderProxy *handle);
	static void SetCustomPostEffect(IEntityRenderProxy *handle, mono::string pPostEffectName);
	static void SetAsPost3dRenderObjectInternal(IEntityRenderProxy *handle, bool bPost3dRenderObject,
												byte groupId, const Vec4 &groupScreenRect);
	static void SetIgnoreHudInterferenceFilter(IEntityRenderProxy *handle, bool bIgnoreFiler);
	static void SetHUDRequireDepthTest(IEntityRenderProxy *handle, bool bRequire);
	static void SetHUDDisableBloom(IEntityRenderProxy *handle, bool bDisableBloom);
	static void SetIgnoreHeatAmount(IEntityRenderProxy *handle, bool bIgnoreHeat);
	static void SetVisionParams(IEntityRenderProxy *handle, float r, float g, float b, float a);
	static uint GetVisionParams(IEntityRenderProxy *handle);
	static void SetHUDSilhouettesParams(IEntityRenderProxy *handle, float r, float g, float b, float a);
	static uint GetHUDSilhouettesParams(IEntityRenderProxy *handle);
	static void SetShadowDissolve(IEntityRenderProxy *handle, bool enable);
	static bool GetShadowDissolve(IEntityRenderProxy *handle);
	static void SetEffectLayerParams(IEntityRenderProxy *handle, const Vec4 &pParams);
	static void SetEffectLayerParamsEnc(IEntityRenderProxy *handle, uint nEncodedParams);
	static uint GetEffectLayerParams(IEntityRenderProxy *handle);
	static void SetOpacity(IEntityRenderProxy *handle, float fAmount);
	static float GetOpacity(IEntityRenderProxy *handle);
	static float GetLastSeenTime(IEntityRenderProxy *handle);
	static void ClearSlotsInternal(IEntityRenderProxy *handle);
	static void SetMotionBlur(IEntityRenderProxy *handle, bool enable);
	static void SetViewDistRatio(IEntityRenderProxy *handle, int nViewDistRatio);
	static void SetLodRatio(IEntityRenderProxy *handle, int nLodRatio);
};