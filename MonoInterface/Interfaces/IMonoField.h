#pragma once

#include "IMonoAliases.h"

//! Enumeration of flags that can be set for the Mono field.
enum MonoFieldAttributes : unsigned int
{
	FIELD_ACCESS_MASK    = 0x0007,	//!< A value that is a mask for access field access flags.
	COMPILER_CONTROLLED  = 0x0000,
	Private              = 0x0001,	//!< Set when the field is private.
	InternalAndProtected = 0x0002,	//!< Set when the field is internal and protected.
	Internal             = 0x0003,	//!< Set when the field is internal.
	Protected            = 0x0004,	//!< Set when the field is protected.
	InternalOrProtected  = 0x0005,	//!< Set when the field is internal or protected.
	Public               = 0x0006,	//!< Set when the field is public.
	Static               = 0x0010,	//!< Set when the field is static.
	ReadOnly             = 0x0020,	//!< Set when the field is read only (with readonly keyword).
	Constant             = 0x0040,	//!< Set when the field is compile-time constant.
	NotSerialized        = 0x0080,	//!< Set when the field does not have to be serialized when the type is remoted.
	SpecialName          = 0x0200,	//!< Set when the field's name contains underscore symbol.
	PInvokeImpl          = 0x2000	//!< reserved.
};

//! Defines a wrapper for a Mono field metadata object.
struct IMonoField : public IMonoFunctionalityWrapper
{
	//! Gets name of the field.
	__declspec(property(get = GetName)) const char *Name;
	//! Gets the class where this field is declared.
	__declspec(property(get = GetDeclaringType)) IMonoClass *DeclaringType;
	//! Gets offset of the field.
	__declspec(property(get = GetOffset)) unsigned int Offset;
	//! Gets attributes of the field.
	__declspec(property(get = GetAttributes)) MonoFieldAttributes Attributes;

	//! Gets the value of this field on the given object.
	//!
	//! @param obj   Object for which to get the value from.
	//! @param value Pointer to the object that will contain the value of the field.
	VIRTUAL_API virtual void Get(mono::object obj, void *value) = 0;
	//! Sets the value of this field on the given object.
	//!
	//! @param obj   Object for which to set the value on.
	//! @param value Pointer to the object that contains the new value for the field.
	VIRTUAL_API virtual void Set(mono::object obj, void *value) = 0;

	VIRTUAL_API virtual const char *GetName() = 0;
	VIRTUAL_API virtual IMonoClass *GetDeclaringType() = 0;
	VIRTUAL_API virtual unsigned int GetOffset() = 0;
	VIRTUAL_API virtual MonoFieldAttributes GetAttributes() = 0;

	//! Gets the value of the field.
	//!
	//! @param obj Object of which field to get.
	//!
	//! @tparam Type of the field.
	template<typename FieldType> FieldType Get(mono::object obj)
	{
		FieldType valueContainer;

		this->Get(obj, &valueContainer);

		return valueContainer;
	}
	//! Gets the value of the field.
	//!
	//! @param obj   Object of which field to get.
	//! @param value A value to set.
	//!
	//! @tparam Type of the field.
	template <typename FieldType> void Assign(mono::object obj, FieldType value)
	{
		this->Set(obj, &value);
	}
};