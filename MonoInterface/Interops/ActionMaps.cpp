#include "stdafx.h"

#include "ActionMaps.h"
#include "MonoCryXmlNode.h"

SortedList<unsigned int, ManagedActionMap *> ActionMapsInterop::ManagedActionMaps;

void ActionMapsInterop::InitializeInterops()
{
	REGISTER_METHOD(AddDeviceMapping);
	REGISTER_METHOD(CreateActionMap);
	REGISTER_METHOD(GetActionMap);
	REGISTER_METHOD(CreateInternalActionMap);
	REGISTER_METHOD(SyncRebindDataWithFile);
	REGISTER_METHOD(SyncRebindDataWithNode);
}

int currentDevices;

void ActionMapsInterop::AddDeviceMapping(SupportedInputDevices device)
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

IActionMap *ActionMapsInterop::CreateActionMap(mono::string name)
{
	return MonoEnv->CryAction->GetIActionMapManager()->CreateActionMap(NtText(name));
}

IActionMap *ActionMapsInterop::GetActionMap(mono::string name)
{
	return MonoEnv->CryAction->GetIActionMapManager()->GetActionMap(NtText(name));
}

void ActionMapsInterop::CreateInternalActionMap(IActionMap *actionMap,
												IActionMapAction **actions, int actionCount,
												MonoField **fields, int fieldCount,
												MonoMethod **methods)
{
	if (!actionMap || !actions || actionCount <= 0)
	{
		return;
	}

	unsigned int actionMapNameHash = CCrc32::ComputeLowercase(actionMap->GetName());
	auto managedActionMap = new ManagedActionMap();

	for (int i = 0; i < actionCount; i++)
	{
		ManagedActionHandler handler;
		handler.isEvent = i < fieldCount;
		handler.ptr = handler.isEvent
			? fields[i]
			: static_cast<void *>(mono_method_get_unmanaged_thunk(methods[i - fieldCount]));
		handler.name = mono::string(mono_string_intern(reinterpret_cast<MonoString *>(ToMonoString(actions[i]->GetActionId().c_str()))));

		managedActionMap->handlers.Add(CCrc32::ComputeLowercase(actions[i]->GetActionId().c_str()), handler);
	}

	ManagedActionMaps.Add(actionMapNameHash, managedActionMap);
}

bool ActionMapsInterop::SyncRebindDataWithFile(mono::string file, bool save)
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

bool ActionMapsInterop::SyncRebindDataWithNode(MonoCryXmlNode *node, bool save)
{
	if (!node || !node->handle)
	{
		return false;
	}

	XmlNodeRef nodeRef(node->handle);

	return save ? MonoEnv->CryAction->GetIActionMapManager()->SaveRebindDataToXML(nodeRef)
		: MonoEnv->CryAction->GetIActionMapManager()->LoadRebindDataFromXML(nodeRef);
}
