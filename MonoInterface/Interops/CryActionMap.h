#pragma once

#include "IMonoInterface.h"

struct IActionMap;
struct IActionMapAction;

struct CryActionMapInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryActionMap"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Input.ActionMapping"; }

	virtual void OnRunTimeInitialized() override;

	static IActionMapAction *GetAction(IActionMap *handle, mono::string name);
	static IActionMapAction *CreateActionInternal(IActionMap *handle, mono::string name);
	static bool              RemoveActionInternal(IActionMap *handle, mono::string name);
	static bool              IsEnabled(IActionMap *handle);
	static void              Enable(IActionMap *handle, bool enable);
	static mono::string      GetName(IActionMap *handle);
	static unsigned int      GetNameHash(IActionMap *handle);
};
