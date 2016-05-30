// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include <CryCore/Project/CryModuleDefs.h>
#define eCryModule eCryM_Game
#define GAME_API DLL_EXPORT

#include <CryCore/Platform/platform.h>
#include <algorithm>
#include <vector>
#include <memory>
#include <list>
#include <functional>
#include <limits>
#include <math.h>

#include <CryCore/smartptr.h>

#include <CryThreading/CryThread.h>
#include <CryMath/Cry_Math.h>
#include <CrySystem/ISystem.h>
#include <Cry3DEngine/I3DEngine.h>
#include <CryInput/IInput.h>
#include <CrySystem/IConsole.h>
#include <CrySystem/ITimer.h>
#include <CrySystem/ILog.h>
#include <IGameplayRecorder.h>
#include <CryNetwork/ISerialize.h>
#include <CryGame/IGameFramework.h>
#include <CryPhysics/physinterface.h>

#include "targetver.h"

#include <CryCore/Platform/CryWindows.h>
#include <CryCore/Platform/CryLibrary.h>
#undef RemoveDirectory

#include "IMonoInterface.h"
#include "Text.h"
#include "NtText.h"
#include "List.hpp"

// TODO: reference additional headers your program requires here
