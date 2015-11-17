#include "stdafx.h"

#include "FaceState.h"
#include <IFacialAnimation.h>

void FaceStateInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetEffectorWeight);
	REGISTER_METHOD(SetEffectorWeight);
}

float FaceStateInterop::GetEffectorWeight(IFaceState *handle, int nIndex)
{
	return handle->GetEffectorWeight(nIndex);
}

void FaceStateInterop::SetEffectorWeight(IFaceState *handle, int nIndex, float fWeight)
{
	handle->SetEffectorWeight(nIndex, fWeight);
}
