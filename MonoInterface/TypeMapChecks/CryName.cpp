#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct CryName
{
	const char *str;

	explicit CryName()
	{
		CHECK_TYPES_SIZE(CryName, CCryName);

		this->str = nullptr;
	}
};