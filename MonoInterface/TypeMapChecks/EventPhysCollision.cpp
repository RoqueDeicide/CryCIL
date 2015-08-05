#include "stdafx.h"

#include "CheckingBasics.h"


TYPE_MIRROR struct EventPhysicsCollision
{
	EventPhys *next;
	int idval;
	IPhysicalEntity *pEntity[2];
	void *pForeignData[2];
	int iForeignData[2];
	int idCollider;	// in addition to pEntity[1]
	Vec3 pt; // contact point in world coordinates
	Vec3 n;	// contact normal
	Vec3 vloc[2];	// velocities at the contact point
	float mass[2];
	int partid[2];
	short idmat[2];
	short iPrim[2];
	float penetration; // contact's penetration depth
	float normImpulse; // impulse applied by the solver to resolve the collision
	float radius;	// some characteristic size of the contact area
	void *pEntContact; // reserved for internal use
	char deferredState; // EventPhysCollisionState
	char deferredResult; // stores the result returned by the deferred event
	float fDecalPlacementTestMaxSize;

	EventPhysicsCollision(EventPhysCollision other)
	{
		static_assert(sizeof(EventPhysicsCollision) == sizeof(EventPhysCollision), "EventPhysCollision has been changed.");

		ASSIGN_FIELD(next);
		ASSIGN_FIELD(idval);
		ASSIGN_FIELD(pEntity[0]);
		ASSIGN_FIELD(pEntity[1]);
		ASSIGN_FIELD(pForeignData[0]);
		ASSIGN_FIELD(pForeignData[1]);
		ASSIGN_FIELD(iForeignData[0]);
		ASSIGN_FIELD(iForeignData[1]);
		ASSIGN_FIELD(idCollider);
		ASSIGN_FIELD(pt);
		ASSIGN_FIELD(n);
		ASSIGN_FIELD(vloc[0]);
		ASSIGN_FIELD(mass[0]);
		ASSIGN_FIELD(partid[0]);
		ASSIGN_FIELD(idmat[0]);
		ASSIGN_FIELD(iPrim[0]);
		ASSIGN_FIELD(vloc[1]);
		ASSIGN_FIELD(mass[1]);
		ASSIGN_FIELD(partid[1]);
		ASSIGN_FIELD(idmat[1]);
		ASSIGN_FIELD(iPrim[1]);
		ASSIGN_FIELD(penetration);
		ASSIGN_FIELD(normImpulse);
		ASSIGN_FIELD(radius);
		ASSIGN_FIELD(pEntContact);
		ASSIGN_FIELD(deferredState);
		ASSIGN_FIELD(deferredResult);
		ASSIGN_FIELD(fDecalPlacementTestMaxSize);

		CHECK_TYPE(next);
		CHECK_TYPE(idval);
		CHECK_TYPE(pEntity);
		CHECK_TYPE(pForeignData);
		CHECK_TYPE(iForeignData);
		CHECK_TYPE(idCollider);
		CHECK_TYPE(pt);
		CHECK_TYPE(n);
		CHECK_TYPE(vloc);
		CHECK_TYPE(mass);
		CHECK_TYPE(partid);
		CHECK_TYPE(idmat);
		CHECK_TYPE(iPrim);
		CHECK_TYPE(penetration);
		CHECK_TYPE(normImpulse);
		CHECK_TYPE(radius);
		CHECK_TYPE(pEntContact);
		CHECK_TYPE(deferredState);
		CHECK_TYPE(deferredResult);
		CHECK_TYPE(fDecalPlacementTestMaxSize);
	}
};