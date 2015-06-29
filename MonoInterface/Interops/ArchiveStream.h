#pragma once

#include "IMonoInterface.h"

struct ArchiveStreamInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "ArchiveStream"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Files"; }

	virtual void OnRunTimeInitialized() override;

	static void UpdateFile(mono::object obj, const char *path, byte* data, uint length);
};