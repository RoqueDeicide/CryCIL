#include "stdafx.h"

#include "StackString.h"

void StackStringInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(AssignString);
	REGISTER_METHOD(GetString);
}

void StackStringInterop::AssignString(stack_string *ptr, mono::string str)
{
	ptr->assign(NtText(str));
}

mono::string StackStringInterop::GetString(stack_string *ptr)
{
	return ToMonoString(*ptr);
}
