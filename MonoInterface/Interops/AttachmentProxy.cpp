#include "stdafx.h"

#include "AttachmentProxy.h"
#include <IAttachment.h>

void AttachmentProxyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(GetNameCrc);
	REGISTER_METHOD(ReName);
	REGISTER_METHOD(SetJointName);
	REGISTER_METHOD(GetJointId);
	REGISTER_METHOD(GetProxyAbsoluteDefault);
	REGISTER_METHOD(GetProxyRelativeDefault);
	REGISTER_METHOD(GetProxyModelRelative);
	REGISTER_METHOD(SetProxyAbsoluteDefault);
	REGISTER_METHOD(ProjectProxyInternal);
	REGISTER_METHOD(AlignProxyWithJoint);
	REGISTER_METHOD(GetProxyParams);
	REGISTER_METHOD(SetProxyParams);
	REGISTER_METHOD(GetProxyPurpose);
	REGISTER_METHOD(SetProxyPurpose);
	REGISTER_METHOD(SetHideProxy);
}

mono::string AttachmentProxyInterop::GetName(IProxy *handle)
{
	return ToMonoString(handle->GetName());
}

uint AttachmentProxyInterop::GetNameCrc(IProxy *handle)
{
	return handle->GetNameCRC();
}

uint AttachmentProxyInterop::ReName(IProxy *handle, mono::string szSocketName, uint crc)
{
	return handle->ReName(NtText(szSocketName), crc);
}

uint AttachmentProxyInterop::SetJointName(IProxy *handle, mono::string szJointName)
{
	return handle->SetJointName(NtText(szJointName));
}

uint AttachmentProxyInterop::GetJointId(IProxy *handle)
{
	return handle->GetJointID();
}

QuatT AttachmentProxyInterop::GetProxyAbsoluteDefault(IProxy *handle)
{
	return handle->GetProxyAbsoluteDefault();
}

QuatT AttachmentProxyInterop::GetProxyRelativeDefault(IProxy *handle)
{
	return handle->GetProxyRelativeDefault();
}

QuatT AttachmentProxyInterop::GetProxyModelRelative(IProxy *handle)
{
	return handle->GetProxyModelRelative();
}

void AttachmentProxyInterop::SetProxyAbsoluteDefault(IProxy *handle, const QuatT &rot)
{
	handle->SetProxyAbsoluteDefault(rot);
}

uint AttachmentProxyInterop::ProjectProxyInternal(IProxy *handle)
{
	return handle->ProjectProxy();
}

void AttachmentProxyInterop::AlignProxyWithJoint(IProxy *handle)
{
	handle->AlignProxyWithJoint();
}

Vec4 AttachmentProxyInterop::GetProxyParams(IProxy *handle)
{
	return handle->GetProxyParams();
}

void AttachmentProxyInterop::SetProxyParams(IProxy *handle, const Vec4 &parameters)
{
	handle->SetProxyParams(parameters);
}

int8 AttachmentProxyInterop::GetProxyPurpose(IProxy *handle)
{
	return handle->GetProxyPurpose();
}

void AttachmentProxyInterop::SetProxyPurpose(IProxy *handle, int8 p)
{
	handle->SetProxyPurpose(p);
}

void AttachmentProxyInterop::SetHideProxy(IProxy *handle, uint8 nHideProxy)
{
	handle->SetHideProxy(nHideProxy);
}
