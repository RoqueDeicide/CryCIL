#pragma once

#include "IMonoInterface.h"

struct ArchiveStreamInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "ArchiveStream"; }
	virtual const char *GetNameSpace() { return "CryCil.Engine.Files"; }

	virtual void OnRunTimeInitialized();

	static void UpdateFile(mono::object obj, const char *path, byte* data, uint length);
};