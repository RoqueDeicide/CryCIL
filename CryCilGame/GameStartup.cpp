#include "stdafx.h"

#include "Game.h"
#include "GameStartup.h"
#include <platform_impl.h>
#include <CryLibrary.h>
#include <IHardwareMouse.h>
#include <CryCilHeader.h>

// If we are using static linking than the Dll will be loaded by the OS-specific loader.
#if defined(_LIB)
extern "C" IGameFramework *CreateGameFramework();
#endif

#define DLL_INITFUNC_CREATEGAME "CreateGameFramework"

void CryCilGameErrorObserver::OnAssert(const char *, const char *, const char *, unsigned int)
{
}

void CryCilGameErrorObserver::OnFatalError(const char* message)
{
	CryLogAlways("---FATAL ERROR: message:%s", message);

#if defined(WIN32) || defined (WIN64)
	gEnv->pSystem->debug_LogCallStack();
	CryLogAlways("----------------------------------------");
#endif
}

IGameStartup *CryCilGameShell::Create()
{
	static char buff[sizeof(CryCilGameShell)];
	return new (buff)CryCilGameShell();
}

CryCilGameShell::CryCilGameShell()
	: framework(nullptr)
	, frameworkDll(nullptr)
	, game(nullptr)
	, gameReference(&game)
	, cryCilDll(nullptr)
	, fullscreenCVarSetup(false)
{

}

CryCilGameShell::~CryCilGameShell()
{
	// Shut down the game.
	if (this->game)
	{
		this->game->Shutdown();
		this->game = nullptr;
	}
	// Shut down CryCIL.
	if (MonoEnv)
	{
		MonoEnv->Shutdown();
		MonoEnv = nullptr;
	}
	// Shut down game framework.
	if (this->framework)
	{
		this->framework->Shutdown();
		this->framework = nullptr;
	}
}

IGameRef CryCilGameShell::Init(SSystemInitParams &startupParams)
{
	// Initialize the game framework.
	if (!this->InitializeGameFramework(startupParams))
	{
		return nullptr;
	}

	startupParams.pSystem = GetISystem();

	// Measure how long it takes to finish this function.
	LOADING_TIME_PROFILE_SECTION;

	// Initialize CryCIL.
	List<IMonoSystemListener *> listeners(10);

	// Add your listeners here...

	this->cryCilDll = InitializeCryCIL(this->framework, &listeners, true);

	// Initialize the game.
	static char gameBuffer[sizeof(CryCilGame)];
	this->game = new (static_cast<void*>(gameBuffer))CryCilGame();

	if (!this->game->Init(this->framework))
	{
		CryFatalError("Unable to initialize the game.");
		return nullptr;
	}

	if (!this->framework->CompleteInit())
	{
		this->game->Shutdown();
		return nullptr;
	}

	if (startupParams.bExecuteCommandLine)
	{
		GetISystem()->ExecuteCommandLine();
	}

	GetISystem()->GetISystemEventDispatcher()->RegisterListener(this);
	GetISystem()->RegisterErrorObserver(&this->errorObs);

	return this->gameReference;
}

bool CryCilGameShell::InitializeGameFramework(SSystemInitParams& params)
{
	MEMSTAT_CONTEXT(EMemStatContextTypes::MSC_Other, 0, "Game framwork initialization");

#ifndef _LIB

	// Loading the CryAction library.
	this->frameworkDll = GetFrameworkDLL(params.szBinariesDir);
	if (!this->frameworkDll)
	{
		CryFatalError("Failed to load %s", GAME_FRAMEWORK_FILENAME);
		return false;
	}

	IGameFramework::TEntryFunction CreateGameFramework =
		IGameFramework::TEntryFunction(CryGetProcAddress(this->frameworkDll, DLL_INITFUNC_CREATEGAME));

	if (!CreateGameFramework)
	{
		CryFatalError("%s is not a valid dll: It doesn't export %s function.", GAME_FRAMEWORK_FILENAME,
					  DLL_INITFUNC_CREATEGAME);
		return false;
	}

#endif // _LIB

	// Initializing the game framework.

	this->framework = CreateGameFramework();

	if (!this->framework)
	{
		CryFatalError("Failed to create the GameFramework Interface!");
		return false;
	}

	if (!this->framework->Init(params))
	{
		CryFatalError("Failed to initialize CryENGINE!");
		return false;
	}

	ModuleInitISystem(this->framework->GetISystem(), "CryCilGame");

#ifdef WIN32
	// Allow to get to this object through the user-data pointer that is available for all windows.
	SetWindowLongPtr(reinterpret_cast<HWND>(gEnv->pRenderer->GetHWND()), GWLP_USERDATA, reinterpret_cast<LONG_PTR>(this));
#endif // WIN32

	return true;
}

void CryCilGameShell::FullScreenCVarChanged(ICVar* pVar)
{
	if (GetISystem()->GetISystemEventDispatcher())
	{
		GetISystem()->GetISystemEventDispatcher()->OnSystemEvent(ESYSTEM_EVENT_TOGGLE_FULLSCREEN,
																 pVar->GetIVal(), 0);
	}
}

void CryCilGameShell::Shutdown()
{
	this->~CryCilGameShell();
}

int CryCilGameShell::Update(bool haveFocus, unsigned int updateFlags)
{
#ifdef JOBMANAGER_SUPPORT_PROFILING
	gEnv->GetJobManager()->SetFrameStartTime(gEnv->pTimer->GetAsyncTime());
#endif // JOBMANAGER_SUPPORT_PROFILING

	int returnCode = 0;

	if (gEnv->pConsole)
	{
#ifdef WIN32
		if (gEnv && gEnv->pRenderer && gEnv->pRenderer->GetHWND())
		{
			// Are we in focus?
			bool focus = (::GetFocus() == gEnv->pRenderer->GetHWND());
			static bool focused = focus;
			// Were we in focus?
			if (focused != focus)
			{
				auto dispatcher = GetISystem()->GetISystemEventDispatcher();
				if (dispatcher)
				{
					dispatcher->OnSystemEvent(ESYSTEM_EVENT_CHANGE_FOCUS, focus, 0);
				}
				focused = focus;
			}
		}
#endif // WIN32
	}

	if (this->game)
	{
		returnCode = this->game->Update(haveFocus, updateFlags);
	}

	if (!this->fullscreenCVarSetup && gEnv->pConsole)
	{
		ICVar *fullscreenCvar = gEnv->pConsole->GetCVar("r_Fullscreen");
		if (fullscreenCvar)
		{
			// Install a function that will raise a system event that informs everyone about the changes to the full-screen
			// mode, because there is not other way to do this.
			fullscreenCvar->SetOnChangeCallback(FullScreenCVarChanged);
			this->fullscreenCVarSetup = true;
		}
	}

	return returnCode;
}

bool CryCilGameShell::GetRestartLevel(char **levelName)
{
	bool relaunch = GetISystem()->IsRelaunch();
	if (relaunch)
	{
		*levelName = const_cast<char *>(gEnv->pGame->GetIGameFramework()->GetLevelName());
	}
	return relaunch;
}

const char *CryCilGameShell::GetPatch() const
{
	return nullptr;
}

bool CryCilGameShell::GetRestartMod(char *, int)
{
	return false;
}

int CryCilGameShell::Run(const char *autoStartLevelName)
{
	// Execute the autoexec.cfg file.
	gEnv->pConsole->ExecuteString("exec autoexec.cfg");

	if (autoStartLevelName)
	{
		//load the save-game, if this is a save-game file.
		if (CryStringUtils::stristr(autoStartLevelName, CRY_SAVEGAME_FILE_EXT) != nullptr)
		{
			CryFixedStringT<256> fileName(autoStartLevelName);
			// NOTE! two step trimming is intended!
			fileName.Trim(" ");  // first:  remove enclosing spaces (outside ")
			fileName.Trim("\""); // second: remove potential enclosing "
			gEnv->pGame->GetIGameFramework()->LoadGame(fileName.c_str());
		}
		else	//start specified level
		{
			CryFixedStringT<256> mapCmd("map ");
			mapCmd += autoStartLevelName;
			gEnv->pConsole->ExecuteString(mapCmd.c_str());
		}
	}
	else
	{
		// Load GameZero map.
		gEnv->pConsole->ExecuteString("map example");
	}

	// I have no idea why IHardwareMouse interface has to be retrieved via ISystem implementation in Windows.
#ifdef WIN32
	if (!(gEnv && gEnv->pSystem) || (!gEnv->IsEditor() && !gEnv->IsDedicated()))
	{
		::ShowCursor(FALSE);
		if (GetISystem()->GetIHardwareMouse())
			GetISystem()->GetIHardwareMouse()->DecrementCounter();
	}
#else
	if (gEnv && gEnv->pHardwareMouse)
		gEnv->pHardwareMouse->DecrementCounter();
#endif

	while (this->Update(true, 0) != 0) {}

	return 0;
}

void CryCilGameShell::OnSystemEvent(ESystemEvent _event, UINT_PTR wparam, UINT_PTR)
{
	switch (_event)
	{
	case ESYSTEM_EVENT_RANDOM_SEED:
		// Modifies the global value that is used as a seed for random number generation using the wparam as the value.
		cry_random_seed(gEnv->bNoRandomSeed ? 0 : uint32(wparam));
		break;
// 	case ESYSTEM_EVENT_RANDOM_ENABLE:
// 		break;
// 	case ESYSTEM_EVENT_RANDOM_DISABLE:
// 		break;
// 	case ESYSTEM_EVENT_CHANGE_FOCUS:
// 		break;
// 	case ESYSTEM_EVENT_MOVE:
// 		break;
// 	case ESYSTEM_EVENT_RESIZE:
// 		break;
// 	case ESYSTEM_EVENT_ACTIVATE:
// 		break;
// 	case ESYSTEM_EVENT_POS_CHANGED:
// 		break;
// 	case ESYSTEM_EVENT_STYLE_CHANGED:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_START_PRELOADINGSCREEN:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_RESUME_GAME:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_PREPARE:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_START_LOADINGSCREEN:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_LOADINGSCREEN_ACTIVE:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_START:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_END:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_LOAD_ERROR:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_NOT_READY:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_PRECACHE_START:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_PRECACHE_FIRST_FRAME:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_GAMEPLAY_START:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_UNLOAD:
// 		break;
	case ESYSTEM_EVENT_LEVEL_POST_UNLOAD:
		// Whatever this does. Probably releases some resources or something.
		STLALLOCATOR_CLEANUP;
		break;
// 	case ESYSTEM_EVENT_GAME_POST_INIT:
// 		break;
// 	case ESYSTEM_EVENT_GAME_POST_INIT_DONE:
// 		break;
// 	case ESYSTEM_EVENT_FULL_SHUTDOWN:
// 		break;
	case ESYSTEM_EVENT_FAST_SHUTDOWN:
		break;
// 	case ESYSTEM_EVENT_LANGUAGE_CHANGE:
// 		break;
// 	case ESYSTEM_EVENT_TOGGLE_FULLSCREEN:
// 		break;
// 	case ESYSTEM_EVENT_SHARE_SHADER_COMBINATIONS:
// 		break;
// 	case ESYSTEM_EVENT_3D_POST_RENDERING_START:
// 		break;
// 	case ESYSTEM_EVENT_3D_POST_RENDERING_END:
// 		break;
// 	case ESYSTEM_EVENT_SWITCHING_TO_LEVEL_HEAP:
// 		break;
// 	case ESYSTEM_EVENT_SWITCHED_TO_LEVEL_HEAP:
// 		break;
// 	case ESYSTEM_EVENT_SWITCHING_TO_GLOBAL_HEAP:
// 		break;
// 	case ESYSTEM_EVENT_SWITCHED_TO_GLOBAL_HEAP:
// 		break;
// 	case ESYSTEM_EVENT_LEVEL_PRECACHE_END:
// 		break;
// 	case ESYSTEM_EVENT_GAME_MODE_SWITCH_START:
// 		break;
// 	case ESYSTEM_EVENT_GAME_MODE_SWITCH_END:
// 		break;
// 	case ESYSTEM_EVENT_VIDEO:
// 		break;
// 	case ESYSTEM_EVENT_GAME_PAUSED:
// 		break;
// 	case ESYSTEM_EVENT_GAME_RESUMED:
// 		break;
// 	case ESYSTEM_EVENT_TIME_OF_DAY_SET:
// 		break;
// 	case ESYSTEM_EVENT_EDITOR_ON_INIT:
// 		break;
// 	case ESYSTEM_EVENT_FRONTEND_INITIALISED:
// 		break;
// 	case ESYSTEM_EVENT_EDITOR_GAME_MODE_CHANGED:
// 		break;
// 	case ESYSTEM_EVENT_EDITOR_SIMULATION_MODE_CHANGED:
// 		break;
// 	case ESYSTEM_EVENT_FRONTEND_RELOADED:
// 		break;
// 	case ESYSTEM_EVENT_SW_FORCE_LOAD_START:
// 		break;
// 	case ESYSTEM_EVENT_SW_FORCE_LOAD_END:
// 		break;
// 	case ESYSTEM_EVENT_SW_SHIFT_WORLD:
// 		break;
// 	case ESYSTEM_EVENT_STREAMING_INSTALL_ERROR:
// 		break;
// 	case ESYSTEM_EVENT_ONLINE_SERVICES_INITIALISED:
// 		break;
// 	case ESYSTEM_EVENT_AUDIO_IMPLEMENTATION_LOADED:
// 		break;
// 	case ESYSTEM_EVENT_USER:
// 		break;
// 	case ESYSTEM_BEAM_PLAYER_TO_CAMERA_POS:
// 		break;
	default:
		break;
	}
}