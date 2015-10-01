#pragma once

#include "IMonoInterface.h"
#include <IActionMapManager.h>

struct MonoCryXmlNode;

enum SupportedInputDevices
{
	eSID_None,
	eSID_KeyboardMouse,
	eSID_XboxPad,
	eSID_OrbisPad
};

struct ActionInputSpecification
{
	EActionActivationMode ActivationMode;
	EActionInputBlockType BlockMode;
	mono::Array BlockedInputs;
	float BlockDuration;
	byte BlockedDevice;
	float PressTriggerDelay;
	bool OverridePressTriggerDelayWithRepeat;
	int PressDelayPriority;
	float ReleaseTriggerThreshold;
	float HoldTriggerDelay;
	float HoldTriggerRepeatDelay;
	float HoldTriggerRepeatDelayOverride;
	float AnalogCompareValue;
	EActionAnalogCompareOperation ComparisonOperation;

	SActionInput CreateActionInput(EKeyId input);
};

struct ActionMappingInterop : public IMonoInterop<false, true>, public IActionListener
{
	virtual void OnAction(const ActionId& action, int activationMode, float value) override;

	virtual const char *GetInteropClassName() override { return "ActionMaps"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Input.ActionMapping"; }

	virtual void OnRunTimeInitialized() override;
	virtual void Shutdown() override;

	// Internal calls for ActionMaps.
	static void          AddDeviceMapping(SupportedInputDevices device);
	static IMonoField   *GetActionEventField(MonoClassField *fieldHandle);
	static mono::delegat acquireActionHandler(IMonoField *actionField);
	static IActionMap   *CreateActionMap(mono::string name);
	static void          EnableActionMap(mono::string name, bool enable);
	static bool          SyncRebindDataWithFile(mono::string file, bool save);
	static bool          SyncRebindDataWithNode(MonoCryXmlNode *node, bool save);

	// Internal calls for CryActionMap.
	static IActionMapAction *GetAction(IActionMap *handle, mono::string name);
	static IActionMapAction *CreateActionInternal(IActionMap *handle, mono::string name);
	static bool              RemoveActionInternal(IActionMap *handle, mono::string name);

	// Internal calls for CryInputAction.
	static bool AddInputInternal(IActionMapAction *handle, EKeyId input, ActionInputSpecification spec);
	static bool RemoveInputInternal(IActionMapAction *handle, EKeyId input);
	static bool RebindInputInternal(IActionMapAction *handle, EKeyId oldInput, EKeyId newInput);
};