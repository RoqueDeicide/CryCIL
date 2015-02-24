#pragma once

#include "IMonoInterface.h"

void TestMemberLists(IMonoClass *);
void TestInheritance(IMonoClass *);
void TestInterfaceImplementation();
void TestTypeSpecification();
void TestAssemblyLookBack();
void TestConstructors();

void TestClasses()
{
	CryLogAlways("TEST: Checking whether types that are value-types or enumerations or delegates can be identified as such.");

	TestTypeSpecification();

	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;

	CryLogAlways("TEST: Checking %s.", vector3Class->FullName);

	TestMemberLists(vector3Class);

	CryLogAlways("TEST: Finding out about base class of Vector3 class.");

	TestInheritance(vector3Class);

	TestInterfaceImplementation();
	
	CryLogAlways("TEST: Checking whether IMonoClass::Assembly property returns pointer to correct assembly wrapper.");

	TestAssemblyLookBack();

	TestConstructors();
}

void TestTypeSpecification()
{
	CryLogAlways("TEST: Checking whether a value-type can be identified as such.");

	IMonoClass *quatClass = MonoEnv->Cryambly->Quaternion;

	if (quatClass->IsValueType)
	{
		CryLogAlways("TEST SUCCESS: Quaternion is identified as a value-type.");
	}
	else
	{
		ReportError("TEST FAILURE: Quaternion is not identified as a value-type.");
	}

	CryLogAlways("TEST: Checking whether an enumeration can be identified as such.");

	IMonoClass *enumClass = MonoEnv->CoreLibrary->GetClass("System", "DayOfWeek");

	if (enumClass->IsEnum)
	{
		CryLogAlways("TEST SUCCESS: System.DayOfWeek is identified as an enumeration.");
	}
	else
	{
		ReportError("TEST FAILURE: System.DayOfWeek is not identified as an enumeration.");
	}

	CryLogAlways("TEST: Checking whether a delegate can be identified as such.");

	IMonoClass *delegateClass = MonoEnv->CoreLibrary->GetClass("System", "Action");

	if (delegateClass->IsDelegate)
	{
		CryLogAlways("TEST SUCCESS: System.Action is identified as a delegate.");
	}
	else
	{
		ReportError("TEST FAILURE: System.Action is not identified as a delegate.");
	}
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

void TestAssemblyLookBack()
{
	IMonoClass *quatvecClass = MonoEnv->Cryambly->Quatvec;

	if (quatvecClass->Assembly == MonoEnv->Cryambly)
	{
		CryLogAlways("TEST SUCCESS: Quatvec struct was identified as one defined in Cryambly.");
	}
	else
	{
		ReportError("TEST FAILURE: Quatvec struct was not identified as one defined in Cryambly.");
	}

	IMonoClass *int32Class = MonoEnv->CoreLibrary->Int32;

	if (int32Class->Assembly == MonoEnv->CoreLibrary)
	{
		CryLogAlways("TEST SUCCESS: Int32 struct was identified as one defined in mscorlib.");
	}
	else
	{
		ReportError("TEST FAILURE: Int32 struct was not identified as one defined in mscorlib.");
	}
}

void TestConstructors()
{
	CryLogAlways("TEST: Checking the constructors.");

	IMonoClass *stringClass = MonoEnv->CoreLibrary->String;

	CryLogAlways("TEST: Getting constructor using a Mono array of System.Type objects.");

	CryLogAlways("TEST: Creating an array.");

	IMonoArray *typesArray = MonoEnv->Objects->Arrays->Create(3, MonoEnv->CoreLibrary->Type);

	CryLogAlways("TEST: Filling the array.");

	typesArray->At<mono::type>(0) = MonoEnv->CoreLibrary->Sbyte->MakePointerType();

	CryLogAlways("TEST: Added a pointer type of Sbyte to the array.");

	mono::type int32Type = MonoEnv->CoreLibrary->Int32->GetType();
	typesArray->At<mono::type>(0) = int32Type;
	typesArray->At<mono::type>(0) = int32Type;

	CryLogAlways("TEST: Added Int32 types to the array.");

	CryLogAlways("TEST: Getting a String constructor that accepts [sbyte *], [int] and [int].");

	IMonoConstructor *ctorTypes = stringClass->GetConstructor(typesArray);

	if (ctorTypes)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST: Getting constructor using a simple list of IMonoClass wrappers.");

	List<IMonoClass *> klassList = List<IMonoClass *>(2);

	klassList.Add(MonoEnv->CoreLibrary->Char);
	klassList.Add(MonoEnv->CoreLibrary->Int32);

	IMonoConstructor *ctorSimpleList = stringClass->GetConstructor(klassList);

	if (ctorSimpleList)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST: Getting constructor using a list of IMonoClass wrappers with postfix strings.");

	auto klassPostList = List<Pair<IMonoClass *, const char *>>(1);

	klassPostList.Add(Pair<IMonoClass *, const char *>(MonoEnv->CoreLibrary->Char, "[]"));

	IMonoConstructor *ctorSpecifiedClassList = stringClass->GetConstructor(klassPostList);

	if (ctorSpecifiedClassList)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST: Getting constructor using a list of full type names.");

	auto typeNameList = List<const char *>(3);

	typeNameList.Add("System.Char[]");
	typeNameList.Add("System.Int32");
	typeNameList.Add("System.Int32");

	IMonoConstructor *ctorTypeNameList = stringClass->GetConstructor(typeNameList);

	if (ctorTypeNameList)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST: Getting constructor using a text representation of the signature.");

	IMonoConstructor *ctorTextParams =
		stringClass->GetConstructor("System.Char[],System.Int32");

	if (ctorTextParams)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST: Getting the only String constructor that accepts 4 arguments.");

	IMonoConstructor *ctor4Params = stringClass->GetConstructor(4);

	if (ctor4Params)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST: Creation of objects using the constructors.");

	IMonoClass *ctorTestClass =
		mainTestingAssembly->GetClass("MainTestingAssembly", "ConstructionTestClass");

	CryLogAlways("TEST: Getting a default constructor.");

	IMonoConstructor *defaultCtor = ctorTestClass->GetConstructor();

	CryLogAlways("TEST: Invoking a default constructor.");

	defaultCtor->Invoke(nullptr);

	CryLogAlways("TEST: Getting a constructor that accepts 2 simple integers.");

	IMonoConstructor *ctor2Integers = ctorTestClass->GetConstructor("System.Int32,System.Int32");

	CryLogAlways("TEST: Invoking a constructor.");

	int par1 = 1000;
	int par2 = 20;
	void *params[2];
	params[0] = &par1;
	params[1] = &par2;
	ctor2Integers->Invoke(nullptr, params);

	CryLogAlways("TEST: Getting a constructor that accepts a Double and a reference to String.");

	auto typesDoubleStringRef = List<Pair<IMonoClass *, const char *>>(2);
	typesDoubleStringRef.Add(Pair<IMonoClass *, const char *>(MonoEnv->CoreLibrary->Double, ""));
	typesDoubleStringRef.Add(Pair<IMonoClass *, const char *>(MonoEnv->CoreLibrary->String, "&"));
	IMonoConstructor *ctorDoubleStringRef = ctorTestClass->GetConstructor(typesDoubleStringRef);

	CryLogAlways("TEST: Invoking a constructor.");

	double doublePar = 14.567;
	mono::string textPar;
	void *doubleStringRefParams[2];
	doubleStringRefParams[0] = &doublePar;
	doubleStringRefParams[1] = &textPar;
	ctorDoubleStringRef->Invoke(nullptr, doubleStringRefParams);

	CryLogAlways("TEST: Checking creation of compound objects.");

	const int seed = 100000;

	IMonoClass *randomClass = MonoEnv->CoreLibrary->GetClass("System", "Random");

	CryLogAlways("TEST: Creating a Random with seed %d.", seed);

	void *param;
	param = (int *)(&seed);

	mono::object randomObject = randomClass->GetConstructor(1)->Invoke(nullptr, &param);

	CryLogAlways("TEST: Creating a first component.");

	params[0] = ToMonoString("Some text");
	params[1] = randomObject;
	mono::object firstComponent =
		mainTestingAssembly->GetClass("MainTestingAssembly", "CtorTestComponent1")
						   ->GetConstructor(2)
						   ->Invoke(nullptr, params);

	CryLogAlways("TEST: Creating a second component.");

	unsigned char bytePar = 1;
	unsigned int lengths[3];
	lengths[0] = 3;
	lengths[1] = 10;
	lengths[2] = 2;
	IMonoArray *arrayCube = MonoEnv->Objects->Arrays->Create(3, lengths, MonoEnv->CoreLibrary->Int32);

	params[0] = ToMonoString("Some text");
	params[1] = arrayCube->GetHandle<mono::Array>();
	mono::object secondComponent =
		mainTestingAssembly->GetClass("MainTestingAssembly", "CtorTestComponent2")
						   ->GetConstructor(2)
						   ->Invoke(nullptr, params);

	CryLogAlways("TEST: Creating a compound object.");

	IMonoClass *compoundClass = mainTestingAssembly->GetClass("MainTestingAssembly", "CtorTestCompound");

	params[0] = firstComponent;
	params[1] = secondComponent;

	mono::object compound = compoundClass->GetConstructor(2)->Invoke(nullptr, params);

	CryLogAlways("TEST: Print contents of the compound object.");

	compoundClass->GetMethod("PrintStuff")->Invoke(compound);
}