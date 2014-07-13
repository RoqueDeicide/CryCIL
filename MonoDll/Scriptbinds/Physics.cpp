#include "StdAfx.h"
#include "Physics.h"

#include "MonoScriptSystem.h"

#include "MonoException.h"

#include <IEntitySystem.h>

#include <IMonoArray.h>
#include <IMonoObject.h>

CScriptbind_Physics::CScriptbind_Physics()
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

IPhysicalEntity *CScriptbind_Physics::GetPhysicalEntity(IEntity *pEntity)
{
	return pEntity->GetPhysics();
}

pe_type CScriptbind_Physics::GetPhysicalEntityType(IPhysicalEntity *pPhysEnt)
{
	return pPhysEnt->GetType();
}

void CScriptbind_Physics::Physicalize(IEntity *pEntity, SMonoPhysicalizeParams params)
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

void CScriptbind_Physics::Sleep(IPhysicalEntity *pPhysEnt, bool sleep)
{
	pe_action_awake awake;
	awake.bAwake = !sleep;

	pPhysEnt->Action(&awake);
}

Vec3 CScriptbind_Physics::GetVelocity(IPhysicalEntity *pPhysEnt)
{
	pe_status_dynamics sd;
	if (pPhysEnt->GetStatus(&sd) != 0)
		return sd.v;

	return Vec3(0, 0, 0);
}

void CScriptbind_Physics::SetVelocity(IPhysicalEntity *pPhysEnt, Vec3 vel)
{
	pe_action_set_velocity asv;
	asv.v = vel;

	pPhysEnt->Action(&asv);
}

int CScriptbind_Physics::RayWorldIntersection(Vec3 origin, Vec3 dir, int objFlags, unsigned int flags, int maxHits, mono::object skipEntities, mono::object &hits)
{
	IPhysicalEntity **pSkipEnts = NULL;
	int numSkipEnts = 0;

	if (skipEntities)
	{
		IMonoArray *pSkipEntities = *skipEntities;
		numSkipEnts = pSkipEntities->GetSize();

		pSkipEnts = new IPhysicalEntity*[numSkipEnts];

		for (int i = 0; i < numSkipEnts; i++)
		{
			IMonoObject *pItem = *pSkipEntities->GetItem(i);

#ifndef RELEASE
			if (!pItem)
				GetMonoScriptSystem()->GetCryBraryAssembly()->GetException("CryEngine", "NullPointerException")->Throw();
#endif

			pSkipEnts[i] = pItem->Unbox<IPhysicalEntity *>();
			SAFE_RELEASE(pItem);
		}

		pSkipEntities->Release();
	}

	ray_hit *pHits = new ray_hit[maxHits];
	int numHits = gEnv->pPhysicalWorld->RayWorldIntersection(origin, dir, objFlags, flags, pHits, maxHits, pSkipEnts, numSkipEnts);

	SAFE_DELETE_ARRAY(pSkipEnts);

	if (numHits > 0)
	{
		IMonoClass *pRayHitClass = GetMonoScriptSystem()->GetCryBraryAssembly()->GetClass("RaycastHit");

		IMonoArray *pRayHits = CreateMonoArray(numHits);//, pRayHitClass);
		for (int i = 0; i < numHits; i++)
			pRayHits->InsertMonoObject(pRayHitClass->BoxObject(&pHits[i]));

		hits = pRayHits->GetManagedObject();
		pRayHits->Release();
	}

	delete[] pHits;

	return numHits;
}

mono::object CScriptbind_Physics::SimulateExplosion(pe_explosion explosion)
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

bool CScriptbind_Physics::PhysicalEntityAction(IPhysicalEntity *pPhysEnt, pe_action &action)
{
	return pPhysEnt->Action(&action) != 0;
}

pe_status_dynamics GetDynamicsEntityStatus(IPhysicalEntity *pPhysEnt)
{
	pe_status_dynamics status;
	pPhysEnt->GetStatus(&status);

	return status;
}

bool CScriptbind_Physics::GetPhysicalEntityStatus(IPhysicalEntity *pPhysEnt, pe_status &status)
{
	return pPhysEnt->GetStatus(&status) != 0;
}

bool CScriptbind_Physics::SetPhysicalEntityParams(IPhysicalEntity *pPhysEnt, pe_params &params)
{
	return pPhysEnt->SetParams(&params) != 0;
}

bool CScriptbind_Physics::GetPhysicalEntityParams(IPhysicalEntity *pPhysEnt, pe_params &params)
{
	return pPhysEnt->GetParams(&params) != 0;
}