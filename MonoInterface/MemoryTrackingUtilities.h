#pragma once

#include "ArrayHeader.h"

//! Provides utilities for tracking amount of allocated memory that is used by the type.
//!
//! @typeparam TrackedType Type to track memory for.
template<typename TrackedType>
struct MemoryTracker
{
	friend TrackedType;
private:
	static size_t &MemoryCounter()
	{
		static size_t counter = 0;
		return counter;
	}
public:
	//! Gets number of bytes that were allocated by the TrackedType.
	static size_t AllocatedMemory()
	{
		return MemoryCounter();
	}
private:
	//! Informs this tracker of allocation of specified number of bytes.
	//!
	//! @param amount Number of bytes that were allocated.
	static void AddMemory(size_t amount)
	{
		MemoryCounter() += amount;
	}
	//! Informs this tracker of deallocation of specified number of bytes.
	//!
	//! @param amount Number of bytes that were deallocated.
	static void RemoveMemory(size_t amount)
	{
		MemoryCounter() -= amount;
	}

	//! Informs this tracker of allocation of specified number of objects.
	//!
	//! @typeparam ObjectType Type of objects that were allocated.
	//!
	//! @param count Number of allocated objects.
	template<typename ObjectType>
	static void RegisterObjects(size_t count)
	{
		AddMemory(count * sizeof(ObjectType));
	}
	//! Informs this tracker of deallocation of specified number of objects.
	//!
	//! @typeparam ObjectType Type of objects that were deallocated.
	//!
	//! @param count Number of deallocated objects.
	template<typename ObjectType>
	static void UnregisterObjects(size_t count)
	{
		RemoveMemory(count * sizeof(ObjectType));
	}
	//! Informs this tracker of allocation of an array of data.
	template<typename DataType, typename ReferenceCountType, typename CountType, bool nullTerminated>
	static void RegisterArray(const RefCountedImmutableArrayHeaderTemplate<DataType, ReferenceCountType, CountType, nullTerminated> &header)
	{
		AddMemory(header.AllocatedMemory);
	}
	//! Informs this tracker of deallocation of an array of data.
	template<typename DataType, typename ReferenceCountType, typename CountType, bool nullTerminated>
	static void UnregisterArray(const RefCountedImmutableArrayHeaderTemplate<DataType, ReferenceCountType, CountType, nullTerminated> &header)
	{
		RemoveMemory(header.AllocatedMemory);
	}
};