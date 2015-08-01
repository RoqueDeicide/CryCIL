#include "stdafx.h"

#include "CheckingBasics.h"

enum EntityUpdatePolicy
{
	ENTITY_UPDATE_NEVER_check,			// Never update entity every frame.
	ENTITY_UPDATE_IN_RANGE_check,			// Only update entity if it is in specified range from active camera.
	ENTITY_UPDATE_POT_VISIBLE_check,		// Only update entity if it is potentially visible.
	ENTITY_UPDATE_VISIBLE_check,			// Only update entity if it is visible.
	ENTITY_UPDATE_PHYSICS_check,			// Only update entity if it is need to be updated due to physics.
	ENTITY_UPDATE_PHYSICS_VISIBLE_check,	// Only update entity if it is need to be updated due to physics or if it is visible.
	ENTITY_UPDATE_ALWAYS_check,			// Always update entity every frame.
};

#define CHECK_ENUM(x) static_assert (EntityUpdatePolicy::x ## _check == EEntityUpdatePolicy::x, "EEntityUpdatePolicy enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(ENTITY_UPDATE_NEVER);
	CHECK_ENUM(ENTITY_UPDATE_IN_RANGE);
	CHECK_ENUM(ENTITY_UPDATE_POT_VISIBLE);
	CHECK_ENUM(ENTITY_UPDATE_VISIBLE);
	CHECK_ENUM(ENTITY_UPDATE_PHYSICS);
	CHECK_ENUM(ENTITY_UPDATE_PHYSICS_VISIBLE);
	CHECK_ENUM(ENTITY_UPDATE_ALWAYS);
}