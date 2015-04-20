#include "StdAfx.h"
#include "3DEngine.h"

Engine3DInterop::Engine3DInterop()
{
	REGISTER_METHOD(SetTimeOfDay);
	REGISTER_METHOD(GetTimeOfDay);

	REGISTER_METHOD(GetTimeOfDayAdvancedInfo);
	REGISTER_METHOD(SetTimeOfDayAdvancedInfo);

	REGISTER_METHOD(SetTimeOfDayVariableValue);
	REGISTER_METHOD(SetTimeOfDayVariableValueColor);

	REGISTER_METHOD(ActivatePortal);

	REGISTER_METHOD(GetMaxViewDistance);
}

void Engine3DInterop::SetTimeOfDay(float hour, bool forceUpdate)
{
	gEnv->p3DEngine->GetTimeOfDay()->SetTime(hour, forceUpdate);
}

float Engine3DInterop::GetTimeOfDay()
{
	return gEnv->p3DEngine->GetTimeOfDay()->GetTime();
}

ITimeOfDay::SAdvancedInfo Engine3DInterop::GetTimeOfDayAdvancedInfo()
{
	ITimeOfDay::SAdvancedInfo info;
	gEnv->p3DEngine->GetTimeOfDay()->GetAdvancedInfo(info);

	return info;
}

void Engine3DInterop::SetTimeOfDayAdvancedInfo(ITimeOfDay::SAdvancedInfo advancedInfo)
{
	gEnv->p3DEngine->GetTimeOfDay()->SetAdvancedInfo(advancedInfo);
}

void Engine3DInterop::SetTimeOfDayVariableValue(ITimeOfDay::ETimeOfDayParamID id, float value)
{
	float valueArray[3];
	valueArray[0] = value;
	gEnv->p3DEngine->GetTimeOfDay()->SetVariableValue(id, valueArray);
}

void Engine3DInterop::SetTimeOfDayVariableValueColor(ITimeOfDay::ETimeOfDayParamID id, Vec3 value)
{
	float valueArray[3];
	valueArray[0] = value.x;
	valueArray[1] = value.x;
	valueArray[2] = value.x;

	gEnv->p3DEngine->GetTimeOfDay()->SetVariableValue(id, valueArray);
}

void Engine3DInterop::ActivatePortal(Vec3 pos, bool activate, mono::string entityName)
{
	gEnv->p3DEngine->ActivatePortal(pos, activate, ToCryString(entityName));
}

float Engine3DInterop::GetMaxViewDistance()
{
	return gEnv->p3DEngine->GetMaxViewDistance();
}