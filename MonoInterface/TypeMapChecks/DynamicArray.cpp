#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct DynamicArray
{
	//void *vTable;
	SMeshSubset *ptr;

	explicit DynamicArray(DynArray<SMeshSubset> &)
		: ptr(nullptr)
	{
		CHECK_TYPES_SIZE(DynamicArray, DynArray<SMeshSubset>);
	}
};