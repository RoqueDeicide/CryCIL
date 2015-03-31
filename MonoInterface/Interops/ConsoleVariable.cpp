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

void ConsoleVariableInterop::Release(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		v->Release();
	}
}

void ConsoleVariableInterop::ClearFlags(ICVar **handle, int flags)
{
	ICVar *v = *handle;
	if (v)
	{
		v->ClearFlags(flags);
	}
}

int ConsoleVariableInterop::GetFlags(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		return v->GetFlags();
	}
	return 0;
}

int ConsoleVariableInterop::SetFlags(ICVar **handle, int flags)
{
	ICVar *v = *handle;
	if (v)
	{
		return v->SetFlags(flags);
	}
	return 0;
}

int ConsoleVariableInterop::GetInt(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		return v->GetIVal();
	}
	return 0;
}

float ConsoleVariableInterop::GetFloat(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		return v->GetFVal();
	}
	return 0;
}

mono::string ConsoleVariableInterop::GetString(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		return ToMonoString(v->GetString());
	}
	return nullptr;
}

void ConsoleVariableInterop::SetString(ICVar **handle, mono::string s)
{
	ICVar *v = *handle;
	if (v && s)
	{
		v->Set(ToNativeString(s));
	}
}

void ConsoleVariableInterop::SetFloat(ICVar **handle, float f)
{
	ICVar *v = *handle;
	if (v)
	{
		v->Set(f);
	}
}

void ConsoleVariableInterop::SetInt(ICVar **handle, int i)
{
	ICVar *v = *handle;
	if (v)
	{
		v->Set(i);
	}
}

int ConsoleVariableInterop::GetVariableType(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		return v->GetType();
	}
	return 0;
}

mono::string ConsoleVariableInterop::GetNameVar(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		return ToMonoString(v->GetName());
	}
	return nullptr;
}

mono::string ConsoleVariableInterop::GetHelp(ICVar **handle)
{
	ICVar *v = *handle;
	if (v)
	{
		return ToMonoString(v->GetHelp());
	}
	return nullptr;
}
