#pragma once

#include "IMonoInterface.h"
#include <CryAnimation/ICryAnimation.h>
#include <CryAnimation/IAttachment.h>

enum AttachmentTypes;

struct AttachmentSocketInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AttachmentSocket"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Attachments"; }

	virtual void InitializeInterops() override;

	static void                 AddRef(IAttachment *handle);
	static void                 Release(IAttachment *handle);
	static mono::string         GetName(IAttachment *handle);
	static uint32               GetNameCRC(IAttachment *handle);
	static uint32               ReName(IAttachment *handle, mono::string szSocketName, uint crc);
	static uint32               GetType(IAttachment *handle);
	static uint32               SetJointName(IAttachment *handle, mono::string szJointName);
	static uint                 GetFlags(IAttachment *handle);
	static void                 SetFlags(IAttachment *handle, uint flags);
	static QuatT                GetAttAbsoluteDefault(IAttachment *handle);
	static void                 SetAttAbsoluteDefault(IAttachment *handle, const QuatT &rot);
	static void                 SetAttRelativeDefault(IAttachment *handle, const QuatT &mat);
	static QuatT                GetAttRelativeDefault(IAttachment *handle);
	static QuatT                GetAttModelRelative(IAttachment *handle);
	static QuatTS               GetAttWorldAbsolute(IAttachment *handle);
	static void                 UpdateAttModelRelative(IAttachment *handle);
	static void                 HideAttachment(IAttachment *handle, uint x);
	static uint                 IsAttachmentHidden(IAttachment *handle);
	static void                 HideInRecursion(IAttachment *handle, uint x);
	static uint                 IsAttachmentHiddenInRecursion(IAttachment *handle);
	static void                 HideInShadow(IAttachment *handle, uint x);
	static uint                 IsAttachmentHiddenInShadow(IAttachment *handle);
	static void                 AlignJointAttachment(IAttachment *handle);
	static uint                 GetJointID(IAttachment *handle);
	static void                 AddBinding(IAttachment *handle, IAttachmentObject *pModel, ISkin *pISkin);
	static IAttachmentObject   *GetIAttachmentObject(IAttachment *handle, IAttachmentObject::EType *type);
	static IAttachmentSkin     *GetIAttachmentSkin(IAttachment *handle);
	static void                 ClearBinding(IAttachment *handle, uint nLoadingFlags = 0);
	static void                 SwapBinding(IAttachment *handle, IAttachment *pNewAttachment);
	static SimulationParams    *GetSimulationParams(IAttachment *handle);
	static RowSimulationParams *GetRowParams(IAttachment *handle);
	static void                 Serialize(IAttachment *handle, ISerialize *ser);
};