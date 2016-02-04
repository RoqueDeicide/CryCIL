#include "stdafx.h"

#include "CheckingBasics.h"
#include <IParticles.h>

typedef SpawnParams SParticleSpawnParams;

TYPE_MIRROR struct ParticleSpawnParams
{
	EGeomType			eAttachType;			// What type of object particles emitted from.
	EGeomForm			eAttachForm;			// What aspect of shape emitted from.
	bool					bCountPerUnit;		// Multiply particle count also by geometry extent (length/area/volume).
	bool					bEnableAudio;     // Used by particle effect instances to indicate whether audio should be updated or not.
	bool					bRegisterByBBox;	// Use the Bounding Box instead of Position to Register in VisArea
	float					fCountScale;			// Multiple for particle count (on top of bCountPerUnit if set).
	float					fSizeScale;				// Multiple for all effect sizes.
	float					fSpeedScale;			// Multiple for particle emission speed.
	float					fTimeScale;				// Multiple for emitter time evolution.
	float					fPulsePeriod;			// How often to restart emitter.
	float					fStrength;				// Controls parameter strength curves.
	stack_string	sAudioRTPC;				// Indicates what audio RTPC this particle effect instance drives.

	ParticleSpawnParams(SpawnParams other)
	{
		CHECK_TYPE_SIZE(ParticleSpawnParams);

		ASSIGN_FIELD(eAttachType);
		ASSIGN_FIELD(eAttachForm);
		ASSIGN_FIELD(bCountPerUnit);
		ASSIGN_FIELD(bEnableAudio);
		ASSIGN_FIELD(bRegisterByBBox);
		ASSIGN_FIELD(fCountScale);
		ASSIGN_FIELD(fSizeScale);
		ASSIGN_FIELD(fSpeedScale);
		ASSIGN_FIELD(fTimeScale);
		ASSIGN_FIELD(fPulsePeriod);
		ASSIGN_FIELD(fStrength);
		ASSIGN_FIELD(sAudioRTPC);

		CHECK_TYPE(eAttachType);
		CHECK_TYPE(eAttachForm);
		CHECK_TYPE(bCountPerUnit);
		CHECK_TYPE(bEnableAudio);
		CHECK_TYPE(bRegisterByBBox);
		CHECK_TYPE(fCountScale);
		CHECK_TYPE(fSizeScale);
		CHECK_TYPE(fSpeedScale);
		CHECK_TYPE(fTimeScale);
		CHECK_TYPE(fPulsePeriod);
		CHECK_TYPE(fStrength);
		CHECK_TYPE(sAudioRTPC);
	}
};

struct SGeomRef
{
	IStatObj*	m_pStatObj;
	ICharacterInstance* m_pChar;
	IPhysicalEntity* m_pPhysEnt;

	explicit SGeomRef(const GeomRef &other)
	{
		CHECK_TYPES_SIZE(SGeomRef, GeomRef);

		ASSIGN_FIELD(m_pStatObj);
		ASSIGN_FIELD(m_pChar);
		ASSIGN_FIELD(m_pPhysEnt);

		CHECK_TYPE(m_pStatObj);
		CHECK_TYPE(m_pChar);
		CHECK_TYPE(m_pPhysEnt);
	}
};

struct SEmitParticleData
{
	IStatObj*					pStatObj;				// The displayable geometry object for the entity. If NULL, uses emitter settings for sprite or geometry.
	IPhysicalEntity*	pPhysEnt;				// A physical entity which controls the particle. If NULL, uses emitter settings to physicalise or move particle.
	QuatTS						Location;				// Specified location for particle.
	Velocity3					Velocity;				// Specified linear and rotational velocity for particle.
	bool							bHasLocation;		// Location is specified.
	bool							bHasVel;

	explicit SEmitParticleData(const EmitParticleData &other)
	{
		CHECK_TYPES_SIZE(SEmitParticleData, EmitParticleData);

		ASSIGN_FIELD(pStatObj);
		ASSIGN_FIELD(pPhysEnt);
		ASSIGN_FIELD(Location);
		ASSIGN_FIELD(Velocity);
		ASSIGN_FIELD(bHasLocation);
		ASSIGN_FIELD(bHasVel);

		CHECK_TYPE(pStatObj);
		CHECK_TYPE(pPhysEnt);
		CHECK_TYPE(Location);
		CHECK_TYPE(Velocity);
		CHECK_TYPE(bHasLocation);
		CHECK_TYPE(bHasVel);
	}
};

struct SParticleTarget
{
	Vec3	vTarget;				// Target location for particles.
	Vec3	vVelocity;				// Velocity of moving target.
	float	fRadius;				// Size of target, for orbiting.

	// Options.
	bool	bTarget;				// Target is enabled.
	bool	bPriority;

	explicit SParticleTarget(const ParticleTarget &other)
	{
		CHECK_TYPES_SIZE(SParticleTarget, ParticleTarget);

		ASSIGN_FIELD(vTarget);
		ASSIGN_FIELD(vVelocity);
		ASSIGN_FIELD(fRadius);
		ASSIGN_FIELD(bTarget);
		ASSIGN_FIELD(bPriority);

		CHECK_TYPE(vTarget);
		CHECK_TYPE(vVelocity);
		CHECK_TYPE(fRadius);
		CHECK_TYPE(bTarget);
		CHECK_TYPE(bPriority);
	}
};