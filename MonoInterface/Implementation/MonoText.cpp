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

void MonoText::GetField(const char *name, void *value)
{
	return this->klass->GetField(this->mStr, name, value);
}

void MonoText::SetField(const char *name, void *value)
{
	return this->klass->SetField(this->mStr, name, value);
}

IMonoProperty *MonoText::GetProperty(const char *name)
{
	return this->klass->GetProperty(name);
}

IMonoEvent *MonoText::GetEvent(const char *name)
{
	return this->klass->GetEvent(name);
}

IMonoClass *MonoText::GetClass()
{
	return this->klass;
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
