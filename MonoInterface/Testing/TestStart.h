#pragma once

#include "IMonoInterface.h"

extern const IMonoAssembly *mainTestingAssembly;

//! In a stable version all of the code executed by this function must work 100% perfectly.
void extern BeginTheTest();