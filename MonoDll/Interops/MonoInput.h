/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IActionListener mono extension
//////////////////////////////////////////////////////////////////////////
// 02/02/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#include <IMonoInterop.h>

#include <IActionMapManager.h>
#include <IHardwareMouse.h>

#include "MonoCommon.h"

struct IMonoScript;

class InputInterop
	: public IMonoInterop
	, public IActionListener
	, public IHardwareMouseEventListener
	, public IInputEventListener
{
public:
	InputInterop();
	~InputInterop();

	// IMonoScriptBind
	virtual const char *GetClassName() { return "InputInterop"; }
	// ~IMonoScriptBind

	// IActionListener
	virtual void OnAction(const ActionId& action, int activationMode, float value);
	// ~IActionListener

	// IHardwareMouseEventListener
	virtual void OnHardwareMouseEvent(int iX, int iY, EHARDWAREMOUSEEVENT eHardwareMouseEvent, int wheelDelta = 0);
	// ~IHardwareMouseEventListener

	// IInputEventListener
	virtual bool OnInputEvent(const SInputEvent &event);
	// ~IInputEventListener

	IMonoClass *GetClass();

private:
	static void RegisterAction(mono::string);

	bool OnActionTriggered(EntityId entityId, const ActionId& actionId, int activationMode, float value);

	static TActionHandler<InputInterop>	s_actionHandler;
};
