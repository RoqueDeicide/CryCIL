#include "stdafx.h"

#include "Aliases.h"

void AliasesInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Get);
	REGISTER_METHOD(Set);
}

mono::string AliasesInterop::Get(mono::string alias, bool returnName)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return nullptr;
	}

	return ToMonoString(gEnv->pCryPak->GetAlias(NtText(alias), returnName));
}

void AliasesInterop::Set(mono::string alias, mono::string value)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return;
	}

	CryLogAlways("WARNING: CryCIL is setting an alias, issues with memory management are possible.");
	
	if (!value)
	{
		gEnv->pCryPak->SetAlias(NtText(alias), nullptr, false);
	}
	else
	{
		gEnv->pCryPak->SetAlias(NtText(alias), NtText(value), true);
	}
}
