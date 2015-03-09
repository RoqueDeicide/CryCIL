#pragma once

void TestObjectHandles();
void TestArrays();


void TestObjects()
{
	TestObjectHandles();

	TestArrays();
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