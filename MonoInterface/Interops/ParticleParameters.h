#pragma once

#include "IMonoInterface.h"
#include "CryParticleSystem/ParticleParams.h"

//! Represents a Mono object that is a wrapper for particle parameters struct.
struct ParticleParametersObject;
struct RandomizedSingle;

struct ParticleParametersInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "ParticleParameters"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine"; }

	virtual void InitializeInterops() override;

	static mono::string get_Comment(ParticleParametersObject *obj);
	static void set_Comment(ParticleParametersObject *obj, mono::string value);
	static bool get_Enabled(ParticleParametersObject *obj);
	static void set_Enabled(ParticleParametersObject *obj, bool value);
	static ParticleParams::ESpawn::E get_SpawnIndirection(ParticleParametersObject *obj);
	static void set_SpawnIndirection(ParticleParametersObject *obj, ParticleParams::ESpawn::E value);
	static RandomizedSingle get_Count(ParticleParametersObject *obj);
	static void set_Count(ParticleParametersObject *obj, RandomizedSingle value);
	static bool get_Continuous(ParticleParametersObject *obj);
	static void set_Continuous(ParticleParametersObject *obj, bool value);
	static RandomizedSingle get_SpawnDelay(ParticleParametersObject *obj);
	static void set_SpawnDelay(ParticleParametersObject *obj, RandomizedSingle value);
	static RandomizedSingle get_EmitterLifeTime(ParticleParametersObject *obj);
	static void set_EmitterLifeTime(ParticleParametersObject *obj, RandomizedSingle value);
	static RandomizedSingle get_PulseDelay(ParticleParametersObject *obj);
	static void set_PulseDelay(ParticleParametersObject *obj, RandomizedSingle value);
	static RandomizedSingle get_ParticleLifeTime(ParticleParametersObject *obj);
	static void set_ParticleLifeTime(ParticleParametersObject *obj, RandomizedSingle value);
	static bool get_RemainWhileVisible(ParticleParametersObject *obj);
	static void set_RemainWhileVisible(ParticleParametersObject *obj, bool value);
	static Vec3 get_PositionOffset(ParticleParametersObject *obj);
	static void set_PositionOffset(ParticleParametersObject *obj, Vec3 value);
	static Vec3 get_RandomOffset(ParticleParametersObject *obj);
	static void set_RandomOffset(ParticleParametersObject *obj, Vec3 value);
	static float get_OffsetRoundness(ParticleParametersObject *obj);
	static void set_OffsetRoundness(ParticleParametersObject *obj, float value);
	static float get_OffsetInnerFraction(ParticleParametersObject *obj);
	static void set_OffsetInnerFraction(ParticleParametersObject *obj, float value);
	static EGeomType get_AttachType(ParticleParametersObject *obj);
	static void set_AttachType(ParticleParametersObject *obj, EGeomType value);
	static EGeomForm get_AttachForm(ParticleParametersObject *obj);
	static void set_AttachForm(ParticleParametersObject *obj, EGeomForm value);
	static float get_FocusAngle(ParticleParametersObject *obj);
	static void set_FocusAngle(ParticleParametersObject *obj, float value);
	static float get_FocusAzimuth(ParticleParametersObject *obj);
	static void set_FocusAzimuth(ParticleParametersObject *obj, float value);

	static ParticleParams *Create();
	static void Delete(ParticleParams *handle);
};