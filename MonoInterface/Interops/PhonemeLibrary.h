#pragma once

#include "IMonoInterface.h"

struct IPhonemeLibrary;
struct SPhonemeInfo;

struct PhonemeLibraryInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PhonemeLibrary"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void OnRunTimeInitialized() override;

	static int  GetPhonemeCount(IPhonemeLibrary *handle);
	static bool GetPhonemeInfo(IPhonemeLibrary *handle, int nIndex, SPhonemeInfo &phoneme);
	static int  FindPhonemeByName(IPhonemeLibrary *handle, const char* sPhonemeName);
};