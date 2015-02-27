#pragma once

#include "IMonoInterface.h"

extern IMonoAssembly *mainTestingAssembly;

void TestMemberLists(IMonoClass *);
void TestInheritance(IMonoClass *);
void TestInterfaceImplementation();
void TestTypeSpecification();
void TestAssemblyLookBack();
void TestConstructors();
void TestMethods();
void TestFields();

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

	TestMethods();

	TestFields();
}

#pragma region General Tests
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
#pragma endregion

#pragma region Constructor Tests
void TestGettingTheConstructors();
void TestObjectCreation();

void TestConstructors()
{
	CryLogAlways("TEST: Checking the constructors.");

	TestGettingTheConstructors();

	CryLogAlways("TEST: Creation of objects using the constructors.");

	TestObjectCreation();
}

void TestGettingTheConstructors()
{

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
}

void TestObjectCreation()
{
	void *params[2];

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
	params[0] = &doublePar;
	params[1] = &textPar;
	ctorDoubleStringRef->Invoke(nullptr, params);

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
#pragma endregion

#pragma region Method Tests
void TestGettingMethods();
void TestInvokingMethods();

void TestMethods()
{
	CryLogAlways("TEST: Checking the methods.");

	TestGettingMethods();

	TestInvokingMethods();
}

void TestGettingMethods()
{
	CryLogAlways("TEST: Getting the methods.");

	IMonoClass *mathClass   = MonoEnv->CoreLibrary->GetClass("System", "Math");
	IMonoClass *typeClass   = MonoEnv->CoreLibrary->Type;
	IMonoClass *stringClass = MonoEnv->CoreLibrary->String;
	IMonoClass *arrayClass  = MonoEnv->CoreLibrary->Array;

	CryLogAlways("TEST: Getting the method using an array of System.Type objects.");

	IMonoArray *typesArray = MonoEnv->Objects->Arrays->Create(1, typeClass);

	typesArray->At<mono::type>(0) = typeClass->MakeArrayType();

	IMonoMethod *method = typeClass->GetMethod("GetConstructor", typesArray);

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST: Getting the method using a list of IMonoClass objects.");

	auto classes = List<IMonoClass *>(2);

	classes.Add(stringClass);
	classes.Add(arrayClass);

	method = typeClass->GetMethod("GetMethod", classes);

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST: Getting the method using a list of IMonoClass objects with post-fixes.");

	auto specifiedClasses = List<Pair<IMonoClass *, const char *>>(3);

	specifiedClasses.Add(Pair<IMonoClass *, const char *>(stringClass, ""));
	specifiedClasses.Add(Pair<IMonoClass *, const char *>(typeClass,   ""));
	specifiedClasses.Add(Pair<IMonoClass *, const char *>(typeClass, "[]"));

	method = typeClass->GetMethod("GetProperty", specifiedClasses);

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST: Getting the method using a list of type names.");

	auto typeNames = List<const char *>(2);

	typeNames.Add("System.Double");
	typeNames.Add("System.Double");

	method = mathClass->GetMethod("Pow", typeNames);

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST: Getting the method using a text representation of the signature.");

	method = mathClass->GetMethod("Min", "System.UInt32,System.UInt32");

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST: Getting the method using a number of parameters.");

	method = MonoEnv->CoreLibrary->GetClass("System", "GC")->GetMethod("Collect", 1);

	if (method)
	{
		CryLogAlways("TEST SUCCESS: Method has been successfully acquired.");
	}
	else
	{
		ReportError("TEST FAILURE: Method wasn't acquired.");
	}

	CryLogAlways("TEST: Getting a list of methods using just a name.");

	int count;
	auto methods = mathClass->GetMethods("Min", count);

	if (methods)
	{
		CryLogAlways("TEST SUCCESS: Got a list of Min method overloads.");

		CryLogAlways("TEST: A list of methods:");

		for (int i = 0; i < count; i++)
		{
			CryLogAlways("%d) %s::%s(%s);", i + 1, mathClass->FullName, methods[i]->Name, methods[i]->Parameters);
		}
		
		delete methods;
	}
	else
	{
		ReportError("TEST FAILURE: Couldn't get a list of Min method overloads.");
	}

	CryLogAlways("TEST: Getting a list of methods using a name and a number of parameters.");

	methods = mathClass->GetMethods("Round", 2, count);

	if (methods)
	{
		CryLogAlways("TEST SUCCESS: Got a list of Round method overloads.");

		CryLogAlways("TEST: A list of methods:");

		for (int i = 0; i < count; i++)
		{
			CryLogAlways("%d) %s::%s(%s);", i + 1, mathClass->FullName, methods[i]->Name, methods[i]->Parameters);
		}

		delete methods;
	}
	else
	{
		ReportError("TEST FAILURE: Couldn't get a list of Round method overloads.");
	}
}

void TestStaticMethodInvocation();
void TestInstanceMethodInvocation();
void TestVirtualMethodInvocation();
void TestStaticThunkInvocation();
void TestInstanceThunkInvocation();

void PrintDigitsArray(mono::Array digits)
{
	IMonoArray *digitsArray = MonoEnv->Objects->Arrays->Wrap(digits);

	ConstructiveText digitsText(30);
	char symbol[2];

	digitsText << itoa(digitsArray->At<int>(0), &symbol[0], 10);
	for (int i = 1; i < digitsArray->Length; i++)
	{
		digitsText << " ";
		digitsText << itoa(digitsArray->At<int>(i), &symbol[0], 10);
	}

	const char *ntText = digitsText.ToNTString();
	CryLogAlways("TEST: Digits are: %s", ntText);
	delete ntText;
}

void TestInvokingMethods()
{
	CryLogAlways("TEST: Invoking the methods.");

	TestStaticMethodInvocation();
	
	TestInstanceMethodInvocation();
	
	TestVirtualMethodInvocation();
	
	TestStaticThunkInvocation();

	TestInstanceThunkInvocation();
}

void TestStaticMethodInvocation()
{
	CryLogAlways("TEST: Invoking static methods through IMonoMethod.");

	IMonoClass *lerpClass    = MonoEnv->Cryambly->GetClass("CryCil", "Interpolation")
												->GetNestedType("Linear");
	IMonoClass *vector3Class = MonoEnv->Cryambly->Vector3;
	IMonoClass *singleClass  = MonoEnv->CoreLibrary->Single;
	IMonoClass *int32Class   = MonoEnv->CoreLibrary->Int32;

	CryLogAlways("TEST: Getting a method that accepts a reference to value-type object.");

	auto specClasses = List<Pair<IMonoClass *, const char *>>(4);
	specClasses.Add(Pair<IMonoClass *, const char *>(vector3Class, "&"));
	specClasses.Add(Pair<IMonoClass *, const char *>(vector3Class, ""));
	specClasses.Add(Pair<IMonoClass *, const char *>(vector3Class, ""));
	specClasses.Add(Pair<IMonoClass *, const char *>(singleClass,  ""));

	IMonoMethod *staticMethod = lerpClass->GetMethod("Apply", specClasses);

	CryLogAlways("TEST: Invoking a method.");

	Vec3 res;
	Vec3 start(0, 0, 0);
	Vec3 end(100, 100, 100);
	float parameter = 0.5;

	void *params4[4];
	params4[0] = &res;
	params4[1] = &start;
	params4[2] = &end;
	params4[3] = &parameter;

	staticMethod->Invoke(nullptr, params4);

	CryLogAlways("Result of interpolation: (%*.2f, %*.2f, %*.2f)", res.x, res.y, res.z);

	CryLogAlways("TEST: Getting a method that accepts a reference to an array.");

	staticMethod = mainTestingAssembly->GetClass("MainTestingAssembly", "MethodTestClass")
									  ->GetMethod("NumberToDigits", 2);

	CryLogAlways("TEST: Invoking a method.");

	mono::Array digits;
	int number = 123456;

	void *params2[2];
	params2[0] = &number;
	params2[1] = &digits;

	if (Unbox<bool>(staticMethod->Invoke(nullptr, params2)))
	{
		PrintDigitsArray(digits);
	}
}

void TestInstanceMethodInvocation()
{
	CryLogAlways("TEST: Invoking instance methods through IMonoMethod.");

	IMonoClass *stringClass = MonoEnv->CoreLibrary->String;

	mono::string text = ToMonoString("Some text for testing.");

	IMonoGCHandle *handle = MonoEnv->GC->Pin(text);

	CryLogAlways("TEST: Invocation of the method that accepts 4 arguments and returns an signed integer.");

	IMonoMethod *indexOfMethod = stringClass->GetMethod("IndexOf", 4);

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

	handle->Release();
}

void TestVirtualMethodInvocation()
{
	float number = 1023.56f;

	CryLogAlways("TEST: Invoking virtual methods through IMonoMethod.");

	IMonoClass *singleClass = MonoEnv->CoreLibrary->Single;

	CryLogAlways("TEST: Getting the method wrapper.");

	IMonoMethod *toStringFormatMethod = singleClass->GetMethod("ToString", 2);

	CryLogAlways("TEST: Invoking a virtual method using early binding.");

	void *null = 0;

	void *params[2];
	params[0] = &null;
	params[1] = &null;

	const char *result = ToNativeString(toStringFormatMethod->Invoke(&number, params));

	CryLogAlways("TEST: Result of early-bound invocation: %s.", result);

	delete result;

	CryLogAlways("TEST: Invoking a virtual method using late binding.");

	result = ToNativeString(toStringFormatMethod->Invoke(&number, params, nullptr, true));

	CryLogAlways("TEST: Result of late-bound invocation: %s.", result);

	delete result;
}

struct TestObject
{
	int Number;
	mono::string Text;
};

void TestStaticThunkInvocation()
{
	CryLogAlways("TEST: Invoking static methods through unmanaged thunk.");

	IMonoClass *methodTestClass = mainTestingAssembly->GetClass("MainTestingAssembly", "MethodTestClass");

	CryLogAlways("TEST: Getting the method wrapper of NumberToDigits.");

	IMonoMethod *method = methodTestClass->GetMethod("NumberToDigits", 2);

	CryLogAlways("TEST: Getting the unmanaged thunk.");

	bool(__stdcall *numberToDigitsThunk)(int *, mono::Array *, mono::exception *) =
		(bool(__stdcall *)(int *, mono::Array *, mono::exception *))method->UnmanagedThunk;

	CryLogAlways("TEST: Invoking the unmanaged thunk.");

	int number = 123456;
	mono::Array digits;
	mono::exception ex;

	bool success = numberToDigitsThunk(&number, &digits, &ex);

	if (!ex && success)
	{
		PrintDigitsArray(digits);
	}

	CryLogAlways("TEST: Getting the method wrapper of CreateValueTypeObject.");

	method = methodTestClass->GetMethod("CreateValueTypeObject", 2);

	CryLogAlways("TEST: Getting the unmanaged thunk.");

	void(__stdcall *createValueThunk)(int, mono::object, mono::exception *) =
		(void(__stdcall *)(int, mono::object, mono::exception *))method->UnmanagedThunk;

	CryLogAlways("TEST: Invoking the unmanaged thunk.");

	TestObject tObj;
	mono::object testObj = mainTestingAssembly->GetClass("MainTestingAssembly", "CustomValueType")->Box(&tObj);

	createValueThunk(number, testObj, &ex);

	auto unboxedObj = Unbox<TestObject>(testObj);
	const char *text = ToNativeString(unboxedObj.Text);
	CryLogAlways("TEST: Result of invocation = %s", text);
	delete text;
}

void TestInstanceThunkInvocation()
{
	CryLogAlways("TEST: Invoking instance methods through unmanaged thunk.");

	CryLogAlways("TEST: Getting the method wrapper of Byte::ToString(string).");

	IMonoMethod *method = MonoEnv->CoreLibrary->Byte->GetMethod("ToString", "System.String");

	CryLogAlways("TEST: Getting the unmanaged thunk.");

	mono::string(__stdcall *thunk)(mono::object, mono::string, mono::exception *) =
		(mono::string(__stdcall *)(mono::object, mono::string, mono::exception *))method->UnmanagedThunk;

	CryLogAlways("TEST: Invoking the unmanaged thunk.");

	mono::exception ex;

	mono::string textObj = thunk(Box((unsigned char)100), nullptr, &ex);

	if (!ex)
	{
		const char *text = ToNativeString(textObj);
		CryLogAlways("TEST: Result of invocation = %s", text);
		delete text;
	}
}
#pragma endregion

#pragma region Field Tests
void TestInstanceFields();
void TestStaticFields();

void TestFields()
{
	TestInstanceFields();

	TestStaticFields();
}

void TestInstanceFields()
{
	CryLogAlways("TEST: Testing instance fields.");

	IMonoClass *fieldTestClass = mainTestingAssembly->GetClass("MainTestingAssembly", "FieldTestClass");

	CryLogAlways("TEST: Creating the object.");

	int number = 12000;
	mono::string text = ToMonoString("Something texty.");

	void *params[2];
	params[0] = &number;
	params[1] = text;

	mono::object obj = fieldTestClass->GetConstructor(2)->Invoke(nullptr, params);

	IMonoGCHandle *handle = MonoEnv->GC->Pin(obj);

	CryLogAlways("TEST: Getting the values of fields.");

	CryLogAlways("TEST: Getting the wrappers for both fields.");

	IMonoField *numberField = fieldTestClass->GetField("Number");
	IMonoField *textField   = fieldTestClass->GetField("Text");

	CryLogAlways("TEST: Getting the number field value through the wrapper.");

	int numberValue;
	numberField->Get(obj, &numberValue);

	CryLogAlways("TEST: Acquired value = %d", numberValue);

	CryLogAlways("TEST: Getting the text value through the name.");

	mono::string textValue;
	fieldTestClass->GetField(obj, "Text", &textValue);

	const char *ntText = ToNativeString(textValue);
	CryLogAlways("TEST: Acquired text value = %s", ntText);
	delete ntText;

	CryLogAlways("TEST: Setting the values of fields.");

	CryLogAlways("TEST: Setting the text field value using the wrapper.");

	textValue = ToMonoString("Some different text.");
	fieldTestClass->SetField(obj, textField, &textValue);

	CryLogAlways("TEST: Setting the number field value through the name.");

	numberValue = 9999;
	fieldTestClass->SetField(obj, "Number", &numberValue);

	CryLogAlways("TEST: Getting the values of fields.");

	CryLogAlways("TEST: Getting the number field value using the wrapper.");

	fieldTestClass->GetField(obj, numberField, &numberValue);

	CryLogAlways("TEST: Acquired value = %d", numberValue);

	CryLogAlways("TEST: Getting the text value through the wrapper.");

	textField->Get(obj, &textValue);

	ntText = ToNativeString(textValue);
	CryLogAlways("TEST: Acquired text value = %s", ntText);
	delete ntText;

	handle->Release();
}

void TestStaticFields()
{
	CryLogAlways("TEST: Testing static fields.");

	IMonoClass *fieldTestClass = mainTestingAssembly->GetClass("MainTestingAssembly", "FieldTestClass");

	int number = 12000;
	mono::string text = ToMonoString("Something texty.");

	void *params[2];
	params[0] = &number;
	params[1] = text;

	mono::object obj = fieldTestClass->GetConstructor(2)->Invoke(nullptr, params);

	IMonoGCHandle *handle = MonoEnv->GC->Pin(obj);

	CryLogAlways("TEST: Setting a static field.");

	IMonoField *field = fieldTestClass->GetField("ObjectField");

	fieldTestClass->SetField(nullptr, field, &obj);

	handle->Release();

	CryLogAlways("TEST: Getting a static field.");

	fieldTestClass->GetField(nullptr, field, &obj);

	IMonoField *numberField = fieldTestClass->GetField("Number");
	IMonoField *textField   = fieldTestClass->GetField("Text");

	numberField->Get(obj, &number);
	textField->Get(obj, &text);

	CryLogAlways("TEST: Numeric value = %d", number);
	const char *ntText = ToNativeString(text);
	CryLogAlways("TEST: Text value = %s", ntText);
	delete ntText;
}

#pragma endregion