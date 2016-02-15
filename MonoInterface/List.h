#pragma once

#include <functional>

#ifdef CRYCIL_MODULE
#include <ISystem.h>

#define FatalError(message) CryFatalError(message)

#else

#include <stdexcept>
#define FatalError(message) throw std::logic_error(message)

#endif // CRYCIL_MODULE

#include <initializer_list>
#include "ReadOnlyList.h"

template<typename ElementType> class List;

//! Specializations of this template define the static field named "shift" are used by a list iterator to advance to next
//! object.
template<bool normal>
struct DirectionTraits
{
};
//! Used by iterators that advance in normal direction.
template<>
struct DirectionTraits<true>
{
	static const int shift = 1;
};
//! Used by iterators that advance in reversed direction.
template<>
struct DirectionTraits<false>
{
	static const int shift = -1;
};

//! Represents an object that iterates through a List.
//!
//! @tparam ElementType     Type that represents items the list contains.
//! @tparam NormalIteration Indicates whether the iterator will not reverse the order of iteration.
template<typename ElementType, bool NormalIteration>
class ListIterator
{
	const List<ElementType> *list;
	int currentPosition;
public:
	//! Creates an iterator for a given list that starts iteration from a specified position.
	//!
	//! @param list 
	ListIterator(const List<ElementType> *list, int initialPosition)
	{
		this->list = list;
		this->currentPosition = initialPosition;
	}
	//! Determines whether this iterator is not on the same position within the same list as the other iterator.
	bool operator !=(const ListIterator<ElementType, NormalIteration> &other) const
	{
		// I don't think that the second check is needed, but whatever, its mostly the same in terms of performance.
		return this->currentPosition != other.currentPosition && this->list != other.list;
	}
	//! Returns modifiable reference to the element this iterator is currently at.
	ElementType &operator *();
	//! Returns unmodifiable reference to the element this iterator is currently at.
	const ElementType &operator *() const;
	//! Moves this iterator to the next element in the list.
	const ListIterator<ElementType, NormalIteration> &operator ++()
	{
		this->currentPosition += DirectionTraits<NormalIteration>::shift;
		return *this;
	}
	//! Moves this iterator to the previous element in the list.
	const ListIterator<ElementType, NormalIteration> &operator --()
	{
		this->currentPosition -= DirectionTraits<NormalIteration>::shift;
		return *this;
	}
	//! Removes the element this iterator is currently at from the list.
	void RemoveHere();
};

//! Default comparison function that uses KeyType's comparison operators.
template<typename KeyType>
inline int DefaultComparison(KeyType &k1, KeyType &k2)
{
	if (k1 > k2)
	{
		return 1;
	}
	if (k2 > k1)
	{
		return -1;
	}
	return 0;
}

//! Represents an expandable list of items.
//!
//! @tparam ElementType        Type that represents items this list contains.
template<typename ElementType>
class List
{
public:
	typedef std::function<void(ElementType&)> Enumerator;
	typedef std::function<void(ElementType&, int)> EnumeratorIndex;
	typedef std::function<bool(ElementType&)> Predicate;
private:
	ElementType *elements;
	int length;
	int capacity;
public:
	//! Assigns contents of the temporary object to the new one.
	List(List<ElementType> &&other)
		: elements(other.elements)
		, length(other.length)
		, capacity(other.length)
	{
		other.elements = nullptr;
	}
	//! Creates a default list.
	//!
	//! Default capacity is 10.
	List()
	{
		this->length = 0;
		this->capacity = 10;
		this->elements = new ElementType[this->capacity];
	}
	//! Creates a new list.
	//!
	//! @param initialCapacity Number of items this list is planned to hold.
	explicit List(int initialCapacity)
	{
		this->length = 0;
		this->capacity = initialCapacity;
		this->elements = new ElementType[this->capacity];
	}
	//! Creates a new list.
	//!
	//! @param collection Vector that contains elements to pre-populate this list with.
	explicit List(std::vector<ElementType> &collection)
	{
		this->length = 0;
		int collectionSize = collection.size();
		if (collectionSize <= 0)
		{
			this->capacity = 10;
		}
		this->elements = new ElementType[this->capacity];
		if (collectionSize > 0)
		{
			this->AddRange(collection);
		}
	}
	//! Constructs a deep copy of the list.
	List(const List<ElementType> &list)
	{
		this->capacity = list.capacity;
		this->length = list.length;
		this->elements = new ElementType[this->capacity];
		for (int i = 0; i < this->length; i++)
		{
			this->elements[i] = list.elements[i];
		}
	}
	//! Creates new list from data that was detached from another one.
	//!
	//! List should be trimmed before detachment.
	List(ElementType *elements, int capacity)
	{
		this->capacity = capacity;
		this->length   = capacity;
		this->elements = elements;
	}
	//! Creates a new list with specified elements.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//!
	//! List<const char *> lines = { "First line", "Second line", "Third line", "Fourth line" };
	//!
	//! @endcode
	//!
	//! @param initialElements A sequence of elements that were specified inside the braced initialization
	//!                        list.
	List(std::initializer_list<ElementType> initialElements, int capacity = -1)
		: length(0)
	{
		if (initialElements.size() == 0)
		{
			this->length = 0;
			this->capacity = capacity < 0 ? 10 : capacity;
			this->elements = new ElementType[this->capacity];
		}
		else
		{
			int length = initialElements.size();
			this->capacity = capacity < length ? length : capacity;
			this->elements = new ElementType[length];

			for (auto current = initialElements.begin(); current < initialElements.end(); current++)
			{
				this->elements[this->length++] = *current;
			}
		}
	}
	~List()
	{
		if (this->elements)
		{
			//std::cout << "Trying to free elements:" << std::endl;
			delete[] this->elements;
			this->elements = nullptr;
			this->length = 0;
			this->capacity = 0;
		}
	}
	//! Swaps internal data between too lists.
	SWAP_ASSIGNMENT List<ElementType> &operator=(List<ElementType> &other)
	{
		if (this->elements != other.elements)
		{
			std::swap(this->elements, other.elements);
			std::swap(this->length,   other.length);
			std::swap(this->capacity, other.capacity);
		}
		return *this;
	}
	//! Places an item at the end of this list.
	//!
	//! @param item Item to add.
	//!
	//! @returns A reference to this object to allow chaining of Add() calls.
	List<ElementType> &Add(ElementType item)
	{
		this->Expand(this->length + 1);
		this->elements[this->length++] = item;
		return *this;
	}
	//! Adds a sequence of items to the list.
	//!
	//! @returns A reference to this object to allow chaining of Add() calls.
	List<ElementType> &Add(std::initializer_list<ElementType> elements)
	{
		if (elements.size() == 0)
		{
			return *this;
		}

		this->Expand(this->length + elements.size());
		for (auto i = elements.begin(); i < elements.end(); i++)
		{
			this->elements[this->length++] = *i;
		}
		return *this;
	}
	//! Adds a range of items to the end of the list.
	//!
	//! @param collection A collection of elements to add.
	void AddRange(std::vector<ElementType> &collection)
	{
		int collectionSize = collection.size();
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
		if (itemCount <= 0)
		{
			return;
		}

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
		if (!items || items->Length <= 0)
		{
			return;
		}

		this->Expand(this->length + items->length);
		for (int i = 0; i < items->length; i++)
		{
			this->elements[this->length++] = items->At(i);
		}
	}
	//! Linearly searches for a first item for which given condition is true and replaces it with given one, or just adds
	//! the item to the end of the list, if there are no items in the list for which the condition is true.
	//!
	//! @param item  An item to put into the list/override existing item with.
	//! @param match An object that represents a condition that must be met for the item to be replaced with given one.
	//!
	//! @returns True, if the item was found and replaced, otherwise false.
	bool AddOverride(ElementType item, Predicate match)
	{
		int index = this->IndexOf(match);
		if (index == -1)
		{
			this->Add(item);
			return false;
		}
		else
		{
			ElementType overridenItem = this->elements[index];
			this->elements[index] = item;

			// We are not going to do anything about the overridden item: we just gonna let its destructor invoked.
			return true;
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
	//! Applies delete operator to each element in this list.
	void DeleteAll()
	{
		for (int i = 0; i < this->length; i++)
		{
			delete this->elements[i];
		}
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
	//! Performs a binary search for an element.
	//!
	//! Don't use this method on lists that are not sorted.
	//!
	//! In order to ensure that the list is sorted, you must only insert new elements at indexes that
	//! are represented by negated (bit-wise with a ~ (tilde) operator) indexes that are returned by this
	//! method.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//!
	//! // Lets make a sorted list:
	//! List<int> integers(5);
	//!
	//! integers.Insert(5, ~BinarySearch(5));
	//! integers.Insert(1, ~BinarySearch(1));
	//! integers.Insert(3, ~BinarySearch(3));
	//! integers.Insert(4, ~BinarySearch(4));
	//! integers.Insert(2, ~BinarySearch(2));
	//!
	//! for (int i = 0; i < integers.Length; i++)
	//! {
	//!     // Should print out 1, 2, 3, 4, 5 (with new lines in place of commas.)
	//!     CryLogAlways("%d", integers[i]);
	//! }
	//!
	//! @endcode
	//!
	//! @param element  An element we want to find.
	//! @param comparer An optional object that performs comparison of objects within this list.
	//!
	//! @returns A zero-based index of the element, if it was found in the list.
	int BinarySearch(ElementType &element, std::function<int(ElementType&, ElementType&)> comparer = nullptr)
	{
		std::function<int(ElementType&, ElementType&)> comparison;
		if (comparer)
		{
			comparison = comparer;
		}
		else
		{
			comparison = DefaultComparison<ElementType>;
		}
		
		int lo = 0;
		int hi = this->Length - 1;
		while (lo <= hi)
		{
			int i = lo + ((hi - lo) >> 1);
			int order = comparison(this->elements[i], element);

			if (order == 0) return i;
			if (order < 0)
			{
				lo = i + 1;
			}
			else
			{
				hi = i - 1;
			}
		}

		return ~lo;
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
		for (int i = index, counter; counter < count; i++, counter++)
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
		for (int i = index, counter; counter < count; i++, counter++)
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
	ElementType *Find(Predicate match)
	{
		for (int i = 0; i < this->length; i++)
		{
			if (match(this->elements[i]))
			{
				return &this->elements[i];
			}
		}
		return nullptr;
	}
	//! Goes through the list and returns index of the first element that satisfies a condition.
	//!
	//! @param match Predicate function that represents a condition.
	//!
	//! @return Zero-based index of the first element that matched the condition, if any, otherwise -1.
	int IndexOf(Predicate match)
	{
		for (int i = 0; i < this->length; i++)
		{
			if (match(this->elements[i]))
			{
				return i;
			}
		}
		return -1;
	}
	//! Goes through the list and returns index of the last element that satisfies a condition.
	//!
	//! @param match Predicate function that represents a condition.
	//!
	//! @return Zero-based index of the last element that matched the condition, if any, otherwise -1.
	int LastIndexOf(Predicate match)
	{
		for (int i = this->length - 1; i >= 0; i--)
		{
			if (match(this->elements[i]))
			{
				return i;
			}
		}
		return -1;
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
		// Allocate a new region in memory.
		ElementType *newElements = new ElementType[this->capacity];
		// Copy objects from old location to the new one.
		for (int i = 0; i < this->length; i++)
		{
			newElements[i] = this->elements[i];
		}
		// Deallocate old array.
		delete[] this->elements;
		// Assign new array to this list.
		this->elements = newElements;
	}
	//! Gets or sets number of elements within this list.
	__declspec(property(get=GetLength)) int Length;
	int GetLength() const
	{
		return this->length;
	}
	//! Returns a reference to the element at specified index. No checks are performed.
	ElementType &operator[](int index)
	{
		return this->elements[index];
	}
	//! Returns a reference to the element at specified index. No checks are performed.
	const ElementType &operator[](int index) const
	{
		return this->elements[index];
	}
	ElementType *Data() const
	{
		return this->elements;
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
	List<ElementType> &Set(int index, ElementType& el)
	{
		if (index < 0)
		{
			index = 0;
		}
		if (index >= this->length)
		{
			index = this->length - 1;
		}
		this->elements[index] = el;
		return *this;
	}
	//! Releases memory held by this list.
	void Dispose()
	{
		this->~List();
	}
	//! Detaches underlying array of elements allowing this list object to be released without
	//! releasing memory.
	//!
	//! It is highly recommended to trim this list before detaching its data.
	//!
	//! @param capacity Number of elements that returned array can hold.
	//!
	//! @returns A pointer to the array of elements that used to be bound to this list.
	ElementType *Detach(int &capacity)
	{
		capacity = this->capacity;
		ElementType *elems = this->elements;

		this->elements = nullptr;
		this->length = 0;
		this->capacity = 0;
		
		return elems;
	}
	//! Sets capacity of this list to number of valid elements it currently holds.
	void Trim()
	{
		this->Capacity = this->length;
	}
	//! Creates an object that allows this list to be looked through without changing it.
	__declspec(property(get = GetAsReadOnly)) ReadOnlyList<ElementType> AsReadOnly;
	ReadOnlyList<ElementType> GetAsReadOnly()
	{
		return ReadOnlyList<ElementType>(this->elements, this->length, this->capacity);
	}
private:
	void Expand(int size)
	{
		if (this->capacity < size)
		{
			// Set new capacity.
			this->Capacity = this->capacity * 2;
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
	// For STL-style iteration:
public:
	//! Creates an object that can iterate this list from the start.
	ListIterator<ElementType, true> begin() const
	{
		return ListIterator<ElementType, true>(this, 0);
	}
	//! Creates an object that can iterate this list from a specified position.
	//!
	//! @param start Zero-based index of the first element to start iteration from.
	ListIterator<ElementType, true> begin_from(int start) const
	{
		return ListIterator<ElementType, true>(this, start);
	}
	//! Creates an object that represents an iterator that reached the end of this list.
	ListIterator<ElementType, true> end() const
	{
		return ListIterator<ElementType, true>(this, this->length);
	}
	//! Used by this class when iterating through the list in reverse.
	class ReversedList
	{
		const List<ElementType> *list;
	public:
		ReversedList(const List<ElementType> *list)
		{
			this->list = list;
		}
		//! Creates an object that can iterate this list from the last element.
		ListIterator<ElementType, false> begin() const
		{
			return ListIterator<ElementType, false>(this->list, this->list->length - 1);
		}
		//! Creates an object that can iterate this list from a specified position.
		//!
		//! @param start Zero-based index of the first element to start iteration from.
		ListIterator<ElementType, false> begin_from(int start) const
		{
			return ListIterator<ElementType, false>(this, this->list->length - 1 - start);
		}
		//! Creates an object that represents an iterator that reached the end of this list.
		ListIterator<ElementType, false> end() const
		{
			return ListIterator<ElementType, false>(this, -1);
		}
	};
	//! Gets the object that allows this list to be iterated in reversed order.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//! List<int> numbers(4);
	//! // Fill the list with indexes.
	//! for (int i = 0; i < numbers.Length; i++)
	//! {
	//!     numbers[i] = i;
	//! }
	//! // Print the numbers in reverse order.
	//! for (const int &number : numbers.Reversed)
	//! {
	//!     std::cout << number;
	//! }
	//! @endcode
	//! 
	//! The code above will print: "3210".
	__declspec(property(get = GetReversed)) ReversedList Reversed;
	ReversedList GetReversed() const
	{
		return ReversedList(this);
	}
};

template<typename ElementType, bool NormalIteration>
inline const ElementType &ListIterator<ElementType, NormalIteration>::operator*() const
{
	return (*this->list)[this->currentPosition];
}

template<typename ElementType, bool NormalIteration>
inline ElementType &ListIterator<ElementType, NormalIteration>::operator*()
{
	return (*this->list)[this->currentPosition];
}

template<typename ElementType, bool NormalIteration>
inline void ListIterator<ElementType, NormalIteration>::RemoveHere()
{
	this->list->RemoveAt(this->currentPosition);
	if (NormalIteration)
	{
		// We only need to change our current position when iterating in normal order.
		this->currentPosition--;
	}
}