#pragma once

#include "SortedList.Object.hpp"

//! Represents a sorted collection of key-value pairs.
//!
//! This implementation doesn't permit having multiple values with the same key.
//!
//! @tparam KeyType        Type of keys. Needs to overload comparison operators.
//! @tparam ElementType    Type of elements.
//! @tparam AllocatorType  Type of objects to use to (de)allocate and (de)initialize objects in the list.
//!                        This must be a template type with ability to "rebind" to a different type of objects.
//!                        Look at the implementation of DefaultAllocator`1 for details.
//! @tparam ComparatorType Type of objects to use as a comparator for sorting.
//! @tparam Ascending      Indicates whether the collection must be processed in ascending order.
template<
	typename KeyType,
	typename ElementType,
	typename AllocatorType = DefaultAllocator<KeyType>,
	typename ComparatorType = DefaultComparison<KeyType, KeyType>>
class SortedList
{
public:
	typedef KeyType key_type;
	typedef ElementType value_type;
	typedef AllocatorType key_allocator_type;
	typedef typename AllocatorType::template rebind<value_type>::other value_allocator_type;
	typedef ComparatorType comparator_type;
	typedef SortedListObject<key_type, value_type, AllocatorType, comparator_type> list_object_type;
	typedef typename AllocatorType::template rebind<list_object_type>::other list_object_allocator_type;
	typedef typename list_object_type::iterator_type iterator_type;

	typedef size_t size_type;
	typedef ptrdiff_t difference_type;

private:
	list_object_type *list;

public:
	//! Gets the reference to the object that is used to (de)allocate memory and (de)initialize objects that
	//! represent keys.
	const key_allocator_type &KeyAllocator() const
	{
		return this->list->KeyAllocator();
	}
	//! Gets the reference to the object that is used to (de)allocate memory and (de)initialize objects that
	//! represent values.
	const value_allocator_type &ValueAllocator() const
	{
		return this->list->ValueAllocator();
	}

	//! Creates a default list.
	SortedList()
	{
		this->CreateObject(key_allocator_type(), comparator_type());
	}
	//! Creates a shallow copy of the list.
	//!
	//! @param other Reference to the list to copy the contents from.
	SortedList(const SortedList &other)
		: list(other.list)
	{
		this->list->RegisterReference();
	}
	//! Creates a deep copy of the list.
	//!
	//! @param other Reference to the list to copy the contents from.
	SortedList(const SortedList &other, const key_allocator_type &allocator,
			   const comparator_type &comparator = comparator_type())
	{
		this->CreateObject(allocator, comparator);

		this->list->Assign(other.list);
	}
	//! Assigns contents of the temporary object to the new one.
	//!
	//! @param other Reference to another temporary sorted list to move into the new one.
	SortedList(SortedList &&other)
		: list(other.list)
	{
		other.list = nullptr;
	}
	~SortedList()
	{
		this->ReleaseObject();
	}

	//! Assigns a shallow copy of another list.
	SortedList &operator =(const SortedList &other)
	{
		this->ReleaseObject();

		this->list = other.list;
		if (this->list)
		{
			this->list->RegisterReference();
		}

		return *this;
	}
	//! Assigns a shallow copy of another list.
	SortedList &operator =(SortedList &&other)
	{
		this->ReleaseObject();

		this->list = other.list;
		other.list = nullptr;

		return *this;
	}
	//! Assigns a brace initialization list to this object.
	SortedList &operator =(std::initializer_list<Pair<key_type, value_type>> values)
	{
		this->list->Assign(values.begin(), values.end());
		return *this;
	}

	//! Determines whether there is a key in the collection.
	//!
	//! @tparam KeyT Type of reference to the key.
	template<typename KeyT>
	bool Contains(KeyT &&key) const
	{
		return this->list->Contains(std::forward<KeyT>(key));
	}
	//! Adds a key-value pair to this list.
	//!
	//! @tparam KeyT   Type of reference to the key.
	//! @tparam ValueT Type of reference to the value.
	//!
	//! @param key     An object that represents the key that will be used to access the value later.
	//! @param element An object that represents the value that will be accessed by the key.
	template<typename KeyT, typename ValueT>
	void Add(KeyT &&key, ValueT &&element)
	{
		this->list->Add(std::forward<KeyT>(key), std::forward<ValueT>(element));
	}
	//! Adds a new key-value pair to the list or updates a value on existing one.
	//!
	//! @tparam KeyT   Type of reference to the key.
	//! @tparam ValueT Type of reference to the value.
	//!
	//! @param key     An object that represents the key that will be used to access the value later.
	//! @param element An object that represents the value that will be accessed by the key.
	//!
	//! @returns A boolean value that indicates whether a value was updated, rather then inserted.
	template<typename KeyT, typename ValueT>
	bool Update(KeyT &&key, ValueT &&value)
	{
		return this->list->Update(std::forward<KeyT>(key), std::forward<ValueT>(value));
	}
	//! Attempts to add an object that is constructed out or given objects to this list.
	//!
	//! @tparam KeyT          Type of reference to the key.
	//! @tparam ArgumentTypes Types of arguments that are used to construct the new value.
	//!
	//! @param key       An object that represents the key that will be used to access the value later.
	//! @param arguments A sequence of arguments to pass to the constructor of the value.
	template<typename KeyT, typename... ArgumentTypes>
	void Make(KeyT &&key, ArgumentTypes &&... arguments)
	{
		this->list->Make(std::forward<KeyT>(key), std::forward<ArgumentTypes>(arguments)...);
	}
	//! Adds a new key-value pair where value is constructed from given arguments, or updates the value on
	//! existing one.
	//!
	//! @tparam KeyT          Type of reference to the key.
	//! @tparam ArgumentTypes Types of arguments that are used to construct the new value.
	//!
	//! @param key       An object that represents the key that will be used to access the value later.
	//! @param arguments A sequence of arguments to pass to the constructor of the value.
	//!
	//! @returns A boolean value that indicates whether a value was updated, rather then inserted.
	template<typename KeyT, typename... ArgumentTypes>
	bool Restruct(KeyT &&key, ArgumentTypes &&... arguments)
	{
		return this->list->Restruct(std::forward<KeyT>(key), std::forward<ArgumentTypes>(arguments)...);
	}
	//! Ensures existence of a key-value pair with given key, by adding it with provided value, if it's not in
	//! the list.
	//!
	//! @tparam KeyT   Type of reference to the key.
	//! @tparam ValueT Type of reference to the value.
	//!
	//! @param key     An object that represents the key that will be used to access the value later.
	//! @param element An object that represents the value that will be accessed by the key.
	//!
	//! @returns Reference to the value that is associated with the given key.
	template<typename KeyT, typename ValueT>
	value_type &Ensure(KeyT &&key, ValueT &&value)
	{
		return this->list->Ensure(std::forward<KeyT>(key), std::forward<ValueT>(value));
	}
	//! Ensures existence of a key-value pair with given key, by adding it with an object that is constructed
	//! from provided arguments, if it's not in the list.
	//!
	//! @tparam KeyT          Type of reference to the key.
	//! @tparam ArgumentTypes Types of arguments that are used to construct the new value.
	//!
	//! @param key       An object that represents the key that will be used to access the value later.
	//! @param arguments A sequence of arguments to pass to the constructor of the value.
	//!
	//! @returns Reference to the value that is associated with the given key.
	template<typename KeyT, typename... ArgumentTypes>
	value_type &Establish(KeyT &&key, ArgumentTypes && ... arguments)
	{
		return this->list->Establish(std::forward<KeyT>(key), std::forward<ArgumentTypes>(arguments)...);
	}
	//! Removes a value mapped to a given key, and indicates removal.
	//!
	//! @tparam KeyT Type of reference to the key.
	//!
	//! @param key An object that represents the key that is associated with the value that needs removal.
	template<typename KeyT>
	bool Remove(KeyT &&key)
	{
		return this->list->Remove(std::forward<KeyT>(key));
	}
	//! Provides read/write access to the value associated with given key.
	//!
	//! An std::logic_error exception is thrown, if a value with provided key is not found.
	//!
	//! @tparam KeyT Type of reference to the key.
	//!
	//! @param key An object that represents the key that is associated with the value that needs access.
	template<typename KeyT>
	ElementType &operator[](KeyT &&key)
	{
		return this->list->At(std::forward<KeyT>(key));
	}
	//! Provides read-only access to the value associated with given key.
	//!
	//! An std::logic_error exception is thrown, if a value with provided key is not found.
	//!
	//! @tparam KeyT Type of reference to the key.
	//!
	//! @param key An object that represents the key that is associated with the value that needs access.
	template<typename KeyT>
	const ElementType &operator[](KeyT &&key) const
	{
		return this->list->At(std::forward<KeyT>(key));
	}
	//! Attempts to get the value that is supposed to be associated with the key.
	//!
	//! @tparam KeyT Type of reference to the key.
	//!
	//! @param key           An object that represents the key that is associated with the value that needs
	//!                      access.
	//! @param returnedValue Reference to the object that will contain a copy of the retrieved value if this
	//!                      method returns true.
	//!
	//! @returns A boolean value that indicates whether a value was retrieved successfully.
	template<typename KeyT>
	bool TryGet(KeyT &&key, ElementType &returnedValue) const
	{
		return this->list->TryGet(std::forward<KeyT>(key), returnedValue);
	}
	//! Attempts to set the value that is supposed to be associated with the key.
	//!
	//! @tparam KeyT   Type of reference to the key.
	//! @tparam ValueT Type of reference to the value.
	//!
	//! @param key           An object that represents the key that is associated with the value that needs
	//!                      access.
	//! @param returnedValue Reference to the object that will replace existing value if this method returns
	//!                      true.
	//!
	//! @returns A boolean value that indicates whether a value was modified successfully.
	template<typename KeyT, typename ValueT>
	bool TrySet(KeyT &&key, ValueT &&value)
	{
		return this->list->TrySet(std::forward<KeyT>(key), std::forward<ValueT>(value));
	}
	//! Gets the iterator that can be used to start iteration through this sorted list in ascending order.
	iterator_type begin() const
	{
		return this->ascend();
	}
	//! Gets the iterator that represents the end of iteration through this sorted list in ascending order.
	iterator_type end() const
	{
		return this->top();
	}
	//! Gets the iterator that can be used to start iteration through this sorted list in ascending order.
	iterator_type ascend() const
	{
		return iterator_type(0, this->list);
	}
	//! Gets the iterator that represents the end of iteration through this sorted list in ascending order.
	iterator_type top() const
	{
		return iterator_type(difference_type(this->Length), this->list);
	}
	//! Gets the iterator that can be used to start iteration through this sorted list in descending order.
	iterator_type descend() const
	{
		return iterator_type(difference_type(this->Length - 1), this->list, -1);
	}
	//! Gets the iterator that represents the end of iteration through this sorted list in descending order.
	iterator_type bottom() const
	{
		return iterator_type(-1, this->list, -1);
	}
	//! Sets capacity of this list to number of valid elements it currently holds.
	void Trim()
	{
		this->list->Keys().Trim();
		this->list->Values().Trim();
	}
	//! Gets number of elements in the collection.
	__declspec(property(get=GetLength)) int Length;
	int GetLength() const
	{
		return this->Keys.Length;
	}

	//! Gets read/only access to the collection of keys.
	__declspec(property(get = GetKeys)) const List<KeyType> &Keys;
	const List<KeyType> &GetKeys() const
	{
		return this->list->Keys();
	}
	//! Gets read/only access to the collection of elements.
	__declspec(property(get = GetElements)) const List<ElementType> &Elements;
	const List<ElementType> &GetElements() const
	{
		return this->list->Values();
	}
private:
	// Creates a list object.
	template<typename AllocType, typename CompType>
	void CreateObject(AllocType &&alloc, CompType &&comparator)
	{
		// Create an allocator object from forwarded reference.
		AllocType allocator(std::forward<AllocType>(alloc));

		// Create an allocator that can allocate objects of type SortedListObject`5.
		list_object_allocator_type objectAllocator(allocator);

		// Allocate memory for the object.
		this->list = objectAllocator.Allocate(1);

		// Initialize the object.
		objectAllocator.Initialize(this->list, std::move(allocator), std::forward<CompType>(comparator));
	}
	// Destroys the list object, if its reference count reaches 0.
	void ReleaseObject()
	{
		if (!this->list || this->list->UnregisterReference())
		{
			return;
		}

		// Delete the object, since there are no live references to it.
		list_object_allocator_type objectAllocator(this->KeyAllocator());

		objectAllocator.Deallocate(this->list);
		this->list = nullptr;
	}
};