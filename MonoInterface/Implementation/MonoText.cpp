#include "stdafx.h"

#include "MonoText.h"
#include "MonoClass.h"

#include "Text.h"

bool MonoText::Equals(IMonoText *other)
{
	return mono_string_equal(this->str, ((MonoText *)other)->str) != 0;
}

bool MonoText::Equals(mono::string other)
{
	return mono_string_equal(this->str, (MonoString *)other) != 0;
}

void MonoText::Intern()
{
	this->str = mono_string_intern(this->str);
}

wchar_t &MonoText::At(int index)
{
	return ((wchar_t *)mono_string_chars(this->str))[index];
}

int MonoText::GetHashCode()
{
	return mono_string_hash(this->str);
}

bool MonoText::IsInterned()
{
	return mono_string_is_interned(this->str) != 0;
}

mono::object MonoText::Get()
{
	return this->mStr;
}

mono::object MonoText::CallMethod(const char *name, IMonoArray *args)
{
	MonoMethod *method =
		mono_class_get_method_from_name(mono_object_get_class(this->obj), name, args->Length);
	MonoObject *exception = nullptr;
	MonoObject *result = mono_runtime_invoke_array
		(method, this->obj, (MonoArray *)args->GetWrappedPointer(), &exception);
	if (exception)
	{
		MonoEnv->HandleException((mono::object)exception);
	}
	else
	{
		return (mono::object)result;
	}
	return nullptr;
}

void MonoText::GetField(const char *name, void *value)
{
	return this->GetClass()->GetField(this->mStr, name, value);
}

void MonoText::SetField(const char *name, void *value)
{
	return this->GetClass()->SetField(this->mStr, name, value);
}

IMonoProperty *MonoText::GetProperty(const char *name)
{
	return this->GetClass()->GetProperty(name);
}

IMonoEvent *MonoText::GetEvent(const char *name)
{
	return this->GetClass()->GetEvent(name);
}

IMonoClass *MonoText::GetClass()
{
	if (!this->type)
	{
		// Cache the type of this object, so we don't have to get it over and over again.
		this->type = MonoClassCache::Wrap(mono_object_get_class(this->obj));
	}
	return this->type;
}

void *MonoText::UnboxObject()
{
	gEnv->pLog->LogError("Attempt to unbox a string object was made.");
	return nullptr;
}

void *MonoText::GetWrappedPointer()
{
	return this->obj;
}

const char *MonoText::ToNativeUTF8()
{
	MonoError er;
	char *t = mono_string_to_utf8_checked(this->str, &er);
	if (mono_error_ok(&er))
	{
		gEnv->pLog->LogError("%s", mono_error_get_message(&er));
		return nullptr;
	}
	Text *text = new Text(t);
	mono_free(t);
	t = const_cast<char *>(text->ToNTString());
	delete text;
	return t;
}

const wchar_t *MonoText::ToNativeUTF16()
{
	wchar_t *chars = (wchar_t *)mono_string_to_utf16(this->str);

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
