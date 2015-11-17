#pragma once

#include "IMonoInterface.h"

struct IFacialAnimSequence;
struct IFacialEffectorsLibrary;

struct FacialAnimationInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialAnimation"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void OnRunTimeInitialized() override;

	static void                     ClearAllCachesInternal();
	static IFacialEffectorsLibrary *CreateEffectorsLibraryInternal();
	static void                     ClearEffectorsLibraryFromCacheInternal(mono::string filename);
	static IFacialEffectorsLibrary *LoadEffectorsLibraryInternal(mono::string filename);
	static IFacialAnimSequence     *CreateSequenceInternal();
	static void                     ClearSequenceFromCacheInternal(mono::string filename);
	static IFacialAnimSequence     *LoadSequenceInternal(mono::string filename, bool bNoWarnings, bool addToCache);
	static IFacialAnimSequence     *StartStreamingSequenceInternal(mono::string filename);
	static IFacialAnimSequence     *FindLoadedSequenceInternal(mono::string filename);
};