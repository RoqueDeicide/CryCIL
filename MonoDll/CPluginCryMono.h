#ifndef ___CRYMONO_PLUGINSDK_INTEGRATION_H__
#define ___CRYMONO_PLUGINSDK_INTEGRATION_H__
#include "MonoScriptSystem.h"

#ifdef PLUGIN_SDK

#include <IPluginManager.h>
#include <IPluginBase.h>
#include <CPluginBaseMinimal.hpp>

#define PLUGIN_NAME "CryMono"
#define CRYMONO_VERSION "0.7-dev"

namespace CryMonoPlugin
{
    /**
    * @brief Provides information and manages the resources of this plugin.
    */
    class CPluginCryMono :
        public PluginManager::CPluginBaseMinimal
    {
        public:
            CPluginCryMono();
            ~CPluginCryMono();

            // IPluginBase
            bool Release( bool bForce = false );

            int GetInitializationMode() const
            {
                return int( PluginManager::IM_BeforeFramework );
            };

            bool Init( SSystemGlobalEnvironment& env, SSystemInitParams& startupParams, IPluginBase* pPluginManager, const char* sPluginDirectory );

            bool RegisterTypes( int nFactoryType, bool bUnregister );

            const char* GetVersion() const
            {
                return CRYMONO_VERSION;
            };

            const char* GetName() const
            {
                return PLUGIN_NAME;
            };

            const char* GetCategory() const
            {
                return "Framework";
            };

            const char* ListAuthors() const
            {
                return "Filip \"i59\" Lundgren,\nins,\nRuan";
            };

            const char* GetStatus() const
            {
                return "Undefined";
            }

            const char* GetCurrentConcreteInterfaceVersion() const
            {
                return CRYMONO_VERSION;
            };

            void* GetConcreteInterface( const char* sInterfaceVersion )
            {
                if( !sInterfaceVersion || strcmp(sInterfaceVersion, CRYMONO_VERSION) == 0 )
                {
                    return GetMonoScriptSystem();
                }
                else
                {
                    return NULL;
                }
            };
    };

    extern CPluginCryMono* gPlugin;
}

#endif // PLUGIN_SDK
#endif // ___CRYMONO_PLUGINSDK_INTEGRATION_H__