#pragma once

#include "IMonoInterface.h"
#include <ILevelSystem.h>

struct MonoMinimapInfo
{
	mono::string sMinimapName;
	int iWidth;
	int iHeight;
	float fStartX;
	float fStartY;
	float fEndX;
	float fEndY;
	float fDimX;
	float fDimY;

	MonoMinimapInfo(const ILevelInfo::SMinimapInfo &info)
	{
		this->sMinimapName = ToMonoString(info.sMinimapName.c_str());
		this->iWidth = info.iWidth;
		this->iHeight = info.iHeight;
		this->fStartX = info.fStartX;
		this->fStartY = info.fStartY;
		this->fEndX = info.fEndX;
		this->fEndY = info.fEndY;
		this->fDimX = info.fDimX;
		this->fDimY = info.fDimY;
	}
};

struct MonoGameTypeInfo
{
	mono::string name;
	mono::string xmlFile;
	int cgfCount;

	MonoGameTypeInfo(const ILevelInfo::TGameTypeInfo &info)
	{
		this->name = ToMonoString(info.name.c_str());
		this->xmlFile = ToMonoString(info.xmlFile.c_str());
		this->cgfCount = info.cgfCount;
	}
};

struct LevelInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Level"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.CryAction"; }

	virtual void InitializeInterops() override;

	static mono::string GetName(ILevelInfo *handle);
	static bool         IsOfTypeInternal(ILevelInfo *handle, mono::string sType);
	static mono::string GetPath(ILevelInfo *handle);
	static mono::string GetPaks(ILevelInfo *handle);
	static mono::string GetDisplayName(ILevelInfo *handle);
	static mono::string GetPreviewImagePath(ILevelInfo *handle);
	static mono::string GetBackgroundImagePath(ILevelInfo *handle);
	static mono::string GetMinimapImagePath(ILevelInfo *handle);
	static int          GetHeightmapSize(ILevelInfo *handle);
	static bool         MetadataLoaded(ILevelInfo *handle);
	static bool         GetIsModLevel(ILevelInfo *handle);
	static uint         GetScanTag(ILevelInfo *handle);
	static uint         GetLevelTag(ILevelInfo *handle);
	static int          GetGameTypeCount(ILevelInfo *handle);
	static bool         GetGameType(ILevelInfo *handle, int gameType, MonoGameTypeInfo &info);
	static bool         SupportsGameType(ILevelInfo *handle, mono::string gameTypeName);
	static void         GetDefaultGameType(ILevelInfo *handle, MonoGameTypeInfo &info);
	static mono::string GetGameRules(ILevelInfo *handle, int index);
	static int          GetGameRulesCount(ILevelInfo *handle);
	static bool         HasGameRules(ILevelInfo *handle);
	static mono::string GetDefaultGameRules(ILevelInfo *handle);
	static MonoMinimapInfo GetMinimapInfo(ILevelInfo *handle);
};