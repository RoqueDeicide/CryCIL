#include "stdafx.h"

#include "CryInputAction.h"
#include "ActionInputSpecification.h"
#include <ActionMap.h>

void CryInputActionInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(AddInputInternal);
	REGISTER_METHOD(RemoveInputInternal);
	REGISTER_METHOD(RebindInputInternal);
}

bool CryInputActionInterop::AddInputInternal(IActionMapAction *handle, EKeyId input, ActionInputSpecification spec)
{
	CActionMapAction *action = static_cast<CActionMapAction *>(handle);
	SActionInput actionInput = spec.CreateActionInput(input);

	return action->GetParentActionMap()->AddActionInput(handle->GetActionId(), actionInput);
}

bool CryInputActionInterop::RemoveInputInternal(IActionMapAction *handle, EKeyId input)
{
	CActionMapAction *action = static_cast<CActionMapAction *>(handle);

	return action->GetParentActionMap()->RemoveActionInput(action->GetActionId(), GetInputName(input));
}

bool CryInputActionInterop::RebindInputInternal(IActionMapAction *handle, EKeyId oldInput, EKeyId newInput)
{
	CActionMapAction *action = static_cast<CActionMapAction *>(handle);

	return action->GetParentActionMap()->ReBindActionInput(action->GetActionId(), GetInputName(oldInput),
														   GetInputName(newInput));
}
