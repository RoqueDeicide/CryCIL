#pragma once

#include "IMonoInterface.h"

struct IFacialAnimSoundEntry;
struct IFacialSentence;

struct FacialAnimationSoundEntryInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialAnimationSoundEntry"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static void             SetSoundFile(IFacialAnimSoundEntry *handle, mono::string sSoundFile);
	static mono::string     GetSoundFile(IFacialAnimSoundEntry *handle);
	static IFacialSentence *GetSentence (IFacialAnimSoundEntry *handle);
	static float            GetStartTime(IFacialAnimSoundEntry *handle);
	static void             SetStartTime(IFacialAnimSoundEntry *handle, float time);
};