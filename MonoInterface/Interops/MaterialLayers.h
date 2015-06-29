#pragma once

#include "IMonoInterface.h"

struct MaterialLayerCollectionInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "MaterialLayerCollection"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized() override;

	static void SetLayerCount(IMaterial *handle, uint nCount);
	static uint GetLayerCount(IMaterial *handle);
	static void SetLayer(IMaterial *handle, uint nSlot, IMaterialLayer *pLayer);
	static void SetLayerChecked(IMaterial *handle, uint nSlot, IMaterialLayer *pLayer);
	static IMaterialLayer *GetLayer(IMaterial *handle, uint nSlot);
	static IMaterialLayer *GetLayerChecked(IMaterial *handle, uint nSlot);
};

struct MaterialLayerInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "MaterialLayerCollection"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Rendering"; }

	virtual void OnRunTimeInitialized() override;

	static void Ctor(IMaterialLayer **handle, IMaterial *mat);
	static void EnableInternal(IMaterialLayer *handle, bool bEnable);
	static bool IsEnabled(IMaterialLayer *handle);
	static void FadeOutInternal(IMaterialLayer *handle, bool bFadeOut);
	static bool DoesFadeOutInternal(IMaterialLayer *handle);
	static SShaderItem GetShaderItem(IMaterialLayer *handle);
	static void SetFlags(IMaterialLayer *handle, byte nFlags);
	static byte GetFlags(IMaterialLayer *handle);
};