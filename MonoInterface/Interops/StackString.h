#pragma once

#include "IMonoInterface.h"

struct StackStringInterop : public IMonoInterop < true, true >
{
	virtual const char *GetName() override { return "StackString"; }
	virtual const char *GetNameSpace() override { return "CryCil.Utilities"; }

	virtual void OnRunTimeInitialized() override;

	static void AssignString(stack_string *ptr, mono::string str);
	static mono::string GetString(stack_string *ptr);
};