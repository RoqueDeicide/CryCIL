#pragma once

#include "IMonoInterface.h"

#include <CryInput/IInput.h>

RAW_THUNK typedef void(*OnCharacterInputThunk)(uint32, bool *);
RAW_THUNK typedef void(*OnKeyChangedThunk)(uint32, int, bool, bool *);

RAW_THUNK typedef void(*OnMouseWheelThunk)(int, int, float, bool *);
RAW_THUNK typedef void(*OnAxisMoveThunk)(int, float, bool *);

RAW_THUNK typedef void(*OnGamepadButtonThunk)(uint32, uint8, bool, bool *);
RAW_THUNK typedef void(*OnTriggerThunk)(uint32, uint8, int, float, bool *);
RAW_THUNK typedef void(*OnThumbAxisMoveThunk)(int, uint8, float, bool *);

RAW_THUNK typedef void(*OnTouchEventThunk)(int, uint8, uint8, float, float);

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
	virtual const char *GetInteropClassName() override { return ""; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Input"; }

	virtual void InitializeInterops() override;

	virtual bool OnInputEvent(const SInputEvent &_event) override;
	virtual bool OnInputEventUI(const SUnicodeEvent &_event) override;

	virtual void OnTouchEvent(const STouchEvent& _event) override;
	
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