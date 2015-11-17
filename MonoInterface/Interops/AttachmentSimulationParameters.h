#pragma once

#include "IMonoInterface.h"

struct SimulationParams;

struct AttachmentSimulationParametersInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AttachmentSimulationParameters"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Attachments"; }

	virtual void OnRunTimeInitialized() override;

	static mono::Array GetProxyNames(SimulationParams *handle);
	static void SetProxyNames(SimulationParams *handle, mono::Array names);
};