#include "stdafx.h"

#include "Level.h"

#include <ILocalizationManager.h>

void LevelInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_Name);
	REGISTER_METHOD(get_DisplayName);
	REGISTER_METHOD(get_Path);
	REGISTER_METHOD(get_Paks);
	REGISTER_METHOD(get_IsFromMod);
	REGISTER_METHOD(get_PreviewPath);
	REGISTER_METHOD(get_BackgroundPath);
	REGISTER_METHOD(get_MinimapPath);
	REGISTER_METHOD(get_Minimap);

	REGISTER_METHOD(IsOfType);
}

mono::string LevelInterop::get_Name(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return ToMonoString(info->GetName());
}

mono::string LevelInterop::get_DisplayName(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}
	string displayName;
	gEnv->pSystem->GetLocalizationManager()->LocalizeLabel(NtText(info->GetDisplayName()), displayName);
	return ToMonoString(displayName.c_str());
}

mono::string LevelInterop::get_Path(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return ToMonoString(info->GetPath());
}

mono::string LevelInterop::get_Paks(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return ToMonoString(info->GetPaks());
}

bool LevelInterop::get_IsFromMod(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return info->GetIsModLevel();
}

mono::string LevelInterop::get_PreviewPath(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return ToMonoString(info->GetPreviewImagePath());
}

mono::string LevelInterop::get_BackgroundPath(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return ToMonoString(info->GetBackgroundImagePath());
}

mono::string LevelInterop::get_MinimapPath(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return ToMonoString(info->GetMinimapImagePath());
}

ILevelInfo::SMinimapInfo LevelInterop::get_Minimap(mono::object levelObj)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}

	return info->GetMinimapInfo();
}

bool LevelInterop::IsOfType(mono::object levelObj, mono::string type)
{
	ILevelInfo *info = *GET_BOXED_OBJECT_DATA(ILevelInfo *, levelObj);
	if (!info)
	{
		NullReferenceException("This level object is not valid.").Throw();
	}
	if (!type)
	{
		ArgumentNullException("Name of the level type cannot be null.").Throw();
	}

	return info->IsOfType(NtText(type));
}
