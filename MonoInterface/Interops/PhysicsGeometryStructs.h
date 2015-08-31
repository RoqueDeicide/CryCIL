#pragma once

#include "IMonoInterface.h"

struct GeometryParameters
{
	bool initialized;
	int type;
	float density;
	float mass;
	Vec3 pos;
	Quat q;
	float scale;
	Matrix34 pMtx3x4;
	int surface_idx;
	unsigned int flags;
	unsigned int flagsCollider;
	float minContactDist;
	int idmatBreakable;
	ITetrLattice *pLattice;
	mono::Array matMappings;
	int* pMatMapping;
	int nMats;
	int bRecalcBBox;

	void ToGeomParams(pe_geomparams &params)
	{
		params.bRecalcBBox    = this->bRecalcBBox;
		params.density        = this->density;
		params.flags          = this->flags;
		params.flagsCollider  = this->flagsCollider;
		params.idmatBreakable = this->idmatBreakable;
		params.mass           = this->mass;
		params.minContactDist = this->minContactDist;
		params.nMats          = this->nMats;
		params.pLattice       = this->pLattice;
		params.pMatMapping    = this->pMatMapping;
		params.pMtx3x4        = is_unused(this->pMtx3x4.m00) ? nullptr : &this->pMtx3x4;
		params.pos            = this->pos;
		params.q              = this->q;
		params.scale          = this->scale;
		params.surface_idx    = this->surface_idx;
	}
};

struct GeometryParametersArticulatedBody
{
	GeometryParameters Base;
	int idBody;

	void ToGeomParams(pe_articgeomparams &params)
	{
		this->Base.ToGeomParams(params);
		params.idbody = this->idBody;
	}
};

struct GeometryParametersVehicle
{
	GeometryParameters Base;
	int bDriving;
	int iAxle;
	int bCanBrake;
	int bRayCast;
	int bCanSteer;
	Vec3 pivot;
	float lenMax;
	float lenInitial;
	float kStiffness;
	float kStiffnessWeight;
	float kDamping;
	float minFriction, maxFriction;
	float kLatFriction;

	void ToGeomParams(pe_cargeomparams &params)
	{
		this->Base.ToGeomParams(params);
		params.bDriving         = this->bDriving;
		params.iAxle            = this->iAxle;
		params.bCanBrake        = this->bCanBrake;
		params.bRayCast         = this->bRayCast;
		params.bCanSteer        = this->bCanSteer;
		params.pivot            = this->pivot;
		params.lenMax           = this->lenMax;
		params.lenInitial       = this->lenInitial;
		params.kStiffness       = this->kStiffness;
		params.kStiffnessWeight = this->kStiffnessWeight;
		params.kDamping         = this->kDamping;
		params.minFriction      = this->minFriction;
		params.maxFriction      = this->maxFriction;
		params.kLatFriction     = this->kLatFriction;
	}
};