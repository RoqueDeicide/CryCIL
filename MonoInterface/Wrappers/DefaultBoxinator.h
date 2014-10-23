#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

struct DefaultBoxinator : IDefaultBoxinator
{
private:
	__forceinline mono::object box(MonoClass *klass, void *value)
	{
		return (mono::object)mono_value_box((MonoDomain *)MonoEnv->AppDomain, klass, value);
	}
	__forceinline mono::object box(const char *className, const char *nameSpace, void *value)
	{
		return MonoEnv->Cryambly->GetClass(className, nameSpace)->Box(value);
	}
public:
	VIRTUAL_API virtual mono::object BoxUPtr(void *value)
	{
		return this->box(mono_get_uintptr_class(), &value);
	}

	VIRTUAL_API virtual mono::object BoxPtr(void *value)
	{
		return this->box(mono_get_intptr_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(bool value)
	{
		return this->box(mono_get_boolean_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(char value)
	{
		return this->box(mono_get_char_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(signed char value)
	{
		return this->box(mono_get_sbyte_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(unsigned char value)
	{
		return this->box(mono_get_byte_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(short value)
	{
		return this->box(mono_get_int16_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(unsigned short value)
	{
		return this->box(mono_get_uint16_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(int value)
	{
		return this->box(mono_get_int32_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(unsigned int value)
	{
		return this->box(mono_get_uint32_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(__int64 value)
	{
		return this->box(mono_get_int64_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(unsigned __int64 value)
	{
		return this->box(mono_get_uint64_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(float value)
	{
		return this->box(mono_get_single_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(double value)
	{
		return this->box(mono_get_double_class(), &value);
	}

	VIRTUAL_API virtual mono::object Box(Vec2 value)
	{
		return this->box("Vector2", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Vec3 value)
	{
		return this->box("Vector3", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Vec4 value)
	{
		return this->box("Vector4", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Ang3 value)
	{
		return this->box("EulerAngles", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Quat value)
	{
		return this->box("Quaternion", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(QuatT value)
	{
		return this->box("QuaternionTranslation", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Matrix33 value)
	{
		return this->box("Matrix33", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Matrix34 value)
	{
		return this->box("Matrix34", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Matrix44 value)
	{
		return this->box("Matrix44", "CryCil.Mathematics", &value);
	}

	VIRTUAL_API virtual mono::object Box(Plane value)
	{
		return this->box("Plane", "CryCil.Mathematics.Geometry", &value);
	}

	VIRTUAL_API virtual mono::object Box(Ray value)
	{
		return this->box("Ray", "CryCil.Mathematics.Geometry", &value);
	}

	VIRTUAL_API virtual mono::object Box(ColorB value)
	{
		return this->box("ColorByte", "CryCil.Graphics", &value);
	}

	VIRTUAL_API virtual mono::object Box(ColorF value)
	{
		return this->box("ColorSingle", "CryCil.Graphics", &value);
	}

	VIRTUAL_API virtual mono::object Box(AABB value)
	{
		return this->box("BoundingBox", "CryCil.Mathematics", &value);
	}

};