#include "stdafx.h"

#include "Levels.h"
#include <ILevelSystem.h>

void LevelsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetCount);
	REGISTER_METHOD(GetItemInt);
	REGISTER_METHOD(GetItem);
}

int LevelsInterop::GetCount()
{
	return MonoEnv->CryAction->GetILevelSystem()->GetLevelCount();
}

ILevelInfo *LevelsInterop::GetItemInt(int index, bool &outOfRange)
{
	auto levelSystem = MonoEnv->CryAction->GetILevelSystem();

	if (index < 0 || index >= levelSystem->GetLevelCount())
	{
		outOfRange = true;
		return nullptr;
	}

	outOfRange = false;
	return levelSystem->GetLevelInfo(index);
}

ILevelInfo *LevelsInterop::GetItem(mono::string name)
{
	return MonoEnv->CryAction->GetILevelSystem()->GetLevelInfo(NtText(name));
}