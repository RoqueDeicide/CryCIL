#if !defined(AFX_STDAFX_H__B36C365D_F0EA_4545_B3BC_1E0EAB3B5E43__INCLUDED_)
#define AFX_STDAFX_H__B36C365D_F0EA_4545_B3BC_1E0EAB3B5E43__INCLUDED_

#include <CryModuleDefs.h>

#define _FORCEDLL

#ifndef _RELEASE
#define USE_CRY_ASSERT
#endif

// Insert your headers here
#include <platform.h>
#include <algorithm>
#include <vector>
#include <memory>
#include <list>
#include <functional>
#include <limits>

#include <smartptr.h>

#include <CryThread.h>
#include <Cry_Math.h>
#include <ISystem.h>
#include <I3DEngine.h>
#include <IInput.h>
#include <IConsole.h>
#include <ITimer.h>
#include <ILog.h>
#include <IGameplayRecorder.h>
#include <ISerialize.h>

#include <mono/mini/jit.h>

#undef NULL
#define NULL nullptr

#pragma comment(lib, "monosgen")

#ifndef MONODLL_EXPORTS
#define MONODLL_EXPORTS
#endif

#ifdef MONODLL_EXPORTS
#define CRYMONO_API DLL_EXPORT
#else
#define CRYMONO_API
#endif

//#define PLUGIN_SDK

//////////////////////////////////////////////////////////////////////////
//! Reports a warning to validator with WARNING severity.
inline void MonoWarning( const char *format,... ) PRINTF_PARAMS(1, 2);
inline void MonoWarning( const char *format,... )
{
	if (!format)
		return;
	va_list args;
	va_start(args, format);
	GetISystem()->WarningV( VALIDATOR_MODULE_GAME,VALIDATOR_WARNING,0,nullptr,format,args );
	va_end(args);
}

inline void GameWarning( const char *format,... )
{
	if (!format)
		return;
	va_list args;
	va_start(args, format);
	GetISystem()->WarningV( VALIDATOR_MODULE_GAME,VALIDATOR_WARNING,0,nullptr,format,args );
	va_end(args);
}

extern struct SCVars *g_pMonoCVars;

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif //AFX_STDAFX_H__B36C365D_F0EA_4545_B3BC_1E0EAB3B5E43__INCLUDED_