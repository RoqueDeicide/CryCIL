#include "stdafx.h"

#include "DebugEventReporter.h"

void DebugEventReporter::OnPreInitialization()
{
	CryLogAlways("Ready to initialize CryCIL.");
}

void DebugEventReporter::OnRunTimeInitializing()
{
	CryLogAlways("Initializing Mono run-time environment.");
}

void DebugEventReporter::OnRunTimeInitialized()
{
	CryLogAlways("Mono run-time environment initialized successfully.");
}

void DebugEventReporter::OnCryamblyInitilizing()
{
	CryLogAlways("Initializing [Cryambly]CryCil.RunTime.MonoInterface object.");
}

void DebugEventReporter::OnCompilationStarting()
{
	CryLogAlways("Compiling the code.");
}

void DebugEventReporter::OnCompilationComplete(bool success)
{
	CryLogAlways("Compilation of the code was %ssuccessful.", success ? "" : "not ");
}

List<int> *DebugEventReporter::GetSubscribedStages()
{
	List<int> *stages = new List<int>(4);
	stages->Add(ENTITY_REGISTRATION_STAGE);				// Entities registration.
	stages->Add(ACTORS_REGISTRATION_STAGE);				// Actors registration.
	stages->Add(GAME_MODE_REGISTRATION_STAGE);			// Game modes registration.
	stages->Add(FLOWNODE_RECOGNITION_STAGE);			// Flow graph nodes recognition.
	return stages;
}

void DebugEventReporter::OnInitializationStage(int stageIndex)
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
		CryLogAlways("Preparing registration data for FlowGraph nodes.");
		break;
	default:
		break;
	}
}

void DebugEventReporter::OnCryamblyInitilized()
{
	CryLogAlways("Cryambly initialization complete.");
}

void DebugEventReporter::OnPostInitialization()
{
	CryLogAlways("CryCIL initialization complete.");
}

void DebugEventReporter::Update()
{
	FlickerComment("CryCIL logical frame started.");
}

void DebugEventReporter::PostUpdate()
{
	FlickerComment("CryCIL logical frame ended.");
}

void DebugEventReporter::Shutdown()
{
	CryLogAlways("CryCIL shutdown sequence started.");
}