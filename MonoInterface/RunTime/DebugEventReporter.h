#pragma once

#include "IMonoInterface.h"
//! Reports events that occur within CryCIL.
struct DebugEventReporter : IMonoSystemListener
{
	virtual void SetInterface(IMonoInterface *handle)
	{
		// No need to set anything. We have access to MonoEnv,
		// since we are defined within MonoInterface.dll
	}

	virtual void OnPreInitialization()
	{
		CryLogAlways("Ready to initialize CryCIL.");
	}

	virtual void OnRunTimeInitializing()
	{
		CryLogAlways("Initializing Mono run-time environment.");
	}

	virtual void OnRunTimeInitialized()
	{
		CryLogAlways("Mono run-time environment initialized successfully.");
	}

	virtual void OnCryamblyInitilizing()
	{
		CryLogAlways("Initializing [Cryambly]CryCil.RunTime.MonoInterface object.");
	}

	virtual void OnCompilationStarting()
	{
		CryLogAlways("Compiling the code.");
	}

	virtual void OnCompilationComplete(bool success)
	{
		CryLogAlways("Compilation of the code was %ssuccessful.", success ? "" : "not ");
	}

	virtual int * GetSubscribedStages(int &stageCount)
	{
		stageCount = 4;
		int *stages = new int[stageCount];
		stages[0] = ENTITY_REGISTRATION_STAGE;				// Entities registration.
		stages[1] = ACTORS_REGISTRATION_STAGE;				// Actors registration.
		stages[2] = GAME_MODE_REGISTRATION_STAGE;			// Game modes registration.
		stages[3] = FLOWNODE_RECOGNITION_STAGE;				// Flow graph nodes recognition.
		return stages;
	}

	virtual void OnInitializationStage(int stageIndex)
	{
		switch (stageIndex)
		{
		case ENTITY_REGISTRATION_STAGE:
			CryLogAlways("Registering entities.");
			break;
		case ACTORS_REGISTRATION_STAGE:
			CryLogAlways("Registering actors.");
			break;
		case GAME_MODE_REGISTRATION_STAGE:
			CryLogAlways("Registering game modes.");
			break;
		case FLOWNODE_RECOGNITION_STAGE:
			CryLogAlways("Preparing registration data for FLowGraph nodes.");
			break;
		default:
			break;
		}
	}

	virtual void OnCryamblyInitilized()
	{
		CryLogAlways("Cryambly initialization complete.");
	}

	virtual void OnPostInitialization()
	{
		CryLogAlways("CryCIL initialization complete.");
	}

	virtual void Update()
	{
		CryComment("CryCIL logical frame started.");
	}

	virtual void PostUpdate()
	{
		CryComment("CryCIL logical frame ended.");
	}

	virtual void Shutdown()
	{
		CryLogAlways("CryCIL shutdown sequence started.");
	}
};