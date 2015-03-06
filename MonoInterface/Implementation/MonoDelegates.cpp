#include "stdafx.h"

#include "MonoDelegates.h"

IMonoDelegate *MonoDelegates::Wrap(mono::delegat delegat)
{
	return new MonoDelegateWrapper(delegat);
}

IMonoDelegate *MonoDelegates::Create(IMonoClass *delegateType, IMonoMethod *method, IMonoClass *klass)
{
	if (!delegateType)
	{
		gEnv->pLog->LogError("Unable to create a static delegate: delegateType is null.");
		return nullptr;
	}
	if (!method)
	{
		gEnv->pLog->LogError("Unable to create a static delegate: method is null.");
		return nullptr;
	}
	if (!klass)
	{
		gEnv->pLog->LogError("Unable to create a static delegate: klass is null.");
		return nullptr;
	}

	if (!createStaticDelegate)
	{
		createStaticDelegate = (CreateStaticDelegate)
			MonoEnv->CoreLibrary
				   ->GetClass("System", "Delegate")
				   ->GetFunction("CreateDelegate", "System.Type,System.Reflection.MethodInfo")
				   ->UnmanagedThunk;
	}

	mono::exception ex;
	(
		mono_type_get_object
		(
			mono_domain_get(),
			mono_class_get_type((MonoClass *)delegateType->GetWrappedPointer())
		),
		mono_method_get_object
		(
			mono_domain_get(),
			(MonoMethod *)method->GetWrappedPointer(),
			(MonoClass *)klass->GetWrappedPointer()
		),
		&ex
	);
	mono::delegat res = createStaticDelegate

	if (ex)
	{
		gEnv->pLog->LogError("Unable to create a static delegate: unhandled exception was thrown.");

		return nullptr;
	}

	return new MonoDelegateWrapper(res);
}

IMonoDelegate *MonoDelegates::Create(IMonoClass *delegateType, IMonoMethod *method, mono::object target)
{
	if (!delegateType)
	{
		gEnv->pLog->LogError("Unable to create an instance delegate: delegateType is null.");
		return nullptr;
	}
	if (!method)
	{
		gEnv->pLog->LogError("Unable to create an instance delegate: method is null.");
		return nullptr;
	}

	if (!createInstanceDelegate)
	{
		createInstanceDelegate = (CreateInstanceDelegate)
			MonoEnv->CoreLibrary->GetClass("System", "Delegate")
			->GetFunction("CreateDelegate", "System.Type,System.Object,System.Reflection.MethodInfo")
			->UnmanagedThunk;
	}

	mono::exception ex;
	(
		mono_type_get_object
		(
			mono_domain_get(),
			mono_class_get_type((MonoClass *)delegateType->GetWrappedPointer())
		),
		target,
		mono_method_get_object
		(
			mono_domain_get(),
			(MonoMethod *)method->GetWrappedPointer(),
			mono_object_get_class((MonoObject *)target)
		),
		&ex
	);
	mono::delegat res = createInstanceDelegate

	if (ex)
	{
		gEnv->pLog->LogError("Unable to create an instance delegate: unhandled exception was thrown.");

		return nullptr;
	}

	return new MonoDelegateWrapper(res);
}

CreateInstanceDelegate           MonoDelegates::createInstanceDelegate = nullptr;
CreateStaticDelegate             MonoDelegates::createStaticDelegate = nullptr;
