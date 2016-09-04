// CryCil.NativeInterface.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

// Included only once per module.
#include <CryCore/Platform/platform_impl.inl>

#include <CrySystem/IEngineModule.h>
#include <CryExtension/ICryFactory.h>
#include <CryExtension/ClassWeaver.h>

struct CryCil_NativeInterface_EngineModule : public IEngineModule
{
	CRYINTERFACE_SIMPLE(IEngineModule);
	CRYGENERATE_SINGLETONCLASS(CryCil_NativeInterface_EngineModule, "CryCil_NativeInterface_EngineModule",
							   0x2e173e2036b54640, 0x9cc71fff387c0457);
	
	const char *GetName() override { return "NativeInterface"; }
	const char *GetCategory() override { return "CryCil"; }
	
	bool Initialize(SSystemGlobalEnvironment &env, const SSystemInitParams &initParams) override
	{
		//
		// Initialize Mono interface.
		//

		return true;
	}
};

CRYREGISTER_SINGLETON_CLASS(CryCil_NativeInterface_EngineModule)