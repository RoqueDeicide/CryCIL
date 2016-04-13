#include "stdafx.h"
#include "IMonoInterface.h"

#include <CryLibrary.h>

struct SimpleListener : public IMonoSystemListener
{
private:
	IMonoInterface *monoEnv;
public:
	SimpleListener() : monoEnv(nullptr) {}

	virtual void SetInterface(IMonoInterface *handle) override
	{
		// This is the only IMonoInterface pointer we can use before MonoEnv is initialized.
		this->monoEnv = handle;
	}

	virtual void OnPreInitialization() override
	{
		// Cannot really interact with IMonoInterface right now.
		CryLogAlways("CryCIL initialization is about to begin.");
	}

	virtual void OnRunTimeInitializing() override
	{
		// Same as in the method above.
		CryLogAlways("Mono run-time initialization is about to begin.");
	}

	virtual void OnRunTimeInitialized() override
	{
		// Now we can use Mono.
		mono::nothing(*BeepFunc)(mono::exception *);
		BeepFunc = reinterpret_cast<mono::nothing(*)(mono::exception *)>
			(this->monoEnv->CoreLibrary->GetClass("Console", "System")->GetFunction("Beep")->UnmanagedThunk);
		// Beep!
		mono::exception ex;
		BeepFunc(&ex);
	}

	virtual void OnCryamblyInitilizing() override
	{
		// Cryambly is loaded, now we can do stuff like working with quaternions.
		Vec3 up(0, 0, 1);
		float rads = 60 * PI / 180;

		auto thunk =
			reinterpret_cast<mono::quaternion(*)(mono::vector3, float, mono::exception *)>
			(this->monoEnv->Cryambly->GetClass("Rotation", "CryCil.Geometry")
									->GetNestedType("AroundAxis")
									->GetFunction("CreateQuaternion", 2)
									->UnmanagedThunk);

		mono::exception ex;
		Quat rotation = Unbox<Quat>((thunk)(Box(up), rads, &ex));
		CryLogAlways("Quat = %s, %s, %s, %s", rotation.v.x, rotation.v.y, rotation.v.z, rotation.w);
	}

	virtual void OnCompilationStarting() override
	{
		CryLogAlways("Extra assemblies in Modules folder are loaded at this point.");
	}

	virtual void OnCompilationComplete(bool success) override
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

	virtual List<int> *GetSubscribedStages() override
	{
		List<int> *stages = new List<int>(8);
		stages->Add(ENTITY_REGISTRATION_STAGE - 1);			// Before entities registration.
		stages->Add(ENTITY_REGISTRATION_STAGE + 1);			// After entities registration.
		stages->Add(ACTION_MAPS_REGISTRATION_STAGE - 1);			// Before actors registration.
		stages->Add(ACTION_MAPS_REGISTRATION_STAGE + 1);			// After actors registration.
		stages->Add(GAME_MODE_REGISTRATION_STAGE - 1);		// Before game modes registration.
		stages->Add(GAME_MODE_REGISTRATION_STAGE + 1);		// After game modes registration.
		stages->Add(FLOWNODE_RECOGNITION_STAGE - 1);		// Before flow graph nodes recognition.
		stages->Add(FLOWNODE_RECOGNITION_STAGE + 1);		// After flow graph nodes recognition.
		return stages;
	}

	virtual void OnInitializationStage(int stageIndex) override
	{
		switch (stageIndex)
		{
		case ENTITY_REGISTRATION_STAGE - 1:
			CryLogAlways("Entities defined by CryCIL are about to be registered.");
			break;
		case ENTITY_REGISTRATION_STAGE + 1:
			CryLogAlways("Entities defined by CryCIL are registered.");
			break;
		case ACTION_MAPS_REGISTRATION_STAGE - 1:
			CryLogAlways("Actors defined by CryCIL are about to be registered.");
			break;
		case ACTION_MAPS_REGISTRATION_STAGE + 1:
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

	virtual void OnCryamblyInitilized() override
	{
		CryLogAlways("Managed part of IMonoInterface implementation is fully initialized.");
	}

	virtual void OnPostInitialization() override
	{
		CryLogAlways("CryCIL is fully initialized.");
	}

	virtual void Update() override
	{
		CryLogAlways("Logical update of CryCIL.");
	}

	virtual void PostUpdate() override
	{
		CryLogAlways("Logical update of CryCIL is over.");
	}

	virtual void Shutdown() override
	{
		CryLogAlways("Shutting down.");
	}
};

inline void HowToRegisterListeners()
{
	// We have to pass listeners to InitializeModule if you them to react to initialization events.
	auto listeners = List<IMonoSystemListener *>(1);
	listeners.Add(new SimpleListener());
	// Now we can initialize CryCIL.
	HMODULE monoInterfaceDll = CryLoadLibrary(MONOINTERFACE_LIBRARY);
	if (monoInterfaceDll)
	{
		auto init = reinterpret_cast<InitializeMonoInterface>(CryGetProcAddress(monoInterfaceDll, MONO_INTERFACE_INIT));
		// Replace nullptr with a pointer to IGameFramework.
		init(nullptr, &listeners);
	}
}