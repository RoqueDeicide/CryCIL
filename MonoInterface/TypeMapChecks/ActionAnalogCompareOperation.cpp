#include "stdafx.h"

#include "CheckingBasics.h"
#include <IActionMapManager.h>

TYPE_MIRROR enum ActionAnalogCompareOperation
{
	eAACO_None_check = 0,
	eAACO_Equals_check,
	eAACO_NotEquals_check,
	eAACO_GreaterThan_check,
	eAACO_LessThan_check
};

#define CHECK_ENUM(x) static_assert (ActionAnalogCompareOperation::x ## _check == EActionAnalogCompareOperation::x, "EActionAnalogCompareOperation enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(eAACO_None);
	CHECK_ENUM(eAACO_Equals);
	CHECK_ENUM(eAACO_NotEquals);
	CHECK_ENUM(eAACO_GreaterThan);
	CHECK_ENUM(eAACO_LessThan);
}