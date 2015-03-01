#include "stdafx.h"
#include "MonoFunction.h"
#include "MonoClass.h"

MonoFunction::MonoFunction(MonoMethod *method, IMonoClass *klass)
	: rawThunk(nullptr)
{
	if (klass)
	{
		this->klass = klass;
	}
	else
	{
		MonoClass *methodClass = mono_method_get_class(method);
		if (methodClass)
		{
			this->klass = MonoClassCache::Wrap(methodClass);
		}
		else
		{
			this->klass = nullptr;
		}
	}

	this->wrappedMethod = method;
	this->signature = mono_method_signature(this->wrappedMethod);
	this->paramCount = mono_signature_get_param_count(this->signature);
	this->name = mono_method_get_name(this->wrappedMethod);

	ConstructiveText params = ConstructiveText(100);

	this->paramClasses = List<IMonoClass *>(5);
	this->paramTypeNames = List<const char *>(5);

	void *iter = nullptr;
	while (MonoType *paramType = mono_signature_get_params(this->signature, &iter))
	{
		if (this->paramTypeNames.Length != 0)
		{
			params << ",";
		}
		Text t = Text(mono_type_get_name(paramType));
		const char *typeName = t.ToNTString();
		params << typeName;
		paramTypeNames.Add(typeName);
	}
}

IMonoMethod *MonoFunction::ToInstance()
{
	return dynamic_cast<IMonoMethod *>(this);
}

IMonoStaticMethod *MonoFunction::ToStatic()
{
	return dynamic_cast<IMonoStaticMethod *>(this);
}

IMonoConstructor *MonoFunction::ToCtor()
{
	return dynamic_cast<IMonoConstructor *>(this);
}

void *MonoFunction::GetThunk()
{
	return mono_method_get_unmanaged_thunk(this->wrappedMethod);
}

const char *MonoFunction::GetName()
{
	return this->name;
}

int MonoFunction::GetParameterCount()
{
	return this->paramCount;
}

void *MonoFunction::GetWrappedPointer()
{
	return this->wrappedMethod;
}

List<const char *> *MonoFunction::GetParameterTypeNames()
{
	return &this->paramTypeNames;
}

List<IMonoClass *> *MonoFunction::GetParameterClasses()
{
	if (this->paramClasses.Length == this->paramCount)
	{
		return &this->paramClasses;
	}

	static IMonoClass *arrayClass;
	static IMonoClass *intPtrClass;

	if (!arrayClass)
	{
		arrayClass = MonoEnv->CoreLibrary->GetClass("System", "Array");
	}
	if (!intPtrClass)
	{
		intPtrClass = MonoEnv->CoreLibrary->GetClass("System", "IntPtr");
	}

	void *iter = nullptr;
	while (MonoType *paramType = mono_signature_get_params(this->signature, &iter))
	{
		MonoTypeEnum typeId = (MonoTypeEnum)mono_type_get_type(paramType);
		if (typeId == MonoTypeEnum::MONO_TYPE_ARRAY ||
			typeId == MonoTypeEnum::MONO_TYPE_SZARRAY)
		{
			this->paramClasses.Add(arrayClass);
		}
		else if (typeId == MonoTypeEnum::MONO_TYPE_PTR)
		{
			this->paramClasses.Add(intPtrClass);
		}
		else
		{
			this->paramClasses.Add(MonoClassCache::Wrap(mono_class_from_mono_type(paramType)));
		}
	}

	return &this->paramClasses;
}

const char *MonoFunction::GetParametersList()
{
	return this->paramList;
}

void *MonoFunction::GetFunctionPointer()
{
	if (!CompileMethod)
	{
		CryLogAlways("Getting the compile method's thunk.");
		CompileMethod = (CompileMethodThunk)MonoEnv->CoreLibrary
			->GetClass("System", "RuntimeMethodHandle")
			->GetFunction("GetFunctionPointer", 1)
			->UnmanagedThunk;
		CryLogAlways("Got the compile method's thunk.");
	}

	if (!this->rawThunk)
	{
		ReportMessage("Compiling the raw thunk for the method %s::%s(%s)",
					  this->klass ? (this->klass->FullNameIL) : "",
					  this->name,
					  this->paramList);
		mono::exception ex;
		mono::intptr result = CompileMethod(BoxPtr(this->wrappedMethod), &ex);
		if (!ex)
		{
			this->rawThunk = Unbox<void *>(result);
			ReportMessage("Compilation successful.");
		}
		else
		{
			ReportError("Unable to compile the method into a raw thunk.");
		}
	}

	return this->rawThunk;
}

IMonoClass *MonoFunction::GetDeclaringClass()
{
	return this->klass;
}

mono::object MonoFunction::InternalInvoke(void *object, void **args, mono::exception *ex, bool polymorph)
{
	MonoMethod *methodToInvoke;
	if (polymorph)
	{
		methodToInvoke =
			mono_object_get_virtual_method((MonoObject *)object, this->wrappedMethod);
	}
	else
	{
		methodToInvoke = this->wrappedMethod;
	}
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke(methodToInvoke, object, args, &exception);
	if (exception)
	{
		if (ex)
		{
			*ex = (mono::exception)exception;
		}
		else
		{
#ifdef _DEBUG
			MonoEnv->HandleException((mono::exception)exception);
#else
			MonoObject *messageString = &exception[3];
			const char *message = mono_string_to_utf8((MonoString *)messageString);
			ReportError("Unhandled exception was caught with message: %s", message);
			mono_free(message);
#endif // _DEBUG
		}
		return nullptr;
	}
	return (mono::object)result;
}

mono::object MonoFunction::InternalInvokeArray(void *object, IMonoArray *args, mono::exception *ex, bool polymorph)
{
	MonoMethod *methodToInvoke;
	if (polymorph)
	{
		methodToInvoke =
			mono_object_get_virtual_method((MonoObject *)object, this->wrappedMethod);
	}
	else
	{
		methodToInvoke = this->wrappedMethod;
	}
	MonoArray *paramsArray = args->GetHandle<MonoArray>();
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke_array(methodToInvoke, object, paramsArray, &exception);
	if (exception)
	{
		if (ex)
		{
			*ex = (mono::exception)exception;
		}
		else
		{
#ifdef _DEBUG
			MonoEnv->HandleException((mono::exception)exception);
#else
			MonoObject *messageString = &exception[3];
			const char *message = mono_string_to_utf8((MonoString *)messageString);
			ReportError("Unhandled exception was caught with message: %s", message);
			mono_free(message);
#endif // _DEBUG
		}
		return nullptr;
	}
	return (mono::object)result;
}

CompileMethodThunk MonoFunction::CompileMethod = nullptr;