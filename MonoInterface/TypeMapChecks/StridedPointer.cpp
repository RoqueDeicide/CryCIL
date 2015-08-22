#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct StridedPointerMirror
{
	void *data;
	int iStride;

	explicit StridedPointerMirror(strided_pointer<void> other)
	{
		static_assert(sizeof(strided_pointer<void>) == sizeof(StridedPointerMirror), "strided_pointer<void *> structure has been changed.");
		
		ASSIGN_FIELD(data);
		ASSIGN_FIELD(iStride);

		CHECK_TYPE(data);
		CHECK_TYPE(iStride);
	}
};