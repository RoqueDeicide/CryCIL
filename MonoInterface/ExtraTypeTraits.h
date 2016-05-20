#pragma once

//! A special type that defines a typedef for _DefineIfTrue_ type, if _indicator_ is true.
//!
//! Example:
//!
//! @code
//!
//! // This function is only defined if its argument is of a POD type.
//! template<typename podType, typename = EnableIf<std::is_pod<podType>, void>>
//! void ProcessPod(podType arg)
//! {
//! }
//!
//! @endcode
//!
//! @tparam DefineIfTrue A type a typedef of which is defined in this type if _indicator_ is true.
//! @tparam indicator    Indicates whether a typedef must be defined of _DefineIfTrue_.
template<bool indicator, typename DefineIfTrue>
struct EnableIf
{
	// This is a specialization for the case where indicator is not true. The typedef is not defined.
};
template<typename DefineIfTrue>
struct EnableIf<true, DefineIfTrue>
{
	// This is a specialization for the case where indicator is true. The typedef is defined.
	typedef DefineIfTrue type;
};