#include "stdafx.h"

#include "MathHelpers.h"

void MathHelpersInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(RsqrtSingle);
	REGISTER_METHOD(RsqrtDouble);
	REGISTER_METHOD(SinCosSingle);
	REGISTER_METHOD(SinCosDouble);
	REGISTER_METHOD(LogQuat);
	REGISTER_METHOD(ExpVector);
}

float MathHelpersInterop::RsqrtSingle(float value)
{
	return 1 / sqrtf(value);
}

double MathHelpersInterop::RsqrtDouble(double value)
{
	return 1 / sqrt(value);
}

void MathHelpersInterop::SinCosSingle(float value, float &sine, float &cosine)
{
	sine = sinf(value);
	cosine = sqrtf(1 - sine * sine);
}

void MathHelpersInterop::SinCosDouble(double value, double &sine, double &cosine)
{
	sine = sin(value);
	cosine = sqrt(1 - sine * sine);
}

Vec3 MathHelpersInterop::LogQuat(Quat value)
{
	float lengthSquared = value.v.len2();

	if (lengthSquared <= 0.0f)
	{
		// logarithm of a quaternion, imaginary part (the real part of the logarithm is always 0).
		return Vec3(ZERO);
	}

	float length = sqrtf(lengthSquared);
	float angle = atan2f(length, value.w) / length;
	return value.v * angle;
}

Quat MathHelpersInterop::ExpVector(Vec3 value)
{
	float lengthSquared = value.len2();

	if (lengthSquared <= 0.0f)
	{
		return Quat(ZERO);
	}

	float length = sqrtf(lengthSquared);
	float s, c;
	sincos_tpl(length, &s, &c);
	s /= length;
	return Quat(c, value * s);
}
