#include "stdafx.h"

#include "Level.h"

void LevelInterop::InitializeInterops()
{
	REGISTER_METHOD(GetName);
	REGISTER_METHOD(IsOfTypeInternal);
	REGISTER_METHOD(GetPath);
	REGISTER_METHOD(GetPaks);
	REGISTER_METHOD(GetDisplayName);
	REGISTER_METHOD(GetPreviewImagePath);
	REGISTER_METHOD(GetBackgroundImagePath);
	REGISTER_METHOD(GetMinimapImagePath);
	REGISTER_METHOD(GetHeightmapSize);
	REGISTER_METHOD(MetadataLoaded);
	REGISTER_METHOD(GetIsModLevel);
	REGISTER_METHOD(GetScanTag);
	REGISTER_METHOD(GetLevelTag);
	REGISTER_METHOD(GetGameTypeCount);
	REGISTER_METHOD(GetGameType);
	REGISTER_METHOD(SupportsGameType);
	REGISTER_METHOD(GetDefaultGameType);
	REGISTER_METHOD(GetGameRules);
	REGISTER_METHOD(GetGameRulesCount);
	REGISTER_METHOD(HasGameRules);
	REGISTER_METHOD(GetDefaultGameRules);
	REGISTER_METHOD(GetMinimapInfo);
}

mono::string LevelInterop::GetName(ILevelInfo *handle)
{
	return ToMonoString(handle->GetName());
}

bool LevelInterop::IsOfTypeInternal(ILevelInfo *handle, mono::string sType)
{
	return handle->IsOfType(NtText(sType));
}

mono::string LevelInterop::GetPath(ILevelInfo *handle)
{
	return ToMonoString(handle->GetPath());
}

mono::string LevelInterop::GetPaks(ILevelInfo *handle)
{
	return ToMonoString(handle->GetPaks());
}

mono::string LevelInterop::GetDisplayName(ILevelInfo *handle)
{
	return ToMonoString(handle->GetDisplayName());
}

mono::string LevelInterop::GetPreviewImagePath(ILevelInfo *handle)
{
	return ToMonoString(handle->GetPreviewImagePath());
}

mono::string LevelInterop::GetBackgroundImagePath(ILevelInfo *handle)
{
	return ToMonoString(handle->GetBackgroundImagePath());
}

mono::string LevelInterop::GetMinimapImagePath(ILevelInfo *handle)
{
	return ToMonoString(handle->GetMinimapImagePath());
}

int LevelInterop::GetHeightmapSize(ILevelInfo *handle)
{
	return handle->GetHeightmapSize();
}

bool LevelInterop::MetadataLoaded(ILevelInfo *handle)
{
	return handle->MetadataLoaded();
}

bool LevelInterop::GetIsModLevel(ILevelInfo *handle)
{
	return handle->GetIsModLevel();
}

uint LevelInterop::GetScanTag(ILevelInfo *handle)
{
	return handle->GetScanTag();
}

uint LevelInterop::GetLevelTag(ILevelInfo *handle)
{
	return handle->GetLevelTag();
}

int LevelInterop::GetGameTypeCount(ILevelInfo *handle)
{
	return handle->GetGameTypeCount();
}

bool LevelInterop::GetGameType(ILevelInfo *handle, int gameType, MonoGameTypeInfo &info)
{
	if (gameType < 0 || gameType >= handle->GetGameTypeCount())
	{
		return false;
	}

	info = *handle->GetGameType(gameType);

	return true;
}

bool LevelInterop::SupportsGameType(ILevelInfo *handle, mono::string gameTypeName)
{
	return handle->SupportsGameType(NtText(gameTypeName));
}

void LevelInterop::GetDefaultGameType(ILevelInfo *handle, MonoGameTypeInfo &info)
{
	info = *handle->GetDefaultGameType();
}

mono::string LevelInterop::GetGameRules(ILevelInfo *handle, int index)
{
	return ToMonoString(handle->GetGameRules()[index].c_str());
}

int LevelInterop::GetGameRulesCount(ILevelInfo *handle)
{
	return handle->GetGameRules().size();
}

bool LevelInterop::HasGameRules(ILevelInfo *handle)
{
	return handle->HasGameRules();
}

mono::string LevelInterop::GetDefaultGameRules(ILevelInfo *handle)
{
	return ToMonoString(handle->GetDefaultGameRules());
}

MonoMinimapInfo LevelInterop::GetMinimapInfo(ILevelInfo *handle)
{
	return handle->GetMinimapInfo();
}
