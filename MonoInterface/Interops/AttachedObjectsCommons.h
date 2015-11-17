#pragma once

#include "IMonoInterface.h"

struct IAttachmentSkin;
struct IAttachmentObject;
struct CSKINAttachment;
struct CSKELAttachment;
struct CCGFAttachment;
struct CEntityAttachment;
struct CLightAttachment;
struct CEffectAttachment;
struct IAttachment;

struct AttachedObjectsCommonsInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AttachedObjectsCommons"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Attachments"; }

	virtual void OnRunTimeInitialized() override;

	static void                ProcessAttachment(IAttachmentObject *handle, IAttachment *pIAttachment);
	static AABB                GetAabb(IAttachmentObject *handle);
	static float               GetRadiusSqr(IAttachmentObject *handle);
	static IStatObj           *GetIStatObj(CCGFAttachment *handle);
	static void                SetIStatObj(CCGFAttachment *handle, IStatObj *staticObject);
	static ICharacterInstance *GetICharacterInstance(CSKELAttachment *handle);
	static void                SetICharacterInstance(CSKELAttachment *handle, ICharacterInstance *character);
	static IAttachmentSkin    *GetIAttachmentSkin(CSKINAttachment *handle);
	static void                SetIAttachmentSkin(CSKINAttachment *handle, IAttachmentSkin *skin);
	static mono::string        GetObjectFilePath(IAttachmentObject *handle);
	static IMaterial          *GetBaseMaterial(IAttachmentObject *handle, uint nLod);
	static void                SetReplacementMaterial(IAttachmentObject *handle, IMaterial *pMaterial, uint nLod);
	static IMaterial          *GetReplacementMaterial(IAttachmentObject *handle, uint nLod);
	static void                Release(IAttachmentObject *handle);
	static void                SetEntityId(CEntityAttachment *handle, EntityId id);
	static EntityId            GetEntityId(CEntityAttachment *handle);
	static void                LoadLight(CLightAttachment *handle, const CDLight &light);
	static ILightSource       *GetLightSource(CLightAttachment *handle);
	static IParticleEmitter   *GetEmitter(CEffectAttachment *handle);
	static CEffectAttachment  *LoadEffectAttachment(mono::string effectName, const Vec3 &offset, const Vec3 &dir,
													float scale);
	static CEffectAttachment  *CreateEffectAttachment(IParticleEffect *pParticleEffect, const Vec3 &offset,
													  const Vec3 &dir, float scale);
};