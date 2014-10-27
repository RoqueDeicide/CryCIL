// stdafx.cpp : source file that includes just the standard includes
// MonoInterface.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"
#include <platform_impl.h>			// One time this file is included.

// TODO: reference any additional headers you need in STDAFX.H
// and not in this file

IMonoInterface *MonoEnv = nullptr;
//! Provides access to IGameFramework implementation.
IGameFramework *Framework = nullptr;