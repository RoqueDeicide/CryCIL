#pragma once

#include <functional>

#ifdef CRYCIL_MODULE
#include <ISystem.h>

#define FatalError(message) CryFatalError(message)

#else

#include <stdexcept>
#define FatalError(message) throw std::logic_error(message)

#endif // CRYCIL_MODULE

//! Represents a read-only list of items.
//!
//! @tparam ElementType        Type that represents items this list contains.
template<typename ElementType>
class ReadOnlyList
{
private:
	ElementType *elements;
	int length;
	int capacity;
public:

	//! Gets or sets capacity of this list.
	__declspec(property(get = GetCapacity)) int Capacity;
	int GetCapacity()
	{
		return this->capacity;
	}
	//! Gets or sets number of elements within this list.
	__declspec(property(get = GetLength)) int Length;
	int GetLength()
	{
		return this->length;
	}
	//! Returns a reference to the element at specified index. No checks are performed.
	const ElementType &operator[](int index) const
	{
		return this->elements[index];
	}
	//! Returns a reference to the element at specified index. Index is clamped into the range.
	const ElementType &At(int index) const
	{
		if (index < 0)
		{
			index = 0;
		}
		if (index >= this->length)
		{
			index = this->length - 1;
		}
		return this->elements[index];
	}
};