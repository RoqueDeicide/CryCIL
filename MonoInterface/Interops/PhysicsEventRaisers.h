#pragma once

#include "IMonoInterface.h"

//
// Mirrors for C# structures.
//

struct ForeignData
{
	void *handle;
	int id;
};

struct MonoPhysicsEventData
{
	IPhysicalEntity *entity;
	ForeignData foreignData;
};

struct StereoPhysicsEventData
{
	IPhysicalEntity *firstEntity;
	IPhysicalEntity *secondEntity;
	ForeignData firstForeignData;
	ForeignData secondForeignData;
};

struct CollisionInfo
{
	int idCollider;
	Vec3 point;
	Vec3 normal;
	float penetration;
	float impulse;
	float size;
	float decalSize;
};

struct CollisionParticipantInfo
{
	Vec3 velocity;
	float mass;
	int partId;
	short matId;
	short iPrim;
};

struct MeshUpdateInfoProvider
{
	IGeometry *geometry;
	bop_meshupdate *lastUpdate;
};

struct CreatedPartInfo
{
	IPhysicalEntity *newEntity;
	int oldPartId;
	int newPartId;
	int totalPartCount;
	bool invalid;
	int reason;
	Vec3 breakageImpulse;
	Ang3 breakageAngularImpulse;
	Vec3 ejectionVelocity;
	Ang3 ejectionAngularVelocity;
	float breakageSize;
	float cutRadius;
	Vec3 cutSourcePosition;
	Vec3 cutPartPosition;
	Vec3 cutSourceNormal;
	Vec3 cutPartNormal;
	IGeometry *newMesh;
	MeshUpdateInfoProvider meshUpdates;
};

struct JointBreakInfo
{
	int jointId;
	bool isJoint;
	int partIdEpicenter;
	Vec3 point;
	Vec3 zAxis;
	int sourcePartId;
	int targetPartId;
	int sourcePartMaterial;
	int targetPartMaterial;
	IPhysicalEntity *sourceEntity;
	IPhysicalEntity *targetEntity;
};

struct PhysicalEntityStateInfo
{
	int simClass;
	AABB boundingBox;
};

struct TimeStepInfo
{
	float dt;
	Vec3 pos;
	quaternionf q;
	int idStep;
};

//
// Type defs for signatures of methods that raise events on C# side.
//

typedef bool(__stdcall *BBoxOverlapPhysicsEventThunk)(StereoPhysicsEventData *, bool, mono::exception *);
typedef bool(__stdcall *CollisionPhysicsEventThunk)(StereoPhysicsEventData *, CollisionInfo *, CollisionParticipantInfo *,
													CollisionParticipantInfo *, bool, mono::exception *);
typedef bool(__stdcall *StateChangePhysicsEventThunk)(MonoPhysicsEventData *, PhysicalEntityStateInfo *,
													  PhysicalEntityStateInfo *, float, bool, mono::exception *);
typedef bool(__stdcall *EnvChangePhysicsEventThunk)(MonoPhysicsEventData *, IPhysicalEntity *, IPhysicalEntity *, bool,
													mono::exception *);
typedef bool(__stdcall *PostStepPhysicsEventThunk)(MonoPhysicsEventData *, TimeStepInfo *, bool, mono::exception *);
typedef bool(__stdcall *UpdateMeshPhysicsEventThunk)(MonoPhysicsEventData *, int, bool, int, IGeometry *,
													 MeshUpdateInfoProvider *, Matrix34 *, IGeometry *, bool,
													 mono::exception *);
typedef bool(__stdcall *CreateEntityPartPhysicsEventThunk)(MonoPhysicsEventData *, CreatedPartInfo *, bool,
														   mono::exception *);
typedef bool(__stdcall *RevealEntityPartPhysicsEventThunk)(MonoPhysicsEventData *, int, bool, mono::exception *);
typedef bool(__stdcall *JointBrokenPhysicsEventThunk)(StereoPhysicsEventData *, JointBreakInfo *, bool, mono::exception *);
typedef bool(__stdcall *EntityDeletedPhysicsEventThunk)(MonoPhysicsEventData *, int, bool, mono::exception *);

//
// Macros that make definitions of physics event clients shorter.
//

#define BEGIN_STEREO_EVENT_CLIENT(clientName, thunkType, eventType, eventName) \
inline int clientName(const EventPhys*_event, bool logged) \
{ \
	static thunkType raise = thunkType(MonoEnv->Cryambly->GetClass("CryCil.Engine.Physics", "PhysicalWorld") \
														->GetEvent(eventName)->Raise->UnmanagedThunk); \
		 \
	mono::exception ex; \
	const eventType *_eventInfo = static_cast<const eventType *>(_event); \
	 \
	StereoPhysicsEventData data; \
	data.firstEntity              = _eventInfo->pEntity[0]; \
	data.firstForeignData.handle  = _eventInfo->pForeignData[0]; \
	data.firstForeignData.id      = _eventInfo->iForeignData[0]; \
	data.secondEntity             = _eventInfo->pEntity[1]; \
	data.secondForeignData.handle = _eventInfo->pForeignData[1]; \
	data.secondForeignData.id     = _eventInfo->iForeignData[1];

#define BEGIN_MONO_EVENT_CLIENT(clientName, thunkType, eventType, eventName) \
inline int clientName(const EventPhys*_event, bool logged) \
{ \
	static thunkType raise = thunkType(MonoEnv->Cryambly->GetClass("CryCil.Engine.Physics", "PhysicalWorld") \
														->GetEvent(eventName)->Raise->UnmanagedThunk); \
		 \
	mono::exception ex; \
	const eventType *_eventInfo = static_cast<const eventType *>(_event); \
	 \
	MonoPhysicsEventData data; \
	data.entity             = _eventInfo->pEntity; \
	data.foreignData.handle = _eventInfo->pForeignData; \
	data.foreignData.id     = _eventInfo->iForeignData;

#define END_EVENT_CLIENT return result ? 1 : 0; \
}

//
// Definitions of event clients.
//

BEGIN_STEREO_EVENT_CLIENT(BBoxOverlapEventClient, BBoxOverlapPhysicsEventThunk, EventPhysBBoxOverlap, "BoundingBoxOverlapped")
auto result = raise(&data, logged, &ex);
END_EVENT_CLIENT

BEGIN_STEREO_EVENT_CLIENT(CollisionEventClient, CollisionPhysicsEventThunk, EventPhysCollision, "CollisionHappened")
CollisionInfo collisionInfo;
collisionInfo.decalSize   = _eventInfo->fDecalPlacementTestMaxSize;
collisionInfo.idCollider  = _eventInfo->idCollider;
collisionInfo.impulse     = _eventInfo->normImpulse;
collisionInfo.normal      = _eventInfo->n;
collisionInfo.penetration = _eventInfo->penetration;
collisionInfo.point       = _eventInfo->pt;
collisionInfo.size        = _eventInfo->radius;

CollisionParticipantInfo collider;
collider.iPrim    = _eventInfo->iPrim[0];
collider.mass     = _eventInfo->mass[0];
collider.matId    = _eventInfo->idmat[0];
collider.partId   = _eventInfo->partid[0];
collider.velocity = _eventInfo->vloc[0];

CollisionParticipantInfo collidee;
collidee.iPrim    = _eventInfo->iPrim[1];
collidee.mass     = _eventInfo->mass[1];
collidee.matId    = _eventInfo->idmat[1];
collidee.partId   = _eventInfo->partid[1];
collidee.velocity = _eventInfo->vloc[1];

auto result = raise(&data, &collisionInfo, &collider, &collidee, logged, &ex);
END_EVENT_CLIENT

BEGIN_MONO_EVENT_CLIENT(StateChangeEventClient, StateChangePhysicsEventThunk, EventPhysStateChange, "EntityStateChanged")
PhysicalEntityStateInfo oldState;
oldState.simClass    = _eventInfo->iSimClass[0];
oldState.boundingBox = AABB(_eventInfo->BBoxOld[0], _eventInfo->BBoxOld[1]);

PhysicalEntityStateInfo newState;
newState.simClass = _eventInfo->iSimClass[1];
newState.boundingBox = AABB(_eventInfo->BBoxNew[0], _eventInfo->BBoxNew[1]);

auto result = raise(&data, &oldState, &newState, _eventInfo->timeIdle, logged, &ex);
END_EVENT_CLIENT

BEGIN_MONO_EVENT_CLIENT(EnvChangeEventClient, EnvChangePhysicsEventThunk, EventPhysEnvChange, "EnvironmentChanged")
auto result = raise(&data, _eventInfo->pentSrc, _eventInfo->pentNew, logged, &ex);
END_EVENT_CLIENT

BEGIN_MONO_EVENT_CLIENT(PostStepEventClient, PostStepPhysicsEventThunk, EventPhysPostStep, "StepComplete")
TimeStepInfo step;
step.dt     = _eventInfo->dt;
step.idStep = _eventInfo->idStep;
step.pos    = _eventInfo->pos;
step.q      = _eventInfo->q;

auto result = raise(&data, &step, logged, &ex);
END_EVENT_CLIENT

BEGIN_MONO_EVENT_CLIENT(UpdateMeshEventClient, UpdateMeshPhysicsEventThunk, EventPhysUpdateMesh, "MeshChanged")
MeshUpdateInfoProvider updatesInfo;
updatesInfo.geometry   = _eventInfo->pMesh;
updatesInfo.lastUpdate = _eventInfo->pLastUpdate;

Matrix34 m = _eventInfo->mtxSkelToMesh;

auto result = raise(&data, _eventInfo->partid, _eventInfo->bInvalid != 0, _eventInfo->iReason, _eventInfo->pMesh,
					&updatesInfo, &m, _eventInfo->pMeshSkel, logged, &ex);
END_EVENT_CLIENT

BEGIN_MONO_EVENT_CLIENT(CreateEntityPartEventClient, CreateEntityPartPhysicsEventThunk, EventPhysCreateEntityPart, "PartCreated")
CreatedPartInfo partInfo;
partInfo.breakageAngularImpulse  = Ang3(_eventInfo->breakAngImpulse);
partInfo.breakageImpulse         = _eventInfo->breakImpulse;
partInfo.breakageSize            = _eventInfo->breakSize;
partInfo.cutSourceNormal         = _eventInfo->cutDirLoc[0];
partInfo.cutSourcePosition       = _eventInfo->cutPtLoc[0];
partInfo.cutPartNormal           = _eventInfo->cutDirLoc[1];
partInfo.cutPartPosition         = _eventInfo->cutPtLoc[1];
partInfo.cutRadius               = _eventInfo->cutRadius;
partInfo.ejectionAngularVelocity = Ang3(_eventInfo->w);
partInfo.ejectionVelocity        = _eventInfo->v;
partInfo.invalid                 = _eventInfo->bInvalid != 0;
partInfo.meshUpdates.geometry    = _eventInfo->pMeshNew;
partInfo.meshUpdates.lastUpdate  = _eventInfo->pLastUpdate;
partInfo.newEntity               = _eventInfo->pEntNew;
partInfo.newMesh                 = _eventInfo->pMeshNew;
partInfo.newPartId               = _eventInfo->partidNew;
partInfo.oldPartId               = _eventInfo->partidSrc;
partInfo.reason                  = _eventInfo->iReason;
partInfo.totalPartCount          = _eventInfo->nTotParts;

auto result = raise(&data, &partInfo, logged, &ex);
END_EVENT_CLIENT

BEGIN_MONO_EVENT_CLIENT(RevealEntityPartEventClient, RevealEntityPartPhysicsEventThunk, EventPhysRevealEntityPart, "PartRevealed")
auto result = raise(&data, _eventInfo->partId, logged, &ex);
END_EVENT_CLIENT

BEGIN_STEREO_EVENT_CLIENT(JointBrokenEventClient, JointBrokenPhysicsEventThunk, EventPhysJointBroken, "JointBroken")
JointBreakInfo info;
info.isJoint            = _eventInfo->bJoint != 0;
info.jointId            = _eventInfo->idJoint;
info.partIdEpicenter    = _eventInfo->partidEpicenter;
info.point              = _eventInfo->pt;
info.sourceEntity       = _eventInfo->pNewEntity[0];
info.targetEntity       = _eventInfo->pNewEntity[1];
info.sourcePartId       = _eventInfo->partid[0];
info.targetPartId       = _eventInfo->partid[1];
info.sourcePartMaterial = _eventInfo->partmat[0];
info.targetPartMaterial = _eventInfo->partmat[1];
info.zAxis              = _eventInfo->n;

auto result = raise(&data, &info, logged, &ex);
END_EVENT_CLIENT

BEGIN_MONO_EVENT_CLIENT(EntityDeletedEventClient, EntityDeletedPhysicsEventThunk, EventPhysEntityDeleted, "EntityDeleted")
auto result = raise(&data, _eventInfo->mode, logged, &ex);
END_EVENT_CLIENT

//
// Helpers for registration/unregistration of event clients in physical world.
//

typedef int(*EventClientFunc)(const EventPhys*, bool logged);

template<EventClientFunc func, bool logged>
inline int InvokeEventClient(const EventPhys *_event)
{
	return func(_event, logged);
}

#define ADD_EVENT_CLIENT(type, func) \
	gEnv->pPhysicalWorld->AddEventClient(type, InvokeEventClient<func, false>, 0); \
	gEnv->pPhysicalWorld->AddEventClient(type, InvokeEventClient<func, true>, 1);

#define REMOVE_EVENT_CLIENT(type, func) \
	gEnv->pPhysicalWorld->RemoveEventClient(type, InvokeEventClient<func, false>, 0); \
	gEnv->pPhysicalWorld->RemoveEventClient(type, InvokeEventClient<func, true>, 1);


inline void RegisterEventClients()
{
	ADD_EVENT_CLIENT(EventPhysBBoxOverlap::id,       BBoxOverlapEventClient);
	ADD_EVENT_CLIENT(EventPhysCollision::id,         CollisionEventClient);
	ADD_EVENT_CLIENT(EventPhysStateChange::id,       StateChangeEventClient);
	ADD_EVENT_CLIENT(EventPhysEnvChange::id,         EnvChangeEventClient);
	ADD_EVENT_CLIENT(EventPhysPostStep::id,          PostStepEventClient);
	ADD_EVENT_CLIENT(EventPhysUpdateMesh::id,        UpdateMeshEventClient);
	ADD_EVENT_CLIENT(EventPhysCreateEntityPart::id,  CreateEntityPartEventClient);
	ADD_EVENT_CLIENT(EventPhysRevealEntityPart::id,  RevealEntityPartEventClient);
	ADD_EVENT_CLIENT(EventPhysJointBroken::id,       JointBrokenEventClient);
	ADD_EVENT_CLIENT(EventPhysEntityDeleted::id,     EntityDeletedEventClient);
	//ADD_EVENT_CLIENT(EventPhysRemoveEntityParts::id, RemoveEntityPartsEventClient);
}

inline void UnregisterEventClients()
{
	REMOVE_EVENT_CLIENT(EventPhysBBoxOverlap::id,       BBoxOverlapEventClient);
	REMOVE_EVENT_CLIENT(EventPhysCollision::id,         CollisionEventClient);
	REMOVE_EVENT_CLIENT(EventPhysStateChange::id,       StateChangeEventClient);
	REMOVE_EVENT_CLIENT(EventPhysEnvChange::id,         EnvChangeEventClient);
	REMOVE_EVENT_CLIENT(EventPhysPostStep::id,          PostStepEventClient);
	REMOVE_EVENT_CLIENT(EventPhysUpdateMesh::id,        UpdateMeshEventClient);
	REMOVE_EVENT_CLIENT(EventPhysCreateEntityPart::id,  CreateEntityPartEventClient);
	REMOVE_EVENT_CLIENT(EventPhysRevealEntityPart::id,  RevealEntityPartEventClient);
	REMOVE_EVENT_CLIENT(EventPhysJointBroken::id,       JointBrokenEventClient);
	REMOVE_EVENT_CLIENT(EventPhysEntityDeleted::id,     EntityDeletedEventClient);
	////REMOVE_EVENT_CLIENT(EventPhysRemoveEntityParts::id, RemoveEntityPartsEventClient);
}