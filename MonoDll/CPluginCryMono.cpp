#include <StdAfx.h>
#include <CPluginCryMono.h>

#ifdef PLUGIN_SDK
namespace CryMonoPlugin
{
	CPluginCryMono* gPlugin = NULL;

	CPluginCryMono::CPluginCryMono()
		: m_pScriptSystem(nullptr)
	{
		gPlugin = this;
	}

	CPluginCryMono::~CPluginCryMono()
	{
		Release(true);

		gPlugin = NULL;
	}

	bool CPluginCryMono::Release(bool bForce)
	{
		bool bRet = true;
		bool bWasInitialized = m_bIsFullyInitialized; // Will be reset by base class so backup

		if (!m_bCanUnload)
		{
			// Note: Type Unregistration will be automatically done by the Base class (Through RegisterTypes)
			// Should be called while Game is still active otherwise there might be leaks/problems
			bRet = CPluginBaseMinimal::Release(bForce);

			if (bRet)
			{
				// Cleanup like this always (since the class is static its cleaned up when the dll is unloaded)
				gPluginManager->UnloadPlugin(GetName());

				// Allow Plugin Manager garbage collector to unload this plugin
				AllowDllUnload();
			}
		}

		return bRet;
	};

	bool CPluginCryMono::Init(SSystemGlobalEnvironment& env, SSystemInitParams& startupParams, IPluginBase* pPluginManager, const char* sPluginDirectory)
	{
		gPluginManager = (PluginManager::IPluginManager*)pPluginManager->GetConcreteInterface(NULL);
		CPluginBaseMinimal::Init(env, startupParams, pPluginManager, sPluginDirectory);

		m_pScriptSystem = new CScriptSystem(gEnv->pGame->GetIGameFramework());

		return m_pScriptSystem != nullptr;
	}

	bool CPluginCryMono::RegisterTypes(int nFactoryType, bool bUnregister)
	{
		bool bRet = CPluginBaseMinimal::RegisterTypes(nFactoryType, bUnregister);

		using namespace PluginManager;
		eFactoryType enFactoryType = eFactoryType(nFactoryType);

		if (bRet)
		{
			if (gEnv && gEnv->pSystem && !gEnv->pSystem->IsQuitting())
			{
				// Flownodes
				//if(enFactoryType == FT_Flownode)
				//GetMonoRunTime()->RegisterFlownodes();
			}
		}

		return bRet;
	}
}

#endif // #ifdef PLUGIN_SDK