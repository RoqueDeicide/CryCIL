#pragma once

#include "IMonoInterface.h"

struct MonoTodVarInfo;
struct MonoCryXmlNode;

struct TimeOfDayInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "TimeOfDay"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Environment"; }

	virtual void OnRunTimeInitialized();

	static ITimeOfDay::SAdvancedInfo get_Cycle();
	static void set_Cycle(ITimeOfDay::SAdvancedInfo value);
	static ITimeOfDay::SEnvironmentInfo get_Sun();
	static void set_Sun(ITimeOfDay::SEnvironmentInfo value);
	static float get_Time();
	static void set_Time(float value);
	
	static void Pause();
	static void Unpause();
	static void Save(MonoCryXmlNode *node);
	static void Load(MonoCryXmlNode *node);
	static void Sync(ISerialize *sync);
	static void NetworkSync(ISerialize *sync, uint syncFlags);
	static bool GetVariableInfo(int id, MonoTodVarInfo *info);
	static void SetVariableValue(int id, float value);
	static void SetVariableValueColor(int id, ColorF *value);
	static void ResetVariables();

	static bool todSunLink;
};