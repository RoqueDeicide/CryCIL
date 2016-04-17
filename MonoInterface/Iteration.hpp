#pragma once

#include <stdexcept>
#include "ExtraTypeTraits.h"

#ifdef _DEBUG
#define DEBUG_ITERATION
#endif // _DEBUG

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
// Base list iterator.
//

//! Represents an object that can be used to more or less safely iterate through the list in read-only mode.
template<typename ListType>
class ListIteratorConst
{
	friend ListType;
public:
	typedef RandomAccessIteratorTag iterator_category;
	typedef typename ListType::value_type value_type;
	typedef typename ListType::const_pointer pointer;
	typedef typename ListType::const_pointer const_pointer;
	typedef typename ListType::reference reference;
	typedef typename ListType::const_reference const_reference;

	typedef typename ListType::size_type size_type;
	typedef typename ListType::difference_type difference_type;

protected:
	ListType *parent;			//!< The list this iterator is going through.
	ListIteratorConst *next;	//!< Next iterator in the chain of iterators that share the same parent.
	int direction;				//!< Either 1 or -1 that represents direction of iteration.
	pointer current;			//!< Pointer to the value in the list this iterator is currently on.

public:
	//! Creates an orphan iterator.
	ListIteratorConst() : parent(nullptr), next(nullptr), direction(1), current(nullptr)
	{
	}
	//! Copies another iterator.
	ListIteratorConst(const ListIteratorConst &other)
		: ListIteratorConst()
	{
		*this = other;
	}
	//! Creates a new iterator for the list that is initialized to be at the specified position.
	ListIteratorConst(pointer element, const ListType *list, int direction = 1)
		: parent(nullptr), next(nullptr), direction(direction), current(element)
	{
		this->BecomeAdopted(const_cast<ListType *>(list));
	}
	~ListIteratorConst()
	{
		this->BecomeOrphaned();
	}
	//! Assigns a copy of another iterator to this one.
	ListIteratorConst& operator=(const ListIteratorConst &other)
	{
		if (this->parent == other.parent)
		{
			// Don't do anything since both iterators are adopted by the same list.
		}
		else if (other.parent != nullptr)
		{
			// Switch this iterator to the new parent.
			this->BecomeAdopted(other.parent);
		}
		else
		{
#ifdef DEBUG_ITERATION
			// Just become an orphan.
			this->BecomeOrphaned();
#endif // DEBUG_ITERATION
		}
		this->current = other.current;
		this->direction = other.direction;
		return (*this);
	}

	//! Changes a parent of this iterator.
	void BecomeAdopted(ListType *list)
	{
		if (this->parent != list)
		{
			// Cut the ties to the existing parent.
			this->BecomeOrphaned();
		}

		if (!list || this->parent == list)
		{
			// Either we are not supposed to get adopted, or we are already adopted by that parent.
			return;
		}

#ifdef DEBUG_ITERATION
		// Let the parent know that we are adopted by inserting ourselves at the start of the chain of iterators.
		this->next = list->FirstIterator;
		list->FirstIterator = this;
#endif // DEBUG_ITERATION

		this->parent = list;
	}
	//! Cuts ties with a parent.
	void BecomeOrphaned()
	{
#ifdef DEBUG_ITERATION
		if (!this->parent)
		{
			return;
		}

		// Find the address of a field that points at this iterator.
		ListIteratorConst **next = &this->parent->FirstIterator;
		while (*next != nullptr && *next != this)
		{
			next = &(*next)->next;
		}

		if (*next == nullptr)
		{
			throw std::logic_error("Attempt was made to orphan the iterator which wasn't in the chain.");
		}

		*next = this->next;
		this->parent = nullptr;
#endif // DEBUG_ITERATION
	}
	//! Clears the info about the parent without going through the iterator chain.
	void BecomeDisowned()
	{
		this->parent = nullptr;
	}

	//
	// Optimization functions.
	//
	
	//! Gets the pointer to the current element of the list this iterator is currently at, allowing unsafe but
	//! fast access to the list.
	pointer GetUnchecked() const
	{
#ifdef DEBUG_ITERATION
		// Check the iterator for validity.
		if (!this->parent)
		{
			throw std::logic_error("The iterator cannot be dereferenced: no connection to the parent.");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be dereferenced: position is not set.");
		}
		if (this->current < this->parent->First() || this->current >= this->parent->Last())
		{
			throw std::out_of_range("The iterator cannot be dereferenced: current position is outside the list.");
		}
#endif // DEBUG_ITERATION

		return this->current;
	}
	//! Sets position of this iterator to the specified one.
	//!
	//! This method is used to update the iterator after its position was changed after it was acquired via
	//! @see GetUnchecked() method.
	void SetRechecked(pointer position)
	{
		this->current = position;
	}

	//
	// Element access operators.
	//

	//! Gets the reference to the element of the list this iterator is currently at.
	reference operator *() const
	{
		return *this->operator->();
	}
	//! Gets the pointer to the current element in the list.
	pointer operator->() const
	{
#ifdef DEBUG_ITERATION
		// Check the iterator for validity.
		if (!this->parent)
		{
			throw std::logic_error("The iterator cannot be dereferenced: no connection to the parent.");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be dereferenced: position is not set.");
		}
		if (this->current < this->parent->First() || this->current >= this->parent->Last())
		{
			throw std::out_of_range("The iterator cannot be dereferenced: current position is outside the list.");
		}
#endif // DEBUG_ITERATION

		return this->current;
	}

	//
	// Movement operators.
	//

	//! Advances this iterator to the next element in the list. This is a pre-increment operator.
	ListIteratorConst &operator++()
	{
		this->Move(1);

		return *this;
	}
	//! Advances this iterator to the next element in the list. This is a post-increment operator.
	ListIteratorConst operator++(int)
	{
		ListIteratorConst temp = *this;
		++*this;
		return temp;
	}
	//! Retracts this iterator to the previous element in the list. This is a pre-decrement operator.
	ListIteratorConst &operator--()
	{
		this->Move(-1);

		return *this;
	}
	//! Retracts this iterator to the previous element in the list. This is a post-decrement operator.
	ListIteratorConst operator--(int)
	{
		ListIteratorConst temp = *this;
		--*this;
		return temp;
	}

	//! Advances this iterator by specified number of elements.
	ListIteratorConst &operator+=(difference_type delta)
	{
		this->Move(delta);

		return *this;
	}
	//! Creates a new iterator that is this iterator but advanced by specified number of elements.
	ListIteratorConst operator+(difference_type delta) const
	{
		ListIteratorConst result = *this;

		return result += delta;
	}
	//! Retracts this iterator by specified number of elements.
	ListIteratorConst &operator-=(difference_type delta)
	{
		return *this += -delta;
	}
	//! Creates a new iterator that is this iterator but retracted by specified number of elements.
	ListIteratorConst operator-(difference_type delta) const
	{
		return *this + -delta;
	}

	//! Gets the difference between this and other iterators.
	difference_type operator-(const ListIteratorConst &other) const
	{
		this->AssertCompatibility(other);
		return this->current - other.current;
	}

	//! Gets the reference to the element of the list that is specified number of elements away from this
	//! iterator.
	reference operator[](difference_type delta) const
	{
		return *(*this + delta);
	}

	//
	// Comparison operators.
	//

	//! Determines whether 2 iterators are on the same position of the list.
	bool operator== (const ListIteratorConst &other) const
	{
		this->AssertCompatibility(other);
		return this->current == other.current;
	}
	//! Determines whether 2 iterators are not on the same position of the list.
	bool operator!= (const ListIteratorConst &other) const
	{
		return !(*this == other);
	}
	//! Determines whether this iterator is at the position that is after the other iterator.
	bool operator >(const ListIteratorConst &other) const
	{
		this->AssertCompatibility(other);
		return this->current > other.current;
	}
	//! Determines whether this iterator is at the position that is before the other iterator.
	bool operator <(const ListIteratorConst &other) const
	{
		this->AssertCompatibility(other);
		return this->current < other.current;
	}
	//! Determines whether this iterator is at the position that is after the other iterator or is on the same
	//! one.
	bool operator >=(const ListIteratorConst &other) const
	{
		this->AssertCompatibility(other);
		return this->current >= other.current;
	}
	//! Determines whether this iterator is at the position that is before the other iterator or is on the same
	//! one.
	bool operator <=(const ListIteratorConst &other) const
	{
		this->AssertCompatibility(other);
		return this->current <= other.current;
	}
private:
	void AssertCompatibility(const ListIteratorConst &
#ifdef DEBUG_ITERATION
							 other
#endif
							 ) const
	{
#ifdef DEBUG_ITERATION
		if (!this->parent)
		{
			throw std::logic_error("This iterator is not compatible with the other: it's an orphan.");
		}
		if (this->parent != other.parent)
		{
			throw std::invalid_argument("This iterator is not compatible with the other: its parent is different.");
		}
		if (this->direction != other.direction)
		{
			throw std::logic_error("This iterator is not compatible with the other: They are going in reverse directions.");
		}
#endif // DEBUG_ITERATION
	}
	void Move(difference_type delta)
	{
		delta *= direction;
#ifdef DEBUG_ITERATION
		// Check the iterator for validity.
		if (!this->parent)
		{
			throw std::logic_error("The iterator cannot be moved: no connection to the parent.");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be moved: position is not set.");
		}
		if (this->current + delta - 1 < this->parent->First() ||
			this->current + delta - 1 >= this->parent->Last())
		{
			throw std::out_of_range("The iterator cannot be moved: new position is outside of the list.");
		}
#endif // DEBUG_ITERATION
		this->current += delta;
	}
};

//! Represents an iterator that can be used for read/write access to the list's contents.
template<typename ListType>
class ListIterator : public ListIteratorConst<ListType>
{
public:
	typedef ListIteratorConst<ListType> BaseType;
	typedef typename ListType::value_type value_type;
	typedef typename ListType::pointer pointer;
	typedef typename ListType::const_pointer const_pointer;
	typedef typename ListType::reference reference;
	typedef typename ListType::const_reference const_reference;

	typedef typename ListType::size_type size_type;
	typedef typename ListType::difference_type difference_type;

	//! Copies another iterator.
	ListIterator(const ListIterator &other)
		: ListIteratorConst()
	{
		static_cast<ListIteratorConst<ListType> *>(this)->operator=(other);
	}
	//! Creates a new iterator for the list that is initialized to be at the specified position.
	ListIterator(pointer element, const ListType *list, int direction = 1)
		: ListIteratorConst(element, list, direction)
	{
	}

	//
	// Optimization functions.
	//

	//! Gets the pointer to the current element of the list this iterator is currently at, allowing unsafe but
	//! fast access to the list.
	pointer GetUnchecked() const
	{
#ifdef DEBUG_ITERATION
		// Check the iterator for validity.
		if (!this->parent)
		{
			throw std::logic_error("The iterator cannot be dereferenced: no connection to the parent.");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be dereferenced: position is not set.");
		}
		if (this->current < this->parent->First() || this->current >= this->parent->Last())
		{
			throw std::out_of_range("The iterator cannot be dereferenced: current position is outside the list.");
		}
#endif // DEBUG_ITERATION

		return this->current;
	}
	//! Sets position of this iterator to the specified one.
	//!
	//! This method is used to update the iterator after its position was changed after it was acquired via
	//! @see GetUnchecked() method.
	void SetRechecked(pointer position)
	{
		this->current = position;
	}

	//
	// Element access operators.
	//

	//! Gets the reference to the element of the list this iterator is currently at.
	reference operator *() const
	{
		return static_cast<const BaseType *>(this)->operator *();
	}
	//! Gets the pointer to the current element in the list.
	pointer operator->() const
	{
		return static_cast<const BaseType *>(this)->operator->();
	}

	//
	// Movement operators.
	//

	//! Advances this iterator to the next element in the list. This is a pre-increment operator.
	ListIterator &operator++()
	{
		static_cast<BaseType *>(this)->operator++();
		return *this;
	}
	//! Advances this iterator to the next element in the list. This is a post-increment operator.
	ListIterator operator++(int)
	{
		ListIterator t = *this;
		static_cast<BaseType *>(this)->operator++();
		return t;
	}
	//! Retracts this iterator to the previous element in the list. This is a pre-decrement operator.
	ListIterator &operator--()
	{
		static_cast<BaseType *>(this)->operator--();
		return *this;
	}
	//! Retracts this iterator to the previous element in the list. This is a post-decrement operator.
	ListIterator operator--(int)
	{
		ListIterator t = *this;
		static_cast<BaseType *>(this)->operator--();
		return t;
	}

	//! Advances this iterator by specified number of elements.
	ListIterator &operator+=(difference_type delta)
	{
		static_cast<BaseType *>(this)->operator+=(delta);
		return *this;
	}
	//! Creates a new iterator that is this iterator but advanced by specified number of elements.
	ListIterator operator+(difference_type delta) const
	{
		ListIterator result = *this;

		return result += delta;
	}
	//! Retracts this iterator by specified number of elements.
	ListIterator &operator-=(difference_type delta)
	{
		static_cast<BaseType *>(this)->operator-=(delta);
		return *this;
	}
	//! Creates a new iterator that is this iterator but retracted by specified number of elements.
	ListIterator operator-(difference_type delta) const
	{
		ListIterator result = *this;

		return result -= delta;
	}

	//! Gets the difference between this and other iterators.
	difference_type operator-(const ListIterator &other) const
	{
		this->AssertCompatibility(other);
		return this->current - other.current;
	}

	//! Gets the reference to the element of the list that is specified number of elements away from this
	//! iterator.
	reference operator[](difference_type delta) const
	{
		return static_cast<const BaseType *>(this)->operator [](delta);
	}
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
//! Specializations that specify that objects of type ListIteratorBase are valid iterators.
template<typename ListType>
struct IsIterator<ListIteratorConst<ListType>>
{
	static constexpr bool value = true;
};
template<typename ListType>
struct IsIterator<ListIterator<ListType>>
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