#include "stdafx.h"

#include "AttachedObjectsCommons.h"
#include <IAttachment.h>

void AttachedObjectsCommonsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(ProcessAttachment);
	REGISTER_METHOD(GetAabb);
	REGISTER_METHOD(GetRadiusSqr);
	REGISTER_METHOD(GetIStatObj);
	REGISTER_METHOD(SetIStatObj);
	REGISTER_METHOD(GetICharacterInstance);
	REGISTER_METHOD(SetICharacterInstance);
	REGISTER_METHOD(GetIAttachmentSkin);
	REGISTER_METHOD(SetIAttachmentSkin);
	REGISTER_METHOD(GetObjectFilePath);
	REGISTER_METHOD(GetBaseMaterial);
	REGISTER_METHOD(SetReplacementMaterial);
	REGISTER_METHOD(GetReplacementMaterial);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(SetEntityId);
	REGISTER_METHOD(GetEntityId);
	REGISTER_METHOD(LoadLight);
	REGISTER_METHOD(GetLightSource);
	REGISTER_METHOD(GetEmitter);
	REGISTER_METHOD(LoadEffectAttachment);
	REGISTER_METHOD(CreateEffectAttachment);
}

void AttachedObjectsCommonsInterop::ProcessAttachment(IAttachmentObject *handle, IAttachment *pIAttachment)
{
	handle->ProcessAttachment(pIAttachment);
}

AABB AttachedObjectsCommonsInterop::GetAabb(IAttachmentObject *handle)
{
	return handle->GetAABB();
}

float AttachedObjectsCommonsInterop::GetRadiusSqr(IAttachmentObject *handle)
{
	return handle->GetRadiusSqr();
}

IStatObj *AttachedObjectsCommonsInterop::GetIStatObj(CCGFAttachment *handle)
{
	return handle->pObj;
}

void AttachedObjectsCommonsInterop::SetIStatObj(CCGFAttachment *handle, IStatObj *staticObject)
{
	handle->pObj = staticObject;
}

ICharacterInstance *AttachedObjectsCommonsInterop::GetICharacterInstance(CSKELAttachment *handle)
{
	return handle->m_pCharInstance;
}

void AttachedObjectsCommonsInterop::SetICharacterInstance(CSKELAttachment *handle, ICharacterInstance *character)
{
	handle->m_pCharInstance = character;
}

IAttachmentSkin *AttachedObjectsCommonsInterop::GetIAttachmentSkin(CSKINAttachment *handle)
{
	return handle->m_pIAttachmentSkin;
}

void AttachedObjectsCommonsInterop::SetIAttachmentSkin(CSKINAttachment *handle, IAttachmentSkin *skin)
{
	handle->m_pIAttachmentSkin = skin;
}

mono::string AttachedObjectsCommonsInterop::GetObjectFilePath(IAttachmentObject *handle)
{
	return ToMonoString(handle->GetObjectFilePath());
}

IMaterial *AttachedObjectsCommonsInterop::GetBaseMaterial(IAttachmentObject *handle, uint nLod)
{
	return handle->GetBaseMaterial(nLod);
}

void AttachedObjectsCommonsInterop::SetReplacementMaterial(IAttachmentObject *handle, IMaterial *pMaterial, uint nLod)
{
	handle->SetReplacementMaterial(pMaterial, nLod);
}

IMaterial *AttachedObjectsCommonsInterop::GetReplacementMaterial(IAttachmentObject *handle, uint nLod)
{
	return handle->GetReplacementMaterial(nLod);
}

void AttachedObjectsCommonsInterop::Release(IAttachmentObject *handle)
{
	handle->Release();
}

void AttachedObjectsCommonsInterop::SetEntityId(CEntityAttachment *handle, EntityId id)
{
	handle->SetEntityId(id);
}

EntityId AttachedObjectsCommonsInterop::GetEntityId(CEntityAttachment *handle)
{
	return handle->GetEntityId();
}

void AttachedObjectsCommonsInterop::LoadLight(CLightAttachment *handle, const CDLight &light)
{
	handle->LoadLight(light);
}

ILightSource *AttachedObjectsCommonsInterop::GetLightSource(CLightAttachment *handle)
{
	return handle->GetLightSource();
}

IParticleEmitter *AttachedObjectsCommonsInterop::GetEmitter(CEffectAttachment *handle)
{
	return handle->GetEmitter();
}

CEffectAttachment *AttachedObjectsCommonsInterop::LoadEffectAttachment(mono::string effectName, const Vec3 &offset,
																	   const Vec3 &dir, float scale)
{
	return new CEffectAttachment(NtText(effectName), offset, dir, scale);
}

CEffectAttachment *AttachedObjectsCommonsInterop::CreateEffectAttachment(IParticleEffect *pParticleEffect,
																		 const Vec3 &offset, const Vec3 &dir, float scale)
{
	return new CEffectAttachment(pParticleEffect, offset, dir, scale);
}
