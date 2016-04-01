#pragma once

#include "IMonoInterface.h"

struct SimulationParams;

struct AttachmentRowSimulationParametersInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AttachmentRowSimulationParameters"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Attachments"; }

	virtual void InitializeInterops() override;

	static mono::Array GetProxyNames(SimulationParams *handle);
	static void SetProxyNames(SimulationParams *handle, mono::Array names);
};