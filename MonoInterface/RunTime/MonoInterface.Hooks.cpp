#include "stdafx.h"

#include "MonoInterface.h"
#include "Implementation/MonoAssemblies.h"

#include <mono/utils/mono-logger.h>
static IMiniLog::ELogType monoToEngineLevels[] =
{
	IMiniLog::eAlways,
	IMiniLog::eErrorAlways,
	IMiniLog::eError,
	IMiniLog::eWarning,
	IMiniLog::eMessage,
	IMiniLog::eMessage,
	IMiniLog::eComment
};

void MonoInterface::MonoLogCallback(const char *log_domain, const char *log_level, const char *message,
									 mono_bool, void *)
{
	auto logLevel = monoToEngineLevels[0];
	if (log_level)
	{
		for (int i = 1; i < 7; i++)
		{
			// Only compare the first character since those are different in all names.
			if (monoLogLevels[i][0] == log_level[0])
			{
				logLevel = monoToEngineLevels[i];
			}
		}
	}

	gEnv->pLog->LogWithType(logLevel, "[CryCIL][%s][%s]: %s", log_domain, log_level, message);
}

void MonoInterface::MonoPrintCallback(const char *string, mono_bool)
{
	gEnv->pLog->Log(string);
}

void MonoInterface::MonoPrintErrorCallback(const char *string, mono_bool)
{
	gEnv->pLog->LogError(string);
}

void MonoInterface::RegisterHooks(MonoLog::Level logLevel)
{
	// Tell Mono what priority to give to log messages.
	this->MonoLogLevel = logLevel;

	// Install IO related hooks.
	mono_trace_set_log_handler(MonoLogCallback, this);
	mono_trace_set_print_handler(MonoPrintCallback);
	mono_trace_set_printerr_handler(MonoPrintErrorCallback);

	// Install assembly-related hooks.
	static_cast<MonoAssemblies *>(this->assemblies)->RegisterAssemblyHooks();
}
