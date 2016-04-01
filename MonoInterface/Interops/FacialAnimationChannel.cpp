#include "stdafx.h"

#include "FacialAnimationChannel.h"
#include <IFacialAnimation.h>

void FacialAnimationChannelInterop::InitializeInterops()
{
	REGISTER_METHOD(SetIdentifier);
	REGISTER_METHOD(GetIdentifier);
	REGISTER_METHOD(SetEffectorIdentifier);
	REGISTER_METHOD(GetEffectorIdentifier);
	REGISTER_METHOD(SetParent);
	REGISTER_METHOD(GetParent);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(SetEffector);
	REGISTER_METHOD(GetEffector);
	REGISTER_METHOD(GetInterpolator);
	REGISTER_METHOD(GetLastInterpolator);
	REGISTER_METHOD(AddInterpolator);
	REGISTER_METHOD(DeleteInterpolator);
	REGISTER_METHOD(GetInterpolatorCount);
	REGISTER_METHOD(CleanupKeysInternal);
	REGISTER_METHOD(SmoothKeysInternal);
	REGISTER_METHOD(RemoveNoiseInternal);
}

void FacialAnimationChannelInterop::SetIdentifier(IFacialAnimChannel *handle, CFaceIdentifierHandle ident)
{
	handle->SetIdentifier(ident);
}

CFaceIdentifierHandle FacialAnimationChannelInterop::GetIdentifier(IFacialAnimChannel *handle)
{
	return handle->GetIdentifier();
}

void FacialAnimationChannelInterop::SetEffectorIdentifier(IFacialAnimChannel *handle, CFaceIdentifierHandle ident)
{
	handle->SetEffectorIdentifier(ident);
}

CFaceIdentifierHandle FacialAnimationChannelInterop::GetEffectorIdentifier(IFacialAnimChannel *handle)
{
	return handle->GetEffectorIdentifier();
}

void FacialAnimationChannelInterop::SetParent(IFacialAnimChannel *handle, IFacialAnimChannel *pParent)
{
	handle->SetParent(pParent);
}

IFacialAnimChannel *FacialAnimationChannelInterop::GetParent(IFacialAnimChannel *handle)
{
	return handle->GetParent();
}

void FacialAnimationChannelInterop::SetFlags(IFacialAnimChannel *handle, uint32 nFlags)
{
	handle->SetFlags(nFlags);
}

uint32 FacialAnimationChannelInterop::GetFlags(IFacialAnimChannel *handle)
{
	return handle->GetFlags();
}

void FacialAnimationChannelInterop::SetEffector(IFacialAnimChannel *handle, IFacialEffector *pEffector)
{
	handle->SetEffector(pEffector);
}

IFacialEffector *FacialAnimationChannelInterop::GetEffector(IFacialAnimChannel *handle)
{
	return handle->GetEffector();
}

ISplineInterpolator *FacialAnimationChannelInterop::GetInterpolator(IFacialAnimChannel *handle, int i)
{
	return handle->GetInterpolator(i);
}

ISplineInterpolator *FacialAnimationChannelInterop::GetLastInterpolator(IFacialAnimChannel *handle)
{
	return handle->GetLastInterpolator();
}

void FacialAnimationChannelInterop::AddInterpolator(IFacialAnimChannel *handle)
{
	handle->AddInterpolator();
}

void FacialAnimationChannelInterop::DeleteInterpolator(IFacialAnimChannel *handle, int i)
{
	handle->DeleteInterpolator(i);
}

int FacialAnimationChannelInterop::GetInterpolatorCount(IFacialAnimChannel *handle)
{
	return handle->GetInterpolatorCount();
}

void FacialAnimationChannelInterop::CleanupKeysInternal(IFacialAnimChannel *handle, float fErrorMax)
{
	handle->CleanupKeys(fErrorMax);
}

void FacialAnimationChannelInterop::SmoothKeysInternal(IFacialAnimChannel *handle, float sigma)
{
	handle->SmoothKeys(sigma);
}

void FacialAnimationChannelInterop::RemoveNoiseInternal(IFacialAnimChannel *handle, float sigma, float threshold)
{
	handle->RemoveNoise(sigma, threshold);
}
