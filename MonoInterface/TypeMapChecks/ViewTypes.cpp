#include "stdafx.h"

#include "CheckingBasics.h"
#include <IViewSystem.h>

TYPE_MIRROR struct ViewParams
{
	Vec3 position;
	Quat rotation;
	Quat localRotationLast;
	float nearplane;
	float	fov;
	uint8 viewID;
	bool  groundOnly;
	float shakingRatio;
	Quat currentShakeQuat;
	Vec3 currentShakeShift;
	EntityId idTarget;
	Vec3 targetPos;
	float frameTime;
	float angleVel;
	float vel;
	float dist;
	bool	blend;
	float	blendPosSpeed;
	float	blendRotSpeed;
	float	blendFOVSpeed;
	Vec3	blendPosOffset;
	Quat	blendRotOffset;
	float   blendFOVOffset;
	bool	justActivated;

private:
	uint8 viewIDLast;
	Vec3 positionLast;//last view position
	Quat rotationLast;//last view orientation
	float FOVLast;
public:
	explicit ViewParams(const SViewParams &other)
		: viewIDLast(0)
		, positionLast(ZERO)
		, rotationLast(ZERO)
		, FOVLast(0)
	{
		CHECK_TYPE_SIZE(ViewParams);

		ASSIGN_FIELD(position);
		ASSIGN_FIELD(rotation);
		ASSIGN_FIELD(localRotationLast);
		ASSIGN_FIELD(nearplane);
		ASSIGN_FIELD(fov);
		ASSIGN_FIELD(viewID);
		ASSIGN_FIELD(groundOnly);
		ASSIGN_FIELD(shakingRatio);
		ASSIGN_FIELD(currentShakeQuat);
		ASSIGN_FIELD(currentShakeShift);
		ASSIGN_FIELD(idTarget);
		ASSIGN_FIELD(targetPos);
		ASSIGN_FIELD(frameTime);
		ASSIGN_FIELD(angleVel);
		ASSIGN_FIELD(vel);
		ASSIGN_FIELD(dist);
		ASSIGN_FIELD(blend);
		ASSIGN_FIELD(blendPosSpeed);
		ASSIGN_FIELD(blendRotSpeed);
		ASSIGN_FIELD(blendFOVSpeed);
		ASSIGN_FIELD(blendPosOffset);
		ASSIGN_FIELD(blendRotOffset);
		ASSIGN_FIELD(blendFOVOffset);
		ASSIGN_FIELD(justActivated);

		CHECK_TYPE(position);
		CHECK_TYPE(rotation);
		CHECK_TYPE(localRotationLast);
		CHECK_TYPE(nearplane);
		CHECK_TYPE(fov);
		CHECK_TYPE(viewID);
		CHECK_TYPE(groundOnly);
		CHECK_TYPE(shakingRatio);
		CHECK_TYPE(currentShakeQuat);
		CHECK_TYPE(currentShakeShift);
		CHECK_TYPE(idTarget);
		CHECK_TYPE(targetPos);
		CHECK_TYPE(frameTime);
		CHECK_TYPE(angleVel);
		CHECK_TYPE(vel);
		CHECK_TYPE(dist);
		CHECK_TYPE(blend);
		CHECK_TYPE(blendPosSpeed);
		CHECK_TYPE(blendRotSpeed);
		CHECK_TYPE(blendFOVSpeed);
		CHECK_TYPE(blendPosOffset);
		CHECK_TYPE(blendRotOffset);
		CHECK_TYPE(blendFOVOffset);
		CHECK_TYPE(justActivated);
	}
};

TYPE_MIRROR enum MotionBlurType
{
	eMBT_None_check = 0,
	eMBT_Accumulation_check = 1,
	eMBT_Velocity_check = 2
};

#define CHECK_ENUM(x) static_assert (MotionBlurType::x ## _check == EMotionBlurType::x, "EMotionBlurType enumeration has been changed.")

inline void CheckMotionBlurType()
{
	CHECK_ENUM(eMBT_None);
	CHECK_ENUM(eMBT_Accumulation);
	CHECK_ENUM(eMBT_Velocity);
}

TYPE_MIRROR enum DefaultViewIds
{
	VIEWID_NORMAL_check = 0,
	VIEWID_FOLLOWHEAD_check = 1,
	VIEWID_VEHICLE_check = 2,
	VIEWID_RAGDOLL_check = 3
};

#define CHECK_ENUM(x) static_assert (DefaultViewIds::x ## _check == x, "Defines for default view ids have been changed.")

inline void CheckDefaultViewIds()
{
	CHECK_ENUM(VIEWID_NORMAL);
	CHECK_ENUM(VIEWID_FOLLOWHEAD);
	CHECK_ENUM(VIEWID_VEHICLE);
	CHECK_ENUM(VIEWID_RAGDOLL);
}

TYPE_MIRROR struct ShakeParams
{
	Ang3 shakeAngle;
	Vec3 shakeShift;
	float sustainDuration;
	float fadeInDuration;
	float fadeOutDuration;
	float frequency;
	float randomness;
	int shakeID;
	bool bFlipVec;
	bool bUpdateOnly;
	bool bGroundOnly;
	bool bPermanent; // if true, sustainDuration is ignored
	bool isSmooth;

	explicit ShakeParams(const IView::SShakeParams &other)
	{
		CHECK_TYPES_SIZE(ShakeParams, IView::SShakeParams);

		ASSIGN_FIELD(shakeAngle);
		ASSIGN_FIELD(shakeShift);
		ASSIGN_FIELD(sustainDuration);
		ASSIGN_FIELD(fadeInDuration);
		ASSIGN_FIELD(fadeOutDuration);
		ASSIGN_FIELD(frequency);
		ASSIGN_FIELD(randomness);
		ASSIGN_FIELD(shakeID);
		ASSIGN_FIELD(bFlipVec);
		ASSIGN_FIELD(bUpdateOnly);
		ASSIGN_FIELD(bGroundOnly);
		ASSIGN_FIELD(bPermanent); // if true, sustainDuration is ignored
		ASSIGN_FIELD(isSmooth);

		CHECK_TYPE(shakeAngle);
		CHECK_TYPE(shakeShift);
		CHECK_TYPE(sustainDuration);
		CHECK_TYPE(fadeInDuration);
		CHECK_TYPE(fadeOutDuration);
		CHECK_TYPE(frequency);
		CHECK_TYPE(randomness);
		CHECK_TYPE(shakeID);
		CHECK_TYPE(bFlipVec);
		CHECK_TYPE(bUpdateOnly);
		CHECK_TYPE(bGroundOnly);
		CHECK_TYPE(bPermanent); // if true, sustainDuration is ignored
		CHECK_TYPE(isSmooth);
	}
};