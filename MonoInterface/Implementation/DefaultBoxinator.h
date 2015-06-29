#pragma once

#include "IMonoInterface.h"
#include "MonoHeaders.h"

//! Implementation for IDefaultBoxinator.
struct DefaultBoxinator : public IDefaultBoxinator
{
private:
	__forceinline mono::object box(MonoClass *klass, void *value)
	{
		return (mono::object)mono_value_box((MonoDomain *)MonoEnv->AppDomain, klass, value);
	}
public:
	virtual mono::object BoxUPtr(void *value) override;
	virtual mono::object BoxPtr(void *value) override;
	virtual mono::object Box(bool value) override;
	virtual mono::object Box(char value) override;
	virtual mono::object Box(signed char value) override;
	virtual mono::object Box(unsigned char value) override;
	virtual mono::object Box(short value) override;
	virtual mono::object Box(unsigned short value) override;
	virtual mono::object Box(int value) override;
	virtual mono::object Box(unsigned int value) override;
	virtual mono::object Box(__int64 value) override;
	virtual mono::object Box(unsigned __int64 value) override;
	virtual mono::object Box(float value) override;
	virtual mono::object Box(double value) override;
	virtual mono::object Box(Vec2 value) override;
	virtual mono::object Box(Vec3 value) override;
	virtual mono::object Box(Vec4 value) override;
	virtual mono::object Box(Ang3 value) override;
	virtual mono::object Box(Quat value) override;
	virtual mono::object Box(QuatT value) override;
	virtual mono::object Box(Matrix33 value) override;
	virtual mono::object Box(Matrix34 value) override;
	virtual mono::object Box(Matrix44 value) override;
	virtual mono::object Box(Plane value) override;
	virtual mono::object Box(Ray value) override;
	virtual mono::object Box(ColorB value) override;
	virtual mono::object Box(ColorF value) override;
	virtual mono::object Box(AABB value) override;
};