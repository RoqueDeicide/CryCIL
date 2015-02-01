#include "stdafx.h"
#include "API_ImplementationHeaders.h"



mono::object DefaultBoxinator::BoxUPtr(void *value)
{
	return this->box(mono_get_uintptr_class(), &value);
}

mono::object DefaultBoxinator::BoxPtr(void *value)
{
	return this->box(mono_get_intptr_class(), &value);
}

mono::object DefaultBoxinator::Box(bool value)
{
	return this->box(mono_get_boolean_class(), &value);
}

mono::object DefaultBoxinator::Box(char value)
{
	return this->box(mono_get_char_class(), &value);
}

mono::object DefaultBoxinator::Box(signed char value)
{
	return this->box(mono_get_sbyte_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned char value)
{
	return this->box(mono_get_byte_class(), &value);
}

mono::object DefaultBoxinator::Box(short value)
{
	return this->box(mono_get_int16_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned short value)
{
	return this->box(mono_get_uint16_class(), &value);
}

mono::object DefaultBoxinator::Box(int value)
{
	return this->box(mono_get_int32_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned int value)
{
	return this->box(mono_get_uint32_class(), &value);
}

mono::object DefaultBoxinator::Box(__int64 value)
{
	return this->box(mono_get_int64_class(), &value);
}

mono::object DefaultBoxinator::Box(unsigned __int64 value)
{
	return this->box(mono_get_uint64_class(), &value);
}

mono::object DefaultBoxinator::Box(float value)
{
	return this->box(mono_get_single_class(), &value);
}

mono::object DefaultBoxinator::Box(double value)
{
	return this->box(mono_get_double_class(), &value);
}

mono::object DefaultBoxinator::Box(Vec2 value)
{
	return this->box("Vector2", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Vec3 value)
{
	return this->box("Vector3", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Vec4 value)
{
	return this->box("Vector4", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Ang3 value)
{
	return this->box("EulerAngles", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Quat value)
{
	return this->box("Quaternion", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(QuatT value)
{
	return this->box("QuaternionTranslation", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Matrix33 value)
{
	return this->box("Matrix33", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Matrix34 value)
{
	return this->box("Matrix34", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Matrix44 value)
{
	return this->box("Matrix44", "CryCil.Mathematics", &value);
}

mono::object DefaultBoxinator::Box(Plane value)
{
	return this->box("Plane", "CryCil.Mathematics.Geometry", &value);
}

mono::object DefaultBoxinator::Box(Ray value)
{
	return this->box("Ray", "CryCil.Mathematics.Geometry", &value);
}

mono::object DefaultBoxinator::Box(ColorB value)
{
	return this->box("ColorByte", "CryCil.Graphics", &value);
}

mono::object DefaultBoxinator::Box(ColorF value)
{
	return this->box("ColorSingle", "CryCil.Graphics", &value);
}

mono::object DefaultBoxinator::Box(AABB value)
{
	return this->box("BoundingBox", "CryCil.Mathematics", &value);
}