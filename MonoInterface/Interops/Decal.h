#pragma once

#include "IMonoInterface.h"

struct MonoDecalInfo;

struct DecalInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Decal"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void InitializeInterops() override;

	static void CreateDecal(MonoDecalInfo &info);
	static void DeleteDecalsInRange(AABB *pAreaBox, IRenderNode *pEntity);
	static void DeleteEntityDecals(IRenderNode *pEntity);
};