#include "stdafx.h"

#include "PhonemeLibrary.h"
#include <IFacialAnimation.h>

void PhonemeLibraryInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(GetPhonemeCount);
	REGISTER_METHOD(GetPhonemeInfo);
	REGISTER_METHOD(FindPhonemeByName);
}

int PhonemeLibraryInterop::GetPhonemeCount(IPhonemeLibrary *handle)
{
	return handle->GetPhonemeCount();
}

bool PhonemeLibraryInterop::GetPhonemeInfo(IPhonemeLibrary *handle, int nIndex, SPhonemeInfo &phoneme)
{
	return handle->GetPhonemeInfo(nIndex, phoneme);
}

int PhonemeLibraryInterop::FindPhonemeByName(IPhonemeLibrary *handle, const char* sPhonemeName)
{
	return handle->FindPhonemeByName(sPhonemeName);
}
