#pragma once

#include <functional>
#include "ExtraTypeTraits.h"
#include "Tuples.h"
#include "Allocation.hpp"
#include "Iteration.hpp"
#include "ListObject.hpp"

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

//! Represents a mutable reference-counted dynamic array of elements.
template<typename ElementType, typename AllocatorType = DefaultAllocator<ElementType>>
class List
{
	friend ListIteratorBase<List>;
public:
	typedef ElementType value_type;
	typedef ElementType &reference;
	typedef const ElementType &const_reference;
	typedef ElementType *pointer;
	typedef const ElementType *const_pointer;

	typedef size_t size_type;
	typedef ptrdiff_t difference_type;

	typedef AllocatorType allocator_type;
	typedef ListObject<value_type, allocator_type> list_object_type;
	typedef ListIteratorBase<list_object_type> iterator_base;
	typedef ListIterator<list_object_type> iterator_read_write;
	typedef ListIteratorConst<list_object_type> iterator_read_only;

	typedef std::function<int(const_reference, const_reference)> Comparison;

private:
	//! A compressed pair of 2 objects: an allocator object that is used to work with memory and a pointer to
	//! the list header object.
	CompressedPair<allocator_type, list_object_type *> list;
	
	//! Gets the list object.
	list_object_type *&Object()
	{
		return this->list.Second();
	}
	list_object_type * const &Object() const
	{
		return this->list.Second();
	}
	//! Gets the allocator object.
	allocator_type &Allocator()
	{
		return this->list.First();
	}
	const allocator_type &Allocator() const
	{
		return this->list.First();
	}

public:
	//! Gets the pointer to the first element in the list.
	pointer &First()
	{
		return this->list.Second()->First();
	}
	const_pointer &First() const
	{
		return this->list.Second()->First();
	}
	//! Gets the pointer to the element after last live object in the list.
	pointer &Last()
	{
		return this->list.Second()->Last();
	}
	const_pointer &Last() const
	{
		return this->list.Second()->Last();
	}
	//! Gets the pointer to the element after last non-live object in the list.
	pointer &End()
	{
		return this->list.Second()->End();
	}
	const_pointer &End() const
	{
		return this->list.Second()->End();
	}

	//! Gets the number of live objects that can fit into this list before it has to expand.
	__declspec(property(get = GetCapacity)) size_type Capacity;
	size_type GetCapacity() const
	{
		return this->End() - this->First();
	}
	//! Gets the number of live objects that can be added to this list before it has to expand.
	__declspec(property(get = GetUnusedCapacity)) size_type UnusedCapacity;
	size_type GetUnusedCapacity() const
	{
		return this->End() - this->Last();
	}
	//! Gets the number of live objects that are currently currently contained within this list.
	__declspec(property(get = GetLength)) size_type Length;
	size_type GetLength() const
	{
		return this->Last() - this->First();
	}
	//! Indicates whether this list is empty.
	__declspec(property(get = IsEmpty)) bool Empty;
	bool IsEmpty() const
	{
		return this->Last() - this->First() == 0;
	}

	//! Provides read/write access to the element in the list.
	reference operator [](size_type index)
	{
#ifdef DEBUG_ITERATION
		if (index >= this->Length)
		{
			throw std::out_of_range("Attempted to access the element of the list that is out of range.");
		}
#endif // DEBUG_ITERATION

		return this->First() + index;
	}
	//! Provides read-only access to the element in the list.
	const_reference operator[](size_type index) const
	{
		return (*this)[index];
	}
public:
	//! Creates a new empty list.
	//!
	//! @param allocator An optional value that is an object to use to work with memory.
	List(const allocator_type &allocator = allocator_type())
		: list(OneToFirstRestToSecond(), allocator)
	{
		this->CreateObject();
	}
	//! Creates a new empty list.
	//!
	//! @param allocator An optional value that is a temporary object to use to work with memory.
	explicit List(allocator_type &&allocator)
		: list(OneToFirstRestToSecond(), std::move(allocator))
	{
		this->CreateObject();
	}
	//! Creates an empty that has enough memory to fit specified number of objects.
	explicit List(size_type initialCapacity, const allocator_type &allocator = allocator_type())
		: list(OneToFirstRestToSecond(), allocator)
	{
		this->CreateObject();
		
		this->Object()->AllocateStorage(initialCapacity);
	}
	//! Creates a list that is filled with a number of copies of the object.
	List(size_type initialSize, const_reference filler, const allocator_type &allocator = allocator_type())
		: list(OneToFirstRestToSecond(), allocator)
	{
		this->CreateObject();

		this->Object()->AllocateStorage(initialSize);

		// Fill the memory with copies of the filler.
		this->Allocator().InitializeRange(this->First(), this->End(), filler);

		this->Last() = this->End();
	}
	//! Creates a new list that contains a deep copy of the range of elements that is delimited by 2 iterators.
	//!
	//! @tparam IteratorType Type of the iterators.
	//!
	//! @param left  An iterator that defines the start of the range.
	//! @param right An iterator that defines the end of the range (points at the element after last element in
	//!              the range).
	template<typename IteratorType, typename = typename EnableIf<IsIterator<IteratorType>::value, void>::type>
	List(IteratorType left, IteratorType right, const allocator_type &allocator = allocator_type())
		: list(OneToFirstRestToSecond(), allocator)
	{
		this->CreateObject();

		this->Build(left, right);
	}
	//! Creates a new list that is populated with copies of objects from the list.
	List(std::initializer_list<value_type> elements, const allocator_type &allocator = allocator_type())
		: List(elements.begin, elements.end, allocator)
	{
	}
	//! Creates a shallow copy of another list.
	List(const List &other)
	{
		this->list = other.list;
		if (this->Object())
		{
			this->Object()->RegisterReference();
		}
	}
	//! Creates a deep copy of another list.
	List(const List &other, const allocator_type &allocator)
		: list(OneToFirstRestToSecond(), allocator)
	{
		this->CreateObject();

		if (other.Object())
		{
			this->Assign(other.First(), other.Last());
		}
	}
	//! Moves a reference to the list object to this one without raising it's reference count.
	List(List &&other)
	{
		this->list.First() = std::move(other.list.First());		// Use move constructor here, since allocator is
		this->list.Second() = other.list.Second();				// the only thing to actually moves, the list
		other.Object() = nullptr;								// itself remains in the same place.
	}
	//! Destroys this shallow copy of the list. Destroys the deep copy as well, if this is the last shallow one.
	~List()
	{
		this->ReleaseObject();
	}

	//! Moves contents of another list to this one.
	List &operator =(List &&other)
	{
		if (this == &other)
		{
			return;
		}

		this->list.First() = std::move(other.list.First());		// Use move constructor here, since allocator is
		this->list.Second() = other.list.Second();				// the only thing to actually moves, the list
		other.Object() = nullptr;								// itself remains in the same place.

		return *this;
	}
	//! Assigns a shallow copy of another list to this one.
	List &operator =(const List &other)
	{
		this->ReleaseObject();

		this->Object() = other.Object();
		if (this->Object())
		{
			this->Object()->RegisterReference();
		}

		return *this;
	}
	//! Assigns a collection of items to this list.
	List &operator =(std::initializer_list<value_type> items)
	{
		this->Assign(items.begin(), items.end());

		return *this;
	}

	//! Removes all live objects from this list.
	void Clear()
	{
		if (this->First() == this->Last())
		{
			// Already cleared.
			return;
		}

		this->Allocator().DeinitializeRange(this->First(), this->Last());
		this->Last() = this->First();
	}
	//! Ensure that this list can fit specified number of elements.
	//!
	//! The capacity grows exponentially.
	void EnsureCapacity(size_type capacity)
	{
		if (this->First() == nullptr)
		{
			this->Object()->AllocateStorage(capacity);
			return;
		}
		
		if (this->End() - this->First() < capacity)
		{
			// Gotta expand.
			auto newCapacity = this->CalculateExpandedCapacity(capacity);
			this->Object()->ReallocateStorage(newCapacity);
		}
	}
	//! Ensures that this list can have _count_ items added to it.
	//!
	//! The capacity grows exponentially.
	void Reserve(size_type count)
	{
		this->EnsureCapacity(count + this->End() - this->First());
	}

	//
	// Putting elements into the list.
	//

	//! Copies an element into the end of the list.
	void Add(const_reference item)
	{
		this->Reserve(1);

		this->Object()->InvalidateIterators(this->Last());

		this->Allocator().Initialize(this->Last(), item);
		++this->Last();
	}
	//! Moves an element into the end of this list.
	void Add(value_type &&item)
	{
		this->Reserve(1);

		this->Object()->InvalidateIterators(this->Last());

		this->Allocator().Initialize(this->Last(), std::forward<value_type>(item));
		++this->Last();
	}
	//! Adds an item to the end of this list.
	List &operator <<(const_reference item)
	{
		this->Add(item);
		return *this;
	}
	//! Adds an item to the end of this list.
	List &operator <<(value_type &&item)
	{
		this->Add(std::forward(item));
		return *this;
	}
	//! Adds a bunch of items to the end of this list.
	List &operator <<(std::initializer_list<value_type> items)
	{
		this->AddRange(items);
		return *this;
	}

	//! Adds a collection of objects into the end of this collection.
	//!
	//! @tparam CollectionType A type of the collection that must satisfy the same conditions as one must satisfy
	//!                        to be used in ranged for loop.
	template<typename CollectionType>
	void AddCollection(const CollectionType &collection)
	{
		for (const auto &item : collection)
		{
			this->Add(item);
		}
	}

	//! Adds a range of items to this collection.
	template<typename IteratorType, typename = typename EnableIf<IsIterator<IteratorType>::value, void>::type>
	void AddRange(IteratorType left, IteratorType right)
	{
		this->AddRange(left, right, IteratorCategory(left));
	}
	template<typename IteratorType>
	void AddRange(IteratorType left, IteratorType right, InputIteratorTag)
	{
		// Since input iterators are single-pass, we cannot find the number of elements between them.
		for (; left != right; left++)
		{
			this->Add(*left);
		}
	}
	template<typename IteratorType>
	void AddRange(IteratorType left, IteratorType right, ForwardIteratorTag)
	{
		this->Reserve(right - left);

		for (; left != right; left++)
		{
			this->Allocator().Initialize(this->Last()++, *left);	// Make a copy.
		}
	}
	//! Inserts another list into this one.
	void AddRange(const List &other)
	{
		this->AddRange(std::remove_reference<const_pointer>::type(other.First()),
					   std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	void AddRange(List &&other)
	{
		this->AddRange(std::remove_reference<const_pointer>::type(other.First()),
					   std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	void AddRange(std::initializer_list<value_type> other)
	{
		this->AddRange(other.begin(), other.end());
	}

	//! Inserts an item into this list by copying it into specified position.
	void Insert(size_type index, const_reference item)
	{
		const_pointer itemPtr = this->Allocator().Address(item);
		this->InsertRange(index, itemPtr, itemPtr + 1);
	}
	//! Inserts an item into this list by copying it into specified position.
	void Insert(const iterator_base &position, const_reference item)
	{
		this->Insert(this->Object()->GetIteratorCurrentIndex(position), item);
	}
	//! Inserts an item into this list by moving it into specified position.
	void Insert(size_type index, value_type &&item)
	{
		this->Emplace(index, std::forward(item));
	}
	//! Inserts an item into this list by moving it into specified position.
	void Insert(const iterator_base &position, value_type &&item)
	{
		this->Insert(this->Object()->GetIteratorCurrentIndex(position), std::forward<value_type>(item));
	}

	//! Inserts a range of elements into the position in the list.
	template<typename IteratorType, typename = typename EnableIf<IsIterator<IteratorType>::value, void>::type>
	void InsertRange(size_type index, IteratorType first, IteratorType last)
	{
		this->InsertRange(index, first, last, IteratorCategory(first));
	}
private:
	template<typename IteratorType>
	void InsertRange(size_type index, IteratorType first, IteratorType last, InputIteratorTag)
	{
		if (index >= this->Length)
		{
			this->AddRange(first, last);
		}
		for (; first != last; ++first)
		{
			this->Insert(index, *first);
		}
	}
	template<typename IteratorType>
	void InsertRange(size_type index, IteratorType first, IteratorType last, ForwardIteratorTag)
	{
		auto oldLength = this->Length;
		if (index >= oldLength)
		{
			this->AddRange(first, last);
		}
		size_type rangeLength = last - first;
		if (this->UnusedCapacity < rangeLength)
		{
			// Allocate new memory for the list.
			size_type oldCapacity = this->Capacity;
			pointer oldMemory = this->First();

			size_type newCapacity = this->CalculateExpandedCapacity(oldCapacity + rangeLength);
			pointer newMemory = this->Allocator().Allocate(newCapacity);
			pointer newMemoryLast = newMemory + oldLength + rangeLength;
			pointer newMemoryEnd = newMemory + newCapacity;

			// Move elements before the insertion point.
			pointer currentSource = oldMemory;
			pointer currentDest = newMemory;
			pointer insertionPoint = newMemory + index;

			for (; currentDest != insertionPoint; currentSource++, currentDest++)
			{
				this->Allocator().Initialize(currentDest, std::move(*currentSource));
			}

			// Copy the elements that need insertion.
			for (; first != last; ++first, currentDest++)
			{
				this->Allocator().Initialize(currentDest, *first);
			}

			// Move the rest.
			for (; currentDest != newMemoryLast; currentSource++, currentDest++)
			{
				this->Allocator().Initialize(currentDest, std::move(*currentSource));
			}

			// Deinitialize and deallocate old memory.
			this->Object()->FreeStorage();

			this->First() = newMemory;
			this->Last() = newMemoryLast;
			this->End() = newMemoryEnd;
		}
		else
		{
			pointer start = this->First();
			pointer insertionRangeStart = start + index;
			pointer insertionRangeEnd = insertionRangeStart + rangeLength;

			// Orphan iterators that are at the insertion point or beyond it.
			this->Object()->InvalidateIterators(this->First() + index, this->Last());

			// Move the range of last elements to make space for other elements.
			this->ShiftRange(insertionRangeStart, this->Last(), insertionRangeEnd);
			this->Last() += rangeLength;

			// Deinitialize the elements in the hole.
			this->Allocator().DeinitializeRange(insertionRangeStart, insertionRangeEnd);

			// Copy elements into the insertion hole.
			pointer currentInsertionPoint = insertionRangeStart;
			for (; currentInsertionPoint != insertionRangeEnd; currentInsertionPoint++, ++first)
			{
				this->Allocator().Initialize(currentInsertionPoint, *first);
			}
		}
	}

public:
	//! Inserts another list into this one.
	void InsertRange(size_type index, const List &other)
	{
		this->InsertRange(index,
						  std::remove_reference<const_pointer>::type(other.First()),
						  std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	void InsertRange(size_type index, List &&other)
	{
		this->InsertRange(index,
						  std::remove_reference<const_pointer>::type(other.First()),
						  std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	void InsertRange(size_type index, std::initializer_list<value_type> other)
	{
		this->InsertRange(index, other.begin(), other.end());
	}
	//! Inserts a collection of items into this list.
	template<typename CollectionType>
	void InsertCollection(size_type index, const CollectionType &collection)
	{
		for (const auto &current : collection)
		{
			this->Insert(index, current);
		}
	}

	//! Adds a new object at the end of the list by constructing it from provided arguments.
	template<typename... ArgumentTypes>
	void Make(ArgumentTypes &&... arguments)
	{
		this->Reserve(1);
		this->Object()->InvalidateIterators(this->Last());

		this->Allocator().Initialize(this->Last(), std::forward(arguments)...);
		++this->Last();
	}

	//! Inserts an object that is constructed from provided arguments at the specified position in the list.
	template<typename... ArgumentTypes>
	void Emplace(size_type index, ArgumentTypes &&... arguments)
	{
		auto oldLength = this->Length;
		if (index >= oldLength)
		{
			this->Make(std::forward(arguments)...);
		}
		if (this->UnusedCapacity < 1)
		{
			// Allocate new memory for the list.
			size_type oldCapacity = this->Capacity;
			pointer oldMemory = this->First();

			size_type newCapacity = this->CalculateExpandedCapacity(oldCapacity + 1);
			pointer newMemory = this->Allocator().Allocate(newCapacity);
			pointer newMemoryLast = newMemory + oldLength + 1;
			pointer newMemoryEnd = newMemory + newCapacity;

			// Move elements before the insertion point.
			pointer currentSource = oldMemory;
			pointer currentDest = newMemory;
			pointer insertionPoint = newMemory + index;

			for (; currentDest != insertionPoint; currentSource++, currentDest++)
			{
				this->Allocator().Initialize(currentDest, std::move(*currentSource));
			}

			// Initialize the element.
			this->Allocator().Initialize(currentDest, std::forward(arguments)...);

			// Move the rest.
			for (; currentDest != newMemoryLast; currentSource++, currentDest++)
			{
				this->Allocator().Initialize(currentDest, std::move(*currentSource));
			}

			// Deinitialize and deallocate old memory.
			this->Object()->FreeStorage();

			this->First() = newMemory;
			this->Last() = newMemoryLast;
			this->End() = newMemoryEnd;
		}
		else
		{
			pointer start = this->First();
			pointer insertionPoint = start + index;

			// Orphan iterators that are at the insertion point or beyond it.
			this->Object()->InvalidateIterators(this->First() + index, this->Last());

			// Move the range of last elements to make space for other elements.
			this->ShiftRange(insertionPoint, this->Last(), insertionPoint + 1);
			this->Last()++;

			// Deinitialize the elements in the hole.
			this->Allocator().Deinitialize(insertionPoint);

			// Initialize the element in the insertion hole.
			this->Allocator().Initialize(insertionPoint, std::forward(arguments)...);
		}
	}

	//! Replaces an item at the specified position with another one.
	void Replace(size_type index, const value_type &item)
	{
#ifdef DEBUG_ITERATION
		if (index >= this->Last())
		{
			throw std::out_of_range("Attempted to access an element outside of the list.");
		}
#endif // DEBUG_ITERATION

		pointer position = this->First() + index;

		this->Allocator().Deinitialize(position);
		this->Allocator().Initialize(position, item);
	}
	//! Replaces an item at the specified position with another one.
	void Replace(size_type index, value_type &&item)
	{
#ifdef DEBUG_ITERATION
		if (index >= this->Last())
		{
			throw std::out_of_range("Attempted to access an element outside of the list.");
		}
#endif // DEBUG_ITERATION

		pointer position = this->First() + index;

		this->Allocator().Deinitialize(position);
		this->Allocator().Initialize(position, std::forward(item));
	}

	//! Removes an element from the back of this list.
	void Cut()
	{
		if (this->Empty)
		{
			return;
		}
		this->Allocator().Deinitialize(this->Last()--);
		this->Object()->InvalidateIterators(this->Last());
	}
	//! Removes specified number of elements from the back of this list.
	void Cut(size_type count)
	{
		for (size_t i = 0; i < count && this->Last() != this->First(); i++)
		{
			this->Allocator().Deinitialize(this->Last()--);
		}
		this->Object()->InvalidateIterators(this->Last(), this->Last() + count - 1);
	}
	//! Removes a range of elements from this list.
	void Erase(iterator_base left, iterator_base right)
	{
		this->Erase(this->Object()->GetIteratorCurrentIndex(left),
					this->Object()->GetIteratorCurrentIndex(right));
	}
	//! Removes an element from this list.
	void Erase(iterator_base position)
	{
		pointer pos = this->Object()->GetIteratorCurrentIndex(position);
		this->Erase(pos, pos + 1);
	}
	//! Removes an element from this list.
	void Erase(size_type position)
	{
		pointer pos = this->First() + position;
#ifdef DEBUG_ITERATION
		if (pos >= this->Last())
		{
			throw std::out_of_range("Attempted to remove an element outside the list.");
		}
#endif // DEBUG_ITERATION

		this->Erase(pos, pos + 1);
	}
private:
	//! Removes a range of elements from this list.
	void Erase(pointer left, pointer right)
	{
		this->Object()->InvalidateIterators(left, right - 1);

		this->Allocator().DeinitializeRange(left, right);

		this->ShiftRange(right, this->Last());

		this->Cut(right - left);
	}

public:
	//! Shrinks the capacity of this list to be exact fit for all live objects currently in it.
	void Trim()
	{
		if (this->UnusedCapacity == 0)
		{
			return;
		}
		this->Object()->ReallocateStorage(this->Length);
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
	//! integers.Insert(~integers.BinarySearch(5), 5);
	//! integers.Insert(~integers.BinarySearch(1), 1);
	//! integers.Insert(~integers.BinarySearch(3), 3);
	//! integers.Insert(~integers.BinarySearch(4), 4);
	//! integers.Insert(~integers.BinarySearch(2), 2);
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
	int BinarySearch(reference element, Comparison comparer = nullptr) const
	{
		Comparison comparison;
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
			difference_type i = lo + (hi - lo >> 1);
			int order = comparison(this->First()[i], element);

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

	//! Creates a deep copy of this list.
	//!
	//! @param itemCount Reference to the number that, once this function completes, will contain the length of
	//!                  the resultant array.
	//! @param allocator An object to use to allocate the memory for the deep copy.
	//!
	//! @returns A pointer to array that will have to be deleted manually. A null pointer is returned, if this
	//!          list is empty.
	template<typename CustomAllocatorType = DefaultAllocator<value_type>>
	pointer Duplicate(size_type &itemCount, const CustomAllocatorType &allocator)
	{
		if (this->Empty)
		{
			return nullptr;
		}

		itemCount = this->Length;
		pointer items = allocator.Allocate(itemCount);
		pointer last = this->Last();

		for (pointer currentSource = this->First(), currentDest = items;
			 currentSource != last;
			 currentSource++, currentDest++)
		{
			allocator.Initialize(currentDest, *currentSource);
		}

		return items;
	}

	//! Creates an iterator that can be used to begin traversal of this list.
	iterator_read_write begin()
	{
		return iterator_read_write(this->First(), this->Object());
	}
	//! Creates an iterator that can be used to finish traversal of this list.
	iterator_read_write end()
	{
		return iterator_read_write(this->Last(), this->Object());
	}
	//! Creates an iterator that can be used to begin traversal of this list.
	iterator_read_only begin() const
	{
		return iterator_read_only(this->First(), this->Object());
	}
	//! Creates an iterator that can be used to finish traversal of this list.
	iterator_read_only end() const
	{
		return iterator_read_only(this->Last(), this->Object());
	}
	//! Creates an iterator that can be used to begin traversal of this list.
	iterator_read_only cbegin() const
	{
		return iterator_read_only(this->First(), this->Object());
	}
	//! Creates an iterator that can be used to finish traversal of this list.
	iterator_read_only cend() const
	{
		return iterator_read_only(this->Last(), this->Object());
	}
	//! Creates an iterator that can be used to begin traversal of this list in reverse.
	iterator_read_write rbegin()
	{
		return iterator_read_write(this->Last() - 1, this->Object(), -1);
	}
	//! Creates an iterator that can be used to finish traversal of this list in reverse.
	iterator_read_write rend()
	{
		return iterator_read_write(this->First() - 1, this->Object(), -1);
	}
	//! Creates an iterator that can be used to begin traversal of this list in reverse.
	iterator_read_only rbegin() const
	{
		return iterator_read_only(this->Last() - 1, this->Object(), -1);
	}
	//! Creates an iterator that can be used to finish traversal of this list in reverse.
	iterator_read_only rend() const
	{
		return iterator_read_only(this->First() - 1, this->Object(), -1);
	}
	//! Creates an iterator that can be used to begin traversal of this list in reverse.
	iterator_read_only crbegin() const
	{
		return iterator_read_only(this->Last() - 1, this->Object(), -1);
	}
	//! Creates an iterator that can be used to finish traversal of this list in reverse.
	iterator_read_only crend() const
	{
		return iterator_read_only(this->First() - 1, this->Object(), -1);
	}

private:
	// Creates a list object.
	void CreateObject()
	{
		typename allocator_type::template rebind<list_object_type>::other objectAllocator(this->Allocator());

		this->Object() = objectAllocator.Allocate(1);
		objectAllocator.Initialize(this->Object(), list_object_type());
	}
	// Destroys the list object, if its reference count reaches 0.
	void ReleaseObject()
	{
		if (!this->Object() || this->Object()->UnregisterReference())
		{
			return;
		}

		// Delete the object, since there are no live references to it.
		typename allocator_type::template rebind<list_object_type>::other objectAllocator(this->Allocator());

		objectAllocator.Deallocate(this->Object());
		this->Object() = nullptr;
	}

	void Assign(const_pointer left, const_pointer right)
	{
		this->Clear();

		if (left == right || left == nullptr || right == nullptr)
		{
			return;
		}
		if (left > right)
		{
			std::swap(left, right);
		}

		size_type capacity = right - left;
		this->EnsureCapacity(capacity);

		this->Last() = this->CopyRange(this->First(), left, right);
	}
	//! Copies a range of elements by using a copy constructor.
	pointer CopyRange(pointer leftDestination, const_pointer leftSource, const_pointer rightSource)
	{
		pointer currentDestination = leftDestination;
		const_pointer currentSource = leftSource;

		while (currentSource != rightSource)
		{
			// Use the constant reference to make sure that it's the copy constructor that gets invoked.
			const_reference currentObjectToCopy = currentSource;
			this->Allocator().Initialize(currentDestination, currentObjectToCopy);

			currentDestination++;
			currentSource++;
		}

		return currentDestination;
	}
	void ShiftRange(pointer firstSource, pointer lastSource, pointer destination)
	{
		difference_type offset = destination - firstSource;

		pointer last = this->Last();

		if (offset > 0)
		{
			pointer currentSource = lastSource;
			pointer currentDestination = currentSource + offset;

			// Move right to left.
			for (; currentSource != firstSource; currentSource--, currentDestination--)
			{
				if (currentDestination < last)
				{
					// Destroy the item, if it is within this list.
					this->Allocator().Deinitialize(currentDestination);
				}

				this->Allocator().Initialize(currentDestination, std::move(*currentSource));
			}
		}
		else if (offset < 0)
		{
			pointer currentSource = firstSource;
			pointer currentDestination = currentSource + offset;

			// Move right to left.
			for (; currentSource != lastSource; currentSource++, currentDestination++)
			{
				if (currentDestination < last)
				{
					// Destroy the item, if it is within this list.
					this->Allocator().Deinitialize(currentDestination);
				}

				this->Allocator().Initialize(currentDestination, std::move(*currentSource));
			}
		}
	}
	size_type CalculateExpandedCapacity(size_type minimalDesiredCapacity) const
	{
		size_type result = this->Capacity;

		if (this->Allocator().MaxSize() - result / 2 < result)
		{
			result = 0;	// Too big for normal expansion.
		}
		else
		{
			result += result / 2;
		}

		return result < minimalDesiredCapacity
			? minimalDesiredCapacity
			: result;
	}
	template<typename IteratorType>
	void Build(IteratorType first, IteratorType last)
	{
		this->Build(first, last, IteratorCategory(first));
	}
	template<typename IteratorType>
	void Build(IteratorType first, IteratorType last, InputIteratorTag)
	{
		// Since input iterators are single-pass, we cannot find the number of elements between them.
		for (; first != last; ++first)
		{
			this->Make(*first);
		}
	}
	template<typename IteratorType>
	void Build(IteratorType first, IteratorType last, ForwardIteratorTag)
	{
		this->EnsureCapacity(last - first);

		for (; first != last; first++)
		{
			this->Allocator().Initialize(this->Last()++, *first);	// Make a copy.
		}
	}
};

template<typename ElementType, typename AllocatorType>
inline void DeleteAll(List<ElementType, AllocatorType> &list)
{
	for (auto &current : list)
	{
		delete current;
	}
}