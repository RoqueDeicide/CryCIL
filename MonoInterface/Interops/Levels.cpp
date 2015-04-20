#include "stdafx.h"

#include "Levels.h"
#include <ILevelSystem.h>

void LevelsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_Count);

	this->RegisterInteropMethod("get_Item(int)",    get_ItemInt);
	this->RegisterInteropMethod("get_Item(string)", get_Item);

	system    = MonoEnv->CryAction->GetILevelSystem();
	levelCtor = MonoEnv->Cryambly->GetClass(this->GetNameSpace(), this->GetName())->GetConstructor(1);
}

int LevelsInterop::get_Count(mono::object obj)
{
	return system->GetLevelCount();
}

mono::object LevelsInterop::get_ItemInt(mono::object obj, int index)
{
	void *param = system->GetLevelInfo(index);
	return levelCtor->Create(&param);
}

mono::object LevelsInterop::get_Item(mono::object obj, mono::string name)
{
	if (!name)
	{
		return nullptr;
	}

	void *param = system->GetLevelInfo(NtText(name));
	return levelCtor->Create(&param);
}

ILevelSystem     *LevelsInterop::system;
IMonoConstructor *LevelsInterop::levelCtor;