#pragma once

#include "IMonoInterface.h"
#include "ILocalizationManager.h"

struct LocaleInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Translator"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Localization"; }

	virtual void InitializeInterops() override;

	static mono::string Translate(mono::string text, bool forceEnglish);
	static mono::string TranslateLabel(mono::string labelName, bool forceEnglish);
	static bool         LoadXml(mono::string fileName, bool reload);
	static void         ReloadData();
	static void         ReleaseData();
	static mono::string GetLanguageNameInternal(ILocalizationManager::EPlatformIndependentLanguageID lang);
	static mono::string GetCurrentLanguage();
	static void         SetCurrentLanguage(const char *name);
};