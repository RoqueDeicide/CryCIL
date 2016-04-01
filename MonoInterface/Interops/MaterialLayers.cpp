#include "stdafx.h"

#include "MaterialLayers.h"

void MaterialLayerCollectionInterop::InitializeInterops()
{
	REGISTER_METHOD(SetLayerCount);
	REGISTER_METHOD(GetLayerCount);
	REGISTER_METHOD(SetLayer);
	REGISTER_METHOD(SetLayerChecked);
	REGISTER_METHOD(GetLayer);
	REGISTER_METHOD(GetLayerChecked);
}

void MaterialLayerCollectionInterop::SetLayerCount(IMaterial *handle, uint nCount)
{
	handle->SetLayerCount(nCount);
}

uint MaterialLayerCollectionInterop::GetLayerCount(IMaterial *handle)
{
	return handle->GetLayerCount();
}

void MaterialLayerCollectionInterop::SetLayer(IMaterial *handle, uint nSlot, IMaterialLayer *pLayer)
{
	handle->SetLayer(nSlot, pLayer);
}

void MaterialLayerCollectionInterop::SetLayerChecked(IMaterial *handle, uint nSlot, IMaterialLayer *pLayer, bool &error)
{
	if (nSlot >= handle->GetLayerCount())
	{
		error = true;
		return;
	}
	error = false;
	handle->SetLayer(nSlot, pLayer);
}

IMaterialLayer *MaterialLayerCollectionInterop::GetLayer(IMaterial *handle, uint nSlot)
{
	return const_cast<IMaterialLayer *>(handle->GetLayer(nSlot));
}

IMaterialLayer *MaterialLayerCollectionInterop::GetLayerChecked(IMaterial *handle, uint nSlot, bool &error)
{
	if (nSlot >= handle->GetLayerCount())
	{
		error = true;
		return nullptr;
	}
	error = false;
	return const_cast<IMaterialLayer *>(handle->GetLayer(nSlot));
}

void MaterialLayerInterop::InitializeInterops()
{
	REGISTER_CTOR(Ctor);

	REGISTER_METHOD(EnableInternal);
	REGISTER_METHOD(IsEnabled);
	REGISTER_METHOD(FadeOutInternal);
	REGISTER_METHOD(DoesFadeOutInternal);
	REGISTER_METHOD(GetShaderItem);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
}

IMaterialLayer *MaterialLayerInterop::Ctor(IMaterial *mat)
{
	return mat->CreateLayer();
}

void MaterialLayerInterop::EnableInternal(IMaterialLayer *handle, bool bEnable)
{
	handle->Enable(bEnable);
}

bool MaterialLayerInterop::IsEnabled(IMaterialLayer *handle)
{
	return handle->IsEnabled();
}

void MaterialLayerInterop::FadeOutInternal(IMaterialLayer *handle, bool bFadeOut)
{
	handle->FadeOut(bFadeOut);
}

bool MaterialLayerInterop::DoesFadeOutInternal(IMaterialLayer *handle)
{
	return handle->DoesFadeOut();
}

SShaderItem MaterialLayerInterop::GetShaderItem(IMaterialLayer *handle)
{
	return handle->GetShaderItem();
}

void MaterialLayerInterop::SetFlags(IMaterialLayer *handle, byte nFlags)
{
	handle->SetFlags(nFlags);
}

byte MaterialLayerInterop::GetFlags(IMaterialLayer *handle)
{
	return handle->GetFlags();
}
