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
	return MonoEnv->Cryambly->Vector2->Box(&value);
}

mono::object DefaultBoxinator::Box(Vec3 value)
{
	return MonoEnv->Cryambly->Vector3->Box(&value);
}

mono::object DefaultBoxinator::Box(Vec4 value)
{
	return MonoEnv->Cryambly->Vector4->Box(&value);
}

mono::object DefaultBoxinator::Box(Ang3 value)
{
	return MonoEnv->Cryambly->EulerAngles->Box(&value);
}

mono::object DefaultBoxinator::Box(Quat value)
{
	return MonoEnv->Cryambly->Quaternion->Box(&value);
}

mono::object DefaultBoxinator::Box(QuatT value)
{
	return MonoEnv->Cryambly->Quatvec->Box(&value);
}

mono::object DefaultBoxinator::Box(Matrix33 value)
{
	return MonoEnv->Cryambly->Matrix33->Box(&value);
}

mono::object DefaultBoxinator::Box(Matrix34 value)
{
	return MonoEnv->Cryambly->Matrix34->Box(&value);
}

mono::object DefaultBoxinator::Box(Matrix44 value)
{
	return MonoEnv->Cryambly->Matrix44->Box(&value);
}

mono::object DefaultBoxinator::Box(Plane value)
{
	return MonoEnv->Cryambly->Plane->Box(&value);
}

mono::object DefaultBoxinator::Box(Ray value)
{
	return MonoEnv->Cryambly->Ray->Box(&value);
}

mono::object DefaultBoxinator::Box(ColorB value)
{
	return MonoEnv->Cryambly->ColorByte->Box(&value);
}

mono::object DefaultBoxinator::Box(ColorF value)
{
	return MonoEnv->Cryambly->ColorSingle->Box(&value);
}

mono::object DefaultBoxinator::Box(AABB value)
{
	return MonoEnv->Cryambly->BoundingBox->Box(&value);
}