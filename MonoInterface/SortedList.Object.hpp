#pragma once

#include "List.hpp"
#include "SortedList.Iteration.hpp"

//! Represents an object that is encapsulated by objects of type SortedList`5.
template<
	typename KeyType,
	typename ElementType,
	typename AllocatorType  = DefaultAllocator<KeyType>,
	typename ComparatorType = DefaultComparison<KeyType, KeyType> >
class SortedListObject : public CollectionBase
{
// Whoever invented name hiding can go die in hell.
using CollectionBase::InvalidateIterators;

friend SortedListIterator<SortedListObject>;
public:
	typedef KeyType key_type;
	typedef ElementType value_type;
	typedef AllocatorType key_allocator_type;
	typedef typename AllocatorType::template rebind<value_type>::other value_allocator_type;
	typedef List<key_type, key_allocator_type> keys_list_type;
	typedef List<value_type, value_allocator_type> values_list_type;
	typedef ComparatorType comparator_type;
	typedef SortedListIterator<SortedListObject> iterator_type;

	typedef size_t size_type;
	typedef ptrdiff_t difference_type;
private:
	keys_list_type   keys;           //!< List of keys.
	values_list_type values;         //!< List of values.
	comparator_type  comparator;     //!< An object to use to sort the elements.
	size_type        ReferenceCount; //!< Number of live references to this list.
public:

	//! Gets the reference to the object that is used to sort the values.
	const comparator_type &Comparator() const
	{
		return this->comparator;
	}
	comparator_type &Comparator()
	{
		return this->comparator;
	}
	//! Gets the list of keys.
	const keys_list_type &Keys() const
	{
		return this->keys;
	}
	keys_list_type &Keys()
	{
		return this->keys;
	}
	//! Gets the list of values.
	const values_list_type &Values() const
	{
		return this->values;
	}
	values_list_type &Values()
	{
		return this->values;
	}
	size_type Length() const
	{
		return this->keys.Length;
	}
	//! Gets the reference to the object that is used to (de)allocate memory and (de)initialize objects that
	//! represent keys.
	const key_allocator_type &KeyAllocator() const
	{
		const auto &ks = this->keys;
		return ks.Allocator();
	}
	//! Gets the reference to the object that is used to (de)allocate memory and (de)initialize objects that
	//! represent values.
	const value_allocator_type &ValueAllocator() const
	{
		const auto &vs = this->values;
		return vs.Allocator();
	}

	//! Creates a new object.
	//!
	//! @param allocator  An object to use for working with memory.
	//! @param comparator An object to use for sorting the objects.
	SortedListObject(key_allocator_type &&allocator, const comparator_type &comparator)
		: keys(allocator)
		, values(value_allocator_type(allocator))
		, comparator(comparator)
		, ReferenceCount(1)
	{
		this->AllocateIteratorChain();
	}
	//! Creates a new object.
	//!
	//! @param allocator  An object to use for working with memory.
	//! @param comparator An object to use for sorting the objects.
	SortedListObject(key_allocator_type &&allocator, comparator_type &&comparator)
		: keys(allocator)
		, values(value_allocator_type(allocator))
		, comparator(comparator)
		, ReferenceCount(1)
	{
		this->AllocateIteratorChain();
	}
	~SortedListObject()
	{
		this->ReleaseIteratorChain();
	}
	//! Registers a live reference to this object.
	void RegisterReference()
	{
		this->ReferenceCount++;
	}
	//! Informs this object about removal of reference to this object.
	//!
	//! @returns Number of references after decrement.
	size_type UnregisterReference()
	{
		if (--this->ReferenceCount == 0)
		{
			this->InvalidateIterators();
			// Deinitialize this object to force the lists to destroy themselves.
			this->~SortedListObject();
		}

		return this->ReferenceCount;
	}
	//! Adds a new key-value pair to the list.
	template<typename KeyT, typename ValueT>
	void Add(KeyT &&key, ValueT &&value)
	{
		auto index = this->keys.BinarySearch(key, this->comparator);

		if (index >= 0)
		{
			throw std::logic_error("There is already a value with this key.");
		}

		index = ~index;

		this->keys.Insert(index, std::forward<KeyT>(key));
		this->values.Insert(index, std::forward<ValueT>(value));
	}
	//! Adds a new key-value pair to the list or updates a value on existing one.
	template<typename KeyT, typename ValueT>
	bool Update(KeyT &&key, ValueT &&value)
	{
		auto index = this->keys.BinarySearch(key, this->comparator);

		if (index >= 0)
		{
			this->values.Replace(index, std::forward<ValueT>(value));
			return true;
		}

		index = ~index;

		this->keys.Insert(index, std::forward<KeyT>(key));
		this->values.Insert(index, std::forward<ValueT>(value));
		return false;
	}
	//! Attempts to add an object that is constructed out or given objects to this list.
	template<typename KeyT, typename... ArgumentTypes>
	void Make(KeyT &&key, ArgumentTypes && ... arguments)
	{
		auto index = this->keys.BinarySearch(key, this->comparator);

		if (index >= 0)
		{
			throw std::logic_error("There is already a value with this key.");
		}

		index = ~index;

		this->keys.Insert(index, std::forward<KeyT>(key));
		this->values.Emplace(index, std::forward<ArgumentTypes>(arguments) ...);
	}
	//! Adds a new key-value pair where value is constructed from given arguments, or updates the value on
	//! existing one.
	template<typename KeyT, typename... ArgumentTypes>
	bool Restruct(KeyT &&key, ArgumentTypes && ... arguments)
	{
		auto index = this->keys.BinarySearch(key, this->comparator);

		if (index >= 0)
		{
			this->values.Replace(index, std::forward<ArgumentTypes>(arguments) ...);
			return true;
		}

		index = ~index;

		this->keys.Insert(index, std::forward<KeyT>(key));
		this->values.Emplace(index, std::forward<ArgumentTypes>(arguments) ...);
		return false;
	}
	//! Ensures existence of a key-value pair with given key, by adding it with provided value, if it's not in
	//! the list.
	template<typename KeyT, typename ValueT>
	value_type &Ensure(KeyT &&key, ValueT &&value)
	{
		auto index = this->keys.BinarySearch(key, this->comparator);

		if (index < 0)
		{
			index = ~index;

			this->keys.Insert(index, std::forward<KeyT>(key));
			this->values.Insert(index, std::forward<ValueT>(value));
		}

		return this->values[index];
	}
	//! Ensures existence of a key-value pair with given key, by adding it with an object that is constructed
	//! from provided arguments, if it's not in the list.
	template<typename KeyT, typename... ArgumentTypes>
	value_type &Establish(KeyT &&key, ArgumentTypes && ... arguments)
	{
		auto index = this->keys.BinarySearch(key, this->comparator);

		if (index < 0)
		{
			index = ~index;

			this->keys.Insert(index, std::forward<KeyT>(key));
			this->values.Emplace(index, std::forward<ArgumentTypes>(arguments) ...);
		}

		return this->values[index];
	}
	//! Attempts to remove a value that is associated with the key.
	template<typename KeyT>
	bool Remove(KeyT &&key)
	{
		int index = this->keys.BinarySearch(key, this->comparator);
		if (index >= 0)
		{
			this->keys.RemoveAt(index);
			this->values.RemoveAt(index);
			this->InvalidateIterators(index);
			return true;
		}
		return false;
	}
	//! Determines whether there is a key in the collection.
	template<typename KeyT>
	bool Contains(KeyT &&key) const
	{
		return this->keys.BinarySearch(key, this->comparator) >= 0;
	}
	//! Provides read/write access to the value associated with given key.
	template<typename KeyT>
	ElementType &At(KeyT &&key)
	{
		int index = this->keys.BinarySearch(key, this->comparator);
		if (index >= 0)
		{
			return this->values[index];
		}
		throw std::logic_error("Attempt to get a value that has no key associated with it in the collection.");
	}
	//! Provides read-only access to the value associated with given key.
	template<typename KeyT>
	const ElementType &At(KeyT &&key) const
	{
		int index = this->keys.BinarySearch(key, this->comparator);
		if (index >= 0)
		{
			return this->values[index];
		}
		throw std::logic_error("Attempt to get a value that has no key associated with it in the collection.");
	}
	//! Attempts to get the value that is supposed to be associated with the key.
	template<typename KeyT>
	bool TryGet(KeyT &&key, ElementType &returnedValue) const
	{
		int index = this->keys.BinarySearch(key, this->comparator);
		if (index >= 0)
		{
			const key_allocator_type &valueAllocator = this->values.Allocator();
			valueAllocator.Initialize(valueAllocator.Address(returnedValue),
									  std::add_lvalue_reference_t<ElementType>(this->values[index]));
			return true;
		}
		return false;
	}
	//! Attempts to set the value that is supposed to be associated with the key.
	template<typename KeyT, typename ValueT>
	bool TrySet(KeyT &&key, ValueT &&value)
	{
		int index = this->keys.BinarySearch(key, this->comparator);
		if (index >= 0)
		{
			this->values.Replace(index, std::forward<ValueT>(value));
			return true;
		}
		return false;
	}
	//! Assigns contents from another sorted list.
	void Assign(const SortedListObject *other)
	{
		this->keys.Clear();
		this->values.Clear();

		// Transfer the keys and values while using a new sorting comparator.
		key_type   *currentKey   = other->keys.First();
		value_type *currentValue = other->values.First();

		key_type *lastKey = other->keys.Last();

		while (currentKey != lastKey)
		{
			this->Add(*currentKey++, *currentValue++);
		}
	}
	//! Assigns a range of values to this list.
	template<typename IteratorType>
	void Assign(const IteratorType &first, const IteratorType &last)
	{
		this->keys.Clear();
		this->values.Clear();

		for (; first != last; ++first)
		{
			Pair<key_type, value_type> currentPair = *first;
			this->Add(currentPair.Value1, currentPair.Value2);
		}
	}
private:
	// Invalidates all iterators that are at the index or beyond it.
	void InvalidateIterators(size_type
#ifdef DEBUG_ITERATION
							 index
#endif // DEBUG_ITERATION
							 )
	{
#ifdef DEBUG_ITERATION
		iterator_type **next = static_cast<iterator_type **>(this->GetIteratorsChain());
		if (!*next)
		{
			return;
		}

		// Orphan iterators within the range.
		while (*next)
		{
			auto current = (*next)->current;
			if (current >= index && current <= this->keys.Length)
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
#endif  // DEBUG_ITERATION
	}
	void AllocateIteratorChain()
	{
		typename key_allocator_type::template rebind<CollectionIterators>::other chainAllocator(this->KeyAllocator());
		this->iterators = chainAllocator.Allocate(1);
		chainAllocator.Initialize(this->iterators, CollectionIterators());
		this->iterators->collection = this;
	}
	void ReleaseIteratorChain()
	{
		typename key_allocator_type::template rebind<CollectionIterators>::other chainAllocator(this->KeyAllocator());
		this->InvalidateIterators();
		chainAllocator.Deinitialize(this->iterators);
		chainAllocator.Deallocate(this->iterators);
		this->iterators = nullptr;
	}
};