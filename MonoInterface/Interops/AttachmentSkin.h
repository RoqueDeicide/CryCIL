#pragma once

#include "IMonoInterface.h"

struct IAttachmentSkin;
struct ISkin;

struct AttachmentSkinInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AttachmentSkin"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Attachments"; }

	virtual void InitializeInterops() override;

	static void   AddRef(IAttachmentSkin *handle);
	static void   Release(IAttachmentSkin *handle);
	static ISkin *GetISkin(IAttachmentSkin *handle);
	static void   GetRandomPos(IAttachmentSkin *handle, EGeomForm aspect, CRndGen &seed, PosNorm &ran);
};