#pragma once

#include <type_traits>

//! Types that are marked with this macro are mirrors of specific types and are used to detect most changes that can be made
//! the mirrored type at compile time with only one change being undetectable which is reordering of fields.
#define TYPE_MIRROR

//! This template class is used to check equality of 2 types at compile-time.
//!
//! This allows quick and easy detection of changes that were made to objects that have mirror types in C# code.
//!
//! This template in general will indicate that types are not equal.
template<typename T, typename U>
struct is_same_type
{
	static const bool value = false;

	is_same_type(T first, U second) {}
};

//! This specialization of the above template is only used when the same type is specified for both parameters and
//! indicates that those types are equal.
template<typename T>
struct is_same_type<T, T>
{
	static const bool value = true;

	is_same_type(T first, T second) {}
};

// Determines whether a field that is defined in declaring type is of the same type as the same field in the mirrored type.
#define CHECK_TYPE(fieldName) static_assert (is_same_type<decltype(this->fieldName), decltype(other.fieldName)>::value, "Type of the field named "#fieldName" has been changed.")