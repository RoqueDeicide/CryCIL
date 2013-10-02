/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IMonoScriptSystem interface for external projects, i.e. CryGame.
//////////////////////////////////////////////////////////////////////////
// 20/11/2011 : Created by Filip 'i59' Lundgren (Based on version by ins\)
////////////////////////////////////////////////////////////////////////*/
#ifndef __I_MONO_SCRIPT_SYSTEM_H__
#define __I_MONO_SCRIPT_SYSTEM_H__

// define if Plugin SDK should be used
//#define PLUGIN_SDK
#ifdef PLUGIN_SDK
#include <IPluginManager.h>
#endif

struct IMonoScriptManager;

struct IMonoObject;
struct IMonoArray;
struct IMonoClass;
struct IMonoAssembly;
struct IMonoDomain;

struct IMonoEntityManager;
struct IMonoConverter;
struct IMonoScriptEventListener;

struct ICryScriptInstance;

struct IGameFramework;

/// <summary>
/// Script flags are passed to IMonoScriptSystem::InstantiateScript and RemoveScriptInstance as a way to identify scripts more effectively, and to solve the issue with scripts being of multiple types.
/// </summary>
enum EMonoScriptFlags
{
	/// <summary>
	/// Scripts not inheriting from CryScriptInstance will utilize this script type.
	/// </summary>
	eScriptFlag_Any = BIT(0),
	/// <summary>
	/// Scripts inheriting from CryScriptInstance, but no other CryMono base script will be linked to this script type.
	/// </summary>
	eScriptFlag_CryScriptInstance = BIT(1),
	/// <summary>
	/// Scripts inheriting from GameRules will utilize this script type.
	/// </summary>
	eScriptFlag_GameRules = BIT(2),
	/// <summary>
	/// Scripts inheriting from FlowNode will utilize this script type.
	/// </summary>
	eScriptFlag_FlowNode = BIT(3),
	/// <summary>
	/// Scripts inheriting from Entity will utilize this script type.
	/// </summary>
	eScriptFlag_Entity = BIT(4),
	/// <summary>
	/// Scripts inheriting from Actor will utilize this script type.
	/// </summary>
	eScriptFlag_Actor = BIT(5),
	/// <summary>
	/// Scripts inheriting from AIActor will utilize this script type.
	/// </summary>
	eScriptFlag_AIActor = BIT(6),
	/// <summary>
	/// Scripts inheriting from EntityFlowNode will utilize this script type.
	/// </summary>
	eScriptFlag_EntityFlowNode = BIT(7),
};

/// <summary>
/// The main module in CryMono; initializes mono domain and handles calls to C# scripts.
/// </summary>
struct IMonoScriptSystem
{
	/// <summary>
	/// Returns true when the root domain has been initialized.
	/// </summary>
	virtual bool IsInitialized() = 0;

	/// <summary>
	/// Used to start script recompilation / serialization.
	/// </summary>
	/// <returns>True if successful, False if aborted.</returns>
	virtual bool Reload() = 0;

	virtual void AddListener(IMonoScriptEventListener *pListener) = 0;
	virtual void RemoveListener(IMonoScriptEventListener *pListener) = 0;

	/// <summary>
	/// Deletes script system instance; cleans up mono objects etc.
	/// Called from the dll which implements CryMono on engine shutdown (CGameStartup destructor within the sample project)
	/// </summary>
	virtual void Release() = 0;

	/// <summary>
	/// Registers a method binding, called from IMonoScriptBind.
	/// </summary>
	/// <param name="fullMethodName">i.e. "CryEngine.GameRulesSystem::GetPlayerId"</param>
	virtual void RegisterMethodBinding(const void *method, const char *fullMethodName) = 0;

	/// <summary>
	/// Instantiates a script (with constructor parameters if supplied) of type and name
	/// This assumes that the script was present in a .dll in Plugins or within a .cs file when PostInit was called.
	//  Note: ICryScriptInstance can also be casted to IMonoObject.
	/// </summary>
	virtual ICryScriptInstance *InstantiateScript(const char *scriptName, EMonoScriptFlags scriptType = eScriptFlag_Any, IMonoArray *pConstructorParameters = nullptr, bool throwOnFail = true) = 0;
	/// <summary>
	/// Removes and destructs an instantiated script with the supplied id if found.
	/// </summary>
	virtual void RemoveScriptInstance(int id, EMonoScriptFlags scriptType = eScriptFlag_Any) = 0;

	virtual IMonoObject *GetScriptManager() = 0;

	/// <summary>
	/// Gets the CryEngine.Initialization.CrySerializer class and returns it.
	/// </summary>
	virtual IMonoClass *GetCrySerializerClass()= 0;

	/// <summary>
	/// Gets a pointer to the CryBrary assembly containing all default managed CryMono types.
	/// </summary>
	virtual IMonoAssembly *GetCryBraryAssembly() = 0;

	/// <summary>
	/// Gets the core assembly, containing the System namespace etc.
	/// </summary>
	virtual IMonoAssembly *GetCorlibAssembly() = 0;

	/// <summary>
	/// Gets the root domain created on script system initialization.
	/// </summary>
	virtual IMonoDomain *GetRootDomain() = 0;

	/// <summary>
	/// Creates a new app domain.
	/// </summary>
	virtual IMonoDomain *CreateDomain(const char *name, const char *configurationFile = nullptr, bool setActive = false) = 0;

	/// <summary>
	/// Gets the currently active app domain.
	/// </summary>
	virtual IMonoDomain *GetActiveDomain() = 0;

	/// <summary>
	/// Gets the domain in which scripts are stored and executed.
	/// </summary>
	virtual IMonoDomain *GetScriptDomain() = 0;

	/// <summary>
	/// Retrieves an instance of the IMonoConverter; a class used to easily convert C# types to C++ and the other way around.
	/// </summary>
	virtual IMonoConverter *GetConverter() = 0;

	/// <summary>
	/// Call from IGame::RegisterGameFlownodes in order to have CryMono flow nodes appear in the Flowgraph Editor.
	/// </summary>
	virtual void RegisterFlownodes() = 0;

#ifndef PLUGIN_SDK
	/// <summary>
	/// Entry point of the dll, used to set up CryMono when not using the Plugin SDK.
	/// </summary>
	typedef IMonoScriptSystem *(*TEntryFunction)(ISystem* pSystem, IGameFramework *pGameFramework);
#endif
};

struct IMonoScriptEventListener
{
	virtual void OnReloadStart() = 0;
	virtual void OnReloadComplete() = 0;

	virtual void OnScriptInstanceCreated(const char *scriptName, EMonoScriptFlags scriptType, ICryScriptInstance *pScriptInstance) = 0;
	virtual void OnScriptInstanceInitialized(ICryScriptInstance *pScriptInstance) = 0;
	virtual void OnScriptInstanceReleased(ICryScriptInstance *pScriptInstance, int scriptId) = 0;

	/// <summary>
	/// Called when the script system commences shutting down
	/// </summary>
	virtual void OnShutdown() = 0;
};

#ifdef PLUGIN_SDK
static IMonoScriptSystem *_pMonoScriptSystem = nullptr; // internal storage to avoid having the extra overhead from having to call GetPluginByName all the time.

static IMonoScriptSystem *GetMonoScriptSystem()
{
	if(_pMonoScriptSystem == nullptr)
		_pMonoScriptSystem = static_cast<IMonoScriptSystem *>(gPluginManager->GetPluginByName("CryMono")->GetConcreteInterface());

	return _pMonoScriptSystem;
}
#else
static IMonoScriptSystem *GetMonoScriptSystem()
{
	return gEnv->pMonoScriptSystem;
}
#endif

#endif //__I_MONO_SCRIPT_SYSTEM_H__