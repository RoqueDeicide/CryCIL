#pragma once

#include <CrySystem/IEngineModule.h>
#include <CryExtension/ClassWeaver.h>

//! Represents the object that defines this engine module as an extension of CryEngine.
class CryCilModule : public IEngineModule
{
	IGameFramework *framework;
	List<IMonoSystemListener *> *listeners;

	CRYINTERFACE_SIMPLE(CryCilModule);
	CRYGENERATE_SINGLETONCLASS(CryCilModule, "CryCilModule", 0x7044970d351f4b62, 0x888a3368f1aafff);

public:
	CryCilModule(IGameFramework *framework, List<IMonoSystemListener *> *listeners)
		: framework(framework)
		, listeners(listeners)
	{
	}

	const char *GetName() override { return "CryCilSystem"; }
	const char *GetCategory() override { return "Mono"; }
	
	bool Initialize(SSystemGlobalEnvironment& env, const SSystemInitParams& initParams) override;
};