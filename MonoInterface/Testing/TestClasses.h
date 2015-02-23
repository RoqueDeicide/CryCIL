#pragma once

#include "IMonoInterface.h"

void TestMemberLists(IMonoClass *);
void TestInheritance(IMonoClass *);
void TestInterfaceImplementation();

void TestClasses()
{
	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;

	CryLogAlways("TEST: Checking %s.", vector3Class->FullName);

	TestMemberLists(vector3Class);

	CryLogAlways("TEST: Finding out about base class of Vector3 class.");

	TestInheritance(vector3Class);

	TestInterfaceImplementation();
}

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

void TestInheritance(IMonoClass *klass)
{
	IMonoClass *baseClass = klass->Base;

	if (!baseClass)
	{
		ReportError("TEST FAILURE: Unable to get base class of %s.", klass->Name);
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Name of the base class of %s is %s.", klass->Name, baseClass->Name);
	}

	CryLogAlways("TEST: Testing inheritance detection on %s.", klass->Name);

	IMonoClass *valueTypeClass = MonoEnv->CoreLibrary->ValueType;

	if (!valueTypeClass)
	{
		ReportError("TEST FAILURE: Unable to get the wrapper for System.ValueType class.");
	}
	else
	{
		if (klass->Inherits(valueTypeClass))
		{
			CryLogAlways("TEST %s: %s inherits from System.ValueType.",
						 klass->IsValueType ? "SUCCESS" : "FAILURE",
						 klass->Name);
		}
		else
		{
			ReportError("TEST %s: %s doesn't inherit from System.ValueType.",
						klass->IsValueType ? "SUCCESS" : "FAILURE",
						klass->Name);
		}

		if (klass->Inherits(valueTypeClass, true))
		{
			CryLogAlways("TEST %s: %s directly inherits from System.ValueType.",
						 klass->IsValueType ? "SUCCESS" : "FAILURE",
						 klass->Name);
		}
		else
		{
			ReportError("TEST %s: %s doesn't directly inherit from System.ValueType.",
						klass->IsValueType ? "SUCCESS" : "FAILURE",
						klass->Name);
		}
	}

	if (klass->Inherits("System", "Object"))
	{
		CryLogAlways("TEST SUCCESS: %s inherits from System.Object.", klass->Name);
	}
	else
	{
		ReportError("TEST FAILURE: For some reason %s doesn't inherit from System.Object.", klass->Name);
	}

	if (klass->Inherits("System", "Object", true))
	{
		ReportError("TEST FAILURE: %s directly inherits from System.Object.", klass->Name);
	}
	else
	{
		CryLogAlways("TEST SUCCESS: %s doesn't inherit from System.Object directly.", klass->Name);
	}
}

void TestInterfaceImplementation()
{
	CryLogAlways("TEST: Checking direct interface implementation detection.");

	IMonoClass *vsProjectClass =
		MonoEnv->Cryambly->GetClass("CryCil.RunTime.Compilation", "VisualStudioDotNetProject");

	IMonoClass *iProjectInterface =
		MonoEnv->Cryambly->GetClass("CryCil.RunTime.Compilation", "IProject");

	if (vsProjectClass->Implements(iProjectInterface))
	{
		CryLogAlways("TEST SUCCESS: VisualStudioDotNetProject implements IProject.");
	}
	else
	{
		ReportError("TEST FAILURE: VisualStudioDotNetProject is supposed to implement IProject.");
	}

	CryLogAlways("TEST: Checking indirect interface implementation detection.");

	IMonoClass *logWriterClass =
		MonoEnv->Cryambly->GetClass("CryCil.RunTime.Logging", "ConsoleLogWriter");

	IMonoClass *iDisposableInterface =
		MonoEnv->CoreLibrary->GetClass("System", "IDisposable");

	if (logWriterClass->Implements(iDisposableInterface))
	{
		CryLogAlways("TEST SUCCESS: ConsoleLogWriter implements IDisposable.");
	}
	else
	{
		ReportError("TEST FAILURE: ConsoleLogWriter is supposed to implement IDisposable.");
	}
}