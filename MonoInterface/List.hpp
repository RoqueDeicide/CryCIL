#pragma once

#include <functional>
#include "ExtraTypeTraits.h"
#include "Tuples.h"
#include "Allocation.hpp"
#include "Iteration.hpp"
#include "ListObject.hpp"

//! Represents a functor that compares objects using their comparison operators.
template<typename ValType1, typename ValType2>
struct DefaultComparison
{
	int operator()(const ValType1 &value1, const ValType2 &value2) const
	{
		return value1 > value2
			? 1
			: value2 > value1
			? -1
			: 0;
	}
};

//! Represents a mutable reference-counted dynamic array of elements.
//!
//! This type resembles a fusion of std::vector from STL and System.Collections.Generic.List`1 from .Net.
//!
//! @tparam ElementType   Type of objects that are going to be contained within this array.
//! @tparam AllocatorType Type of object to use to allocate and deallocate memory, initialize and destroy
//!                       objects. Implementation of such is best done by specializing the DefaultAllocator`1
//!                       template.
template<typename ElementType, typename AllocatorType = DefaultAllocator<ElementType>>
class List
{
	friend ListIteratorConst<List>;
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
	typedef ListIterator<list_object_type> iterator_type;
	typedef ListIteratorConst<list_object_type> const_iterator_type;

	typedef std::function<int(const_reference, const_reference)> Comparison;

private:
	//! A compressed pair of 2 objects: an allocator object that is used to work with memory and a pointer to
	//! the list header object.
	list_object_type *list;
	//! Gets appropriately decorated pointer to the list object.
	list_object_type *Object()
	{
		return this->list;
	}
	const list_object_type *Object() const
	{
		return this->list;
	}

	//! Gets the allocator object.
	allocator_type &Allocator()
	{
		return this->Object()->Allocator();
	}

public:
	//! Gets the object that is used to allocate and deallocate memory, and initialize and destroy objects.
	const allocator_type &Allocator() const
	{
		return this->Object()->Allocator();
	}

	//! Gets the pointer to the first element in the list.
	pointer &First()
	{
		return this->Object()->First();
	}
	const pointer &First() const
	{
		return this->Object()->First();
	}
	//! Gets the pointer to the element after last live object in the list.
	pointer &Last()
	{
		return this->Object()->Last();
	}
	const pointer &Last() const
	{
		return this->Object()->Last();
	}
	//! Gets the pointer to the element after last non-live object in the list.
	pointer &End()
	{
		return this->Object()->End();
	}
	const pointer &End() const
	{
		return this->Object()->End();
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
		auto last = this->Last();
		auto first = this->First();
		return last - first;
	}
	//! Indicates whether this list is empty.
	__declspec(property(get = IsEmpty)) bool Empty;
	bool IsEmpty() const
	{
		return this->Last() - this->First() == 0;
	}

	//! Provides read/write access to the element in the list.
	//!
	//! @param index Zero-based index of the object to access.
	reference operator [](size_type index)
	{
#ifdef DEBUG_ITERATION
		if (index >= this->Length)
		{
			throw std::out_of_range("Attempted to access the element of the list that is out of range.");
		}
#endif // DEBUG_ITERATION

		return *(this->First() + index);
	}
	//! Provides read-only access to the element in the list.
	//!
	//! @param index Zero-based index of the object to access.
	const_reference operator[](size_type index) const
	{
		return (*this)[index];
	}
	//! Creates the iterator that can be used to start iteration of the list from.
	//!
	//! @param index A value that defines the position of the iterator; depending on the sign of this index
	//!              it can represent position away from the start of the list (when it's not negative) or away
	//!              from the last element in the list (when it's negative). Passing 0 is equivalent to calling
	//!              @see begin(). Passing -1 is equivalent of calling @see end().
	iterator_type from(difference_type index)
	{
		return iterator_type(this->GetPositionInternal(index), this->Object());
	}
	const_iterator_type from(difference_type index) const
	{
		return const_iterator_type(this->GetPositionInternal(index), this->Object());
	}
	//! Creates the iterator that can be used to finish iteration of the list at.
	//!
	//! @param index A value that defines the position of the iterator; depending on the sign of this index
	//!              it can represent position away from the start of the list (when it's not negative) or away
	//!              from the last element in the list (when it's negative). Passing 0 is equivalent to calling
	//!              @see begin(). Passing -1 is equivalent of calling @see end().
	iterator_type to(difference_type index)
	{
		return iterator_type(this->GetPositionInternal(index), this->Object());
	}
	const_iterator_type to(difference_type index) const
	{
		return const_iterator_type(this->GetPositionInternal(index), this->Object());
	}
private:
	pointer GetPositionInternal(difference_type index)
	{
		if (index < 0)
		{
			return this->Last() + index + 1;
		}
		return this->First() + index;
	}

public:
	//! Creates a new empty list.
	//!
	//! @param allocator An optional value that is an object to use to work with memory.
	List(const allocator_type &allocator = allocator_type())
	{
		this->CreateObject(allocator);
	}
	//! Creates a new empty list.
	//!
	//! @param allocator An optional value that is a temporary object to use to work with memory.
	explicit List(allocator_type &&allocator)
	{
		this->CreateObject(allocator);
	}
	//! Creates an empty that has enough memory to fit specified number of objects.
	//!
	//! @param initialCapacity Number of objects to allocate the memory for.
	//! @param allocator       An optional object to use to work with the memory.
	explicit List(size_type initialCapacity, const allocator_type &allocator = allocator_type())
	{
		this->CreateObject(allocator);
		
		this->list->AllocateStorage(initialCapacity);
	}
	//! Creates a list that is filled with a number of copies of the object.
	//!
	//! @param initialSize Number of copies of given object to populate the new list with.
	//! @param filler      An object to copy when pre-populating the new list.
	//! @param allocator   An optional object to use to work with the memory.
	List(size_type initialSize, const_reference filler, const allocator_type &allocator = allocator_type())
	{
		this->CreateObject(allocator);

		this->list->AllocateStorage(initialSize);

		// Fill the memory with copies of the filler.
		this->Allocator().InitializeRange(this->First(), this->End(), filler);

		this->Last() = this->End();
	}
	//! Creates a new list that contains a deep copy of the range of elements that is delimited by 2 iterators.
	//!
	//! @tparam IteratorType Type of the iterators.
	//!
	//! @param left      An iterator that defines the start of the range.
	//! @param right     An iterator that defines the end of the range (points at the element after last element
	//!                  in the range).
	//! @param allocator An optional object to use to work with the memory.
	template<typename IteratorType, typename = typename EnableIf<IsIterator<IteratorType>::value, void>::type>
	List(IteratorType left, IteratorType right, const allocator_type &allocator = allocator_type())
	{
		this->CreateObject(allocator);

		this->Build(left, right);
	}
	//! Creates a new list that is populated with copies of objects from the list.
	//!
	//! @param elements  A brace initialization list that contains objects to populate the new list with.
	//! @param allocator An optional object to use to work with the memory.
	List(std::initializer_list<value_type> elements, const allocator_type &allocator = allocator_type())
		: List(elements.begin, elements.end, allocator)
	{
	}
	//! Creates a shallow copy of another list.
	//!
	//! @param other Reference to another list.
	List(const List &other)
	{
		this->list = other.list;
		if (this->list)
		{
			this->list->RegisterReference();
		}
	}
	//! Creates a deep copy of another list.
	//!
	//! @param other     Reference to another list.
	//! @param allocator An object to use to work with memory.
	List(const List &other, const allocator_type &allocator)
	{
		this->CreateObject(allocator);

		if (other.list)
		{
			this->Assign(other.First(), other.Last());
		}
	}
	//! Moves a reference to the list object to this one without raising it's reference count.
	//!
	//! @param other Reference to the temporary list.
	List(List &&other)
	{
		this->list = other.list;
		other.list = nullptr;
	}
	//! Destroys this shallow copy of the list. Destroys the deep copy as well, if this is the last shallow one.
	~List()
	{
		this->ReleaseObject();
	}

	//! Moves contents of another list to this one.
	//!
	//! @param other Reference to the temporary list.
	List &operator =(List &&other)
	{
		if (this == &other)
		{
			return;
		}

		this->list = other.list;
		other.list = nullptr;

		return *this;
	}
	//! Assigns a shallow copy of another list to this one.
	//!
	//! @param other Reference to another list.
	List &operator =(const List &other)
	{
		this->ReleaseObject();

		this->list = other.list;
		if (this->list)
		{
			this->list->RegisterReference();
		}

		return *this;
	}
	//! Assigns a collection of items to this list.
	//!
	//! @param items A set of objects to fill this list with after clearing it.
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
	//!
	//! @param capacity The number of objects that must fit into this list before it has to expand after this
	//!                 method is done.
	void EnsureCapacity(size_type capacity)
	{
		if (this->First() == nullptr)
		{
			this->list->AllocateStorage(capacity);
			return;
		}
		
		if (this->Capacity < capacity)
		{
			// Gotta expand.
			auto newCapacity = this->CalculateExpandedCapacity(capacity);
			this->list->ReallocateStorage(newCapacity);
		}
	}
	//! Ensures that this list can have _count_ items added to it.
	//!
	//! The capacity grows exponentially.
	//!
	//! @param count The number of objects that must be allowed to be added to this list without it having
	//!              to expand after this method is done.
	void Reserve(size_type count)
	{
		this->EnsureCapacity(count + this->Last() - this->First());
	}

	//
	// Putting elements into the list.
	//

	//! Copies an element into the end of the list.
	//!
	//! @param item Reference to the object to copy into this list.
	void Add(const_reference item)
	{
		this->Reserve(1);

		pointer last = this->Last();

		this->list->InvalidateIterators(last);

		this->Allocator().Initialize(last, item);
		this->Last() = last + 1;
	}
	//! Moves an element into the end of this list.
	//!
	//! @param item Reference to the object to move into this list.
	void Add(value_type &&item)
	{
		this->Reserve(1);

		pointer last = this->Last();

		this->list->InvalidateIterators(last);

		this->Allocator().Initialize(last, std::forward<value_type>(item));
		this->Last() = last + 1;
	}
	//! Adds an item to the end of this list.
	//!
	//! @param item Reference to the object to copy into this list.
	List &operator <<(const_reference item)
	{
		this->Add(item);
		return *this;
	}
	//! Adds an item to the end of this list.
	//!
	//! @param item Reference to the object to move into this list.
	List &operator <<(value_type &&item)
	{
		this->Add(std::forward(item));
		return *this;
	}
	//! Adds a bunch of items to the end of this list.
	//!
	//! @param items A list of items to add to this list.
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
	//!
	//! @tparam IteratorType Type of the iterators.
	//!
	//! @param left  An iterator that points at the first object to add to this list.
	//! @param right An iterator that marks the end of the range.
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
	//!
	//! @param other Reference to the list to add to this one.
	void AddRange(const List &other)
	{
		this->AddRange(std::remove_reference<const_pointer>::type(other.First()),
					   std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	//!
	//! @param other Reference to the list to add to this one.
	void AddRange(List &&other)
	{
		this->AddRange(std::remove_reference<const_pointer>::type(other.First()),
					   std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	//!
	//! @param other Reference to the list to add to this one.
	void AddRange(std::initializer_list<value_type> other)
	{
		this->AddRange(other.begin(), other.end());
	}

	//! Inserts an item into this list by copying it into specified position.
	//!
	//! @param index Zero-based index of the position to insert the item into. If this value is out of list's
	//!              range then the item is added to end.
	//! @param item  Reference to the object to copy.
	void Insert(size_type index, const_reference item)
	{
		this->Insert(this->First() + index, item);
	}
	//! Inserts an item into this list by copying it into specified position.
	//!
	//! @param position Iterator that represents the position to insert the item into. If this value is out of
	//!                 list's range then the item is added to end.
	//! @param item     Reference to the object to copy.
	void Insert(const_iterator_type position, const_reference item)
	{
		this->Insert(this->GetUncheckedPosition(position), item);
	}
	//! Inserts an item into this list by moving it into specified position.
	//!
	//! @param index Zero-based index of the position to insert the item into. If this value is out of list's
	//!              range then the item is added to end.
	//! @param item  Reference to the object to move.
	void Insert(size_type index, value_type &&item)
	{
		this->Emplace(index, std::forward<value_type>(item));
	}
	//! Inserts an item into this list by moving it into specified position.
	//!
	//! @param position Iterator that represents the position to insert the item into. If this value is out of
	//!                 list's range then the item is added to end.
	//! @param item     Reference to the object to move.
	void Insert(const_iterator_type position, value_type &&item)
	{
		this->Insert(this->GetUncheckedPosition(position), std::forward<value_type>(item));
	}

	//! Inserts a range of elements into the position in the list.
	//!
	//! @tparam IteratorType Type of iterators.
	//!
	//! @param index Zero-based index of the position to insert the items into. If this value is out of list's
	//!              range then the items are added to end.
	//! @param first An iterator that represents the start of range of items to add.
	//! @param last  An iterator that represents the end of the range of items.
	template<typename IteratorType, typename = typename EnableIf<IsIterator<IteratorType>::value, void>::type>
	void InsertRange(size_type index, IteratorType first, IteratorType last)
	{
		this->InsertRange(this->First() + index, first, last);
	}
	//! Inserts a range of elements into the position in the list.
	//!
	//! @tparam IteratorType Type of iterators.
	//!
	//! @param position Iterator that represents the position to insert the items into. If this value is out of
	//!                 list's range then the items are added to end.
	//! @param first    An iterator that represents the start of range of items to add.
	//! @param last     An iterator that represents the end of the range of items.
	template<typename IteratorType, typename = typename EnableIf<IsIterator<IteratorType>::value, void>::type>
	void InsertRange(const_iterator_type position, IteratorType first, IteratorType last)
	{
		this->InsertRange(this->GetUncheckedPosition(position), first, last);
	}
private:
	void Insert(pointer position, const_reference item)
	{
		const_pointer itemPtr = this->Allocator().Address(item);
		this->InsertRange(position, itemPtr, itemPtr + 1);
	}
	void Insert(pointer position, value_type &&item)
	{
		this->Emplace(position, std::forward<value_type>(item));
	}
	template<typename IteratorType>
	void InsertRange(pointer position, IteratorType first, IteratorType last)
	{
		this->InsertRange(position, first, last, IteratorCategory(first));
	}
	template<typename IteratorType>
	void InsertRange(pointer position, IteratorType first, IteratorType last, InputIteratorTag)
	{
		if (position >= this->Last())
		{
			this->AddRange(first, last);
		}
		for (; first != last; ++first)
		{
			this->Insert(position, *first);
		}
	}
	template<typename IteratorType>
	void InsertRange(pointer position, IteratorType first, IteratorType last, ForwardIteratorTag)
	{
		if (position >= this->Last())
		{
			this->AddRange(first, last);
			return;
		}
		size_type rangeLength = last - first;
		if (this->UnusedCapacity < rangeLength)
		{
			// Allocate new memory for the list.
			size_type oldCapacity = this->Capacity;
			pointer oldMemory = this->First();

			auto oldLength = this->Length;
			size_type index = position - oldMemory;

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
			this->list->FreeStorage();

			this->First() = newMemory;
			this->Last() = newMemoryLast;
			this->End() = newMemoryEnd;
		}
		else
		{
			pointer insertionRangeStart = position;
			pointer insertionRangeEnd = insertionRangeStart + rangeLength;
			pointer insertionRangeLast =			// This value is used to prevent deinitialization of
				insertionRangeEnd > this->Last()	// objects beyond usable bounds of the list.
				? this->Last()
				: insertionRangeEnd;

			// Orphan iterators that are at the insertion point or beyond it.
			this->list->InvalidateIterators(insertionRangeStart, this->Last());

			// Move the range of last elements to make space for other elements.
			this->ShiftRange(insertionRangeStart, this->Last(), insertionRangeEnd);
			this->Last() += rangeLength;

			// Deinitialize the elements in the hole.
			this->Allocator().DeinitializeRange(insertionRangeStart, insertionRangeLast);

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
	//!
	//! @param index Zero-based index of the position to insert the items into. If this value is out of list's
	//!              range then the items are added to end.
	//! @param other Reference to the list to copy the items from.
	void InsertRange(size_type index, const List &other)
	{
		this->InsertRange(this->First() + index,
						  std::remove_reference<const_pointer>::type(other.First()),
						  std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	//!
	//! @param position Iterator that represents the position to insert the items into. If this value is out of
	//!                 list's range then the items are added to end.
	//! @param other    Reference to the list to copy the items from.
	void InsertRange(const_iterator_type position, const List &other)
	{
		this->InsertRange(this->GetUncheckedPosition(position),
						  std::remove_reference<const_pointer>::type(other.First()),
						  std::remove_reference<const_pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	//!
	//! @param index Zero-based index of the position to insert the items into. If this value is out of list's
	//!              range then the items are added to end.
	//! @param other Reference to the list to copy the items from.
	void InsertRange(size_type index, List &&other)
	{
		this->InsertRange(this->First() + index,
						  std::remove_reference<pointer>::type(other.First()),
						  std::remove_reference<pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	//!
	//! @param position Iterator that represents the position to insert the items into. If this value is out of
	//!                 list's range then the items are added to end.
	//! @param other    Reference to the list to copy the items from.
	void InsertRange(const_iterator_type position, List &&other)
	{
		this->InsertRange(this->GetUncheckedPosition(position),
						  std::remove_reference<pointer>::type(other.First()),
						  std::remove_reference<pointer>::type(other.Last()));
	}
	//! Inserts another list into this one.
	//!
	//! @param index Zero-based index of the position to insert the items into. If this value is out of list's
	//!              range then the items are added to end.
	//! @param other Reference to the list to copy the items from.
	void InsertRange(size_type index, std::initializer_list<value_type> other)
	{
		this->InsertRange(this->First() + index, other.begin(), other.end());
	}
	//! Inserts another list into this one.
	//!
	//! @param position Iterator that represents the position to insert the items into. If this value is out of
	//!                 list's range then the items are added to end.
	//! @param other    Reference to the list to copy the items from.
	void InsertRange(const_iterator_type position, std::initializer_list<value_type> other)
	{
		this->InsertRange(this->GetUncheckedPosition(position), other.begin(), other.end());
	}
	//! Inserts a collection of items into this list.
	//!
	//! @tparam CollectionType A type of the collection that must satisfy the same conditions as one must satisfy
	//!                        to be used in ranged for loop.
	//!
	//! @param index Zero-based index of the position to insert the items into. If this value is out of list's
	//!              range then the items are added to end.
	template<typename CollectionType>
	void InsertCollection(size_type index, const CollectionType &collection)
	{
		for (const auto &current : collection)
		{
			this->Insert(this->First() + index, current);
		}
	}
	//! Inserts a collection of items into this list.
	//!
	//! @tparam CollectionType A type of the collection that must satisfy the same conditions as one must satisfy
	//!                        to be used in ranged for loop.
	//!
	//! @param position Iterator that represents the position to insert the items into. If this value is out of
	//!                 list's range then the items are added to end.
	template<typename CollectionType>
	void InsertCollection(const_iterator_type position, const CollectionType &collection)
	{
		for (const auto &current : collection)
		{
			this->Insert(this->GetUncheckedPosition(position), current);
		}
	}

	//! Adds a new object at the end of the list by constructing it from provided arguments.
	//!
	//! @tparam ArgumentTypes Types of arguments to pass to the constructor.
	//!
	//! @param arguments A set of arguments to pass to the constructor.
	template<typename... ArgumentTypes>
	void Make(ArgumentTypes &&... arguments)
	{
		this->Reserve(1);
		this->list->InvalidateIterators(this->Last());

		this->Allocator().Initialize(this->Last(), std::forward<ArgumentTypes>(arguments)...);
		++this->Last();
	}

	//! Inserts an object that is constructed from provided arguments at the specified position in the list.
	//!
	//!
	//! @tparam ArgumentTypes Types of arguments to pass to the constructor.
	//!
	//! @param index     Zero-based index of the position to insert the item into. If this value is out of list's
	//!                  range then the item is added to end.
	//! @param arguments A set of arguments to pass to the constructor.
	template<typename... ArgumentTypes>
	void Emplace(size_type index, ArgumentTypes &&... arguments)
	{
		this->Emplace(this->First() + index, std::forward<ArgumentTypes>(arguments)...);
	}
	//! Inserts an object that is constructed from provided arguments at the specified position in the list.
	//!
	//!
	//! @tparam ArgumentTypes Types of arguments to pass to the constructor.
	//!
	//! @param position Iterator that represents the position to insert the item into. If this value is out of
	//!                 list's range then the item is added to end.
	//! @param arguments A set of arguments to pass to the constructor.
	template<typename... ArgumentTypes>
	void Emplace(const_iterator_type position, ArgumentTypes &&... arguments)
	{
		this->Emplace(this->GetUncheckedPosition(position), std::forward<ArgumentTypes>(arguments)...);
	}
private:
	template<typename... ArgumentTypes>
	void Emplace(pointer position, ArgumentTypes &&... arguments)
	{
		if (position >= this->Last())
		{
			this->Make(std::forward<ArgumentTypes>(arguments)...);
			return;
		}
		if (this->UnusedCapacity < 1)
		{
			// Allocate new memory for the list.
			size_type oldCapacity = this->Capacity;
			pointer oldMemory = this->First();
			auto oldLength = this->Length;
			size_type index = position - oldMemory;

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
			this->Allocator().Initialize(currentDest++, std::forward<ArgumentTypes>(arguments)...);

			// Move the rest.
			for (; currentDest != newMemoryLast; currentSource++, currentDest++)
			{
				this->Allocator().Initialize(currentDest, std::move(*currentSource));
			}

			// Deinitialize and deallocate old memory.
			this->list->FreeStorage();

			this->First() = newMemory;
			this->Last() = newMemoryLast;
			this->End() = newMemoryEnd;
		}
		else
		{
			// Orphan iterators that are at the insertion point or beyond it.
			this->list->InvalidateIterators(position, this->Last());

			// Move the range of last elements to make space for other elements.
			this->ShiftRange(position, this->Last(), position + 1);
			this->Last()++;

			// Deinitialize the elements in the hole.
			this->Allocator().Deinitialize(position);

			// Initialize the element in the insertion hole.
			this->Allocator().Initialize(position, std::forward<ArgumentTypes>(arguments)...);
		}
	}

public:
	//! Replaces an item at the specified position with another one.
	//!
	//! @param index Zero-based index of the item to replace.
	//! @param item  Reference to the object which copy is going to replace the element in the list.
	void Replace(size_type index, const value_type &item)
	{
		this->ReplaceInternal(this->First() + index, item);
	}
	//! Replaces an item at the specified position with another one.
	//!
	//! @param position Iterator that points at the item to replace.
	//! @param item     Reference to the object which copy is going to replace the element in the list.
	void Replace(const_iterator_type position, const value_type &item)
	{
		this->ReplaceInternal(this->GetUncheckedPosition(position), item);
	}
	//! Replaces an item at the specified position with another one.
	//!
	//! @param index Zero-based index of the item to replace.
	//! @param item  Reference to the object which is going to replace the element in the list.
	void Replace(size_type index, value_type &&item)
	{
		this->ReplaceInternal(this->First() + index, std::forward<value_type>(item));
	}
	//! Replaces an item at the specified position with another one.
	//!
	//! @param position Iterator that points at the item to replace.
	//! @param item     Reference to the object which is going to replace the element in the list.
	void Replace(const_iterator_type position, value_type &&item)
	{
		this->ReplaceInternal(this->GetUncheckedPosition(position), std::forward<value_type>(item));
	}

private:
	void ReplaceInternal(pointer position, const value_type &item)
	{
#ifdef DEBUG_ITERATION
		if (position >= this->Last())
		{
			throw std::out_of_range("Attempted to access an element outside of the list.");
		}
#endif // DEBUG_ITERATION

		this->Allocator().Deinitialize(position);
		this->Allocator().Initialize(position, item);
	}
	void ReplaceInternal(pointer position, value_type &&item)
	{
#ifdef DEBUG_ITERATION
		if (position >= this->Last())
		{
			throw std::out_of_range("Attempted to access an element outside of the list.");
		}
#endif // DEBUG_ITERATION

		this->Allocator().Deinitialize(position);
		this->Allocator().Initialize(position, std::forward<value_type>(item));
	}

public:

	//! Removes specified number of elements from the back of this list.
	//!
	//! @param count Number of elements to remove.
	void Cut(size_type count = 1)
	{
		if (this->Empty || count == 0)
		{
			return;
		}
		auto length = this->Length;
		if (count > length)
		{
			count = length;
		}
		pointer firstToRemove = this->Last() - count;
		this->Allocator().DeinitializeRange(firstToRemove, this->Last());
		this->list->InvalidateIterators(firstToRemove, this->Last() - 1);
		this->Last() -= count;
	}
	//! Removes a range of elements from this list.
	//!
	//! @param left  An iterator that represents the start of the range of elements to remove.
	//! @param right An iterator that represents the end of the range of elements to remove.
	void Erase(const_iterator_type left, const_iterator_type right)
	{
		this->Erase(left.GetUnchecked(), right.GetUnchecked());
	}
	//! Removes a range of elements from this list.
	//!
	//! @param index Zero-based index of the first element to erase.
	//! @param count Number of elements to remove.
	void Erase(size_type index, size_type count)
	{
		pointer start = this->First() + index;

#ifdef DEBUG_ITERATION
		if (start + count >= this->Last())
		{
			throw std::out_of_range("Attempted to remove elements outside the list.");
		}
#endif // DEBUG_ITERATION

		this->Erase(start, start + count);
	}
	//! Removes an element from this list.
	//!
	//! @param index Zero-based index of the element to erase.
	void Erase(size_type index)
	{
		pointer pos = this->First() + index;
#ifdef DEBUG_ITERATION
		if (pos >= this->Last())
		{
			throw std::out_of_range("Attempted to remove an element outside the list.");
		}
#endif // DEBUG_ITERATION
		this->Erase(pos, pos + 1);
	}
	//! Removes an element from this list.
	//!
	//! @param position Iterator that is at the element to erase.
	void Erase(const_iterator_type position)
	{
		pointer pos = position.GetUnchecked();
		this->Erase(pos, pos + 1);
	}

private:
	//! Removes a range of elements from this list.
	void Erase(pointer left, pointer right)
	{
		this->list->InvalidateIterators(left, right - 1);

		this->ShiftRange(right, this->Last(), left);

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
		this->list->ReallocateStorage(this->Length);
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
	//!                 This object must have a parentheses operator (like a function or functor) that can
	//!                 accept 2 arguments. During the function execution this object will be invoked in the
	//!                 following way: comparer(element in the list, provided element), so make sure the
	//!                 parameters are of appropriate types.
	//!
	//! @returns A zero-based index of the element, if it was found in the list.
	template<typename ValType>
	difference_type BinarySearch(const ValType &element) const
	{
		return this->BinarySearchInternal(element, DefaultComparison<ValType, ValType>());
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
	//!                 This object must have a parentheses operator (like a function or functor) that can
	//!                 accept 2 arguments. During the function execution this object will be invoked in the
	//!                 following way: comparer(element in the list, provided element), so make sure the
	//!                 parameters are of appropriate types.
	//!
	//! @returns A zero-based index of the element, if it was found in the list.
	template<typename ValType, typename PredicateType>
	difference_type BinarySearch(const ValType &element, PredicateType comparer) const
	{
		return this->BinarySearchInternal(element, comparer);
	}
private:
	template<typename ValType, typename PredicateType>
	difference_type BinarySearchInternal(const ValType &value, PredicateType predicate) const
	{
		difference_type lo = 0;
		difference_type hi = this->Length - 1;
		while (lo <= hi)
		{
			difference_type i = lo + ((hi - lo) >> 1);
			int order = predicate(this->First()[i], value);

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
public:

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
	iterator_type begin()
	{
		return iterator_type(this->First(), this->list);
	}
	//! Creates an iterator that can be used to finish traversal of this list.
	iterator_type end()
	{
		return iterator_type(this->Last(), this->list);
	}
	//! Creates an iterator that can be used to begin traversal of this list.
	const_iterator_type begin() const
	{
		return const_iterator_type(this->First(), this->list);
	}
	//! Creates an iterator that can be used to finish traversal of this list.
	const_iterator_type end() const
	{
		return const_iterator_type(this->Last(), this->list);
	}
	//! Creates an iterator that can be used to begin traversal of this list.
	const_iterator_type cbegin() const
	{
		return const_iterator_type(this->First(), this->list);
	}
	//! Creates an iterator that can be used to finish traversal of this list.
	const_iterator_type cend() const
	{
		return const_iterator_type(this->Last(), this->list);
	}
	//! Creates an iterator that can be used to begin traversal of this list in reverse.
	iterator_type rbegin()
	{
		return iterator_type(this->Last() - 1, this->list, -1);
	}
	//! Creates an iterator that can be used to finish traversal of this list in reverse.
	iterator_type rend()
	{
		return iterator_type(this->First() - 1, this->list, -1);
	}
	//! Creates an iterator that can be used to begin traversal of this list in reverse.
	const_iterator_type rbegin() const
	{
		return const_iterator_type(this->Last() - 1, this->list, -1);
	}
	//! Creates an iterator that can be used to finish traversal of this list in reverse.
	const_iterator_type rend() const
	{
		return const_iterator_type(this->First() - 1, this->list, -1);
	}
	//! Creates an iterator that can be used to begin traversal of this list in reverse.
	const_iterator_type crbegin() const
	{
		return const_iterator_type(this->Last() - 1, this->list, -1);
	}
	//! Creates an iterator that can be used to finish traversal of this list in reverse.
	const_iterator_type crend() const
	{
		return const_iterator_type(this->First() - 1, this->list, -1);
	}

private:
	// Creates a list object.
	template<typename AllocType>
	void CreateObject(AllocType &&allocator)
	{
		// Create the allocator object from the forwarded reference.
		AllocType allocObj(std::forward<AllocType>(allocator));

		// Create a copy of the allocator object that can handle allocation of the ListObject.
		typename allocator_type::template rebind<list_object_type>::other objectAllocator(allocObj);

		// Allocate memory for the list object.
		this->list = objectAllocator.Allocate(1);

		// Initialize the list object with a moved allocator object.
		objectAllocator.Initialize(this->list, std::move(allocObj));
	}
	// Destroys the list object, if its reference count reaches 0.
	void ReleaseObject()
	{
		if (!this->list || this->list->UnregisterReference())
		{
			return;
		}

		// Delete the object, since there are no live references to it.
		typename allocator_type::template rebind<list_object_type>::other objectAllocator(this->Allocator());

		objectAllocator.Deallocate(this->list);
		this->list = nullptr;
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
	// firstSource First element in the range.
	// lastSource  Element after last element in the range.
	void ShiftRange(pointer firstSource, pointer lastSource, pointer destination)
	{
		pointer lastItem = this->Last();

		difference_type shift = destination - firstSource;

		if (shift > 0)
		{
			// Shift a range to the right.

			// This is done by going through elements from right to left
			pointer currentSource = lastSource - 1;
			pointer currentDest = currentSource + shift;
			pointer currentSourceEnd = firstSource - 1;

			for (; currentSource != currentSourceEnd; currentSource--, currentDest--)
			{
				// Destroy the item, if it is within this list.
				if (currentDest < lastItem)
				{
					this->Allocator().Deinitialize(currentDest);
				}

				// Shift the element
				this->Allocator().Initialize(currentDest, std::move(*currentSource));
			}
		}
		else if (shift < 0)
		{
			// Shift the range to the left.

			// This is done by going through elements from left to right.
			pointer currentSource = firstSource;
			pointer currentDest = currentSource + shift;
			pointer currentSourceEnd = lastSource;

			for (; currentSource != currentSourceEnd; currentSource++, currentDest++)
			{
				if (currentDest < lastItem)
				{
					// Destroy the item, if it is within this list.
					this->Allocator().Deinitialize(currentDest);
				}

				this->Allocator().Initialize(currentDest, std::move(*currentSource));
			}
		}
	}
	size_type CalculateExpandedCapacity(size_type minimalDesiredCapacity) const
	{
		size_type result = this->Capacity;

		if (this->Allocator().MaxLength() - result / 2 < result)
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
	pointer GetUncheckedPosition(const const_iterator_type &iter)
	{
		return pointer(iter.GetUnchecked());
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