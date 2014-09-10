/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// 3DEngine scriptbind
//////////////////////////////////////////////////////////////////////////
// 13/01/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_3DENGINE__
#define __SCRIPTBIND_3DENGINE__

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

#include "Renderer.h"

class CScriptbind_3DEngine : public IMonoScriptBind
{
public:
	CScriptbind_3DEngine();
	~CScriptbind_3DEngine() {}

	// IMonoScriptBind
	virtual const char *GetClassName() { return "Engine3DInterop"; }
	// ~IMonoScriptBind

	static float GetTerrainElevation(float x, float y);
	static float GetTerrainZ(int x, int y);

	static int GetTerrainSize();
	static int GetTerrainSectorSize();
	static int GetTerrainUnitSize();

	static void SetTimeOfDay(float hour, bool forceUpdate);
	static float GetTimeOfDay();

	static ITimeOfDay::SAdvancedInfo GetTimeOfDayAdvancedInfo();
	static void SetTimeOfDayAdvancedInfo(ITimeOfDay::SAdvancedInfo advancedInfo);

	static void SetTimeOfDayVariableValue(ITimeOfDay::ETimeOfDayParamID id, float value);
	static void SetTimeOfDayVariableValueColor(ITimeOfDay::ETimeOfDayParamID id, Vec3 value);

	static void ActivatePortal(Vec3 pos, bool activate, mono::string entityName);

	static float GetMaxViewDistance();
};

#endif //__SCRIPTBIND_3DENGINE__