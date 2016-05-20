// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include <CryModuleDefs.h>
#define eCryModule eCryM_Game
#define GAME_API DLL_EXPORT

#include <platform.h>
#include <algorithm>
#include <vector>
#include <memory>
#include <list>
#include <functional>
#include <limits>
#include <math.h>

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

#include "targetver.h"

#include <CryWindows.h>
#include <CryLibrary.h>
#undef RemoveDirectory

#include "IMonoInterface.h"
#include "Text.h"
#include "NtText.h"
#include "List.hpp"

// TODO: reference additional headers your program requires here
