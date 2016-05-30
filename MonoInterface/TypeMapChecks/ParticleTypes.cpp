#include "stdafx.h"

#include "CheckingBasics.h"
#include <CryParticleSystem/IParticles.h>

struct SGeomRef
{
	IMeshObj *m_pMeshObj;
	IPhysicalEntity* m_pPhysEnt;

	explicit SGeomRef(const GeomRef &other)
	{
		CHECK_TYPES_SIZE(SGeomRef, GeomRef);

		ASSIGN_FIELD(m_pMeshObj);
		ASSIGN_FIELD(m_pPhysEnt);

		CHECK_TYPE(m_pMeshObj);
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