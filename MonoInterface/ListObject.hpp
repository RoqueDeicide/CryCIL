#pragma once

template<typename ElementType, typename AllocatorType> class List;

//! Represents a header of the list. All shallow copies of the same list point at the same header object.
template<typename ElementType, typename AllocatorType>
class ListObject
{
	friend List<ElementType, AllocatorType>;
	friend ListIteratorConst<ListObject>;
public:
	typedef ElementType value_type;
	typedef ElementType &reference;
	typedef const ElementType &const_reference;
	typedef ElementType *pointer;
	typedef const ElementType *const_pointer;

	typedef size_t size_type;
	typedef ptrdiff_t difference_type;

	typedef AllocatorType allocator_type;
	typedef ListIteratorConst<ListObject> iterator_type;

	//
	// Fields.
	//

private:
	//! Encapsulates dimensions of the list.
	struct ListDimensions
	{
		pointer First;	//!< Pointer to the first element in the memory that was allocated for this list.
		pointer Last;	//!< Pointer to the element after last live object in the list.
		pointer End;	//!< Pointer to the element after last piece of memory that was allocated for this list.

		ListDimensions(pointer first, pointer last, pointer end)
			: First(first)
			, Last(last)
			, End(end)
		{
		}
		ListDimensions()
			: First(nullptr)
			, Last(nullptr)
			, End(nullptr)
		{
		}
	};

	CompressedPair<allocator_type, ListDimensions> AllocDimensionsPair;

	size_type ReferenceCount;	//!< Number of live references to this list.

#ifdef DEBUG_ITERATION
	//! Pointer to the first iterator_type in the chain of all iterators that work on this list.
	iterator_type *FirstIterator;
#endif // DEBUG_ITERATION

	//
	// Properties.
	//

	//! Gets the allocator object.
	allocator_type &Allocator()
	{
		return this->AllocDimensionsPair.First();
	}
	const allocator_type &Allocator() const
	{
		return this->AllocDimensionsPair.First();
	}
	//! Gets the object that defines dimensions of this list.
	ListDimensions &Dimensions()
	{
		return this->AllocDimensionsPair.Second();
	}
	const ListDimensions &Dimensions() const
	{
		return this->AllocDimensionsPair.Second();
	}

public:
	//! Gets the pointer to the first element in the list.
	pointer &First()
	{
		return this->Dimensions().First;
	}
	const pointer &First() const
	{
		return this->Dimensions().First;
	}
	//! Gets the pointer to the element after last live object in the list.
	pointer &Last()
	{
		return this->Dimensions().Last;
	}
	const pointer &Last() const
	{
		return this->Dimensions().Last;
	}
	//! Gets the pointer to the element after last non-live object in the list.
	pointer &End()
	{
		return this->Dimensions().End;
	}
	const pointer &End() const
	{
		return this->Dimensions().End;
	}

	explicit ListObject(const allocator_type &allocator = allocator_type())
		: AllocDimensionsPair(OneToFirstRestToSecond(), allocator, nullptr, nullptr, nullptr)
		, ReferenceCount(1)
#ifdef DEBUG_ITERATION
		, FirstIterator(nullptr)
#endif // DEBUG_ITERATION
	{
	}
	explicit ListObject(allocator_type &&allocator)
		: AllocDimensionsPair(OneToFirstRestToSecond(), std::move(allocator), nullptr, nullptr, nullptr)
		, ReferenceCount(1)
#ifdef DEBUG_ITERATION
		, FirstIterator(nullptr)
#endif // DEBUG_ITERATION
	{
	}
	//! Registers a live reference to this object.
	void RegisterReference()
	{
		this->ReferenceCount++;
	}
	//! Informs this object about removal of reference to this object.
	size_type UnregisterReference()
	{
		if (--this->ReferenceCount == 0)
		{
			this->InvalidateIterators();
			this->FreeStorage();
		}

		return this->ReferenceCount;
	}

	//
	// Iterator management.
	//

	// Marks all iterators as invalid.
	void InvalidateIterators()
	{
#ifdef DEBUG_ITERATION
		for (iterator_type **next = &this->FirstIterator; *next != 0; *next = (*next)->next)
		{
			(*next)->BecomeDisowned();
		}
		this->FirstIterator = nullptr;
#endif // DEBUG_ITERATION
	}
	// Marks iterators that currently in the provided inclusive range as invalid.
	void InvalidateIterators(pointer
#ifdef DEBUG_ITERATION
							 first
#endif // DEBUG_ITERATION
							 , pointer
#ifdef DEBUG_ITERATION
							 last
#endif // DEBUG_ITERATION
							 )
	{
#ifdef DEBUG_ITERATION
		iterator_type **next = &this->FirstIterator;
		if (!(*next))
		{
			return;
		}

		// Orphan iterators within the range.
		while (*next)
		{
			auto current = (*next)->current;
			if (current >= first && current <= last)
			{
				// Orphan this iterator.
				(*next)->BecomeDisowned();
				*next = (*next)->next;
			}
			else
			{
				// Advance to the next iterator.
				next = &(*next)->next;
			}
		}
#endif // DEBUG_ITERATION
	}
	// Marks iterators that currently at the provided position as invalid.
	void InvalidateIterators(pointer
#ifdef DEBUG_ITERATION
							 element
#endif // DEBUG_ITERATION
							 )
	{
#ifdef DEBUG_ITERATION
		iterator_type **next = &this->FirstIterator;
		if (!(*next))
		{
			return;
		}

		// Orphan iterators at the element.
		while (*next)
		{
			if ((*next)->current == element)
			{
				// Orphan this iterator.
				(*next)->BecomeDisowned();
				*next = (*next)->next;
			}
			else
			{
				// Advance to the next iterator.
				next = &(*next)->next;
			}
		}
#endif // DEBUG_ITERATION
	}

	//
	// Memory management.
	//

private:
	//! Allocates storage for this list.
	//!
	//! If this list already has memory allocated, an error is thrown.
	void AllocateStorage(size_type capacity)
	{
		if (capacity >= this->Allocator().MaxLength())
		{
			throw std::length_error("Attempted to locate too much memory.");
		}
		if (this->First())
		{
			throw std::length_error("Attempted to allocate new memory for the list before discarding old memory.");
		}
		if (capacity == 0)
		{
			return;
		}

		// Allocate memory.
		pointer memBlock = this->Allocator().Allocate(capacity);

		// Initialize pointers.
		this->First() = memBlock;
		this->Last() = memBlock;
		this->End() = memBlock + capacity;
	}
	//! Reallocates the memory that is used by this list.
	void ReallocateStorage(size_type newCapacity)
	{
		if (newCapacity >= this->Allocator().MaxLength())
		{
			throw std::length_error("Attempted to locate too much memory.");
		}
		
		if (!this->First())
		{
			this->AllocateStorage(newCapacity);
			return;
		}
		if (newCapacity == 0)
		{
			this->FreeStorage();
		}

		pointer oldMemory = this->First();
		pointer oldLast = this->Last();

		// Allocate new memory.
		pointer newMemory = this->Allocator().Allocate(newCapacity);
		pointer newEnd = newMemory + newCapacity;
		pointer currentNew = newMemory;

		// Copy all old data to the new memory while invoking move constructors to inform objects about their
		// new locations.
		{
			pointer currentOld = oldMemory;
			for (; currentOld < oldLast && currentNew < newEnd; currentNew++, currentOld++)
			{
				this->Allocator().Initialize(currentNew, std::move(*currentOld));
				this->Allocator().Deinitialize(currentOld);
			}

			// If new capacity is not enough to fit all live objects, then deinitialize the remainder.
			if (currentOld < oldLast)
			{
				this->Allocator().DeinitializeRange(currentOld, oldLast);
			}
		}

		// Deallocate old memory.
		this->Allocator().Deallocate(oldMemory);

		// Update pointers.
		this->First() = newMemory;
		this->Last() = currentNew;
		this->End() = newEnd;
	}
	//! Releases memory that is taken up by this list.
	void FreeStorage()
	{
		if (!this->First())
		{
			return;
			//throw std::logic_error("Attempted to release memory that wasn't allocated.");
		}

		// Deconstruct the objects.
		this->Allocator().DeinitializeRange(this->First(), this->Last());

		// Deallocate memory.
		this->Allocator().Deallocate(this->First());

		// Clear the pointers.
		this->AllocDimensionsPair.Second() = ListDimensions();
	}
	bool IsInside(const_pointer ptr)
	{
		return ptr >= this->First() && ptr < this->Last();
	}
	bool IsInside(const_pointer ptr, const_pointer left, const_pointer right)
	{
		return ptr >= left && ptr < right;
	}
};