#pragma once

#include <CrySystem/ISystem.h>

#define HIGH_LOG_VERBOSITY
#define EXTRA_HIGH_LOG_VERBOSITY


// General comments.
#ifdef HIGH_LOG_VERBOSITY
#define ReportComment CryLogAlways
#else
#define ReportComment CryComment
#endif // HIGH_LOG_VERBOSITY
// General flicks.
#ifdef EXTRA_HIGH_LOG_VERBOSITY
#define FlickerComment CryLogAlways
#else
#define FlickerComment CryComment
#endif // HIGH_LOG_VERBOSITY
// General messages.
#ifdef HIGH_LOG_VERBOSITY
#define ReportMessage CryLogAlways
#else
#define ReportMessage CryLog
#endif // HIGH_LOG_VERBOSITY

// Errors.
#define ReportError gEnv->pLog->LogError