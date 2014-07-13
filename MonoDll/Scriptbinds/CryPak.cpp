#include "StdAfx.h"
#include "CryPak.h"

CScriptbind_CryPak::CScriptbind_CryPak()
{
	REGISTER_METHOD(GetGameFolder);
	REGISTER_METHOD(SetGameFolder);

	REGISTER_METHOD(SetAlias);
	REGISTER_METHOD(GetAlias);

	REGISTER_METHOD(AdjustFileName);
}

mono::string CScriptbind_CryPak::GetGameFolder()
{
	return ToMonoString(gEnv->pCryPak->GetGameFolder());
}

void CScriptbind_CryPak::SetGameFolder(mono::string folder)
{
	gEnv->pCryPak->SetGameFolder(ToCryString(folder));
}

void CScriptbind_CryPak::SetAlias(mono::string name, mono::string alias, bool bAdd)
{
	gEnv->pCryPak->SetAlias(ToCryString(name), ToCryString(alias), bAdd);
}

mono::string CScriptbind_CryPak::GetAlias(mono::string name, bool returnSame)
{
	return ToMonoString(gEnv->pCryPak->GetAlias(ToCryString(name), returnSame));
}

mono::string CScriptbind_CryPak::AdjustFileName(mono::string src, unsigned int flags)
{
	char path[ICryPak::g_nMaxPath];
	path[sizeof(path)-1] = 0;

	return ToMonoString(gEnv->pCryPak->AdjustFileName(ToCryString(src), path, flags));
}