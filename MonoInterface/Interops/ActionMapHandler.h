#pragma once

#include "IMonoInterface.h"
#include <IActionMapManager.h>
#include "ActionMaps.h"

struct ActionMapHandlerInterop : IMonoInterop<true, true>, IActionListener
{
private:
	//! Pointer to the object that provides means of executing managed code when activating actions.
	ManagedActionMap *managedActionMap;
	//! A weak GC handle that can be used to access the object to execute the functions on.
	//!
	//! If this field doesn't contain a valid object handle, then functions are executed on a static class.
	MonoGCHandle objHandle;
	//! A pointer to the object that represents the action map this object is listening to.
	IActionMap *actionMap;
	//! Indicates whether this handler is listening to the action map.
	bool active;
	//! Indicates whether this handler's action map is handled by the static class.
	bool isStatic;
public:
	ActionMapHandlerInterop()
		: managedActionMap(nullptr)
		, objHandle(-1)
		, actionMap(nullptr)
		, active(false)
		, isStatic(true)
	{}

	virtual const char *GetInteropClassName() override { return "ActionMapHandler"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Input.ActionMapping"; }

	virtual void InitializeInterops() override;

	virtual void OnAction(const ActionId& action, int activationMode, float value) override;

	static ActionMapHandlerInterop *CreateInternal(mono::object obj, IActionMap *actionMap);
	static void                     DestroyInternal(ActionMapHandlerInterop *handler);
	static bool                     IsActive(ActionMapHandlerInterop *handler);
	static void                     Activate(ActionMapHandlerInterop *handler, bool activate);
};
