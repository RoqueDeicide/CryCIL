#pragma once

struct IMonoArrays
{
	virtual ~IMonoArrays() {}

	//! Creates object of specified type with specified capacity.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param capacity    Number of elements that can be held by the array.
	//! @param klass       Pointer to the class that will represent objects within the array.
	//!                    If null, System.Object will be used.
	//! @param lowerBounds An optional value that specifies the index of the first element of the array.
	VIRTUAL_API virtual mono::Array Create(int capacity, IMonoClass *klass = nullptr,
										   intptr_t lowerBound = 0) = 0;
	//! Creates a multi-dimensional array.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param lengths A list of lengths of each dimension.
	//! @param klass   Type of elements of the array. If null, System.Object will be used.
	VIRTUAL_API virtual mono::Array Create(const List<uintptr_t> &lengths, IMonoClass *klass = nullptr) = 0;
};