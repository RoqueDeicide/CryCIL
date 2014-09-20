#pragma once

#include "IMonoRunTime.h"
#include "PathUtils.h"
#include "SystemEventListener_CryMono.h"
#include "MonoAssembly.h"
#include "MonoCommon.h"
#include "MonoArray.h"
#include "MonoClass.h"
#include "MonoObject.h"
#include "MonoDomain.h"
#include "AllInterops.h"
#include "MonoCVars.h"
#include "platform.h"

#include <IGameFramework.h>
#include <ISystem.h>

#include <mono/metadata/assembly.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/mono-debug.h>
#include <mono/mini/jit.h>

#include <cstdlib>
#include <csignal>

void HandleSignalAbort(int error)
{
	CryLogAlways("Aborted %i", error);
}

/*!
* Class that provides access to Mono run-time from CryEngine.
*/
class MonoRunTime
	: public IMonoRunTime
	, public IGameFrameworkListener
{
	typedef std::map<const void *, const char *> InteropList;
private:
	CScriptDomain *mainAppDomain;			//!< Main AppDomain where managed code is executed.
	IMonoAssembly *cryBraryAssembly;		//!< Managed assembly that handles CryMono inner workings.
	IMonoAssembly *pdb2MdbAssembly;			//!< Optional assembly that contains code that converts .pdb to .mdb files.
	IGameFramework *gameFramework;			//!< Object that provides access to CryAction subsystems.
	IMonoObject *monoInterface;				//!< Object that handles CryEngine-Mono interoperability.
	InteropList interopsMethods;			//!< List of interop methods to register as internal calls.
	std::vector<IMonoInterop *> interops;	//!< List of interop classes.
	CFlowManager *flowGraphInterop;			//!< Object that handles interops with FlowGraph.
	bool quitting;							//!< Indicates whether disposal of Mono Run-Time is in progress.
	SCVars *consoleVariables;				//!< Class that provides the console variables.
public:
	MonoRunTime(IGameFramework *pGameFramework);
	~MonoRunTime();
	// IGameFrameworkListener overrides.
	virtual void OnPostUpdate(float fDeltaTime)
	{
		// Notify everything about the update.
		if (this->monoInterface)
		{
			this->monoInterface->CallMethod
			(
				"Think",
				fDeltaTime,
				gEnv->pTimer->GetFrameStartTime().GetMilliSeconds(),
				gEnv->pTimer->GetAsyncTime().GetMilliSeconds(),
				gEnv->pTimer->GetFrameRate(),
				gEnv->pTimer->GetTimeScale()
			);
		}
	}
	virtual void OnSaveGame(ISaveGame* pSaveGame) {}
	virtual void OnLoadGame(ILoadGame* pLoadGame) {}
	virtual void OnLevelEnd(const char* nextLevel) {}
	virtual void OnActionEvent(const SActionEvent& event) {}

	void RegisterInteropMethod(const void *method, const char *fullMethodName)
	{
		// If app domain is live then add internal call, otherwise save for later.
		if (this->mainAppDomain)
		{
			mono_add_internal_call(fullMethodName, method);
		}
		else
		{
			this->interopsMethods.insert(InteropList::value_type(method, fullMethodName));
		}
	}
#define REGISTER_INTEROP(T) this->interops.push_back(new T());
	//! Registers most standard interop classes that come with CryMono.
	void RegisterMainInterops()
	{
		REGISTER_INTEROP(Engine3DInterop);
		REGISTER_INTEROP(MaterialManagerInterop);
		REGISTER_INTEROP(ParticleSystemInterop);
		REGISTER_INTEROP(EntityInterop);
		REGISTER_INTEROP(GameObjectInterop);
		REGISTER_INTEROP(PhysicsInterop);
		REGISTER_INTEROP(ActorSystemInterop);
		REGISTER_INTEROP(GameRulesInterop);
		REGISTER_INTEROP(CryMarshalInterop);
		REGISTER_INTEROP(CMeshInterop);
		REGISTER_INTEROP(StaticObjectInterop);
		REGISTER_INTEROP(ConsoleInterop);
		REGISTER_INTEROP(CryPakInterop);
		REGISTER_INTEROP(DebugInterop);
		REGISTER_INTEROP(TimeInterop);
		REGISTER_INTEROP(RendererInterop);
		REGISTER_INTEROP(ViewSystemInterop);
		REGISTER_INTEROP(CrySerializeInterop);
		REGISTER_INTEROP(LevelSystemInterop);
		REGISTER_INTEROP(NetworkInterop);
		REGISTER_INTEROP(PlatformInterop);
		REGISTER_INTEROP(ScriptTableInterop);
		// Some special treatment for flow manager, because we need to increase its ref count.
		this->flowGraphInterop = new CFlowManager();
		this->flowGraphInterop->AddRef();
	}
	//! Registers interop classes and methods that require late processing,
	//! and those that were put into registration queue prior to CryMono initialization.
	void RegisterSecondaryInterops()
	{
		// Register methods, that for some reason were put
		// into queue before initialization of Mono Run-Time.
		if (this->interopsMethods.size() > 0)
		{
			for
			(
				InteropList::iterator it = this->interopsMethods.begin();
			it != this->interopsMethods.end();
			++it
		)
		this->RegisterInteropMethod((*it).first, (*it).second);
		}
		REGISTER_INTEROP(InputInterop);
		// Clear those who were late to the party.
		this->interopsMethods.clear();
	}
#undef REGISTER_INTEROP
	//! Triggers destructor in specified interop and deletes it from the list.
	void DeleteInterop(IMonoInterop *interop)
	{
		if (this->quitting)
		{
			return;
		}
		stl::find_and_erase(this->interops, interop);
	}
	//! Signals CryMono to register the flow nodes.
	void RegisterFlowNodes()
	{
		if (this->monoInterface && this->gameFramework->GetIFlowSystem())
		{
			this->monoInterface->CallMethod("RegisterFlowNodeTypes");
		}
	}

	IMonoArray *ToArray(IMonoObject *arr)
	{
		CRY_ASSERT(arr);

		return new CScriptArray(arr);
	}

	IMonoObject *ToObject(IMonoObject *obj, bool allowGC)
	{
		CRY_ASSERT(obj);

		return new CScriptObject((MonoObject *)obj, allowGC);
	}
	void Release()
	{
		delete this;
	}
	// Properties.
	IMonoAssembly *GetCryBrary() const { return this->cryBraryAssembly; }
	__declspec(property(get=GetCryBrary)) IMonoAssembly *CryBrary;
	IMonoAssembly *GetPdbMdbAssembly() const { return this->pdb2MdbAssembly; }
	__declspec(property(get=GetPdbMdbAssembly)) IMonoAssembly *Pdb2Mdb;
	IGameFramework *GetGameFramework() const { return this->gameFramework; }
	__declspec(property(get=GetGameFramework)) IGameFramework *GameFramework;
	IMonoAssembly *GetCoreLibrary() const
	{
		return this->mainAppDomain->TryGetAssembly(mono_get_corlib());
	}
	__declspec(property(get=GetGameFramework)) IMonoAssembly *CoreLibrary;
	CScriptDomain *GetAppDomain() const { return this->mainAppDomain; }
	__declspec(property(get=GetAppDomain)) CScriptDomain *AppDomain;
	bool HasBeenInitialized() const { return this->mainAppDomain != nullptr; }
	__declspec(property(get=HasBeenInitialized)) bool IsInitialized;
	CFlowManager *GetFlowManager() const { return this->flowGraphInterop; }
	__declspec(property(get=GetFlowManager)) CFlowManager *FlowManager;
};
//! Gets global MonoRunTime instance.
static MonoRunTime *GetMonoRunTime()
{
	return static_cast<MonoRunTime *>(IMonoRunTime::instance);
}