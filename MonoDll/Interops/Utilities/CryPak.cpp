#include "StdAfx.h"
#include "CryPak.h"

CryPakInterop::CryPakInterop()
{
	REGISTER_METHOD(GetGameFolder);
	REGISTER_METHOD(SetGameFolder);

	REGISTER_METHOD(SetAlias);
	REGISTER_METHOD(GetAlias);

	REGISTER_METHOD(AdjustFileName);
}

mono::string CryPakInterop::GetGameFolder()
{
	return ToMonoString(gEnv->pCryPak->GetGameFolder());
}

void CryPakInterop::SetGameFolder(mono::string folder)
{
	gEnv->pCryPak->SetGameFolder(ToCryString(folder));
}

void CryPakInterop::SetAlias(mono::string name, mono::string alias, bool bAdd)
{
	gEnv->pCryPak->SetAlias(ToCryString(name), ToCryString(alias), bAdd);
}

mono::string CryPakInterop::GetAlias(mono::string name, bool returnSame)
{
	return ToMonoString(gEnv->pCryPak->GetAlias(ToCryString(name), returnSame));
}

mono::string CryPakInterop::AdjustFileName(mono::string src, unsigned int flags)
{
	char path[ICryPak::g_nMaxPath];
	path[sizeof(path)-1] = 0;

	return ToMonoString(gEnv->pCryPak->AdjustFileName(ToCryString(src), path, flags));
}