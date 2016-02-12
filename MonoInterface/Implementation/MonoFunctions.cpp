#include "stdafx.h"
#include "MonoFunctions.h"
#include "MonoClass.h"

#if 1
#define FunctionsMessage CryLogAlways
#else
#define FunctionsMessage(...) void(0)
#endif

IMonoClass *MonoFunctions::GetDeclaringClass(_MonoMethod *method)
{
	MonoClass *klass = mono_method_get_class(method);
	return (klass) ? MonoClassCache::Wrap(klass) : nullptr;
}

const char *MonoFunctions::GetName(_MonoMethod *method)
{
	return mono_method_get_name(method);
}

void MonoFunctions::AddInternalCall(const char *nameSpace, const char *className, const char *name,
									void *functionPointer)
{
	FunctionsMessage("Adding an internal call for the method %s.%s.%s", nameSpace, className, name);
	mono_add_internal_call(NtText({ nameSpace, ".", className, "::", name }), functionPointer);
	FunctionsMessage("Added the internal call.");
}

void *MonoFunctions::LookupInternalCall(IMonoFunction *func)
{
	return mono_lookup_internal_call(func->GetHandle<MonoMethod>());
}

mono::object MonoFunctions::InternalInvoke(_MonoMethod *func, void *object, void **args, mono::exception *ex,
										   bool polymorph)
{
	MonoMethod *methodToInvoke;
	if (polymorph)
	{
		methodToInvoke = mono_object_get_virtual_method(static_cast<MonoObject *>(object), func);
	}
	else
	{
		methodToInvoke = func;
	}
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke(methodToInvoke, object, args, &exception);
	if (exception)
	{
		if (ex)
		{
			*ex = mono::exception(exception);
		}
		else
		{
#ifdef _DEBUG
			MonoEnv->HandleException(mono::exception(exception));
#else
			MonoObject *messageString = &exception[3];
			const char *message = mono_string_to_utf8((MonoString *)messageString);
			ReportError("Unhandled exception was caught with message: %s", message);
			mono_free(message);
#endif // _DEBUG
		}
		return nullptr;
	}
	return mono::object(result);
}

mono::object MonoFunctions::InternalInvokeArray(_MonoMethod *func, void *object, IMonoArray<> &args,
												mono::exception *ex, bool polymorph)
{
	MonoMethod *methodToInvoke;
	if (polymorph)
	{
		methodToInvoke = mono_object_get_virtual_method(static_cast<MonoObject *>(object), func);
	}
	else
	{
		methodToInvoke = func;
	}
	MonoArray *paramsArray = reinterpret_cast<MonoArray *>(mono::object(args));
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke_array(methodToInvoke, object, paramsArray, &exception);
	if (exception)
	{
		if (ex)
		{
			*ex = mono::exception(exception);
		}
		else
		{
#ifdef _DEBUG
			MonoEnv->HandleException(mono::exception(exception));
#else
			MonoObject *messageString = &exception[3];
			const char *message = mono_string_to_utf8((MonoString *)messageString);
			ReportError("Unhandled exception was caught with message: %s", message);
			mono_free(message);
#endif // _DEBUG
		}
		return nullptr;
	}
	return mono::object(result);
}

void *MonoFunctions::GetUnmanagedThunk(_MonoMethod *func)
{
	FunctionsMessage("Getting the thunk.");

	void *thunk = mono_method_get_unmanaged_thunk(func);
	
	FunctionsMessage("Got the thunk.");

	return thunk;
}

void *MonoFunctions::GetRawThunk(_MonoMethod *func)
{
	FunctionsMessage("Querying the thunk.");
	if (func)
	{
		FunctionsMessage("Getting the thunk.");
		auto thunk = mono_compile_method(func);
		FunctionsMessage("Got the thunk.");

		return thunk;
	}
	return nullptr;
}

int MonoFunctions::ParseSignature(_MonoMethod *func, List<const char *> &names, const char *&params)
{
	MonoMethodSignature *sig = mono_method_signature(func);
	
	int paramCount = mono_signature_get_param_count(sig);
	
	names = List<const char *>(paramCount);

	TextBuilder paramsText = TextBuilder(100);
	
	void *iter = nullptr;
	while (MonoType *paramType = mono_signature_get_params(sig, &iter))
	{
		if (names.Length != 0)
		{
			paramsText << ",";
		}
		
		const char *typeName = NtText(mono_type_get_name(paramType)).Detach();
		
		paramsText << typeName;
		
		names.Add(typeName);
	}

	params = paramsText.ToNTString();

	return paramCount;
}

void MonoFunctions::GetParameterClasses(_MonoMethod *func, List<IMonoClass *> &classes)
{
	static IMonoClass *arrayClass = nullptr;
	static IMonoClass *intPtrClass = nullptr;

	if (!arrayClass)
	{
		arrayClass = MonoEnv->CoreLibrary->GetClass("System", "Array");
	}
	if (!intPtrClass)
	{
		intPtrClass = MonoEnv->CoreLibrary->GetClass("System", "IntPtr");
	}

	MonoMethodSignature *sig = mono_method_signature(func);

	void *iter = nullptr;
	while (MonoType *paramType = mono_signature_get_params(sig, &iter))
	{
		MonoTypeEnum typeId = MonoTypeEnum(mono_type_get_type(paramType));
		if (typeId == MonoTypeEnum::MONO_TYPE_ARRAY ||
			typeId == MonoTypeEnum::MONO_TYPE_SZARRAY)
		{
			classes.Add(arrayClass);
		}
		else if (typeId == MonoTypeEnum::MONO_TYPE_PTR)
		{
			classes.Add(intPtrClass);
		}
		else
		{
			classes.Add(MonoClassCache::Wrap(mono_class_from_mono_type(paramType)));
		}
	}
}

mono::object MonoFunctions::GetReflectionObject(_MonoMethod *func)
{
	return mono::object(mono_method_get_object(mono_domain_get(), func, nullptr));
}
