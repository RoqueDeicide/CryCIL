#pragma once

#include "IMonoInterface.h"

struct IPhonemeLibrary;
struct SPhonemeInfo;

struct PhonemeLibraryInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PhonemeLibrary"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static int  GetPhonemeCount();
	static bool GetPhonemeInfo(int nIndex, SPhonemeInfo &phoneme);
	static int  FindPhonemeByName(const char* sPhonemeName);
};