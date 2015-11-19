#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct SVelocity3
{
	Vec3 vLin;
	Vec3 vRot;

	explicit SVelocity3(const Velocity3 &other)
	{
		CHECK_TYPES_SIZE(SVelocity3, Velocity3);

		ASSIGN_FIELD(vLin);
		ASSIGN_FIELD(vRot);

		CHECK_TYPE(vLin);
		CHECK_TYPE(vRot);
	}
};