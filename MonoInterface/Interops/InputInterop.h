#pragma once

#include "IMonoInterface.h"

#include <IInput.h>

typedef void(__stdcall *OnCharacterInputThunk)(uint32, bool *, mono::exception *);
typedef void(__stdcall *OnKeyChangedThunk)(unsigned int, int, bool, bool *, mono::exception *);

typedef void(__stdcall *OnMouseWheelThunk)(int, int, float, bool *, mono::exception *);
typedef void(__stdcall *OnAxisMoveThunk)(int, float, bool *, mono::exception *);

typedef void(__stdcall *OnGamepadButtonThunk)(unsigned int, unsigned char, bool, bool *, mono::exception *);
typedef void(__stdcall *OnTriggerThunk)(unsigned int, unsigned char, int, float, bool *, mono::exception *);
typedef void(__stdcall *OnThumbAxisMoveThunk)(int, unsigned char, float, bool *, mono::exception *);

typedef void(__stdcall *OnTouchEventThunk)(int, unsigned char, unsigned char, float, float, mono::exception *);

struct InputInterop
	: public IMonoInterop<false, true>
	
	, public IInputEventListener
	, public ITouchEventListener
{
	~InputInterop()
	{
		if (!gEnv || !gEnv->pInput)
		{
			return;
		}

		gEnv->pInput->RemoveEventListener(this);
		gEnv->pInput->RemoveTouchEventListener(this);
	}
	//! Not used since this class defines internal calls for multiple classes.
	virtual const char *GetName() { return ""; }

	virtual const char *GetNameSpace() { return "CryCil.Engine.Input"; }

	virtual void OnRunTimeInitialized();

	virtual bool OnInputEvent(const SInputEvent &_event);
	virtual bool OnInputEventUI(const SUnicodeEvent &_event);

	virtual void OnTouchEvent(const STouchEvent& _event);
	
	static bool thunksInitialized;

	static void XboxRumble(float time, float strengthLeft, float strengthRight, byte deviceIndex);
	static void XboxSetDeadzone(float value);
	static void XboxRestoreDeadzone();
	static bool GamepadConnected(uint16 index);

	static bool DeviceAvailable(int deviceType);
	static int GetModifiers();
	static void ClearKeys();
	static void ClearAnalogInputs();

	// Keyboard.
	static OnKeyChangedThunk onKeyChanged;
	static OnCharacterInputThunk onCharacterInput;
	// Mouse.
	static OnKeyChangedThunk onMouseButton;
	static OnMouseWheelThunk onWheelUp;
	static OnMouseWheelThunk onWheelDown;
	static OnAxisMoveThunk onX;
	static OnAxisMoveThunk onY;
	static OnAxisMoveThunk onZ;
	// Xbox gamepad.
	static OnGamepadButtonThunk onXboxButton;
	static OnTriggerThunk onLeftTrigger;
	static OnTriggerThunk onRightTrigger;
	static OnThumbAxisMoveThunk onLeftThumbX;
	static OnThumbAxisMoveThunk onLeftThumbY;
	static OnThumbAxisMoveThunk onRightThumbX;
	static OnThumbAxisMoveThunk onRightThumbY;
	static OnGamepadButtonThunk onThumbDirection;
	// Touch screens.
	static OnTouchEventThunk onTouchEvent;
};