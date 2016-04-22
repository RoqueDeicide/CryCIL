// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#define _SILENCE_STDEXT_HASH_DEPRECATION_WARNINGS

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
#include <map>
#include <functional>
#include <limits>

#include <smartptr.h>

#include <CryThread.h>
#include <Cry_Math.h>
#include <ISystem.h>

#pragma warning(push)
#pragma warning(disable : 4316)

#include <I3DEngine.h>

#pragma warning(pop)

#include <IInput.h>
#include <IConsole.h>
#include <ITimer.h>
#include <ILog.h>
#include <IGameplayRecorder.h>
#include <ISerialize.h>
#include <IGameFramework.h>

#define USE_CRYCIL_API

#include "MonoHeaders.h"

#include "IMonoInterface.h"
#include "Text.h"
#include "NtText.h"
#include "List.hpp"

// Include monosgen-2.0.lib from relevant folder. (Folder is defined in project properties.)
#pragma comment(lib, "monosgen-2.0.lib")

#include "targetver.h"

#include <CryWindows.h>
#include <CryLibrary.h>
#undef RemoveDirectory

//! Gets the pointer to the data represented by MonoObject instance.
//!
//! @param type Type the pointer to which to cast the result to. (Type of data represented by MonoObject).
//! @param obj  Pointer to object.
#define GET_BOXED_OBJECT_DATA(type, obj) (type *)(((unsigned char *)(obj)) + sizeof(MonoObject))

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the MONOINTERFACE_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// MONOINTERFACE_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef MONOINTERFACE_EXPORTS
#define MONOINTERFACE_API __declspec(dllexport)
#else
#define MONOINTERFACE_API __declspec(dllimport)
#endif

// TODO: reference additional headers your program requires here

//////////////////////////////////////////////////////////////////////////
//! Reports a warning to validator with WARNING severity.
inline void MonoWarning(const char *format, ...) PRINTF_PARAMS(1, 2);
inline void MonoWarning(const char *format, ...)
{
	if (!format)
		return;
	va_list args;
	va_start(args, format);
	GetISystem()->WarningV(VALIDATOR_MODULE_GAME, VALIDATOR_WARNING, 0, nullptr, format, args);
	va_end(args);
}
//! Prints out a warning that is associated with a game.
//!
//! This definition is needed for declaration and implementation of RMI calls.
inline void GameWarning(const char *format, ...)
{
	if (!format)
		return;
	va_list args;
	va_start(args, format);
	GetISystem()->WarningV(VALIDATOR_MODULE_GAME, VALIDATOR_WARNING, 0, nullptr, format, args);
	va_end(args);
}