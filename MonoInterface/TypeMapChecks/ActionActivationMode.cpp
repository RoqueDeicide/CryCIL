#include "stdafx.h"

#include "CheckingBasics.h"
#include <IActionMapManager.h>

TYPE_MIRROR enum ActionActivationMode
{
	eAAM_Invalid_check = 0,
	eAAM_OnPress_check = BIT(0), // Used when the action key is pressed
	eAAM_OnRelease_check = BIT(1), // Used when the action key is released
	eAAM_OnHold_check = BIT(2), // Used when the action key is held
	eAAM_Always_check = BIT(3),

	// Special modifiers.
	eAAM_Retriggerable_check = BIT(4),
	eAAM_NoModifiers_check = BIT(5),
	eAAM_ConsoleCmd_check = BIT(6),
	eAAM_AnalogCompare_check = BIT(7)
};

#define CHECK_ENUM(x) static_assert (ActionActivationMode::x ## _check == EActionActivationMode::x, "EActionActivationMode enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(eAAM_Invalid);
	CHECK_ENUM(eAAM_OnPress);
	CHECK_ENUM(eAAM_OnRelease);
	CHECK_ENUM(eAAM_OnHold);
	CHECK_ENUM(eAAM_Always);
	CHECK_ENUM(eAAM_Retriggerable);
	CHECK_ENUM(eAAM_NoModifiers);
	CHECK_ENUM(eAAM_ConsoleCmd);
	CHECK_ENUM(eAAM_AnalogCompare);
}