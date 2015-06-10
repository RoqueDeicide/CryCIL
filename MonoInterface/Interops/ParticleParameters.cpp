#include "stdafx.h"

#include "ParticleParameters.h"

struct ParticleParametersObject
{
	MonoObject obj;
	ParticleParams *handle;
	bool unbound;
};
struct RandomizedSingle
{
	float _base;
	float range;
	template < class S >
	RandomizedSingle(TVarParam<S> p)
	{
		this->_base = (float)p.Base();
		this->range = (float)p.GetRandomRange();
	}
	template < class S >
	RandomizedSingle(TVarEParam<S> p)
	{
		this->_base = (float)p;
		this->range = (float)p.GetRandomRange();
	}
};

void ParticleParametersInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_Comment);
	REGISTER_METHOD(set_Comment);
	REGISTER_METHOD(get_Enabled);
	REGISTER_METHOD(set_Enabled);
	REGISTER_METHOD(get_SpawnIndirection);
	REGISTER_METHOD(set_SpawnIndirection);
	REGISTER_METHOD(get_Count);
	REGISTER_METHOD(set_Count);
	REGISTER_METHOD(get_Continuous);
	REGISTER_METHOD(set_Continuous);
	REGISTER_METHOD(get_SpawnDelay);
	REGISTER_METHOD(set_SpawnDelay);
	REGISTER_METHOD(get_EmitterLifeTime);
	REGISTER_METHOD(set_EmitterLifeTime);
	REGISTER_METHOD(get_PulseDelay);
	REGISTER_METHOD(set_PulseDelay);
	REGISTER_METHOD(get_ParticleLifeTime);
	REGISTER_METHOD(set_ParticleLifeTime);
	REGISTER_METHOD(get_RemainWhileVisible);
	REGISTER_METHOD(set_RemainWhileVisible);
	REGISTER_METHOD(get_PositionOffset);
	REGISTER_METHOD(set_PositionOffset);
	REGISTER_METHOD(get_RandomOffset);
	REGISTER_METHOD(set_RandomOffset);
	REGISTER_METHOD(get_OffsetRoundness);
	REGISTER_METHOD(set_OffsetRoundness);
	REGISTER_METHOD(get_OffsetInnerFraction);
	REGISTER_METHOD(set_OffsetInnerFraction);
	REGISTER_METHOD(get_AttachType);
	REGISTER_METHOD(set_AttachType);
	REGISTER_METHOD(get_AttachForm);
	REGISTER_METHOD(set_AttachForm);
	REGISTER_METHOD(get_FocusAngle);
	REGISTER_METHOD(set_FocusAngle);
	REGISTER_METHOD(get_FocusAzimuth);
	REGISTER_METHOD(set_FocusAzimuth);

	REGISTER_METHOD(Create);
	REGISTER_METHOD(Delete);
}

mono::string ParticleParametersInterop::get_Comment(ParticleParametersObject *obj)
{
	return ToMonoString(obj->handle->sComment);
}

void ParticleParametersInterop::set_Comment(ParticleParametersObject *obj, mono::string value)
{
	obj->handle->sComment.assign(NtText(value));
}

bool ParticleParametersInterop::get_Enabled(ParticleParametersObject *obj)
{
	return obj->handle->bEnabled;
}

void ParticleParametersInterop::set_Enabled(ParticleParametersObject *obj, bool value)
{
	obj->handle->bEnabled = value;
}

ParticleParams::ESpawn::E ParticleParametersInterop::get_SpawnIndirection(ParticleParametersObject *obj)
{
	return obj->handle->eSpawnIndirection;
}

void ParticleParametersInterop::set_SpawnIndirection(ParticleParametersObject *obj, ParticleParams::ESpawn::E value)
{
	obj->handle->eSpawnIndirection = value;
}

RandomizedSingle ParticleParametersInterop::get_Count(ParticleParametersObject *obj)
{
	return obj->handle->fCount;
}

void ParticleParametersInterop::set_Count(ParticleParametersObject *obj, RandomizedSingle value)
{
	obj->handle->fCount.Set(value._base, value.range);
}

bool ParticleParametersInterop::get_Continuous(ParticleParametersObject *obj)
{
	return obj->handle->bContinuous;
}

void ParticleParametersInterop::set_Continuous(ParticleParametersObject *obj, bool value)
{
	obj->handle->bContinuous = value;
}

RandomizedSingle ParticleParametersInterop::get_SpawnDelay(ParticleParametersObject *obj)
{
	return obj->handle->fSpawnDelay;
}

void ParticleParametersInterop::set_SpawnDelay(ParticleParametersObject *obj, RandomizedSingle value)
{
	obj->handle->fSpawnDelay.Set(value._base, value.range);
}

RandomizedSingle ParticleParametersInterop::get_EmitterLifeTime(ParticleParametersObject *obj)
{
	return obj->handle->fEmitterLifeTime;
}

void ParticleParametersInterop::set_EmitterLifeTime(ParticleParametersObject *obj, RandomizedSingle value)
{
	obj->handle->fEmitterLifeTime.Set(value._base, value.range);
}

RandomizedSingle ParticleParametersInterop::get_PulseDelay(ParticleParametersObject *obj)
{
	return obj->handle->fPulsePeriod;
}

void ParticleParametersInterop::set_PulseDelay(ParticleParametersObject *obj, RandomizedSingle value)
{
	obj->handle->fPulsePeriod.Set(value._base, value.range);
}

RandomizedSingle ParticleParametersInterop::get_ParticleLifeTime(ParticleParametersObject *obj)
{
	return obj->handle->fParticleLifeTime;
}

void ParticleParametersInterop::set_ParticleLifeTime(ParticleParametersObject *obj, RandomizedSingle value)
{
	obj->handle->fParticleLifeTime.Set(value._base, value.range);
}

bool ParticleParametersInterop::get_RemainWhileVisible(ParticleParametersObject *obj)
{
	return obj->handle->bRemainWhileVisible;
}

void ParticleParametersInterop::set_RemainWhileVisible(ParticleParametersObject *obj, bool value)
{
	obj->handle->bRemainWhileVisible = value;
}

Vec3 ParticleParametersInterop::get_PositionOffset(ParticleParametersObject *obj)
{
	return obj->handle->vPositionOffset;
}

void ParticleParametersInterop::set_PositionOffset(ParticleParametersObject *obj, Vec3 value)
{
	obj->handle->vPositionOffset = value;
}

Vec3 ParticleParametersInterop::get_RandomOffset(ParticleParametersObject *obj)
{
	return obj->handle->vRandomOffset;
}

void ParticleParametersInterop::set_RandomOffset(ParticleParametersObject *obj, Vec3 value)
{
	obj->handle->vRandomOffset = value;
}

float ParticleParametersInterop::get_OffsetRoundness(ParticleParametersObject *obj)
{
	return obj->handle->fOffsetRoundness;
}

void ParticleParametersInterop::set_OffsetRoundness(ParticleParametersObject *obj, float value)
{
	obj->handle->fOffsetRoundness = value;
}

float ParticleParametersInterop::get_OffsetInnerFraction(ParticleParametersObject *obj)
{
	return obj->handle->fOffsetInnerFraction;
}

void ParticleParametersInterop::set_OffsetInnerFraction(ParticleParametersObject *obj, float value)
{
	obj->handle->fOffsetInnerFraction = value;
}

EGeomType ParticleParametersInterop::get_AttachType(ParticleParametersObject *obj)
{
	return obj->handle->eAttachType;
}

void ParticleParametersInterop::set_AttachType(ParticleParametersObject *obj, EGeomType value)
{
	obj->handle->eAttachType = value;
}

EGeomForm ParticleParametersInterop::get_AttachForm(ParticleParametersObject *obj)
{
	return obj->handle->eAttachForm;
}

void ParticleParametersInterop::set_AttachForm(ParticleParametersObject *obj, EGeomForm value)
{
	obj->handle->eAttachForm = value;
}

float ParticleParametersInterop::get_FocusAngle(ParticleParametersObject *obj)
{
	return obj->handle->fFocusAngle;
}

void ParticleParametersInterop::set_FocusAngle(ParticleParametersObject *obj, float value)
{
	obj->handle->fFocusAngle = value;
}

float ParticleParametersInterop::get_FocusAzimuth(ParticleParametersObject *obj)
{
	return obj->handle->fFocusAzimuth;
}

void ParticleParametersInterop::set_FocusAzimuth(ParticleParametersObject *obj, float value)
{
	obj->handle->fFocusAzimuth = value;
}

ParticleParams *ParticleParametersInterop::Create()
{
	return new ParticleParams();
}

void ParticleParametersInterop::Delete(ParticleParams *handle)
{
	if (handle) delete handle;
}
