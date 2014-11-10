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

//! Represents a sorted collection of key-value pairs.
//!
//! @tparam KeyType     Type of keys. Needs to overload comparison operators.
//! @tparam ElementType Type of elements.
//! @tparam Ascending   Indicates whether the collection must be processed in ascending order.
template<typename KeyType,typename ElementType, bool Ascending = true>
class SortedList
{
private:
	List<KeyType> keys;
	List<ElementType> values;
public:
	//! Creates a default list.
	SortedList()
		: keys(10)
		, values(10)
	{
		
	}
	//! Creates a list with specific capacity.
	SortedList(int capacity)
		: keys(capacity)
		, values(capacity)
	{

	}
	~SortedList()
	{
		this->keys.~List();
		this->values.~List();
	}
	//! Determines whether there is a key in the collection.
	bool Contains(KeyType key)
	{
		return this->BinarySearch(key) != -1;
	}
	//! Adds a key-value pair.
	void Add(KeyType key, ElementType element)
	{
		std::cout << key << std::endl;
		int index = this->BinarySearchIndex(key);
		if (index == -1)
		{
			FatalError("Attempt to insert a key that is already in the collection.");
		}
		this->keys.Insert(key, index);
		this->values.Insert(element, index);
	}
	//! Removes a value mapped to a given key, and indicates removal.
	bool Remove(KeyType key)
	{
		int index = this->BinarySearch(key);
		if (index != -1)
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
		if (index != -1)
		{
			return this->values[index];
		}
		FatalError("Attempt to get a value that has no key associated with it in the collection.");
	}
	//! Processes a collection.
	void ForEach(std::function<void(KeyType, ElementType)> processor)
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
	__declspec(property(get=GetLength)) int Length;
	int GetLength()
	{
		return this->keys.Length;
	}
private:
	//! Looks for a position where a value associated with given key can be put.
	int BinarySearchIndex(KeyType key)
	{
		if (this->keys.Length == 0)
		{
			return 0;
		}
		int left = 0;
		int right = this->keys.Length - 1;
		while (left != right)
		{
			int mid = left + (right - left) / 2;
			if (this->keys[mid] < key)
			{
				left = mid + 1;
			}
			else if (this->keys[mid] > key)
			{
				right = mid;
			}
			else
			{
				return -1;
			}
		}
		KeyType lastKey = this->keys[left];

		if (lastKey == key)
		{
			return -1;
		}
		if (lastKey < key)
		{
			return Ascending ? left + 1 : left;
		}
		return Ascending ? left : left + 1;
	}
	int BinarySearch(KeyType key)
	{
		if (this->keys.Length == 0)
		{
			return -1;
		}
		int left = 0;
		int right = this->keys.Length - 1;
		while (left != right)
		{
			int mid = left + (right - left) / 2;
			if (this->keys[mid] < key)
			{
				left = mid + 1;
			}
			else if (this->keys[mid] > key)
			{
				right = mid;
			}
			else
			{
				return mid;
			}
		}
		return -1;
	}
};