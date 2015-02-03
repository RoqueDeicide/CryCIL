#pragma once

#include "IMonoAliases.h"

//! Interface for the object that does default boxing operations.
struct IDefaultBoxinator
{
	virtual ~IDefaultBoxinator() {}

	//! Boxes an unsigned pointer value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uintptr BoxUPtr(void *value) = 0;
	//! Boxes a pointer value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::intptr BoxPtr(void *value) = 0;
	//! Boxes a boolean value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::boolean Box(bool value) = 0;
	//! Boxes a signed byte value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::character Box(char value) = 0;
	//! Boxes a signed byte value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::sbyte Box(signed char value) = 0;
	//! Boxes an unsigned byte value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::byte Box(unsigned char value) = 0;
	//! Boxes an Int16 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::int16 Box(short value) = 0;
	//! Boxes a UInt16 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uint16 Box(unsigned short value) = 0;
	//! Boxes an Int32 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::int32 Box(int value) = 0;
	//! Boxes a UInt32 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uint32 Box(unsigned int value) = 0;
	//! Boxes an Int64 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::int64 Box(__int64 value) = 0;
	//! Boxes a UInt64 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uint64 Box(unsigned __int64 value) = 0;
	//! Boxes a float value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::float32 Box(float value) = 0;
	//! Boxes a double value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::float64 Box(double value) = 0;
	//! Boxes a vector value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::vector2 Box(Vec2 value) = 0;
	//! Boxes a vector value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::vector3 Box(Vec3 value) = 0;
	//! Boxes a vector value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::vector4 Box(Vec4 value) = 0;
	//! Boxes a EulerAngles value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::angles3 Box(Ang3 value) = 0;
	//! Boxes a Quaternion value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::quaternion Box(Quat value) = 0;
	//! Boxes an QuaternionTranslation value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::quat_trans Box(QuatT value) = 0;
	//! Boxes a Matrix33 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::matrix33 Box(Matrix33 value) = 0;
	//! Boxes an Matrix34 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::matrix34 Box(Matrix34 value) = 0;
	//! Boxes a Matrix44 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::matrix44 Box(Matrix44 value) = 0;
	//! Boxes a Plane value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::plane Box(Plane value) = 0;
	//! Boxes a Ray value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::ray Box(Ray value) = 0;
	//! Boxes a ColorB value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::byte_color Box(ColorB value) = 0;
	//! Boxes a ColorF value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::float32_color Box(ColorF value) = 0;
	//! Boxes a AABB value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::aabb Box(AABB value) = 0;
};