#pragma once

#include "IMonoInterface.h"

class CFaceIdentifierHandle;

struct FaceIdentifierInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FaceIdentifier"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void OnRunTimeInitialized() override;

	static void CreateIdentifier(mono::string name, CFaceIdentifierHandle *stringHandle);
};