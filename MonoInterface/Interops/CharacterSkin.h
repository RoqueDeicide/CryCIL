#pragma once

#include "IMonoInterface.h"

struct ISkin;

struct CharacterSkinInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CharacterSkin"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters"; }

	virtual void InitializeInterops() override;

	static IRenderMesh *GetIRenderMesh(ISkin *handle, uint nLOD);
	static mono::string GetModelFilePath(ISkin *handle);
	static IMaterial   *GetIMaterial(ISkin *handle, uint nLOD);
};