#pragma once

//! Represents an object that encapsulates memory management strategy. It is used to allocate, deallocate,
//! construct and destruct objects.
//!
//! @tparam ObjectType Type of objects to work with.
template<typename ObjectType>
class DefaultAllocator
{
public:
	typedef ObjectType value_type;
	typedef ObjectType &reference;
	typedef const ObjectType &const_reference;
	typedef ObjectType &&right_reference;
	typedef const ObjectType &&const_right_reference;

	typedef ObjectType *pointer;
	typedef const ObjectType *const_pointer;

	typedef size_t size_type;
	typedef ptrdiff_t diff_type;

	//! Special type that is used when creating allocators for other types of objects through the allocators of
	//! this type.
	//!
	//! Example:
	//!
	//! // Here's a class that uses an allocator
	//! template<typename ElemType, typename AllocType = DefaultAllocator<ElemType>>
	//! class Container
	//! {
	//!     AllocType alloc;						// Our allocator object.
	//!     AssistantObject *assistantObject;		// Pointer to assistant object.
	//!     void Foo()
	//!     {
	//!         // Let's say this class needs to allocates some assistant data. It's best to use the same
	//!         // allocator for consistency, however, allocator we have only works for allocation of
	//!         // objects of type ElemType, so we need a way to get a different instantiation of the same
	//!         // allocator template. This is where rebind comes in:
	//!
	//!         typename AllocType::template rebind<AssistantObject>::other assistantAllocator(this->alloc);
	//!
	//!         this->assistantObject = assistantAllocator.Allocate(1);
	//!         assistantAllocator.Initialize(this->assistantObject, AssistantObject());
	//!     }
	//! };
	template<typename OtherObjectType>
	struct rebind
	{
		typedef DefaultAllocator<OtherObjectType> other;
	};

	DefaultAllocator() noexcept
	{
	}
	DefaultAllocator(const DefaultAllocator<value_type> &) noexcept
	{
	}
	template<typename OtherObjectType>
	DefaultAllocator(const DefaultAllocator<OtherObjectType> &) noexcept
	{
	}
	template<typename OtherObjectType>
	DefaultAllocator<ObjectType> &operator =(const DefaultAllocator<OtherObjectType> &)
	{
		return *this;
	}

	//! Finds address of the mutable object that is referenced by the given value.
	pointer Address(reference value) const
	{
		return std::addressof(value);
	}
	//! Finds address of the immutable object that is referenced by the given value.
	const_pointer Address(const_reference value) const
	{
		return std::addressof(value);
	}
	//! Allocates enough memory to fit specified number of objects. No object is initialized by this function.
	//!
	//! @param count Number of objects to allocate memory for.
	//!
	//! @returns A pointer to the first uninitialized object in the allocated memory block.
	pointer Allocate(size_type count) const
	{
		return static_cast<pointer>(::operator new (count * sizeof(value_type)));
	}
	//! Allocates enough memory to fit specified number of objects. No object is initialized by this function.
	//!
	//! @param count Number of objects to allocate memory for.
	//! @param hint  Ignored.
	//!
	//! @returns A pointer to the first uninitialized object in the allocated memory block.
	pointer Allocate(size_type count, const void *) const
	{
		return static_cast<pointer>(::operator new (count * sizeof(value_type)));
	}
	//! Deallocates memory that was previously allocated by this object.
	//!
	//! @param ptr A pointer that was previously returned by the call to one of the overloads of Allocate
	//!            function.
	void Deallocate(pointer ptr) const
	{
		::operator delete(ptr);
	}
	//! Initializes the object by calling its constructor.
	//!
	//! @tparam ConstructionType Type which constructor to invoke.
	//! @tparam InitArgTypes     Set of types of arguments that are accepted by the constructor.
	//!
	//! @param value Pointer the memory that needs initialization.
	//! @param args  A set of arguments to pass to the constructor.
	template<typename ConstructionType, typename... InitArgTypes>
	void Initialize(ConstructionType *value, InitArgTypes && ... args) const
	{
		::new (value) ConstructionType(std::forward<InitArgTypes>(args)...);
	}
	//! Initializes a range of objects by calling a constructor for each of them.
	//!
	//! @tparam ConstructionType Type which constructor to invoke.
	//! @tparam InitArgTypes     Set of types of arguments that are accepted by the constructor.
	//!
	//! @param first Pointer to the first element to initialize.
	//! @param last  Pointer to the element after last element to initialize.
	template<typename ConstructionType, typename... InitArgTypes>
	void InitializeRange(ConstructionType *first, ConstructionType *last, InitArgTypes && ... args) const
	{
		for (; first != last; first++)
		{
			this->Initialize(first, std::forward<InitArgTypes>(args)...);
		}
	}
	//! Deinitializes the object by calling its destructor.
	//!
	//! @tparam DestructionType Type which destructor to invoke.
	//!
	//! @param value The object to deinitialize.
	template<typename DestructionType>
	void Deinitialize(DestructionType *value) const
	{
		value->~DestructionType();
	}
	//! Deinitializes a range of objects by calling a destructor for each of them.
	//!
	//! @tparam DestructionType Type which destructor to invoke.
	//!
	//! @param first Pointer to the first element to deinitialize.
	//! @param last  Pointer to the element after last element to deinitialize.
	template<typename DestructionType>
	void DeinitializeRange(DestructionType *first, DestructionType *last) const
	{
		for (; first != last; first++)
		{
			this->Deinitialize(first);
		}
	}
	//! Gets the length of the longest array that can be created by this allocator.
	size_type MaxLength() const noexcept
	{
		return size_type(-1) / sizeof(ObjectType);
	}
};
// Specialization of the allocator type for type void.
template<>
class DefaultAllocator<void>
{
public:
	typedef void value_type;

	typedef void *pointer;
	typedef const void *const_pointer;

	DefaultAllocator() noexcept
	{
	}
	DefaultAllocator(const DefaultAllocator<value_type> &) noexcept
	{
	}
	template<typename OtherObjectType>
	DefaultAllocator(const DefaultAllocator<OtherObjectType> &) noexcept
	{
	}
	template<typename OtherObjectType>
	DefaultAllocator<void> &operator =(const DefaultAllocator<OtherObjectType> &)
	{
		return *this;
	}
};

//! Default implementation of the operator that checks equality of 2 allocators. By default all allocators are
//! equal to each other.
template<typename FirstType, typename SecondType>
inline bool operator ==(DefaultAllocator<FirstType>, DefaultAllocator<SecondType>) noexcept
{
	return true;
}

//! Default implementation of the operator that checks inequality of 2 allocators. By default all allocators are
//! equal to each other.
template<typename FirstType, typename SecondType>
inline bool operator !=(DefaultAllocator<FirstType>, DefaultAllocator<SecondType>) noexcept
{
	return false;
}
