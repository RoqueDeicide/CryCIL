#pragma once

#include "IMonoInterface.h"

inline int BBoxOverlapEventClient(const EventPhys*_event, bool logged)
{
	
}
inline int CollisionEventClient(const EventPhys*_event, bool logged);
inline int StateChangeEventClient(const EventPhys*_event, bool logged);
inline int EnvChangeEventClient(const EventPhys*_event, bool logged);
inline int PostStepEventClient(const EventPhys*_event, bool logged);
inline int UpdateMeshEventClient(const EventPhys*_event, bool logged);
inline int CreateEntityPartEventClient(const EventPhys*_event, bool logged);
//inline int RemoveEntityPartsEventClient(const EventPhys*_event, bool logged);
inline int RevealEntityPartEventClient(const EventPhys*_event, bool logged);
inline int JointBrokenEventClient(const EventPhys*_event, bool logged);
inline int AreaEventClient(const EventPhys*_event, bool logged);
inline int AreaChangeEventClient(const EventPhys*_event, bool logged);
inline int EntityDeletedEventClient(const EventPhys*_event, bool logged);

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
	//ADD_EVENT_CLIENT(EventPhysRemoveEntityParts::id, RemoveEntityPartsEventClient);
	ADD_EVENT_CLIENT(EventPhysRevealEntityPart::id,  RevealEntityPartEventClient);
	ADD_EVENT_CLIENT(EventPhysJointBroken::id,       JointBrokenEventClient);
	ADD_EVENT_CLIENT(EventPhysEntityDeleted::id,     EntityDeletedEventClient);
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
	////REMOVE_EVENT_CLIENT(EventPhysRemoveEntityParts::id, RemoveEntityPartsEventClient);
	REMOVE_EVENT_CLIENT(EventPhysRevealEntityPart::id,  RevealEntityPartEventClient);
	REMOVE_EVENT_CLIENT(EventPhysJointBroken::id,       JointBrokenEventClient);
	REMOVE_EVENT_CLIENT(EventPhysEntityDeleted::id,     EntityDeletedEventClient);
}