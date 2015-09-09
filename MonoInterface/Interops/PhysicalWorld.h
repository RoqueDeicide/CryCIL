#pragma once

#include "IMonoInterface.h"

struct ForeignData;
struct WaterManagerParameters;
struct ExplosionResult;
struct ExplosionParameters;

struct PhysicalWorldInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "PhysicalWorld"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Physics"; }

	virtual void OnRunTimeInitialized() override;
	virtual void Shutdown() override;

	static ExplosionResult SimulateExplosion(const ExplosionParameters &parameters, mono::Array entitiesToSkip, int types);
	static int             AddExplosionShape(IGeometry *shape, float size, int index, float probability = 1.0f);
	static void            RemoveExplosionShape(int index);
	static void            RemoveAllExplosionShapes();
	static void            GetWaterManagerParameters(WaterManagerParameters &parameters);
	static void            SetWaterManagerParameters(const WaterManagerParameters &parameters);
	static int             PrimitiveIntersectionInternal(mono::Array *contacts, primitives::primitive *primitive,
														 int primitiveType, int queryFlags, int flagsAll, int flagsAny,
														 intersection_params *parameters, SCollisionClass collisionClass,
														 IPhysicalEntity **entitiesToSkip, int skipCount);
	static float           PrimitiveCastInternal(geom_contact *contact, primitives::primitive *primitive,
												 int primitiveType, Vec3 *sweepDirection, int queryFlags, int flagsAll,
												 int flagsAny, intersection_params *parameters,
												 SCollisionClass collisionClass, IPhysicalEntity **entitiesToSkip,
												 int skipCount);
	static IPhysicalEntity *CreatePhysicalEntity(pe_type type, pe_params *initialParameters, ForeignData foreignData,
												 int id);
	static IPhysicalEntity *CreatePhysicalEntityNoParams(pe_type type, ForeignData foreignData, int id);
	static IPhysicalEntity *CreatePhysicalEntityFromHolder(pe_type type, float lifeTime, pe_params *initialParameters,
														   ForeignData foreignData, int id, IPhysicalEntity *placeHolder);
	static IPhysicalEntity *CreatePhysicalEntityNoParamsFromHolder(pe_type type, float lifeTime, ForeignData foreignData,
																   int id, IPhysicalEntity *placeHolder);
	static IPhysicalEntity *CreatePlaceHolder(pe_type type, pe_params *initialParameters, ForeignData foreignData, int id);
	static IPhysicalEntity *CreatePlaceHolderNoParams(pe_type type, ForeignData foreignData, int id);
	static int              DestroyPhysicalEntity(IPhysicalEntity *pent, int mode, int bThreadSafe);
};