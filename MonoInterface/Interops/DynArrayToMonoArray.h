#pragma once

#include "IMonoInterface.h"

inline mono::Array ToMonoArray(DynArray<CCryName> &names)
{
	if (names.size() == 0)
	{
		return nullptr;
	}

	IMonoArray<mono::string> namesArray =
		MonoEnv->Objects->Arrays->Create(names.size(), MonoEnv->CoreLibrary->String);

	MonoGCHandle arrayHandle = MonoEnv->GC->Pin(namesArray);
	MonoGCHandle *stringHandles = new MonoGCHandle[names.size()];	// Just in case.

	for (int i = 0; i < names.size(); i++)
	{
		mono::string name = ToMonoString(names[i].c_str());

		stringHandles[i] = MonoEnv->GC->Pin(name);

		namesArray[i] = name;
	}
	// Unpin each name.
	delete[] stringHandles;

	return namesArray;
}

inline void OverwriteFromMonoArray(mono::Array monoArray, DynArray<CCryName> &names)
{
	names.clear();

	if (!monoArray)
	{
		return;
	}

	IMonoArray<mono::string> namesArray = monoArray;

	if (namesArray.Length == 0)
	{
		return;
	}

	MonoGCHandle arrayHandle = MonoEnv->GC->Pin(monoArray);

	int length = namesArray.Length;
	for (int i = 0; i < length; i++)
	{
		names.push_back(CCryName(NtText(namesArray[i])));
	}
}