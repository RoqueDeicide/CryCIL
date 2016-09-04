// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

// TODO: reference additional headers your program requires here
#include <CryCore/Project/CryModuleDefs.h>

// Insert your headers here
#include <CryCore/Platform/platform.h>

#include <memory>
#include <vector>
#include <map>
#include <queue>
#include <string>
#include <exception>
#include <stdlib.h>
#include <string.h>
#include <stdio.h>

#include <CryCore/Project/ProjectDefines.h>
#include <CryMath/Cry_Math.h>
#include <CryMath/Cry_Camera.h>
#include <CrySystem/ISystem.h>
#include <CryNetwork/INetwork.h>
#include <CryInput/IInput.h>
#include <CryScriptSystem/IScriptSystem.h>
#include <CryEntitySystem/IEntitySystem.h>
#include <CryNetwork/NetHelpers.h>
#include <CrySystem/File/ICryPak.h>
#include <CrySystem/IConsole.h>
#include <CrySystem/ITimer.h>
#include <CrySystem/ILog.h>
#include <CryNetwork/IRemoteControl.h>
#include <CryNetwork/ISimpleHttpServer.h>
#include <CryGame/IGameFramework.h>
#include <CryGame/IGame.h>
#include <CryFlowGraph/IFlowSystem.h>

#include <CryAudio/IAudioSystem.h>
