#include "stdafx.h"
#include "TestStart.h"

void TestObjectHandles();
void TestArrays();
void TestDelegates();
void TestExceptions();
void TestStrings();
void TestThreads();

void TestObjects()
{
	TestObjectHandles();

	TestArrays();

	TestDelegates();

	TestExceptions();

	TestStrings();

	TestThreads();
}

inline void TestObjectHandles()
{
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing IMonoHandle implementation.");

	IMonoClass *testObjectClass = mainTestingAssembly->GetClass("MainTestingAssembly", "TestObject");

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

	CryLogAlways("TEST: The text field's value: %s", NtText(fieldText).c_str());

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

	if (mono::object(obj))
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

inline void TestArrays()
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
	Matrix33 end = Matrix33::CreateRotationZ(PI / 2.0);

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
	Vec2 x1 = Vec2(1.0f, 0.0f);
	Vec2 y1 = Vec2(0.0f, 1.0f);

	auto indices = List<int>({ 0, 0 });
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

inline void __cdecl NativeTestFunctionCdecl(int arg)
{
	CryLogAlways("TEST: Native function has been invoked through the delegate with a number %d passed as an argument using C calling convention.", arg);
}

inline void __stdcall NativeTestFunctionStdCall(int arg)
{
	CryLogAlways("TEST: Native function has been invoked through the delegate with a number %d passed as an argument using standard calling convention.", arg);
}

inline void TestDelegates()
{
	IMonoClass *staticTestDelegateClass = mainTestingAssembly->GetClass("MainTestingAssembly", "StaticTestDelegate");
	IMonoClass *instanceTestDelegateClass = mainTestingAssembly->GetClass("MainTestingAssembly", "InstanceTestDelegate");

	IMonoClass *staticTestClass = mainTestingAssembly->GetClass("MainTestingAssembly", "StaticTest");

	IMonoClass *instanceTestClass1 = mainTestingAssembly->GetClass("MainTestingAssembly", "InstanceTest1");
	IMonoClass *instanceTestClass2 = mainTestingAssembly->GetClass("MainTestingAssembly", "InstanceTest2");
	IMonoClass *instanceTestClass3 = mainTestingAssembly->GetClass("MainTestingAssembly", "InstanceTest3");

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

	reinterpret_cast<void(*)(mono::string)>(instanceDel1.Trampoline)(text);

	CryLogAlways("TEST: Testing delegates that wrap function pointers.");
	CryLogAlways("TEST:");

	IMonoClass *nativeTestDelegateCdeclClass = mainTestingAssembly->GetClass("MainTestingAssembly", "NativeTestDelegateCdecl");
	IMonoClass *nativeTestDelegateStdCallClass = mainTestingAssembly->GetClass("MainTestingAssembly", "NativeTestDelegateStdCall");

	CryLogAlways("TEST: Creating 2 delegate objects.");
	CryLogAlways("TEST:");

	IMonoDelegate nativeDel1 = MonoEnv->Objects->Delegates->Create(nativeTestDelegateCdeclClass, NativeTestFunctionCdecl);
	IMonoDelegate nativeDel2 = MonoEnv->Objects->Delegates->Create(nativeTestDelegateStdCallClass, NativeTestFunctionStdCall);

	CryLogAlways("TEST: Invoking the delegates.");
	CryLogAlways("TEST:");

	int argInt = 10;
	param = &argInt;
	nativeDel1.Invoke(&param);
	argInt = 19;
	nativeDel2.Invoke(&param);
}

inline void ThrowExceptionInternal(mono::exception ex)
{
	IMonoException(ex).Throw();
}

void TestExceptionObject(mono::exception ex, const char *typeName);

inline void TestExceptions()
{
	MonoEnv->Functions->AddInternalCall("MainTestingAssembly",
										"ExceptionTestingMethods",
										"ThrowExceptionInternal",
										ThrowExceptionInternal);

	auto testClass = mainTestingAssembly->GetClass("MainTestingAssembly", "ExceptionTesting");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing IMonoException implementation.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Trying to throw exception using CryCIL API.");
	CryLogAlways("TEST:");

	testClass->GetFunction("TestUnderlyingExceptionThrowing")->ToStatic()->Invoke();

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing catching exceptions.");
	CryLogAlways("TEST:");

	mono::exception ex;
	void *param = ToMonoString("Message for the exception object.");
	testClass->GetFunction("MakeAndThrowException")->ToStatic()->Invoke(&param, &ex);

	IMonoException exc(ex);

	CryLogAlways("TEST: Caught exception's details are: Message = \"%s\", Length of the stack-trace is %d.", NtText(exc.Message).c_str(), NtText(exc.StackTrace).Length, exc.Class->FullName);
	CryLogAlways("TEST:");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing creation of exceptions via IMonoExceptions::Create.");
	CryLogAlways("TEST:");

	auto inner = MonoEnv->Objects->Exceptions
		->Create
		(mainTestingAssembly,
		"MainTestingAssembly",
		"CryCilTestException",
		"Message for object that was created using IMonoExceptions::Create."
		);

	if (inner)
	{
		CryLogAlways("TEST SUCCESS: A simple exception object was created.");
	}
	else
	{
		ReportError("TEST FAILURE: A simple exception object was not created.");
	}

	CryLogAlways("TEST:");

	param = inner;
	auto returnedInner = IMonoException(testClass->GetFunction("GetExceptionWithInnerOne")->ToStatic()->Invoke(&param)).InnerException;

	if (returnedInner)
	{
		CryLogAlways("TEST SUCCESS: IMonoException::InnerException property works.");
	}
	else
	{
		ReportError("TEST FAILURE: IMonoException::InnerException property doesn't work.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing creation of various built-in exceptions.");
	CryLogAlways("TEST:");

	IMonoExceptions *exes = MonoEnv->Objects->Exceptions;

	TestExceptionObject(exes->AppDomainUnloaded("Test Message"), "AppDomainUnloadedException");
	TestExceptionObject(exes->Argument("Argument", "Test"), "ArgumentException");
	TestExceptionObject(exes->ArgumentNull("Test Message"), "ArgumentNullException");
	TestExceptionObject(exes->ArgumentOutOfRange("Test Message"), "ArgumentOutOfRangeException");
	TestExceptionObject(exes->Arithmetic("Test Message"), "ArithmeticException");
	TestExceptionObject(exes->ArrayTypeMismatch("Test Message"), "ArrayTypeMismatchException");
	TestExceptionObject(exes->BadImageFormat("Test Message"), "BadImageFormatException");
	TestExceptionObject(exes->BaseException("Test Message"), "Exception");
	TestExceptionObject(exes->CannotUnloadAppDomain("Test Message"), "CannotUnloadAppDomainException");
	TestExceptionObject(exes->DivideByZero("Test Message"), "DivideByZeroException");
	TestExceptionObject(exes->ExecutionEngine("Test Message"), "ExecutionEngineException");
	TestExceptionObject(exes->FileNotFound("SomeFile.txt", "Test"), "FileNotFoundException");
	TestExceptionObject(exes->IndexOutOfRange("Test Message"), "IndexOutOfRangeException");
	TestExceptionObject(exes->InvalidCast("Test Message"), "InvalidCastException");
	TestExceptionObject(exes->IO("Test Message"), "IOException");
	TestExceptionObject(exes->MissingField("Test Message"), "MissingFieldException");
	TestExceptionObject(exes->MissingMethod("Test Message"), "MissingMethodException");
	TestExceptionObject(exes->NotImplemented("Test Message"), "NotImplementedException");
	TestExceptionObject(exes->NotSupported("Test Message"), "NotSupportedException");
	TestExceptionObject(exes->NullReference("Test Message"), "NullReferenceException");
	TestExceptionObject(exes->Overflow("Test Message"), "OverflowException");
	TestExceptionObject(exes->Security("Test Message"), "SecurityException");
	TestExceptionObject(exes->Serialization("Test Message"), "SerializationException");
	TestExceptionObject(exes->StackOverflow("Test Message"), "StackOverflowException");
	TestExceptionObject(exes->SynchronizationLock("Test Message"), "SynchronizationLockException");
	TestExceptionObject(exes->ThreadAbort("Test Message"), "ThreadAbortException");
	TestExceptionObject(exes->ThreadState("Test Message"), "ThreadStateException");
	TestExceptionObject(exes->TypeInitialization("Test Message"), "TypeInitializationException");
	TestExceptionObject(exes->TypeLoad("Test Message"), "TypeLoadException");

	CryLogAlways("TEST:");
}

inline void TestExceptionObject(mono::exception ex, const char *typeName)
{
	if (ex)
	{
		CryLogAlways("TEST SUCCESS: The exception object of type %s was created.", typeName);
	}
	else
	{
		ReportError("TEST FAILURE: The exception object of type %s was not created.", typeName);
		return;
	}
}

inline void TestStrings()
{
	auto testClass = mainTestingAssembly->GetClass("MainTestingAssembly", "StringTest");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing IMonoText implementation.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing getting a hash code of the string.");
	CryLogAlways("TEST:");

	int hashCode = IMonoText("Some text for testing purposes.").HashCode;
	CryLogAlways("TEST: Hash code of the string = %d", hashCode);

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing detection of interned strings when working with literals.");
	CryLogAlways("TEST:");

	IMonoText text(testClass->GetFunction("")->ToStatic()->Invoke());
	if (text.Interned)
	{
		CryLogAlways("TEST SUCCESS: Literals returned from Mono are properly recognized as interned strings.");
	}
	else
	{
		ReportError("TEST FAILURE: Literals returned from Mono are not recognized as interned strings.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing detection of interned strings that were interned at run-time.");
	CryLogAlways("TEST:");

	text = IMonoText("Some text that is not interned normally, but is about to be interned.");
	if (!text.Interned)
	{
		CryLogAlways("TEST SUCCESS: Strings created from null-terminated ones are not recognized as interned ones.");
	}
	else
	{
		ReportError("TEST FAILURE: Strings created from null-terminated ones are recognized as interned ones.");
	}

	CryLogAlways("TEST:");

	text.Intern();
	if (text.Interned)
	{
		CryLogAlways("TEST SUCCESS: A string was successfully interned.");
	}
	else
	{
		ReportError("TEST FAILURE: A string was not interned.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing equality checks.");
	CryLogAlways("TEST:");

	if (text.Equals(IMonoText("Some text that is not interned normally, but is about to be interned.")))
	{
		CryLogAlways("TEST SUCCESS: 2 exactly the same strings are equal.");
	}
	else
	{
		ReportError("TEST FAILURE: 2 exactly the same strings are not equal.");
	}

	CryLogAlways("TEST:");

	if (text.Equals(ToMonoString("Some text that is not interned normally.")))
	{
		CryLogAlways("TEST SUCCESS: 2 different strings are not equal.");
	}
	else
	{
		ReportError("TEST FAILURE: 2 different strings are equal.");
	}

	CryLogAlways("TEST:");
	CryLogAlways("TEST: The interned string is: %s.", NtText(text.NativeUTF8).c_str());
	CryLogAlways("TEST:");
}

inline const char *ToOrdinal(int number)
{
	char buffer[20];
	itoa(number, buffer, 10);

	if (number <= 0) return NtText(buffer).Detach();

	switch (number % 100)
	{
	case 11:
	case 12:
	case 13:
		return NtText(buffer, "th").Detach();
	}

	switch (number % 10)
	{
	case 1:
		return NtText(buffer, "st").Detach();
	case 2:
		return NtText(buffer, "nd").Detach();
	case 3:
		return NtText(buffer, "rd").Detach();
	default:
		return NtText(buffer, "th").Detach();
	}
}

void ProcessStuffs(IMonoClass *klass, const char *threadName);

inline void ThreadFunction()
{
	CryLogAlways("TEST: Unmanaged Worker: A test thread with unmanaged function has been started.");

	IMonoThread thread = MonoEnv->Objects->Threads->Attach();

	CryLogAlways("TEST: Unmanaged Worker: Setting the name of this thread to [Unmanaged Worker].");

	thread.Name = ToMonoString("Unmanaged Worker");

	CryLogAlways("TEST: Unmanaged Worker: This thread's name is now: [%s].", NtText(thread.Name).c_str());

	ProcessStuffs(mainTestingAssembly->GetClass("MainTestingAssembly", "ThreadTestClass"), "Unmanaged Worker");

	CryLogAlways("TEST: Unmanaged Worker: Putting this thread to sleep for 500 milliseconds.");

	MonoEnv->Objects->Threads->Sleep(500);

	CryLogAlways("TEST: Unmanaged Worker: Work complete.");
}

inline void ProcessStuffs(IMonoClass *klass, const char *threadName)
{

	CryLogAlways("TEST: %s: About to enter a critical section.", threadName);

	mono::object lockObject = klass->GetField<mono::object>(nullptr, "Lock");

	MonoEnv->Objects->MonitorEnter(lockObject);

	CryLogAlways("TEST: %s: Entered the critical section.", threadName);

	if (MonoEnv->Objects->MonitorIsEntered(lockObject))
	{
		CryLogAlways("TEST SUCCESS: %s: This thread is in fact in the critical section.", threadName);
	}
	else
	{
		ReportError("TEST FAILURE: %s: This thread is not detected as one in the critical section.", threadName);
	}

	IMonoField *lockField = klass->GetField("Counter");
	int accessCounter = klass->GetField<int>(nullptr, lockField) + 1;
	klass->SetField(nullptr, lockField, &accessCounter);

	CryLogAlways("TEST: %s: This thread was %d to enter critical section.", threadName, NtText(ToOrdinal(accessCounter)).c_str());
	CryLogAlways("TEST: %s: Leaving the critical section.", threadName);

	MonoEnv->Objects->MonitorExit(lockObject);
}

inline void TestThreads()
{
	auto testClass = mainTestingAssembly->GetClass("MainTestingAssembly", "ThreadTestClass");
	auto paramThreadStart = MonoEnv->CoreLibrary->GetClass("System.Threading", "ParameterizedThreadStart");
	auto threadStart = MonoEnv->CoreLibrary->GetClass("System.Threading", "ThreadStart");

	CryLogAlways("TEST:");
	CryLogAlways("TEST: Testing IMonoThread implementation.");
	CryLogAlways("TEST:");
	CryLogAlways("TEST: Creating a thread object with parameterized method.");
	CryLogAlways("TEST:");

	auto paramFunc = testClass->GetFunction("ThreadingWithParameters", -1)->ToStatic();
	mono::delegat paramDelegat = MonoEnv->Objects->Delegates->Create(paramThreadStart, paramFunc);
	IMonoThread paramThread = MonoEnv->Objects->Threads->CreateParametrized(paramDelegat);

	CryLogAlways("TEST: Creating a thread object with unmanaged function.");
	CryLogAlways("TEST:");

	mono::delegat threadDelegat = MonoEnv->Objects->Delegates->Create(threadStart, ThreadFunction);
	IMonoThread paramlessThread = MonoEnv->Objects->Threads->Create(threadDelegat);

	CryLogAlways("TEST: Starting a thread with no parameters.");
	CryLogAlways("TEST:");

	paramlessThread.Start();

	CryLogAlways("TEST: Starting a thread with parameterless thread as a parameter.");
	CryLogAlways("TEST:");

	paramThread.Start(paramlessThread);

	ProcessStuffs(testClass, "Main Thread");

	CryLogAlways("TEST:");
}