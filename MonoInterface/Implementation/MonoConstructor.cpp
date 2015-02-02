#include "stdafx.h"

#include "MonoConstructor.h"
#include "MonoClass.h"

MonoConstructor::MonoConstructor(MonoMethod *method)
{
	this->wrappedMethod = method;
	this->signature = mono_method_signature(this->wrappedMethod);
	this->paramCount = mono_signature_get_param_count(this->signature);

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


mono::object MonoConstructor::Invoke(void *object, mono::exception *exc /*= nullptr*/, bool polymorph /*= false*/)
{
	if (!object)
	{
		object = mono_object_new(mono_domain_get(), mono_method_get_class(this->wrappedMethod));
	}
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke(this->wrappedMethod, object, nullptr, &exception);
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

mono::object MonoConstructor::Invoke(void *object, IMonoArray *params, mono::exception *exc /*= nullptr*/, bool polymorph /*= false*/)
{
	if (!object)
	{
		object = mono_object_new(mono_domain_get(), mono_method_get_class(this->wrappedMethod));
	}
	MonoArray *paramsArray = (MonoArray *)params->GetWrappedPointer();
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke_array(this->wrappedMethod, object, paramsArray, &exception);
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

mono::object MonoConstructor::Invoke(void *object, void **params, mono::exception *exc /*= nullptr*/, bool polymorph /*= false*/)
{
	if (!object)
	{
		object = mono_object_new(mono_domain_get(), mono_method_get_class(this->wrappedMethod));
	}
	MonoObject *exception;
	MonoObject *result = mono_runtime_invoke(this->wrappedMethod, object, params, &exception);
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

void *MonoConstructor::GetThunk()
{
	return mono_method_get_unmanaged_thunk(this->wrappedMethod);
}

const char *MonoConstructor::GetName()
{
	return ".ctor";
}

int MonoConstructor::GetParameterCount()
{
	return this->paramCount;
}

List<const char *> *MonoConstructor::GetParameterTypeNames()
{
	return &this->paramTypeNames;
}

List<IMonoClass *> *MonoConstructor::GetParameterClasses()
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

const char *MonoConstructor::GetParametersList()
{
	return this->paramList;
}

void *MonoConstructor::GetWrappedPointer()
{
	return this->wrappedMethod;
}