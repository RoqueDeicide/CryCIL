#pragma once

#include "IMonoInterface.h"
#include <IAudioSystem.h>

struct AudioSystemInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "AudioSystem"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil."; }

	virtual void OnRunTimeInitialized() override;

	
};