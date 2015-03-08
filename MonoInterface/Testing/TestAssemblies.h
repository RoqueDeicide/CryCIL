#pragma once

#include "IMonoInterface.h"

void TestClassFromAssembly(IMonoClass *, const char *);

void TestAssemblies()
{
	CryLogAlways("TEST: Testing IMonoAssemblyCollection and IMonoAssembly implementations.");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Loading an assembly that wasn't loaded previously.");
	CryLogAlways("TEST:");

	mainTestingAssembly =
		MonoEnv->Assemblies->Load("Testing\\MainTestingAssembly.dll");

	if (!mainTestingAssembly)
	{
		CryLogAlways("TEST FAILURE: Loading an assembly that wasn't loaded previously was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Loading an assembly that wasn't loaded previously was successful.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Acquiring a pointer to the wrapper of the assembly that was loaded previously.");
	CryLogAlways("TEST:");

	mainTestingAssembly = MonoEnv->Assemblies->Load("Testing\\MainTestingAssembly.dll");

	if (!mainTestingAssembly)
	{
		CryLogAlways("TEST FAILURE: Acquiring a pointer to the wrapper of the assembly that was loaded previously was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Acquiring a pointer to the wrapper of the assembly that was loaded previously was successful.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Acquiring a pointer to the wrapper of the assembly using its short name.");
	CryLogAlways("TEST:");

	mainTestingAssembly = MonoEnv->Assemblies->GetAssembly("MainTestingAssembly");

	if (!mainTestingAssembly)
	{
		CryLogAlways("TEST FAILURE: Acquiring a pointer to the wrapper of the assembly using its short name was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Acquiring a pointer to the wrapper of the assembly using its short name was successful.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Acquiring a pointer to the wrapper of the assembly using its full name.");
	CryLogAlways("TEST:");

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

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a class from the assembly.");
	CryLogAlways("TEST:");

	IMonoClass *class1 = mainTestingAssembly->GetClass("MainTestingAssembly", "Class1");

	if (!class1)
	{
		CryLogAlways("TEST FAILURE: Getting a class from the assembly was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Getting a class from the assembly was successful.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting assembly details.");
	CryLogAlways("TEST:");

	const char *detail = mainTestingAssembly->Name->ToNTString();
	CryLogAlways("TEST: Assembly short name: %s", mainTestingAssembly->Name);
	delete detail;
	detail = mainTestingAssembly->FullName->ToNTString();
	CryLogAlways("TEST: Assembly full name: %s", mainTestingAssembly->FullName);
	delete detail;
	detail = mainTestingAssembly->FileName->ToNTString();
	CryLogAlways("TEST: Assembly file name: %s", mainTestingAssembly->FileName);
	delete detail;

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting assembly reflection object.");
	CryLogAlways("TEST:");

	mono::assembly refAssembly = mainTestingAssembly->ReflectionObject;

	if (!refAssembly)
	{
		CryLogAlways("TEST FAILURE: Getting assembly reflection object was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Getting assembly reflection object was successful.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Wrapping assembly reflection object.");
	CryLogAlways("TEST:");

	IMonoHandle *refAssemblyWrapper = MonoEnv->Objects->Wrap(refAssembly);

	if (!refAssemblyWrapper)
	{
		CryLogAlways("TEST FAILURE: Wrapping assembly reflection object was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Wrapping assembly reflection object was successful.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Full assembly name from reflection object:");
	CryLogAlways("TEST:");

	const char *fullAssemblyName =
		ToNativeString(refAssemblyWrapper->GetProperty("FullName")->Getter->ToInstance()->Invoke(refAssembly));
	CryLogAlways("TEST: %s", fullAssemblyName);
	delete fullAssemblyName;

	delete refAssemblyWrapper;

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a wrapper for mscorlib.");
	CryLogAlways("TEST:");

	IMonoCoreLibrary *corlib = MonoEnv->CoreLibrary;

	if (!corlib)
	{
		CryLogAlways("TEST FAILURE: Getting a wrapper for mscorlib was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Getting a wrapper for mscorlib was successful.");
	}

	CryLogAlways("TEST:");
	TestClassFromAssembly(corlib->Array,     "System.Array");
	TestClassFromAssembly(corlib->Boolean,   "System.Boolean");
	TestClassFromAssembly(corlib->Byte,      "System.Byte");
	TestClassFromAssembly(corlib->Char,      "System.Char");
	TestClassFromAssembly(corlib->Single,    "System.Single");
	TestClassFromAssembly(corlib->Double,    "System.Double");
	TestClassFromAssembly(corlib->Enum,      "System.Enum");
	TestClassFromAssembly(corlib->Exception, "System.Exception");
	TestClassFromAssembly(corlib->Int16,     "System.Int16");
	TestClassFromAssembly(corlib->Int32,     "System.Int32");
	TestClassFromAssembly(corlib->Int64,     "System.Int64");
	TestClassFromAssembly(corlib->UInt16,    "System.UInt16");
	TestClassFromAssembly(corlib->UInt32,    "System.UInt32");
	TestClassFromAssembly(corlib->UInt64,    "System.UInt64");
	TestClassFromAssembly(corlib->IntPtr,    "System.IntPtr");
	TestClassFromAssembly(corlib->UIntPtr,   "System.UIntPtr");
	TestClassFromAssembly(corlib->Sbyte,     "System.Sbyte");
	TestClassFromAssembly(corlib->String,    "System.String");
	TestClassFromAssembly(corlib->Type,      "System.Type");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a wrapper for Cryambly.");
	CryLogAlways("TEST:");

	ICryambly *cryambly = MonoEnv->Cryambly;

	if (!cryambly)
	{
		CryLogAlways("TEST FAILURE: Getting a wrapper for Cryambly was not successful.");
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Getting a wrapper for Cryambly was successful.");
	}

	CryLogAlways("TEST:");
	TestClassFromAssembly(cryambly->Matrix33,    "CryCil.Matrix33");
	TestClassFromAssembly(cryambly->Matrix34,    "CryCil.Matrix34");
	TestClassFromAssembly(cryambly->Matrix44,    "CryCil.Matrix44");
	TestClassFromAssembly(cryambly->AngleAxis,   "CryCil.Geometry.AngleAxis");
	TestClassFromAssembly(cryambly->BoundingBox, "CryCil.Geometry.BoundingBox");
	TestClassFromAssembly(cryambly->ColorByte,   "CryCil.Graphics.ColorByte");
	TestClassFromAssembly(cryambly->ColorSingle, "CryCil.Graphics.ColorSingle");
	TestClassFromAssembly(cryambly->EulerAngles, "CryCil.Geometry.EulerAngles");
	TestClassFromAssembly(cryambly->Plane,       "CryCil.Geometry.Plane");
	TestClassFromAssembly(cryambly->Quaternion,  "CryCil.Geometry.Quaternion");
	TestClassFromAssembly(cryambly->Quatvec,     "CryCil.Geometry.Quatvec");
	TestClassFromAssembly(cryambly->Ray,         "CryCil.Geometry.Ray");
	TestClassFromAssembly(cryambly->Vector2,     "CryCil.Vector2");
	TestClassFromAssembly(cryambly->Vector3,     "CryCil.Vector3");
	TestClassFromAssembly(cryambly->Vector4, "CryCil.Vector4");
	CryLogAlways("TEST:");
}

void TestClassFromAssembly(IMonoClass *klass, const char *name)
{
	CryLogAlways("TEST: Getting a wrapper for a klass.");

	if (!klass)
	{
		CryLogAlways("TEST FAILURE: Getting a wrapper for a klass %s was not successful.", name);
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Getting a wrapper for a klass %s was successful.", name);
	}
}