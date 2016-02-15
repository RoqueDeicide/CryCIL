#include "stdafx.h"

#include "MonoDelegates.h"

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
		createStaticDelegate = CreateStaticDelegate
			(MonoEnv->CoreLibrary
					->GetClass("System", "Delegate")
					->GetFunction("CreateDelegate", "System.Type,System.Reflection.MethodInfo")
					->UnmanagedThunk);
	}

	mono::exception ex;
	mono::delegat res = createStaticDelegate
		(delegateType->GetType(), method->ReflectionObject, &ex);

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
		createInstanceDelegate = CreateInstanceDelegate(MonoEnv->CoreLibrary->GetClass("System", "Delegate")
															   ->GetFunction
															   (
																   "CreateDelegate",
																   "System.Type,System.Object,System.Reflection.MethodInfo"
															   )
															   ->UnmanagedThunk);
	}

	mono::exception ex;
	mono::delegat res = createInstanceDelegate
		(delegateType->GetType(), target, method->ReflectionObject, &ex);

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
		createDelegateForFunctionPointer =
			CreateDelegateForFunctionPointer(MonoEnv->CoreLibrary
													->GetClass("System.Runtime.InteropServices", "Marshal")
													->GetFunction("GetDelegateForFunctionPointerInternal", 2)
													->UnmanagedThunk);
	}

	mono::exception ex;
	mono::delegat res = createDelegateForFunctionPointer(functionPointer, delegateTypeObj, &ex);

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
