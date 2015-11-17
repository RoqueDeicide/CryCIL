#include "stdafx.h"

#include "AttachmentManager.h"
#include <IAttachment.h>

void AttachmentManagerInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetSkelInstance);
	REGISTER_METHOD(LoadAttachmentList);
	REGISTER_METHOD(CreateAttachment);
	REGISTER_METHOD(RemoveAttachmentByInterface);
	REGISTER_METHOD(RemoveAttachmentByName);
	REGISTER_METHOD(RemoveAttachmentByNameCrc);
	REGISTER_METHOD(GetInterfaceByName);
	REGISTER_METHOD(GetInterfaceByIndex);
	REGISTER_METHOD(GetInterfaceByNameCrc);
	REGISTER_METHOD(GetAttachmentCount);
	REGISTER_METHOD(GetIndexByName);
	REGISTER_METHOD(GetIndexByNameCrc);
	REGISTER_METHOD(ProjectAllAttachment);
	REGISTER_METHOD(PhysicalizeAttachment);
	REGISTER_METHOD(DephysicalizeAttachment);
	REGISTER_METHOD(GetProxyCount);
	REGISTER_METHOD(CreateProxy);
	REGISTER_METHOD(GetProxyInterfaceByIndex);
	REGISTER_METHOD(GetProxyInterfaceByName);
	REGISTER_METHOD(GetProxyInterfaceByCrc);
	REGISTER_METHOD(GetProxyIndexByName);
	REGISTER_METHOD(RemoveProxyByInterface);
	REGISTER_METHOD(RemoveProxyByName);
	REGISTER_METHOD(RemoveProxyByNameCrc);
	REGISTER_METHOD(DrawProxies);
}

ICharacterInstance *AttachmentManagerInterop::GetSkelInstance(IAttachmentManager *handle)
{
	return handle->GetSkelInstance();
}

uint AttachmentManagerInterop::LoadAttachmentList(IAttachmentManager *handle, mono::string pathname)
{
	return handle->LoadAttachmentList(NtText(pathname));
}

IAttachment *AttachmentManagerInterop::CreateAttachment(IAttachmentManager *handle, mono::string szName, uint type, mono::string szJointName, bool bCallProject)
{
	return handle->CreateAttachment(NtText(szName), type, NtText(szJointName), bCallProject);
}

int AttachmentManagerInterop::RemoveAttachmentByInterface(IAttachmentManager *handle, IAttachment *ptr)
{
	return handle->RemoveAttachmentByInterface(ptr);
}

int AttachmentManagerInterop::RemoveAttachmentByName(IAttachmentManager *handle, mono::string szName)
{
	return handle->RemoveAttachmentByName(NtText(szName));
}

int AttachmentManagerInterop::RemoveAttachmentByNameCrc(IAttachmentManager *handle, uint nameCrc)
{
	return handle->RemoveAttachmentByNameCRC(nameCrc);
}

IAttachment *AttachmentManagerInterop::GetInterfaceByName(IAttachmentManager *handle, mono::string szName)
{
	return handle->GetInterfaceByName(NtText(szName));
}

IAttachment *AttachmentManagerInterop::GetInterfaceByIndex(IAttachmentManager *handle, uint c)
{
	return handle->GetInterfaceByIndex(c);
}

IAttachment *AttachmentManagerInterop::GetInterfaceByNameCrc(IAttachmentManager *handle, uint nameCrc)
{
	return handle->GetInterfaceByNameCRC(nameCrc);
}

int AttachmentManagerInterop::GetAttachmentCount(IAttachmentManager *handle)
{
	return handle->GetAttachmentCount();
}

int AttachmentManagerInterop::GetIndexByName(IAttachmentManager *handle, mono::string szName)
{
	return handle->GetIndexByName(NtText(szName));
}

int AttachmentManagerInterop::GetIndexByNameCrc(IAttachmentManager *handle, uint nameCrc)
{
	return handle->GetIndexByNameCRC(nameCrc);
}

uint AttachmentManagerInterop::ProjectAllAttachment(IAttachmentManager *handle)
{
	return handle->ProjectAllAttachment();
}

void AttachmentManagerInterop::PhysicalizeAttachment(IAttachmentManager *handle, int idx, IPhysicalEntity *pent, int nLod)
{
	handle->PhysicalizeAttachment(idx, pent, nLod);
}

void AttachmentManagerInterop::DephysicalizeAttachment(IAttachmentManager *handle, int idx, IPhysicalEntity *pent)
{
	handle->DephysicalizeAttachment(idx, pent);
}

int AttachmentManagerInterop::GetProxyCount(IAttachmentManager *handle)
{
	return handle->GetProxyCount();
}

IProxy *AttachmentManagerInterop::CreateProxy(IAttachmentManager *handle, mono::string szName, mono::string szJointName)
{
	return handle->CreateProxy(NtText(szName), NtText(szJointName));
}

IProxy *AttachmentManagerInterop::GetProxyInterfaceByIndex(IAttachmentManager *handle, uint c)
{
	return handle->GetProxyInterfaceByIndex(c);
}

IProxy *AttachmentManagerInterop::GetProxyInterfaceByName(IAttachmentManager *handle, mono::string szName)
{
	return handle->GetProxyInterfaceByName(NtText(szName));
}

IProxy *AttachmentManagerInterop::GetProxyInterfaceByCrc(IAttachmentManager *handle, uint nameCrc)
{
	return handle->GetProxyInterfaceByCRC(nameCrc);
}

int AttachmentManagerInterop::GetProxyIndexByName(IAttachmentManager *handle, mono::string szName)
{
	return handle->GetProxyIndexByName(NtText(szName));
}

int AttachmentManagerInterop::RemoveProxyByInterface(IAttachmentManager *handle, IProxy *ptr)
{
	return handle->RemoveProxyByInterface(ptr);
}

int AttachmentManagerInterop::RemoveProxyByName(IAttachmentManager *handle, mono::string szName)
{
	return handle->RemoveProxyByName(NtText(szName));
}

int AttachmentManagerInterop::RemoveProxyByNameCrc(IAttachmentManager *handle, uint nameCrc)
{
	return handle->RemoveProxyByNameCRC(nameCrc);
}

void AttachmentManagerInterop::DrawProxies(IAttachmentManager *handle, uint enable)
{
	handle->DrawProxies(enable);
}
