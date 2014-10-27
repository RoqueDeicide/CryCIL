#include "stdafx.h"
#include "ExampleDefines.h"

#ifdef EXAMPLES
#include "IMonoInterface.h"

#ifndef PI
#define PI 3.14159265358979323f
#endif

#include <CryLibrary.h>

struct SimpleListener : public IMonoSystemListener
{
private:
	IMonoInterface *monoEnv;
public:
	SimpleListener() : monoEnv(nullptr){}

	virtual void SetInterface(IMonoInterface *handle)
	{
		// This is the only IMonoInterface pointer we can use before MonoEnv is initialized.
		this->monoEnv = handle;
	}

	virtual void OnPreInitialization()
	{
		// Cannot really interact with IMonoInterface right now.
		CryLogAlways("CryCIL initialization is about to begin.");
	}

	virtual void OnRunTimeInitializing()
	{
		// Same as in the method above.
		CryLogAlways("Mono run-time initialization is about to begin.");
	}

	virtual void OnRunTimeInitialized()
	{
		// Now we can use Mono.
		mono::nothing(*BeepFunc)(mono::exception *);
		BeepFunc = (mono::nothing(*)(mono::exception *))this->monoEnv->CoreLibrary
					->GetClass("Console", "System")->GetMethod("Beep")->UnmanagedThunk;
		// Beep!
		mono::exception ex;
		BeepFunc(&ex);
	}

	virtual void OnCryamblyInitilizing()
	{
		// Cryambly is loaded, now we can do stuff like working with quaternions.
		Vec3 up(0, 0, 1);
		float rads = 60 * PI / 180;

		mono::exception ex;
		Quat rotation = Unbox<Quat>(((mono::quaternion(*)(mono::float32, mono::vector3, mono::exception *))this->monoEnv->Cryambly->GetClass("Quaternion", "CryCil.Mathematics")->GetMethod("CreateRotationAngleAxis", 2)->UnmanagedThunk)(Box(rads), Box(up), &ex));
		CryLogAlways("Quat = %s, %s, %s, %s", rotation.v.x, rotation.v.y, rotation.v.z, rotation.w);
	}

	virtual void OnCompilationStarting()
	{
		CryLogAlways("Extra assemblies in Modules folder are loaded at this point.");
	}

	virtual void OnCompilationComplete(bool success)
	{
		if (success)
		{
			CryLogAlways("Compiled assemblies can be accessed.");
		}
		else
		{
			CryLogAlways("Compiled assemblies cannot be accessed.");
		}
	}

	virtual int *GetSubscribedStages(int &stageCount)
	{
		stageCount = 8;
		int *stages = new int[stageCount];
		stages[0] = ENTITY_REGISTRATION_STAGE - 1;			// Before entities registration.
		stages[1] = ENTITY_REGISTRATION_STAGE + 1;			// After entities registration.
		stages[2] = ACTORS_REGISTRATION_STAGE - 1;			// Before actors registration.
		stages[3] = ACTORS_REGISTRATION_STAGE + 1;			// After actors registration.
		stages[4] = GAME_MODE_REGISTRATION_STAGE - 1;		// Before game modes registration.
		stages[5] = GAME_MODE_REGISTRATION_STAGE + 1;		// After game modes registration.
		stages[6] = FLOWNODE_RECOGNITION_STAGE - 1;			// Before flow graph nodes recognition.
		stages[7] = FLOWNODE_RECOGNITION_STAGE + 1;			// After flow graph nodes recognition.
		return stages;
	}

	virtual void OnInitializationStage(int stageIndex)
	{
		switch (stageIndex)
		{
		case ENTITY_REGISTRATION_STAGE - 1:
			CryLogAlways("Entities defined by CryCIL are about to be registered.");
			break;
		case ENTITY_REGISTRATION_STAGE + 1:
			CryLogAlways("Entities defined by CryCIL are registered.");
			break;
		case ACTORS_REGISTRATION_STAGE - 1:
			CryLogAlways("Actors defined by CryCIL are about to be registered.");
			break;
		case ACTORS_REGISTRATION_STAGE + 1:
			CryLogAlways("Actors defined by CryCIL are registered.");
			break;
		case GAME_MODE_REGISTRATION_STAGE - 1:
			CryLogAlways("Game modes defined by CryCIL are about to be registered.");
			break;
		case GAME_MODE_REGISTRATION_STAGE + 1:
			CryLogAlways("Game modes defined by CryCIL are registered.");
			break;
		case FLOWNODE_RECOGNITION_STAGE - 1:
			CryLogAlways("FlowGraph nodes defined by CryCIL are about to be recognized.");
			break;
		case FLOWNODE_RECOGNITION_STAGE + 1:
			CryLogAlways("FlowGraph nodes defined by CryCIL are recognized.");
			break;
		default:
			break;
		}
	}

	virtual void OnCryamblyInitilized()
	{
		CryLogAlways("Managed part of IMonoInterface implementation is fully initialized.");
	}

	virtual void OnPostInitialization()
	{
		CryLogAlways("CryCIL is fully initialized.");
	}

	virtual void Update()
	{
		CryLogAlways("Logical update of CryCIL.");
	}

	virtual void PostUpdate()
	{
		CryLogAlways("Logical update of CryCIL is over.");
	}

	virtual void Shutdown()
	{
		CryLogAlways("Shutting down.");
	}
};

void HowToRegisterListeners()
{
	// We have to pass listeners to InitializeModule if you them to react to initialization events.
	int listenerCount = 1;
	IMonoSystemListener **listeners = new IMonoSystemListener *[listenerCount];
	listeners[0] = new SimpleListener();
	// Now we can initialize CryCIL.
	HMODULE monoInterfaceDll = CryLoadLibrary(MONOINTERFACE_LIBRARY);
	if (monoInterfaceDll)
	{
		InitializeMonoInterface init =
			(InitializeMonoInterface)CryGetProcAddress(monoInterfaceDll, MONO_INTERFACE_INIT);
		// Replace nullptr with a pointer to IGameFramework.
		init(nullptr, listeners, listenerCount);
	}
}

#endif // EXAMPLES