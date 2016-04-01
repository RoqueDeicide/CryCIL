#pragma once

#include "IMonoInterface.h"
#include <IActionMapManager.h>
#include "ActionInputSpecification.h"

struct MonoField;
struct MonoCryXmlNode;

enum SupportedInputDevices
{
	eSID_None,
	eSID_KeyboardMouse,
	eSID_XboxPad,
	eSID_OrbisPad
};

struct ManagedActionHandler
{
	void *ptr;			//!< A pointer to either a MonoField, or an unmanaged thunk.
	bool isEvent;		//!< Indicates whether @see ptr is field of the event.
	mono::string name;	//!< Pointer to the intern string that represents the name of the action.
};

struct ManagedActionMap
{
	SortedList<unsigned int, ManagedActionHandler> handlers;
};

struct ActionMapsInterop : IMonoInterop<true, true>
{
	//! A sorted list that maps hashes of lower-case versions of names of action maps to objects that help
	//! handle activated actions.
	static SortedList<unsigned int, ManagedActionMap *> ManagedActionMaps;

	virtual const char *GetInteropClassName() override { return "ActionMaps"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Input.ActionMapping"; }

	virtual void InitializeInterops() override;

	static void        AddDeviceMapping(SupportedInputDevices device);
	static IActionMap *CreateActionMap(mono::string name);
	static IActionMap *GetActionMap(mono::string name);
	static void        CreateInternalActionMap(IActionMap *actionMap,
											   IActionMapAction **actions, int actionCount,
											   MonoField **fields, int fieldCount,
											   MonoMethod **methods);
	static bool        SyncRebindDataWithFile(mono::string file, bool save);
	static bool        SyncRebindDataWithNode(MonoCryXmlNode *node, bool save);
};
