/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// PhysicalWorld scriptbind to implement necessary physical world methods,
// i.e. RayWorldIntersection.
//////////////////////////////////////////////////////////////////////////
// 13/01/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_PHYSICALWORLD__
#define __SCRIPTBIND_PHYSICALWORLD__

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

struct SMonoPhysicalizeParams
{
	int type;

	int flagsOR;
	int flagsAND;

	int slot;

	float density;
	float mass;

	int lod;

	EntityId attachToEntity;

	int attachToPart;

	float stiffnessScale;

	bool copyJointVelocities;

	pe_player_dimensions playerDim;
	pe_player_dynamics playerDyn;

	pe_params_particle particleParams;
};

class CScriptbind_Physics : public IMonoScriptBind
{
public:
	CScriptbind_Physics();
	~CScriptbind_Physics() {}

	// IMonoScriptBind
	virtual const char *GetClassName() { return "NativePhysicsMethods"; }
	// ~IMonoScriptBind

	static IPhysicalEntity *GetPhysicalEntity(IEntity *pEntity);
	static pe_type GetPhysicalEntityType(IPhysicalEntity *pPhysEnt);

	static void Physicalize(IEntity *pEntity, SMonoPhysicalizeParams params);

	static void Sleep(IPhysicalEntity *pPhysEnt, bool sleep);

	static Vec3 GetVelocity(IPhysicalEntity *pPhysEnt);
	static void SetVelocity(IPhysicalEntity *pPhysEnt, Vec3 vel);

	static int RayWorldIntersection(Vec3, Vec3, int, unsigned int, int, IPhysicalEntity **entitiesToSkip, int skipCount, ray_hit *hits);

	static mono::object SimulateExplosion(pe_explosion explosion);

	static bool PhysicalEntityAction(IPhysicalEntity *pPhysEnt, pe_action &action);

	static bool GetPhysicalEntityStatus(IPhysicalEntity *pPhysEnt, pe_status &status);

	static bool SetPhysicalEntityParams(IPhysicalEntity *pPhysEnt, pe_params &params);
	static bool GetPhysicalEntityParams(IPhysicalEntity *pPhysEnt, pe_params &params);
};

#endif //__SCRIPTBIND_PHYSICALWORLD__