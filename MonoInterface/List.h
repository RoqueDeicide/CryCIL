#pragma once

#include <functional>

#ifdef CRYCIL_MODULE
#include <ISystem.h>

#define FatalError(message) CryFatalError(message)

#else

#include <stdexcept>
#define FatalError(message) throw std::logic_error(message)

#endif // CRYCIL_MODULE

//! Represents an expandable list of items.
//!
//! @tparam ElementType        Type that represents items this list contains.
template<typename ElementType>
class List
{
public:
	typedef std::function<void(ElementType)> Enumerator;
	typedef std::function<void(ElementType, int)> EnumeratorIndex;
	typedef std::function<bool(ElementType)> Predicate;
private:
	ElementType *elements;
	int length;
	int capacity;
public:
	//! Creates a default list.
	//!
	//! Default capacity is 10.
	List()
	{
		this->length = 0;
		this->capacity = 10;
		this->elements = (ElementType *)malloc(this->capacity * sizeof(ElementType));
	}
	//! Creates a new list.
	//!
	//! @param initialCapacity Number of items this list is planned to hold.
	List(int initialCapacity)
	{
		this->length = 0;
		this->capacity = initialCapacity;
		this->elements = (ElementType *)malloc(this->capacity * sizeof(ElementType));
	}
	//! Creates a new list.
	//!
	//! @param collection Vector that contains elements to pre-populate this list with.
	List(std::vector<ElementType> &collection)
	{
		this->length = 0;
		int collectionSize = collection.size();
		if (collectionSize <= 0)
		{
			this->capacity = 10;
		}
		this->elements = (ElementType *)malloc(this->capacity * sizeof(ElementType));
		if (collectionSize > 0)
		{
			this->AddRange(collection);
		}
	}
	//! Constructs a deep copy of the list.
	List(List<ElementType> &list)
	{
		this->capacity = list.capacity;
		this->length = list.length;
		this->elements = (ElementType *)malloc(this->capacity * sizeof(ElementType));
		for (int i = 0; i < this->length; i++)
		{
			this->elements[i] = list.elements[i];
		}
	}
	~List()
	{
		this->Dispose();
	}
	//! Places an item at the end of this list.
	//!
	//! @param item Item to add.
	void Add(ElementType item)
	{
		this->Expand(this->length + 1);
		this->elements[this->length++] = item;
	}
	//! Adds a range of items to the end of the list.
	//!
	//! @param collection A collection of elements to add.
	void AddRange(std::vector<ElementType> &collection)
	{
		collectionSize = collection.size();
		if (collectionSize <= 0)
		{
			return;
		}
		this->Expand(this->length + collectionSize);
		for (auto i = collection.begin(); i != collection.end(); i++)
		{
			this->elements[this->length++] = i;
		}
	}
	//! Adds a range of items to the end of the list.
	//!
	//! @param items     Pointer to the first element to add.
	//! @param itemCount Number of elements to add.
	void AddRange(ElementType *items, int itemCount)
	{
		this->Expand(this->length + itemCount);
		for (int i = 0; i < itemCount; i++)
		{
			this->elements[this->length++] = items[i];
		}
	}
	//! Adds a range of items to the end of the list.
	//!
	//! @param items A collection of items to add.
	void AddRange(List<ElementType> *items)
	{
		this->Expand(this->length + items->length);
		for (int i = 0; i < items->length; i++)
		{
			this->elements[this->length++] = items->At(i);
		}
	}
	//! Inserts an item into specific position within the list.
	//!
	//! @param item     Item to insert.
	//! @param position Zero-based index of the position where the item will be located after
	//!                 insertion. If position is less then 0 then it will be equated to zero.
	//!                 If position is greater then length of the list, item will be added to
	//!                 the end of the list.
	void Insert(ElementType &item, int position)
	{
		if (position < 0)
		{
			position = 0;
		}
		if (position > this->length)
		{
			this->Add(item);
		}
		else
		{
			this->Expand(this->length + 1);
			this->MoveRange(position, position + 1, this->length - position);
			this->elements[position] = item;
			this->length++;
		}
	}
	//! Inserts a collection of items into specific position within the list.
	//!
	//! @param items     Pointer to the first element to copy.
	//! @param itemCount Number of elements to copy.
	//! @param position  Zero-based index of the position where the first element of
	//!                  inserted collection will located after insertion.
	//!                  If position is less then 0 then it will be equated to zero.
	//!                  If position is greater then length of the list, item will be
	//!                  added to the end of the list.
	void InsertRange(ElementType *items, int itemCount, int position)
	{
		if (position < 0)
		{
			position = 0;
		}
		if (position > this->length)
		{
			this->AddRange(items, itemCount);
		}
		else
		{
			this->Expand(this->length + 1);
			this->MoveRange(position, position + itemCount, this->length - position + itemCount);
			for (int i = 0; i < itemCount; i++)
			{
				this->elements[position + i] = items[i];
			}
			this->length += itemCount;
		}
	}
	//! Inserts a collection of items into specific position within the list.
	//!
	//! @param items    A collection of elements to copy.
	//! @param position Zero-based index of the position where the first element of
	//!                 inserted collection will located after insertion.
	//!                 If position is less then 0 then it will be equated to zero.
	//!                 If position is greater then length of the list, item will be
	//!                 added to the end of the list.
	void InsertRange(std::vector<ElementType> &items, int position)
	{
		if (items.size() <= 0)
		{
			return;
		}
		this->InsertRange(items.data(), items.size(), position);
	}
	//! Inserts a collection of items into specific position within the list.
	//!
	//! @param items    A collection of elements to copy.
	//! @param position Zero-based index of the position where the first element of
	//!                 inserted collection will located after insertion.
	//!                 If position is less then 0 then it will be equated to zero.
	//!                 If position is greater then length of the list, item will be
	//!                 added to the end of the list.
	void InsertRange(List<ElementType> &items, int position)
	{
		this->InsertRange(items.elements, items.length, position);
	}
	//! Removes an element at the specified position.
	//!
	//! @param index Zero-based index of the position of the element to remove.
	//!              If position is less then 0 then it will be equated to zero.
	//!              If position is greater then length of the list, item will be
	//!              removed from the end of the list.
	//!
	//! @return A removed element.
	ElementType RemoveAt(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		if (index > this->length)
		{
			return this->RemoveBack();
		}
		ElementType removedElement = this->elements[index];
		this->MoveRange(index + 1, index, this->length - index - 1);
		this->length--;
		return removedElement;
	}
	//! Removes an element at the specified position.
	//!
	//! @param index Zero-based index of the position of the element to remove.
	//!              If position is less then 0 then it will be equated to zero.
	//!              If position is greater then length of the list, item will be
	//!              removed from the end of the list.
	//! @param count Number of elements to remove.
	void RemoveRangeAt(int index, int count)
	{
		if (index < 0)
		{
			index = 0;
		}
		if (index > this->length)
		{
			return this->RemoveRangeBack(count);
		}
		this->MoveRange(index + count, index, this->length - index - count);
		this->length -= count;
	}
	//! Removes an element from the end of the list.
	//!
	//! Example:
	//!
	//! @code
	//! // Delete all elements from the collection while invoking destructor on each.
	//! while(collection->Length)
	//! {
	//!     delete collection->RemoveBack();
	//! }
	//! @endcode
	//!
	//! @return A removed element.
	ElementType RemoveBack()
	{
		return this->elements[--(this->length)];
	}
	//! Removes a range of elements from the end of the list.
	//!
	//! @param count Number of elements to remove.
	void RemoveRangeBack(int count)
	{
		this->length -= count;
	}
	//! Clears the collection.
	void Clear()
	{
		this->length = 0;
	}
	//! Clears the collection while allowing an action to be done to the each element.
	//!
	//! @param func Pointer to the function that takes an object of type ElementType
	//!             that is invoked for each element within this list.
	void Clear(Enumerator func)
	{
		for (int i = 0; i < this->length; i++)
		{
			func(this->elements[i]);
		}
		this->length = 0;
	}
	//! Clears the collection while allowing an action to be done to the each element.
	//!
	//! @param func Pointer to the function that takes an object of type ElementType
	//!             and integer number that is invoked for each element within this
	//!             list, within integer number being the index of the object.
	void Clear(EnumeratorIndex func)
	{
		for (int i = 0; i < this->length; i++)
		{
			func(this->elements[i], i);
		}
		this->length = 0;
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func Pointer to the function that takes an object of type ElementType
	//!             that is invoked for each element within this list.
	void ForEach(Enumerator func)
	{
		for (int i = 0; i < this->length; i++)
		{
			func(this->elements[i]);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              that is invoked for each element within this list.
	//! @param index Zero-based index of the first element with which to start iteration.
	void ForEach(Enumerator func, int index)
	{
		for (int i = index; i < this->length; i++)
		{
			func(this->elements[i]);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              that is invoked for each element within this list.
	//! @param index Zero-based index of the first element with which to start iteration.
	//! @param count Number of elements to iterate through.
	void ForEach(Enumerator func, int index, int count)
	{
		for (int i = 0, counter; counter < count; i++, counter++)
		{
			func(this->elements[i]);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func Pointer to the function that takes an object of type ElementType
	//!             and integer number that is invoked for each element within this
	//!             list, within integer number being the index of the object.
	void ThroughEach(EnumeratorIndex func)
	{
		for (int i = 0; i < this->length; i++)
		{
			func(this->elements[i], i);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              and integer number that is invoked for each element within this
	//!              list, within integer number being the index of the object.
	//! @param index Zero-based index of the first element with which to start iteration.
	void ThroughEach(EnumeratorIndex func, int index)
	{
		for (int i = index; i < this->length; i++)
		{
			func(this->elements[i], i);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              and integer number that is invoked for each element within this
	//!              list, within integer number being the index of the object.
	//! @param index Zero-based index of the first element with which to start iteration.
	//! @param count Number of elements to iterate through.
	void ThroughEach(EnumeratorIndex func, int index, int count)
	{
		for (int i = 0, counter; counter < count; i++, counter++)
		{
			func(this->elements[i], i);
		}
	}
	//! Goes through the list and returns first element that satisfies a condition.
	//!
	//! @param match Predicate function that represents a condition.
	//!
	//! @return The first element that matched the condition, if any, otherwise 0 casted
	//!         to the type of elements of the list is returned.
	ElementType Find(Predicate match)
	{
		for (int i = 0; i < this->length; i++)
		{
			if (match(this->elements[i]))
			{
				return this->elements[i];
			}
		}
		return (ElementType)0;
	}
	//! Gets or sets capacity of this list.
	__declspec(property(get=GetCapacity, put=SetCapacity)) int Capacity;
	int GetCapacity()
	{
		return this->capacity;
	}
	void SetCapacity(int value)
	{
		this->capacity = value;
		this->elements = realloc(this->elements, this->capacity * sizeof(ElementType));
	}
	//! Gets or sets number of elements within this list.
	__declspec(property(get=GetLength)) int Length;
	int GetLength()
	{
		return this->length;
	}
	//! Returns a reference to the element at specified index. No checks are performed.
	ElementType &operator[](int index)
	{
		return this->elements[index];
	}
	//! Returns a reference to the element at specified index. Index is clamped into the range.
	ElementType &At(int index)
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
	//! Releases memory held by this list.
	void Dispose()
	{
		if (this->elements)
		{
			//std::cout << "Trying to free elements:" << std::endl;
			free(this->elements);
			this->elements = nullptr;
			this->length = 0;
			this->capacity = 0;
		}
	}
private:
	void Expand(int size)
	{
		if (this->capacity < size)
		{
			// Set new capacity.
			this->capacity *= 2;
			//std::cout << "Expanding to the capacity of " << this->capacity << std::endl;
			// Reallocate the memory.
			this->elements =
				(ElementType *)realloc(this->elements, this->capacity * sizeof(ElementType));
		}
	}
	void MoveRange(int originalIndex, int destinationIndex, int count)
	{
		int offset = destinationIndex - originalIndex;
		if (offset > 0)
		{
			// Move right to left.
			for (int i = originalIndex + count - 1; i >= originalIndex; i--)
			{
				this->elements[i + offset] = this->elements[i];
			}
		}
		else if (offset < 0)
		{
			// Move left to right.
			for (int i = 0; i < count; i++)
			{
				this->elements[destinationIndex + i] = this->elements[originalIndex + i];
			}
		}
	}
};