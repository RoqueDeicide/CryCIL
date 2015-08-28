#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum UpdateMeshReason
{
	ReasonExplosion_check,
	ReasonFracture_check,
	ReasonRequest_check,
	ReasonDeform_check
};

TYPE_MIRROR enum CreatePartReason
{
	ReasonMeshSplit_check,
	ReasonJointsBroken_check
};

#define CHECK_ENUM(x) static_assert (UpdateMeshReason::x ## _check == EventPhysUpdateMesh::x, "EventPhysUpdateMesh::reason enumeration has been changed.")
#define CHECK_ENUM2(x) static_assert (CreatePartReason::x ## _check == EventPhysCreateEntityPart::x, "EventPhysCreateEntityPart::reason enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(ReasonExplosion);
	CHECK_ENUM(ReasonFracture);
	CHECK_ENUM(ReasonRequest);
	CHECK_ENUM(ReasonDeform);

	CHECK_ENUM2(ReasonMeshSplit);
	CHECK_ENUM2(ReasonJointsBroken);
}