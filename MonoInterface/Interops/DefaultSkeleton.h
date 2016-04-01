#pragma once

#include "IMonoInterface.h"

struct IDefaultSkeleton;

struct DefaultSkeletonInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "DefaultSkeleton"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void InitializeInterops() override;

	static uint GetJointCount(IDefaultSkeleton *handle);
	static int GetJointParentIDByID(IDefaultSkeleton *handle, int id);
	static int GetControllerIDByID(IDefaultSkeleton *handle, int id);
	static int GetJointIDByCRC32(IDefaultSkeleton *handle, uint crc32);
	static uint GetJointCRC32ByID(IDefaultSkeleton *handle, int id);
	static mono::string GetJointNameByID(IDefaultSkeleton *handle, int id);
	static int GetJointIDByName(IDefaultSkeleton *handle, mono::string name);
	static void GetDefaultAbsJointByID(IDefaultSkeleton *handle, uint nJointIdx, QuatT &jointLocation);
	static void GetDefaultRelJointByID(IDefaultSkeleton *handle, uint nJointIdx, QuatT &jointLocation);
	static mono::string GetModelFilePath(IDefaultSkeleton *handle);
};