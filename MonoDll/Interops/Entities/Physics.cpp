#include "StdAfx.h"
#include "Physics.h"

#include "MonoScriptSystem.h"

#include "MonoException.h"

#include <IEntitySystem.h>

#include <IMonoArray.h>
#include <IMonoObject.h>

PhysicsInterop::PhysicsInterop()
{
	REGISTER_METHOD(GetPhysicalEntity);
	REGISTER_METHOD(GetPhysicalEntityType);

	REGISTER_METHOD(Physicalize);
	REGISTER_METHOD(Sleep);

	REGISTER_METHOD(GetVelocity);
	REGISTER_METHOD(SetVelocity);

	REGISTER_METHOD(RayWorldIntersection);

	REGISTER_METHOD(SimulateExplosion);

	REGISTER_METHOD_NAME(PhysicalEntityAction, "ActionImpulse");

	REGISTER_METHOD_NAME(GetPhysicalEntityStatus, "GetLivingEntityStatus");
	REGISTER_METHOD_NAME(GetPhysicalEntityStatus, "GetDynamicsEntityStatus");

	REGISTER_METHOD_NAME(SetPhysicalEntityParams, "SetParticleParams");
	REGISTER_METHOD_NAME(GetPhysicalEntityParams, "GetParticleParams");

	REGISTER_METHOD_NAME(SetPhysicalEntityParams, "SetFlagParams");
	REGISTER_METHOD_NAME(GetPhysicalEntityParams, "GetFlagParams");

	REGISTER_METHOD_NAME(SetPhysicalEntityParams, "SetSimulationParams");
	REGISTER_METHOD_NAME(GetPhysicalEntityParams, "GetSimulationParams");
}

IPhysicalEntity *PhysicsInterop::GetPhysicalEntity(IEntity *pEntity)
{
	return pEntity->GetPhysics();
}

pe_type PhysicsInterop::GetPhysicalEntityType(IPhysicalEntity *pPhysEnt)
{
	return pPhysEnt->GetType();
}

void PhysicsInterop::Physicalize(IEntity *pEntity, SMonoPhysicalizeParams params)
{
	// Unphysicalize
	{
		const Ang3 oldRotation = pEntity->GetWorldAngles();
		const Quat newRotation = Quat::CreateRotationZ(oldRotation.z);
		pEntity->SetRotation(newRotation);

		SEntityPhysicalizeParams pp;
		pp.type = PE_NONE;
		pEntity->Physicalize(pp);
	}
	// ~Unphysicalize

	SEntityPhysicalizeParams pp;

	pp.bCopyJointVelocities = params.copyJointVelocities;
	pp.density = params.density;
	pp.fStiffnessScale = params.stiffnessScale;
	pp.mass = params.mass;
	pp.nAttachToPart = params.attachToPart;
	pp.nLod = params.lod;
	pp.nSlot = params.slot;
	pp.type = params.type;

	pp.nFlagsOR = params.flagsOR;
	pp.nFlagsAND = params.flagsAND;

	if (params.attachToEntity != 0)
	{
		if (IPhysicalEntity *pPhysEnt = gEnv->pPhysicalWorld->GetPhysicalEntityById(params.attachToEntity))
			pp.pAttachToEntity = pPhysEnt;
	}

	if (pp.type == PE_LIVING)
	{
		pp.pPlayerDimensions = &params.playerDim;
		pp.pPlayerDynamics = &params.playerDyn;
	}
	else if (pp.type == PE_PARTICLE)
		pp.pParticle = &params.particleParams;

	pEntity->Physicalize(pp);
}

void PhysicsInterop::Sleep(IPhysicalEntity *pPhysEnt, bool sleep)
{
	pe_action_awake awake;
	awake.bAwake = !sleep;

	pPhysEnt->Action(&awake);
}

Vec3 PhysicsInterop::GetVelocity(IPhysicalEntity *pPhysEnt)
{
	pe_status_dynamics sd;
	if (pPhysEnt->GetStatus(&sd) != 0)
		return sd.v;

	return Vec3(0, 0, 0);
}

void PhysicsInterop::SetVelocity(IPhysicalEntity *pPhysEnt, Vec3 vel)
{
	pe_action_set_velocity asv;
	asv.v = vel;

	pPhysEnt->Action(&asv);
}
// Returns number of hits.
int PhysicsInterop::RayWorldIntersection
(
	Vec3 origin,						// Origin of the ray cast.
	Vec3 dir,							// Direction of ray cast.
	int objFlags,						// Object query flags.
	unsigned int flags,					// Intersection detection flags.
	int maxHits,						// Maximal number of hits.
	IPhysicalEntity **entitiesToSkip,	// Pointer to entities to skip.
	int skipCount,						// Number of entities to skip.
	ray_hit *hits						// Pointer to hits.
)
{
	return gEnv->pPhysicalWorld->RayWorldIntersection(origin, dir, objFlags, flags, hits, maxHits, entitiesToSkip, skipCount);
}

mono::object PhysicsInterop::SimulateExplosion(pe_explosion explosion)
{
	gEnv->pPhysicalWorld->SimulateExplosion(&explosion);

	if (explosion.nAffectedEnts > 0)
	{
		IMonoArray *pAffectedEnts = CreateMonoArray(explosion.nAffectedEnts);

		for (int i = 0; i < explosion.nAffectedEnts; i++)
			pAffectedEnts->InsertNativePointer(explosion.pAffectedEnts[i]);

		pAffectedEnts->Release();
		return pAffectedEnts->GetManagedObject();
	}

	return NULL;
}

bool PhysicsInterop::PhysicalEntityAction(IPhysicalEntity *pPhysEnt, pe_action &action)
{
	return pPhysEnt->Action(&action) != 0;
}

pe_status_dynamics GetDynamicsEntityStatus(IPhysicalEntity *pPhysEnt)
{
	pe_status_dynamics status;
	pPhysEnt->GetStatus(&status);

	return status;
}

bool PhysicsInterop::GetPhysicalEntityStatus(IPhysicalEntity *pPhysEnt, pe_status &status)
{
	return pPhysEnt->GetStatus(&status) != 0;
}

bool PhysicsInterop::SetPhysicalEntityParams(IPhysicalEntity *pPhysEnt, pe_params &params)
{
	return pPhysEnt->SetParams(&params) != 0;
}

bool PhysicsInterop::GetPhysicalEntityParams(IPhysicalEntity *pPhysEnt, pe_params &params)
{
	return pPhysEnt->GetParams(&params) != 0;
}