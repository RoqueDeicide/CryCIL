#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

struct DefaultBoxinator : public IDefaultBoxinator
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
	VIRTUAL_API virtual mono::object BoxUPtr(void *value);

	VIRTUAL_API virtual mono::object BoxPtr(void *value);

	VIRTUAL_API virtual mono::object Box(bool value);

	VIRTUAL_API virtual mono::object Box(char value);

	VIRTUAL_API virtual mono::object Box(signed char value);

	VIRTUAL_API virtual mono::object Box(unsigned char value);

	VIRTUAL_API virtual mono::object Box(short value);

	VIRTUAL_API virtual mono::object Box(unsigned short value);

	VIRTUAL_API virtual mono::object Box(int value);

	VIRTUAL_API virtual mono::object Box(unsigned int value);

	VIRTUAL_API virtual mono::object Box(__int64 value);

	VIRTUAL_API virtual mono::object Box(unsigned __int64 value);

	VIRTUAL_API virtual mono::object Box(float value);

	VIRTUAL_API virtual mono::object Box(double value);

	VIRTUAL_API virtual mono::object Box(Vec2 value);

	VIRTUAL_API virtual mono::object Box(Vec3 value);

	VIRTUAL_API virtual mono::object Box(Vec4 value);

	VIRTUAL_API virtual mono::object Box(Ang3 value);

	VIRTUAL_API virtual mono::object Box(Quat value);

	VIRTUAL_API virtual mono::object Box(QuatT value);

	VIRTUAL_API virtual mono::object Box(Matrix33 value);

	VIRTUAL_API virtual mono::object Box(Matrix34 value);

	VIRTUAL_API virtual mono::object Box(Matrix44 value);

	VIRTUAL_API virtual mono::object Box(Plane value);

	VIRTUAL_API virtual mono::object Box(Ray value);

	VIRTUAL_API virtual mono::object Box(ColorB value);

	VIRTUAL_API virtual mono::object Box(ColorF value);

	VIRTUAL_API virtual mono::object Box(AABB value);

};