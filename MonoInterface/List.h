#pragma once

#include <functional>
#include <initializer_list>
#include "ArrayHeader.h"
#include "ReadOnlyList.h"

#ifdef CRYCIL_MODULE
#include <ISystem.h>

#define FatalError(message) CryFatalError(message)

#else

#include <stdexcept>
#include <assert.h>
#define FatalError(message) throw std::logic_error(message)

#endif // CRYCIL_MODULE

template<typename ElementType, typename AllocatorType> class List;

//! Specializations of this template define the static field named "shift" are used by a list iterator to advance
//! to next object.
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
template<typename ElementType, typename AllocatorType, bool NormalIteration>
class ListIterator
{
	const List<ElementType, AllocatorType> *list;
	int currentPosition;
public:
	//! Creates an iterator for a given list that starts iteration from a specified position.
	//!
	//! @param list 
	ListIterator(const List<ElementType, AllocatorType> *list, int initialPosition)
	{
		this->list = list;
		this->currentPosition = initialPosition;
	}
	//! Determines whether this iterator is not on the same position within the same list as the other iterator.
	bool operator !=(const ListIterator &other) const
	{
		// I don't think that the second check is needed, but whatever, its mostly the same in terms of performance.
		return this->currentPosition != other.currentPosition && this->list != other.list;
	}
	//! Returns modifiable reference to the element this iterator is currently at.
	ElementType &operator *();
	//! Returns unmodifiable reference to the element this iterator is currently at.
	const ElementType &operator *() const;
	//! Moves this iterator to the next element in the list.
	const ListIterator &operator ++()
	{
		this->currentPosition += DirectionTraits<NormalIteration>::shift;
		return *this;
	}
	//! Moves this iterator to the previous element in the list.
	const ListIterator &operator --()
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

//! Defines static functions that can be used by objects of type List to manipulate memory for elements.
//!
//! @typeparam T     Type of objects that are contained within the list.
//! @typeparam align Optional value that specifies over-alignment of each element.
template<typename T, size_t align = alignof(T)>
struct DefaultListAllocator
{
	static_assert(align >= 1, "Cannot use negative values for alignment.");
	static_assert(align >= alignof(T), "Alignment value must be greater then or equal to type's minimal alignment requirement.");

	//! Allocates enough memory to fit "count" elements.
	static T *alloc(size_t count)
	{
		return static_cast<T *>(_aligned_malloc(count * sizeof(T), align));
	}
	//! Reallocates enough memory to fit "count" elements.
	static T *realloc(T *ptr, size_t count)
	{
		return static_cast<T *>(_aligned_realloc(ptr, count * sizeof(T), align));
	}
	//! Releases the memory.
	static void free(T *ptr)
	{
		_aligned_free(ptr);
	}
};

#pragma warning(push)
#pragma warning(disable : 4522)

//! Represents a mutable list of objects.
//!
//! @typeparam ElementType   Type of objects in this list.
//! @typeparam AllocatorType A type that defines static functions alloc and free that are invoked by objects
//!                          of this type to manipulate memory for elements. Look at @see DefaultListAllocator
//!                          to see how it should be implemented.
template<typename ElementType, typename AllocatorType = DefaultListAllocator<ElementType>>
class List
{
public:
	// Type defs for everything.
	typedef ElementType value_type;
	typedef ElementType *pointer;
	typedef const ElementType *const_pointer;
	typedef ElementType &reference;
	typedef const ElementType &const_reference;

	typedef size_t size_type;
	typedef ptrdiff_t difference_type;
	
	typedef std::function<void(reference)> Enumerator;
	typedef std::function<void(reference,  size_type)> EnumeratorIndex;
	typedef std::function<bool(reference)> Predicate;

	typedef DefaultListAllocator<ElementType> allocator_type;
private:
	struct ListObj
	{
		int refCount;
		size_type length;
		size_type capacity;
		pointer elements;

		ListObj()
			: refCount(1)
			, length(0)
			, capacity(0)
			, elements(nullptr)
		{
		}
	};
	ListObj *list;
public:
	//! Returns a reference to the element at specified index. No checks are performed.
	reference operator[](size_type index)
	{
		return this->list->elements[index];
	}
	//! Returns a reference to the element at specified index. No checks are performed.
	const_reference operator[](size_type index) const
	{
		return this->list->elements[index];
	}
	//! Returns a reference to the element at specified index. Index is clamped into the range.
	ElementType &At(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		if (index >= this->list->length)
		{
			index = this->list->length - 1;
		}
		return this->list->elements[index];
	}
	//! Returns a reference to the element at specified index. Index is clamped into the range.
	const ElementType &At(int index) const
	{
		if (index < 0)
		{
			index = 0;
		}
		if (index >= this->list->length)
		{
			index = this->list->length - 1;
		}
		return this->list->elements[index];
	}
	ElementType *Data() const
	{
		return this->list->elements;
	}

	//! Gets or sets capacity of this list.
	__declspec(property(get = GetCapacity, put = SetCapacity))size_type Capacity;
	size_type GetCapacity() const
	{
		return this->list->capacity;
	}
	void SetCapacity(size_type value)
	{
		assert(value >= 0);

		auto oldCapacity = this->list->capacity;

		if (this->list->length > value)
		{
			// Destroy all items that are now outside of the list.
			this->Length = value;
		}

		this->list->elements = allocator_type::realloc(this->list->elements, value);
		this->list->capacity = value;
	}

	//! Gets the number of live objects in this list.
	__declspec(property(get = GetLength, put = SetLength))size_type Length;
	size_type GetLength() const
	{
		return this->list->length;
	}
private:
	void SetLength(size_type value)
	{
		assert(value >= 0);

		for (size_type i = this->list->length - 1; i >= value; i--)
		{
			this->list->elements[i].~value_type();
		}
	}
public:
	//! Creates a default object of this type.
	List() : List(20) {}
	//! Creates a new list that can fit certain number of items.
	List(size_type initialCapacity)
	{
		this->list = new ListObj();
		this->Capacity = initialCapacity;
	}

	~List()
	{
		std::cout << "Invoking the list destructor." << std::endl;

		this->Release();

		std::cout << "Invoked the list destructor." << std::endl;
	}
private:
	void Release()
	{
		std::cout << "Releasing the list. Reference count is " << this->list->refCount << std::endl;
		if (--this->list->refCount == 0)
		{
			std::cout << "Destroying the elements. Reference count is " << this->list->refCount << std::endl;

			// Destroy the elements.
			for (size_type i = 0; i < this->list->length; i++)
			{
				this->list->elements[i].~value_type();
			}

			// Deallocate the memory.
			allocator_type::free(this->list->elements);

			// Delete the object.
			delete this->list;
		}

		this->list = nullptr;
	}
public:
	//! Assigns a deep copy of another list to this one.
	List &operator =(const List &other)
	{
		this->Release();

		this->list = new ListObj();

		this->Capacity = other.Capacity;
		
		for (size_type i = 0; i < other.list->length; i++)
		{
			this->list->elements[i] = other.list->elements[i];
		}
		this->list->length = other.list->length;

		return *this;
	}
	//! Assigns a shallow copy of another list to this one.
	List &operator =(List &other)
	{
		this->Release();

		this->list = other.list;
		this->list->refCount++;

		return *this;
	}
	//! Assigns contents of the initializer list to this one.
	List &operator =(std::initializer_list<value_type> objects)
	{
		// Destroy all objects.
		this->Length = 0;

		// Copy the objects.
		for (auto current = objects.begin(); current != objects.end(); current++)
		{
			this->list->elements[this->list->length++] = *current;
		}

		return *this;
	}

	//! Inserts a new element at the end of this list.
	List &operator <<(reference obj)
	{
		return this->Add(obj);
	}
	//! Inserts a set of elements at the end of this list.
	List &operator <<(std::initializer_list<value_type> items)
	{
		return this->Add(items.begin(), items.size());
	}
	//! Inserts a set of elements at the end of this list.
	List &operator <<(const List &items)
	{
		return this->Add(items.list->elements, items.list->length);
	}

	//! Adds a default object of type ElementType to the end of this list.
	List &Add()
	{
		this->EnsureCapacity(this->Length + 1);

		new (&this->list->elements[this->list->length++]) value_type();

		return *this;
	}
	//! Inserts a default object of type ElementType into this list.
	//!
	//! @param index Zero-based index of the element to insert the object into. This value is clamped into
	//!              [0; currentListLength] range before actual operation.
	List &Insert(size_type index)
	{
		if (index >= this->list->length)
		{
			return this->Add();
		}

		this->EnsureCapacity(this->Length + 1);

		this->MoveRange(index, index + 1, this->list->length - index);

		new (&this->list->elements[index]) value_type();
		this->list->length++;

		return *this;
	}
	//! Adds a new object of type ElementType to the end of this list.
	List &Add(reference item)
	{
		this->EnsureCapacity(this->Length + 1);

		this->list->elements[this->list->length++] = item;

		return *this;
	}
	//! Inserts an object of type ElementType into this list.
	//!
	//! @param index Zero-based index of the element to insert the object into. This value is clamped into
	//!              [0; currentListLength] range before actual operation.
	List &Insert(size_type index, reference item)
	{
		if (index >= this->list->length)
		{
			return this->Add();
		}

		this->EnsureCapacity(this->Length + 1);

		this->MoveRange(index, index + 1, this->list->length - index);

		this->list->elements[index] = item;
		this->list->length++;

		return *this;
	}
	//! Adds a set of items to the end of this list.
	List &Add(std::initializer_list<value_type> items)
	{
		return this->Add(items.begin(), items.size());
	}
	//! Inserts a set of objects of type ElementType into this list.
	//!
	//! @param index Zero-based index of the element to insert the first object into. This value is clamped into
	//!              [0; currentListLength] range before actual operation.
	List &Insert(size_type index, std::initializer_list<value_type> items)
	{
		return this->Insert(index, items.begin(), items.size());
	}
	//! Adds a set of items to the end of this list.
	List &Add(const List &items)
	{
		return this->Add(items.list->elements, items.list->length);
	}
	//! Inserts a set of objects of type ElementType into this list.
	//!
	//! @param index Zero-based index of the element to insert the first object into. This value is clamped into
	//!              [0; currentListLength] range before actual operation.
	List &Insert(size_type index, const List &items)
	{
		return this->Insert(index, items.list->elements, items.list->length);
	}
	//! Adds a set of items to the end of this list.
	List &Add(const_pointer items, size_type count)
	{
		if (count == 0)
		{
			return;
		}

		this->EnsureCapacity(this->list->elements + count);

		// Copy the objects.
		for (size_type i = 0; i < count; i++)
		{
			this->list->elements[this->list->length++] = items[i];
		}

		return *this;
	}
	//! Inserts a set of objects of type ElementType into this list.
	//!
	//! @param index Zero-based index of the element to insert the first object into. This value is clamped into
	//!              [0; currentListLength] range before actual operation.
	List &Insert(size_type index, const_pointer items, size_type count)
	{
		if (count == 0)
		{
			return;
		}

		if (index >= this->list->length)
		{
			this->Add(items);
		}

		this->EnsureCapacity(this->list->elements + count);

		this->MoveRange(index, index + count, this->list->length - index + count);

		// Copy the objects.
		for (size_type i = 0; i < count; i++)
		{
			this->list->elements[index++] = items[i];
		}
		this->list->length += count;

		return *this;
	}
	//! Linearly searches for a first item for which given condition is true and replaces it with given one, or just adds
	//! the item to the end of the list, if there are no items in the list for which the condition is true.
	//!
	//! @param item  An item to put into the list/override existing item with.
	//! @param match An object that represents a condition that must be met for the item to be replaced with given one.
	//!
	//! @returns True, if the item was found and replaced, otherwise false.
	bool AddOverride(reference item, Predicate predicate)
	{
		difference_type index = this->IndexOf(predicate);
		if (index == -1)
		{
			this->Add(item);
			return false;
		}
		ElementType overridenItem = this->list->elements[index];
		this->list->elements[index] = item;

		// We are not going to do anything about the overridden item: we just gonna let its destructor invoked.
		return true;
	}
	//! Removes an element at the specified position.
	//!
	//! @param index Zero-based index of the position of the element to remove.
	//!              If position is less then 0 then it will be equated to zero.
	//!              If position is greater then length of the list, item will be
	//!              removed from the end of the list.
	//!
	//! @return A removed element.
	ElementType RemoveAt(size_type index)
	{
		if (index > this->list->length)
		{
			return this->RemoveBack();
		}
		ElementType removedElement = this->list->elements[index];
		this->MoveRange(index + 1, index, this->list->length - index - 1);
		this->list->length--;
		return removedElement;
	}
	//! Removes an element at the specified position.
	//!
	//! @param index Zero-based index of the position of the element to remove.
	//!              If position is less then 0 then it will be equated to zero.
	//!              If position is greater then length of the list, item will be
	//!              removed from the end of the list.
	//! @param count Number of elements to remove.
	void RemoveAt(size_type index, size_type count)
	{
		if (index > this->list->length)
		{
			return this->RemoveBack(count);
		}

		this->MoveRange(index + count, index, this->list->length - index - count);
		this->Length -= count;
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
		if (this->list->length == 0)
		{
			return;
		}
		return this->list->elements[--(this->list->length)];
	}
	//! Removes a range of elements from the end of the list.
	//!
	//! @param count Number of elements to remove.
	void RemoveBack(size_type count)
	{
		this->Length -= count;
	}
	//! Clears the collection.
	void Clear()
	{
		this->Length = 0;
	}
	//! Clears the collection while allowing an action to be done to the each element.
	//!
	//! @param func Pointer to the function that takes an object of type ElementType
	//!             that is invoked for each element within this list.
	void Clear(Enumerator func)
	{
		for (int i = 0; i < this->list->length; i++)
		{
			func(this->list->elements[i]);
		}
		this->Length = 0;
	}
	//! Applies delete operator to each element in this list.
	void DeleteAll()
	{
		for (int i = 0; i < this->list->length; i++)
		{
			delete this->list->elements[i];
		}
	}
	//! Clears the collection while allowing an action to be done to the each element.
	//!
	//! @param func Pointer to the function that takes an object of type ElementType
	//!             and integer number that is invoked for each element within this
	//!             list, within integer number being the index of the object.
	void Clear(EnumeratorIndex func)
	{
		for (int i = 0; i < this->list->length; i++)
		{
			func(this->list->elements[i], i);
		}
		this->Length = 0;
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
	int BinarySearch(ElementType &element, std::function<int(ElementType&, ElementType&)> comparer = nullptr) const
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

		difference_type lo = 0;
		difference_type hi = this->Length - 1;
		while (lo <= hi)
		{
			int i = lo + ((hi - lo) >> 1);
			int order = comparison(this->list->elements[i], element);

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
		for (int i = 0; i < this->list->length; i++)
		{
			func(this->list->elements[i]);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              that is invoked for each element within this list.
	//! @param index Zero-based index of the first element with which to start iteration.
	void ForEach(Enumerator func, size_type index)
	{
		for (int i = index; i < this->list->length; i++)
		{
			func(this->list->elements[i]);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              that is invoked for each element within this list.
	//! @param index Zero-based index of the first element with which to start iteration.
	//! @param count Number of elements to iterate through.
	void ForEach(Enumerator func, size_type index, size_type count)
	{
		int counter = 0;
		for (int i = index; counter < count; i++, counter++)
		{
			func(this->list->elements[i]);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func Pointer to the function that takes an object of type ElementType
	//!             and integer number that is invoked for each element within this
	//!             list, within integer number being the index of the object.
	void ThroughEach(EnumeratorIndex func)
	{
		for (int i = 0; i < this->list->length; i++)
		{
			func(this->list->elements[i], i);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              and integer number that is invoked for each element within this
	//!              list, within integer number being the index of the object.
	//! @param index Zero-based index of the first element with which to start iteration.
	void ThroughEach(EnumeratorIndex func, size_type index)
	{
		for (int i = index; i < this->list->length; i++)
		{
			func(this->list->elements[i], i);
		}
	}
	//! Performs an action on each element within the list.
	//!
	//! @param func  Pointer to the function that takes an object of type ElementType
	//!              and integer number that is invoked for each element within this
	//!              list, within integer number being the index of the object.
	//! @param index Zero-based index of the first element with which to start iteration.
	//! @param count Number of elements to iterate through.
	void ThroughEach(EnumeratorIndex func, size_type index, size_type count)
	{
		int counter = 0;
		for (int i = index; counter < count && i < this->list->length; i++, counter++)
		{
			func(this->list->elements[i], i);
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
		for (int i = 0; i < this->list->length; i++)
		{
			if (match(this->list->elements[i]))
			{
				return &this->list->elements[i];
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
		for (int i = 0; i < this->list->length; i++)
		{
			if (match(this->list->elements[i]))
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
		for (int i = this->list->length - 1; i >= 0; i--)
		{
			if (match(this->list->elements[i]))
			{
				return i;
			}
		}
		return -1;
	}
	//! Creates a deep copy of this list.
	//!
	//! @returns A pointer to array that will have to be deleted manually.
	pointer Duplicate(size_type &itemCount)
	{
		if (this->list->length == 0)
		{
			return nullptr;
		}

		pointer items = new value_type[this->list->length];
		itemCount = this->list->length;

		for (size_type i = 0; i < itemCount; i++)
		{
			items[i] = this->list->elements[i];
		}

		return items;
	}
	//! Sets capacity of this list to number of valid elements it currently holds.
	void Trim()
	{
		this->Capacity = this->list->length;
	}
	//! Creates an object that allows this list to be looked through without changing it.
	__declspec(property(get = GetAsReadOnly)) ReadOnlyList<ElementType> AsReadOnly;
	ReadOnlyList<ElementType> GetAsReadOnly() const
	{
		return ReadOnlyList<ElementType>(this->list->elements, this->list->length, this->list->capacity);
	}
private:
	void EnsureCapacity(size_type newSize)
	{
		if (this->list->capacity >= newSize)
		{
			return;
		}

		// Determine new capacity.
		size_type newCapacity = this->list->capacity != 0 ? this->list->capacity : 20;
		while (this->list->capacity < newSize)
		{
			newCapacity *= 2;
		}

		// Reallocate the memory.
		this->list->elements = allocator_type::realloc(this->list->elements, newCapacity);
		this->list->capacity = newCapacity;
	}
	void MoveRange(int originalIndex, int destinationIndex, int count)
	{
		int offset = destinationIndex - originalIndex;
		if (offset > 0)
		{
			// Move right to left.
			for (int i = originalIndex + count - 1; i >= originalIndex; i--)
			{
				this->list->elements[i + offset] = this->list->elements[i];
			}
		}
		else if (offset < 0)
		{
			// Move left to right.
			for (int i = 0; i < count; i++)
			{
				this->list->elements[destinationIndex + i] = this->list->elements[originalIndex + i];
			}
		}
	}
	// For STL-style iteration:
public:
	//! Creates an object that can iterate this list from the start.
	ListIterator<ElementType, AllocatorType, true> begin() const
	{
		return ListIterator<ElementType, AllocatorType, true>(this, 0);
	}
	//! Creates an object that can iterate this list from a specified position.
	//!
	//! @param start Zero-based index of the first element to start iteration from.
	ListIterator<ElementType, AllocatorType, true> begin_from(int start) const
	{
		return ListIterator<ElementType, AllocatorType, true>(this, start);
	}
	//! Creates an object that represents an iterator that reached the end of this list.
	ListIterator<ElementType, AllocatorType, true> end() const
	{
		return ListIterator<ElementType, AllocatorType, true>(this, this->list->length);
	}
	//! Used by this class when iterating through the list in reverse.
	class ReversedList
	{
		const List<ElementType, allocator_type> *list;
	public:
		ReversedList(const List<ElementType> *list)
		{
			this->list = list;
		}
		//! Creates an object that can iterate this list from the last element.
		ListIterator<ElementType, AllocatorType, false> begin() const
		{
			return ListIterator<ElementType, AllocatorType, false>(this->list, this->list->length - 1);
		}
		//! Creates an object that can iterate this list from a specified position.
		//!
		//! @param start Zero-based index of the first element to start iteration from.
		ListIterator<ElementType, AllocatorType, false> begin_from(int start) const
		{
			return ListIterator<ElementType, AllocatorType, false>(this, this->list->length - 1 - start);
		}
		//! Creates an object that represents an iterator that reached the end of this list.
		ListIterator<ElementType, AllocatorType, false> end() const
		{
			return ListIterator<ElementType, AllocatorType, false>(this, -1);
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

#pragma warning(pop)

template<typename ElementType, typename AllocatorType, bool NormalIteration>
inline const ElementType &ListIterator<ElementType, AllocatorType, NormalIteration>::operator*() const
{
	return (*this->list)[this->currentPosition];
}

template<typename ElementType, typename AllocatorType, bool NormalIteration>
inline ElementType &ListIterator<ElementType, AllocatorType, NormalIteration>::operator*()
{
	return (*this->list)[this->currentPosition];
}

template<typename ElementType, typename AllocatorType, bool NormalIteration>
inline void ListIterator<ElementType, AllocatorType, NormalIteration>::RemoveHere()
{
	this->list->RemoveAt(this->currentPosition);
	if (NormalIteration)
	{
		// We only need to change our current position when iterating in normal order.
		this->currentPosition--;
	}
}
