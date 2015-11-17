#include "stdafx.h"

#include "FacialSentence.h"

void FacialSentenceInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetText);
	REGISTER_METHOD(GetText);
	REGISTER_METHOD(GetPhonemeLib);
	REGISTER_METHOD(ClearAllPhonemes);
	REGISTER_METHOD(GetPhonemeCount);
	REGISTER_METHOD(GetPhoneme);
	REGISTER_METHOD(AddPhoneme);
	REGISTER_METHOD(ClearAllWords);
	REGISTER_METHOD(GetWordCount);
	REGISTER_METHOD(GetWord);
	REGISTER_METHOD(AddWord);
}

void FacialSentenceInterop::SetText(IFacialSentence *handle, mono::string text)
{
	handle->SetText(NtText(text));
}

mono::string FacialSentenceInterop::GetText(IFacialSentence *handle)
{
	return ToMonoString(handle->GetText());
}

IPhonemeLibrary *FacialSentenceInterop::GetPhonemeLib(IFacialSentence *handle)
{
	return handle->GetPhonemeLib();
}

void FacialSentenceInterop::ClearAllPhonemes(IFacialSentence *handle)
{
	handle->ClearAllPhonemes();
}

int FacialSentenceInterop::GetPhonemeCount(IFacialSentence *handle)
{
	return handle->GetPhonemeCount();
}

bool FacialSentenceInterop::GetPhoneme(IFacialSentence *handle, int index, IFacialSentence::Phoneme &ph)
{
	return handle->GetPhoneme(index, ph);
}

int FacialSentenceInterop::AddPhoneme(IFacialSentence *handle, const IFacialSentence::Phoneme &ph)
{
	return handle->AddPhoneme(ph);
}

void FacialSentenceInterop::ClearAllWords(IFacialSentence *handle)
{
	handle->ClearAllWords();
}

int FacialSentenceInterop::GetWordCount(IFacialSentence *handle)
{
	return handle->GetWordCount();
}

bool FacialSentenceInterop::GetWord(IFacialSentence *handle, int index, IFacialSentence::Word &wrd)
{
	return handle->GetWord(index, wrd);
}

void FacialSentenceInterop::AddWord(IFacialSentence *handle, const IFacialSentence::Word &wrd)
{
	handle->AddWord(wrd);
}
