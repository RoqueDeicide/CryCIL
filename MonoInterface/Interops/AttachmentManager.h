#pragma once

#include "IMonoInterface.h"

struct IAttachment;
struct IAttachmentManager;
struct IProxy;

struct AttachmentManagerInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AttachmentManager"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Attachments"; }

	virtual void OnRunTimeInitialized() override;

	static ICharacterInstance *GetSkelInstance(IAttachmentManager *handle);
	static uint         LoadAttachmentList(IAttachmentManager *handle, mono::string pathname);
	static IAttachment *CreateAttachment(IAttachmentManager *handle, mono::string szName, uint type, mono::string szJointName, bool bCallProject);
	static int          RemoveAttachmentByInterface(IAttachmentManager *handle, IAttachment *ptr);
	static int          RemoveAttachmentByName(IAttachmentManager *handle, mono::string szName);
	static int          RemoveAttachmentByNameCrc(IAttachmentManager *handle, uint nameCrc);
	static IAttachment *GetInterfaceByName(IAttachmentManager *handle, mono::string szName);
	static IAttachment *GetInterfaceByIndex(IAttachmentManager *handle, uint c);
	static IAttachment *GetInterfaceByNameCrc(IAttachmentManager *handle, uint nameCrc);
	static int          GetAttachmentCount(IAttachmentManager *handle);
	static int          GetIndexByName(IAttachmentManager *handle, mono::string szName);
	static int          GetIndexByNameCrc(IAttachmentManager *handle, uint nameCrc);
	static uint         ProjectAllAttachment(IAttachmentManager *handle);
	static void         PhysicalizeAttachment(IAttachmentManager *handle, int idx, IPhysicalEntity *pent, int nLod);
	static void         DephysicalizeAttachment(IAttachmentManager *handle, int idx, IPhysicalEntity *pent);
	static int          GetProxyCount(IAttachmentManager *handle);
	static IProxy      *CreateProxy(IAttachmentManager *handle, mono::string szName, mono::string szJointName);
	static IProxy      *GetProxyInterfaceByIndex(IAttachmentManager *handle, uint c);
	static IProxy      *GetProxyInterfaceByName(IAttachmentManager *handle, mono::string szName);
	static IProxy      *GetProxyInterfaceByCrc(IAttachmentManager *handle, uint nameCrc);
	static int          GetProxyIndexByName(IAttachmentManager *handle, mono::string szName);
	static int          RemoveProxyByInterface(IAttachmentManager *handle, IProxy *ptr);
	static int          RemoveProxyByName(IAttachmentManager *handle, mono::string szName);
	static int          RemoveProxyByNameCrc(IAttachmentManager *handle, uint nameCrc);
	static void         DrawProxies(IAttachmentManager *handle, uint enable);
};