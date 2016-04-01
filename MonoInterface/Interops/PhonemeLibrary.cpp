#include "stdafx.h"

#include "PhonemeLibrary.h"
#include <ICryAnimation.h>
#include <IFacialAnimation.h>

void PhonemeLibraryInterop::InitializeInterops()
{
	REGISTER_METHOD(GetPhonemeCount);
	REGISTER_METHOD(GetPhonemeInfo);
	REGISTER_METHOD(FindPhonemeByName);
}

int PhonemeLibraryInterop::GetPhonemeCount()
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->GetPhonemeLibrary()->GetPhonemeCount();
}

bool PhonemeLibraryInterop::GetPhonemeInfo(int nIndex, SPhonemeInfo &phoneme)
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->GetPhonemeLibrary()->GetPhonemeInfo(nIndex, phoneme);
}

int PhonemeLibraryInterop::FindPhonemeByName(const char* sPhonemeName)
{
	return gEnv->pCharacterManager->GetIFacialAnimation()->GetPhonemeLibrary()->FindPhonemeByName(sPhonemeName);
}
