#pragma once

#include <type_traits>

//! Encapsulates two objects.
template<typename T1, typename T2>
struct Pair
{
	T1 Value1;					//!< First value.
	T2 Value2;					//!< Second value.
	//! Creates default instance of this type.
	Pair()
		: Value1()
		, Value2()
	{
	}
	//! Creates a new instance of this type.
	//!
	//! @param value1 First object.
	//! @param value2 Second object.
	Pair(T1 value1, T2 value2)
	{
		this->Value1 = value1;
		this->Value2 = value2;
	}
};
//! Encapsulates three objects.
template<typename T1, typename T2, typename T3>
struct Triple
{
	T1 Value1;					//!< First value.
	T2 Value2;					//!< Second value.
	T3 Value3;					//!< Third value.
	//! Creates default instance of this type.
	Triple()
		: Value1()
		, Value2()
		, Value3()
	{}
	//! Creates a new instance of this type.
	//!
	//! @param value1 First object.
	//! @param value2 Second object.
	//! @param value3 Third object.
	Triple(T1 value1, T2 value2, T3 value3)
	{
		this->Value1 = value1;
		this->Value2 = value2;
		this->Value3 = value3;
	}
};
//! Encapsulates four objects.
template<typename T1, typename T2, typename T3, typename T4>
struct Quad
{
	T1 Value1;					//!< First value.
	T2 Value2;					//!< Second value.
	T3 Value3;					//!< Third value.
	T4 Value4;					//!< Fourth value.
	//! Creates default instance of this type.
	Quad()
		: Value1()
		, Value2()
		, Value3()
		, Value4()
	{}
	//! Creates a new instance of this type.
	//!
	//! @param value1 First object.
	//! @param value2 Second object.
	//! @param value3 Third object.
	//! @param value4 Fourth object.
	Quad(T1 value1, T2 value2, T3 value3, T4 value4)
	{
		this->Value1 = value1;
		this->Value2 = value2;
		this->Value3 = value3;
		this->Value4 = value4;
	}
};

//! A utility type that is used by UncompressedPair to specify the constructor that performs default
//! initialization of the first object and forwards remaining arguments to the second object's constructor.
struct NoneToFirstRestToSecond
{
};

//! A utility type that is used by UncompressedPair to specify the constructor that performs initializes the
//! first object with the first argument and forwards remaining arguments to the second object's constructor.
struct OneToFirstRestToSecond
{
};

//! Represents an object that uses a nifty trick in C++ to be the most compact object that contains 2 other
//! objects.
//!
//! The core of the trick is that rules for placement and alignment of the base part of the object are different
//! from when the base part is contained within the object.
//!
//! Imagine we have 2 classes: One with no fields and another with 32-bit integer:
//!
//! @code
//!
//! class First {};
//!
//! class Second
//! {
//! public:
//!     int number;
//! };
//!
//! @endcode
//!
//! Now here are 2 classes, both of which contain an instance of each of the above classes.
//!
//! @code
//!
//! class UncompressedPair
//! {
//!     First  first;
//!     Second second;
//! };
//!
//! class CompressedPair : private First
//! {
//!     Second second;
//! };
//!
//! @endcode
//!
//! Both of the above consist of 2 objects: one of type First and one of type Second. The difference between them
//! is the size of the object.
//!
//! @code
//!
//! cout << sizeof(UncompressedPair) << endl;        // Prints 8.
//! cout << sizeof(CompressedPair)   << endl;        // Prints 4.
//!
//! @endcode
//!
//! As you can see from the output of the above code: the compressed pair allows empty components to not take up
//! any space in the memory.
//!
//! @tparam FirstPartType  Type of the first object in the pair. If this type is empty and is not final, then
//!                        this type will derive from it to avoid extra memory consumption, otherwise both
//!                        objects are stored in this one as fields.
//! @tparam SecondPartType Type of the second object in the pair. This object has the most options when it
//!                        comes to initialization.
template<typename FirstPartType, typename SecondPartType,
	bool = std::is_empty<FirstPartType>::value && ! std::is_final<FirstPartType>::value>
class CompressedPair final : private FirstPartType
{
	SecondPartType second;
public:
	//! Creates a new pair by performing default initialization of the first object and forwarding the rest of
	//! the arguments to the constructor of the second object.
	//!
	//! @tparam InitializationArgumentTypes Types of arguments to pass to the constructor of the second object.
	//!
	//! @param      An object that forces compiler to select this overload.
	//! @param args Arguments to pass to the constructor of the second object.
	template<typename... InitializationArgumentTypes>
	CompressedPair(NoneToFirstRestToSecond, InitializationArgumentTypes&&... args)
		: FirstPartType()
		, second(std::forward<InitializationArgumentTypes>(args)...)
	{
	}
	//! Creates a new pair by calling a constructor of the first object and forwarding one argument to it and
	//! forwarding the rest of arguments to the constructor of the second object.
	//!
	//! @tparam InitializationArgumentType  Type of the argument to pass to the constructor of the first object.
	//! @tparam InitializationArgumentTypes Types of arguments to pass to the constructor of the second object.
	//!
	//! @param      An object that forces compiler to select this overload.
	//! @param arg  An argument to pass to the constructor of the first object.
	//! @param args Arguments to pass to the constructor of the second object.
	template<typename InitializationArgumentType, typename... InitializationArgumentTypes>
	CompressedPair(OneToFirstRestToSecond, InitializationArgumentType &&arg,
				   InitializationArgumentTypes&&... args)
		: FirstPartType(std::forward<InitializationArgumentType>(arg))
		, second(std::forward<InitializationArgumentTypes>(args)...)
	{
	}

	//! Provides a reference to the first object.
	FirstPartType& First() noexcept
	{
		return *this;
	}
	const FirstPartType& First() const noexcept
	{
		return *this;
	}
	volatile FirstPartType& First() volatile noexcept
	{
		return *this;
	}
	const volatile FirstPartType& First() const volatile noexcept
	{
		return *this;
	}

	//! Provides a reference to the second object.
	SecondPartType& Second() noexcept
	{
		return this->second;
	}
	const SecondPartType& Second() const noexcept
	{
		return this->second;
	}
	volatile SecondPartType& Second() volatile noexcept
	{
		return this->second;
	}
	const volatile SecondPartType& Second() const volatile noexcept
	{
		return this->second;
	}
};
// Specialization for the case when the first object is either not empty or cannot be inherited.
template<typename FirstPartType, typename SecondPartType>
class CompressedPair<FirstPartType, SecondPartType, false> final : private FirstPartType
{
	FirstPartType  first;
	SecondPartType second;
public:
	//! Creates a new pair by performing default initialization of the first object and forwarding the rest of
	//! the arguments to the constructor of the second object.
	//!
	//! @tparam InitializationArgumentTypes Types of arguments to pass to the constructor of the second object.
	//!
	//! @param      An object that forces compiler to select this overload.
	//! @param args Arguments to pass to the constructor of the second object.
	template<typename... InitializationArgumentTypes>
	CompressedPair(NoneToFirstRestToSecond, InitializationArgumentTypes&&... args)
		: first()
		, second(std::forward<InitializationArgumentTypes>(args)...)
	{
	}
	//! Creates a new pair by calling a constructor of the first object and forwarding one argument to it and
	//! forwarding the rest of arguments to the constructor of the second object.
	//!
	//! @tparam InitializationArgumentType  Type of the argument to pass to the constructor of the first object.
	//! @tparam InitializationArgumentTypes Types of arguments to pass to the constructor of the second object.
	//!
	//! @param      An object that forces compiler to select this overload.
	//! @param arg  An argument to pass to the constructor of the first object.
	//! @param args Arguments to pass to the constructor of the second object.
	template<typename InitializationArgumentType, typename... InitializationArgumentTypes>
	CompressedPair(OneToFirstRestToSecond, InitializationArgumentType &&arg,
				   InitializationArgumentTypes&&... args)
		: first(std::forward<InitializationArgumentType>(arg))
		, second(std::forward<InitializationArgumentTypes>(args)...)
	{
	}

	//! Provides a reference to the first object.
	FirstPartType& First() noexcept
	{
		return this->first;
	}
	const FirstPartType& First() const noexcept
	{
		return this->first;
	}
	volatile FirstPartType& First() volatile noexcept
	{
		return this->first;
	}
	const volatile FirstPartType& First() const volatile noexcept
	{
		return this->first;
	}

	//! Provides a reference to the second object.
	SecondPartType& Second() noexcept
	{
		return this->second;
	}
	const SecondPartType& Second() const noexcept
	{
		return this->second;
	}
	volatile SecondPartType& Second() volatile noexcept
	{
		return this->second;
	}
	const volatile SecondPartType& Second() const volatile noexcept
	{
		return this->second;
	}
};