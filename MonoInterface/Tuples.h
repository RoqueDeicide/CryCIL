#pragma once

#include <type_traits>

//! Encapsulates two objects.
template<typename T1, typename T2>
struct Pair
{
	T1 Value1;                  //!< First value.
	T2 Value2;                  //!< Second value.
	//! Creates default instance of this type.
	Pair() : Value1(), Value2()
	{
	}
	//! Creates a new instance of this type.
	//!
	//! @param value1 First object.
	//! @param value2 Second object.
	constexpr Pair(const T1 &value1, const T2 &value2) : Value1(value1), Value2(value2)
	{
	}
	//! Creates a default copy of the object.
	Pair(const Pair &other) = default;
	//! Creates an object from a temporary one.
	Pair(Pair &&other) = default;
	//! Creates a pair of objects by converting a pair of other objects.
	//!
	//! @tparam Other1 Type of the first object in the given pair.
	//! @tparam Other2 Type of the second object in the given pair.
	//!
	//! @param other A pair of objects to convert into respective parts of the new pair.
	template<typename Other1, typename Other2,
			 typename = typename EnableIf<std::is_convertible<const Other1 &, T1>::value &&
										  std::is_convertible<const Other2 &, T2>::value,
										  void>::type>
	constexpr Pair(const Pair<Other1, Other2> &other)
		: Value1(other.Value1), Value2(other.Value2)
	{
	}
	//! Creates a pair of objects by converting a pair of other objects.
	//!
	//! @tparam Other1 Type of the first object.
	//! @tparam Other2 Type of the second object.
	//!
	//! @param value1 An object to construct the first part of the new pair from.
	//! @param value2 An object to construct the second part of the new pair from.
	template<typename Other1, typename Other2,
			 typename = typename EnableIf<std::is_convertible<Other1, T1>::value &&
										  std::is_convertible<Other2, T2>::value,
										  void>::type>
	constexpr Pair(Other1 &&value1, Other2 &&value2)
		noexcept(std::is_nothrow_constructible<T1, Other1 &&>::value &&
				 std::is_nothrow_constructible<T2, Other2 &&>::value)
		: Value1(std::forward<Other1>(value1))
		, Value2(std::forward<Other2>(value2))
	{
	}
	//! Creates a pair of objects by converting a pair of other objects.
	//!
	//! @tparam Other1 Type of the first object.
	//! @tparam Other2 Type of the second object.
	//!
	//! @param value1 An object to construct the first part of the new pair from.
	//! @param value2 An object to construct the second part of the new pair from.
	template<typename Other1, typename Other2,
			 typename = typename EnableIf<std::is_convertible<Other1, T1>::value &&
										  std::is_convertible<Other2, T2>::value,
										  void>::type>
	constexpr Pair(Pair<Other1, Other2> &&other)
		noexcept(std::is_nothrow_constructible<T1, Other1 &&>::value &&
				 std::is_nothrow_constructible<T2, Other2 &&>::value)
		: Value1(std::forward<Other1>(other.Value1))
		, Value2(std::forward<Other2>(other.Value2))
	{
	}
	//! Assign a compatible pair.
	template<typename Other1, typename Other2>
	Pair & operator =(const Pair<Other1, Other2> &other)
	{
		this->Value1 = other.Value1;
		this->Value2 = other.Value2;
		return *this;
	}
	//! Assign a pair.
	Pair & operator =(const Pair &other)
	{
		this->Value1 = other.Value1;
		this->Value2 = other.Value2;
		return *this;
	}
	//! Assign a compatible temporary pair.
	template<typename Other1, typename Other2>
	Pair & operator =(Pair<Other1, Other2> &&other)
	{
		this->Value1 = std::forward<Other1>(other.Value1);
		this->Value2 = std::forward<Other2>(other.Value2);
		return *this;
	}
	//! Assign a temporary pair.
	Pair & operator =(Pair &&other)
	{
		this->Value1 = std::forward<T1>(other.Value1);
		this->Value2 = std::forward<T2>(other.Value2);
		return *this;
	}
};
//! Encapsulates three objects.
template<typename T1, typename T2, typename T3>
struct Triple
{
	T1 Value1;                  //!< First value.
	T2 Value2;                  //!< Second value.
	T3 Value3;                  //!< Third value.
	//! Creates default instance of this type.
	Triple() : Value1(), Value2(), Value3()
	{}
	//! Creates a new instance of this type.
	//!
	//! @param value1 An object a copy of which is used to initialize the first value.
	//! @param value2 An object a copy of which is used to initialize the second value.
	//! @param value3 An object a copy of which is used to initialize the third value.
	constexpr Triple(const T1 &value1, const T2 &value2, const T3 &value3)
		: Value1(value1), Value2(value2), Value3(value3)
	{
	}
	//! Creates a default copy of the object.
	Triple(const Triple &other) = default;
	//! Creates an object from a temporary one.
	Triple(Triple &&other) = default;
	//! Creates a tuple of objects by converting a tuple of other objects.
	//!
	//! @tparam Other1 Type of the first object in the given tuple.
	//! @tparam Other2 Type of the second object in the given tuple.
	//! @tparam Other3 Type of the third object in the given tuple.
	//!
	//! @param other A tuple of objects to convert into respective parts of the new tuple.
	template<typename Other1, typename Other2, typename Other3,
			 typename = typename EnableIf<std::is_convertible<const Other1 &, T1>::value &&
										  std::is_convertible<const Other2 &, T2>::value &&
										  std::is_convertible<const Other3 &, T3>::value,
										  void>::type>
	constexpr Triple(const Triple<Other1, Other2, Other3> &other)
		: Value1(other.Value1)
		, Value2(other.Value2)
		, Value3(other.Value3)
	{
	}
	//! Creates a tuple of objects by converting a tuple of other objects.
	//!
	//! @tparam Other1 Type of the first object.
	//! @tparam Other2 Type of the second object.
	//! @tparam Other3 Type of the third object.
	//! @tparam Other4 Type of the fourth object.
	//!
	//! @param value1 An object to construct the first part of the new object from.
	//! @param value2 An object to construct the second part of the new object from.
	//! @param value3 An object to construct the third part of the new object from.
	template<typename Other1, typename Other2, typename Other3,
			 typename = typename EnableIf<std::is_convertible<Other1, T1>::value &&
										  std::is_convertible<Other2, T2>::value &&
										  std::is_convertible<Other3, T3>::value,
										  void>::type>
	constexpr Triple(Other1 &&value1, Other2 &&value2, Other3 &&value3)
		noexcept(std::is_nothrow_constructible<T1, Other1 &&>::value &&
				 std::is_nothrow_constructible<T2, Other2 &&>::value &&
				 std::is_nothrow_constructible<T3, Other3 &&>::value)
		: Value1(std::forward<Other1>(value1))
		, Value2(std::forward<Other2>(value2))
		, Value3(std::forward<Other3>(value3))
	{
	}
	//! Creates a tuple of objects by converting a tuple of other objects.
	//!
	//! @tparam Other1 Type of the first object in the given tuple.
	//! @tparam Other2 Type of the second object in the given tuple.
	//! @tparam Other3 Type of the third object in the given tuple.
	//!
	//! @param other A tuple of objects to convert into respective parts of the new tuple.
	template<typename Other1, typename Other2, typename Other3,
			 typename = typename EnableIf<std::is_convertible<Other1, T1>::value &&
										  std::is_convertible<Other2, T2>::value &&
										  std::is_convertible<Other3, T3>::value,
										  void>::type>
	constexpr Triple(Triple<Other1, Other2, Other3> &&other)
		noexcept(std::is_nothrow_constructible<T1, Other1 &&>::value &&
				 std::is_nothrow_constructible<T2, Other2 &&>::value &&
				 std::is_nothrow_constructible<T3, Other3 &&>::value)
		: Value1(std::forward<Other1>(other.Value1))
		, Value2(std::forward<Other2>(other.Value2))
		, Value3(std::forward<Other3>(other.Value3))
	{
	}
	//! Assign a compatible tuple.
	//!
	//! @tparam Other1 Type of the first object in the given tuple.
	//! @tparam Other2 Type of the second object in the given tuple.
	//! @tparam Other3 Type of the third object in the given tuple.
	//!
	//! @param other A tuple of objects to convert into respective parts of the new tuple.
	//!
	//! @returns Reference to this object, after it was assigned.
	template<typename Other1, typename Other2, typename Other3>
	Triple & operator =(const Triple<Other1, Other2, Other3> &other)
	{
		this->Value1 = other.Value1;
		this->Value2 = other.Value2;
		this->Value3 = other.Value3;
		return *this;
	}
	//! Assign a tuple.
	//!
	//! @param other Reference to another tuple.
	//!
	//! @returns Reference to this object, after it was assigned.
	Triple & operator =(const Triple &other)
	{
		this->Value1 = other.Value1;
		this->Value2 = other.Value2;
		this->Value3 = other.Value3;
		return *this;
	}
	//! Assign a compatible temporary tuple.
	//!
	//! @param other Reference to the temporary tuple which parts are converted into respective elements of this
	//!              object.
	//!
	//! @returns Reference to this object, after it was assigned.
	template<typename Other1, typename Other2, typename Other3>
	Triple & operator =(Triple<Other1, Other2, Other3> &&other)
	{
		this->Value1 = std::forward<Other1>(other.Value1);
		this->Value2 = std::forward<Other2>(other.Value2);
		this->Value3 = std::forward<Other3>(other.Value3);
		return *this;
	}
	//! Assign a temporary tuple.
	//!
	//! @param other Reference to the temporary tuple.
	//!
	//! @returns Reference to this object, after it was assigned.
	Triple & operator =(Triple &&other)
	{
		this->Value1 = std::forward<T1>(other.Value1);
		this->Value2 = std::forward<T2>(other.Value2);
		this->Value3 = std::forward<T3>(other.Value3);
		return *this;
	}
};
//! Encapsulates four objects.
template<typename T1, typename T2, typename T3, typename T4>
struct Quad
{
	T1 Value1;                  //!< First value.
	T2 Value2;                  //!< Second value.
	T3 Value3;                  //!< Third value.
	T4 Value4;                  //!< Fourth value.
	Quad() : Value1(), Value2(), Value3(), Value4()
	{
	}
	//! Creates a new instance of this type.
	//!
	//! @param value1 An object a copy of which is used to initialize the first value.
	//! @param value2 An object a copy of which is used to initialize the second value.
	//! @param value3 An object a copy of which is used to initialize the third value.
	//! @param value4 An object a copy of which is used to initialize the fourth value.
	constexpr Quad(const T1 &value1, const T2 &value2, const T3 &value3, const T4 &value4)
		: Value1(value1)
		, Value2(value2)
		, Value3(value3)
		, Value4(value4)
	{
	}
	//! Creates a default copy of the object.
	Quad(const Quad &other) = default;
	//! Creates an object from a temporary one.
	Quad(Quad &&other) = default;
	//! Creates a tuple of objects by converting a tuple of other objects.
	//!
	//! @tparam Other1 Type of the first object in the given tuple.
	//! @tparam Other2 Type of the second object in the given tuple.
	//! @tparam Other3 Type of the third object in the given tuple.
	//! @tparam Other4 Type of the fourth object in the given tuple.
	//!
	//! @param other A tuple of objects to convert into respective parts of the new tuple.
	template<typename Other1, typename Other2, typename Other3, typename Other4,
			 typename = typename EnableIf<std::is_convertible<const Other1 &, T1>::value &&
										  std::is_convertible<const Other2 &, T2>::value &&
										  std::is_convertible<const Other3 &, T3>::value &&
										  std::is_convertible<const Other4 &, T4>::value,
										  void>::type>
	constexpr Quad(const Quad<Other1, Other2, Other3, Other4> &other)
		: Value1(other.Value1)
		, Value2(other.Value2)
		, Value3(other.Value3)
		, Value4(other.Value4)
	{
	}
	//! Creates a tuple of objects by converting a tuple of other objects.
	//!
	//! @tparam Other1 Type of the first object.
	//! @tparam Other2 Type of the second object.
	//! @tparam Other3 Type of the third object.
	//! @tparam Other4 Type of the fourth object.
	//!
	//! @param value1 An object to construct the first part of the new object from.
	//! @param value2 An object to construct the second part of the new object from.
	//! @param value3 An object to construct the third part of the new object from.
	//! @param value4 An object to construct the fourth part of the new object from.
	template<typename Other1, typename Other2, typename Other3, typename Other4,
			 typename = typename EnableIf<std::is_convertible<Other1, T1>::value &&
										  std::is_convertible<Other2, T2>::value &&
										  std::is_convertible<Other3, T3>::value &&
										  std::is_convertible<Other4, T4>::value,
										  void>::type>
	constexpr Quad(Other1 &&value1, Other2 &&value2, Other3 &&value3, Other4 &&value4)
		noexcept(std::is_nothrow_constructible<T1, Other1 &&>::value &&
				 std::is_nothrow_constructible<T2, Other2 &&>::value &&
				 std::is_nothrow_constructible<T3, Other3 &&>::value &&
				 std::is_nothrow_constructible<T4, Other4 &&>::value)
		: Value1(std::forward<Other1>(value1))
		, Value2(std::forward<Other2>(value2))
		, Value3(std::forward<Other3>(value3))
		, Value4(std::forward<Other4>(value4))
	{
	}
	//! Creates a tuple of objects by converting a tuple of other objects.
	//!
	//! @tparam Other1 Type of the first object in the given tuple.
	//! @tparam Other2 Type of the second object in the given tuple.
	//! @tparam Other3 Type of the third object in the given tuple.
	//! @tparam Other4 Type of the fourth object in the given tuple.
	//!
	//! @param other A tuple of objects to convert into respective parts of the new tuple.
	template<typename Other1, typename Other2, typename Other3, typename Other4,
			 typename = typename EnableIf<std::is_convertible<Other1, T1>::value &&
										  std::is_convertible<Other2, T2>::value &&
										  std::is_convertible<Other3, T3>::value &&
										  std::is_convertible<Other4, T4>::value,
										  void>::type>
	constexpr Quad(Quad<Other1, Other2, Other3, Other4> &&other)
		noexcept(std::is_nothrow_constructible<T1, Other1 &&>::value &&
				 std::is_nothrow_constructible<T2, Other2 &&>::value &&
				 std::is_nothrow_constructible<T3, Other3 &&>::value &&
				 std::is_nothrow_constructible<T4, Other4 &&>::value)
		: Value1(std::forward<Other1>(other.Value1))
		, Value2(std::forward<Other2>(other.Value2))
		, Value3(std::forward<Other3>(other.Value3))
		, Value4(std::forward<Other4>(other.Value4))
	{
	}
	//! Assign a compatible tuple.
	//!
	//! @tparam Other1 Type of the first object in the given tuple.
	//! @tparam Other2 Type of the second object in the given tuple.
	//! @tparam Other3 Type of the third object in the given tuple.
	//! @tparam Other4 Type of the fourth object in the given tuple.
	//!
	//! @param other A tuple of objects to convert into respective parts of the new tuple.
	//!
	//! @returns Reference to this object, after it was assigned.
	template<typename Other1, typename Other2, typename Other3, typename Other4>
	Quad & operator =(const Quad<Other1, Other2, Other3, Other4> &other)
	{
		this->Value1 = other.Value1;
		this->Value2 = other.Value2;
		this->Value3 = other.Value3;
		this->Value4 = other.Value4;
		return *this;
	}
	//! Assign a tuple.
	//!
	//! @param other Reference to another tuple.
	//!
	//! @returns Reference to this object, after it was assigned.
	Quad & operator =(const Quad &other)
	{
		this->Value1 = other.Value1;
		this->Value2 = other.Value2;
		this->Value3 = other.Value3;
		this->Value4 = other.Value4;
		return *this;
	}
	//! Assign a compatible temporary tuple.
	//!
	//! @param other Reference to the temporary tuple which parts are converted into respective elements of this
	//!              object.
	//!
	//! @returns Reference to this object, after it was assigned.
	template<typename Other1, typename Other2, typename Other3, typename Other4>
	Quad & operator =(Quad<Other1, Other2, Other3, Other4> &&other)
	{
		this->Value1 = std::forward<Other1>(other.Value1);
		this->Value2 = std::forward<Other2>(other.Value2);
		this->Value3 = std::forward<Other3>(other.Value3);
		this->Value4 = std::forward<Other4>(other.Value4);
		return *this;
	}
	//! Assign a temporary tuple.
	//!
	//! @param other Reference to the temporary tuple.
	//!
	//! @returns Reference to this object, after it was assigned.
	Quad & operator =(Quad &&other)
	{
		this->Value1 = std::forward<T1>(other.Value1);
		this->Value2 = std::forward<T2>(other.Value2);
		this->Value3 = std::forward<T3>(other.Value3);
		this->Value4 = std::forward<T4>(other.Value4);
		return *this;
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
		 bool = std::is_empty<FirstPartType>::value && !std::is_final<FirstPartType>::value>
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
	CompressedPair(NoneToFirstRestToSecond, InitializationArgumentTypes && ... args)
		: FirstPartType()
		, second(std::forward<InitializationArgumentTypes>(args) ...)
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
				   InitializationArgumentTypes && ... args)
		: FirstPartType(std::forward<InitializationArgumentType>(arg))
		, second(std::forward<InitializationArgumentTypes>(args) ...)
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
	CompressedPair(NoneToFirstRestToSecond, InitializationArgumentTypes && ... args)
		: first()
		, second(std::forward<InitializationArgumentTypes>(args) ...)
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
				   InitializationArgumentTypes && ... args)
		: first(std::forward<InitializationArgumentType>(arg))
		, second(std::forward<InitializationArgumentTypes>(args) ...)
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