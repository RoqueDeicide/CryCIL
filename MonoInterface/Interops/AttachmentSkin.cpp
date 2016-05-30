#include "stdafx.h"

#include "AttachmentSkin.h"
#include <CryAnimation/IAttachment.h>

void AttachmentSkinInterop::InitializeInterops()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(GetISkin);
	REGISTER_METHOD(GetRandomPos);
}

void AttachmentSkinInterop::AddRef(IAttachmentSkin *handle)
{
	handle->AddRef();
}

void AttachmentSkinInterop::Release(IAttachmentSkin *handle)
{
	handle->Release();
}

ISkin *AttachmentSkinInterop::GetISkin(IAttachmentSkin *handle)
{
	return handle->GetISkin();
}

void AttachmentSkinInterop::GetRandomPos(IAttachmentSkin *handle, EGeomForm aspect, CRndGen &seed, PosNorm &ran)
{
	handle->GetExtent(aspect);
	handle->GetRandomPos(ran, seed, aspect);
}
