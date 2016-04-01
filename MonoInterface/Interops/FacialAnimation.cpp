#include "stdafx.h"

#include "FacialAnimation.h"
#include <ICryAnimation.h>
#include <IFacialAnimation.h>

void FacialAnimationInterop::InitializeInterops()
{
	REGISTER_METHOD(ClearAllCachesInternal);
	REGISTER_METHOD(CreateEffectorsLibraryInternal);
	REGISTER_METHOD(ClearEffectorsLibraryFromCacheInternal);
	REGISTER_METHOD(LoadEffectorsLibraryInternal);
	REGISTER_METHOD(CreateSequenceInternal);
	REGISTER_METHOD(ClearSequenceFromCacheInternal);
	REGISTER_METHOD(LoadSequenceInternal);
	REGISTER_METHOD(StartStreamingSequenceInternal);
	REGISTER_METHOD(FindLoadedSequenceInternal);
}

void FacialAnimationInterop::ClearAllCachesInternal()
{
	gEnv->pCharacterManager->GetIFacialAnimation()->ClearAllCaches();
}

IFacialEffectorsLibrary *FacialAnimationInterop::CreateEffectorsLibraryInternal()
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->CreateEffectorsLibrary();
}

void FacialAnimationInterop::ClearEffectorsLibraryFromCacheInternal(mono::string filename)
{
	gEnv->pCharacterManager->GetIFacialAnimation()->ClearEffectorsLibraryFromCache(NtText(filename));
}

IFacialEffectorsLibrary *FacialAnimationInterop::LoadEffectorsLibraryInternal(mono::string filename)
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->LoadEffectorsLibrary(NtText(filename));
}

IFacialAnimSequence *FacialAnimationInterop::CreateSequenceInternal()
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->CreateSequence();
}

void FacialAnimationInterop::ClearSequenceFromCacheInternal(mono::string filename)
{
	gEnv->pCharacterManager->GetIFacialAnimation()->ClearSequenceFromCache(NtText(filename));
}

IFacialAnimSequence *FacialAnimationInterop::LoadSequenceInternal(mono::string filename, bool bNoWarnings, bool addToCache)
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->LoadSequence(NtText(filename), bNoWarnings, addToCache);
}

IFacialAnimSequence *FacialAnimationInterop::StartStreamingSequenceInternal(mono::string filename)
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->StartStreamingSequence(NtText(filename));
}

IFacialAnimSequence *FacialAnimationInterop::FindLoadedSequenceInternal(mono::string filename)
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->FindLoadedSequence(NtText(filename));
}
