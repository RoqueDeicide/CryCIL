#include "stdafx.h"

#include "TimeOfDay.h"
#include "MonoCryXmlNode.h"

struct MonoTodVarInfo
{
	const char *name;  // Variable name.
	const char *displayName;  // Variable user readable name.
	const char *group; // Group name.
	int nParamId;
	ITimeOfDay::EVariableType type;
	float fValue[3];    // Value of the variable (3 needed for color type)
	ISplineInterpolator* pInterpolator; // Splines that control variable value
};

void TimeOfDayInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_Cycle);
	REGISTER_METHOD(set_Cycle);
	REGISTER_METHOD(get_Sun);
	REGISTER_METHOD(set_Sun);
	REGISTER_METHOD(get_Time);
	REGISTER_METHOD(set_Time);

	REGISTER_METHOD(Pause);
	REGISTER_METHOD(Unpause);
	REGISTER_METHOD(Save);
	REGISTER_METHOD(Load);
	REGISTER_METHOD(Sync);
	REGISTER_METHOD(NetworkSync);
	REGISTER_METHOD(GetVariableInfo);
	REGISTER_METHOD(SetVariableValue);
	REGISTER_METHOD(SetVariableValueColor);
	REGISTER_METHOD(ResetVariables);
}

ITimeOfDay::SAdvancedInfo TimeOfDayInterop::get_Cycle()
{
	ITimeOfDay::SAdvancedInfo info;
	gEnv->p3DEngine->GetTimeOfDay()->GetAdvancedInfo(info);
	return info;
}

void TimeOfDayInterop::set_Cycle(ITimeOfDay::SAdvancedInfo value)
{
	gEnv->p3DEngine->GetTimeOfDay()->SetAdvancedInfo(value);
}

ITimeOfDay::SEnvironmentInfo TimeOfDayInterop::get_Sun()
{
	ITimeOfDay::SEnvironmentInfo info;

	info.bSunLinkedToTOD = todSunLink;
	info.sunRotationLatitude = gEnv->p3DEngine->GetTimeOfDay()->GetSunLatitude();
	info.sunRotationLongitude = gEnv->p3DEngine->GetTimeOfDay()->GetSunLongitude();

	return info;
}

void TimeOfDayInterop::set_Sun(ITimeOfDay::SEnvironmentInfo value)
{
	gEnv->p3DEngine->GetTimeOfDay()->SetEnvironmentSettings(value);
}

float TimeOfDayInterop::get_Time()
{
	return gEnv->p3DEngine->GetTimeOfDay()->GetTime();
}

void TimeOfDayInterop::set_Time(float value)
{
	gEnv->p3DEngine->GetTimeOfDay()->SetTime(value);
}

void TimeOfDayInterop::Pause()
{
	gEnv->p3DEngine->GetTimeOfDay()->SetPaused(true);
}

void TimeOfDayInterop::Unpause()
{
	gEnv->p3DEngine->GetTimeOfDay()->SetPaused(false);
}

void TimeOfDayInterop::Save(MonoCryXmlNode *node)
{
	if (!node)
	{
		ArgumentNullException("The Xml data provider cannot be null.").Throw();
	}
	if (!node->handle)
	{
		ObjectDisposedException("The Xml data provider is not usable.").Throw();
	}

	XmlNodeRef nodeRef(node->handle);
	gEnv->p3DEngine->GetTimeOfDay()->Serialize(nodeRef, false);
}

void TimeOfDayInterop::Load(MonoCryXmlNode *node)
{
	if (!node)
	{
		ArgumentNullException("The Xml data provider cannot be null.").Throw();
	}
	if (!node->handle)
	{
		ObjectDisposedException("The Xml data provider is not usable.").Throw();
	}

	XmlNodeRef nodeRef(node->handle);
	gEnv->p3DEngine->GetTimeOfDay()->Serialize(nodeRef, true);
}

void TimeOfDayInterop::Sync(ISerialize *sync)
{
	if (!sync)
	{
		ArgumentNullException("Synchronization context is invalid.").Throw();
	}

	TSerialize s(sync);
	gEnv->p3DEngine->GetTimeOfDay()->Serialize(s);
}

void TimeOfDayInterop::NetworkSync(ISerialize *sync, uint syncFlags)
{
	if (!sync)
	{
		ArgumentNullException("Synchronization context is invalid.").Throw();
	}

	TSerialize s(sync);
	gEnv->p3DEngine->GetTimeOfDay()->NetSerialize(s, 0, syncFlags);
}

bool TimeOfDayInterop::GetVariableInfo(int id, MonoTodVarInfo *info)
{
	ITimeOfDay::SVariableInfo varInfo;

	if (gEnv->p3DEngine->GetTimeOfDay()->GetVariableInfo(id, varInfo))
	{
		*info = *reinterpret_cast<MonoTodVarInfo *>(&varInfo);
		return true;
	}

	return false;
}

void TimeOfDayInterop::SetVariableValue(int id, float value)
{
	float vals[3];
	vals[0] = value;
	vals[1] = 0;
	vals[2] = 0;
	gEnv->p3DEngine->GetTimeOfDay()->SetVariableValue(id, vals);
}

void TimeOfDayInterop::SetVariableValueColor(int id, ColorF *value)
{
	float vals[3];
	vals[0] = value->r;
	vals[1] = value->g;
	vals[2] = value->b;
	gEnv->p3DEngine->GetTimeOfDay()->SetVariableValue(id, vals);
}

void TimeOfDayInterop::ResetVariables()
{
	gEnv->p3DEngine->GetTimeOfDay()->ResetVariables();
}

bool TimeOfDayInterop::todSunLink = false;
