#include "StdAfx.h"
#include "3DEngine.h"

CScriptbind_3DEngine::CScriptbind_3DEngine()
{
	REGISTER_METHOD(GetTerrainElevation);
	REGISTER_METHOD(GetTerrainZ);

	REGISTER_METHOD(GetTerrainSize);
	REGISTER_METHOD(GetTerrainSectorSize);
	REGISTER_METHOD(GetTerrainUnitSize);

	REGISTER_METHOD(SetTimeOfDay);
	REGISTER_METHOD(GetTimeOfDay);

	REGISTER_METHOD(GetTimeOfDayAdvancedInfo);
	REGISTER_METHOD(SetTimeOfDayAdvancedInfo);

	REGISTER_METHOD(SetTimeOfDayVariableValue);
	REGISTER_METHOD(SetTimeOfDayVariableValueColor);

	REGISTER_METHOD(ActivatePortal);

	REGISTER_METHOD(GetMaxViewDistance);
}

float CScriptbind_3DEngine::GetTerrainElevation(float x, float y)
{
	return gEnv->p3DEngine->GetTerrainElevation(x, y);
}

float CScriptbind_3DEngine::GetTerrainZ(int x, int y)
{
	return gEnv->p3DEngine->GetTerrainZ(x, y);
}

int CScriptbind_3DEngine::GetTerrainUnitSize()
{
	return gEnv->p3DEngine->GetHeightMapUnitSize();
}

int CScriptbind_3DEngine::GetTerrainSize()
{
	return gEnv->p3DEngine->GetTerrainSize();
}

int CScriptbind_3DEngine::GetTerrainSectorSize()
{
	return gEnv->p3DEngine->GetTerrainSectorSize();
}

void CScriptbind_3DEngine::SetTimeOfDay(float hour, bool forceUpdate)
{
	gEnv->p3DEngine->GetTimeOfDay()->SetTime(hour, forceUpdate);
}

float CScriptbind_3DEngine::GetTimeOfDay()
{
	return gEnv->p3DEngine->GetTimeOfDay()->GetTime();
}

ITimeOfDay::SAdvancedInfo CScriptbind_3DEngine::GetTimeOfDayAdvancedInfo()
{
	ITimeOfDay::SAdvancedInfo info;
	gEnv->p3DEngine->GetTimeOfDay()->GetAdvancedInfo(info);

	return info;
}

void CScriptbind_3DEngine::SetTimeOfDayAdvancedInfo(ITimeOfDay::SAdvancedInfo advancedInfo)
{
	gEnv->p3DEngine->GetTimeOfDay()->SetAdvancedInfo(advancedInfo);
}

void CScriptbind_3DEngine::SetTimeOfDayVariableValue(ITimeOfDay::ETimeOfDayParamID id, float value)
{
	float valueArray[3];
	valueArray[0] = value;
	gEnv->p3DEngine->GetTimeOfDay()->SetVariableValue(id, valueArray);
}

void CScriptbind_3DEngine::SetTimeOfDayVariableValueColor(ITimeOfDay::ETimeOfDayParamID id, Vec3 value)
{
	float valueArray[3];
	valueArray[0] = value.x;
	valueArray[1] = value.x;
	valueArray[2] = value.x;

	gEnv->p3DEngine->GetTimeOfDay()->SetVariableValue(id, valueArray);
}

void CScriptbind_3DEngine::ActivatePortal(Vec3 pos, bool activate, mono::string entityName)
{
	gEnv->p3DEngine->ActivatePortal(pos, activate, ToCryString(entityName));
}

float CScriptbind_3DEngine::GetMaxViewDistance()
{
	return gEnv->p3DEngine->GetMaxViewDistance();
}