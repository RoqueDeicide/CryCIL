#include "stdafx.h"

#include "FacialModel.h"
#include <CryAnimation/IFacialAnimation.h>

void FacialModelInterop::InitializeInterops()
{
	REGISTER_METHOD(GetEffectorCount);
	REGISTER_METHOD(GetEffector);
	REGISTER_METHOD(AssignLibrary);
	REGISTER_METHOD(GetLibrary);
	REGISTER_METHOD(GetMorphTargetCount);
	REGISTER_METHOD(GetMorphTargetName);
}

int FacialModelInterop::GetEffectorCount(IFacialModel *handle)
{
	return handle->GetEffectorCount();
}

IFacialEffector *FacialModelInterop::GetEffector(IFacialModel *handle, int nIndex)
{
	return handle->GetEffector(nIndex);
}

void FacialModelInterop::AssignLibrary(IFacialModel *handle, IFacialEffectorsLibrary *pLibrary)
{
	handle->AssignLibrary(pLibrary);
}

IFacialEffectorsLibrary *FacialModelInterop::GetLibrary(IFacialModel *handle)
{
	return handle->GetLibrary();
}

int FacialModelInterop::GetMorphTargetCount(IFacialModel *handle)
{
	return handle->GetMorphTargetCount();
}

mono::string FacialModelInterop::GetMorphTargetName(IFacialModel *handle, int morphTargetIndex)
{
	return ToMonoString(handle->GetMorphTargetName(morphTargetIndex));
}
