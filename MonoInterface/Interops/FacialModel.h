#pragma once

#include "IMonoInterface.h"

struct IFacialModel;
struct IFacialEffectorsLibrary;
struct IFacialEffector;

struct FacialModelInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialModel"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static int                      GetEffectorCount(IFacialModel *handle);
	static IFacialEffector         *GetEffector(IFacialModel *handle, int nIndex);
	static void                     AssignLibrary(IFacialModel *handle, IFacialEffectorsLibrary *pLibrary);
	static IFacialEffectorsLibrary *GetLibrary(IFacialModel *handle);
	static int                      GetMorphTargetCount(IFacialModel *handle);
	static mono::string             GetMorphTargetName(IFacialModel *handle, int morphTargetIndex);
};