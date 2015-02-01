#pragma once

#include "IMonoInterface.h"

void TestAssemblies()
{
	CryLogAlways("Testing IMonoAssemblyCollection and IMonoAssembly implementations.");

	CryLogAlways("TEST: Loading an assembly that wasn't loaded previously.");

	IMonoAssembly *mainTestingAssembly =
		MonoEnv->Assemblies->Load("Testing\\MainTestingAssembly.dll");

	if (!mainTestingAssembly)
	{
		CryLogAlways("TEST FAILURE: Loading an assembly that wasn't loaded previously was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Loading an assembly that wasn't loaded previously was successful.");
	}

	CryLogAlways("TEST: Acquiring a pointer to the wrapper of the assembly that was loaded previously.");

	mainTestingAssembly = MonoEnv->Assemblies->Load("Testing\\MainTestingAssembly.dll");

	if (!mainTestingAssembly)
	{
		CryLogAlways("TEST FAILURE: Acquiring a pointer to the wrapper of the assembly that was loaded previously was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Acquiring a pointer to the wrapper of the assembly that was loaded previously was successful.");
	}

	CryLogAlways("TEST: Acquiring a pointer to the wrapper of the assembly using its short name.");

	mainTestingAssembly = MonoEnv->Assemblies->GetAssembly("MainTestingAssembly");

	if (!mainTestingAssembly)
	{
		CryLogAlways("TEST FAILURE: Acquiring a pointer to the wrapper of the assembly using its short name was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Acquiring a pointer to the wrapper of the assembly using its short name was successful.");
	}

	CryLogAlways("TEST: Acquiring a pointer to the wrapper of the assembly using its full name.");

	mainTestingAssembly =
		MonoEnv->Assemblies->GetAssemblyFullName
			("MainTestingAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

	if (!mainTestingAssembly)
	{
		CryLogAlways("TEST FAILURE: Acquiring a pointer to the wrapper of the assembly using its full name was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Acquiring a pointer to the wrapper of the assembly using its full name was successful.");
	}

	CryLogAlways("TEST: Getting a class from the assembly.");

	IMonoClass *class1 = mainTestingAssembly->GetClass("MainTestingAssembly", "Class1");

	if (!class1)
	{
		CryLogAlways("TEST FAILURE: Getting a class from the assembly was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Getting a class from the assembly was successful.");
	}

	CryLogAlways("TEST: Getting assembly details.");

	const char *detail = mainTestingAssembly->Name->ToNTString();
	CryLogAlways("TEST: Assembly short name: %s", mainTestingAssembly->Name);
	delete detail;
	detail = mainTestingAssembly->FullName->ToNTString();
	CryLogAlways("TEST: Assembly full name: %s", mainTestingAssembly->FullName);
	delete detail;
	detail = mainTestingAssembly->FileName->ToNTString();
	CryLogAlways("TEST: Assembly file name: %s", mainTestingAssembly->FileName);
	delete detail;

	CryLogAlways("TEST: Getting assembly reflection object.");

	mono::assembly refAssembly = mainTestingAssembly->ReflectionObject;

	if (!refAssembly)
	{
		CryLogAlways("TEST FAILURE: Getting assembly reflection object was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Getting assembly reflection object was successful.");
	}

	CryLogAlways("TEST: Wrapping assembly reflection object.");

	IMonoHandle *refAssemblyWrapper = MonoEnv->Objects->Wrap(refAssembly);

	if (!refAssemblyWrapper)
	{
		CryLogAlways("TEST FAILURE: Wrapping assembly reflection object was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Wrapping assembly reflection object was successful.");
	}

	CryLogAlways("TEST: Full assembly name from reflection object:");

	const char *fullAssemblyName = ToNativeString(refAssemblyWrapper->GetProperty("FullName")->Getter->Invoke(refAssembly));
	CryLogAlways("TEST: %s", fullAssemblyName);
	delete fullAssemblyName;
}