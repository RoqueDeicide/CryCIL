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
		return MonoEnv->Cryambly->GetClass(nameSpace, className)->Box(value);
	}
public:
	virtual mono::object BoxUPtr(void *value);

	virtual mono::object BoxPtr(void *value);

	virtual mono::object Box(bool value);

	virtual mono::object Box(char value);

	virtual mono::object Box(signed char value);

	virtual mono::object Box(unsigned char value);

	virtual mono::object Box(short value);

	virtual mono::object Box(unsigned short value);

	virtual mono::object Box(int value);

	virtual mono::object Box(unsigned int value);

	virtual mono::object Box(__int64 value);

	virtual mono::object Box(unsigned __int64 value);

	virtual mono::object Box(float value);

	virtual mono::object Box(double value);

	virtual mono::object Box(Vec2 value);

	virtual mono::object Box(Vec3 value);

	virtual mono::object Box(Vec4 value);

	virtual mono::object Box(Ang3 value);

	virtual mono::object Box(Quat value);

	virtual mono::object Box(QuatT value);

	virtual mono::object Box(Matrix33 value);

	virtual mono::object Box(Matrix34 value);

	virtual mono::object Box(Matrix44 value);

	virtual mono::object Box(Plane value);

	virtual mono::object Box(Ray value);

	virtual mono::object Box(ColorB value);

	virtual mono::object Box(ColorF value);

	virtual mono::object Box(AABB value);
};