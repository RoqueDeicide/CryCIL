#include "stdafx.h"

#include "CryPak.h"

void CryPakInterop::OnRunTimeInitialized()
{
	this->RegisterInteropMethod("Open(string,CryCil.Engine.Files.PathResolutionRules)", OpenPack);
	this->RegisterInteropMethod("Open(string,string,CryCil.Engine.Files.PathResolutionRules)", OpenPackRooted);
	
	this->RegisterInteropMethod("Open(string,string[]&,CryCil.Engine.Files.PathResolutionRules)", OpenPacks);
	this->RegisterInteropMethod("Open(string,string,string[]&,CryCil.Engine.Files.PathResolutionRules)", OpenPacksRooted);
	
	this->RegisterInteropMethod("Close", ClosePack);
	
	REGISTER_METHOD(Exist);

	REGISTER_METHOD(SetPacksAccessible);
	REGISTER_METHOD(SetPackAccessible);
}

mono::string CryPakInterop::OpenPack(mono::string name, ICryPak::EPathResolutionRules rules)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return nullptr;
	}

	if (!name)
	{
		ArgumentNullException("Cannot open a .pak file using a null name.").Throw();
	}

	CryFixedStringT<ICryPak::g_nMaxPath> finalPath;

	if (gEnv->pCryPak->OpenPack(NtText(name), rules, nullptr, &finalPath))
	{
		return ToMonoString(finalPath.c_str());
	}

	return nullptr;
}

mono::string CryPakInterop::OpenPackRooted(mono::string root, mono::string name, ICryPak::EPathResolutionRules rules)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return nullptr;
	}

	if (!root)
	{
		ArgumentNullException("Cannot open a .pak file using a null root.").Throw();
	}
	if (!name)
	{
		ArgumentNullException("Cannot open a .pak file using a null name.").Throw();
	}

	CryFixedStringT<ICryPak::g_nMaxPath> finalPath;

	if (gEnv->pCryPak->OpenPack(NtText(root), NtText(name), rules, nullptr, &finalPath))
	{
		return ToMonoString(finalPath.c_str());
	}

	return nullptr;
}

bool CryPakInterop::OpenPacks(mono::string wildcard, mono::Array *paths, ICryPak::EPathResolutionRules rules)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return nullptr;
	}

	if (!wildcard)
	{
		ArgumentNullException("Cannot open a .pak file using a null wildcard.").Throw();
	}

	std::vector< CryFixedStringT<ICryPak::g_nMaxPath> > finalPaths;

	if (gEnv->pCryPak->OpenPacks(NtText(wildcard), rules, &finalPaths))
	{
		IMonoArray<mono::string> pathsArray =
			MonoEnv->Objects->Arrays->Create(finalPaths.size(), MonoEnv->CoreLibrary->String);

		MonoGCHandle handle = MonoEnv->GC->Pin(pathsArray);

		int arrayPos = 0;
		for (auto i = finalPaths.begin(); i != finalPaths.end(); i++)
		{
			pathsArray[arrayPos++] = ToMonoString(*i);
		}

		*paths = pathsArray;
	}

	*paths = nullptr;

	return false;
}

bool CryPakInterop::OpenPacksRooted(mono::string root, mono::string wildcard, mono::Array *paths, ICryPak::EPathResolutionRules rules)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return nullptr;
	}

	if (!root)
	{
		ArgumentNullException("Cannot open a .pak file using a null root.").Throw();
	}
	if (!wildcard)
	{
		ArgumentNullException("Cannot open a .pak file using a null wildcard.").Throw();
	}

	std::vector< CryFixedStringT<ICryPak::g_nMaxPath> > finalPaths;

	if (gEnv->pCryPak->OpenPacks(NtText(root), NtText(wildcard), rules, &finalPaths))
	{
		IMonoArray<mono::string> pathsArray =
			MonoEnv->Objects->Arrays->Create(finalPaths.size(), MonoEnv->CoreLibrary->String);

		MonoGCHandle handle = MonoEnv->GC->Pin(pathsArray);

		int arrayPos = 0;
		for (auto i = finalPaths.begin(); i != finalPaths.end(); i++)
		{
			pathsArray[arrayPos++] = ToMonoString(*i);
		}

		*paths = pathsArray;
	}

	*paths = nullptr;

	return false;
}

bool CryPakInterop::ClosePack(mono::string name, bool closeMany, ICryPak::EPathResolutionRules rules)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return false;
	}

	if (!name)
	{
		ArgumentNullException("Cannot close a .pak file using a null name/wildcard.").Throw();
	}

	bool ok = false;

	if (closeMany)
	{
		ok = gEnv->pCryPak->ClosePacks(NtText(name), rules);
	}
	else
	{
		ok = gEnv->pCryPak->ClosePack(NtText(name), rules);
	}

	return ok;
}

bool CryPakInterop::Exist(mono::string wildcard)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return false;
	}

	if (!wildcard)
	{
		ArgumentNullException("The wildcard pattern cannot be null.").Throw();
	}

	return gEnv->pCryPak->FindPacks(NtText(wildcard));
}

bool CryPakInterop::SetPacksAccessible(bool bAccessible, mono::string pWildcard)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return false;
	}

	if (!pWildcard)
	{
		ArgumentNullException("The wildcard pattern cannot be null.").Throw();
	}

	return gEnv->pCryPak->SetPacksAccessible(bAccessible, NtText(pWildcard));
}

bool CryPakInterop::SetPackAccessible(bool bAccessible, mono::string pName)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return false;
	}

	if (!pName)
	{
		ArgumentNullException("The name of the file cannot be null.").Throw();
	}

	return gEnv->pCryPak->SetPackAccessible(bAccessible, NtText(pName));
}
