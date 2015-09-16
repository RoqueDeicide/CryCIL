#include "stdafx.h"

#include "Levels.h"
#include <ILevelSystem.h>

void LevelsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_Count);

	this->RegisterInteropMethod("get_Item(int)",    get_ItemInt);
	this->RegisterInteropMethod("get_Item(string)", get_Item);

	system    = MonoEnv->CryAction->GetILevelSystem();
	levelCtor = MonoEnv->Cryambly->GetClass(this->GetInteropNameSpace(), this->GetInteropClassName())->GetConstructor(1);
}

int LevelsInterop::get_Count(mono::object)
{
	return system->GetLevelCount();
}

mono::object LevelsInterop::get_ItemInt(mono::object, int index)
{
	void *param = system->GetLevelInfo(index);
	return levelCtor->Create(&param);
}

mono::object LevelsInterop::get_Item(mono::object, mono::string name)
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