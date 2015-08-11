#include "stdafx.h"

#include "CheckingBasics.h"
#include <IActionMapManager.h>

TYPE_MIRROR enum ActionInputBlockType
{
	eAIBT_None_check = 0,
	eAIBT_BlockInputs_check,
	eAIBT_Clear_check
};

#define CHECK_ENUM(x) static_assert (ActionInputBlockType::x ## _check == EActionInputBlockType::x, "EActionInputBlockType enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(eAIBT_None);
	CHECK_ENUM(eAIBT_BlockInputs);
	CHECK_ENUM(eAIBT_Clear);
}