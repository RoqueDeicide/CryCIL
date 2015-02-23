#pragma once

#include "IMonoInterface.h"

void TestMemberLists(IMonoClass *);

void TestClasses()
{
	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;

	CryLogAlways("TEST: Checking %s.", vector3Class->FullName);

	TestMemberLists(vector3Class);



void TestMemberLists(IMonoClass *klass)
{

	CryLogAlways("TEST: Printing a list of fields of type %s.", klass->Name);

	auto fields = klass->Fields;

	for (int i = 0; i < fields->Length; i++)
	{
		CryLogAlways("TEST: Field #%d: %s", i, fields->At(i)->Name);
	}

	CryLogAlways("TEST: Printing a list of properties of type %s.", klass->Name);

	auto properties = klass->Properties;

	for (int i = 0; i < properties->Length; i++)
	{
		CryLogAlways("TEST: Property #%d: %s", i, properties->At(i)->Name);
	}

	CryLogAlways("TEST: Printing a list of methods of type %s.", klass->Name);

	auto methods = klass->Methods;

	for (int i = 0; i < methods->Length; i++)
	{
		CryLogAlways("TEST: Method #%d: %s", i, methods->At(i)->Name);
	}
}

	{
		CryLogAlways("TEST: Field #%d: %s", i, vector3Fields->At(i)->Name);
	}
}