#pragma once

#include <stdexcept>
#include "ExtraTypeTraits.h"

//
// Base list iterator.
//

//! Represents an object that can be used to more or less safely iterate through the list in read-only mode.
template<typename ListType>
class ListIteratorConst : public IteratorBase
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
	int direction;		//!< Either 1 or -1 that represents direction of iteration.
	pointer current;	//!< Pointer to the value in the list this iterator is currently on.

	const ListType *GetList() const
	{
		return static_cast<const ListType *>(this->GetCollection());
	}
public:
	//! Creates an orphan iterator.
	ListIteratorConst() : direction(1), current(nullptr)
	{
	}
	//! Creates a new iterator for the list that is initialized to be at the specified position.
	ListIteratorConst(pointer element, const CollectionBase *list, int direction = 1)
		: direction(direction), current(element)
	{
		this->BecomeAdopted(list);
	}
	~ListIteratorConst()
	{
		this->BecomeOrphaned();
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
		if (!this->GetList())
		{
			throw std::logic_error("The iterator cannot be dereferenced: no connection to the parent.");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be dereferenced: position is not set.");
		}
		if (this->current < this->GetList()->First() || this->current >= this->GetList()->Last())
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
	// Validity check.
	//

	//! Indicates whether this iterator is valid.
	operator bool() const
	{
		auto list = this->GetList();
		return list && this->current && this->current >= list->First() && this->current < list->Last();
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
		if (!this->GetList())
		{
			throw std::logic_error("The iterator cannot be dereferenced: no connection to the parent.");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be dereferenced: position is not set.");
		}
		if (this->current < this->GetList()->First() || this->current >= this->GetList()->Last())
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
		if (!this->GetList())
		{
			throw std::logic_error("This iterator is not compatible with the other: it's an orphan.");
		}
		if (this->GetList() != other.GetList())
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
		if (!this->GetList())
		{
			throw std::logic_error("The iterator cannot be moved: no connection to the GetList().");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be moved: position is not set.");
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

	//! Creates a new iterator for the list that is initialized to be at the specified position.
	ListIterator(pointer element, const CollectionBase *list, int direction = 1)
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
		if (!this->GetList())
		{
			throw std::logic_error("The iterator cannot be dereferenced: no connection to the parent.");
		}
		if (!this->current)
		{
			throw std::logic_error("The iterator cannot be dereferenced: position is not set.");
		}
		if (this->current < this->GetList()->First() || this->current >= this->GetList()->Last())
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