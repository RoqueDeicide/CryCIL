#include "stdafx.h"

#include "CheckingBasics.h"
#include <CryCore/stridedptr.h>

TYPE_MIRROR struct StridedPointerMirror
{
	int *data;
	int iStride;

	explicit StridedPointerMirror(strided_pointer<int> other)
	{
		static_assert(sizeof(strided_pointer<int>) == sizeof(StridedPointerMirror), "strided_pointer<int> structure has been changed.");
		
		ASSIGN_FIELD(data);
		ASSIGN_FIELD(iStride);

		CHECK_TYPE(data);
		CHECK_TYPE(iStride);
	}
};