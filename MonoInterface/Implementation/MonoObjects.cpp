#include "stdafx.h"

#include "MonoClass.h"
#include "MonoObjects.h"
#include "MonoStaticMethod.h"
#include "MonoMethod.h"
#include "MonoConstructor.h"

#include "MonoDefinitionFiles/MonoDelegate.h"

void *MonoObjects::Unbox(mono::object value)
{
	return mono_object_unbox((MonoObject *)value);
}

IMonoArrays *MonoObjects::GetArrays()
{
	return this->arrays;
}

IMonoTexts *MonoObjects::GetTexts()
{
	return this->texts;
}

IMonoExceptions *MonoObjects::GetExceptions()
{
	return this->exceptions;
}

IMonoDelegates *MonoObjects::GetDelegates()
{
	return this->delegates;
}

IDefaultBoxinator *MonoObjects::GetBoxinator()
{
	return this->boxinator;
}

IMonoThreads *MonoObjects::GetThreads()
{
	return this->threads;
}

int MonoObjects::GetArrayRank(mono::Array ar)
{
	return mono_class_get_rank(mono_object_get_class((MonoObject *)ar));
}

int MonoObjects::GetArrayElementSize(mono::Array ar)
{
	return mono_array_element_size(mono_object_get_class((MonoObject *)ar));
}

IMonoClass *MonoObjects::GetArrayElementClass(mono::Array ar)
{
	return  MonoClassCache::Wrap
			(
				mono_class_get_element_class
				(
					mono_object_get_class((MonoObject *)ar)
				)
			);
}

void MonoObjects::ThrowException(mono::exception ex)
{
	mono_raise_exception((MonoException *)ex);
}

IMonoFunction *MonoObjects::GetDelegateFunction(mono::delegat delegat)
{
	MonoMethod *m = ((MonoDelegate *)delegat)->method;
	if (!m)
	{
		return nullptr;
	}

	IMonoClass *klass = nullptr;

	if (MonoClass *methodClass = mono_method_get_class(m))
	{
		klass = MonoClassCache::Wrap(methodClass);
	}

	if (mono_signature_is_instance(mono_method_signature(m)))
	{
		if (strcmp(mono_method_get_name(m), ".ctor") == 0)
		{
			return new MonoConstructor(m, klass);
		}
		else
		{
			return new MonoMethodWrapper(m, klass);
		}
	}
	else
	{
		return new MonoStaticMethod(m, klass);
	}
}

mono::object MonoObjects::GetDelegateTarget(mono::delegat delegat)
{
	return (mono::object)((MonoDelegate *)delegat)->target;
}

void *MonoObjects::GetDelegateTrampoline(mono::delegat delegat)
{
	static void *(*mono_delegate_to_ftnptr_FnPtr)(mono::delegat) = nullptr;
	
	if (!mono_delegate_to_ftnptr_FnPtr)
	{
		MonoClass *marshalClass =
			mono_class_from_name(mono_get_corlib(), "System.Runtime.InteropServices", "Marshal");
		MonoMethod *method =
			mono_class_get_method_from_name(marshalClass, "GetFunctionPointerForDelegateInternal", 1);
		mono_delegate_to_ftnptr_FnPtr = (void *(*)(mono::delegat))
			mono_lookup_internal_call(method);
	}

	return mono_delegate_to_ftnptr_FnPtr(delegat);
}

mono::object MonoObjects::InvokeDelegate(mono::delegat delegat, void **params, mono::exception *ex)
{
	return (mono::object)mono_runtime_delegate_invoke((MonoObject *)delegat, params, (MonoObject **)ex);
}


bool MonoObjects::StringEquals(mono::string str, mono::string other)
{
	return mono_string_equal((MonoString *)str, (MonoString *)other) != 0;
}

mono::string MonoObjects::InternString(mono::string str)
{
	return (mono::string)mono_string_intern((MonoString *)str);
}

wchar_t &MonoObjects::StringAt(mono::string str, int index)
{
	return ((wchar_t *)mono_string_chars((MonoString *)str))[index];
}

int MonoObjects::GetStringHashCode(mono::string str)
{
	return mono_string_hash((MonoString *)str);
}

bool MonoObjects::IsStringInterned(mono::string str)
{
	return mono_string_is_interned((MonoString *)str) != 0;
}

const char *MonoObjects::StringToNativeUTF8(mono::string str)
{
	MonoError er;
	char *t = mono_string_to_utf8_checked((MonoString *)str, &er);
	if (mono_error_ok(&er))
	{
		ReportError("%s", mono_error_get_message(&er));
		return nullptr;
	}
	int length = strlen(t) + 1;
	char *text = new char[length];
	for (int i = 0; i < length; i++)
	{
		text[i] = t[i];
	}
	mono_free(t);
	return text;
}

const wchar_t *MonoObjects::StringToNativeUTF16(mono::string str)
{
	wchar_t *chars = (wchar_t *)mono_string_to_utf16((MonoString *)str);

	int length = wcslen(chars);

	wchar_t *t = new wchar_t[length + 1];

	for (int i = 0; i < length; i++)
	{
		t[i] = chars[i];
	}
	t[length] = '\0';

	mono_free(chars);

	return t;
}

void MonoObjects::ThreadDetach(mono::Thread thr)
{
	mono_thread_detach((MonoThread *)thr);
}

IMonoClass *MonoObjects::GetObjectClass(mono::object obj)
{
	if (obj)
	{
		return MonoClassCache::Wrap(mono_object_get_class((MonoObject *)obj));
	}
	return nullptr;
}