#include "stdafx.h"
#include "API_ImplementationHeaders.h"

MonoMethodWrapper::MonoMethodWrapper(MonoMethod *method, IMonoClass *klass)
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

	this->paramClasses   = List<IMonoClass *>(5);
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

mono::object MonoMethodWrapper::Invoke
(
	void *object,
	IMonoArray *params,
	mono::exception *exc,
	bool polymorph
)
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
	MonoArray *paramsArray = (MonoArray *)params->GetWrappedPointer();
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke_array(methodToInvoke, object, paramsArray, &exception);
	if (exception)
	{
		if (exc)
		{
			*exc = (mono::exception)exception;
		}
		else
		{
			MonoEnv->HandleException((mono::exception)exception);
		}
		return nullptr;
	}
	return (mono::object)result;
}

mono::object MonoMethodWrapper::Invoke
(
	void *object,
	void **params,
	mono::exception *exc,
	bool polymorph
)
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
	MonoObject *result = mono_runtime_invoke(methodToInvoke, object, params, &exception);
	if (exception)
	{
		if (exc)
		{
			*exc = (mono::exception)exception;
		}
		else
		{
			MonoEnv->HandleException((mono::exception)exception);
		}
		return nullptr;
	}
	return (mono::object)result;
}

mono::object MonoMethodWrapper::Invoke(void *object, mono::exception *exc /*= nullptr*/, bool polymorph /*= false */)
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
	MonoObject *result = mono_runtime_invoke(methodToInvoke, object, nullptr, &exception);
	if (exception)
	{
		if (exc)
		{
			*exc = (mono::exception)exception;
		}
		else
		{
			MonoEnv->HandleException((mono::exception)exception);
		}
		return nullptr;
	}
	return (mono::object)result;
}

void *MonoMethodWrapper::GetThunk()
{
	return mono_method_get_unmanaged_thunk(this->wrappedMethod);
}

const char *MonoMethodWrapper::GetName()
{
	return this->name;
}

int MonoMethodWrapper::GetParameterCount()
{
	return this->paramCount;
}

void *MonoMethodWrapper::GetWrappedPointer()
{
	return this->wrappedMethod;
}

List<const char *> *MonoMethodWrapper::GetParameterTypeNames()
{
	return &this->paramTypeNames;
}

List<IMonoClass *> *MonoMethodWrapper::GetParameterClasses()
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

const char *MonoMethodWrapper::GetParametersList()
{
	return this->paramList;
}

void *MonoMethodWrapper::GetFunctionPointer()
{
	if (!CompileMethod)
	{
		CryLogAlways("Getting the compile method's thunk.");
		CompileMethod = (CompileMethodThunk)MonoEnv->CoreLibrary
												   ->GetClass("System", "RuntimeMethodHandle")
												   ->GetMethod("GetFunctionPointer", 1)
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

IMonoClass *MonoMethodWrapper::GetDeclaringClass()
{
	return this->klass;
}

CompileMethodThunk MonoMethodWrapper::CompileMethod = nullptr;
