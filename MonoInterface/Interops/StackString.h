#pragma once

#include "IMonoInterface.h"

struct StackStringInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "StackString"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Utilities"; }

	virtual void InitializeInterops() override;

	static void AssignString(stack_string *ptr, mono::string str);
	static mono::string GetString(stack_string *ptr);
};