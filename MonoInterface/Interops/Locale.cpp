#include "stdafx.h"

#include "Locale.h"

void LocaleInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Translate);
	REGISTER_METHOD(TranslateLabel);
	REGISTER_METHOD(LoadXml);
	REGISTER_METHOD(ReloadData);
	REGISTER_METHOD(ReleaseData);
	REGISTER_METHOD(GetLanguageNameInternal);
	REGISTER_METHOD(GetCurrentLanguage);
	REGISTER_METHOD(SetCurrentLanguage);
}

mono::string LocaleInterop::Translate(mono::string text, bool forceEnglish)
{
	if (!text)
	{
		ArgumentNullException("Text for translation cannot be null.").Throw();
	}

	auto loc = GetISystem()->GetLocalizationManager();
	
	wstring localized;
	if (loc->LocalizeString(NtText(text), localized, forceEnglish))
	{
		return ToMonoString(localized.c_str());
	}
	return nullptr;
}

mono::string LocaleInterop::TranslateLabel(mono::string labelName, bool forceEnglish)
{
	if (!labelName)
	{
		ArgumentNullException("Localization label cannot be null.").Throw();
	}

	auto loc = GetISystem()->GetLocalizationManager();

	wstring localized;
	if (loc->LocalizeLabel(NtText(labelName), localized, forceEnglish))
	{
		return ToMonoString(localized.c_str());
	}
	return nullptr;
}

bool LocaleInterop::LoadXml(mono::string fileName, bool reload)
{
	if (!fileName)
	{
		ArgumentNullException("Name of the file to load cannot be null.").Throw();
	}

	auto loc = GetISystem()->GetLocalizationManager();

	return loc->LoadExcelXmlSpreadsheet(NtText(fileName), reload);
}

void LocaleInterop::ReloadData()
{
	GetISystem()->GetLocalizationManager()->ReloadData();
}

void LocaleInterop::ReleaseData()
{
	GetISystem()->GetLocalizationManager()->FreeData();
}

mono::string LocaleInterop::GetLanguageNameInternal(ILocalizationManager::EPlatformIndependentLanguageID language)
{
	return ToMonoString(GetISystem()->GetLocalizationManager()->LangNameFromPILID(language));
}

mono::string LocaleInterop::GetCurrentLanguage()
{
	return ToMonoString(GetISystem()->GetLocalizationManager()->GetLanguage());
}

void LocaleInterop::SetCurrentLanguage(const char *name)
{
	GetISystem()->GetLocalizationManager()->SetLanguage(name);
}
