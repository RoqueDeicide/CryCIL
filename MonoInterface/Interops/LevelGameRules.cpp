#include "stdafx.h"

#include "LevelGameRules.h"
#include <ILevelSystem.h>

void LevelGameRulesInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetDefault);
	REGISTER_METHOD(GetItem);
	REGISTER_METHOD(GetCount);
}

mono::string LevelGameRulesInterop::GetDefault(ILevelInfo **info)
{
	return ToMonoString((*info)->GetDefaultGameRules());
}

mono::string LevelGameRulesInterop::GetItem(ILevelInfo **info, int index)
{
	return ToMonoString((*info)->GetGameRules().at(index));
}

int LevelGameRulesInterop::GetCount(ILevelInfo **info)
{
	return (*info)->GetGameRules().size();
}
