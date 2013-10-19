/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Main module; handles all sub-systems within the CryMono dll.
//////////////////////////////////////////////////////////////////////////
// 20/11/2011 : Created by Filip 'i59' Lundgren (Based on version by ins\)
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_H__
#define __MONO_H__

#include <IMonoScriptSystem.h>
#include <IMonoDomain.h>
#include <IMonoScriptbind.h>

#include <MonoCommon.h>

#include <IFileChangeMonitor.h>
#include <IGameFramework.h>
#include <IEntitySystem.h>

struct IMonoScriptManager;
struct IMonoEntityManager;

struct SCVars;

class CFlowManager;
class CInput;

class CScriptDomain;
class CScriptObject;

enum EScriptReloadResult
{
	EScriptReloadResult_Abort = 0,
	EScriptReloadResult_Success,
	EScriptReloadResult_Retry,
	EScriptReloadResult_Revert
};

class CScriptSystem
	: public IMonoScriptSystem
	, public IFileChangeListener
	, public IGameFrameworkListener
{
	typedef std::map<const void *, const char *> TMethodBindings;

public:
	CScriptSystem(IGameFramework *pGameFramework);
	~CScriptSystem();

	// IMonoScriptSystem
	virtual bool IsInitialized() override { return m_pRootDomain != nullptr; }

	virtual bool Reload() override;

	virtual void AddListener(IMonoScriptEventListener *pListener) override { m_listeners.push_back(pListener); }
	virtual void RemoveListener(IMonoScriptEventListener *pListener) override { if(!m_bQuitting) stl::find_and_erase(m_listeners, pListener); }

	virtual void Release() override { delete this; }

	virtual void RegisterMethodBinding(const void *method, const char *fullMethodName) override;

	virtual ICryScriptInstance *InstantiateScript(const char *scriptName, EMonoScriptFlags scriptType = eScriptFlag_Any, IMonoArray *pConstructorParameters = nullptr, bool throwOnFail = true) override;
	virtual void RemoveScriptInstance(int id, EMonoScriptFlags scriptType = eScriptFlag_Any) override;
	
	virtual IMonoObject *GetScriptManager() override { return m_pScriptManager; }

	virtual IMonoClass *GetCrySerializerClass() override;

	virtual IMonoAssembly *GetCryBraryAssembly() override { return m_pCryBraryAssembly; }
	virtual IMonoAssembly *GetCorlibAssembly() override;

	virtual IMonoDomain *GetRootDomain() override { return (IMonoDomain *)m_pRootDomain; }
	virtual IMonoDomain *CreateDomain(const char *name, const char *configurationFile = nullptr, bool setActive = false);
	virtual IMonoDomain *GetActiveDomain() override;
	virtual IMonoDomain *GetScriptDomain() { return m_pScriptDomain; }

	virtual IMonoConverter *GetConverter() override { return m_pConverter; }

	virtual void RegisterFlownodes() override;
	// ~IMonoScriptSystem

	// IFileChangeMonitor
	virtual void OnFileChange(const char *fileName);
	// ~IFileChangeMonitor

	// ~IGameFrameworkListener	  	
	virtual void OnPostUpdate(float fDeltaTime);
	virtual void OnSaveGame(ISaveGame* pSaveGame) {} 	
	virtual void OnLoadGame(ILoadGame* pLoadGame) {}  	
	virtual void OnLevelEnd(const char* nextLevel) {}  	
	virtual void OnActionEvent(const SActionEvent& event) {} 	
	// ~IGameFrameworkListener

	CScriptDomain *TryGetDomain(MonoDomain *pDomain);
	void OnDomainReleased(CScriptDomain *pDomain);

	IMonoAssembly *GetDebugDatabaseCreator() { return m_pPdb2MdbAssembly; }

	CFlowManager *GetFlowManager() const { return m_pFlowManager; }

	void EraseBinding(IMonoScriptBind *pScriptBind);

	mono::object InitializeScriptInstance(ICryScriptInstance *pScriptInstance, IMonoArray *pParams); 
	void ReportScriptInstanceDestroyed(ICryScriptInstance *pScriptInstance, int scriptId);

	bool DetectedChanges() { return m_bDetectedChanges; }

	IGameFramework *GetIGameFramework() { return m_pGameFramework; }

protected:
	bool CompleteInit();

	void RegisterPriorityBindings();
	void RegisterSecondaryBindings();

	void CacheManagedResources();

	// The primary app domain, not really used for anything besides holding the script domain. Do *not* unload this at runtime, we cannot execute another root domain again without restarting.
	CScriptDomain *m_pRootDomain;
	std::vector<CScriptDomain *> m_domains;

	IMonoDomain *m_pScriptDomain;

	IMonoObject *m_pScriptManager;

	bool m_bFirstReload;
	bool m_bReloading;
	bool m_bDetectedChanges;

	bool m_bQuitting;

	CFlowManager *m_pFlowManager;

	IMonoConverter *m_pConverter;

	IMonoAssembly *m_pCryBraryAssembly;
	IMonoAssembly *m_pPdb2MdbAssembly;

	SCVars *m_pCVars;

	IGameFramework *m_pGameFramework;

	// We temporarily store scriptbind methods here if developers attempt to register them prior to the script system has been initialized properly.
	TMethodBindings m_methodBindings;

	std::vector<IMonoScriptBind *> m_localScriptBinds;

	std::vector<IMonoScriptEventListener *> m_listeners;
};

struct SGameRulesInitializationParams
{
	SGameRulesInitializationParams()
		: id(1)
	{
		pEntity = gEnv->pEntitySystem->GetEntity(id);
	}

	EntityId id;
	IEntity *pEntity;
};

extern CScriptSystem *g_pScriptSystem;

#endif //__MONO_H__