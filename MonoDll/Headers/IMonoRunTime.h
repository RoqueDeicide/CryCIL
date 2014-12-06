//////////////////////////////////////////////////////////////////////////
// IMonoRunTime interface for external projects, i.e. CryGame.
//////////////////////////////////////////////////////////////////////////
// 20/11/2011 : Created by Igor 'RoqueDeicide' Rock
// (Based on version by i59 which is based on version by ins\)
////////////////////////////////////////////////////////////////////////*/
#pragma once

// uncomment if Plugin SDK should be used
//#define PLUGIN_SDK
#ifdef PLUGIN_SDK
#include <IPluginManager.h>
#endif

#if defined(LINUX)
#define CRYMONO_LIBRARY "CryMono.so"
#elif defined(APPLE)
#define CRYMONO_LIBRARY "CryMono.dylib"
#else
#define CRYMONO_LIBRARY "CryMono.dll"
#endif

namespace mono
{
	class _string; typedef _string *string;
	class _object; typedef _object *object;

	struct entityId
	{
		entityId() : id(0) {}
		entityId(EntityId Id) : id(Id) {}

		EntityId id;
	};
};

struct IMonoScriptManager;

struct IMonoObject;
struct IMonoArray;
struct IMonoClass;
struct IMonoAssembly;
struct IMonoDomain;

struct IMonoEntityManager;
struct IMonoScriptEventListener;

struct ICryScriptInstance;

struct IGameFramework;
struct ISystem;

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

struct IMonoRunTime
{
	/*!
	* Initialization method that is used when Plugin SDK is not used.
	*/
	typedef IMonoRunTime *(*TEntryFunction)(ISystem* pSystem, IGameFramework *pGameFramework);
	static IMonoRunTime *instance;	///< Since there is only one Mono run-time, interface can be represented by a single object.
};