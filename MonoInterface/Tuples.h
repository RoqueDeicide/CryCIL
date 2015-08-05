#pragma once
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