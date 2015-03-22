#pragma once

void TestObjectHandles();
void TestArrays();
void TestDelegates();


void TestObjects()
{
	TestObjectHandles();

	TestArrays();

	TestDelegates();
}

void TestObjectHandles()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing IMonoHandle implementation.");

	IMonoClass *testObjectClass = mainTestingAssembly->GetClass("Test", "TestObject");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Creating an object.");
	CryLogAlways("TEST:");

	double number = 34.567;
	void *param = &number;
	mono::object testObj = testObjectClass->GetConstructor(1)->Create(&param);
	auto obj = IMonoObject(testObj);

	MonoGCHandle handle = MonoEnv->GC->Keep(obj);

	CryLogAlways("TEST: Testing object's fields.");
	CryLogAlways("TEST:");

	int fieldNumber = 0;
	obj.GetField("Number", &fieldNumber);

	CryLogAlways("TEST: The integer field's value: %d", fieldNumber);

	mono::string fieldText;
	obj.GetField("Text", &fieldText);

	CryLogAlways("TEST: The text field's value: %s", NtText(fieldText));

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing object's property.");
	CryLogAlways("TEST:");

	auto prop = obj.GetProperty("DecimalNumber");

	if (prop)
	{
		CryLogAlways("TEST SUCCESS: Successfully got a wrapper for a property %s.", prop->Name);
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the wrapper for a property DecimalNumber.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing object's events.");
	CryLogAlways("TEST:");

	auto _event = obj.GetEvent("Something");

	if (_event)
	{
		CryLogAlways("TEST SUCCESS: Successfully got a wrapper for an event %s.", _event->Name);
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the wrapper for an event Something.");
	}

	IMonoClass *declaringClass = obj.GetClass();

	if (declaringClass)
	{
		CryLogAlways("TEST SUCCESS: Successfully got a wrapper for the object's class %s.", declaringClass->Name);
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the wrapper for the object's class.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing updating the reference to the wrapped object after triggering GC.");
	CryLogAlways("TEST:");

	MonoEnv->GC->Collect();

	obj = handle.Object;

	if ((mono::object)obj)
	{
		CryLogAlways("TEST SUCCESS: Successfully got a reference to the object's new location.");
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the reference to the object's new location.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing getting the field value after getting a new reference to the object.");
	CryLogAlways("TEST:");

	obj.GetField("Number", &fieldNumber);

	CryLogAlways("TEST: The integer field's value: %d", fieldNumber);
	CryLogAlways("TEST:");
}

void TestArrays()
{
	IMonoClass *matrix33Class = MonoEnv->Cryambly->Matrix33;
	IMonoArrays *arrays = MonoEnv->Objects->Arrays;

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing IMonoArray implementation.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Creating an array of 3x3 matrices.");
	CryLogAlways("TEST:");
	
	auto matrices = IMonoArray<Matrix33>(arrays->Create(5, matrix33Class));
	
	CryLogAlways("TEST: Pinning the array in place.");
	CryLogAlways("TEST:");

	MonoGCHandle handle = MonoEnv->GC->Pin(matrices);

	CryLogAlways("TEST: Initializing the array with spherical linear interpolations.");
	CryLogAlways("TEST:");

	Matrix33 start = Matrix33::CreateRotationX(PI / 2.0);
	Matrix33 end   = Matrix33::CreateRotationZ(PI / 2.0);

	int length = matrices.Length;
	for (int i = 0; i < length; i++)
	{
		matrices[i] = Matrix33::CreateSlerp(start, end, i / 5.0f);
	}

	CryLogAlways("TEST: Printing out the determinants of matrices in the array.");
	CryLogAlways("TEST:");

	for (int i = 0; i < length; i++)
	{
		CryLogAlways("TEST: %d) %f;", i + 1, matrices[i].Determinant());
	}

	CryLogAlways("TEST: Creating 2D array of 2-component vectors.");
	CryLogAlways("TEST:");

	unsigned int lengths[2];
	lengths[0] = 3;
	lengths[1] = 3;
	auto vectors =
		IMonoArray<Vec2>(MonoEnv->Objects->Arrays->Create(2, lengths, MonoEnv->Cryambly->Vector2));

	CryLogAlways("TEST: Pinning the array in place.");

	handle = MonoEnv->GC->Pin(vectors);

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Filling the array with linear interpolations.");
	CryLogAlways("TEST:");
	
	Vec2 zero = Vec2(0, 0);
	Vec2 x1   = Vec2(1.0f, 0.0f);
	Vec2 y1   = Vec2(0.0f, 1.0f);

	auto indices = List<int>(2).Add(0, 0);
	for (int i = 0; i < vectors.GetLength(0); i++)
	{
		indices[0] = i;
		for (int j = 0; j < vectors.GetLength(1); j++)
		{
			vectors[indices.Set(1, j)] =
				Vec2::CreateLerp(zero, x1, i / 3.0f) + Vec2::CreateLerp(zero, y1, j / 3.0f);
		}
	}
	
	CryLogAlways("TEST: Printing the array.");
	CryLogAlways("TEST:");

	for (int i = 0; i < vectors.GetLength(0); i++)
	{
		indices[0] = i;
		for (int j = 0; j < vectors.GetLength(1); j++)
		{
			CryLogAlways("TEST: %f", vectors[indices.Set(1, j)]);
		}
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Creating a Pascal-style array of 10 integers with first index = 1.");
	CryLogAlways("TEST:");

	unsigned int pascalLength = 10;
	int lowerBound = 1;
	auto pascalArray = IMonoArray<int>(MonoEnv->Objects->Arrays->Create(1, &pascalLength, MonoEnv->CoreLibrary->Int32, &lowerBound));

	CryLogAlways("TEST: Pinning the array in place.");

	handle = MonoEnv->GC->Pin(pascalArray);

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Filling the array of integers.");
	CryLogAlways("TEST:");

	for (int i = pascalArray.GetLowerBound(0); i < pascalArray.GetLength(0) + pascalArray.GetLowerBound(0); i++)
	{
		pascalArray[List<int>(1).Add(i)] = i * 3 - 1;
	}

	CryLogAlways("TEST: Printing the array of integers.");
	CryLogAlways("TEST:");

	for (int i = pascalArray.GetLowerBound(0); i < pascalArray.GetLength(0) + pascalArray.GetLowerBound(0); i++)
	{
		CryLogAlways("TEST: %d", pascalArray[List<int>(1).Add(i)]);
	}

	CryLogAlways("TEST:");
}

void __cdecl NativeTestFunctionCdecl(int arg)
{
	CryLogAlways("TEST: Native function has been invoked through the delegate with a number %d passed as an argument using C calling convention.", arg);
}

void __stdcall NativeTestFunctionStdCall(int arg)
{
	CryLogAlways("TEST: Native function has been invoked through the delegate with a number %d passed as an argument using standard calling convention.", arg);
}

void TestDelegates()
{
	IMonoClass *staticTestDelegateClass   = mainTestingAssembly->GetClass("Test", "StaticTestDelegate");
	IMonoClass *instanceTestDelegateClass = mainTestingAssembly->GetClass("Test", "InstanceTestDelegate");
	
	IMonoClass *staticTestClass = mainTestingAssembly->GetClass("Test", "StaticTest");

	IMonoClass *instanceTestClass1 = mainTestingAssembly->GetClass("Test", "InstanceTest1");
	IMonoClass *instanceTestClass2 = mainTestingAssembly->GetClass("Test", "InstanceTest2");
	IMonoClass *instanceTestClass3 = mainTestingAssembly->GetClass("Test", "InstanceTest3");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing IMonoDelegate implementation.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing delegates that wrap static methods.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Creating 3 static delegate objects.");
	CryLogAlways("TEST:");

	IMonoDelegate staticDel1 = MonoEnv->Objects->Delegates->Create(staticTestDelegateClass, staticTestClass->GetFunction("Test1", -1)->ToStatic());
	IMonoDelegate staticDel2 = MonoEnv->Objects->Delegates->Create(staticTestDelegateClass, staticTestClass->GetFunction("Test2", -1)->ToStatic());
	IMonoDelegate staticDel3 = MonoEnv->Objects->Delegates->Create(staticTestDelegateClass, staticTestClass->GetFunction("Test3", -1)->ToStatic());

	CryLogAlways("TEST: Combining invocation lists of all delegates.");
	CryLogAlways("TEST:");

	staticDel1 += staticDel2 + staticDel3;

	CryLogAlways("TEST: Invoking the delegate.");
	CryLogAlways("TEST:");

	staticDel1.Invoke(nullptr);

	CryLogAlways("TEST: Removing second delegate from the invocation list.");
	CryLogAlways("TEST:");

	staticDel1 -= staticDel2;

	CryLogAlways("TEST: Invoking the delegate again.");
	CryLogAlways("TEST:");

	staticDel1.Invoke(nullptr);

	CryLogAlways("TEST: Testing delegates that wrap instance methods.");
	CryLogAlways("TEST:");

	mono::object target1 = instanceTestClass1->GetConstructor(-1)->Create();
	mono::object target2 = instanceTestClass2->GetConstructor(-1)->Create();
	mono::object target3 = instanceTestClass3->GetConstructor(-1)->Create();

	CryLogAlways("TEST: Creating 3 instance delegate objects.");
	CryLogAlways("TEST:");

	IMonoDelegate instanceDel1 = MonoEnv->Objects->Delegates->Create(instanceTestDelegateClass, instanceTestClass1->GetFunction(nullptr, 1)->ToInstance(), target1);
	IMonoDelegate instanceDel2 = MonoEnv->Objects->Delegates->Create(instanceTestDelegateClass, instanceTestClass2->GetFunction(nullptr, 1)->ToInstance(), target2);
	IMonoDelegate instanceDel3 = MonoEnv->Objects->Delegates->Create(instanceTestDelegateClass, instanceTestClass3->GetFunction(nullptr, 1)->ToInstance(), target3);

	CryLogAlways("TEST: Combining invocation lists of all delegates.");
	CryLogAlways("TEST:");

	instanceDel1 += instanceDel2 + instanceDel3;

	CryLogAlways("TEST: Invoking the delegate.");
	CryLogAlways("TEST:");

	mono::string text = ToMonoString("Some text with a number 129 in it.");
	void *param = text;
	instanceDel1.Invoke(&param);

	CryLogAlways("TEST: Removing third delegate from the invocation list.");
	CryLogAlways("TEST:");

	instanceDel1 -= instanceDel3;

	CryLogAlways("TEST: Invoking the delegate again.");
	CryLogAlways("TEST:");

	instanceDel1.Invoke(&param);

	CryLogAlways("TEST: Testing some delegate properties.");
	CryLogAlways("TEST:");

	CryLogAlways("TEST: Third instance delegate's function name: %s", instanceDel3.Function->Name);
	CryLogAlways("TEST:");

	int fieldValue;
	IMonoObject tar = instanceDel2.Target;
	tar.GetField("SomeField", &fieldValue);

	CryLogAlways("TEST: Value of the field: %d.", fieldValue);
	CryLogAlways("TEST:");

	CryLogAlways("TEST: Invoking first instance delegate through trampoline");
	CryLogAlways("TEST:");

	((void(*)(mono::string))instanceDel1.Trampoline)(text);

	CryLogAlways("TEST: Testing delegates that wrap function pointers.");
	CryLogAlways("TEST:");
	
	IMonoClass *nativeTestDelegateCdeclClass   = mainTestingAssembly->GetClass("Test", "NativeTestDelegateCdecl");
	IMonoClass *nativeTestDelegateStdCallClass = mainTestingAssembly->GetClass("Test", "NativeTestDelegateStdCall");

	CryLogAlways("TEST: Creating 2 delegate objects.");
	CryLogAlways("TEST:");

	IMonoDelegate nativeDel1 = MonoEnv->Objects->Delegates->Create(nativeTestDelegateCdeclClass,   NativeTestFunctionCdecl);
	IMonoDelegate nativeDel2 = MonoEnv->Objects->Delegates->Create(nativeTestDelegateStdCallClass, NativeTestFunctionStdCall);

	CryLogAlways("TEST: Invoking the delegates.");
	CryLogAlways("TEST:");

	int argInt = 10;
	param = &argInt;
	nativeDel1.Invoke(&param);
	argInt = 19;
	nativeDel2.Invoke(&param);
}