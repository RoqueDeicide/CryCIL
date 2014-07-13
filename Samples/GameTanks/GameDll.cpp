/*************************************************************************
	Crytek Source File.
	Copyright (C), Crytek Studios, 2001-2004.
	-------------------------------------------------------------------------
	$Id$
	$DateTime$
	Description: Game DLL entry point.

	-------------------------------------------------------------------------
	History:
	- 2:8:2004   10:38 : Created by Marcio Martins

*************************************************************************/
#include "StdAfx.h"
#include "Game.h"
#include "GameStartup.h"
#include "EditorGame.h"

#include <CryLibrary.h>

extern "C"
{
	GAME_API IGame *CreateGame(IGameFramework* pGameFramework)
	{
		ModuleInitISystem(pGameFramework->GetISystem(),"CryGame");

		static char pGameBuffer[sizeof(CGame)];
		return new ((void*)pGameBuffer) CGame();
	}

	GAME_API IGameStartup *CreateGameStartup()
	{
		// at this point... we have no dynamic memory allocation, and we cannot
		// rely on atexit() doing the right thing; the only recourse is to
		// have a static buffer that we use for this object
		static char gameStartup_buffer[sizeof(CGameStartup)];
		return new ((void*)gameStartup_buffer) CGameStartup();
	}
	GAME_API IEditorGame *CreateEditorGame()
	{
		return new CEditorGame();
	}
}


/*
 * this section makes sure that the framework dll is loaded and cleaned up
 * at the appropriate time
 */

#if !defined(_LIB) && !defined(PS3)

static HMODULE s_frameworkDLL;

static void CleanupFrameworkDLL()
{
	assert( s_frameworkDLL );
	CryFreeLibrary( s_frameworkDLL );
	s_frameworkDLL = 0;
}

HMODULE GetFrameworkDLL(const char* binariesDir)
{
	MEMSTAT_CONTEXT_FMT(EMemStatContextTypes::MSC_Other, 0, "Load %s",GAME_FRAMEWORK_FILENAME );
	if (!s_frameworkDLL)
	{
		if (binariesDir && binariesDir[0])
		{
			string dllName = PathUtil::Make(binariesDir, GAME_FRAMEWORK_FILENAME);
			s_frameworkDLL = CryLoadLibrary(dllName.c_str());		
		}
		else
		{
			s_frameworkDLL = CryLoadLibrary(GAME_FRAMEWORK_FILENAME);
		}
		atexit( CleanupFrameworkDLL );
	}
	return s_frameworkDLL;
}

#endif // !defined(_LIB) && !defined(PS3)
