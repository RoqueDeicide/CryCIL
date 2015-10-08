#pragma once

#include "IMonoInterface.h"

struct StaticSubObjectInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "StaticSubObject"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.StaticObjects"; }

	virtual void OnRunTimeInitialized() override;

	static mono::string GetName(IStatObj::SSubObject *obj);
	static void         SetName(IStatObj::SSubObject *obj, mono::string name);
	static mono::string GetProperties(IStatObj::SSubObject *obj);
	static void         SetProperties(IStatObj::SSubObject *obj, mono::string props);
};