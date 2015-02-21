// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once
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
#include <IGameFramework.h>

#include "MonoHeaders.h"

#include "IMonoInterface.h"
#include "Text.h"

// Include monosgen.lib from relevant folder. (Folder is defined in project properties.)
#pragma comment(lib, "monosgen")

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>

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
