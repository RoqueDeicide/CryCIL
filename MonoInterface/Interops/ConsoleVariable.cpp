#include "stdafx.h"

#include "ConsoleVariable.h"

void ConsoleVariableInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(Release);
	REGISTER_METHOD(ClearFlags);
	REGISTER_METHOD(GetFlags);
	REGISTER_METHOD(SetFlags);
	REGISTER_METHOD(GetInt);
	REGISTER_METHOD(GetFloat);
	REGISTER_METHOD(GetString);
	REGISTER_METHOD(SetString);
	REGISTER_METHOD(SetFloat);
	REGISTER_METHOD(SetInt);
	REGISTER_METHOD(GetVariableType);
	REGISTER_METHOD(GetNameVar);
	REGISTER_METHOD(GetHelp);
}

void ConsoleVariableInterop::Release(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		v->Release();
	}
}

void ConsoleVariableInterop::ClearFlags(mono::object handle, int flags)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		v->ClearFlags(flags);
	}
}

int ConsoleVariableInterop::GetFlags(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return v->GetFlags();
	}
	return 0;
}

int ConsoleVariableInterop::SetFlags(mono::object handle, int flags)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return v->SetFlags(flags);
	}
	return 0;
}

int ConsoleVariableInterop::GetInt(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return v->GetIVal();
	}
	return 0;
}

float ConsoleVariableInterop::GetFloat(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return v->GetFVal();
	}
	return 0;
}

mono::string ConsoleVariableInterop::GetString(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return ToMonoString(v->GetString());
	}
	return nullptr;
}

void ConsoleVariableInterop::SetString(mono::object handle, mono::string s)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v && s)
	{
		v->Set(ToNativeString(s));
	}
}

void ConsoleVariableInterop::SetFloat(mono::object handle, float f)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		v->Set(f);
	}
}

void ConsoleVariableInterop::SetInt(mono::object handle, int i)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		v->Set(i);
	}
}

int ConsoleVariableInterop::GetVariableType(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return v->GetType();
	}
	return 0;
}

mono::string ConsoleVariableInterop::GetNameVar(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return ToMonoString(v->GetName());
	}
	return nullptr;
}

mono::string ConsoleVariableInterop::GetHelp(mono::object handle)
{
	ICVar *v = *GET_BOXED_OBJECT_DATA(ICVar *, handle);
	if (v)
	{
		return ToMonoString(v->GetHelp());
	}
	return nullptr;
}
