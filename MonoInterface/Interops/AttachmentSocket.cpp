#include "stdafx.h"

#include "AttachmentSocket.h"

void AttachmentSocketInterop::InitializeInterops()
{
	REGISTER_METHOD(AddRef);
	REGISTER_METHOD(Release);
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(GetNameCRC);
	REGISTER_METHOD(ReName);
	REGISTER_METHOD(GetType);
	REGISTER_METHOD(SetJointName);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetAttAbsoluteDefault);
	REGISTER_METHOD(SetAttAbsoluteDefault);
	REGISTER_METHOD(SetAttRelativeDefault);
	REGISTER_METHOD(GetAttRelativeDefault);
	REGISTER_METHOD(GetAttModelRelative);
	REGISTER_METHOD(GetAttWorldAbsolute);
	REGISTER_METHOD(UpdateAttModelRelative);
	REGISTER_METHOD(HideAttachment);
	REGISTER_METHOD(IsAttachmentHidden);
	REGISTER_METHOD(HideInRecursion);
	REGISTER_METHOD(IsAttachmentHiddenInRecursion);
	REGISTER_METHOD(HideInShadow);
	REGISTER_METHOD(IsAttachmentHiddenInShadow);
	REGISTER_METHOD(AlignJointAttachment);
	REGISTER_METHOD(GetJointID);
	REGISTER_METHOD(AddBinding);
	REGISTER_METHOD(GetIAttachmentObject);
	REGISTER_METHOD(GetIAttachmentSkin);
	REGISTER_METHOD(ClearBinding);
	REGISTER_METHOD(SwapBinding);
	REGISTER_METHOD(GetSimulationParams);
	REGISTER_METHOD(GetRowParams);
	REGISTER_METHOD(Serialize);
}

void AttachmentSocketInterop::AddRef(IAttachment *handle)
{
	handle->AddRef();
}

void AttachmentSocketInterop::Release(IAttachment *handle)
{
	handle->Release();
}

mono::string AttachmentSocketInterop::GetName(IAttachment *handle)
{
	return ToMonoString(handle->GetName());
}

uint32 AttachmentSocketInterop::GetNameCRC(IAttachment *handle)
{
	return handle->GetNameCRC();
}

uint32 AttachmentSocketInterop::ReName(IAttachment *handle, mono::string szSocketName, uint crc)
{
	return handle->ReName(NtText(szSocketName), crc);
}

uint32 AttachmentSocketInterop::GetType(IAttachment *handle)
{
	return handle->GetType();
}

uint32 AttachmentSocketInterop::SetJointName(IAttachment *handle, mono::string szJointName)
{
	return handle->SetJointName(NtText(szJointName));
}

uint AttachmentSocketInterop::GetFlags(IAttachment *handle)
{
	return handle->GetFlags();
}

void AttachmentSocketInterop::SetFlags(IAttachment *handle, uint flags)
{
	handle->SetFlags(flags);
}

QuatT AttachmentSocketInterop::GetAttAbsoluteDefault(IAttachment *handle)
{
	return handle->GetAttAbsoluteDefault();
}

void AttachmentSocketInterop::SetAttAbsoluteDefault(IAttachment *handle, const QuatT &rot)
{
	handle->SetAttAbsoluteDefault(rot);
}

void AttachmentSocketInterop::SetAttRelativeDefault(IAttachment *handle, const QuatT &mat)
{
	handle->SetAttRelativeDefault(mat);
}

QuatT AttachmentSocketInterop::GetAttRelativeDefault(IAttachment *handle)
{
	return handle->GetAttRelativeDefault();
}

QuatT AttachmentSocketInterop::GetAttModelRelative(IAttachment *handle)
{
	return handle->GetAttModelRelative();
}

QuatTS AttachmentSocketInterop::GetAttWorldAbsolute(IAttachment *handle)
{
	return handle->GetAttWorldAbsolute();
}

void AttachmentSocketInterop::UpdateAttModelRelative(IAttachment *handle)
{
	handle->UpdateAttModelRelative();
}

void AttachmentSocketInterop::HideAttachment(IAttachment *handle, uint x)
{
	handle->HideAttachment(x);
}

uint AttachmentSocketInterop::IsAttachmentHidden(IAttachment *handle)
{
	return handle->IsAttachmentHidden();
}

void AttachmentSocketInterop::HideInRecursion(IAttachment *handle, uint x)
{
	handle->HideInRecursion(x);
}

uint AttachmentSocketInterop::IsAttachmentHiddenInRecursion(IAttachment *handle)
{
	return handle->IsAttachmentHiddenInRecursion();
}

void AttachmentSocketInterop::HideInShadow(IAttachment *handle, uint x)
{
	handle->HideInShadow(x);
}

uint AttachmentSocketInterop::IsAttachmentHiddenInShadow(IAttachment *handle)
{
	return handle->IsAttachmentHiddenInShadow();
}

void AttachmentSocketInterop::AlignJointAttachment(IAttachment *handle)
{
	handle->AlignJointAttachment();
}

uint AttachmentSocketInterop::GetJointID(IAttachment *handle)
{
	return handle->GetJointID();
}

void AttachmentSocketInterop::AddBinding(IAttachment *handle, IAttachmentObject *pModel, ISkin *pISkin)
{
	return handle->AddBinding(pModel, pISkin);
}

IAttachmentObject *AttachmentSocketInterop::GetIAttachmentObject(IAttachment *handle, IAttachmentObject::EType *type)
{
	auto obj = handle->GetIAttachmentObject();
	*type = obj->GetAttachmentType();

	return obj;
}

IAttachmentSkin *AttachmentSocketInterop::GetIAttachmentSkin(IAttachment *handle)
{
	return handle->GetIAttachmentSkin();
}

void AttachmentSocketInterop::ClearBinding(IAttachment *handle, uint nLoadingFlags /*= 0*/)
{
	handle->ClearBinding(nLoadingFlags);
}

void AttachmentSocketInterop::SwapBinding(IAttachment *handle, IAttachment *pNewAttachment)
{
	return handle->SwapBinding(pNewAttachment);
}

SimulationParams *AttachmentSocketInterop::GetSimulationParams(IAttachment *handle)
{
	return &handle->GetSimulationParams();
}

RowSimulationParams *AttachmentSocketInterop::GetRowParams(IAttachment *handle)
{
	return &handle->GetRowParams();
}

void AttachmentSocketInterop::Serialize(IAttachment *handle, ISerialize *ser)
{
	handle->Serialize(ser);
}
