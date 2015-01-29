#pragma once

struct IMonoArrays
{
	//! Creates object of specified type with specified capacity.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param capacity Number of elements that can be held by the array.
	//! @param klass    Pointer to the class that will represent objects within the array.
	//!                 If null, System.Object will be used.
	VIRTUAL_API virtual IMonoArray *Create(int capacity, IMonoClass *klass = nullptr) = 0;
	//! Creates a multi-dimensional array.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param dimCount    Number of dimensions of the array.
	//! @param lengths     An array of lengths of dimensions.
	//! @param klass       Type of elements of the array. If null, System.Object will be used.
	//! @param lowerBounds An optional array of lower bounds of dimensions. If null, zeros will
	//!                    be used.
	VIRTUAL_API virtual IMonoArray *Create
		(int dimCount, unsigned int *lengths, IMonoClass *klass = nullptr, int *lowerBounds = nullptr) = 0;
	//! Wraps already existing Mono array.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param arrayHandle Pointer to the array that needs to be wrapped.
	VIRTUAL_API virtual IMonoArray *Wrap(mono::Array arrayHandle) = 0;
};