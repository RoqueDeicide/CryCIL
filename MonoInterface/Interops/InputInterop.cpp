#include "stdafx.h"

#include "InputInterop.h"

template<typename thunkT>
thunkT getThunk(IMonoClass *klass, const char *name)
{
	return (thunkT)klass->GetFunction(name, -1)->UnmanagedThunk;
}

void InputInterop::OnRunTimeInitialized()
{
	gEnv->pInput->AddEventListener(this);
	gEnv->pInput->AddTouchEventListener(this, "CryCilInput");

	auto cryambly = MonoEnv->Cryambly;
	
	auto touchClass = cryambly->GetClass(this->GetNameSpace(), "Touch");
	auto mouseClass = cryambly->GetClass(this->GetNameSpace(), "Mouse");
	auto xboxClass = cryambly->GetClass(this->GetNameSpace(), "XboxGamepad");
	auto keyboardClass = cryambly->GetClass(this->GetNameSpace(), "Keyboard");
	auto hmdClass = cryambly->GetClass(this->GetNameSpace(), "HeadMountedDevice");

	// Touchy stuff.
	onTouchEvent = getThunk<OnTouchEventThunk>(touchClass, "OnEvent");
	// Xbox controller.
	onThumbDirection = getThunk<OnGamepadButtonThunk>(xboxClass, "OnThumbDirection");
	onRightThumbY    = getThunk<OnThumbAxisMoveThunk>(xboxClass, "OnRightThumbY");
	onRightThumbX    = getThunk<OnThumbAxisMoveThunk>(xboxClass, "OnRightThumbX");
	onLeftThumbY     = getThunk<OnThumbAxisMoveThunk>(xboxClass, "OnLeftThumbY");
	onLeftThumbX     = getThunk<OnThumbAxisMoveThunk>(xboxClass, "OnLeftThumbX");
	onRightTrigger   = getThunk<OnTriggerThunk>      (xboxClass, "OnRightTrigger");
	onLeftTrigger    = getThunk<OnTriggerThunk>      (xboxClass, "OnLeftTrigger");
	onXboxButton     = getThunk<OnGamepadButtonThunk>(xboxClass, "OnButton");
	// Mouse.
	onZ           = getThunk<OnAxisMoveThunk>  (mouseClass, "OnZ");
	onY           = getThunk<OnAxisMoveThunk>  (mouseClass, "OnY");
	onX           = getThunk<OnAxisMoveThunk>  (mouseClass, "OnX");
	onWheelDown   = getThunk<OnMouseWheelThunk>(mouseClass, "OnWheelDown");
	onWheelUp     = getThunk<OnMouseWheelThunk>(mouseClass, "OnWheelUp");
	onMouseButton = getThunk<OnKeyChangedThunk>(mouseClass, "OnButton");
	// Keyboard.
	onCharacterInput = getThunk<OnCharacterInputThunk>(keyboardClass, "OnCharacterInput");
	onKeyChanged     = getThunk<OnKeyChangedThunk>    (keyboardClass, "OnKeyChanged");

	thunksInitialized = true;

	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "XboxGamepad", "Rumble", XboxRumble);
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "XboxGamepad", "SetDeadzone", XboxSetDeadzone);
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "XboxGamepad", "RestoreDeadzone", XboxRestoreDeadzone);
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "XboxGamepad", "Connected", GamepadConnected);
	
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "Inputs", "DeviceAvailable", DeviceAvailable);
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "Inputs", "GetModifiers", GetModifiers);
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "Inputs", "ClearKeys", ClearKeys);
	MonoEnv->Functions->AddInternalCall(this->GetNameSpace(), "Inputs", "ClearAnalogInputs", ClearAnalogInputs);
}

bool InputInterop::OnInputEvent(const SInputEvent &_event)
{
	if (!thunksInitialized)
	{
		return false;
	}

	mono::exception ex;
	bool blocked = false;
	switch (_event.deviceType)
	{
	case eIDT_Mouse:
	{
		switch (_event.keyId)
		{
		case eKI_Mouse1:
		case eKI_Mouse2:
		case eKI_Mouse3:
		case eKI_Mouse4:
		case eKI_Mouse5:
		case eKI_Mouse6:
		case eKI_Mouse7:
		case eKI_Mouse8:
		{
			onMouseButton(_event.keyId, _event.modifiers, _event.state == eIS_Pressed, &blocked, &ex);
			break;
		}
		case eKI_MouseWheelUp:
		{
			onWheelUp(_event.modifiers, _event.state, _event.value, &blocked, &ex);
			break;
		}
		case eKI_MouseWheelDown:
		{
			onWheelDown(_event.modifiers, _event.state, _event.value, &blocked, &ex);
			break;
		}
		case eKI_MouseX:
		{
			onX(_event.modifiers, _event.value, &blocked, &ex);
			break;
		}
		case eKI_MouseY:
		{
			onY(_event.modifiers, _event.value, &blocked, &ex);
			break;
		}
		case eKI_MouseZ:
		{
			onZ(_event.modifiers, _event.value, &blocked, &ex);
			break;
		}
		default:
			break;
		}
		break;
	}
	case eIDT_Keyboard:
	{
		onKeyChanged(_event.keyId, _event.modifiers, _event.state == eIS_Pressed, &blocked, &ex);
		break;
	}
	case eIDT_Joystick:
		break;
	case eIDT_Gamepad:
		if (_event.keyId < eKI_PS3_Select)
		{
			switch (_event.keyId)
			{
			case eKI_XI_DPadUp:
			case eKI_XI_DPadDown:
			case eKI_XI_DPadLeft:
			case eKI_XI_DPadRight:
			case eKI_XI_Start:
			case eKI_XI_Back:
			case eKI_XI_ThumbL:
			case eKI_XI_ThumbR:
			case eKI_XI_ShoulderL:
			case eKI_XI_ShoulderR:
			case eKI_XI_A:
			case eKI_XI_B:
			case eKI_XI_X:
			case eKI_XI_Y:
			{
				onXboxButton(_event.keyId, _event.deviceIndex, _event.state == eIS_Pressed, &blocked, &ex);
				break;
			}
			case eKI_XI_TriggerL:
			case eKI_XI_TriggerLBtn:
			{
				onLeftTrigger(_event.keyId, _event.deviceIndex, _event.state, _event.value, &blocked, &ex);
				break;
			}
			case eKI_XI_TriggerR:
			case eKI_XI_TriggerRBtn:
			{
				onRightTrigger(_event.keyId, _event.deviceIndex, _event.state, _event.value, &blocked, &ex);
				break;
			}
			case eKI_XI_ThumbLX:
			{
				onLeftThumbX(_event.state, _event.deviceIndex, _event.value, &blocked, &ex);
				break;
			}
			case eKI_XI_ThumbLY:
			{
				onLeftThumbY(_event.state, _event.deviceIndex, _event.value, &blocked, &ex);
				break;
			}
			case eKI_XI_ThumbLUp:
			case eKI_XI_ThumbLDown:
			case eKI_XI_ThumbLLeft:
			case eKI_XI_ThumbLRight:
			case eKI_XI_ThumbRUp:
			case eKI_XI_ThumbRDown:
			case eKI_XI_ThumbRLeft:
			case eKI_XI_ThumbRRight:
			{
				onThumbDirection(_event.keyId, _event.deviceIndex, _event.state == eIS_Pressed, &blocked, &ex);
				break;
			}
			case eKI_XI_ThumbRX:
			{
				onRightThumbX(_event.state, _event.deviceIndex, _event.value, &blocked, &ex);
				break;
			}
			case eKI_XI_ThumbRY:
			{
				onRightThumbY(_event.state, _event.deviceIndex, _event.value, &blocked, &ex);
				break;
			}
			default:
				break;
			}
		}
		break;
	default:
		break;
	}
	return blocked;
}

bool InputInterop::OnInputEventUI(const SInputEvent &_event)
{
	if (!thunksInitialized)
	{
		return false;
	}

	mono::exception ex;
	bool blocked;

	onCharacterInput(_event.inputChar, _event.modifiers, &blocked, &ex);

	return blocked;
}


void InputInterop::OnTouchEvent(const STouchEvent& _event)
{
	if (!thunksInitialized)
	{
		return;
	}

	mono::exception ex;
	onTouchEvent(_event.deviceType, _event.deviceIndex, _event.id, _event.pos.x, _event.pos.y, &ex);
}

void InputInterop::XboxRumble(float time, float strengthLeft, float strengthRight, byte deviceIndex)
{
	if (!gEnv || !gEnv->pInput)
	{
		return;
	}

	SFFOutputEvent effect;
	effect.amplifierS = strengthLeft;
	effect.amplifierA = strengthRight;
	effect.timeInSeconds = time;
	effect.deviceType = eIDT_Gamepad;
	effect.eventId = time < 0.001 ? eFF_Rumble_Frame : eFF_Rumble_Basic;

	gEnv->pInput->ForceFeedbackSetDeviceIndex(deviceIndex);
	gEnv->pInput->ForceFeedbackEvent(effect);
}

void InputInterop::XboxSetDeadzone(float value)
{
	if (!gEnv || !gEnv->pInput)
	{
		return;
	}

	gEnv->pInput->SetDeadZone(value);
}

void InputInterop::XboxRestoreDeadzone()
{
	if (!gEnv || !gEnv->pInput)
	{
		return;
	}

	gEnv->pInput->RestoreDefaultDeadZone();
}

bool InputInterop::DeviceAvailable(int deviceType)
{
	if (!gEnv || !gEnv->pInput)
	{
		return false;
	}

	return gEnv->pInput->HasInputDeviceOfType((EInputDeviceType)deviceType);
}

bool InputInterop::GamepadConnected(uint16 index)
{
	if (!gEnv || !gEnv->pInput)
	{
		return false;
	}

	return gEnv->pInput->GetDevice(index, eIDT_Gamepad) != 0;
}

int InputInterop::GetModifiers()
{
	if (!gEnv || !gEnv->pInput)
	{
		return 0;
	}

	return gEnv->pInput->GetModifiers();
}

void InputInterop::ClearKeys()
{
	if (!gEnv || !gEnv->pInput)
	{
		return;
	}

	gEnv->pInput->ClearKeyState();
}

void InputInterop::ClearAnalogInputs()
{
	if (!gEnv || !gEnv->pInput)
	{
		return;
	}

	gEnv->pInput->ClearAnalogKeyState();
}

OnTouchEventThunk InputInterop::onTouchEvent;

OnGamepadButtonThunk InputInterop::onThumbDirection;
OnThumbAxisMoveThunk InputInterop::onRightThumbY;
OnThumbAxisMoveThunk InputInterop::onRightThumbX;
OnThumbAxisMoveThunk InputInterop::onLeftThumbY;
OnThumbAxisMoveThunk InputInterop::onLeftThumbX;
OnTriggerThunk InputInterop::onRightTrigger;
OnTriggerThunk InputInterop::onLeftTrigger;
OnGamepadButtonThunk InputInterop::onXboxButton;

OnAxisMoveThunk InputInterop::onYaw;
OnAxisMoveThunk InputInterop::onRoll;
OnAxisMoveThunk InputInterop::onPitch;

OnAxisMoveThunk InputInterop::onZ;
OnAxisMoveThunk InputInterop::onY;
OnAxisMoveThunk InputInterop::onX;
OnMouseWheelThunk InputInterop::onWheelDown;
OnMouseWheelThunk InputInterop::onWheelUp;
OnKeyChangedThunk InputInterop::onMouseButton;

OnCharacterInputThunk InputInterop::onCharacterInput;
OnKeyChangedThunk InputInterop::onKeyChanged;

bool InputInterop::thunksInitialized = false;
