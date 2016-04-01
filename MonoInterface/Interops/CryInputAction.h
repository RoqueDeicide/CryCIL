#pragma once

#include "IMonoInterface.h"

struct ActionInputSpecification;
struct IActionMapAction;

struct CryInputActionInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryInputAction"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Input.ActionMapping"; }

	virtual void InitializeInterops() override;

	static bool AddInputInternal(IActionMapAction *handle, EKeyId input, ActionInputSpecification spec);
	static bool RemoveInputInternal(IActionMapAction *handle, EKeyId input);
	static bool RebindInputInternal(IActionMapAction *handle, EKeyId oldInput, EKeyId newInput);
};
