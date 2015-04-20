#include "stdafx.h"

#include "LevelMissions.h"
#include <ILevelSystem.h>

void LevelMissionsInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Supports);
	REGISTER_METHOD(GetDefault);
	REGISTER_METHOD(GetItem);
	REGISTER_METHOD(GetCount);
}

bool LevelMissionsInterop::Supports(ILevelInfo **info, mono::string name)
{
	if (!*info)
	{
		NullReferenceException("The level missions collection is not valid.").Throw();
	}
	if (!name)
	{
		return false;
	}

	return (*info)->SupportsGameType(NtText(name));
}

LevelMission LevelMissionsInterop::GetDefault(ILevelInfo **info)
{
	auto m = (*info)->GetDefaultGameType();
	LevelMission mission;
	mission.name = (m->name.empty()) ? ToMonoString(m->name.c_str()) : nullptr;
	mission.xmlPath = (m->xmlFile.empty()) ? ToMonoString(m->xmlFile.c_str()) : nullptr;
	mission.cgfCount = m->cgfCount;
	return mission;
}

LevelMission LevelMissionsInterop::GetItem(ILevelInfo **info, int index)
{
	auto m = (*info)->GetGameType(index);
	LevelMission mission;
	mission.name = (m->name.empty()) ? ToMonoString(m->name.c_str()) : nullptr;
	mission.xmlPath = (m->xmlFile.empty()) ? ToMonoString(m->xmlFile.c_str()) : nullptr;
	mission.cgfCount = m->cgfCount;
	return mission;
}

int LevelMissionsInterop::GetCount(ILevelInfo **info)
{
	return (*info)->GetGameTypeCount();
}
