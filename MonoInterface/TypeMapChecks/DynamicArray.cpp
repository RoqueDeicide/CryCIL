#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct DynamicArray
{
	//void *vTable;
	SMeshSubset *ptr;

	explicit DynamicArray(DynArray<SMeshSubset> &other)
	{
		CHECK_TYPES_SIZE(DynamicArray, DynArray<SMeshSubset>);
	}
};