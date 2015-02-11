#pragma once

#include "IMonoInterface.h"

void TestClasses()
{
	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;

	CryLogAlways("TEST: Checking %s.", vector3Class->FullName);

	CryLogAlways("TEST: Printing a list of fields.");

	auto vector3Fields = vector3Class->Fields;

	for (int i = 0; i < vector3Fields->Length; i++)
	{
		CryLogAlways("TEST: Field #%d: %s", i, vector3Fields->At(i)->Name);
	}
}