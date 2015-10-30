#include "stdafx.h"

#include "DefaultSkeleton.h"
#include <ICryAnimation.h>

void DefaultSkeletonInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetJointCount);
	REGISTER_METHOD(GetJointParentIDByID);
	REGISTER_METHOD(GetControllerIDByID);
	REGISTER_METHOD(GetJointIDByCRC32);
	REGISTER_METHOD(GetJointCRC32ByID);
	REGISTER_METHOD(GetJointNameByID);
	REGISTER_METHOD(GetJointIDByName);
	REGISTER_METHOD(GetDefaultAbsJointByID);
	REGISTER_METHOD(GetDefaultRelJointByID);
	REGISTER_METHOD(GetModelFilePath);
}

uint DefaultSkeletonInterop::GetJointCount(IDefaultSkeleton *handle)
{
	return handle->GetJointCount();
}

int DefaultSkeletonInterop::GetJointParentIDByID(IDefaultSkeleton *handle, int id)
{
	return handle->GetJointParentIDByID(id);
}

int DefaultSkeletonInterop::GetControllerIDByID(IDefaultSkeleton *handle, int id)
{
	return handle->GetControllerIDByID(id);
}

int DefaultSkeletonInterop::GetJointIDByCRC32(IDefaultSkeleton *handle, uint crc32)
{
	return handle->GetJointIDByCRC32(crc32);
}

uint DefaultSkeletonInterop::GetJointCRC32ByID(IDefaultSkeleton *handle, int id)
{
	return handle->GetJointCRC32ByID(id);
}

mono::string DefaultSkeletonInterop::GetJointNameByID(IDefaultSkeleton *handle, int id)
{
	return ToMonoString(handle->GetJointNameByID(id));
}

int DefaultSkeletonInterop::GetJointIDByName(IDefaultSkeleton *handle, mono::string name)
{
	return handle->GetJointIDByName(NtText(name));
}

void DefaultSkeletonInterop::GetDefaultAbsJointByID(IDefaultSkeleton *handle, uint nJointIdx, QuatT &jointLocation)
{
	jointLocation = handle->GetDefaultAbsJointByID(nJointIdx);
}

void DefaultSkeletonInterop::GetDefaultRelJointByID(IDefaultSkeleton *handle, uint nJointIdx, QuatT &jointLocation)
{
	jointLocation = handle->GetDefaultRelJointByID(nJointIdx);
}

mono::string DefaultSkeletonInterop::GetModelFilePath(IDefaultSkeleton *handle)
{
	return ToMonoString(handle->GetModelFilePath());
}
