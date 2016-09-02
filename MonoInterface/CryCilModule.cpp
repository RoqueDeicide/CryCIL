#include "stdafx.h"
#include "CryCilModule.h"
#include <CryCore/Platform/platform_impl.inl>			// One time this file is included.
#include "RunTime/MonoInterface.h"

IMonoInterface *MonoEnv = nullptr;
//! Provides access to IGameFramework implementation.
IGameFramework *Framework = nullptr;

// Statically allocated interface object.
MonoInterface g_MonoInterface;

bool CryCilModule::Initialize(SSystemGlobalEnvironment& env, const SSystemInitParams& initParams)
{
	// Initializes gEnv variable, registers some objects.
	// Fun fact: Module name is only used for Unit Tests.
	ModuleInitISystem(env.pSystem, "CryCIL");

	// Use static allocation. Allows to not have to call a destructor.
	return new (&g_MonoInterface)MonoInterface(this->framework, this->listeners, MonoLog::Level::Message,
											   initParams);
}
