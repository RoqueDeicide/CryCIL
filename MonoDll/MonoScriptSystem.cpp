#include "stdafx.h"
#include "MonoScriptSystem.h"

#include "SystemEventListener_CryMono.h"

#include "MonoAssembly.h"
#include "MonoCommon.h"
#include "MonoArray.h"
#include "MonoClass.h"
#include "MonoObject.h"
#include "MonoDomain.h"

#include "CryScriptInstance.h"

#include <mono/mini/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/mono-debug.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/appdomain.h>
#include <mono/metadata/object.h>
#include <mono/metadata/threads.h>
#include <mono/metadata/environment.h>
#include <mono/metadata/mono-gc.h>

#include <ICmdLine.h>
#include <ISystem.h>

#include "MonoConverter.h"

// Bindings
#include "Scriptbinds\Console.h"
#include "Scriptbinds\GameRules.h"
#include "Scriptbinds\ActorSystem.h"
#include "Scriptbinds\3DEngine.h"
#include "Scriptbinds\Physics.h"
#include "Scriptbinds\Renderer.h"
#include "Scriptbinds\Debug.h"
#include "Scriptbinds\MaterialManager.h"
#include "Scriptbinds\ParticleSystem.h"
#include "Scriptbinds\ViewSystem.h"
#include "Scriptbinds\LevelSystem.h"
#include "Scriptbinds\Entity.h"
#include "Scriptbinds\Network.h"
#include "Scriptbinds\Time.h"
#include "Scriptbinds\ScriptTable.h" 
#include "Scriptbinds\CrySerialize.h"
#include "Scriptbinds\GameObject.h"
#include "Scriptbinds\Sound.h"
#include "Scriptbinds\CryPak.h"

#include "FlowManager.h"
#include "MonoFlowNode.h"
#include "MonoInput.h"

#include "Actor.h"

#include "EntityEventHandling.h"

#include "MonoCVars.h"
#include "PathUtils.h"

#include <Windows.h>

SCVars *g_pMonoCVars = 0;
CScriptSystem *g_pScriptSystem = 0;

CScriptSystem::CScriptSystem(IGameFramework *pGameFramework)
	: m_pRootDomain(nullptr)
	, m_pCryBraryAssembly(nullptr)
	, m_pPdb2MdbAssembly(nullptr)
	, m_pScriptManager(nullptr)
	, m_pScriptDomain(nullptr)
	, m_bReloading(false)
	, m_bDetectedChanges(false)
	, m_bQuitting(false)
	, m_pConverter(nullptr)
	, m_bFirstReload(true)
	, m_pGameFramework(pGameFramework)
{
	CryLogAlways("Initializing Mono Script System");

#ifndef PLUGIN_SDK
	gEnv->pMonoScriptSystem = this;
#endif;

	g_pScriptSystem = this;

	m_pCVars = new SCVars();
	g_pMonoCVars = m_pCVars;
	
	// We should look into storing mono binaries, configuration as well as scripts via CryPak.
	mono_set_dirs(PathUtils::GetMonoLibPath(), PathUtils::GetMonoConfigPath());

#ifndef _RELEASE
	// Enable Mono signal handling
	// Makes sure that Mono sends back exceptions it tries to handle, for CE crash handling.
	mono_set_signal_chaining(true);
#endif


	string monoCmdOptions = "";

	if(auto *pArg = gEnv->pSystem->GetICmdLine()->FindArg(eCLAT_Pre, "monoArgs"))
		monoCmdOptions.append(pArg->GetValue());
	// Commandline switch -DEBUG makes the process connect to the debugging server. Warning: Failure to connect to a debugging server WILL result in a crash.
	// This is currently a WIP feature which requires custom MonoDevelop extensions and other irritating things.
	const ICmdLineArg* arg = gEnv->pSystem->GetICmdLine()->FindArg(eCLAT_Pre, "DEBUG");
	if (arg != nullptr)
		monoCmdOptions.append("--debugger-agent=transport=dt_socket,address=127.0.0.1:65432,embedding=1");
#ifndef _RELEASE
	else if(g_pMonoCVars->mono_softBreakpoints) // Soft breakpoints not compatible with debugging server
	{
		CryLogAlways("		[Performance Warning] Mono soft breakpoints are enabled!");

		// Prevents managed null reference exceptions causing crashes in unmanaged code
		// See: https://bugzilla.xamarin.com/show_bug.cgi?id=5963
		monoCmdOptions.append("--soft-breakpoints");
	}
#endif

	char *options = new char[monoCmdOptions.size() + 1];
	strcpy(options, monoCmdOptions.c_str());

	// Note: iPhone requires AOT compilation, this can be enforced via mono options. TODO: Get Crytek to add CryMobile support to the Free SDK.
	mono_jit_parse_options(1, &options);

#ifndef _RELEASE
	// Required for mdb's to load for detailed stack traces etc.
	mono_debug_init(MONO_DEBUG_FORMAT_MONO);
#endif

	m_pConverter = new CConverter();

	if(!CompleteInit())
	{
		CryLogAlways("CryMono initialization failed!");
		return;
	}

	CacheManagedResources();

	RegisterSecondaryBindings();

	pGameFramework->RegisterListener(this, "CryMono", FRAMEWORKLISTENERPRIORITY_GAME);

	gEnv->pSystem->GetISystemEventDispatcher()->RegisterListener(&g_systemEventListener_CryMono);

	if(IFileChangeMonitor *pFileChangeMonitor = gEnv->pFileChangeMonitor)
		pFileChangeMonitor->RegisterListener(this, "scripts\\");

	CryModuleMemoryInfo memInfo;
	CryModuleGetMemoryInfo(&memInfo);

	IMonoClass *pCryStats = m_pCryBraryAssembly->GetClass("CryStats", "CryEngine.Utilities");

	IMonoObject *pMemoryUsage = *pCryStats->GetPropertyValue(NULL, "MemoryUsage");
	CryLogAlways("		Initializing CryMono done, MemUsage=%iKb", (memInfo.allocated + pMemoryUsage->Unbox<long>()) / 1024);
	pMemoryUsage->Release();
}

CScriptSystem::~CScriptSystem()
{
	m_bQuitting = true;

	for each(auto listener in m_listeners)
		listener->OnShutdown();
	m_listeners.clear();

	for(auto it = m_localScriptBinds.begin(); it != m_localScriptBinds.end(); ++it)
		delete (*it);
	m_localScriptBinds.clear();

	SAFE_RELEASE(m_pScriptManager);

	for(auto it = m_domains.rbegin(); it != m_domains.rend(); ++it)
		(*it)->Release();
	m_domains.clear();

	if(g_pScriptSystem->GetIGameFramework())
		g_pScriptSystem->GetIGameFramework()->UnregisterListener(this);

	if(IFileChangeMonitor *pFileChangeMonitor = gEnv->pFileChangeMonitor)
		pFileChangeMonitor->UnregisterListener(this);

	m_methodBindings.clear();

	SAFE_DELETE(m_pConverter);

	SAFE_DELETE(m_pCVars);

	g_pScriptSystem = nullptr;

#ifndef PLUGIN_SDK
	gEnv->pMonoScriptSystem = nullptr;
#endif
}

bool CScriptSystem::CompleteInit()
{
	CryLogAlways("		Initializing CryMono ...");
	
	// Create root domain and determine the runtime version we'll be using.
	m_pRootDomain = new CScriptDomain(eRV_4_30319);
	m_domains.push_back(m_pRootDomain);

	CScriptArray::m_pDefaultElementClass = mono_get_object_class();

#ifndef _RELEASE
	m_pPdb2MdbAssembly = m_pRootDomain->LoadAssembly(PathUtils::GetMonoPath() + "bin\\pdb2mdb.dll");
#endif

	RegisterPriorityBindings();

	return Reload();
}

bool CScriptSystem::Reload()
{
	if((!m_bFirstReload && g_pMonoCVars->mono_realtimeScripting == 0) || m_bReloading)
		return false;

	m_bReloading = true;

	if(!m_bFirstReload)
	{
		for each(auto listener in m_listeners)
			listener->OnReloadStart();

		m_pScriptManager->CallMethod("Serialize");
	}

	IMonoDomain *pScriptDomain = CreateDomain("ScriptDomain", nullptr, true);

	IMonoAssembly *pCryBraryAssembly = pScriptDomain->LoadAssembly(PathUtils::GetBinaryPath() + "CryBrary.dll");

	IMonoArray *pCtorParams = CreateMonoArray(2);
	pCtorParams->InsertAny(m_bFirstReload);
	pCtorParams->InsertMonoString(ToMonoString(PathUtils::GetConfigPath()));

	IMonoObject *pScriptManager = *pCryBraryAssembly->GetClass("ScriptManager", "CryEngine.Initialization")->CreateInstance(pCtorParams);
	SAFE_RELEASE(pCtorParams);

	auto result = pScriptManager->CallMethod("Initialize", m_bFirstReload);
	if(result == nullptr)
		return false;

	IMonoObject *pResult = *result;
	auto reloadResult = pResult->Unbox<EScriptReloadResult>();
	SAFE_RELEASE(pResult);

	switch(reloadResult)
	{
	case EScriptReloadResult_Success:
		{
			// revert previous domain
			if(!m_bFirstReload)
				m_pScriptDomain->Release();

			m_pScriptDomain = pScriptDomain;
			m_pScriptManager = pScriptManager;
			m_pCryBraryAssembly = pCryBraryAssembly;

			if(!m_bFirstReload)
				m_pScriptManager->CallMethod("Deserialize");

			// Set Network.Editor etc.
			IMonoClass *pClass = m_pCryBraryAssembly->GetClass("Game");

			IMonoArray *pArgs = CreateMonoArray(2);
			pArgs->Insert(gEnv->IsEditor());
			pArgs->Insert(gEnv->IsDedicated());
			pClass->GetMethod("InitializeGameStatics", 2)->InvokeArray(nullptr, pArgs);
			SAFE_RELEASE(pArgs);

			m_pScriptManager->CallMethod("ProcessWaitingScripts", m_bFirstReload);

			for each(auto listener in m_listeners)
				listener->OnReloadComplete();

			if(!m_bFirstReload && gEnv->IsEditor())
				gEnv->pFlowSystem->ReloadAllNodeTypes();

			m_bReloading = false;
			m_bDetectedChanges = false;

			return true;
		}
		break;
	case EScriptReloadResult_Retry:
		{
			m_bReloading = false;
			return Reload();
		}
	case EScriptReloadResult_Revert:
		{
			pScriptDomain->Release();
			m_pScriptDomain->SetActive();

			m_bReloading = false;

			m_bDetectedChanges = false;
			return false;
		}
		break;
	case EScriptReloadResult_Abort:
		{
			gEnv->pSystem->Quit();

			m_bReloading = false;
		}
		break;
	}

	return false;
}

#define RegisterBinding(T) m_localScriptBinds.push_back(new T());
void CScriptSystem::RegisterPriorityBindings()
{
	RegisterBinding(CScriptbind_ActorSystem);
	RegisterBinding(CScriptbind_3DEngine);
	RegisterBinding(CScriptbind_Physics);
	RegisterBinding(CScriptbind_Renderer);
	RegisterBinding(CScriptbind_Console);
	RegisterBinding(CScriptbind_GameRules);
	RegisterBinding(CScriptbind_Debug);
	RegisterBinding(CScriptbind_Time);
	RegisterBinding(CScriptbind_MaterialManager);
	RegisterBinding(CScriptbind_ParticleSystem);
	RegisterBinding(CScriptbind_ViewSystem);
	RegisterBinding(CScriptbind_LevelSystem);
	RegisterBinding(CScriptbind_Entity);
	RegisterBinding(CScriptbind_Network);
	RegisterBinding(CScriptbind_ScriptTable);
	RegisterBinding(CScriptbind_CrySerialize);
	RegisterBinding(CScriptbind_GameObject);
	RegisterBinding(CScriptbind_Sound);
	RegisterBinding(CScriptbind_CryPak);

	m_pFlowManager = new CFlowManager();
	m_pFlowManager->AddRef();
}

void CScriptSystem::RegisterSecondaryBindings()
{
	// Register what couldn't be registered earlier.
	if(m_methodBindings.size()>0)
	{
		for(TMethodBindings::iterator it = m_methodBindings.begin(); it != m_methodBindings.end(); ++it)
			RegisterMethodBinding((*it).first, (*it).second);
	}

	RegisterBinding(CScriptbind_Input);
}
#undef RegisterBinding

void CScriptSystem::EraseBinding(IMonoScriptBind *pScriptBind)
{
	if(!m_bQuitting)
		stl::find_and_erase(m_localScriptBinds, pScriptBind);
}

void CScriptSystem::OnPostUpdate(float fDeltaTime)
{
	FUNCTION_PROFILER_FAST(GetISystem(), PROFILE_SCRIPT, gEnv->bProfilerEnabled);

	IMonoArray *pArgs = CreateMonoArray(5);
	pArgs->Insert(fDeltaTime);
	pArgs->Insert(gEnv->pTimer->GetFrameStartTime().GetMilliSeconds());
	pArgs->Insert(gEnv->pTimer->GetAsyncTime().GetMilliSeconds());
	pArgs->Insert(gEnv->pTimer->GetFrameRate());
	pArgs->Insert(gEnv->pTimer->GetTimeScale());

	// Updates all scripts and sets Time.FrameTime.
	m_pScriptManagerOnUpdate->Call(m_pScriptManager->GetManagedObject(), fDeltaTime, gEnv->pTimer->GetFrameStartTime().GetMilliSeconds(), gEnv->pTimer->GetAsyncTime().GetMilliSeconds(), gEnv->pTimer->GetFrameRate(), gEnv->pTimer->GetTimeScale());
}

void CScriptSystem::CacheManagedResources()
{
	IMonoClass *pScriptManagerClass = m_pScriptManager->GetClass();

	m_pScriptManagerOnUpdate = pScriptManagerClass->GetMethod("OnUpdate", 5);
	m_pScriptManagerCreateScriptInstance = pScriptManagerClass->GetMethod("CreateScriptInstance", 5);
	m_pScriptManagerRemoveInstance = pScriptManagerClass->GetMethod("RemoveInstance", 2);

	CScriptbind_Input::CacheMethods();
	CCryScriptInstance::CacheMethods();
	CMonoFlowNode::CacheMethods();
	CEntityEventHandler::CacheMethods();
	CMonoActor::CacheMethods();
	CMonoEntityExtension::CacheMethods();
}

void CScriptSystem::OnFileChange(const char *fileName)
{
	FUNCTION_PROFILER_FAST(GetISystem(), PROFILE_SCRIPT, gEnv->bProfilerEnabled);

	if(g_pMonoCVars->mono_realtimeScriptingDetectChanges == 0)
		return;

	const char *fileExt = PathUtil::GetExt(fileName);
	if(!strcmp(fileExt, "cs") || !strcmp(fileExt, "dll"))
	{
		CryLogAlways("[CryMono] Detected change in file %s, preparing for reload..", fileName);

		if(!GetFocus())
		{
			CryLogAlways("CryENGINE did not have focus, waiting..");
			m_bDetectedChanges = true;
			return;
		}

		Reload();
	}
}

void CScriptSystem::RegisterFlownodes()
{
	if(m_pScriptManager && g_pScriptSystem->GetIGameFramework()->GetIFlowSystem())
		m_pScriptManager->CallMethod("RegisterFlownodes");
}

void CScriptSystem::RegisterMethodBinding(const void *method, const char *fullMethodName)
{
	if(!IsInitialized())
		m_methodBindings.insert(TMethodBindings::value_type(method, fullMethodName));
	else
		mono_add_internal_call(fullMethodName, method);
}

ICryScriptInstance *CScriptSystem::InstantiateScript(const char *scriptName, EMonoScriptFlags scriptFlags, IMonoArray *pConstructorParameters, bool throwOnFail)
{
	FUNCTION_PROFILER_FAST(GetISystem(), PROFILE_SCRIPT, gEnv->bProfilerEnabled);

	auto *pInstance = new CCryScriptInstance(scriptFlags);

	IMonoArray *pScriptCreationArgs = CreateMonoArray(5);
	pScriptCreationArgs->Insert(scriptName);
	pScriptCreationArgs->Insert(scriptFlags);
	pScriptCreationArgs->InsertNativePointer(pInstance);
	pScriptCreationArgs->InsertMonoObject((pConstructorParameters != nullptr ? pConstructorParameters->GetManagedObject() : nullptr));
	pScriptCreationArgs->Insert(throwOnFail);

	mono::object result = m_pScriptManagerCreateScriptInstance->InvokeArray(m_pScriptManager->GetManagedObject(), pScriptCreationArgs);
	SAFE_RELEASE(pScriptCreationArgs);

	if(!result)
		return nullptr;
	
	pInstance->SetManagedObject((MonoObject *)result, true);

	if(scriptFlags & eScriptFlag_GameRules)
	{
		IMonoClass *pGameRulesInitParamsClass = m_pCryBraryAssembly->GetClass("GameRulesInitializationParams");

		IMonoArray *pArgs = CreateMonoArray(1);

		SGameRulesInitializationParams params;
		pArgs->InsertMonoObject(pGameRulesInitParamsClass->BoxObject(&params));

		InitializeScriptInstance(pInstance, pArgs);
		SAFE_RELEASE(pArgs);
	}

	for each(auto listener in m_listeners)
		listener->OnScriptInstanceCreated(scriptName, scriptFlags, pInstance);

	return pInstance;
}

void CScriptSystem::RemoveScriptInstance(int id, EMonoScriptFlags scriptType)
{
	if(id==-1)
		return;

	m_pScriptManagerRemoveInstance->Call(m_pScriptManager->GetManagedObject(), id, scriptType);
}

mono::object CScriptSystem::InitializeScriptInstance(ICryScriptInstance *pScriptInstance, IMonoArray *pParams)
{
	FUNCTION_PROFILER_FAST(GetISystem(), PROFILE_SCRIPT, gEnv->bProfilerEnabled);

	CRY_ASSERT(pScriptInstance);
	
	mono::object result = pScriptInstance->GetClass()->GetMethod("InternalInitialize", pParams->GetSize())->InvokeArray(pScriptInstance->GetManagedObject(), pParams);

	for each(auto listener in m_listeners)
		listener->OnScriptInstanceInitialized(pScriptInstance);

	return result;
}

void CScriptSystem::ReportScriptInstanceDestroyed(ICryScriptInstance *pScriptInstance, int scriptId)
{
	for each(auto listener in m_listeners)
		listener->OnScriptInstanceReleased(pScriptInstance, scriptId);
}

IMonoClass *CScriptSystem::GetCrySerializerClass()
{
	return m_pCryBraryAssembly->GetClass("CrySerializer", "CryEngine.Serialization");
}

IMonoAssembly *CScriptSystem::GetCorlibAssembly()
{
	return m_pRootDomain->TryGetAssembly(mono_get_corlib());
}

IMonoDomain *CScriptSystem::CreateDomain(const char *name, const char *configurationFile, bool setActive)
{
	CScriptDomain *pDomain = new CScriptDomain(name, configurationFile, setActive);
	m_domains.push_back(pDomain);

	return pDomain;
}

IMonoDomain *CScriptSystem::GetActiveDomain()
{
	MonoDomain *pActiveDomain = mono_domain_get();
	if(pActiveDomain == nullptr)
		return nullptr;

	return TryGetDomain(pActiveDomain);
}

CScriptDomain *CScriptSystem::TryGetDomain(MonoDomain *pMonoDomain)
{
	CRY_ASSERT(pMonoDomain);

	for each(auto domain in m_domains)
	{
		if(domain->GetMonoDomain() == pMonoDomain)
			return domain;
	}

	CScriptDomain *pDomain = new CScriptDomain(pMonoDomain);
	m_domains.push_back(pDomain);

	return pDomain;
}

void CScriptSystem::OnDomainReleased(CScriptDomain *pDomain)
{
	if(!m_bQuitting)
		stl::find_and_erase(m_domains, pDomain);
}
