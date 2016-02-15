#include "stdafx.h"

extern IMonoAssembly *mainTestingAssembly;

void TestMemberLists(IMonoClass *);
void TestInheritance(IMonoClass *);
void TestInterfaceImplementation();
void TestTypeSpecification();
void TestAssemblyLookBack();
void TestConstructors();
void TestMethods();
void TestFields();
void TestProperties();
void TestEvents();

void TestClasses()
{
	TestTypeSpecification();

	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;

	TestMemberLists(vector3Class);

	TestInheritance(vector3Class);

	TestInterfaceImplementation();

	TestAssemblyLookBack();

	TestConstructors();

	TestMethods();

	TestFields();

	TestProperties();

	TestEvents();
}

#pragma region General Tests
inline void TestTypeSpecification()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking whether types that are value-types or enumerations or delegates can be identified as such.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking whether a value-type can be identified as such.");
	CryLogAlways("TEST:");

	IMonoClass *quatClass = MonoEnv->Cryambly->Quaternion;

	if (quatClass->IsValueType)
	{
		CryLogAlways("TEST SUCCESS: Quaternion is identified as a value-type.");
	}
	else
	{
		ReportError("TEST FAILURE: Quaternion is not identified as a value-type.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking whether an enumeration can be identified as such.");
	CryLogAlways("TEST:");

	IMonoClass *enumClass = MonoEnv->CoreLibrary->GetClass("System", "DayOfWeek");

	if (enumClass->IsEnum)
	{
		CryLogAlways("TEST SUCCESS: System.DayOfWeek is identified as an enumeration.");
	}
	else
	{
		ReportError("TEST FAILURE: System.DayOfWeek is not identified as an enumeration.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking whether a delegate can be identified as such.");
	CryLogAlways("TEST:");

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

inline void TestMemberLists(IMonoClass *klass)
{

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking %s.", klass->FullName);
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Printing a list of fields of type %s.", klass->Name);
	CryLogAlways("TEST:");

	auto fields = klass->Fields;

	for (int i = 0; i < fields.Length; i++)
	{
		CryLogAlways("TEST: Field #%d: %s", i, fields[i]->Name);
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Printing a list of properties of type %s.", klass->Name);
	CryLogAlways("TEST:");

	auto properties = klass->Properties;

	CryLogAlways("TEST: Acquired the read-only list of properties.");

	for (int i = 0; i < properties.Length; i++)
	{
		CryLogAlways("TEST: Property #%d: %s", i, properties[i]->Name);
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Printing a list of methods of type %s.", klass->Name);
	CryLogAlways("TEST:");

	auto methods = klass->Functions;

	for (int i = 0; i < methods.Length; i++)
	{
		CryLogAlways("TEST: Method #%d: %s", i, methods[i]->Name);
	}
	CryLogAlways("TEST:");
}

inline void TestInheritance(IMonoClass *klass)
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Finding out about base class of Vector3 class.");
	CryLogAlways("TEST:");

	IMonoClass *baseClass = klass->Base;

	if (!baseClass)
	{
		ReportError("TEST FAILURE: Unable to get base class of %s.", klass->Name);
	}
	else
	{
		CryLogAlways("TEST SUCCESS: Name of the base class of %s is %s.", klass->Name, baseClass->Name);
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing inheritance detection on %s.", klass->Name);
	CryLogAlways("TEST:");

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
						klass->IsValueType ? "FAILURE" : "SUCCESS",
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
						klass->IsValueType ? "FAILURE" : "SUCCESS",
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
	CryLogAlways("TEST:");
}

inline void TestInterfaceImplementation()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking direct interface implementation detection.");
	CryLogAlways("TEST:");

	IMonoClass *vsProjectClass = MonoEnv->Cryambly->GetClass("CryCil.RunTime.Compilation",
															 "VisualStudioDotNetProject");

	IMonoClass *iProjectInterface = MonoEnv->Cryambly->GetClass("CryCil.RunTime.Compilation", "IProject");

	if (vsProjectClass->Implements(iProjectInterface))
	{
		CryLogAlways("TEST SUCCESS: VisualStudioDotNetProject implements IProject.");
	}
	else
	{
		ReportError("TEST FAILURE: VisualStudioDotNetProject is supposed to implement IProject.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking indirect interface implementation detection.");
	CryLogAlways("TEST:");

	IMonoClass *logWriterClass = MonoEnv->Cryambly->GetClass("CryCil.Engine.DebugServices",
															 "ConsoleLogWriter");

	IMonoClass *iDisposableInterface = MonoEnv->CoreLibrary->GetClass("System", "IDisposable");

	if (logWriterClass->Implements(iDisposableInterface))
	{
		CryLogAlways("TEST SUCCESS: ConsoleLogWriter implements IDisposable.");
	}
	else
	{
		ReportError("TEST FAILURE: ConsoleLogWriter is supposed to implement IDisposable.");
	}
	CryLogAlways("TEST:");
}

inline void TestAssemblyLookBack()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking whether IMonoClass::Assembly property returns pointer to correct assembly wrapper.");
	CryLogAlways("TEST:");

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
	CryLogAlways("TEST:");
}
#pragma endregion

#pragma region Constructor Tests
void TestGettingTheConstructors();
void TestObjectCreation();

inline void TestConstructors()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking the constructors.");
	CryLogAlways("TEST:");

	TestGettingTheConstructors();

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Creation of objects using the constructors.");
	CryLogAlways("TEST:");

	TestObjectCreation();
}

inline void TestGettingTheConstructors()
{

	IMonoClass *stringClass = MonoEnv->CoreLibrary->String;

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting constructor using a Mono array of System.Type objects.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Creating an array.");
	CryLogAlways("TEST:");

	auto typesArray = IMonoArray<>(MonoEnv->Objects->Arrays->Create(3, MonoEnv->CoreLibrary->Type));

	CryLogAlways("TEST: Filling the array.");

	typesArray[0] = MonoEnv->CoreLibrary->Sbyte->MakePointerType();

	CryLogAlways("TEST: Added a pointer type of Sbyte to the array.");

	mono::type int32Type = MonoEnv->CoreLibrary->Int32->GetType();
	typesArray[1] = int32Type;
	typesArray[2] = int32Type;

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

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting constructor using a simple list of IMonoClass wrappers.");
	CryLogAlways("TEST:");

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

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting constructor using a list of IMonoClass wrappers with postfix strings.");
	CryLogAlways("TEST:");

	auto klassPostList = List<ClassSpec>(1).Add(ClassSpec(MonoEnv->CoreLibrary->Char, "[]"));

	IMonoConstructor *ctorSpecifiedClassList = stringClass->GetConstructor(klassPostList);

	if (ctorSpecifiedClassList)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting constructor using a list of full type names.");
	CryLogAlways("TEST:");

	auto typeNameList = List<const char *>({ "System.Char[]", "System.Int32", "System.Int32" });

	IMonoConstructor *ctorTypeNameList = stringClass->GetConstructor(typeNameList);

	if (ctorTypeNameList)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting constructor using a text representation of the signature.");
	CryLogAlways("TEST:");

	IMonoConstructor *ctorTextParams =
		stringClass->GetConstructor("System.Char[],System.Int32,System.Int32");

	if (ctorTextParams)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the only String constructor that accepts 4 arguments.");
	CryLogAlways("TEST:");

	IMonoConstructor *ctor4Params = stringClass->GetConstructor(4);

	if (ctor4Params)
	{
		CryLogAlways("TEST SUCCESS: Constructor successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Constructor wasn't acquired.");
	}
	CryLogAlways("TEST:");
}

inline void TestObjectCreation()
{
	void *params[2];

	IMonoClass *ctorTestClass =
		mainTestingAssembly->GetClass("MainTestingAssembly", "ConstructionTestClass");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a default constructor.");
	CryLogAlways("TEST:");

	IMonoConstructor *defaultCtor = ctorTestClass->GetConstructor();

	CryLogAlways("TEST: Invoking a default constructor.");
	CryLogAlways("TEST:");

	defaultCtor->Create();

	CryLogAlways("TEST: Getting a constructor that accepts 2 simple integers.");
	CryLogAlways("TEST:");

	IMonoConstructor *ctor2Integers = ctorTestClass->GetConstructor("System.Int32,System.Int32");

	CryLogAlways("TEST: Invoking a constructor.");
	CryLogAlways("TEST:");

	int par1 = 1000;
	int par2 = 20;
	params[0] = &par1;
	params[1] = &par2;
	ctor2Integers->Create(params);

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a constructor that accepts a Double and a reference to String.");
	CryLogAlways("TEST:");

	auto typesDoubleStringRef = List<ClassSpec>(2);
	typesDoubleStringRef.Add(ClassSpec(MonoEnv->CoreLibrary->Double, ""));
	typesDoubleStringRef.Add(ClassSpec(MonoEnv->CoreLibrary->String, "&"));
	IMonoConstructor *ctorDoubleStringRef = ctorTestClass->GetConstructor(typesDoubleStringRef);

	CryLogAlways("TEST: Invoking a constructor.");
	CryLogAlways("TEST:");

	double doublePar = 14.567;
	mono::string textPar;
	params[0] = &doublePar;
	params[1] = &textPar;
	ctorDoubleStringRef->Create(params);
	CryLogAlways("TEST: Returned string: %s", NtText(textPar).c_str());
	CryLogAlways("TEST:");

	CryLogAlways("TEST: Checking creation of compound objects.");
	CryLogAlways("TEST:");

	const int seed = 100000;

	IMonoClass *randomClass = MonoEnv->CoreLibrary->GetClass("System", "Random");

	CryLogAlways("TEST: Creating a Random with seed %d.", seed);
	CryLogAlways("TEST:");

	void *param;
	param = const_cast<int *>(&seed);

	mono::object randomObject = randomClass->GetConstructor(1)->Create(&param);

	CryLogAlways("TEST: Random object: %d.", randomObject);
	CryLogAlways("TEST:");

	CryLogAlways("TEST: Creating a first component.");
	CryLogAlways("TEST:");

	params[0] = ToMonoString("Some text");
	params[1] = randomObject;
	mono::object firstComponent =
		mainTestingAssembly->GetClass("MainTestingAssembly", "CtorTestComponent1")
		->GetConstructor(2)
		->Create(params);

	CryLogAlways("TEST: Creating a second component.");
	CryLogAlways("TEST:");

	unsigned char bytePar = 1;
	unsigned int lengths[3];
	lengths[0] = 3;
	lengths[1] = 10;
	lengths[2] = 2;
	mono::Array arrayCube = MonoEnv->Objects->Arrays->Create(3, lengths, MonoEnv->CoreLibrary->Int32);

	params[0] = &bytePar;
	params[1] = arrayCube;
	mono::object secondComponent =
		mainTestingAssembly->GetClass("MainTestingAssembly", "CtorTestComponent2")
		->GetConstructor(2)
		->Create(params);

	CryLogAlways("TEST: Creating a compound object.");
	CryLogAlways("TEST:");

	IMonoClass *compoundClass = mainTestingAssembly->GetClass("MainTestingAssembly", "CtorTestCompound");

	params[0] = firstComponent;
	params[1] = secondComponent;

	mono::object compound = compoundClass->GetConstructor(2)->Create(params);

	CryLogAlways("TEST: Print contents of the compound object.");
	CryLogAlways("TEST:");

	compoundClass->GetFunction("PrintStuff")->ToInstance()->Invoke(compound);
	CryLogAlways("TEST:");
}
#pragma endregion

#pragma region Method Tests
void TestGettingMethods();
void TestInvokingMethods();

inline void TestMethods()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Checking the methods.");
	CryLogAlways("TEST:");

	TestGettingMethods();

	TestInvokingMethods();
}

inline void TestGettingMethods()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the methods.");
	CryLogAlways("TEST:");

	IMonoClass *mathClass = MonoEnv->CoreLibrary->GetClass("System", "Math");
	IMonoClass *typeClass = MonoEnv->CoreLibrary->Type;
	IMonoClass *stringClass = MonoEnv->CoreLibrary->String;
	IMonoClass *arrayClass = MonoEnv->CoreLibrary->Array;

	CryLogAlways("TEST: Getting the method using an array of System.Type objects.");
	CryLogAlways("TEST:");

	auto typesArray = IMonoArray<>(MonoEnv->Objects->Arrays->Create(1, typeClass));

	typesArray[0] = typeClass->MakeArrayType();

	IMonoMethod *method = typeClass->GetFunction("GetConstructor", typesArray)->ToInstance();

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the method using a list of IMonoClass objects.");
	CryLogAlways("TEST:");

	auto classes = List<IMonoClass *>({ stringClass, arrayClass });

	method = typeClass->GetFunction("GetMethod", classes)->ToInstance();

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the method using a list of IMonoClass objects with post-fixes.");
	CryLogAlways("TEST:");

	auto specifiedClasses = List<ClassSpec>(3).Add({ ClassSpec(stringClass, ""), ClassSpec(typeClass, ""),
												   ClassSpec(typeClass, "[]") });

	method = typeClass->GetFunction("GetProperty", specifiedClasses)->ToInstance();

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the method using a list of type names.");
	CryLogAlways("TEST:");

	auto typeNames = List<const char *>({ "System.Double", "System.Double" });

	auto staticFunc = mathClass->GetFunction("Pow", typeNames)->ToStatic();

	if (staticFunc)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the method using a text representation of the signature.");
	CryLogAlways("TEST:");

	staticFunc = mathClass->GetFunction("Min", "System.UInt32,System.UInt32")->ToStatic();

	if (staticFunc)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the method using a number of parameters.");
	CryLogAlways("TEST:");

	staticFunc = MonoEnv->CoreLibrary->GetClass("System", "GC")->GetFunction("Collect", 1)->ToStatic();

	if (staticFunc)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a list of methods using just a name.");
	CryLogAlways("TEST:");

	auto funcs = mathClass->GetFunctions("Min");

	if (funcs)
	{
		CryLogAlways("TEST SUCCESS: Got a list of Min method overloads.");

		CryLogAlways("TEST: A list of methods:");

		for (int i = 0; i < funcs->Length; i++)
		{
			CryLogAlways("%d) %s::%s(%s);",
						 i + 1, mathClass->FullName, funcs->At(i)->Name, funcs->At(i)->Parameters);
		}

		delete funcs;
	}
	else
	{
		ReportError("TEST FAILURE: Couldn't get a list of Min method overloads.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a list of methods using a name and a number of parameters.");
	CryLogAlways("TEST:");

	funcs = mathClass->GetFunctions("Round", 2);

	if (funcs)
	{
		CryLogAlways("TEST SUCCESS: Got a list of Round method overloads.");

		CryLogAlways("TEST: A list of methods:");

		for (int i = 0; i < funcs->Length; i++)
		{
			CryLogAlways("%d) %s::%s(%s);",
						 i + 1, mathClass->FullName, funcs->At(i)->Name, funcs->At(i)->Parameters);
		}

		delete funcs;
	}
	else
	{
		ReportError("TEST FAILURE: Couldn't get a list of Round method overloads.");
	}
	CryLogAlways("TEST:");
}

void TestStaticMethodInvocation();
void TestInstanceMethodInvocation();
void TestVirtualMethodInvocation();
void TestStaticThunkInvocation();
void TestInstanceThunkInvocation();

inline void PrintDigitsArray(mono::Array digits)
{
	auto digitsArray = IMonoArray<int>(digits);

	TextBuilder digitsText(30);
	char symbol[2];

	digitsText << itoa(digitsArray[0], &symbol[0], 10);
	for (int i = 1; i < digitsArray.Length; i++)
	{
		digitsText << " ";
		digitsText << itoa(digitsArray[i], &symbol[0], 10);
	}

	const char *ntText = digitsText.ToNTString();
	CryLogAlways("TEST: Digits are: %s", ntText);
	delete ntText;
}

inline void TestInvokingMethods()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Invoking the methods.");
	CryLogAlways("TEST:");

	TestStaticMethodInvocation();

	TestInstanceMethodInvocation();

	TestVirtualMethodInvocation();

	TestStaticThunkInvocation();

	TestInstanceThunkInvocation();
}

inline void TestStaticMethodInvocation()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Invoking static methods through IMonoStaticMethod.");
	CryLogAlways("TEST:");

	IMonoClass *lerpClass = MonoEnv->Cryambly->GetClass("CryCil", "Interpolation")->GetNestedType("Linear");
	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;
	IMonoClass *singleClass = MonoEnv->CoreLibrary->Single;

	CryLogAlways("TEST: Getting a method that accepts a reference to value-type object.");
	CryLogAlways("TEST:");

	auto specClasses = List<ClassSpec>(4);
	specClasses.Add(ClassSpec(vector3Class, "&"));
	specClasses.Add(ClassSpec(vector3Class, ""));
	specClasses.Add(ClassSpec(vector3Class, ""));
	specClasses.Add(ClassSpec(singleClass, ""));

	IMonoStaticMethod *staticMethod = lerpClass->GetFunction("Apply", specClasses)->ToStatic();

	CryLogAlways("TEST: Invoking a method.");
	CryLogAlways("TEST:");

	Vec3 res;
	Vec3 start(0, 0, 0);
	Vec3 end(100, 100, 100);
	float parameter = 0.5;

	void *params4[4];
	params4[0] = &res;
	params4[1] = &start;
	params4[2] = &end;
	params4[3] = &parameter;

	staticMethod->Invoke(params4);

	CryLogAlways("Result of interpolation: (%f, %f, %f).", res.x, res.y, res.z);

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting a method that accepts a reference to an array.");
	CryLogAlways("TEST:");

	staticMethod = mainTestingAssembly->GetClass("MainTestingAssembly", "MethodTestClass")
		->GetFunction("NumberToDigits", 2)->ToStatic();

	CryLogAlways("TEST: Invoking a method.");
	CryLogAlways("TEST:");

	mono::Array digits;
	int number = 123456;

	void *params2[2];
	params2[0] = &number;
	params2[1] = &digits;

	if (Unbox<bool>(staticMethod->Invoke(params2)))
	{
		PrintDigitsArray(digits);
	}
	CryLogAlways("TEST:");
}

inline void TestInstanceMethodInvocation()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Invoking instance methods through IMonoMethod.");
	CryLogAlways("TEST:");

	IMonoClass *stringClass = MonoEnv->CoreLibrary->String;

	mono::string text = ToMonoString("Some text for testing.");

	MonoGCHandle handle = MonoEnv->GC->Pin(text);

	CryLogAlways("TEST: Invocation of the method that accepts 4 arguments and returns an signed integer.");
	CryLogAlways("TEST:");

	IMonoMethod *indexOfMethod = stringClass->GetFunction("IndexOf", 4)->ToInstance();

	wchar_t symbol = L't';
	int searchStart = 5;
	int searchSize = 7;
	int stringComparison = 2;			// Invariant culture.

	void *params[4];
	params[0] = &symbol;
	params[1] = &searchStart;
	params[2] = &searchSize;
	params[3] = &stringComparison;

	int index = Unbox<int>(indexOfMethod->Invoke(text, params));

	CryLogAlways("TEST: Index of the first 't' symbol in the test string within range [5; 12] = %d", index);
	CryLogAlways("TEST:");
}

inline void TestVirtualMethodInvocation()
{
	mono::string text = ToMonoString("Some text");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Invoking virtual methods through IMonoMethod.");
	CryLogAlways("TEST:");

	IMonoClass *objectClass = MonoEnv->CoreLibrary->String;

	CryLogAlways("TEST: Getting the method wrapper.");
	CryLogAlways("TEST:");

	IMonoMethod *toStringFormatMethod = objectClass->GetFunction("ToString", 0)->ToInstance();

	CryLogAlways("TEST: Invoking a virtual method using early binding.");
	CryLogAlways("TEST:");

	auto result = NtText(toStringFormatMethod->Invoke(text));

	CryLogAlways("TEST: Result of early-bound invocation: %s.", result.c_str());

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Invoking a virtual method using late binding.");
	CryLogAlways("TEST:");

	result = NtText(toStringFormatMethod->Invoke(text, nullptr, true));

	CryLogAlways("TEST: Result of late-bound invocation: %s.", result.c_str());
	CryLogAlways("TEST:");
}

struct TestObject
{
	int Number;
	mono::string Text;
};

inline void TestStaticThunkInvocation()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Invoking static methods through unmanaged thunk.");
	CryLogAlways("TEST:");

	IMonoClass *methodTestClass = mainTestingAssembly->GetClass("MainTestingAssembly", "MethodTestClass");

	CryLogAlways("TEST: Getting the method wrapper of NumberToDigits.");
	CryLogAlways("TEST:");

	IMonoFunction *method = methodTestClass->GetFunction("NumberToDigits", 2);

	CryLogAlways("TEST: Getting the unmanaged thunk.");
	CryLogAlways("TEST:");

	bool(__stdcall *numberToDigitsThunk)(int *, mono::Array *, mono::exception *) =
		reinterpret_cast<bool(__stdcall *)(int *, mono::Array *, mono::exception *)>(method->UnmanagedThunk);

	CryLogAlways("TEST: Invoking the unmanaged thunk.");
	CryLogAlways("TEST:");

	int number = 123456;
	mono::Array digits;
	mono::exception ex;

	bool success = numberToDigitsThunk(&number, &digits, &ex);

	if (!ex && success)
	{
		PrintDigitsArray(digits);
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the method wrapper of CreateValueTypeObject.");
	CryLogAlways("TEST:");

	method = methodTestClass->GetFunction("CreateValueTypeObject", 2);

	CryLogAlways("TEST: Getting the unmanaged thunk.");
	CryLogAlways("TEST:");

	void(__stdcall *createValueThunk)(int, mono::object, mono::exception *) =
		reinterpret_cast<void(__stdcall *)(int, mono::object, mono::exception *)>(method->UnmanagedThunk);

	CryLogAlways("TEST: Invoking the unmanaged thunk.");
	CryLogAlways("TEST:");

	TestObject tObj;
	mono::object testObj = mainTestingAssembly->GetClass("MainTestingAssembly", "CustomValueType")->Box(&tObj);

	createValueThunk(number, testObj, &ex);

	auto unboxedObj = Unbox<TestObject>(testObj);
	CryLogAlways("TEST: Result of invocation = %s", NtText(unboxedObj.Text).c_str());
	CryLogAlways("TEST:");
}

typedef float(__stdcall *GetDistanceThunk)(mono::object, mono::object, mono::exception *);

inline void TestInstanceThunkInvocation()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Invoking instance methods through unmanaged thunk.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the method wrapper of Vector3::GetDistance(CryCil.Vector3).");
	CryLogAlways("TEST:");

	IMonoFunction *method = MonoEnv->Cryambly->Vector3->GetFunction("GetDistance", "CryCil.Vector3");

	CryLogAlways("TEST: Getting the unmanaged thunk.");
	CryLogAlways("TEST:");

	GetDistanceThunk thunk = GetDistanceThunk(method->UnmanagedThunk);

	CryLogAlways("TEST: Invoking the unmanaged thunk.");
	CryLogAlways("TEST:");

	mono::exception ex;
	float result = thunk(Box(Vec3(0, 0, 1)), Box(Vec3(1, 1, 1)), &ex);

	if (!ex)
	{
		CryLogAlways("TEST: Result of invocation = %f", result);
	}
	CryLogAlways("TEST:");
}
#pragma endregion

#pragma region Field Tests
void TestInstanceFields();
void TestStaticFields();

inline void TestFields()
{
	TestInstanceFields();

	TestStaticFields();
}

inline void TestInstanceFields()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing instance fields.");
	CryLogAlways("TEST:");

	IMonoClass *fieldTestClass = mainTestingAssembly->GetClass("MainTestingAssembly", "FieldTestClass");

	CryLogAlways("TEST: Creating the object.");
	CryLogAlways("TEST:");

	int number = 12000;
	mono::string text = ToMonoString("Something texty.");

	void *params[2];
	params[0] = &number;
	params[1] = text;

	mono::object obj = fieldTestClass->GetConstructor(2)->Create(params);

	MonoGCHandle handle = MonoEnv->GC->Pin(obj);

	CryLogAlways("TEST: Getting the values of fields.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the wrappers for both fields.");
	CryLogAlways("TEST:");

	IMonoField *numberField = fieldTestClass->GetField("Number");
	IMonoField *textField = fieldTestClass->GetField("Text");

	CryLogAlways("TEST: Getting the number field value through the wrapper.");
	CryLogAlways("TEST:");

	int numberValue;
	numberField->Get(obj, &numberValue);

	CryLogAlways("TEST: Acquired value = %d", numberValue);

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the text value through the name.");
	CryLogAlways("TEST:");

	mono::string textValue;
	fieldTestClass->GetField(obj, "Text", &textValue);

	CryLogAlways("TEST: Acquired text value = %s", NtText(textValue).c_str());

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Setting the values of fields.");
	CryLogAlways("TEST:");

	CryLogAlways("TEST: Setting the text field value using the wrapper.");

	textValue = ToMonoString("Some different text.");
	fieldTestClass->SetField(obj, textField, textValue);

	CryLogAlways("TEST: Setting the number field value through the name.");
	CryLogAlways("TEST:");

	numberValue = 9999;
	fieldTestClass->SetField(obj, "Number", &numberValue);

	CryLogAlways("TEST: Getting the values of fields.");
	CryLogAlways("TEST:");

	CryLogAlways("TEST: Getting the number field value using the wrapper.");
	CryLogAlways("TEST:");

	fieldTestClass->GetField(obj, numberField, &numberValue);

	CryLogAlways("TEST: Acquired value = %d", numberValue);

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Getting the text value through the wrapper.");
	CryLogAlways("TEST:");

	textField->Get(obj, &textValue);

	CryLogAlways("TEST: Acquired text value = %s", NtText(textValue).c_str());
	CryLogAlways("TEST:");
}

inline void TestStaticFields()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing static fields.");
	CryLogAlways("TEST:");

	IMonoClass *fieldTestClass = mainTestingAssembly->GetClass("MainTestingAssembly", "FieldTestClass");

	int number = 12000;
	mono::string text = ToMonoString("Something texty.");

	void *params[2];
	params[0] = &number;
	params[1] = text;

	mono::object obj = fieldTestClass->GetConstructor(2)->Create(params);

	MonoGCHandle handle = MonoEnv->GC->Pin(obj);

	CryLogAlways("TEST: Setting a static field.");
	CryLogAlways("TEST:");

	IMonoField *field = fieldTestClass->GetField("ObjectField");

	fieldTestClass->SetField(nullptr, field, obj);

	CryLogAlways("TEST: Getting a static field.");
	CryLogAlways("TEST:");

	fieldTestClass->GetField(nullptr, field, &obj);

	number = fieldTestClass->GetField<int>(obj, "Number");
	text = fieldTestClass->GetField<mono::string>(obj, "Text");

	CryLogAlways("TEST: Numeric value = %d", number);

	CryLogAlways("TEST: Text value = %s", NtText(text).c_str());
	CryLogAlways("TEST:");
}

#pragma endregion

#pragma region Property Tests

inline void TestProperties()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing property wrappers.");
	CryLogAlways("TEST:");

	IMonoClass *gcClass = MonoEnv->CoreLibrary->GetClass("System", "GC");
	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;

	CryLogAlways("TEST: Testing instance properties.");
	CryLogAlways("TEST:");

	IMonoProperty *lengthProp = vector3Class->GetProperty("Length");

	Vec3 vector(10.0f, 30.0f, -15.0f);

	IMonoMethod *lengthGetter = lengthProp->Getter->ToInstance();

	CryLogAlways("TEST: Length of the vector (10, 30, -15) = %f", Unbox<float>(lengthGetter->Invoke(&vector)));

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing indexers.");
	CryLogAlways("TEST:");

	IMonoProperty *indexerProp = vector3Class->GetProperty("Item", 1);

	IMonoMethod *indexerGetter = indexerProp->Getter->ToInstance();

	int index = 1;
	void *param = &index;
	float yComponent = Unbox<float>(indexerGetter->Invoke(&vector, &param));

	CryLogAlways("TEST: Y-coordinate of the vector = %f", yComponent);
	CryLogAlways("TEST:");

	IMonoMethod *indexerSetter = indexerProp->Setter->ToInstance();

	index = 2;
	float coord = 90;
	void *params[2];
	params[0] = &index;
	params[1] = &coord;
	indexerSetter->Invoke(&vector, params);

	CryLogAlways("TEST: Length of the vector after setting z-coordinate to 90 = %f", Unbox<float>(lengthGetter->Invoke(&vector)));

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing static properties.");
	CryLogAlways("TEST:");

	IMonoProperty *maxGenProp = gcClass->GetProperty("MaxGeneration");
	IMonoStaticMethod *maxGenGetter = maxGenProp->Getter->ToStatic();

	CryLogAlways("TEST: Max generation of the GC: %d", Unbox<int>(maxGenGetter->Invoke()));
	CryLogAlways("TEST:");
}

#pragma endregion

#pragma region Event Tests

inline void UnmanagedEventHandler(mono::object, mono::object)
{
	CryLogAlways("TEST: Unmanaged event wrapper has been invoked.");
}

void TestInstanceEvent(mono::object obj, IMonoEvent *_event);
void TestStaticEvent(IMonoEvent *_event);

IMonoClass *eventHandlerClass;

inline void TestEvents()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing events.");
	CryLogAlways("TEST:");

	eventHandlerClass = MonoEnv->CoreLibrary->GetClass("System", "EventHandler");

	IMonoClass *eventTestClass = mainTestingAssembly->GetClass("Test", "EventTest");
	IMonoClass *eventTestClassClass = mainTestingAssembly->GetClass("Test", "EventTestClass");
	IMonoClass *eventTestObjectClass = mainTestingAssembly->GetClass("Test", "EventTestObject");

	CryLogAlways("TEST: Got all class wrappers for testing events.");
	CryLogAlways("TEST:");

	mono::object eventTestObj = eventTestClass->GetFunction("Setup", 0)->ToStatic()->Invoke();

	MonoGCHandle handle = MonoEnv->GC->Pin(eventTestObj);

	CryLogAlways("TEST: Created an object for testing events.");
	CryLogAlways("TEST:");

	CryLogAlways("TEST: Testing instance events.");
	CryLogAlways("TEST:");

	TestInstanceEvent(eventTestObj, eventTestObjectClass->GetEvent("Testing"));
	TestInstanceEvent(eventTestObj, eventTestObjectClass->GetEvent("Tested"));

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing static events.");
	CryLogAlways("TEST:");

	TestStaticEvent(eventTestClassClass->GetEvent("Testing"));
	TestStaticEvent(eventTestClassClass->GetEvent("Tested"));

	CryLogAlways("TEST:");
}

inline void TestInstanceEvent(mono::object obj, IMonoEvent *_event)
{
	CryLogAlways("TEST: Testing an instance event %s::%s.", _event->DeclaringClass->FullName, _event->Name);

	IMonoMethod *_eventAdd = _event->Add->ToInstance();
	IMonoMethod *_eventRemove = _event->Remove->ToInstance();
	IMonoMethod *_eventRaise = _event->Raise->ToInstance();

	CryLogAlways("TEST: Raising the event %s for the first time.");

	_eventRaise->Invoke(obj);

	CryLogAlways("TEST: Adding a delegate to the event that encapsulates an unmanaged function.");

	mono::object fnPtrWrapper =
		MonoEnv->Objects->Delegates->Create(eventHandlerClass, UnmanagedEventHandler);

	MonoGCHandle handle = MonoEnv->GC->Pin(fnPtrWrapper);

	void *param = fnPtrWrapper;
	_eventAdd->Invoke(obj, &param);

	CryLogAlways("TEST: Raising the event %s after adding unmanaged function delegate.");

	_eventRaise->Invoke(obj);

	CryLogAlways("TEST: Removing the delegate from the event's invocation list.");

	_eventRemove->Invoke(&param);

	CryLogAlways("TEST: Raising the event %s after removing previously added delegate.");

	_eventRaise->Invoke(obj);
}
inline void TestStaticEvent(IMonoEvent *_event)
{
	CryLogAlways("TEST: Testing a static event %s::%s.", _event->DeclaringClass->FullName, _event->Name);

	IMonoStaticMethod *_eventAdd = _event->Add->ToStatic();
	IMonoStaticMethod *_eventRemove = _event->Remove->ToStatic();
	IMonoStaticMethod *_eventRaise = _event->Raise->ToStatic();

	CryLogAlways("TEST: Raising the event %s for the first time.");

	_eventRaise->Invoke();

	CryLogAlways("TEST: Adding a delegate to the event that encapsulates an unmanaged function.");

	mono::object fnPtrWrapper =
		MonoEnv->Objects->Delegates->Create(eventHandlerClass, UnmanagedEventHandler);

	MonoGCHandle handle = MonoEnv->GC->Pin(fnPtrWrapper);

	void *param = fnPtrWrapper;
	_eventAdd->Invoke(&param);

	CryLogAlways("TEST: Raising the event %s after adding unmanaged function delegate.");

	_eventRaise->Invoke();

	CryLogAlways("TEST: Removing the delegate from the event's invocation list.");

	_eventRemove->Invoke(&param);

	CryLogAlways("TEST: Raising the event %s after removing previously added delegate.");

	_eventRaise->Invoke();
}

#pragma endregion