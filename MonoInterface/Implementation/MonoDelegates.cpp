#include "stdafx.h"

#include "MonoDelegates.h"

#if 0
#define DelegatesMessage CryLogAlways
#else
#define DelegatesMessage(...) void(0)
#endif

mono::delegat MonoDelegates::Create(IMonoClass *delegateType, IMonoStaticMethod *method)
{
	if (!delegateType)
	{
		ReportError("Unable to create a static delegate: delegateType is null.");
		return nullptr;
	}
	if (!method)
	{
		ReportError("Unable to create a static delegate: method is null.");
		return nullptr;
	}

	if (!createStaticDelegate)
	{
		IMonoClass *delegateClass = MonoEnv->CoreLibrary->GetClass("System", "Delegate");

		const char *params = "System.Type,System.Reflection.MethodInfo";
		IMonoFunction *func = delegateClass->GetFunction("CreateDelegate", params);

		createStaticDelegate = CreateStaticDelegate(func->UnmanagedThunk);
	}

	mono::exception ex;
	mono::delegat res = createStaticDelegate(delegateType->GetType(), method->ReflectionObject, &ex);

	if (ex)
	{
		ReportError("Unable to create a static delegate: unhandled exception was thrown.");

		return nullptr;
	}

	return res;
}

mono::delegat MonoDelegates::Create(IMonoClass *delegateType, IMonoMethod *method, mono::object target)
{
	if (!delegateType)
	{
		ReportError("Unable to create an instance delegate: delegateType is null.");
		return nullptr;
	}
	if (!method)
	{
		ReportError("Unable to create an instance delegate: method is null.");
		return nullptr;
	}

	if (!createInstanceDelegate)
	{
		IMonoClass *delegateClass = MonoEnv->CoreLibrary->GetClass("System", "Delegate");

		const char *params = "System.Type,System.Object,System.Reflection.MethodInfo";
		IMonoFunction *func = delegateClass->GetFunction("CreateDelegate", params);

		createInstanceDelegate = CreateInstanceDelegate(func->UnmanagedThunk);
	}

	mono::exception ex;
	mono::delegat res = createInstanceDelegate(delegateType->GetType(), target, method->ReflectionObject, &ex);

	if (ex)
	{
		ReportError("Unable to create an instance delegate: unhandled exception was thrown.");

		return nullptr;
	}

	return res;
}

mono::delegat MonoDelegates::Create(IMonoClass *delegateType, void *functionPointer)
{
	if (!delegateType)
	{
		ReportError("Unable to create a delegate wrapper for unmanaged function: delegateType is null.");
		return nullptr;
	}
	if (!functionPointer)
	{
		ReportError("Unable to create a delegate wrapper for unmanaged function: function pointer is null.");
		return nullptr;
	}

	if (!createDelegateForFunctionPointer)
	{
		DelegatesMessage("Getting the delegate creation thunk.");

		IMonoClass *marshal = MonoEnv->CoreLibrary->GetClass("System.Runtime.InteropServices", "Marshal");
		IMonoFunction *func = marshal->GetFunction("GetDelegateForFunctionPointerInternal", 2);

		createDelegateForFunctionPointer = CreateDelegateForFunctionPointer(func->UnmanagedThunk);

		DelegatesMessage("Got the delegate creation thunk.");
	}

	mono::type delegateTypeObj = delegateType->GetType();

	DelegatesMessage("Delegate type object = %p.", delegateTypeObj);

	mono::exception ex;
	mono::delegat res = createDelegateForFunctionPointer(functionPointer, delegateTypeObj, &ex);

	DelegatesMessage("Created the delegate object.");

	if (ex)
	{
		ReportError("Unable to create a delegate wrapper for unmanaged function: unhandled exception was thrown.");

		return nullptr;
	}

	return res;
}

CreateDelegateForFunctionPointer MonoDelegates::createDelegateForFunctionPointer = nullptr;
CreateInstanceDelegate           MonoDelegates::createInstanceDelegate = nullptr;
CreateStaticDelegate             MonoDelegates::createStaticDelegate = nullptr;
