#pragma once

void TestObjects()
{
	CryLogAlways("TEST: Testing IMonoHandle implementation.");

	IMonoClass *testObjectClass = mainTestingAssembly->GetClass("Test", "TestObject");

	CryLogAlways("TEST: Creating an object.");

	double number = 34.567;
	void *param = &number;
	mono::object testObj = testObjectClass->GetConstructor(1)->Create(&param);
	IMonoHandle *obj = MonoEnv->Objects->Wrap(testObj);

	IMonoGCHandle *handle = MonoEnv->GC->Keep(obj->Get());

	CryLogAlways("TEST: Testing object's fields.");

	int fieldNumber = 0;
	obj->GetField("Number", &fieldNumber);

	CryLogAlways("TEST: The integer field's value: %d", fieldNumber);

	mono::string fieldText;
	obj->GetField("Text", &fieldText);

	CryLogAlways("TEST: The text field's value: %s", NtText(fieldText));

	CryLogAlways("TEST: Testing object's property.");

	auto prop = obj->GetProperty("DecimalNumber");

	if (prop)
	{
		CryLogAlways("TEST SUCCESS: Successfully got a wrapper for a property %s.", prop->Name);
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the wrapper for a property DecimalNumber.");
	}

	CryLogAlways("TEST: Testing object's events.");

	auto _event = obj->GetEvent("Something");

	if (_event)
	{
		CryLogAlways("TEST SUCCESS: Successfully got a wrapper for an event %s.", _event->Name);
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the wrapper for an event Something.");
	}

	IMonoClass *declaringClass = obj->GetClass();

	if (declaringClass)
	{
		CryLogAlways("TEST SUCCESS: Successfully got a wrapper for the object's class %s.", declaringClass->Name);
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the wrapper for the object's class.");
	}

	CryLogAlways("TEST: Testing updating the reference to the wrapped object after triggering GC.");

	MonoEnv->GC->Collect();

	obj->Update(handle->Object);

	if (obj->Get())
	{
		CryLogAlways("TEST SUCCESS: Successfully got a reference to the object's new location.");
	}
	else
	{
		ReportError("TEST FAILURE: Unable to get the reference to the object's new location.");
	}

	CryLogAlways("TEST: Testing getting the field value after getting a new reference to the object.");

	obj->GetField("Number", &fieldNumber);

	CryLogAlways("TEST: The integer field's value: %d", fieldNumber);

	handle->Release();
}