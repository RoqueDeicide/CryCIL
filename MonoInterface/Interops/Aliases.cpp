#include "stdafx.h"

#include "Aliases.h"

void AliasesInterop::InitializeInterops()
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

	if (!alias)
	{
		ArgumentNullException("Name of the alias cannot be null.").Throw();
	}

	return ToMonoString(gEnv->pCryPak->GetAlias(NtText(alias), returnName));
}

void AliasesInterop::Set(mono::string alias, mono::string value)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return;
	}

	if (!alias)
	{
		ArgumentNullException("Name of the alias cannot be null.").Throw();
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
