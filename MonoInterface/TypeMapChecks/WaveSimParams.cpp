#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct wave_sim
{
	float timeStep;
	float waveSpeed;
	float simDepth;
	float heightLimit;
	float resistance;
	float dampingCenter;
	float dampingRim;
	float minhSpread;
	float minVel;

	explicit wave_sim(params_wavesim &other)
	{
		static_assert(sizeof(wave_sim) == sizeof(params_wavesim), "params_wavesim structure has been changed.");

		ASSIGN_FIELD(timeStep);
		ASSIGN_FIELD(waveSpeed);
		ASSIGN_FIELD(simDepth);
		ASSIGN_FIELD(heightLimit);
		ASSIGN_FIELD(resistance);
		ASSIGN_FIELD(dampingCenter);
		ASSIGN_FIELD(dampingRim);
		ASSIGN_FIELD(minhSpread);
		ASSIGN_FIELD(minVel);

		CHECK_TYPE(timeStep);
		CHECK_TYPE(waveSpeed);
		CHECK_TYPE(simDepth);
		CHECK_TYPE(heightLimit);
		CHECK_TYPE(resistance);
		CHECK_TYPE(dampingCenter);
		CHECK_TYPE(dampingRim);
		CHECK_TYPE(minhSpread);
		CHECK_TYPE(minVel);
	}
};