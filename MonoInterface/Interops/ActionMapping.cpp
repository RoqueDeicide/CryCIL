#include "stdafx.h"

#include "ActionMapping.h"
#include <Implementation/MonoClass.h>
#include <ActionMap.h>
#include <CryCrc32.h>
#include "MonoCryXmlNode.h"

typedef void(__stdcall *ActivateActionThunk)(uint32, int32, float value, mono::exception *);

void ActionMappingInterop::OnAction(const ActionId& action, int activationMode, float value)
{
	static ActivateActionThunk activate =
		ActivateActionThunk(MonoEnv->Cryambly->GetClass(this->GetInteropNameSpace(), this->GetInteropClassName())
											 ->GetFunction("ActivateAction", -1)->UnmanagedThunk);

	mono::exception ex;
	activate(CCrc32::ComputeLowercase(action.c_str()), activationMode, value, &ex);
}

void ActionMappingInterop::OnRunTimeInitialized()
{
	const char *nameSpace = this->GetInteropNameSpace();
	const char *actionMapsClass = "ActionMaps";
	const char *cryActionMapClass = "CryActionMap";
	const char *cryInputActionClass = "CryInputAction";
	MonoEnv->Functions->AddInternalCall(nameSpace, actionMapsClass, "AddDeviceMapping", AddDeviceMapping);
	MonoEnv->Functions->AddInternalCall(nameSpace, actionMapsClass, "GetActionEventField", GetActionEventField);
	MonoEnv->Functions->AddInternalCall(nameSpace, actionMapsClass, "acquireActionHandler", acquireActionHandler);
	MonoEnv->Functions->AddInternalCall(nameSpace, actionMapsClass, "CreateActionMap", CreateActionMap);
	MonoEnv->Functions->AddInternalCall(nameSpace, actionMapsClass, "EnableActionMap", EnableActionMap);
	MonoEnv->Functions->AddInternalCall(nameSpace, actionMapsClass, "SyncRebindDataWithFile", SyncRebindDataWithFile);
	MonoEnv->Functions->AddInternalCall(nameSpace, actionMapsClass, "SyncRebindDataWithNode", SyncRebindDataWithNode);

	// Internal calls for CryActionMap.
	MonoEnv->Functions->AddInternalCall(nameSpace, cryActionMapClass, "GetAction", GetAction);
	MonoEnv->Functions->AddInternalCall(nameSpace, cryActionMapClass, "CreateActionInternal", CreateActionInternal);
	MonoEnv->Functions->AddInternalCall(nameSpace, cryActionMapClass, "RemoveActionInternal", RemoveActionInternal);

	// Internal calls for CryInputAction.
	MonoEnv->Functions->AddInternalCall(nameSpace, cryInputActionClass, "AddInputInternal", AddInputInternal);
	MonoEnv->Functions->AddInternalCall(nameSpace, cryInputActionClass, "RemoveInputInternal", RemoveInputInternal);
	MonoEnv->Functions->AddInternalCall(nameSpace, cryInputActionClass, "RebindInputInternal", RebindInputInternal);

	MonoEnv->CryAction->GetIActionMapManager()->AddExtraActionListener(this);
}

void ActionMappingInterop::Shutdown()
{
	MonoEnv->CryAction->GetIActionMapManager()->RemoveExtraActionListener(this);
}

int currentDevices;

void ActionMappingInterop::AddDeviceMapping(SupportedInputDevices device)
{
	switch (device)
	{
		case eSID_None:
			break;
		case eSID_KeyboardMouse:
			if ((currentDevices & EActionInputDevice::eAID_KeyboardMouse) == 0)
			{
				MonoEnv->CryAction->GetIActionMapManager()->AddInputDeviceMapping(eAID_KeyboardMouse, "keyboard");
				currentDevices |= eAID_KeyboardMouse;
			}
			break;
		case eSID_XboxPad:
			if ((currentDevices & EActionInputDevice::eAID_XboxPad) == 0)
			{
				MonoEnv->CryAction->GetIActionMapManager()->AddInputDeviceMapping(eAID_XboxPad, "xboxpad");
				currentDevices |= eAID_KeyboardMouse;
			}
			break;
		case eSID_OrbisPad:
			if ((currentDevices & EActionInputDevice::eAID_PS4Pad) == 0)
			{
				MonoEnv->CryAction->GetIActionMapManager()->AddInputDeviceMapping(eAID_PS4Pad, "ps4pad");
				currentDevices |= eAID_KeyboardMouse;
			}
			break;
		default: break;
	}
}

IMonoField *ActionMappingInterop::GetActionEventField(MonoClassField *fieldHandle)
{
	IMonoClass *eventClass = MonoClassCache::Wrap(mono_field_get_parent(fieldHandle));

	return eventClass->GetField(mono_field_get_name(fieldHandle));
}

mono::delegat ActionMappingInterop::acquireActionHandler(IMonoField *actionField)
{
	return actionField->Get<mono::delegat>(nullptr);
}

IActionMap *ActionMappingInterop::CreateActionMap(mono::string name)
{
	return MonoEnv->CryAction->GetIActionMapManager()->CreateActionMap(NtText(name));
}

void ActionMappingInterop::EnableActionMap(mono::string name, bool enable)
{
	MonoEnv->CryAction->GetIActionMapManager()->EnableActionMap(NtText(name), enable);
}

IActionMapAction *ActionMappingInterop::GetAction(IActionMap *handle, mono::string name)
{
	ActionId id = ActionId(NtText(name));
	return handle->GetAction(id);
}

bool ActionMappingInterop::SyncRebindDataWithFile(mono::string file, bool save)
{
	if (!file)
	{
		return false;
	}

	NtText fileName(file);

	if (fileName.Length == 0)
	{
		return false;
	}

	XmlNodeRef root;

	if (save)
	{
		root = gEnv->pSystem->CreateXmlNode("rebindData");

		bool syncedWithNode = MonoEnv->CryAction->GetIActionMapManager()->SaveRebindDataToXML(root);

		return syncedWithNode && root->saveToFile(fileName);
	}

	root = gEnv->pSystem->LoadXmlFromFile(fileName);

	return root != nullptr && MonoEnv->CryAction->GetIActionMapManager()->LoadRebindDataFromXML(root);
}

bool ActionMappingInterop::SyncRebindDataWithNode(MonoCryXmlNode *node, bool save)
{
	if (!node || !node->handle)
	{
		return false;
	}
	
	XmlNodeRef nodeRef(node->handle);

	return save ? MonoEnv->CryAction->GetIActionMapManager()->SaveRebindDataToXML(nodeRef)
				: MonoEnv->CryAction->GetIActionMapManager()->LoadRebindDataFromXML(nodeRef);
}

IActionMapAction *ActionMappingInterop::CreateActionInternal(IActionMap *handle, mono::string name)
{
	ActionId id = ActionId(NtText(name));
	if (handle->CreateAction(id))
	{
		return handle->GetAction(id);
	}
	return nullptr;
}

bool ActionMappingInterop::RemoveActionInternal(IActionMap *handle, mono::string name)
{
	ActionId id = ActionId(NtText(name));
	return handle->RemoveAction(id);
}

const char *GetInputName(EKeyId input)
{
	if (input == eKI_XI_Disconnect)
	{
		return "disconnect";
	}
	return gEnv->pInput->LookupSymbol(eIDT_Unknown, -1, input)->name.c_str();
}

EActionInputDevice GetDeviceFromInput(EKeyId input)
{
	if (input < KI_XINPUT_BASE)
	{
		return eAID_KeyboardMouse;
	}
	if (input < KI_ORBIS_BASE)
	{
		return eAID_XboxPad;
	}
	return eAID_PS4Pad;
}

SActionInput ActionInputSpecification::CreateActionInput(EKeyId input)
{
	SActionInput actionInput;

	actionInput.activationMode                   = this->ActivationMode;
	actionInput.fPressTriggerDelay               = this->PressTriggerDelay;
	actionInput.fPressTriggerDelayRepeatOverride = this->OverridePressTriggerDelayWithRepeat ? 1.0f : -1.0f;
	actionInput.fHoldTriggerDelay                = this->HoldTriggerDelay;
	actionInput.fHoldRepeatDelay                 = this->HoldTriggerRepeatDelay;
	actionInput.fHoldTriggerDelayRepeatOverride  = this->HoldTriggerRepeatDelayOverride;
	actionInput.fReleaseTriggerThreshold         = this->ReleaseTriggerThreshold;
	actionInput.iPressDelayPriority              = this->PressDelayPriority;
	
	const char *inputName = GetInputName(input);
	actionInput.input        = inputName;
	actionInput.defaultInput = inputName;
	actionInput.inputDevice  = GetDeviceFromInput(input);

	if (this->ComparisonOperation != eAACO_None)
	{
		actionInput.analogCompareOp   = this->ComparisonOperation;
		actionInput.fAnalogCompareVal = this->AnalogCompareValue;
	}

	SActionInputBlockData blockData;
	blockData.blockType = this->BlockMode;
	if (blockData.blockType == eAIBT_BlockInputs)
	{
		// Transfer all blocked inputs into block data object.
		IMonoArray<EKeyId> blockedInputs = this->BlockedInputs;

		int blockedInputsCount = blockedInputs.Length;
		for (int i = 0; i < blockedInputsCount; i++)
		{
			SActionInputBlocker blocker;
			blocker.keyId = blockedInputs[i];
			blockData.inputs.push_back(blocker);
		}
	}
	actionInput.inputBlockData = blockData;

	return actionInput;
}

bool ActionMappingInterop::AddInputInternal(IActionMapAction *handle, EKeyId input, ActionInputSpecification spec)
{
	CActionMapAction *action = static_cast<CActionMapAction *>(handle);
	SActionInput actionInput = spec.CreateActionInput(input);

	return action->GetParentActionMap()->AddActionInput(handle->GetActionId(), actionInput);
}

bool ActionMappingInterop::RemoveInputInternal(IActionMapAction *handle, EKeyId input)
{
	CActionMapAction *action = static_cast<CActionMapAction *>(handle);

	return action->GetParentActionMap()->RemoveActionInput(action->GetActionId(), GetInputName(input));
}

bool ActionMappingInterop::RebindInputInternal(IActionMapAction *handle, EKeyId oldInput, EKeyId newInput)
{
	CActionMapAction *action = static_cast<CActionMapAction *>(handle);

	return action->GetParentActionMap()->ReBindActionInput(action->GetActionId(),
														   GetInputName(oldInput),
														   GetInputName(newInput));
}