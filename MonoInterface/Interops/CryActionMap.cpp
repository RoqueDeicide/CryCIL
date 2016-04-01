#include "stdafx.h"

#include "CryActionMap.h"
#include <IActionMapManager.h>

void CryActionMapInterop::InitializeInterops()
{
	REGISTER_METHOD(GetAction);
	REGISTER_METHOD(CreateActionInternal);
	REGISTER_METHOD(RemoveActionInternal);
	REGISTER_METHOD(IsEnabled);
	REGISTER_METHOD(Enable);
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(GetNameHash);
}

IActionMapAction *CryActionMapInterop::GetAction(IActionMap *handle, mono::string name)
{
	return handle->GetAction(ActionId(NtText(name)));
}

IActionMapAction *CryActionMapInterop::CreateActionInternal(IActionMap *handle, mono::string name)
{
	ActionId id = ActionId(NtText(name));
	if (handle->CreateAction(id))
	{
		return handle->GetAction(id);
	}
	return nullptr;
}

bool CryActionMapInterop::RemoveActionInternal(IActionMap *handle, mono::string name)
{
	ActionId id = ActionId(NtText(name));
	return handle->RemoveAction(id);
}

bool CryActionMapInterop::IsEnabled(IActionMap *handle)
{
	return handle->Enabled();
}

void CryActionMapInterop::Enable(IActionMap *handle, bool enable)
{
	handle->Enable(enable);
}

mono::string CryActionMapInterop::GetName(IActionMap *handle)
{
	return ToMonoString(handle->GetName());
}

unsigned int CryActionMapInterop::GetNameHash(IActionMap *handle)
{
	return CCrc32::ComputeLowercase(handle->GetName());
}