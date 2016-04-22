#pragma once

#ifdef _DEBUG
#define DEBUG_ITERATION
#endif // _DEBUG

//
// Defines base types for working with collections.
//

struct IteratorBase;
struct CollectionBase;

//! Represents objects that contain pointers to the beginning of the chain of iterators that are going through
//! the collection. Each iterator keeps the pointer to this object.
struct CollectionIterators
{
	const CollectionBase *collection;
	IteratorBase *firstIterator;

	CollectionIterators() : collection(nullptr), firstIterator(nullptr)
	{
	}
};

//! Base type for objects that contain objects and can be iterated through.
struct CollectionBase
{
#ifdef DEBUG_ITERATION
	//! Pointer to the first iterator_type in the chain of all iterators that work on this list.
	CollectionIterators *iterators;

	//! Creates a default collection.
	CollectionBase()
		: iterators(nullptr)
	{
	}
	//! A copy constructor that doesn't actually copy anything.
	CollectionBase(const CollectionBase &)
		: iterators(nullptr)
	{
	}
	//! Invalidates all iterators.
	~CollectionBase()
	{
		this->InvalidateIterators();
	}

	// ReSharper disable once CppMemberFunctionMayBeConst

	//! Gets the pointer to the start of the iterator chain.
	IteratorBase **GetIteratorsChain()
	{
		if (this->iterators)
		{
			return &this->iterators->firstIterator;
		}
		return nullptr;
	}
#endif // DEBUG_ITERATION

	//! Invalidates all iterators that are currently going through this collection.
	void InvalidateIterators();
	//! Switches iterators with another collection.
	void SwapIterators(CollectionBase &other);
};

//! Base type for all iterators.
struct IteratorBase
{
#ifdef DEBUG_ITERATION
	CollectionIterators *iterators;
	IteratorBase *nextIterator;

	//! Creates a default iterator base object.
	IteratorBase() : iterators(nullptr), nextIterator(nullptr)
	{
	}
	//! Copies another iterator.
	IteratorBase(const IteratorBase &other) : iterators(nullptr), nextIterator(nullptr)
	{
		*this = other;
	}

	//! Assigns another iterator to this one.
	IteratorBase &operator=(const IteratorBase &other)
	{
		if (this->iterators == other.iterators)
		{
		}
		else if (other.iterators)
		{
			this->BecomeAdopted(other.iterators->collection);
		}
		else
		{
			this->BecomeOrphaned();
		}

		return *this;
	}
	~IteratorBase()
	{
		this->BecomeOrphaned();
	}
#endif // DEBUG_ITERATION

	//! Changes a parent of this iterator.
	void BecomeAdopted(const CollectionBase *collection)
	{
#ifdef DEBUG_ITERATION
		if (!collection)
		{
			this->BecomeOrphaned();
		}
		else
		{
			CollectionIterators *iters = collection->iterators;

			if (iters != this->iterators)
			{
				// Change the parent.
				this->BecomeOrphaned();
				this->nextIterator = iters->firstIterator;
				iters->firstIterator = this;
				this->iterators = iters;
			}
		}
#endif // DEBUG_ITERATION
	}
	//! Cuts ties with a parent.
	void BecomeOrphaned()
	{
#ifdef DEBUG_ITERATION
		if (!this->iterators)
		{
			return;
		}

		IteratorBase **next = &this->iterators->firstIterator;
		while (*next != nullptr && *next != this)
		{
			next = &(*next)->nextIterator;
		}

		if (!*next)
		{
			throw std::logic_error("Iterator chain is corrupted.");
		}

		*next = this->nextIterator;
		this->iterators = nullptr;
#endif // DEBUG_ITERATION
	}
	//! Clears the info about the parent without going through the iterator chain.
	void BecomeDisowned()
	{
		this->iterators = nullptr;
	}
	//! Gets the collection we are going through.
	const CollectionBase *GetCollection() const
	{
		return this->iterators ? this->iterators->collection : nullptr;
	}
	//! Gets the pointer to the field that contains the pointer to the next iterator.
	IteratorBase **GetNextIterator()
	{
		return &this->nextIterator;
	}
};

inline void CollectionBase::InvalidateIterators()
{
#ifdef DEBUG_ITERATION
	if (!this->iterators)
	{
		return;
	}

	for (IteratorBase **next = this->GetIteratorsChain(); *next; *next = (*next)->nextIterator)
	{
		(*next)->iterators = nullptr;
	}

	this->iterators->firstIterator = nullptr;
#endif // DEBUG_ITERATION
}

inline void CollectionBase::SwapIterators(CollectionBase &other)
{
#ifdef DEBUG_ITERATION
	CollectionIterators *iters = this->iterators;
	this->iterators = other.iterators;
	other.iterators = iters;

	if (this->iterators)
	{
		this->iterators->collection = this;
	}
	if (other.iterators)
	{
		other.iterators->collection = &other;
	}
#endif // DEBUG_ITERATION
}

//
// Tags.
//

//! A special structure that represents a tag that identifies iterators that can provide read access to the
//! element in the collection via a single pass.
struct InputIteratorTag
{
};
//! A special structure that represents a tag that identifies iterators that can provide read/write access
//! to the element in the collection.
struct MutableItaratorTag
{
};
//! A special structure that represents a tag that identifies iterators that can provide write access
//! to the element in the collection via a single pass.
struct OutputIteratorTag : MutableItaratorTag
{
};
//! A special structure that represents a tag that identifies iterators that can provide read/write access
//! to the element in the collection via multiple passes in single direction.
struct ForwardIteratorTag : InputIteratorTag, MutableItaratorTag
{
};
//! A special structure that represents a tag that identifies iterators that can provide read/write access
//! to the element in the collection via multiple passes in both directions.
struct BidirectionalIteratorTag : ForwardIteratorTag
{
};
//! A special structure that represents a tag that identifies iterators that can provide read/write access
//! to any randomly accessible element in the collection.
struct RandomAccessIteratorTag : ForwardIteratorTag
{
};

//
// Iterator determination.
//

//! Special type that indicates whether _TypeToCheck_ is an iterator type.
//!
//! @tparam TypeToCheck A type to check for being an iterator.
template<typename TypeToCheck>
struct IsIterator
{
	//! Indicates whether _TypeToCheck_ is an iterator type.
	static constexpr bool value = false;
};
//! Specialization that specifies that simple pointers to types are valid iterators.
template<typename ObjectType>
struct IsIterator<ObjectType *>
{
	static constexpr bool value = true;
};

//
// Traits.
//

//! Base class for types that provide information about iterators.
template<typename IteratorType, bool = IsIterator<IteratorType>::value>
struct IteratorTraitsBase
{
	typedef typename IteratorType::iterator_category iterator_category;
	typedef typename IteratorType::value_type value_type;
	typedef typename IteratorType::difference_type difference_type;

	typedef typename IteratorType::pointer pointer;
	typedef typename IteratorType::reference reference;
};
//! Specialization for the case when given type is not a normal iterator in which case all typedefs must be
//! defined by the derived type's specialization.
template<typename IteratorType>
struct IteratorTraitsBase<IteratorType, false>
{
	// No typedefs here, they must be defined by the derived type's specialization.
};

//! Provides information about the iterator type.
template<typename IteratorType>
struct IteratorTraits : public IteratorTraitsBase<IteratorType>
{
	// Inherit base typedefs.
};
//! Specialization that defines traits for non-const pointers.
template<typename ObjectType>
struct IteratorTraits<ObjectType *>
{
	typedef RandomAccessIteratorTag iterator_category;
	typedef ObjectType value_type;
	typedef ptrdiff_t difference_type;

	typedef ObjectType *pointer;
	typedef ObjectType &reference;
};
//! Specialization that defines traits for const pointers.
template<typename ObjectType>
struct IteratorTraits<const ObjectType *>
{
	typedef RandomAccessIteratorTag iterator_category;
	typedef ObjectType value_type;
	typedef ptrdiff_t difference_type;

	typedef const ObjectType *pointer;
	typedef const ObjectType &reference;
};

//! Gets an object that identifies the category the iterator belongs to.
template<typename IteratorType>
inline typename IteratorTraits<IteratorType>::iterator_category IteratorCategory(const IteratorType &)
{
	return typename IteratorTraits<IteratorType>::iterator_category();
}