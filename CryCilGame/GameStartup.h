#pragma once

#include <CryGame/IGameFramework.h>

#if CRY_PLATFORM_WINDOWS
  #include <CryCore/Platform/CryWindows.h>
#endif // CRY_PLATFORM_WINDOWS

#define GAME_FRAMEWORK_FILENAME CryLibraryDefName("CryAction")
#define GAME_WINDOW_CLASSNAME "CRYENGINE"

extern HMODULE GetFrameworkDLL(const char *dllLocalDir);

//! This is a game-specific error observation class.
class CryCilGameErrorObserver : public IErrorObserver
{
public:
	//! Currently does nothing.
	virtual void OnAssert(const char *condition, const char *message, const char *fileName, unsigned int fileLineNumber) override;
	//! Prints the message along with a call stack (the latter is printed only on Windows system).
	//!
	//! @param message Text message that describes the error.
	virtual void OnFatalError(const char *message) override;
};

//! An object of this class represents an application shell around the game: it handle the case of the game starting,
//! running the main loop and shutting down.
class CryCilGameShell : public IGameStartup, public ISystemEventListener
{
	CryCilGameErrorObserver errorObs;
	IGameFramework         *framework;
	HMODULE                 frameworkDll;
	IGame                  *game;
	IGameRef                gameReference;
	HMODULE                 cryCilDll;
	bool                    fullscreenCVarSetup; //!< Indicates whether we've setup a console variable for a full-screen mode.

public:
	CryCilGameShell();
	~CryCilGameShell();
	//! Creates an object of this type using static memory allocation.
	static IGameStartup *Create();
	//! This method is invoked by the CrySystem object to initialize the game.
	//!
	//! @param startupParams A reference to the object that encapsulate the set of parameters that specify how the system
	//!                      was initialized.
	//!
	//! @returns A reference to the implementation of the IGame interface that represents the game itself.
	virtual IGameRef Init(SSystemInitParams &startupParams) override;
	//! Shuts down the game.
	virtual void Shutdown() override;
	//! Updates the state of everything within the game, invoked on every frame of the main game loop.
	//!
	//! @param haveFocus   Indicates whether this game has an input focus.
	//! @param updateFlags A set of flags that specifies the update.
	//!
	//! @returns 0 to terminate the game (i.e. when quitting), or any other value to continue.
	virtual int Update(bool haveFocus, unsigned int updateFlags) override;
	//! Indicates whether the game is restarting and gets the name of the level.
	//!
	//! Whoever wrote documentation for the abstract version of this method had too much caffeine, so it's not actually
	//! clear what it does.
	//!
	//! @param levelName When this method concludes this parameter will be pointing at the null-terminated string that
	//!                  represents the name of current level, if this game is restarting, otherwise the argument is left
	//!                  unchanged.
	//!
	//! @returns True, if the system is relaunching.
	virtual bool GetRestartLevel(char **levelName) override;
	//! No idea what the fuck this method is supposed to do.
	virtual const char *GetPatch() const override;
	//! This method has no purpose in CryCIL.
	virtual bool GetRestartMod(char *pModNameBuffer, int modNameBufferSizeInBytes) override;
	//! Starts and runs the main game loop.
	//!
	//! @param autoStartLevelName Name of the level to start, used when the game has been restarted (e.g. to apply new game
	//!                           settings).
	virtual int Run(const char *autoStartLevelName) override;

	//! Processes the system event.
	virtual void OnSystemEvent(ESystemEvent _event, UINT_PTR wparam, UINT_PTR lparam) override;
private:
	// Initializes the game framework system.
	//
	// @returns True, if initialization was successful, otherwise false.
	bool InitializeGameFramework(SSystemInitParams &params);
	// Raises ESYSTEM_EVENT_TOGGLE_FULLSCREEN event.
	static void FullScreenCVarChanged(ICVar *pVar);
};