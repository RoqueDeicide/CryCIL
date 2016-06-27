#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

//! Implementation for IDefaultBoxinator.
struct DefaultBoxinator : public IDefaultBoxinator
{
private:
	static __forceinline mono::object box(MonoClass *klass, void *value)
	{
		return mono::object(mono_value_box(static_cast<MonoDomain *>(MonoEnv->AppDomain), klass, value));
	}
public:
	mono::object BoxUPtr(void *value) override;
	mono::object BoxPtr(void *value) override;
	mono::object Box(bool value) override;
	mono::object Box(char value) override;
	mono::object Box(signed char value) override;
	mono::object Box(unsigned char value) override;
	mono::object Box(short value) override;
	mono::object Box(unsigned short value) override;
	mono::object Box(int value) override;
	mono::object Box(unsigned int value) override;
	mono::object Box(__int64 value) override;
	mono::object Box(unsigned __int64 value) override;
	mono::object Box(float value) override;
	mono::object Box(double value) override;
	mono::object Box(Vec2 value) override;
	mono::object Box(Vec3 value) override;
	mono::object Box(Vec4 value) override;
	mono::object Box(Ang3 value) override;
	mono::object Box(Quat value) override;
	mono::object Box(QuatT value) override;
	mono::object Box(Matrix33 value) override;
	mono::object Box(Matrix34 value) override;
	mono::object Box(Matrix44 value) override;
	mono::object Box(Plane value) override;
	mono::object Box(Ray value) override;
	mono::object Box(ColorB value) override;
	mono::object Box(ColorF value) override;
	mono::object Box(AABB value) override;
};