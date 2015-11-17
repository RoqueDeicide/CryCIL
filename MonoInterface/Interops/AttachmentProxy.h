#pragma once

#include "IMonoInterface.h"

struct IProxy;

struct AttachmentProxyInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AttachmentProxy"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Attachments"; }

	virtual void OnRunTimeInitialized() override;

	static mono::string GetName(IProxy *handle);
	static uint  GetNameCrc(IProxy *handle);
	static uint  ReName(IProxy *handle, mono::string szSocketName, uint crc);
	static uint  SetJointName(IProxy *handle, mono::string szJointName);
	static uint  GetJointId(IProxy *handle);
	static QuatT GetProxyAbsoluteDefault(IProxy *handle);
	static QuatT GetProxyRelativeDefault(IProxy *handle);
	static QuatT GetProxyModelRelative(IProxy *handle);
	static void  SetProxyAbsoluteDefault(IProxy *handle, const QuatT &rot);
	static uint  ProjectProxyInternal(IProxy *handle);
	static void  AlignProxyWithJoint(IProxy *handle);
	static Vec4  GetProxyParams(IProxy *handle);
	static void  SetProxyParams(IProxy *handle, const Vec4 &parameters);
	static int8  GetProxyPurpose(IProxy *handle);
	static void  SetProxyPurpose(IProxy *handle, int8 p);
	static void  SetHideProxy(IProxy *handle, uint8 nHideProxy);
};