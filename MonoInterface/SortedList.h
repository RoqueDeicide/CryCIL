#pragma once

#include <functional>

#ifdef CRYCIL_MODULE
#include <ISystem.h>

#define FatalError(message) CryFatalError(message)

#else

#include <stdexcept>
#define FatalError(message) throw std::logic_error(message)

#endif // CRYCIL_MODULE

#include "List.h"

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

//! Represents a sorted collection of key-value pairs.
//!
//! @tparam KeyType     Type of keys. Needs to overload comparison operators.
//! @tparam ElementType Type of elements.
//! @tparam Ascending   Indicates whether the collection must be processed in ascending order.
template<typename KeyType,typename ElementType, bool Ascending = true>
class SortedList
{
private:
	List<KeyType>     keys;
	List<ElementType> values;
	std::function<int(KeyType&, KeyType&)> comparer;
public:
	//! Creates a default list.
	SortedList()
		: keys(10)
		, values(10)
		, comparer(nullptr)
	{
		
	}
	//! Assigns contents of the temporary object to the new one.
	SortedList(SortedList<KeyType, ElementType, Ascending> &&other)
		: keys(std::move(other.keys))
		, values(std::move(other.values))
		, comparer(std::move(other.comparer))
	{

	}
	//! Creates a list with specific capacity.
	SortedList(int capacity)
		: keys(capacity)
		, values(capacity)
		, comparer(nullptr)
	{

	}
	//! Creates a list with specific capacity that uses special comparison algorithm.
	SortedList(int capacity, std::function<int(KeyType&, KeyType&)> comparisonFunc)
		: keys(capacity)
		, values(capacity)
		, comparer(comparisonFunc)
	{

	}
	//! Creates a deep copy of the list.
	SortedList(const SortedList<KeyType, ElementType, Ascending> &listToCopy)
		: comparer(listToCopy.comparer)
	{
		this->keys(listToCopy.keys);
		this->values(listToCopy.values);
	}
	~SortedList()
	{
	}
	//! Disposes of this list.
	void Dispose()
	{
		this->~SortedList();
	}
	//! Determines whether there is a key in the collection.
	bool Contains(KeyType key)
	{
		return this->BinarySearch(key) >= 0;
	}
	//! Adds a key-value pair.
	void Add(KeyType key, ElementType element)
	{
		int index = this->BinarySearch(key);
		if (index >= 0)
		{
			FatalError("Attempt to insert a key that is already in the collection.");
		}
		this->keys.Insert(key, ~index);
		this->values.Insert(element, ~index);
	}
	//! Removes a value mapped to a given key, and indicates removal.
	bool Remove(KeyType key)
	{
		int index = this->BinarySearch(key);
		if (index >= 0)
		{
			this->keys.RemoveAt(index);
			this->values.RemoveAt(index);
			return true;
		}
		return false;
	}
	//! Provides read/write access to the value associated with given key.
	ElementType &At(KeyType key)
	{
		int index = this->BinarySearch(key);
		if (index >= 0)
		{
			return this->values[index];
		}
		FatalError("Attempt to get a value that has no key associated with it in the collection.");
		return this->values[0];
	}
	//! Attempts to get the value that is supposed to be associated with the key.
	bool TryGet(KeyType key, ElementType &returnedValue)
	{
		int index = this->BinarySearch(key);
		if (index >= 0)
		{
			returnedValue = this->values[index];
			return true;
		}
		return false;
	}
	//! Attempts to set the value that is supposed to be associated with the key.
	bool TrySet(KeyType key, ElementType &value)
	{
		int index = this->BinarySearch(key);
		if (index >= 0)
		{
			this->values[index] = value;
			return true;
		}
		return false;
	}
	//! Processes a collection.
	void ForEach(std::function<void(KeyType, ElementType&)> processor)
	{
		if (Ascending)
		{
			for (int i = 0; i < this->keys.Length; i++)
			{
				processor(this->keys[i], this->values[i]);
			}
		}
		else
		{
			for (int i = this->keys.Length - 1; i >= 0; i--)
			{
				processor(this->keys[i], this->values[i]);
			}
		}
	}
	//! Sets capacity of this list to number of valid elements it currently holds.
	void Trim()
	{
		this->keys.Trim();
		this->values.Trim();
	}
	//! Gets number of elements in the collection.
	__declspec(property(get=GetLength)) int Length;
	int GetLength()
	{
		return this->keys.Length;
	}

	//! Gets read/only access to the collection of keys.
	__declspec(property(get = GetKeys)) ReadOnlyList<KeyType> *Keys;
	ReadOnlyList<KeyType> *GetKeys()
	{
		return (ReadOnlyList<KeyType> *)&this->keys;
	}
	//! Gets read/only access to the collection of elements.
	__declspec(property(get = GetElements)) ReadOnlyList<ElementType> *Elements;
	ReadOnlyList<ElementType> *GetElements()
	{
		return (ReadOnlyList<ElementType> *)&this->values;
	}
private:
	int BinarySearch(KeyType key)
	{
		int lo = 0;
		int hi = this->Length - 1;
		while (lo <= hi)
		{
			int i = lo + ((hi - lo) >> 1);
			int order =
				(this->comparer)
				? this->comparer(this->keys[i], key)
				: DefaultComparison(this->keys[i], key);

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
};