#pragma once

//! Represents an object that is used to iterate through a sorted list that maps keys to values.
template<typename ListType>
class SortedListIterator : public IteratorBase
{
friend ListType;
public:
	typedef RandomAccessIteratorTag iterator_category;

	typedef typename ListType::key_type key_type;
	typedef typename ListType::value_type value_type;

	typedef typename ListType::size_type size_type;
	typedef typename ListType::difference_type difference_type;

	typedef Pair<key_type, value_type> key_value_pair;

private:
	int             direction;   //!< Either 1 or -1 that represents direction of iteration.
	difference_type current;     //!< Zero-based index of the current key/value pair.
	key_value_pair  currentPair; //!< Holds a copy of the key/value pair this iterator is currently at.

	const ListType *GetList() const
	{
		return static_cast<const ListType *>(this->GetCollection());
	}

public:
	//! Gets the key of the current key-value pair.
	//!
	//! @returns Reference to the copy of the key that, unless its shallow and mutable, cannot be used to modify
	//!          the list.
	const key_type &Key() const
	{
		this->AssertPositionAccessibility();
		this->currentPair.Value1 = this->GetKey();
		return this->currentPair.Value1;
	}
	//! Gets the value of the current key-value pair.
	//!
	//! @returns Reference to the copy of the value that, unless its shallow and mutable, cannot be used to
	//!          modify the list.
	value_type &Value() const
	{
		this->AssertPositionAccessibility();
		this->currentPair.Value2 = this->GetValue();
		return this->currentPair.Value2;
	}

	//! Creates an orphan iterator.
	SortedListIterator() : direction(1), current(-1)
	{
	}
	//! Creates a new iterator for the list that is initialized to be at the specified position.
	SortedListIterator(difference_type index, const CollectionBase *list, int direction = 1)
		: direction(direction), current(index)
	{
		this->BecomeAdopted(list);
	}
	~SortedListIterator()
	{
		this->BecomeOrphaned();
	}

	//
	// Validity check.
	//

	//! Indicates whether this iterator is valid.
	operator bool() const
	{
		auto list = this->GetList();
		return list && this->current >= 0 && this->current < list->Length();
	}

	//
	// Element access operators.
	//

	//! Gets the reference to the copy of the element of the list this iterator is currently at.
	//!
	//! @returns A reference to the copy of the key/value pair that, unless its shallow and mutable, cannot be
	//!          used to modify the element in the list.
	Pair<key_type, value_type> &operator *()
	{
		this->AssertPositionAccessibility();
		this->currentPair.Value1 = this->GetKey();
		this->currentPair.Value2 = this->GetValue();
		return this->currentPair;
	}

	//
	// Movement operators.
	//

	//! Advances this iterator to the next element in the list. This is a pre-increment operator.
	SortedListIterator &operator++()
	{
		this->Move(1);

		return *this;
	}
	//! Advances this iterator to the next element in the list. This is a post-increment operator.
	SortedListIterator operator++(int)
	{
		SortedListIterator temp = *this;
		++*this;
		return temp;
	}
	//! Retracts this iterator to the previous element in the list. This is a pre-decrement operator.
	SortedListIterator &operator--()
	{
		this->Move(-1);

		return *this;
	}
	//! Retracts this iterator to the previous element in the list. This is a post-decrement operator.
	SortedListIterator operator--(int)
	{
		SortedListIterator temp = *this;
		--*this;
		return temp;
	}

	//! Advances this iterator by specified number of elements.
	SortedListIterator &operator+=(difference_type delta)
	{
		this->Move(delta);

		return *this;
	}
	//! Creates a new iterator that is this iterator but advanced by specified number of elements.
	SortedListIterator operator+(difference_type delta) const
	{
		SortedListIterator result = *this;

		return result += delta;
	}
	//! Retracts this iterator by specified number of elements.
	SortedListIterator &operator-=(difference_type delta)
	{
		return *this += -delta;
	}
	//! Creates a new iterator that is this iterator but retracted by specified number of elements.
	SortedListIterator operator-(difference_type delta) const
	{
		return *this + -delta;
	}

	//! Gets the difference between this and other iterators.
	difference_type operator-(const SortedListIterator &other) const
	{
		this->AssertCompatibility(other);
		return this->current - other.current;
	}

	//
	// Comparison operators.
	//

	//! Determines whether 2 iterators are on the same position of the list.
	bool operator== (const SortedListIterator &other) const
	{
		this->AssertCompatibility(other);
		return this->current == other.current;
	}
	//! Determines whether 2 iterators are not on the same position of the list.
	bool operator!= (const SortedListIterator &other) const
	{
		return !(*this == other);
	}
	//! Determines whether this iterator is at the position that is after the other iterator.
	bool operator >(const SortedListIterator &other) const
	{
		this->AssertCompatibility(other);
		return this->current > other.current;
	}
	//! Determines whether this iterator is at the position that is before the other iterator.
	bool operator <(const SortedListIterator &other) const
	{
		this->AssertCompatibility(other);
		return this->current < other.current;
	}
	//! Determines whether this iterator is at the position that is after the other iterator or is on the same
	//! one.
	bool operator >=(const SortedListIterator &other) const
	{
		this->AssertCompatibility(other);
		return this->current >= other.current;
	}
	//! Determines whether this iterator is at the position that is before the other iterator or is on the same
	//! one.
	bool operator <=(const SortedListIterator &other) const
	{
		this->AssertCompatibility(other);
		return this->current <= other.current;
	}
private:
	const key_type &GetKey() const
	{
		return this->GetList()->keys[this->current];
	}
	const value_type &GetValue() const
	{
		return this->GetList()->values[this->current];
	}
	void AssertPositionAccessibility() const
	{
#ifdef DEBUG_ITERATION
		auto list = this->GetList();
		if (!list)
		{
			throw std::logic_error("The iterator cannot access its current position: no connection to the list.");
		}
		if (this->current < 0 || this->current >= difference_type(list->Length()))
		{
			throw std::out_of_range("The iterator cannot access its current position: it's out of range.");
		}
#endif // DEBUG_ITERATION
	}
	void AssertCompatibility(const SortedListIterator &
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
			throw std::invalid_argument("This iterator is not compatible with the other: its GetList() is different.");
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
		auto list = this->GetList();
		if (!list)
		{
			throw std::logic_error("The iterator cannot be moved: no connection to the list.");
		}
#endif // DEBUG_ITERATION
		this->current += delta;
	}
};