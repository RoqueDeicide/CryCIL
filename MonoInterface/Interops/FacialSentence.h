#pragma once

#include "IMonoInterface.h"
#include <IFacialAnimation.h>

struct FacialSentenceInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialSentence"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void OnRunTimeInitialized() override;

	static void             SetText(IFacialSentence *handle, mono::string text);
	static mono::string     GetText(IFacialSentence *handle);
	static IPhonemeLibrary *GetPhonemeLib(IFacialSentence *handle);
	static void             ClearAllPhonemes(IFacialSentence *handle);
	static int              GetPhonemeCount(IFacialSentence *handle);
	static bool             GetPhoneme(IFacialSentence *handle, int index, IFacialSentence::Phoneme &ph);
	static int              AddPhoneme(IFacialSentence *handle, const IFacialSentence::Phoneme &ph);
	static void             ClearAllWords(IFacialSentence *handle);
	static int              GetWordCount(IFacialSentence *handle);
	static bool             GetWord(IFacialSentence *handle, int index, IFacialSentence::Word &wrd);
	static void             AddWord(IFacialSentence *handle, const IFacialSentence::Word &wrd);
};